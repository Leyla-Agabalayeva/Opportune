using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Opportune.Domain.Entities;
using Opportune.Domain.Enums;
using Opportune.Persistence;

namespace Opportune.Tests.Persistence;

/// <summary>
/// Integration tests against a real PostgreSQL: they prove that the rules written in the
/// model (constraints, unique indexes, delete behavior, full-text search) are enforced
/// by the database itself, not only by application code.
/// </summary>
[Collection(PostgresCollection.Name)]
public class DatabaseModelTests(PostgresFixture postgres)
{
    private const string CheckViolation = "23514";
    private const string UniqueViolation = "23505";
    private const string ForeignKeyViolation = "23503";

    // ---------- helpers ----------

    private static async Task<ApplicationUser> AddUserAsync(AppDbContext db)
    {
        var id = Guid.NewGuid();
        var user = new ApplicationUser
        {
            Id = id,
            UserName = $"user-{id:N}",
            NormalizedUserName = $"USER-{id:N}".ToUpperInvariant(),
            Email = $"{id:N}@example.com",
            NormalizedEmail = $"{id:N}@EXAMPLE.COM".ToUpperInvariant(),
            FirstName = "Test",
            LastName = "User"
        };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    private static async Task<Opportunity> AddOpportunityAsync(AppDbContext db, Action<Opportunity>? customize = null)
    {
        var owner = await AddUserAsync(db);
        var organization = new Organization
        {
            Id = Guid.NewGuid(),
            OwnerUserId = owner.Id,
            Name = "Test Organization",
            Slug = $"org-{Guid.NewGuid():N}"
        };
        db.Organizations.Add(organization);

        var opportunity = new Opportunity
        {
            Id = Guid.NewGuid(),
            OrganizationId = organization.Id,
            CategoryId = (await db.Categories.OrderBy(c => c.Slug).FirstAsync()).Id,
            CreatedByUserId = owner.Id,
            Title = "Beach cleanup",
            Description = "Join us to clean the Caspian shore in Baku.",
            Scope = OpportunityScope.Local,
            Country = "AZ",
            ApplyUrl = "https://example.org/apply"
        };
        customize?.Invoke(opportunity);

        db.Opportunities.Add(opportunity);
        await db.SaveChangesAsync();
        return opportunity;
    }

    private static async Task AssertViolationAsync(Func<Task> action, string sqlState, string? constraint = null)
    {
        var exception = await Assert.ThrowsAsync<DbUpdateException>(action);
        var pg = Assert.IsType<PostgresException>(exception.InnerException);
        Assert.Equal(sqlState, pg.SqlState);

        if (constraint is not null)
        {
            Assert.Equal(constraint, pg.ConstraintName);
        }
    }

    // ---------- schema and reference data ----------

    [Fact]
    public async Task Migrations_CreateSchemaAndSeedReferenceData()
    {
        await using var db = postgres.NewContext();

        Assert.Equal(11, await db.Categories.CountAsync());
        Assert.Equal(20, await db.Skills.CountAsync());

        var roles = await db.Roles.Select(r => r.Name).OrderBy(n => n).ToListAsync();
        Assert.Equal([AppRoles.Admin, AppRoles.Organizer, AppRoles.Volunteer], roles);
    }

    // ---------- opportunity rules ----------

    [Fact]
    public async Task Opportunity_WithoutCountryAndNotOnline_IsRejected()
    {
        await using var db = postgres.NewContext();

        await AssertViolationAsync(
            () => AddOpportunityAsync(db, o => { o.Country = null; o.IsOnline = false; }),
            CheckViolation, "ck_opportunities_location");
    }

    [Fact]
    public async Task Opportunity_OnlineWithoutCountry_IsAccepted()
    {
        await using var db = postgres.NewContext();

        var opportunity = await AddOpportunityAsync(db, o => { o.Country = null; o.IsOnline = true; });

        Assert.NotEqual(Guid.Empty, opportunity.Id);
    }

    [Fact]
    public async Task Opportunity_MinAgeAboveMaxAge_IsRejected()
    {
        await using var db = postgres.NewContext();

        await AssertViolationAsync(
            () => AddOpportunityAsync(db, o => { o.MinAge = 30; o.MaxAge = 18; }),
            CheckViolation, "ck_opportunities_age_range");
    }

    [Fact]
    public async Task Opportunity_EndBeforeStart_IsRejected()
    {
        await using var db = postgres.NewContext();

        await AssertViolationAsync(
            () => AddOpportunityAsync(db, o =>
            {
                o.StartDate = new DateOnly(2027, 7, 10);
                o.EndDate = new DateOnly(2027, 7, 1);
            }),
            CheckViolation, "ck_opportunities_date_range");
    }

    [Fact]
    public async Task Opportunity_PaidWithoutCurrency_IsRejected_ButFreeIsAccepted()
    {
        await using var db = postgres.NewContext();

        await AssertViolationAsync(
            () => AddOpportunityAsync(db, o => { o.ParticipationFee = 50m; o.Currency = null; }),
            CheckViolation, "ck_opportunities_fee_currency");

        await using var db2 = postgres.NewContext();
        var paid = await AddOpportunityAsync(db2, o => { o.ParticipationFee = 50m; o.Currency = "EUR"; });
        Assert.Equal("EUR", paid.Currency);
    }

    [Fact]
    public async Task Opportunity_DuplicateImport_IsRejected_ButManualRecordsWithoutExternalIdAreAllowed()
    {
        var externalId = $"ext-{Guid.NewGuid():N}";

        await using (var db = postgres.NewContext())
        {
            await AddOpportunityAsync(db, o => { o.Source = OpportunitySource.Import; o.ExternalId = externalId; });
        }

        await using (var db = postgres.NewContext())
        {
            await AssertViolationAsync(
                () => AddOpportunityAsync(db, o => { o.Source = OpportunitySource.Import; o.ExternalId = externalId; }),
                UniqueViolation);
        }

        // Several manual records carry no external id and must not collide with each other.
        await using var db3 = postgres.NewContext();
        await AddOpportunityAsync(db3);
        await AddOpportunityAsync(db3);
    }

    [Fact]
    public async Task FullTextSearch_FindsOpportunityByDescriptionWord()
    {
        var marker = $"zebra{Guid.NewGuid():N}";
        await using var db = postgres.NewContext();
        var opportunity = await AddOpportunityAsync(db, o => o.Description = $"Help with the {marker} sanctuary.");

        var found = await db.Opportunities
            .Where(o => EF.Property<NpgsqlTsVector>(o, "SearchVector")
                .Matches(EF.Functions.PlainToTsQuery("simple", marker)))
            .Select(o => o.Id)
            .ToListAsync();

        Assert.Equal([opportunity.Id], found);
    }

    // ---------- activity rules ----------

    [Fact]
    public async Task Review_RatingOutOfRange_IsRejected_AndOnlyOnePerParticipationIsAllowed()
    {
        await using var db = postgres.NewContext();
        var opportunity = await AddOpportunityAsync(db);
        var volunteer = await AddUserAsync(db);

        var participation = new Participation
        {
            Id = Guid.NewGuid(),
            UserId = volunteer.Id,
            OpportunityId = opportunity.Id
        };
        db.Participations.Add(participation);
        await db.SaveChangesAsync();

        db.Reviews.Add(new Review { Id = Guid.NewGuid(), ParticipationId = participation.Id, Rating = 6, Text = "Too good" });
        await AssertViolationAsync(() => db.SaveChangesAsync(), CheckViolation, "ck_reviews_rating");

        await using var db2 = postgres.NewContext();
        db2.Reviews.Add(new Review { Id = Guid.NewGuid(), ParticipationId = participation.Id, Rating = 5, Text = "Great" });
        await db2.SaveChangesAsync();

        await using var db3 = postgres.NewContext();
        db3.Reviews.Add(new Review { Id = Guid.NewGuid(), ParticipationId = participation.Id, Rating = 4, Text = "Second" });
        await AssertViolationAsync(() => db3.SaveChangesAsync(), UniqueViolation);
    }

    [Fact]
    public async Task ApplicationTracking_SecondEntryForSameOpportunity_IsRejected()
    {
        await using var db = postgres.NewContext();
        var opportunity = await AddOpportunityAsync(db);
        var volunteer = await AddUserAsync(db);

        db.ApplicationTrackings.Add(new ApplicationTracking { Id = Guid.NewGuid(), UserId = volunteer.Id, OpportunityId = opportunity.Id });
        await db.SaveChangesAsync();

        await using var db2 = postgres.NewContext();
        db2.ApplicationTrackings.Add(new ApplicationTracking { Id = Guid.NewGuid(), UserId = volunteer.Id, OpportunityId = opportunity.Id });
        await AssertViolationAsync(() => db2.SaveChangesAsync(), UniqueViolation);
    }

    [Fact]
    public async Task Subscription_NeedsAFilter_AndDuplicatesAreRejectedEvenWithNulls()
    {
        await using var db = postgres.NewContext();
        var volunteer = await AddUserAsync(db);
        var categoryId = (await db.Categories.OrderBy(c => c.Slug).FirstAsync()).Id;

        db.Subscriptions.Add(new Subscription { Id = Guid.NewGuid(), UserId = volunteer.Id });
        await AssertViolationAsync(() => db.SaveChangesAsync(), CheckViolation, "ck_subscriptions_has_filter");

        await using var db2 = postgres.NewContext();
        db2.Subscriptions.Add(new Subscription { Id = Guid.NewGuid(), UserId = volunteer.Id, CategoryId = categoryId });
        await db2.SaveChangesAsync();

        // Same category, country still NULL in both rows: PostgreSQL treats the NULLs as equal.
        await using var db3 = postgres.NewContext();
        db3.Subscriptions.Add(new Subscription { Id = Guid.NewGuid(), UserId = volunteer.Id, CategoryId = categoryId });
        await AssertViolationAsync(() => db3.SaveChangesAsync(), UniqueViolation);
    }

    // ---------- delete behavior ----------

    [Fact]
    public async Task DeletingOpportunityWithParticipation_IsRestricted()
    {
        await using var db = postgres.NewContext();
        var opportunity = await AddOpportunityAsync(db);
        var volunteer = await AddUserAsync(db);
        db.Participations.Add(new Participation { Id = Guid.NewGuid(), UserId = volunteer.Id, OpportunityId = opportunity.Id });
        await db.SaveChangesAsync();

        // A fresh context does not track the participation, so the delete reaches the database
        // and it is PostgreSQL, not EF's change tracker, that refuses it.
        await using var db2 = postgres.NewContext();
        db2.Opportunities.Remove(await db2.Opportunities.SingleAsync(o => o.Id == opportunity.Id));

        await AssertViolationAsync(() => db2.SaveChangesAsync(), ForeignKeyViolation);
    }

    [Fact]
    public async Task DeletingUser_RemovesTheirSavedOpportunities_ButKeepsTheOpportunity()
    {
        await using var db = postgres.NewContext();
        var opportunity = await AddOpportunityAsync(db);
        var volunteer = await AddUserAsync(db);
        db.SavedOpportunities.Add(new SavedOpportunity { UserId = volunteer.Id, OpportunityId = opportunity.Id });
        await db.SaveChangesAsync();

        db.Users.Remove(volunteer);
        await db.SaveChangesAsync();

        Assert.False(await db.SavedOpportunities.AnyAsync(s => s.UserId == volunteer.Id));
        Assert.True(await db.Opportunities.AnyAsync(o => o.Id == opportunity.Id));
    }
}

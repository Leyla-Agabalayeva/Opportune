using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Opportune.Domain.Entities;
using Opportune.Persistence.Seed;

namespace Opportune.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<UserSkill> UserSkills => Set<UserSkill>();
    public DbSet<UserLanguage> UserLanguages => Set<UserLanguage>();
    public DbSet<UserInterest> UserInterests => Set<UserInterest>();
    public DbSet<RecommendationCache> RecommendationCaches => Set<RecommendationCache>();

    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<Opportunity> Opportunities => Set<Opportunity>();
    public DbSet<OpportunityRequiredLanguage> OpportunityRequiredLanguages => Set<OpportunityRequiredLanguage>();
    public DbSet<OpportunitySkill> OpportunitySkills => Set<OpportunitySkill>();
    public DbSet<OpportunityTranslation> OpportunityTranslations => Set<OpportunityTranslation>();

    public DbSet<SavedOpportunity> SavedOpportunities => Set<SavedOpportunity>();
    public DbSet<ApplicationTracking> ApplicationTrackings => Set<ApplicationTracking>();
    public DbSet<Participation> Participations => Set<Participation>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Readable table names instead of the AspNet* defaults.
        builder.Entity<ApplicationUser>().ToTable("users");
        builder.Entity<IdentityRole<Guid>>().ToTable("roles");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        builder.Entity<IdentityRole<Guid>>().HasData(SeedData.Roles);
        builder.Entity<Category>().HasData(SeedData.Categories);
        builder.Entity<Skill>().HasData(SeedData.Skills);
    }
}

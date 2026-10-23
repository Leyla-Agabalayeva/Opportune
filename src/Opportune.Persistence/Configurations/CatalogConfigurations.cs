using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NpgsqlTypes;
using Opportune.Domain.Entities;

namespace Opportune.Persistence.Configurations;

internal class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("organizations");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Name).HasMaxLength(200).IsRequired();
        builder.Property(o => o.Slug).HasMaxLength(120).IsRequired();
        builder.Property(o => o.Description).HasMaxLength(4000);
        builder.Property(o => o.LogoUrl).HasMaxLength(500);
        builder.Property(o => o.WebsiteUrl).HasMaxLength(500);
        builder.Property(o => o.Country).HasMaxLength(2).IsFixedLength();
        builder.Property(o => o.ContactEmail).HasMaxLength(254);
        builder.HasIndex(o => o.Slug).IsUnique();

        // An organization with opportunities must not disappear with its owner.
        builder.HasOne(o => o.Owner).WithMany().HasForeignKey(o => o.OwnerUserId).OnDelete(DeleteBehavior.Restrict);
    }
}

internal class OpportunityConfiguration : IEntityTypeConfiguration<Opportunity>
{
    public void Configure(EntityTypeBuilder<Opportunity> builder)
    {
        builder.ToTable("opportunities", t =>
        {
            t.HasCheckConstraint("ck_opportunities_age_range",
                "min_age IS NULL OR max_age IS NULL OR min_age <= max_age");
            t.HasCheckConstraint("ck_opportunities_date_range",
                "start_date IS NULL OR end_date IS NULL OR end_date >= start_date");
            t.HasCheckConstraint("ck_opportunities_fee_currency",
                "participation_fee >= 0 AND (participation_fee = 0 OR currency IS NOT NULL)");
            t.HasCheckConstraint("ck_opportunities_location",
                "is_online OR country IS NOT NULL");
            t.HasCheckConstraint("ck_opportunities_original_language",
                "original_language IN ('az', 'en', 'ru')");
        });

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Title).HasMaxLength(300).IsRequired();
        builder.Property(o => o.Description).IsRequired();
        builder.Property(o => o.OriginalLanguage).HasMaxLength(5).IsRequired();

        builder.Property(o => o.Scope).HasConversion<string>().HasMaxLength(20);
        builder.Property(o => o.Country).HasMaxLength(2).IsFixedLength();
        builder.Property(o => o.City).HasMaxLength(100);

        builder.Property(o => o.ParticipationFee).HasPrecision(10, 2).HasDefaultValue(0m);
        builder.Property(o => o.Currency).HasMaxLength(3).IsFixedLength();

        builder.Property(o => o.ApplyUrl).HasMaxLength(500).IsRequired();
        builder.Property(o => o.SourceUrl).HasMaxLength(500);
        builder.Property(o => o.ExternalId).HasMaxLength(200);
        builder.Property(o => o.RejectionReason).HasMaxLength(1000);

        builder.Property(o => o.VerificationLevel).HasConversion<string>().HasMaxLength(20);
        builder.Property(o => o.Source).HasConversion<string>().HasMaxLength(20);
        builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(20);

        // Full-text search column maintained by PostgreSQL itself. It is a shadow
        // property so the domain model stays free of database-specific types.
        // The "simple" configuration is language-neutral (no stemming), which suits
        // content written in Azerbaijani, English and Russian.
        builder.Property<NpgsqlTsVector>("SearchVector")
            .HasColumnType("tsvector")
            .HasComputedColumnSql(
                "to_tsvector('simple', coalesce(title, '') || ' ' || coalesce(description, ''))",
                stored: true);
        builder.HasIndex("SearchVector").HasMethod("GIN");

        builder.HasIndex(o => new { o.Status, o.ApplicationDeadline });
        builder.HasIndex(o => o.Country);
        builder.HasIndex(o => o.CategoryId);
        builder.HasIndex(o => o.Scope);
        builder.HasIndex(o => o.OrganizationId);

        // No duplicate imports of the same external record.
        builder.HasIndex(o => new { o.Source, o.ExternalId })
            .IsUnique()
            .HasFilter("external_id IS NOT NULL");

        builder.HasOne(o => o.Organization).WithMany().HasForeignKey(o => o.OrganizationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(o => o.Category).WithMany().HasForeignKey(o => o.CategoryId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(o => o.CreatedBy).WithMany().HasForeignKey(o => o.CreatedByUserId).OnDelete(DeleteBehavior.Restrict);
    }
}

internal class OpportunityRequiredLanguageConfiguration : IEntityTypeConfiguration<OpportunityRequiredLanguage>
{
    public void Configure(EntityTypeBuilder<OpportunityRequiredLanguage> builder)
    {
        builder.ToTable("opportunity_required_languages");
        builder.HasKey(l => new { l.OpportunityId, l.LanguageCode });
        builder.Property(l => l.LanguageCode).HasMaxLength(5);
        builder.Property(l => l.MinLevel).HasConversion<string>().HasMaxLength(10);
        builder.HasOne(l => l.Opportunity).WithMany(o => o.RequiredLanguages)
            .HasForeignKey(l => l.OpportunityId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal class OpportunitySkillConfiguration : IEntityTypeConfiguration<OpportunitySkill>
{
    public void Configure(EntityTypeBuilder<OpportunitySkill> builder)
    {
        builder.ToTable("opportunity_skills");
        builder.HasKey(s => new { s.OpportunityId, s.SkillId });
        builder.HasOne(s => s.Opportunity).WithMany(o => o.Skills)
            .HasForeignKey(s => s.OpportunityId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(s => s.Skill).WithMany().HasForeignKey(s => s.SkillId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal class OpportunityTranslationConfiguration : IEntityTypeConfiguration<OpportunityTranslation>
{
    public void Configure(EntityTypeBuilder<OpportunityTranslation> builder)
    {
        builder.ToTable("opportunity_translations");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.LanguageCode).HasMaxLength(5).IsRequired();
        builder.Property(t => t.Title).HasMaxLength(300).IsRequired();
        builder.Property(t => t.Description).IsRequired();
        builder.Property(t => t.SourceHash).HasMaxLength(100).IsRequired();
        builder.Property(t => t.Model).HasMaxLength(100).IsRequired();
        builder.HasIndex(t => new { t.OpportunityId, t.LanguageCode }).IsUnique();
        builder.HasOne(t => t.Opportunity).WithMany(o => o.Translations)
            .HasForeignKey(t => t.OpportunityId).OnDelete(DeleteBehavior.Cascade);
    }
}

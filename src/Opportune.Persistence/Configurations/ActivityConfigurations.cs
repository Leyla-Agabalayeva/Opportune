using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opportune.Domain.Entities;

namespace Opportune.Persistence.Configurations;

internal class SavedOpportunityConfiguration : IEntityTypeConfiguration<SavedOpportunity>
{
    public void Configure(EntityTypeBuilder<SavedOpportunity> builder)
    {
        builder.ToTable("saved_opportunities");
        builder.HasKey(s => new { s.UserId, s.OpportunityId });
        builder.HasOne(s => s.User).WithMany().HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(s => s.Opportunity).WithMany().HasForeignKey(s => s.OpportunityId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal class ApplicationTrackingConfiguration : IEntityTypeConfiguration<ApplicationTracking>
{
    public void Configure(EntityTypeBuilder<ApplicationTracking> builder)
    {
        builder.ToTable("application_trackings");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(t => t.Notes).HasMaxLength(2000);
        builder.HasIndex(t => new { t.UserId, t.OpportunityId }).IsUnique();
        builder.HasOne(t => t.User).WithMany().HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(t => t.Opportunity).WithMany().HasForeignKey(t => t.OpportunityId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal class ParticipationConfiguration : IEntityTypeConfiguration<Participation>
{
    public void Configure(EntityTypeBuilder<Participation> builder)
    {
        builder.ToTable("participations", t => t.HasCheckConstraint(
            "ck_participations_date_range",
            "start_date IS NULL OR end_date IS NULL OR end_date >= start_date"));
        builder.HasKey(p => p.Id);
        builder.Property(p => p.CertificateUrl).HasMaxLength(500);
        builder.HasIndex(p => new { p.UserId, p.OpportunityId }).IsUnique();
        builder.HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);

        // History and reviews must not vanish: archive an opportunity instead of deleting it.
        builder.HasOne(p => p.Opportunity).WithMany().HasForeignKey(p => p.OpportunityId).OnDelete(DeleteBehavior.Restrict);
    }
}

internal class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews", t => t.HasCheckConstraint("ck_reviews_rating", "rating BETWEEN 1 AND 5"));
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Text).HasMaxLength(4000).IsRequired();
        builder.HasIndex(r => r.ParticipationId).IsUnique();
        builder.HasOne(r => r.Participation).WithOne(p => p.Review)
            .HasForeignKey<Review>(r => r.ParticipationId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable("subscriptions", t => t.HasCheckConstraint(
            "ck_subscriptions_has_filter", "category_id IS NOT NULL OR country IS NOT NULL"));
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Country).HasMaxLength(2).IsFixedLength();

        // NULLs count as equal, so the same category-only or country-only alert cannot be added twice.
        builder.HasIndex(s => new { s.UserId, s.CategoryId, s.Country }).IsUnique().AreNullsDistinct(false);

        builder.HasOne(s => s.User).WithMany().HasForeignKey(s => s.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(s => s.Category).WithMany().HasForeignKey(s => s.CategoryId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Type).HasConversion<string>().HasMaxLength(40);
        builder.Property(n => n.Title).HasMaxLength(200).IsRequired();
        builder.Property(n => n.Message).HasMaxLength(1000).IsRequired();
        builder.HasIndex(n => new { n.UserId, n.IsRead, n.CreatedAt }).IsDescending(false, false, true);
        builder.HasOne(n => n.User).WithMany().HasForeignKey(n => n.UserId).OnDelete(DeleteBehavior.Cascade);

        // The notification text stays readable after the opportunity is gone.
        builder.HasOne(n => n.Opportunity).WithMany().HasForeignKey(n => n.OpportunityId).OnDelete(DeleteBehavior.SetNull);
    }
}

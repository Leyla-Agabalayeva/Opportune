using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opportune.Domain.Entities;

namespace Opportune.Persistence.Configurations;

internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(100).IsRequired();
        builder.Property(u => u.Country).HasMaxLength(2).IsFixedLength();
        builder.Property(u => u.City).HasMaxLength(100);
        builder.Property(u => u.Bio).HasMaxLength(2000);
        builder.Property(u => u.AvatarUrl).HasMaxLength(500);
        builder.Property(u => u.PreferredLanguage).HasMaxLength(5).IsRequired();

        builder.ToTable(t => t.HasCheckConstraint(
            "ck_users_preferred_language", "preferred_language IN ('az', 'en', 'ru')"));
    }
}

internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("refresh_tokens");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.TokenHash).HasMaxLength(200).IsRequired();
        builder.Property(t => t.ReplacedByTokenHash).HasMaxLength(200);
        builder.HasIndex(t => t.TokenHash).IsUnique();
        builder.HasIndex(t => t.UserId);

        builder.HasOne(t => t.User).WithMany().HasForeignKey(t => t.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.ToTable("skills");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Slug).HasMaxLength(60).IsRequired();
        builder.Property(s => s.NameAz).HasMaxLength(100).IsRequired();
        builder.Property(s => s.NameEn).HasMaxLength(100).IsRequired();
        builder.Property(s => s.NameRu).HasMaxLength(100).IsRequired();
        builder.HasIndex(s => s.Slug).IsUnique();
    }
}

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("categories");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Slug).HasMaxLength(60).IsRequired();
        builder.Property(c => c.NameAz).HasMaxLength(100).IsRequired();
        builder.Property(c => c.NameEn).HasMaxLength(100).IsRequired();
        builder.Property(c => c.NameRu).HasMaxLength(100).IsRequired();
        builder.HasIndex(c => c.Slug).IsUnique();
    }
}

internal class UserSkillConfiguration : IEntityTypeConfiguration<UserSkill>
{
    public void Configure(EntityTypeBuilder<UserSkill> builder)
    {
        builder.ToTable("user_skills");
        builder.HasKey(us => new { us.UserId, us.SkillId });
        builder.HasOne(us => us.User).WithMany().HasForeignKey(us => us.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(us => us.Skill).WithMany().HasForeignKey(us => us.SkillId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal class UserLanguageConfiguration : IEntityTypeConfiguration<UserLanguage>
{
    public void Configure(EntityTypeBuilder<UserLanguage> builder)
    {
        builder.ToTable("user_languages");
        builder.HasKey(ul => new { ul.UserId, ul.LanguageCode });
        builder.Property(ul => ul.LanguageCode).HasMaxLength(5);
        builder.Property(ul => ul.Level).HasConversion<string>().HasMaxLength(10);
        builder.HasOne(ul => ul.User).WithMany().HasForeignKey(ul => ul.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal class UserInterestConfiguration : IEntityTypeConfiguration<UserInterest>
{
    public void Configure(EntityTypeBuilder<UserInterest> builder)
    {
        builder.ToTable("user_interests");
        builder.HasKey(ui => new { ui.UserId, ui.CategoryId });
        builder.HasOne(ui => ui.User).WithMany().HasForeignKey(ui => ui.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(ui => ui.Category).WithMany().HasForeignKey(ui => ui.CategoryId).OnDelete(DeleteBehavior.Cascade);
    }
}

internal class RecommendationCacheConfiguration : IEntityTypeConfiguration<RecommendationCache>
{
    public void Configure(EntityTypeBuilder<RecommendationCache> builder)
    {
        builder.ToTable("recommendation_caches");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.ProfileHash).HasMaxLength(100).IsRequired();
        builder.Property(r => r.ResultJson).HasColumnType("jsonb").IsRequired();
        builder.HasIndex(r => new { r.UserId, r.ProfileHash });
        builder.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}

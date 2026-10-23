namespace Opportune.Domain.Entities;

/// <summary>A cached AI recommendation result, valid while the user's profile hash is unchanged.</summary>
public class RecommendationCache
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public string ProfileHash { get; set; } = string.Empty;

    /// <summary>Stored as jsonb.</summary>
    public string ResultJson { get; set; } = "[]";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
}

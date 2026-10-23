using Opportune.Domain.Enums;

namespace Opportune.Domain.Entities;

/// <summary>The user's own record of where they stand with an external application.</summary>
public class ApplicationTracking
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public Guid OpportunityId { get; set; }
    public Opportunity Opportunity { get; set; } = null!;
    public TrackingStatus Status { get; set; } = TrackingStatus.Planned;
    public string? Notes { get; set; }
    public DateTime? AppliedAt { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

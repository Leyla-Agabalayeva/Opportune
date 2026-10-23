namespace Opportune.Domain.Entities;

public class SavedOpportunity
{
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public Guid OpportunityId { get; set; }
    public Opportunity Opportunity { get; set; } = null!;
    public DateTime SavedAt { get; set; } = DateTime.UtcNow;
}

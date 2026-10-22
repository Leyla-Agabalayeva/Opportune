namespace Opportune.Domain.Entities;

/// <summary>A record that the user took part in an opportunity: the basis for history and for writing a review.</summary>
public class Participation
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public Guid OpportunityId { get; set; }
    public Opportunity Opportunity { get; set; } = null!;
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string? CertificateUrl { get; set; }
    public Review? Review { get; set; }
}

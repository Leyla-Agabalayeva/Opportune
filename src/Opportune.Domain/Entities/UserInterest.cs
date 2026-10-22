namespace Opportune.Domain.Entities;

public class UserInterest
{
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}

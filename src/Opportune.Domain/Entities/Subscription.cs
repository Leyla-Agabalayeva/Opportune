namespace Opportune.Domain.Entities;

/// <summary>An alert for new opportunities in a category and/or country. At least one filter is required.</summary>
public class Subscription
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

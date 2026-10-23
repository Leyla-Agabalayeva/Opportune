namespace Opportune.Domain.Entities;

public class Review
{
    public Guid Id { get; set; }
    public Guid ParticipationId { get; set; }
    public Participation Participation { get; set; } = null!;

    /// <summary>1 to 5.</summary>
    public int Rating { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsHidden { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

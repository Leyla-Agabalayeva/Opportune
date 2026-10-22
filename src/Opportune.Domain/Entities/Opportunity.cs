using Opportune.Domain.Enums;

namespace Opportune.Domain.Entities;

/// <summary>A volunteering opportunity: the central entity of the platform.</summary>
public class Opportunity
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public Guid CreatedByUserId { get; set; }
    public ApplicationUser CreatedBy { get; set; } = null!;

    // Content, stored in the language of its source ("az", "en", "ru")
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string OriginalLanguage { get; set; } = ContentLanguages.English;

    // Where
    public OpportunityScope Scope { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public bool IsOnline { get; set; }

    // When
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    /// <summary>Null means applications are accepted on a rolling basis.</summary>
    public DateOnly? ApplicationDeadline { get; set; }
    public int? DurationDays { get; set; }

    // Who
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }

    // Covered costs
    public bool AccommodationCovered { get; set; }
    public bool MealsCovered { get; set; }
    public bool TravelCovered { get; set; }
    public decimal ParticipationFee { get; set; }
    public string? Currency { get; set; }

    /// <summary>The official application page. Applications are not taken inside the platform.</summary>
    public string ApplyUrl { get; set; } = string.Empty;

    // Trust and origin
    public VerificationLevel VerificationLevel { get; set; } = VerificationLevel.External;
    public DateTime? LastVerifiedAt { get; set; }
    public string? SourceUrl { get; set; }
    public OpportunitySource Source { get; set; } = OpportunitySource.Manual;
    public string? ExternalId { get; set; }

    // Lifecycle
    public OpportunityStatus Status { get; set; } = OpportunityStatus.Draft;
    public string? RejectionReason { get; set; }
    public DateTime? PublishedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<OpportunityRequiredLanguage> RequiredLanguages { get; set; } = new List<OpportunityRequiredLanguage>();
    public ICollection<OpportunitySkill> Skills { get; set; } = new List<OpportunitySkill>();
    public ICollection<OpportunityTranslation> Translations { get; set; } = new List<OpportunityTranslation>();
}

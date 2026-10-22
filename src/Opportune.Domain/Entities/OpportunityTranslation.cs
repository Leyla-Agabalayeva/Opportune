namespace Opportune.Domain.Entities;

/// <summary>A cached machine translation of an opportunity's title and description.</summary>
public class OpportunityTranslation
{
    public Guid Id { get; set; }
    public Guid OpportunityId { get; set; }
    public Opportunity Opportunity { get; set; } = null!;
    public string LanguageCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    /// <summary>Hash of the source text; a different hash means the translation is stale.</summary>
    public string SourceHash { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public DateTime TranslatedAt { get; set; } = DateTime.UtcNow;
}

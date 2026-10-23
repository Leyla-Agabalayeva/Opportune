using Opportune.Domain.Enums;

namespace Opportune.Domain.Entities;

public class OpportunityRequiredLanguage
{
    public Guid OpportunityId { get; set; }
    public Opportunity Opportunity { get; set; } = null!;
    public string LanguageCode { get; set; } = string.Empty;
    public LanguageLevel MinLevel { get; set; }
}

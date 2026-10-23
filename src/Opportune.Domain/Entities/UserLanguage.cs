using Opportune.Domain.Enums;

namespace Opportune.Domain.Entities;

public class UserLanguage
{
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public string LanguageCode { get; set; } = string.Empty;
    public LanguageLevel Level { get; set; }
}

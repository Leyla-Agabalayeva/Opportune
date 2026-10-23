using Microsoft.AspNetCore.Identity;

namespace Opportune.Domain.Entities;

/// <summary>A person on the platform. The role (Volunteer, Organizer, Admin) is an Identity role.</summary>
public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly? BirthDate { get; set; }

    /// <summary>ISO 3166-1 alpha-2 code, for example "AZ".</summary>
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
    public string PreferredLanguage { get; set; } = "en";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

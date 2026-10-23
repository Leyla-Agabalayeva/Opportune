namespace Opportune.Domain.Enums;

/// <summary>How much the platform vouches for an opportunity's details.</summary>
public enum VerificationLevel
{
    /// <summary>Taken from an outside source; the user should confirm with the organizer.</summary>
    External,

    /// <summary>Checked against the official page by the team.</summary>
    Verified
}

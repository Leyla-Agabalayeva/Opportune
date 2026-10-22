namespace Opportune.Domain.Enums;

/// <summary>Language codes used for opportunity content and the interface.</summary>
public static class ContentLanguages
{
    public const string Azerbaijani = "az";
    public const string English = "en";
    public const string Russian = "ru";

    public static readonly IReadOnlyList<string> All = [Azerbaijani, English, Russian];
}

namespace VotraxSpeak.Phonetics;

/// <summary>
/// Common whole-context patterns for the NRL letter-to-sound rules.
/// </summary>
internal static class RuleContext
{
    /// <summary>No requirement: matches any context.</summary>
    public const string Anything = "";

    /// <summary>The match must be at the beginning (left) or end (right) of a word.</summary>
    public const string Boundary = " ";
}

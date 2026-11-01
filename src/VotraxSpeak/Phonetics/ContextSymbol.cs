namespace VotraxSpeak.Phonetics;

/// <summary>
/// Special characters in the context patterns of the NRL letter-to-sound rules. Any other
/// character in a pattern must match the text literally.
/// </summary>
internal static class ContextSymbol
{
    /// <summary>A non-letter: the beginning or end of a word.</summary>
    public const char WordBoundary = ' ';

    /// <summary>One or more vowels.</summary>
    public const char OneOrMoreVowels = '#';

    /// <summary>Zero or more consonants.</summary>
    public const char ZeroOrMoreConsonants = ':';

    /// <summary>Exactly one consonant.</summary>
    public const char OneConsonant = '^';

    /// <summary>One voiced consonant: B, D, V, G, J, L, M, N, R, W or Z.</summary>
    public const char VoicedConsonant = '.';

    /// <summary>One front vowel: E, I or Y.</summary>
    public const char FrontVowel = '+';

    /// <summary>A suffix: E, ER, ES, ED, ELY or ING. Only valid in a right context.</summary>
    public const char Suffix = '%';
}

namespace VotraxSpeak.Phonetics;

/// <summary>
/// The letter classes used by the NRL letter-to-sound rules. Text is upper case by the time the
/// rules see it.
/// </summary>
internal static class LetterSets
{
    /// <summary>The vowels. Y counts as a consonant.</summary>
    public const string Vowels = "AEIOU";

    /// <summary>The voiced consonants.</summary>
    public const string VoicedConsonants = "BDVGJLMNRWZ";

    /// <summary>The front vowels.</summary>
    public const string FrontVowels = "EIY";
}

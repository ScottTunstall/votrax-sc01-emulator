namespace VotraxSpeak.Phonetics;

/// <summary>
/// Classifies upper-case letters the way the NRL letter-to-sound rules do.
/// </summary>
internal static class LetterClassifier
{
    /// <summary>Returns whether <paramref name="character"/> is an upper-case letter A to Z.</summary>
    /// <param name="character">The character to test.</param>
    /// <returns><see langword="true"/> for A to Z.</returns>
    public static bool IsLetter(char character) => character is >= 'A' and <= 'Z';

    /// <summary>Returns whether <paramref name="character"/> is a vowel.</summary>
    /// <param name="character">The character to test.</param>
    /// <returns><see langword="true"/> for A, E, I, O or U.</returns>
    public static bool IsVowel(char character) => LetterSets.Vowels.IndexOf(character) >= 0;

    /// <summary>Returns whether <paramref name="character"/> is a consonant (including Y).</summary>
    /// <param name="character">The character to test.</param>
    /// <returns><see langword="true"/> for a letter that is not a vowel.</returns>
    public static bool IsConsonant(char character) => IsLetter(character) && !IsVowel(character);

    /// <summary>Returns whether <paramref name="character"/> is a voiced consonant.</summary>
    /// <param name="character">The character to test.</param>
    /// <returns><see langword="true"/> for B, D, V, G, J, L, M, N, R, W or Z.</returns>
    public static bool IsVoicedConsonant(char character) => LetterSets.VoicedConsonants.IndexOf(character) >= 0;

    /// <summary>Returns whether <paramref name="character"/> is a front vowel.</summary>
    /// <param name="character">The character to test.</param>
    /// <returns><see langword="true"/> for E, I or Y.</returns>
    public static bool IsFrontVowel(char character) => LetterSets.FrontVowels.IndexOf(character) >= 0;
}

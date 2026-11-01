using System.Collections.Generic;
using VotraxSpeak.Synthesis;

namespace VotraxSpeak.Phonetics;

/// <summary>
/// Works out how to say a piece of text on the Votrax SC-01.
/// </summary>
internal interface ITextToPhonemeConverter
{
    /// <summary>
    /// Returns the chip phonemes that say <paramref name="text"/>.
    /// </summary>
    /// <param name="text">The text to say.</param>
    /// <returns>The phonemes, ending with a pause.</returns>
    IReadOnlyList<VotraxPhoneme> Convert(string text);
}

using System.Collections.Generic;
using VotraxSpeak.Synthesis;

namespace VotraxSpeak.Phonetics;

/// <summary>
/// Converts English text to Votrax SC-01 phonemes with the NRL letter-to-sound rules.
/// </summary>
internal sealed class EnglishTextToPhonemeConverter : ITextToPhonemeConverter
{
    private readonly LetterToSoundTranslator _translator;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnglishTextToPhonemeConverter"/> class.
    /// </summary>
    /// <param name="translator">Pronounces normalized text.</param>
    public EnglishTextToPhonemeConverter(LetterToSoundTranslator translator)
    {
        _translator = translator;
    }

    /// <inheritdoc/>
    public IReadOnlyList<VotraxPhoneme> Convert(string text)
    {
        string normalized = TextNormalizer.Normalize(text);
        IReadOnlyList<ArpabetPhoneme> pronunciation = _translator.Translate(normalized);
        return UtteranceShaper.Shape(ArpabetToVotraxMapper.Map(pronunciation));
    }
}

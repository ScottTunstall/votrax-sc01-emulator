using VotraxSpeak.Phonetics;
using Xunit;
using static VotraxSpeak.Phonetics.ArpabetPhoneme;

namespace VotraxSpeak.Tests;

public sealed class LetterToSoundTranslatorTests
{
    private readonly LetterToSoundTranslator _translator = new(EnglishRules.All);

    [Fact]
    public void PronouncesAWholeWordRule() => Assert.Equal([DH, AX], _translator.Translate("THE"));

    [Fact]
    public void PronouncesLetterByLetter() => Assert.Equal([K, AE, T], _translator.Translate("CAT"));

    [Fact]
    public void SeparatesWordsWithShortPausesAndSilencesDoubleLetters() =>
        Assert.Equal([HH, EH, L, OW, ShortPause, W, ER, L, D], _translator.Translate("HELLO WORLD"));

    [Fact]
    public void PausesLongerAtPunctuation() => Assert.Equal([N, OW, LongPause], _translator.Translate("NO."));

    [Fact]
    public void LeavesFinalESilent() => Assert.Equal([M, EY, K], _translator.Translate("MAKE"));

    [Fact]
    public void ReturnsNothingForEmptyText() => Assert.Empty(_translator.Translate(string.Empty));
}

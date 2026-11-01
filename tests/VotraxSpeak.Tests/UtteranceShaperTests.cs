using VotraxSpeak.Phonetics;
using Xunit;
using static VotraxSpeak.Synthesis.VotraxPhoneme;

namespace VotraxSpeak.Tests;

public sealed class UtteranceShaperTests
{
    [Fact]
    public void DropsLeadingPausesAndMergesPauseRunsIntoTheLongest() =>
        Assert.Equal([H, PA1, E, PA0, A, PA1], UtteranceShaper.Shape([PA0, H, PA0, PA1, E, PA0, A]));

    [Fact]
    public void AlwaysEndsWithALongPause() => Assert.Equal([PA1], UtteranceShaper.Shape([]));
}

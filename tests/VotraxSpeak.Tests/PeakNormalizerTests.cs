using System.Linq;
using VotraxSpeak.Audio;
using Xunit;

namespace VotraxSpeak.Tests;

public sealed class PeakNormalizerTests
{
    [Fact]
    public void ScalesThePeakToTheNormalizedLevel()
    {
        AudioClip normalized = PeakNormalizer.Normalize(new AudioClip(8_000, [0.1f, -0.2f, 0.05f]));

        Assert.Equal(AudioLevels.NormalizedPeak, normalized.Samples.Max(System.Math.Abs), 5);
        Assert.Equal(8_000, normalized.SampleRateHz);
    }

    [Fact]
    public void LeavesSilenceAlone()
    {
        var silence = new AudioClip(8_000, [0f, 0f]);

        Assert.Same(silence, PeakNormalizer.Normalize(silence));
    }
}

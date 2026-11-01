using System;
using System.Collections.Generic;
using System.Linq;
using VotraxSpeak.Synthesis;
using Xunit;

namespace VotraxSpeak.Tests;

public sealed class VotraxSc01ChipTests
{
    private readonly PhonemeRom _rom = new(RomImageBuilder.Uniform(TestPhonemes.LoudVowel));

    [Fact]
    public void RequestsAPhonemeWhenIdle()
    {
        var chip = new VotraxSc01Chip(_rom, ChipTiming.NominalClockHz);

        Assert.True(chip.IsRequestAsserted);
    }

    [Fact]
    public void SamplesAtOneEighteenthOfTheClock()
    {
        var chip = new VotraxSc01Chip(_rom, ChipTiming.NominalClockHz);

        Assert.Equal(40_000, chip.SampleRateHz);
    }

    [Fact]
    public void RaisesTheRequestLineWhenThePhonemeHasPlayed()
    {
        // 72 ticks to commit (4 samples), then 16 * (4d + 1) * 36 + 2 ticks of phoneme (32 * (4d + 1) + 1 samples).
        const int ExpectedSamples = 4 + (32 * ((4 * TestPhonemes.VowelDuration) + 1)) + 1;
        var chip = new VotraxSc01Chip(_rom, ChipTiming.NominalClockHz);

        chip.Write(VotraxPhoneme.AH);
        int samples = CountSamplesUntilRequest(chip);

        Assert.Equal(ExpectedSamples, samples);
    }

    [Fact]
    public void DropsTheRequestLineWhenWritten()
    {
        var chip = new VotraxSc01Chip(_rom, ChipTiming.NominalClockHz);

        chip.Write(VotraxPhoneme.AH);

        Assert.False(chip.IsRequestAsserted);
    }

    [Fact]
    public void ProducesFiniteAudibleOutputForAVoicedPhoneme()
    {
        var chip = new VotraxSc01Chip(_rom, ChipTiming.NominalClockHz);
        var output = new List<double>();

        chip.Write(VotraxPhoneme.AH);

        while (!chip.IsRequestAsserted)
        {
            chip.Advance();
            output.Add(chip.Output);
        }

        Assert.All(output, sample => Assert.False(double.IsNaN(sample) || double.IsInfinity(sample)));
        Assert.True(output.Max(Math.Abs) > 0);
    }

    [Fact]
    public void RejectsAnOutOfRangeInflection()
    {
        var chip = new VotraxSc01Chip(_rom, ChipTiming.NominalClockHz);

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => chip.SetInflection(4));
    }

    [Fact]
    public void RejectsANonPositiveClock() => Assert.Throws<ArgumentOutOfRangeException>(() => new VotraxSc01Chip(_rom, 0));

    private static int CountSamplesUntilRequest(VotraxSc01Chip chip)
    {
        int samples = 0;

        while (!chip.IsRequestAsserted)
        {
            chip.Advance();
            samples++;
        }

        return samples;
    }
}

using System;
using VotraxSpeak.Synthesis;
using Xunit;

namespace VotraxSpeak.Tests;

public sealed class PhonemeRomTests
{
    [Fact]
    public void DecodesEveryFieldOfAnEntry()
    {
        var expected = new PhonemeParameters(
            Duration: 0x5A,
            VoiceDelay: 0b1001,
            ClosureDelay: 0b0110,
            NoiseVolume: 0b1100,
            NoiseCutoff: 0b0011,
            VoiceVolume: 0b1010,
            F1: 0b0101,
            F2: 0b1110,
            F2Q: 0b0111,
            F3: 0b1000,
            HasClosure: true,
            IsPause: false);

        var rom = new PhonemeRom(RomImageBuilder.Uniform(expected));

        Assert.Equal(expected, rom.GetParameters(VotraxPhoneme.AH));
    }

    [Fact]
    public void MarksOnlyThePausePhonemesAsPauses()
    {
        var rom = new PhonemeRom(RomImageBuilder.Uniform(TestPhonemes.LoudVowel));

        Assert.True(rom.GetParameters(VotraxPhoneme.PA0).IsPause);
        Assert.True(rom.GetParameters(VotraxPhoneme.PA1).IsPause);
        Assert.False(rom.GetParameters(VotraxPhoneme.Stop).IsPause);
    }

    [Fact]
    public void RejectsAnImageOfTheWrongSize() =>
        Assert.Throws<InvalidRomException>(() => new PhonemeRom(new byte[RomLayout.ImageSize - 1]));

    [Fact]
    public void RejectsAnImageThatDoesNotDescribeEveryPhoneme() =>
        Assert.Throws<InvalidRomException>(() => new PhonemeRom(new byte[RomLayout.ImageSize])); // Every entry claims to be phoneme 0.

    [Fact]
    public void RejectsAnUndefinedPhoneme()
    {
        var rom = new PhonemeRom(RomImageBuilder.Uniform(TestPhonemes.LoudVowel));

        _ = Assert.Throws<ArgumentOutOfRangeException>(() => rom.GetParameters((VotraxPhoneme)RomLayout.EntryCount));
    }
}

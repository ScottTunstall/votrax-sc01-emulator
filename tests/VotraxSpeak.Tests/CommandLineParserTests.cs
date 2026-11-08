using System.Linq;
using VotraxSpeak.Cli;
using VotraxSpeak.Speech;
using Xunit;

namespace VotraxSpeak.Tests;

public sealed class CommandLineParserTests
{
    [Fact]
    public void UsesDefaultsWhenOnlyTextIsGiven()
    {
        CommandLineOptions options = ParseSuccessfully("Hello world");

        Assert.Equal("Hello world", options.Text);
        Assert.Equal(VoiceLimits.DefaultSpeedPercent, options.Voice.SpeedPercent);
        Assert.Equal(VoiceLimits.DefaultPitch, options.Voice.Pitch);
        Assert.Null(options.OutputFile);
        Assert.Null(options.RomPath);
        Assert.False(options.ShowHelp);
    }

    [Fact]
    public void JoinsSeparateWordsIntoOneText()
    {
        CommandLineOptions options = ParseSuccessfully("Hello", "-s", "120", "there");

        Assert.Equal("Hello there", options.Text);
    }

    [Theory]
    [InlineData("-f")]
    [InlineData("--file")]
    public void ReadsTheOutputFile(string option)
    {
        CommandLineOptions options = ParseSuccessfully(option, "out.wav", "Hi");

        Assert.Equal("out.wav", options.OutputFile);
    }

    [Theory]
    [InlineData("-s", "-p")]
    [InlineData("--speed", "--pitch")]
    public void ReadsTheVoiceControls(string speedOption, string pitchOption)
    {
        CommandLineOptions options = ParseSuccessfully(speedOption, "150", pitchOption, "3", "Hi");

        Assert.Equal(150, options.Voice.SpeedPercent);
        Assert.Equal(3, options.Voice.Pitch);
    }

    [Fact]
    public void ReadsTheRomPath()
    {
        CommandLineOptions options = ParseSuccessfully("--rom", "chip.bin", "Hi");

        Assert.Equal("chip.bin", options.RomPath);
    }

    [Fact]
    public void LetsTheLastRepeatedOptionWin()
    {
        CommandLineOptions options = ParseSuccessfully("-p", "1", "-p", "2", "Hi");

        Assert.Equal(2, options.Voice.Pitch);
    }

    [Fact]
    public void TreatsEverythingAfterAtSignAsText()
    {
        CommandLineOptions options = ParseSuccessfully("-p", "1", "@", "-5", "degrees", "-p");

        Assert.Equal("-5 degrees -p", options.Text);
        Assert.Equal(1, options.Voice.Pitch);
    }

    [Theory]
    [InlineData("-h")]
    [InlineData("--help")]
    [InlineData("-?")]
    public void AllowsHelpWithoutText(string option)
    {
        CommandLineOptions options = ParseSuccessfully(option);

        Assert.True(options.ShowHelp);
    }

    [Fact]
    public void RequiresText() => Assert.Contains("No text", ParseError(), System.StringComparison.Ordinal);

    [Fact]
    public void RejectsWhitespaceOnlyText() => Assert.Contains("No text", ParseError("   "), System.StringComparison.Ordinal);

    [Theory]
    [InlineData("-s", "24")]
    [InlineData("-s", "401")]
    [InlineData("-s", "fast")]
    [InlineData("-p", "-1")]
    [InlineData("-p", "4")]
    public void RejectsOutOfRangeValues(string option, string value) =>
        Assert.Contains(option, ParseError(option, value, "Hi"), System.StringComparison.Ordinal);

    [Fact]
    public void RejectsAMissingValue() => Assert.Contains("needs a value", ParseError("Hi", "-f"), System.StringComparison.Ordinal);

    [Fact]
    public void RejectsAnUnknownOption() => Assert.Contains("Unknown option '-x'", ParseError("-x", "Hi"), System.StringComparison.Ordinal);

    [Fact]
    public void ReadsPhonemesInPlaceOfText()
    {
        CommandLineOptions options = ParseSuccessfully("--phonemes", "PA1 N PA1");

        Assert.NotNull(options.Phonemes);
        Assert.Equal(3, options.Phonemes.Count);
        Assert.Equal(string.Empty, options.Text);
    }

    [Fact]
    public void RejectsPhonemesTogetherWithText() =>
        Assert.Contains("not both", ParseError("Hello", "--phonemes", "N"), System.StringComparison.Ordinal);

    [Fact]
    public void ReadsPhonemeNamesGivenAsSeparateArguments()
    {
        CommandLineOptions options = ParseSuccessfully("--gorf", "PA1", "I0", "N", "PA1");

        Assert.Equal(4, options.Phonemes!.Count);
        Assert.Equal(0x3E, (byte)options.Phonemes[0].Phoneme);
        Assert.Equal(0x3E, (byte)options.Phonemes[3].Phoneme);
    }

    [Fact]
    public void ReadsTheSamePhonemesWhetherQuotedOrNot() =>
        Assert.Equal(
            ParseSuccessfully("--gorf", "PA1 I0 N PA1").Phonemes!.Select(cue => cue.Phoneme),
            ParseSuccessfully("--gorf", "PA1", "I0", "N", "PA1").Phonemes!.Select(cue => cue.Phoneme));

    [Theory]
    [InlineData("-f", "--gorf")]
    [InlineData("--gorf", "-f")]
    public void AcceptsGorfBeforeOrAfterTheFileOption(string first, string second)
    {
        string[] arguments = first == "-f"
            ? ["-f", "out.ogg", second, "PA1", "I0", "N"]
            : [first, "PA1", "I0", "N", second, "out.ogg"];

        CommandLineOptions options = ParseSuccessfully(arguments);

        Assert.Equal("out.ogg", options.OutputFile);
        Assert.Equal(3, options.Phonemes!.Count);
        Assert.Equal(string.Empty, options.Text);
    }

    [Fact]
    public void AcceptsQuotedGorfBeforeOrAfterOtherOptions()
    {
        CommandLineOptions before = ParseSuccessfully("--gorf", "PA1 I0 N", "-f", "a.wav", "-s", "120");
        CommandLineOptions after = ParseSuccessfully("-s", "120", "-f", "a.wav", "--gorf", "PA1 I0 N");

        Assert.Equal("a.wav", before.OutputFile);
        Assert.Equal(before.OutputFile, after.OutputFile);
        Assert.Equal(before.Voice.SpeedPercent, after.Voice.SpeedPercent);
        Assert.Equal(before.Phonemes!.Count, after.Phonemes!.Count);
    }

    [Theory]
    [InlineData("--phonemes")]
    [InlineData("--gorf")]
    public void RejectsPhonemeOptionsFollowedByAnotherOption(string option) =>
        Assert.Contains("needs a value", ParseError(option, "-f", "out.wav"), System.StringComparison.Ordinal);

    [Fact]
    public void SuggestsTheDoubleDashSpellingOfALongOption() =>
        Assert.Contains("Did you mean '--gorf'?", ParseError("-gorf", "PA1 I0 N"), System.StringComparison.Ordinal);

    [Fact]
    public void DoesNotSuggestAnythingForAnUnrelatedOption() =>
        Assert.DoesNotContain("Did you mean", ParseError("-x", "Hi"), System.StringComparison.Ordinal);

    [Fact]
    public void ReportsAnUnknownPhonemeName() =>
        Assert.Contains("'--phonemes'", ParseError("--phonemes", "N BOGUS"), System.StringComparison.Ordinal);

    [Fact]
    public void ReadsGorfPhonemesInPlaceOfText()
    {
        CommandLineOptions options = ParseSuccessfully("--gorf", "PA1 pAE K0");

        Assert.NotNull(options.Phonemes);
        Assert.Equal<byte>([0x3E, 0x2E, 0x19], options.Phonemes.Select(cue => (byte)cue.Phoneme));
    }

    [Fact]
    public void ReportsAnUnknownGorfName() =>
        Assert.Contains("'--gorf'", ParseError("--gorf", "N AE"), System.StringComparison.Ordinal);

    private static CommandLineOptions ParseSuccessfully(params string[] arguments)
    {
        CommandLineParseResult result = CommandLineParser.Parse(arguments);

        Assert.Equal(string.Empty, result.Error);
        Assert.NotNull(result.Options);
        return result.Options;
    }

    private static string ParseError(params string[] arguments)
    {
        CommandLineParseResult result = CommandLineParser.Parse(arguments);

        Assert.Null(result.Options);
        return result.Error;
    }
}

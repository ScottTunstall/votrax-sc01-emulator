using System;
using System.Linq;
using VotraxSpeak.Phonetics;
using Xunit;

namespace VotraxSpeak.Tests;

public sealed class PhonemeListParserTests
{
    /// <summary>The "Insert coin." record from the Gorf arcade disassembly (SPK_INSERT), without its length byte.</summary>
    private static readonly byte[] InsertCoinBytes =
        [0x3E, 0x27, 0x0D, 0x1F, 0x3A, 0x2A, 0x3E, 0x19, 0x35, 0x23, 0x09, 0x21, 0x0D, 0x0D, 0x3E];

    [Fact]
    public void ParsesInsertCoinToTheBytesInTheGorfData()
    {
        var cues = PhonemeListParser.Parse("PA1 I N S ER T PA1 K O1 UH3 I3 AY N N PA1", PhonemeDialect.Votrax);

        Assert.Equal(InsertCoinBytes, cues.Select(cue => (byte)cue.Phoneme));
        Assert.All(cues, cue => Assert.Null(cue.Inflection));
    }

    [Fact]
    public void AcceptsCommasWhitespaceAndAnyCase()
    {
        var cues = PhonemeListParser.Parse("ae,  k\tpa1 stop", PhonemeDialect.Votrax);

        Assert.Equal<byte>([0x2E, 0x19, 0x3E, 0x3F], cues.Select(cue => (byte)cue.Phoneme));
    }

    [Fact]
    public void AppliesModifiersToThePhonemeBeforeThem()
    {
        var cues = PhonemeListParser.Parse("AH1 I1 UP Y UP AE HI M HI UP", PhonemeDialect.Votrax);

        Assert.Equal<int?>([null, 1, 1, 2, 3], cues.Select(cue => cue.Inflection));
        Assert.Equal(0x0C, (byte)cues[4].Phoneme);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ,  ")]
    public void RejectsAnEmptyList(string list) =>
        Assert.Contains("empty", Assert.Throws<PhonemeListException>(() => PhonemeListParser.Parse(list, PhonemeDialect.Votrax)).Message, StringComparison.Ordinal);

    [Fact]
    public void RejectsAModifierWithNothingBeforeIt() =>
        Assert.Contains("'UP'", Assert.Throws<PhonemeListException>(() => PhonemeListParser.Parse("UP N", PhonemeDialect.Votrax)).Message, StringComparison.Ordinal);

    [Theory]
    [InlineData("ENDPRIM")]
    [InlineData("5")]
    [InlineData("pAE")]
    public void RejectsAnUnknownName(string name) =>
        Assert.Contains($"'{name}'", Assert.Throws<PhonemeListException>(() => PhonemeListParser.Parse("N " + name, PhonemeDialect.Votrax)).Message, StringComparison.Ordinal);

    [Fact]
    public void KnowsAllSixtyFourPhonemes()
    {
        string[] names = ["EH3", "EH2", "EH1", "PA0", "DT", "A1", "A2", "ZH", "AH2", "I3", "I2", "I1", "M", "N", "B", "V",
            "CH", "SH", "Z", "AW1", "NG", "AH1", "OO1", "OO", "L", "K", "J", "H", "G", "F", "D", "S",
            "A", "AY", "Y1", "UH3", "AH", "P", "O", "I", "U", "Y", "T", "R", "E", "W", "AE", "AE1",
            "AW2", "UH2", "UH1", "UH", "O2", "O1", "IU", "U1", "THV", "TH", "ER", "EH", "E1", "AW", "PA1", "STOP"];

        var cues = PhonemeListParser.Parse(string.Join(" ", names), PhonemeDialect.Votrax);

        Assert.Equal(Enumerable.Range(0, 64).Select(code => (byte)code), cues.Select(cue => (byte)cue.Phoneme));
    }

    [Fact]
    public void ParsesInsertCoinInGorfSpelling()
    {
        var cues = PhonemeListParser.Parse("PA1 I0 N S ER T PA1 K0 O1 UH3 I3 AY N N PA1", PhonemeDialect.Gorf);

        Assert.Equal(InsertCoinBytes, cues.Select(cue => (byte)cue.Phoneme));
    }

    [Fact]
    public void KeepsGorfPA1AndLowerCasePA1Apart()
    {
        var cues = PhonemeListParser.Parse("pA1 PA1", PhonemeDialect.Gorf);

        Assert.Equal<byte>([0x06, 0x3E], cues.Select(cue => (byte)cue.Phoneme));
    }

    [Theory]
    [InlineData("pa1")]
    [InlineData("K")]
    [InlineData("A1")]
    public void RejectsNamesThatAreNotGorfSpelling(string name) =>
        Assert.Contains($"'{name}'", Assert.Throws<PhonemeListException>(() => PhonemeListParser.Parse(name, PhonemeDialect.Gorf)).Message, StringComparison.Ordinal);

    [Fact]
    public void KnowsAllSixtyFourGorfNames()
    {
        string[] names = ["EH3", "EH2", "EH1", "PA0", "DT", "pA2", "pA1", "ZH", "AH2", "I3", "I2", "I1", "M", "N", "pB", "V",
            "CH", "SH", "Z", "AW1", "NG", "AH1", "OO1", "OO", "L", "K0", "J0", "H", "G", "pF", "pD", "S",
            "pA", "AY", "Y1", "UH3", "AH", "P", "O", "I0", "U", "Y", "T", "R", "pE", "W", "pAE", "pAE1",
            "AW2", "UH2", "UH1", "UH", "O2", "O1", "IU", "U1", "THV", "TH", "ER", "EH", "pE1", "AW", "PA1", "STOP"];

        var cues = PhonemeListParser.Parse(string.Join(" ", names), PhonemeDialect.Gorf);

        Assert.Equal(Enumerable.Range(0, 64).Select(code => (byte)code), cues.Select(cue => (byte)cue.Phoneme));
    }
}

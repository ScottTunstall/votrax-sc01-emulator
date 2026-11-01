using VotraxSpeak.Phonetics;
using Xunit;

namespace VotraxSpeak.Tests;

public sealed class NumberToWordsConverterTests
{
    [Theory]
    [InlineData("0", "ZERO")]
    [InlineData("7", "SEVEN")]
    [InlineData("13", "THIRTEEN")]
    [InlineData("20", "TWENTY")]
    [InlineData("42", "FORTY TWO")]
    [InlineData("100", "ONE HUNDRED")]
    [InlineData("1905", "ONE THOUSAND NINE HUNDRED FIVE")]
    [InlineData("2026", "TWO THOUSAND TWENTY SIX")]
    [InlineData("1000000", "ONE MILLION")]
    [InlineData("3000012", "THREE MILLION TWELVE")]
    public void SpellsOutWholeNumbers(string digits, string expected) =>
        Assert.Equal(expected, NumberToWordsConverter.ConvertInteger(digits));

    [Theory]
    [InlineData("007", "ZERO ZERO SEVEN")]
    [InlineData("1234567890123456", "ONE TWO THREE FOUR FIVE SIX SEVEN EIGHT NINE ZERO ONE TWO THREE FOUR FIVE SIX")]
    public void ReadsCodesAndVeryLongNumbersDigitByDigit(string digits, string expected) =>
        Assert.Equal(expected, NumberToWordsConverter.ConvertInteger(digits));
}

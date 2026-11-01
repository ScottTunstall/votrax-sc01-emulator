using VotraxSpeak.Phonetics;
using Xunit;

namespace VotraxSpeak.Tests;

public sealed class TextNormalizerTests
{
    [Theory]
    [InlineData("Hello, world!", "HELLO, WORLD!")]
    [InlineData("  lots   of\tspace\n", "LOTS OF SPACE")]
    [InlineData("I have 2 cats", "I HAVE TWO CATS")]
    [InlineData("pi is 3.14", "PI IS THREE POINT ONE FOUR")]
    [InlineData("1,000 miles", "ONE THOUSAND MILES")]
    [InlineData("R&D", "R AND D")]
    [InlineData("don’t", "DON'T")]
    [InlineData("wait; then go", "WAIT, THEN GO")]
    [InlineData("#$^", "")]
    public void Normalizes(string text, string expected) => Assert.Equal(expected, TextNormalizer.Normalize(text));
}

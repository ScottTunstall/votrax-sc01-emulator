using System.Linq;
using VotraxSpeak.Synthesis;
using Xunit;

namespace VotraxSpeak.Tests;

public sealed class KnownRomImagesTests
{
    [Fact]
    public void AcceptsOnlySc01aBin() =>
        Assert.Equal(["sc01a.bin"], KnownRomImages.All.Select(image => image.FileName));
}

namespace VotraxSpeak.Phonetics;

/// <summary>
/// The words of a phoneme list that are not phoneme names.
/// </summary>
/// <remarks>
/// In the Gorf arcade source, <c>HI</c> and <c>UP</c> follow the phoneme they modify and set the top two bits of
/// its byte ($80 and $40), which the game wires to the chip's inflection input.
/// </remarks>
internal static class PhonemeListSyntax
{
    /// <summary>Modifier that sets the high inflection bit ($80 in the Gorf data).</summary>
    public const string HighModifier = "HI";

    /// <summary>Modifier that sets the low inflection bit ($40 in the Gorf data).</summary>
    public const string UpModifier = "UP";

    /// <summary>The inflection bit that <see cref="HighModifier"/> sets.</summary>
    public const int HighInflectionBit = 2;

    /// <summary>The inflection bit that <see cref="UpModifier"/> sets.</summary>
    public const int UpInflectionBit = 1;
}

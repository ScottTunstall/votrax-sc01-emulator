namespace VotraxSpeak.Phonetics;

/// <summary>
/// The spelling convention a phoneme list is written in.
/// </summary>
internal enum PhonemeDialect
{
    /// <summary>The SC-01 data-sheet names used by MAME (<c>A1</c>, <c>K</c>, <c>AE</c>…); not case-sensitive.</summary>
    Votrax = 0,

    /// <summary>The names used by the Gorf arcade source (<c>pA2</c>, <c>K0</c>, <c>pAE</c>…); case-sensitive.</summary>
    Gorf = 1,
}

namespace VotraxSpeak.Synthesis;

/// <summary>
/// A verified dump of the SC-01's internal phoneme ROM.
/// </summary>
/// <param name="FileName">The file name MAME uses for the dump.</param>
/// <param name="ChipName">The chip revision the dump came from.</param>
/// <param name="Sha256">The lower-case hexadecimal SHA-256 of the 512-byte image.</param>
internal sealed record KnownRomImage(string FileName, string ChipName, string Sha256);

using System.Collections.Generic;

namespace VotraxSpeak.Synthesis;

/// <summary>
/// The phoneme ROM dump this emulation accepts. Its SHA-1 hash matches MAME's ROM definition
/// (sc01a.bin 1d6da90b…); SHA-256 is stored because SHA-1 is deprecated. MAME's older sc01.bin is not accepted.
/// </summary>
internal static class KnownRomImages
{
    /// <summary>Gets the accepted images, in the order they are searched for.</summary>
    public static IReadOnlyList<KnownRomImage> All { get; } =
    [
        new("sc01a.bin", "SC-01-A", "854f7d07fa0cd09afb06dc86216456f47e1a07bbd03810b4161dba2e49055dc4"),
    ];
}

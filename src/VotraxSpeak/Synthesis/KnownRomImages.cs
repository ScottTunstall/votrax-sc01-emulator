using System.Collections.Generic;

namespace VotraxSpeak.Synthesis;

/// <summary>
/// The phoneme ROM dumps this emulation accepts. Their SHA-1 hashes match MAME's ROM definitions
/// (sc01a.bin 1d6da90b…, sc01.bin 268b5884…); SHA-256 is stored because SHA-1 is deprecated.
/// </summary>
internal static class KnownRomImages
{
    /// <summary>Gets the accepted images, in the order they are searched for (newest revision first).</summary>
    public static IReadOnlyList<KnownRomImage> All { get; } =
    [
        new("sc01a.bin", "SC-01-A", "854f7d07fa0cd09afb06dc86216456f47e1a07bbd03810b4161dba2e49055dc4"),
        new("sc01.bin", "SC-01", "5daa68cc2b00ba8e86f8bb8b96e74518d6af12acb71c4c46b304e28f2479607d"),
    ];
}

using System.Collections.Generic;
using VotraxSpeak.Synthesis;

namespace VotraxSpeak.Tests;

/// <summary>
/// Builds synthetic phoneme ROM images, so the tests do not need the real (copyrighted) firmware.
/// </summary>
internal static class RomImageBuilder
{
    /// <summary>
    /// Returns a ROM image in which every phoneme has the same parameters.
    /// </summary>
    /// <param name="parameters">The parameters for every phoneme.</param>
    /// <returns>A 512-byte ROM image.</returns>
    public static byte[] Uniform(PhonemeParameters parameters)
    {
        var image = new byte[RomLayout.ImageSize];

        for (int code = 0; code < RomLayout.EntryCount; code++)
        {
            // Store the entries in reverse order to show the decoder looks phonemes up by code.
            int entry = RomLayout.EntryCount - 1 - code;
            ulong value = Encode(parameters, code);

            for (int i = 0; i < RomLayout.BytesPerEntry; i++)
            {
                image[(entry * RomLayout.BytesPerEntry) + i] = (byte)(value >> (8 * i));
            }
        }

        return image;
    }

    /// <summary>
    /// Returns a ROM entry holding <paramref name="parameters"/> for phoneme <paramref name="code"/>,
    /// using the same bit layout that MAME's decoder expects.
    /// </summary>
    /// <param name="parameters">The parameters to encode.</param>
    /// <param name="code">The phoneme code.</param>
    /// <returns>The 64-bit ROM entry.</returns>
    public static ulong Encode(PhonemeParameters parameters, int code)
    {
        ulong value = (ulong)code << RomLayout.PhoneCodeShift;

        value |= Scatter(parameters.F1, 0, 7, 14, 21);
        value |= Scatter(parameters.VoiceVolume, 1, 8, 15, 22);
        value |= Scatter(parameters.F2, 2, 9, 16, 23);
        value |= Scatter(parameters.NoiseCutoff, 3, 10, 17, 24);
        value |= Scatter(parameters.F2Q, 4, 11, 18, 25);
        value |= Scatter(parameters.F3, 5, 12, 19, 26);
        value |= Scatter(parameters.NoiseVolume, 6, 13, 20, 27);
        value |= Scatter(parameters.ClosureDelay, 34, 32, 30, 28);
        value |= Scatter(parameters.VoiceDelay, 35, 33, 31, 29);
        value |= parameters.HasClosure ? 1UL << RomLayout.ClosureBit : 0;

        // The duration is stored inverted.
        value |= Scatter(~parameters.Duration & 0x7F, 37, 38, 39, 40, 41, 42, 43);

        return value;
    }

    private static ulong Scatter(int bits, params IReadOnlyList<int> positions)
    {
        ulong value = 0;

        for (int i = 0; i < positions.Count; i++)
        {
            int bit = (bits >> (positions.Count - 1 - i)) & 1;
            value |= (ulong)bit << positions[i];
        }

        return value;
    }
}

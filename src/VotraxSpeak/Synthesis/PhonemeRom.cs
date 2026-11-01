// Ported from MAME src/devices/sound/votrax.cpp (phone_commit).
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace VotraxSpeak.Synthesis;

/// <summary>
/// The decoded contents of the SC-01's internal phoneme ROM.
/// </summary>
internal sealed class PhonemeRom
{
    private readonly PhonemeParameters[] _parametersByCode = new PhonemeParameters[RomLayout.EntryCount];

    /// <summary>
    /// Initializes a new instance of the <see cref="PhonemeRom"/> class by decoding a ROM image.
    /// </summary>
    /// <param name="image">The 512-byte ROM image.</param>
    /// <exception cref="InvalidRomException">The image is the wrong size or does not describe all 64 phonemes.</exception>
    public PhonemeRom(IReadOnlyList<byte> image)
    {
        if (image.Count != RomLayout.ImageSize)
        {
            throw new InvalidRomException(string.Format(
                CultureInfo.InvariantCulture,
                "A phoneme ROM image is {0} bytes, but this one is {1} bytes.",
                RomLayout.ImageSize,
                image.Count));
        }

        for (int entry = 0; entry < RomLayout.EntryCount; entry++)
        {
            ulong value = ReadEntry(image, entry);
            int code = (int)(value >> RomLayout.PhoneCodeShift) & ChipConstants.PhoneCodeMask;
            _parametersByCode[code] = Decode(value, code);
        }

        if (_parametersByCode.Any(parameters => parameters is null))
        {
            throw new InvalidRomException("The ROM image does not describe all 64 phonemes.");
        }
    }

    /// <summary>
    /// Returns the ROM parameters for a phoneme.
    /// </summary>
    /// <param name="phoneme">The phoneme.</param>
    /// <returns>Its parameters.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="phoneme"/> is not a defined phoneme.</exception>
    public PhonemeParameters GetParameters(VotraxPhoneme phoneme)
    {
        int code = (int)phoneme;

        if (code is < 0 or >= RomLayout.EntryCount)
        {
            throw new ArgumentOutOfRangeException(nameof(phoneme), phoneme, "Not a Votrax SC-01 phoneme.");
        }

        return _parametersByCode[code];
    }

    private static ulong ReadEntry(IReadOnlyList<byte> image, int entry)
    {
        ulong value = 0;
        int offset = entry * RomLayout.BytesPerEntry;

        for (int i = 0; i < RomLayout.BytesPerEntry; i++)
        {
            value |= (ulong)image[offset + i] << (8 * i);
        }

        return value;
    }

    private static PhonemeParameters Decode(ulong value, int code)
    {
        // The delays have their bit orders inverted compared to everything else, due to a
        // miswired comparator in the prototype that was compensated for in the ROM.
        return new PhonemeParameters(
            Duration: Bits(~value, 37, 38, 39, 40, 41, 42, 43),
            VoiceDelay: Bits(value, 35, 33, 31, 29),
            ClosureDelay: Bits(value, 34, 32, 30, 28),
            NoiseVolume: Bits(value, 6, 13, 20, 27),
            NoiseCutoff: Bits(value, 3, 10, 17, 24),
            VoiceVolume: Bits(value, 1, 8, 15, 22),
            F1: Bits(value, 0, 7, 14, 21),
            F2: Bits(value, 2, 9, 16, 23),
            F2Q: Bits(value, 4, 11, 18, 25),
            F3: Bits(value, 5, 12, 19, 26),
            HasClosure: Bits(value, RomLayout.ClosureBit) != 0,
            IsPause: code is (int)VotraxPhoneme.PA0 or (int)VotraxPhoneme.PA1);
    }

    /// <summary>
    /// Gathers the given bits of <paramref name="value"/> into an integer, most significant first
    /// (MAME's <c>bitswap</c>).
    /// </summary>
    /// <param name="value">The ROM entry.</param>
    /// <param name="positions">The bit positions to gather, most significant first.</param>
    /// <returns>The gathered bits.</returns>
    private static int Bits(ulong value, params int[] positions)
    {
        int result = 0;

        foreach (int position in positions)
        {
            result = (result << 1) | (int)((value >> position) & 1);
        }

        return result;
    }
}

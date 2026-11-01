// Ported from MAME src/devices/sound/votrax.cpp (ROM_START, phone_commit).
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

namespace VotraxSpeak.Synthesis;

/// <summary>
/// Layout of the SC-01's internal phoneme ROM: 64 little-endian 64-bit entries.
/// </summary>
internal static class RomLayout
{
    /// <summary>Size of a ROM image, in bytes.</summary>
    public const int ImageSize = 0x200;

    /// <summary>Number of entries; one per phoneme.</summary>
    public const int EntryCount = 64;

    /// <summary>Size of one entry, in bytes.</summary>
    public const int BytesPerEntry = 8;

    /// <summary>Bit position of the 6-bit phoneme code within an entry.</summary>
    public const int PhoneCodeShift = 56;

    /// <summary>Bit holding the closure flag.</summary>
    public const int ClosureBit = 36;
}

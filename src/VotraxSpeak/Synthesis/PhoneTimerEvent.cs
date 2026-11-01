// Ported from MAME src/devices/sound/votrax.h (T_COMMIT_PHONE, T_END_OF_PHONE).
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

namespace VotraxSpeak.Synthesis;

/// <summary>
/// What happens when the chip's phone timer expires.
/// </summary>
internal enum PhoneTimerEvent
{
    /// <summary>The timer is not running.</summary>
    None = 0,

    /// <summary>The written phoneme is latched and starts playing.</summary>
    CommitPhone = 1,

    /// <summary>The current phoneme has finished and the chip requests the next one.</summary>
    EndOfPhone = 2,
}

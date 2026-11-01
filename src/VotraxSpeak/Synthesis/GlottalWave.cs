// Ported from MAME src/devices/sound/votrax.cpp (s_glottal_wave).
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

using System.Collections.Generic;

namespace VotraxSpeak.Synthesis;

/// <summary>
/// The glottal pulse produced by the SC-01's transistor resistor ladder, rescaled so the
/// middle value is 0 and the top is 1. Pitch counter values past the end of the table are silent.
/// </summary>
internal static class GlottalWave
{
    /// <summary>Gets the wave levels, indexed by the pitch counter divided by eight.</summary>
    public static IReadOnlyList<double> Levels { get; } =
    [
        0,
        -4 / 7.0,
        7 / 7.0,
        6 / 7.0,
        5 / 7.0,
        4 / 7.0,
        3 / 7.0,
        2 / 7.0,
        1 / 7.0,
    ];
}

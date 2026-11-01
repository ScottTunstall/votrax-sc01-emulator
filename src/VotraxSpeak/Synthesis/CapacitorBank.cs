// Ported from MAME src/devices/sound/votrax.h (bits_to_caps).
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

using System.Collections.Generic;

namespace VotraxSpeak.Synthesis;

/// <summary>
/// Adds up the capacitors that a parameter's bits switch into a filter.
/// </summary>
internal static class CapacitorBank
{
    /// <summary>
    /// Returns the total capacitance switched in by <paramref name="bits"/>.
    /// </summary>
    /// <param name="bits">The parameter value; bit 0 selects the first capacitor.</param>
    /// <param name="capacitors">The capacitor for each bit, least significant bit first.</param>
    /// <returns>The sum of the selected capacitors.</returns>
    public static double Total(int bits, IReadOnlyList<double> capacitors)
    {
        double total = 0;

        for (int bit = 0; bit < capacitors.Count; bit++)
        {
            if ((bits & (1 << bit)) != 0)
            {
                total += capacitors[bit];
            }
        }

        return total;
    }
}

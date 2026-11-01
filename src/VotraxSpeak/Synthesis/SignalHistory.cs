// Ported from MAME src/devices/sound/votrax.h (shift_hist).
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

using System;

namespace VotraxSpeak.Synthesis;

/// <summary>
/// A fixed-length history of signal values, newest first, as used by the digital filters.
/// </summary>
internal sealed class SignalHistory
{
    private readonly double[] _values;

    /// <summary>
    /// Initializes a new instance of the <see cref="SignalHistory"/> class, filled with zeros.
    /// </summary>
    /// <param name="length">The number of values to remember.</param>
    public SignalHistory(int length)
    {
        if (length <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), length, "A history must hold at least one value.");
        }

        _values = new double[length];
    }

    /// <summary>Gets the value pushed <paramref name="age"/> pushes ago (0 is the newest).</summary>
    /// <param name="age">How many pushes ago the value was recorded.</param>
    public double this[int age] => _values[age];

    /// <summary>
    /// Records <paramref name="value"/> as the newest value, discarding the oldest.
    /// </summary>
    /// <param name="value">The new value.</param>
    public void Push(double value)
    {
        Array.Copy(_values, 0, _values, 1, _values.Length - 1);
        _values[0] = value;
    }
}

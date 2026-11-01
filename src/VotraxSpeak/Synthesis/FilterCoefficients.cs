// Ported from MAME src/devices/sound/votrax.h (apply_filter).
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

using System;
using System.Collections.Generic;

namespace VotraxSpeak.Synthesis;

/// <summary>
/// The coefficients of a digital filter
/// <c>H(z) = (a0 + a1·z⁻¹ + …) / (b0 + b1·z⁻¹ + …)</c>.
/// </summary>
internal sealed class FilterCoefficients
{
    private readonly double[] _inputCoefficients;
    private readonly double[] _outputCoefficients;

    /// <summary>
    /// Initializes a new instance of the <see cref="FilterCoefficients"/> class.
    /// </summary>
    /// <param name="inputCoefficients">The <c>a</c> coefficients, applied to past inputs.</param>
    /// <param name="outputCoefficients">The <c>b</c> coefficients, applied to past outputs; <c>b0</c> normalizes.</param>
    public FilterCoefficients(IEnumerable<double> inputCoefficients, IEnumerable<double> outputCoefficients)
    {
        _inputCoefficients = [.. inputCoefficients];
        _outputCoefficients = [.. outputCoefficients];

        if (_inputCoefficients.Length == 0 || _outputCoefficients.Length == 0)
        {
            throw new ArgumentException("A filter needs at least one input and one output coefficient.");
        }
    }

    /// <summary>
    /// Computes the next filter output.
    /// </summary>
    /// <param name="inputs">The input history, including the current input at age 0.</param>
    /// <param name="outputs">The output history; age 0 is the previous output.</param>
    /// <returns>The new output value.</returns>
    public double Apply(SignalHistory inputs, SignalHistory outputs)
    {
        double total = 0;

        for (int i = 0; i < _inputCoefficients.Length; i++)
        {
            total += inputs[i] * _inputCoefficients[i];
        }

        for (int i = 1; i < _outputCoefficients.Length; i++)
        {
            total -= outputs[i - 1] * _outputCoefficients[i];
        }

        return total / _outputCoefficients[0];
    }
}

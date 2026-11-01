// Ported from MAME src/devices/sound/votrax.cpp (build_*_filter).
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

using System;

namespace VotraxSpeak.Synthesis;

/// <summary>
/// Turns the SC-01's switched-capacitor circuits into digital filters.
/// </summary>
/// <remarks>
/// Each circuit's analog transfer function H(s) is derived from its capacitor ratios (a switched
/// capacitor behaves like a resistor of 1/(C·f) at switching frequency f), then converted to H(z)
/// with a bilinear transform pre-warped at the filter's peak frequency. The derivations are
/// described at length in the comments of MAME's votrax.cpp.
/// </remarks>
internal sealed class FilterDesigner
{
    private readonly double _capacitorClock;
    private readonly double _sampleClock;

    /// <summary>
    /// Initializes a new instance of the <see cref="FilterDesigner"/> class.
    /// </summary>
    /// <param name="mainClockHz">The chip's main clock, in hertz.</param>
    public FilterDesigner(int mainClockHz)
    {
        if (mainClockHz <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(mainClockHz), mainClockHz, "The clock must be positive.");
        }

        _capacitorClock = mainClockHz / (double)ChipTiming.MainClockTicksPerCapacitorCycle;
        _sampleClock = mainClockHz / (double)ChipTiming.MainClockTicksPerSample;
    }

    /// <summary>
    /// Designs the two-op-amp band-pass circuit used for F1, F2 (voice), F3 and F4.
    /// </summary>
    /// <param name="inputTop">Unswitched capacitor at the input.</param>
    /// <param name="inputBottom">Switched capacitor at the input.</param>
    /// <param name="feedbackTop">Unswitched capacitor over the first op-amp.</param>
    /// <param name="feedbackBottom">Switched capacitor over the first op-amp.</param>
    /// <param name="coupling">Capacitor between the two op-amps.</param>
    /// <param name="output">Capacitor over the second op-amp.</param>
    /// <returns>The digital filter.</returns>
    public FilterCoefficients DesignStandard(
        double inputTop,
        double inputBottom,
        double feedbackTop,
        double feedbackBottom,
        double coupling,
        double output)
    {
        // H(s) = (1 + k0·s) / (1 + k1·s + k2·s²)
        double k0 = inputTop / (_capacitorClock * inputBottom);
        double k1 = output * feedbackTop / (_capacitorClock * inputBottom * coupling);
        double k2 = output * feedbackBottom / (_capacitorClock * _capacitorClock * inputBottom * coupling);

        double peakFrequency = Math.Sqrt(Math.Abs((k0 * k1) - k2)) / (2 * Math.PI * k2);
        double warp = WarpMultiplier(peakFrequency);

        double m0 = warp * k0;
        double m1 = warp * k1;
        double m2 = warp * warp * k2;

        return new FilterCoefficients(
            [1 + m0, 3 + m0, 3 - m0, 1 - m0],
            [1 + m1 + m2, 3 + m1 - m2, 3 - m1 - m2, 1 - m1 + m2]);
    }

    /// <summary>
    /// Designs the single-op-amp low-pass circuit at the end of the signal path.
    /// </summary>
    /// <param name="feedbackTop">Unswitched capacitor over the op-amp.</param>
    /// <param name="feedbackBottom">Switched capacitor over the op-amp.</param>
    /// <returns>The digital filter.</returns>
    public FilterCoefficients DesignLowpass(double feedbackTop, double feedbackBottom)
    {
        // H(s) = 1 / (1 + k·s)
        double k = feedbackBottom / (_capacitorClock * feedbackTop) * AnalogConstants.LowpassCutoffCorrection;

        double peakFrequency = 1 / (2 * Math.PI * k);
        double m = WarpMultiplier(peakFrequency) * k;

        return new FilterCoefficients([1], [1 + m, 1 - m]);
    }

    /// <summary>
    /// Designs the circuit that shapes the white noise source.
    /// </summary>
    /// <param name="firstFeedback">Capacitor over the first op-amp.</param>
    /// <param name="couplingTop">Unswitched capacitor between the op-amps.</param>
    /// <param name="couplingBottom">Switched capacitor between the op-amps.</param>
    /// <param name="secondFeedback">Capacitor over the second op-amp.</param>
    /// <param name="output">Switched capacitor after the second op-amp.</param>
    /// <returns>The digital filter.</returns>
    public FilterCoefficients DesignNoiseShaper(
        double firstFeedback,
        double couplingTop,
        double couplingBottom,
        double secondFeedback,
        double output)
    {
        // H(s) = k0·s / (1 + k1·s + k2·s²)
        double k0 = couplingTop * secondFeedback * couplingBottom / output;
        double k1 = couplingTop * (_capacitorClock * couplingBottom);
        double k2 = firstFeedback * couplingTop * secondFeedback / (_capacitorClock * output);

        double peakFrequency = Math.Sqrt(1 / k2) / (2 * Math.PI);
        double warp = WarpMultiplier(peakFrequency);

        double m0 = warp * k0;
        double m1 = warp * k1;
        double m2 = warp * warp * k2;

        return new FilterCoefficients(
            [m0, 0, -m0],
            [1 + m1 + m2, 2 - (2 * m2), 1 - m1 + m2]);
    }

    /// <summary>
    /// Designs the path that injects noise into the F2 filter, rescaled so that H(∞) = 1.
    /// </summary>
    /// <param name="inputBottom">Switched capacitor at the input.</param>
    /// <param name="feedbackTop">Unswitched capacitor over the first op-amp.</param>
    /// <param name="feedbackBottom">Switched capacitor over the first op-amp.</param>
    /// <param name="coupling">Capacitor between the two op-amps.</param>
    /// <returns>The digital filter.</returns>
    /// <remarks>The capacitor over the second op-amp cancels out of this transfer function.</remarks>
    public FilterCoefficients DesignInjection(
        double inputBottom,
        double feedbackTop,
        double feedbackBottom,
        double coupling)
    {
        // H(s) = (k0 + k2·s) / (k1 + k2·s), not pre-warped.
        double k0 = _capacitorClock * feedbackTop;
        double k1 = _capacitorClock * ((inputBottom * coupling / feedbackTop) + feedbackTop);
        double k2 = feedbackBottom;

        double m = 2 * _sampleClock * k2;

        return new FilterCoefficients([k0 + m, k0 - m], [k1 + m, k1 - m]);
    }

    private double WarpMultiplier(double peakFrequency) =>
        2 * Math.PI * peakFrequency / Math.Tan(Math.PI * peakFrequency / _sampleClock);
}

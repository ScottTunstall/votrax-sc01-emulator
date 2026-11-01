// Ported from MAME src/devices/sound/votrax.cpp (filters_commit).
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

using System.Collections.Generic;

namespace VotraxSpeak.Synthesis;

/// <summary>
/// Capacitor sizes of the SC-01's switched-capacitor filters, in square micrometres as measured
/// on the die. Only their ratios matter. The <c>*Bank</c> lists are the capacitors switched in by
/// each bit of a parameter, least significant bit first.
/// </summary>
internal static class FilterCapacitances
{
    /// <summary>F1: unswitched input capacitor.</summary>
    public const double F1InputTop = 11247;

    /// <summary>F1: switched input capacitor.</summary>
    public const double F1InputBottom = 11797;

    /// <summary>F1: unswitched capacitor over the first op-amp.</summary>
    public const double F1FeedbackTop = 949;

    /// <summary>F1: switched capacitor over the first op-amp.</summary>
    public const double F1FeedbackBottom = 52067;

    /// <summary>F1: fixed part of the capacitor between the op-amps.</summary>
    public const double F1CouplingBase = 2280;

    /// <summary>F1: capacitor over the second op-amp.</summary>
    public const double F1Output = 166272;

    /// <summary>F2: unswitched input capacitor (voice path).</summary>
    public const double F2InputTop = 24840;

    /// <summary>F2: switched input capacitor.</summary>
    public const double F2InputBottom = 29154;

    /// <summary>F2: fixed part of the unswitched capacitor over the first op-amp (set by F2Q).</summary>
    public const double F2FeedbackTopBase = 829;

    /// <summary>F2: switched capacitor over the first op-amp.</summary>
    public const double F2FeedbackBottom = 38180;

    /// <summary>F2: fixed part of the capacitor between the op-amps (set by F2).</summary>
    public const double F2CouplingBase = 2352;

    /// <summary>F2: capacitor over the second op-amp.</summary>
    public const double F2Output = 34270;

    /// <summary>F3: switched input capacitor (there is no unswitched one).</summary>
    public const double F3InputBottom = 17594;

    /// <summary>F3: unswitched capacitor over the first op-amp.</summary>
    public const double F3FeedbackTop = 868;

    /// <summary>F3: switched capacitor over the first op-amp.</summary>
    public const double F3FeedbackBottom = 18828;

    /// <summary>F3: fixed part of the capacitor between the op-amps.</summary>
    public const double F3CouplingBase = 8480;

    /// <summary>F3: capacitor over the second op-amp.</summary>
    public const double F3Output = 50019;

    /// <summary>F4 (fixed): switched input capacitor.</summary>
    public const double F4InputBottom = 28810;

    /// <summary>F4 (fixed): unswitched capacitor over the first op-amp.</summary>
    public const double F4FeedbackTop = 1165;

    /// <summary>F4 (fixed): switched capacitor over the first op-amp.</summary>
    public const double F4FeedbackBottom = 21457;

    /// <summary>F4 (fixed): capacitor between the op-amps.</summary>
    public const double F4Coupling = 8558;

    /// <summary>F4 (fixed): capacitor over the second op-amp.</summary>
    public const double F4Output = 7289;

    /// <summary>Final low-pass: unswitched capacitor over the op-amp.</summary>
    public const double LowpassFeedbackTop = 1122;

    /// <summary>Final low-pass: switched capacitor over the op-amp.</summary>
    public const double LowpassFeedbackBottom = 23131;

    /// <summary>Noise shaper: capacitor over the first op-amp.</summary>
    public const double NoiseShaperFirstFeedback = 15500;

    /// <summary>Noise shaper: unswitched capacitor between the op-amps.</summary>
    public const double NoiseShaperCouplingTop = 14854;

    /// <summary>Noise shaper: switched capacitor between the op-amps.</summary>
    public const double NoiseShaperCouplingBottom = 8450;

    /// <summary>Noise shaper: capacitor over the second op-amp.</summary>
    public const double NoiseShaperSecondFeedback = 9523;

    /// <summary>Noise shaper: switched capacitor after the second op-amp.</summary>
    public const double NoiseShaperOutput = 14083;

    /// <summary>Gets the capacitors switched in by the four F1 bits.</summary>
    public static IReadOnlyList<double> F1Bank { get; } = [2546, 4973, 9861, 19724];

    /// <summary>Gets the capacitors switched in by the four F2Q bits.</summary>
    public static IReadOnlyList<double> F2QBank { get; } = [1390, 2965, 5875, 11297];

    /// <summary>Gets the capacitors switched in by the five F2 bits.</summary>
    public static IReadOnlyList<double> F2Bank { get; } = [833, 1663, 3164, 6327, 12654];

    /// <summary>Gets the capacitors switched in by the four F3 bits.</summary>
    public static IReadOnlyList<double> F3Bank { get; } = [2226, 4485, 9056, 18111];
}

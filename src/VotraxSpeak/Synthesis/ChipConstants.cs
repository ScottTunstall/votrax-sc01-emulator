// Ported from MAME src/devices/sound/votrax.cpp.
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

namespace VotraxSpeak.Synthesis;

/// <summary>
/// Register widths, masks and comparator values of the SC-01's digital control logic.
/// </summary>
internal static class ChipConstants
{
    /// <summary>Only the low six bits of a phoneme write are used.</summary>
    public const int PhoneCodeMask = 0x3F;

    /// <summary>Inflection is a two-bit input.</summary>
    public const int MaximumInflection = 3;

    /// <summary>Position of the inflection bits in the pitch comparator value.</summary>
    public const int InflectionShift = 5;

    /// <summary>The 8-bit interpolation registers wrap at this mask.</summary>
    public const int InterpolationRegisterMask = 0xFF;

    /// <summary>Each interpolation step moves one eighth of the way to the target.</summary>
    public const int InterpolationStepShift = 3;

    /// <summary>Targets are 4-bit ROM values scaled up into the 8-bit register.</summary>
    public const int InterpolationTargetShift = 1;

    /// <summary>Committed 4-bit filter values are the top nibble of an interpolation register.</summary>
    public const int FilterValueShift = 4;

    /// <summary>F2 is committed with five bits of precision rather than four.</summary>
    public const int F2FilterValueShift = 3;

    /// <summary>Period of the counter that produces the 625 Hz and 208 Hz update ticks.</summary>
    public const int UpdateCounterPeriod = 0x30;

    /// <summary>The 625 Hz tick fires when these bits of the update counter are clear.</summary>
    public const int Tick625Mask = 0x0F;

    /// <summary>The 208 Hz tick fires at this update counter value, between two 625 Hz ticks.</summary>
    public const int Tick208Phase = 0x28;

    /// <summary>The glottal closure counter stops at 7 (stored with two extra low bits).</summary>
    public const int ClosureCounterMaximum = 7 << 2;

    /// <summary>Glottal closure level is the closure counter shifted down by this amount.</summary>
    public const int ClosureLevelShift = 2;

    /// <summary>The pitch counter is eight bits wide.</summary>
    public const int PitchCounterMask = 0xFF;

    /// <summary>Base value of the pitch period comparator.</summary>
    public const int PitchComparatorBase = 0xE0;

    /// <summary>F1 is folded into the pitch comparator shifted left by this amount.</summary>
    public const int PitchF1Shift = 1;

    /// <summary>The pitch comparator acts two counts late.</summary>
    public const int PitchComparatorDelay = 2;

    /// <summary>Filters are committed when the masked pitch counter equals <see cref="FilterCommitPitchValue"/>.</summary>
    public const int FilterCommitPitchMask = 0xF9;

    /// <summary>See <see cref="FilterCommitPitchMask"/>: index 1 of the pitch wave.</summary>
    public const int FilterCommitPitchValue = 0x08;

    /// <summary>The 15-bit noise shift register holds this value when all ones.</summary>
    public const int NoiseRegisterAllOnes = 0x7FFF;

    /// <summary>Mask applied to the noise register after shifting left.</summary>
    public const int NoiseRegisterShiftMask = 0x7FFE;

    /// <summary>Bit positions of the two noise register taps that are XNORed for feedback.</summary>
    public const int NoiseTapHigh = 14;

    /// <summary>See <see cref="NoiseTapHigh"/>.</summary>
    public const int NoiseTapLow = 13;
}

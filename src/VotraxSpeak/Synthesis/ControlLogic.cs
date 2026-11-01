// Ported from MAME src/devices/sound/votrax.cpp (phone_commit, interpolate, chip_update, filters_commit).
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

using System;

namespace VotraxSpeak.Synthesis;

/// <summary>
/// The SC-01's digital control logic: phoneme timing, parameter interpolation, the pitch and
/// glottal closure counters, and the noise generator. It is updated at main/36.
/// </summary>
/// <remarks>
/// The fields named after parameters (<c>_noiseVolume</c> to <c>_f3</c>) are the 8-bit
/// interpolation registers, MAME's <c>m_cur_*</c>.
/// </remarks>
internal sealed class ControlLogic
{
    private PhonemeParameters _phoneme;
    private int _inflection;
    private int _noiseVolume;
    private int _noiseCutoff;
    private int _voiceVolume;
    private int _f1;
    private int _f2;
    private int _f2Q;
    private int _f3;

    private int _phoneTick;
    private int _ticks;
    private int _pitch;
    private int _closure;
    private int _updateCounter;
    private bool _isClosed;
    private int _noiseRegister;
    private bool _noiseBit;

    /// <summary>
    /// Initializes a new instance of the <see cref="ControlLogic"/> class in the sane state MAME
    /// resets to (the real chip powers up in a random state).
    /// </summary>
    /// <param name="initialPhoneme">The phoneme the chip starts on, normally <see cref="VotraxPhoneme.Stop"/>.</param>
    public ControlLogic(PhonemeParameters initialPhoneme)
    {
        _phoneme = initialPhoneme;
        CommitPhone(initialPhoneme);
        _isClosed = true;
    }

    /// <summary>Gets the parameters most recently committed to the analog filters.</summary>
    public FilterSettings FilterSettings { get; private set; }

    /// <summary>Gets the signals the analog path needs for the current sample.</summary>
    public AnalogInputs AnalogInputs => new(_pitch, _noiseBit, _closure, FilterSettings);

    private bool AreFormantsFrozen => _phoneme.IsPause && FilterSettings.IsSounding;

    /// <summary>
    /// Sets the two-bit inflection input, which raises the pitch as it increases.
    /// </summary>
    /// <param name="inflection">A value from 0 to 3.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="inflection"/> is not 0 to 3.</exception>
    public void SetInflection(int inflection)
    {
        if (inflection is < 0 or > ChipConstants.MaximumInflection)
        {
            throw new ArgumentOutOfRangeException(nameof(inflection), inflection, "Inflection must be between 0 and 3.");
        }

        _inflection = inflection;
    }

    /// <summary>
    /// Starts playing a phoneme. Only the phone tick counters reset; everything else is free-running.
    /// </summary>
    /// <param name="phoneme">The new phoneme's ROM parameters.</param>
    public void CommitPhone(PhonemeParameters phoneme)
    {
        _phoneme = phoneme;
        _phoneTick = 0;
        _ticks = 0;

        // That does not happen in the SC-01(A) ROM, but MAME covers it anyway.
        if (phoneme.VoiceDelay == 0)
        {
            _isClosed = phoneme.HasClosure;
        }
    }

    /// <summary>
    /// Advances the logic by one update (main/36).
    /// </summary>
    public void Update()
    {
        AdvancePhoneTicks();
        AdvanceUpdateCounter();

        // The 625 Hz and 208 Hz ticks are phased so the 208 Hz one falls exactly between two 625 Hz ones.
        if (_updateCounter == ChipConstants.Tick208Phase && !AreFormantsFrozen)
        {
            InterpolateFormants();
        }

        if ((_updateCounter & ChipConstants.Tick625Mask) == 0)
        {
            InterpolateAmplitudes();
        }

        AdvanceClosure();
        AdvancePitch();

        // Filters are updated at index 1 of the pitch wave, which means four times in a row.
        if ((_pitch & ChipConstants.FilterCommitPitchMask) == ChipConstants.FilterCommitPitchValue)
        {
            CommitFilters();
        }

        AdvanceNoise();
    }

    private static int Interpolate(int register, int target)
    {
        // One step adds one eighth of the distance between the current value and the target.
        int next = register - (register >> ChipConstants.InterpolationStepShift) + (target << ChipConstants.InterpolationTargetShift);
        return next & ChipConstants.InterpolationRegisterMask;
    }

    private void AdvancePhoneTicks()
    {
        // The tick counter stops at 16. The phone tick counter keeps running on the die, but its
        // comparator is disabled.
        if (_ticks == ChipTiming.TicksPerPhone)
        {
            return;
        }

        _phoneTick++;

        // The comparator is with duration << 2, but there is a one-update delay in the path.
        if (_phoneTick != ((_phoneme.Duration << 2) | ChipTiming.PhoneTickComparatorDelay))
        {
            return;
        }

        _phoneTick = 0;
        _ticks++;

        // The closure latches on the same delay that gates the noise amplitude.
        if (_ticks == _phoneme.VoiceDelay)
        {
            _isClosed = _phoneme.HasClosure;
        }
    }

    private void AdvanceUpdateCounter()
    {
        _updateCounter++;

        if (_updateCounter == ChipConstants.UpdateCounterPeriod)
        {
            _updateCounter = 0;
        }
    }

    private void InterpolateFormants()
    {
        // The noise routing (fc) moves with the noise amplitude instead; see InterpolateAmplitudes.
        _f1 = Interpolate(_f1, _phoneme.F1);
        _f2 = Interpolate(_f2, _phoneme.F2);
        _f2Q = Interpolate(_f2Q, _phoneme.F2Q);
        _f3 = Interpolate(_f3, _phoneme.F3);
    }

    private void InterpolateAmplitudes()
    {
        if (_ticks >= _phoneme.VoiceDelay)
        {
            _noiseVolume = Interpolate(_noiseVolume, _phoneme.NoiseVolume);

            // fc routes the noise, so it moves with the noise amplitude: fast and on the same delay.
            _noiseCutoff = Interpolate(_noiseCutoff, _phoneme.NoiseCutoff);
        }

        if (_ticks >= _phoneme.ClosureDelay)
        {
            _voiceVolume = Interpolate(_voiceVolume, _phoneme.VoiceVolume);
        }
    }

    private void AdvanceClosure()
    {
        // The closure level feeds the analog path directly; there is no pitch synchronization.
        if (!_isClosed && FilterSettings.IsSounding)
        {
            _closure = 0;
        }
        else if (_closure != ChipConstants.ClosureCounterMaximum)
        {
            _closure++;
        }
    }

    private void AdvancePitch()
    {
        // Equality comparison, so the inflection inputs can make it miss, but then it wraps.
        _pitch = (_pitch + 1) & ChipConstants.PitchCounterMask;

        int period = (ChipConstants.PitchComparatorBase
            ^ (_inflection << ChipConstants.InflectionShift)
            ^ (FilterSettings.F1 << ChipConstants.PitchF1Shift))
            + ChipConstants.PitchComparatorDelay;

        if (_pitch == period)
        {
            _pitch = 0;
        }
    }

    private void CommitFilters()
    {
        FilterSettings = new FilterSettings(
            _noiseVolume >> ChipConstants.FilterValueShift,
            _noiseCutoff >> ChipConstants.FilterValueShift,
            _voiceVolume >> ChipConstants.FilterValueShift,
            _f1 >> ChipConstants.FilterValueShift,
            _f2 >> ChipConstants.F2FilterValueShift,
            _f2Q >> ChipConstants.FilterValueShift,
            _f3 >> ChipConstants.FilterValueShift);
    }

    private void AdvanceNoise()
    {
        // 15-bit shift register with an XNOR of the top two bits as feedback.
        bool input = _noiseBit && _noiseRegister != ChipConstants.NoiseRegisterAllOnes;
        _noiseRegister = ((_noiseRegister << 1) & ChipConstants.NoiseRegisterShiftMask) | (input ? 1 : 0);
        _noiseBit = (((_noiseRegister >> ChipConstants.NoiseTapHigh) ^ (_noiseRegister >> ChipConstants.NoiseTapLow)) & 1) == 0;
    }
}

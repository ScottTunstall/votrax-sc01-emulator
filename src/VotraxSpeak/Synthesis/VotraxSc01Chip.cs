// Ported from MAME src/devices/sound/votrax.cpp (write, inflection_w, sound_stream_update, phone_tick).
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

using System;

namespace VotraxSpeak.Synthesis;

/// <summary>
/// A sample-accurate simulation of the Votrax SC-01 speech chip.
/// </summary>
/// <remarks>
/// Drive it as you would the real chip: <see cref="Write"/> a phoneme, then call
/// <see cref="Advance"/> once per output sample, reading <see cref="Output"/> each time, until
/// <see cref="IsRequestAsserted"/> (the A/R pin) goes high to ask for the next phoneme.
/// </remarks>
internal sealed class VotraxSc01Chip
{
    private readonly PhonemeRom _rom;
    private readonly ControlLogic _control;
    private readonly AnalogSignalPath _analog;

    private VotraxPhoneme _latchedPhoneme = VotraxPhoneme.Stop;
    private PhoneTimerEvent _pendingEvent = PhoneTimerEvent.None;
    private long _ticksUntilEvent;
    private long _sampleCount;

    /// <summary>
    /// Initializes a new instance of the <see cref="VotraxSc01Chip"/> class, idle on
    /// <see cref="VotraxPhoneme.Stop"/> with the request line asserted.
    /// </summary>
    /// <param name="rom">The chip's phoneme ROM.</param>
    /// <param name="mainClockHz">The main clock; the data sheet's nominal value is <see cref="ChipTiming.NominalClockHz"/>.</param>
    public VotraxSc01Chip(PhonemeRom rom, int mainClockHz)
    {
        if (mainClockHz <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(mainClockHz), mainClockHz, "The clock must be positive.");
        }

        _rom = rom;
        MainClockHz = mainClockHz;
        _control = new ControlLogic(rom.GetParameters(VotraxPhoneme.Stop));
        _analog = new AnalogSignalPath(new FilterDesigner(mainClockHz), _control.FilterSettings);
        IsRequestAsserted = true;
    }

    /// <summary>Gets the main clock, in hertz.</summary>
    public int MainClockHz { get; }

    /// <summary>Gets the output sample rate (main/18), in hertz.</summary>
    public double SampleRateHz => MainClockHz / (double)ChipTiming.MainClockTicksPerSample;

    /// <summary>Gets a value indicating whether the A/R pin is high, i.e. the chip wants the next phoneme.</summary>
    public bool IsRequestAsserted { get; private set; }

    /// <summary>Gets the most recent output sample.</summary>
    public double Output => _analog.Output;

    /// <summary>
    /// Writes a phoneme to the chip. The request line drops, the phoneme is committed shortly
    /// afterwards, and the request line rises again when it has finished playing.
    /// </summary>
    /// <param name="phoneme">The phoneme to play.</param>
    public void Write(VotraxPhoneme phoneme)
    {
        _latchedPhoneme = phoneme;
        IsRequestAsserted = false;

        // Committing takes about 0.1 ms; an in-flight commit is not restarted, but a commit
        // overrides a pending end-of-phone, as the strobe does on the real chip.
        if (_pendingEvent != PhoneTimerEvent.CommitPhone)
        {
            _pendingEvent = PhoneTimerEvent.CommitPhone;
            _ticksUntilEvent = ChipTiming.PhoneCommitDelayTicks;
        }
    }

    /// <summary>
    /// Sets the two-bit inflection input; higher values raise the pitch.
    /// </summary>
    /// <param name="inflection">A value from 0 to 3.</param>
    public void SetInflection(int inflection) => _control.SetInflection(inflection);

    /// <summary>
    /// Advances the chip by one output sample period.
    /// </summary>
    public void Advance()
    {
        _sampleCount++;

        if (_sampleCount % ChipTiming.SamplesPerChipUpdate == 1)
        {
            _control.Update();
        }

        _analog.Advance(_control.AnalogInputs);
        AdvanceTimer();
    }

    private static long EndOfPhoneDelayTicks(int duration) =>
        (ChipTiming.TicksPerPhone
            * ((duration * ChipTiming.ChipUpdatesPerDurationUnit) + ChipTiming.PhoneTickComparatorDelay)
            * ChipTiming.MainClockTicksPerCapacitorCycle)
        + ChipTiming.EndOfPhoneExtraTicks;

    private void AdvanceTimer()
    {
        if (_pendingEvent == PhoneTimerEvent.None)
        {
            return;
        }

        _ticksUntilEvent -= ChipTiming.MainClockTicksPerSample;

        while (_pendingEvent != PhoneTimerEvent.None && _ticksUntilEvent <= 0)
        {
            FireTimer();
        }
    }

    private void FireTimer()
    {
        if (_pendingEvent == PhoneTimerEvent.CommitPhone)
        {
            PhonemeParameters phoneme = _rom.GetParameters(_latchedPhoneme);
            _control.CommitPhone(phoneme);
            _pendingEvent = PhoneTimerEvent.EndOfPhone;
            _ticksUntilEvent += EndOfPhoneDelayTicks(phoneme.Duration);
        }
        else
        {
            _pendingEvent = PhoneTimerEvent.None;
            IsRequestAsserted = true;
        }
    }
}

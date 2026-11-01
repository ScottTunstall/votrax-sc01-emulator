// Ported from MAME src/devices/sound/votrax.cpp (filters_commit, analog_calc).
// license:BSD-3-Clause
// copyright-holders:Olivier Galibert

namespace VotraxSpeak.Synthesis;

/// <summary>
/// The SC-01's analog signal path: a glottal pulse and a white noise source, shaped by the
/// formant filters and mixed into one output, simulated at main/18.
/// </summary>
internal sealed class AnalogSignalPath
{
    private readonly FilterDesigner _designer;
    private readonly FilterCoefficients _f4;
    private readonly FilterCoefficients _finalLowpass;
    private readonly FilterCoefficients _noiseShaper;

    private readonly SignalHistory _voice1 = new(4);
    private readonly SignalHistory _voice2 = new(4);
    private readonly SignalHistory _voice3 = new(4);
    private readonly SignalHistory _noise1 = new(3);
    private readonly SignalHistory _noise2 = new(3);
    private readonly SignalHistory _noise3 = new(2);
    private readonly SignalHistory _noise4 = new(2);
    private readonly SignalHistory _mixed1 = new(4);
    private readonly SignalHistory _mixed2 = new(4);
    private readonly SignalHistory _mixed3 = new(4);
    private readonly SignalHistory _mixed4 = new(4);
    private readonly SignalHistory _mixed5 = new(2);
    private readonly SignalHistory _mixed6 = new(2);

    private FilterSettings _designedFor;
    private FilterCoefficients _f1;
    private FilterCoefficients _f2Voice;
    private FilterCoefficients _f2Noise;
    private FilterCoefficients _f3;

    /// <summary>
    /// Initializes a new instance of the <see cref="AnalogSignalPath"/> class with silent histories.
    /// </summary>
    /// <param name="designer">Designs the filters for the chip's clock.</param>
    /// <param name="initialSettings">The filter parameters to start with.</param>
    public AnalogSignalPath(FilterDesigner designer, FilterSettings initialSettings)
    {
        _designer = designer;
        _designedFor = initialSettings;

        _f1 = DesignF1(initialSettings);
        _f2Voice = DesignF2Voice(initialSettings);
        _f2Noise = DesignF2Noise(initialSettings);
        _f3 = DesignF3(initialSettings);

        _f4 = designer.DesignStandard(
            0,
            FilterCapacitances.F4InputBottom,
            FilterCapacitances.F4FeedbackTop,
            FilterCapacitances.F4FeedbackBottom,
            FilterCapacitances.F4Coupling,
            FilterCapacitances.F4Output);

        _finalLowpass = designer.DesignLowpass(FilterCapacitances.LowpassFeedbackTop, FilterCapacitances.LowpassFeedbackBottom);

        _noiseShaper = designer.DesignNoiseShaper(
            FilterCapacitances.NoiseShaperFirstFeedback,
            FilterCapacitances.NoiseShaperCouplingTop,
            FilterCapacitances.NoiseShaperCouplingBottom,
            FilterCapacitances.NoiseShaperSecondFeedback,
            FilterCapacitances.NoiseShaperOutput);
    }

    /// <summary>Gets the most recently computed output sample.</summary>
    public double Output { get; private set; }

    /// <summary>
    /// Computes the next output sample.
    /// </summary>
    /// <param name="inputs">The digital signals for this sample.</param>
    public void Advance(AnalogInputs inputs)
    {
        RedesignChangedFilters(inputs.Filters);
        RunSignalChain(inputs);
    }

    private void RedesignChangedFilters(FilterSettings settings)
    {
        if (settings.F1 != _designedFor.F1)
        {
            _f1 = DesignF1(settings);
        }

        if (settings.F2 != _designedFor.F2 || settings.F2Q != _designedFor.F2Q)
        {
            _f2Voice = DesignF2Voice(settings);
            _f2Noise = DesignF2Noise(settings);
        }

        if (settings.F3 != _designedFor.F3)
        {
            _f3 = DesignF3(settings);
        }

        _designedFor = settings;
    }

    private void RunSignalChain(AnalogInputs inputs)
    {
        FilterSettings filters = inputs.Filters;

        // Voice-only path.
        // 1. Pick up the pitch wave.
        int glottalStep = inputs.Pitch >> AnalogConstants.GlottalStepShift;
        double voice = glottalStep < GlottalWave.Levels.Count ? GlottalWave.Levels[glottalStep] : 0;

        // 2. Multiply by the initial amplifier. It is linear on the die, even if it is not in the patent.
        voice = voice * filters.VoiceVolume / AnalogConstants.ParameterFullScale;
        _voice1.Push(voice);

        // 3. Apply the F1 filter.
        voice = _f1.Apply(_voice1, _voice2);
        _voice2.Push(voice);

        // 4. Apply the F2 filter, voice half.
        voice = _f2Voice.Apply(_voice2, _voice3);
        _voice3.Push(voice);

        // Noise-only path.
        // 5. Pick up the noise pitch. Amplitude is linear.
        bool noiseHigh = (inputs.Pitch & AnalogConstants.NoisePitchGateBit) != 0 && inputs.Noise;
        double noise = AnalogConstants.NoiseAmplitude * (noiseHigh ? 1 : -1);
        noise = noise * filters.NoiseVolume / AnalogConstants.ParameterFullScale;
        _noise1.Push(noise);

        // 6. Apply the noise shaper.
        noise = _noiseShaper.Apply(_noise1, _noise2);
        _noise2.Push(noise);

        // 7. Scale with the F2 noise input.
        double injectedNoise = noise * filters.NoiseCutoff / AnalogConstants.ParameterFullScale;
        _noise3.Push(injectedNoise);

        // 8. Apply the F2 filter, noise half.
        injectedNoise = _f2Noise.Apply(_noise3, _noise4);
        _noise4.Push(injectedNoise);

        // Mixed path.
        // 9. Add the F2 voice and F2 noise outputs.
        double mixed = voice + injectedNoise;
        _mixed1.Push(mixed);

        // 10. Apply the F3 filter.
        mixed = _f3.Apply(_mixed1, _mixed2);
        _mixed2.Push(mixed);

        // 11. Second noise insertion, complementary to the F2 injection in step 7.
        mixed += noise * (AnalogConstants.ParameterAllBits ^ filters.NoiseCutoff) / AnalogConstants.ParameterFullScale;
        _mixed3.Push(mixed);

        // 12. Apply the F4 filter.
        mixed = _f4.Apply(_mixed3, _mixed4);
        _mixed4.Push(mixed);

        // 13. Apply the glottal closure amplitude, also linear.
        int closureLevel = inputs.Closure >> ChipConstants.ClosureLevelShift;
        mixed = mixed * (AnalogConstants.ClosureAllBits ^ closureLevel) / AnalogConstants.ClosureFullScale;
        _mixed5.Push(mixed);

        // 14. Apply the final fixed filter.
        mixed = _finalLowpass.Apply(_mixed5, _mixed6);
        _mixed6.Push(mixed);

        Output = mixed * AnalogConstants.OutputGain;
    }

    private FilterCoefficients DesignF1(FilterSettings settings) =>
        _designer.DesignStandard(
            FilterCapacitances.F1InputTop,
            FilterCapacitances.F1InputBottom,
            FilterCapacitances.F1FeedbackTop,
            FilterCapacitances.F1FeedbackBottom,
            FilterCapacitances.F1CouplingBase + CapacitorBank.Total(settings.F1, FilterCapacitances.F1Bank),
            FilterCapacitances.F1Output);

    private FilterCoefficients DesignF2Voice(FilterSettings settings) =>
        _designer.DesignStandard(
            FilterCapacitances.F2InputTop,
            FilterCapacitances.F2InputBottom,
            FilterCapacitances.F2FeedbackTopBase + CapacitorBank.Total(settings.F2Q, FilterCapacitances.F2QBank),
            FilterCapacitances.F2FeedbackBottom,
            FilterCapacitances.F2CouplingBase + CapacitorBank.Total(settings.F2, FilterCapacitances.F2Bank),
            FilterCapacitances.F2Output);

    private FilterCoefficients DesignF2Noise(FilterSettings settings) =>
        _designer.DesignInjection(
            FilterCapacitances.F2InputBottom,
            FilterCapacitances.F2FeedbackTopBase + CapacitorBank.Total(settings.F2Q, FilterCapacitances.F2QBank),
            FilterCapacitances.F2FeedbackBottom,
            FilterCapacitances.F2CouplingBase + CapacitorBank.Total(settings.F2, FilterCapacitances.F2Bank));

    private FilterCoefficients DesignF3(FilterSettings settings) =>
        _designer.DesignStandard(
            0,
            FilterCapacitances.F3InputBottom,
            FilterCapacitances.F3FeedbackTop,
            FilterCapacitances.F3FeedbackBottom,
            FilterCapacitances.F3CouplingBase + CapacitorBank.Total(settings.F3, FilterCapacitances.F3Bank),
            FilterCapacitances.F3Output);
}

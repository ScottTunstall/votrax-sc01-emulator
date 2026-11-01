using VotraxSpeak.Synthesis;

namespace VotraxSpeak.Phonetics;

/// <summary>
/// One phoneme to send to the chip, with the inflection to present with it.
/// </summary>
/// <param name="Phoneme">The phoneme.</param>
/// <param name="Inflection">
/// The two-bit inflection (0 to 3) to present with the phoneme, or <see langword="null"/> to use the
/// voice's pitch setting.
/// </param>
internal sealed record PhonemeCue(VotraxPhoneme Phoneme, int? Inflection);

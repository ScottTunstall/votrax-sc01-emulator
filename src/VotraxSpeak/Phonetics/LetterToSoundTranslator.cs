using System.Collections.Generic;
using System.Linq;

namespace VotraxSpeak.Phonetics;

/// <summary>
/// Pronounces normalized English text by applying letter-to-sound rules from left to right.
/// </summary>
internal sealed class LetterToSoundTranslator
{
    private readonly ILookup<char, LetterToSoundRule> _rulesByFirstCharacter;

    /// <summary>
    /// Initializes a new instance of the <see cref="LetterToSoundTranslator"/> class.
    /// </summary>
    /// <param name="rules">The rules, in priority order.</param>
    public LetterToSoundTranslator(IEnumerable<LetterToSoundRule> rules)
    {
        _rulesByFirstCharacter = rules.ToLookup(rule => rule.Match[0]);
    }

    /// <summary>
    /// Returns the pronunciation of <paramref name="normalizedText"/>.
    /// </summary>
    /// <param name="normalizedText">Upper-case text, as produced by <see cref="TextNormalizer"/>.</param>
    /// <returns>The phonemes, with pauses between words and at punctuation.</returns>
    /// <remarks>Characters that no rule covers are skipped.</remarks>
    public IReadOnlyList<ArpabetPhoneme> Translate(string normalizedText)
    {
        string text = " " + normalizedText + " ";
        var phonemes = new List<ArpabetPhoneme>();

        // The padding spaces are context only; they are never pronounced.
        int position = 1;

        while (position < text.Length - 1)
        {
            LetterToSoundRule? rule = FindRule(text, position);

            if (rule is null)
            {
                position++;
                continue;
            }

            phonemes.AddRange(rule.Phonemes);
            position += rule.Match.Length;
        }

        return phonemes;
    }

    private LetterToSoundRule? FindRule(string text, int position) =>
        _rulesByFirstCharacter[text[position]].FirstOrDefault(rule => RuleContextMatcher.Applies(text, position, rule));
}

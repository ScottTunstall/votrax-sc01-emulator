using System;
using System.Collections.Generic;

namespace VotraxSpeak.Phonetics;

/// <summary>
/// One NRL letter-to-sound rule: when <see cref="Match"/> appears in the text between
/// <see cref="LeftContext"/> and <see cref="RightContext"/>, it is pronounced as
/// <see cref="Phonemes"/>.
/// </summary>
internal sealed class LetterToSoundRule
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LetterToSoundRule"/> class.
    /// </summary>
    /// <param name="leftContext">The pattern that must precede the match (see <see cref="ContextSymbol"/>).</param>
    /// <param name="match">The upper-case text the rule pronounces.</param>
    /// <param name="rightContext">The pattern that must follow the match.</param>
    /// <param name="phonemes">The pronunciation; empty for silent letters.</param>
    public LetterToSoundRule(string leftContext, string match, string rightContext, params ArpabetPhoneme[] phonemes)
    {
        if (string.IsNullOrEmpty(match))
        {
            throw new ArgumentException("A rule must match at least one character.", nameof(match));
        }

        LeftContext = leftContext;
        Match = match;
        RightContext = rightContext;
        Phonemes = phonemes;
    }

    /// <summary>Gets the pattern that must precede the match.</summary>
    public string LeftContext { get; }

    /// <summary>Gets the text the rule pronounces.</summary>
    public string Match { get; }

    /// <summary>Gets the pattern that must follow the match.</summary>
    public string RightContext { get; }

    /// <summary>Gets the pronunciation of <see cref="Match"/>.</summary>
    public IReadOnlyList<ArpabetPhoneme> Phonemes { get; }
}

namespace VotraxSpeak.Phonetics;

/// <summary>
/// Decides whether a letter-to-sound rule applies at a position in the text.
/// </summary>
/// <remarks>
/// The text is upper case and padded with a space at each end. Positions outside the text read
/// as spaces, so patterns can always look past the ends of a word.
/// </remarks>
internal static class RuleContextMatcher
{
    /// <summary>
    /// Returns whether <paramref name="rule"/> applies at <paramref name="position"/>.
    /// </summary>
    /// <param name="text">The padded upper-case text.</param>
    /// <param name="position">Where the rule's match would start.</param>
    /// <param name="rule">The rule to test.</param>
    /// <returns><see langword="true"/> if the match text and both contexts fit.</returns>
    public static bool Applies(string text, int position, LetterToSoundRule rule) =>
        MatchesText(text, position, rule.Match)
        && MatchesLeft(text, position - 1, rule.LeftContext)
        && MatchesRight(text, position + rule.Match.Length, rule.RightContext);

    private static bool MatchesText(string text, int position, string match) =>
        position + match.Length <= text.Length
        && string.CompareOrdinal(text, position, match, 0, match.Length) == 0;

    private static bool MatchesLeft(string text, int position, string pattern)
    {
        int? cursor = position;

        // Left contexts are read right to left, starting just before the match.
        for (int i = pattern.Length - 1; i >= 0 && cursor is not null; i--)
        {
            cursor = ConsumeLeft(text, cursor.Value, pattern[i]);
        }

        return cursor is not null;
    }

    private static bool MatchesRight(string text, int position, string pattern)
    {
        int? cursor = position;

        for (int i = 0; i < pattern.Length && cursor is not null; i++)
        {
            cursor = ConsumeRight(text, cursor.Value, pattern[i]);
        }

        return cursor is not null;
    }

    /// <summary>Returns the position left of what <paramref name="symbol"/> consumed, or null if it does not match.</summary>
    /// <param name="text">The padded text.</param>
    /// <param name="position">The position the symbol is matched against.</param>
    /// <param name="symbol">One character of a left-context pattern.</param>
    /// <returns>The next position to match, or null.</returns>
    private static int? ConsumeLeft(string text, int position, char symbol)
    {
        char current = CharacterAt(text, position);

        switch (symbol)
        {
            case ContextSymbol.OneOrMoreVowels:
                if (!LetterClassifier.IsVowel(current))
                {
                    return null;
                }

                int start = position - 1;

                while (LetterClassifier.IsVowel(CharacterAt(text, start)))
                {
                    start--;
                }

                return start;

            case ContextSymbol.ZeroOrMoreConsonants:
                int next = position;

                while (LetterClassifier.IsConsonant(CharacterAt(text, next)))
                {
                    next--;
                }

                return next;

            case ContextSymbol.Suffix:
                return null;

            default:
                return MatchesSingle(current, symbol) ? position - 1 : null;
        }
    }

    /// <summary>Returns the position right of what <paramref name="symbol"/> consumed, or null if it does not match.</summary>
    /// <param name="text">The padded text.</param>
    /// <param name="position">The position the symbol is matched against.</param>
    /// <param name="symbol">One character of a right-context pattern.</param>
    /// <returns>The next position to match, or null.</returns>
    private static int? ConsumeRight(string text, int position, char symbol)
    {
        char current = CharacterAt(text, position);

        switch (symbol)
        {
            case ContextSymbol.OneOrMoreVowels:
                if (!LetterClassifier.IsVowel(current))
                {
                    return null;
                }

                int end = position + 1;

                while (LetterClassifier.IsVowel(CharacterAt(text, end)))
                {
                    end++;
                }

                return end;

            case ContextSymbol.ZeroOrMoreConsonants:
                int next = position;

                while (LetterClassifier.IsConsonant(CharacterAt(text, next)))
                {
                    next++;
                }

                return next;

            case ContextSymbol.Suffix:
                return ConsumeSuffix(text, position);

            default:
                return MatchesSingle(current, symbol) ? position + 1 : null;
        }
    }

    private static int? ConsumeSuffix(string text, int position)
    {
        switch (CharacterAt(text, position))
        {
            case 'E':
                char afterE = CharacterAt(text, position + 1);

                if (afterE == 'L')
                {
                    // ELY; a lone EL only consumes the E.
                    return CharacterAt(text, position + 2) == 'Y' ? position + 3 : position + 1;
                }

                return afterE is 'R' or 'S' or 'D' ? position + 2 : position + 1;

            case 'I':
                bool isIng = CharacterAt(text, position + 1) == 'N' && CharacterAt(text, position + 2) == 'G';
                return isIng ? position + 3 : null;

            default:
                return null;
        }
    }

    private static bool MatchesSingle(char current, char symbol) =>
        symbol switch
        {
            ContextSymbol.WordBoundary => !LetterClassifier.IsLetter(current),
            ContextSymbol.OneConsonant => LetterClassifier.IsConsonant(current),
            ContextSymbol.VoicedConsonant => LetterClassifier.IsVoicedConsonant(current),
            ContextSymbol.FrontVowel => LetterClassifier.IsFrontVowel(current),
            _ => current == symbol,
        };

    private static char CharacterAt(string text, int position) =>
        position >= 0 && position < text.Length ? text[position] : ContextSymbol.WordBoundary;
}

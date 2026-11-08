using System.Collections.Generic;

namespace VotraxSpeak.Cli;

/// <summary>
/// Steps through command-line arguments one at a time.
/// </summary>
internal sealed class ArgumentCursor
{
    private readonly IReadOnlyList<string> _arguments;
    private int _index;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentCursor"/> class at the first argument.
    /// </summary>
    /// <param name="arguments">The arguments to step through.</param>
    public ArgumentCursor(IReadOnlyList<string> arguments)
    {
        _arguments = arguments;
    }

    /// <summary>Gets a value indicating whether every argument has been consumed.</summary>
    public bool IsAtEnd => _index >= _arguments.Count;

    /// <summary>Gets the current argument. Only valid while <see cref="IsAtEnd"/> is false.</summary>
    public string Current => _arguments[_index];

    /// <summary>Gets the argument after the current one, or <see langword="null"/> if the current one is the last.</summary>
    public string? Next => _index + 1 < _arguments.Count ? _arguments[_index + 1] : null;

    /// <summary>
    /// Moves to the next argument.
    /// </summary>
    public void MoveNext() => _index++;
}

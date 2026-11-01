using System;
using System.Runtime.InteropServices;

namespace VotraxSpeak.Audio;

/// <summary>
/// Win32 functions used for audio playback.
/// </summary>
internal static class NativeMethods
{
    /// <summary>
    /// Plays a sound (winmm.dll <c>PlaySoundW</c>).
    /// </summary>
    /// <param name="sound">With <see cref="PlaySoundOptions.Memory"/>, a complete WAV file image.</param>
    /// <param name="module">Unused; pass <see cref="IntPtr.Zero"/>.</param>
    /// <param name="options">How to play the sound.</param>
    /// <returns><see langword="true"/> if the sound was played.</returns>
    [DllImport("winmm.dll", EntryPoint = "PlaySoundW", ExactSpelling = true, SetLastError = true)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool PlaySound(byte[] sound, IntPtr module, PlaySoundOptions options);
}

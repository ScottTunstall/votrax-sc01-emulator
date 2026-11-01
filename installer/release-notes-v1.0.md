## Votrax SC01 Emulator 1.0

First release. A Windows console app that speaks English text through a sample-accurate emulation of
the **Votrax SC-01** speech chip, ported to C# from MAME's `votrax.cpp`.

### What's in 1.0

- **SC-01 / SC-01-A chip emulation**, ported from MAME: phoneme timing, parameter interpolation, glottal
  pulse and noise sources, and the switched-capacitor formant filters.
- **English text to speech.** Text is normalised (numbers, decimals, thousands separators and symbols are
  read out) and converted to SC-01 phonemes with the NRL letter-to-sound rules.
- **Speed and pitch controls:** `-s/--speed` (25-400%) sets the chip clock; `-p/--pitch` (0-3) sets the
  chip's inflection input.
- **`-f/--file <path>`** saves the speech as a WAV file instead of playing it.
- `-r/--rom <path>` selects a phoneme ROM; `-h/--help`; `--` to speak text starting with a dash.
- Clear exit codes (0 ok, 2 bad arguments, 3 ROM problem, 4 output failed).
- Builds for **.NET Framework 4.8** and **.NET 10**; 66 unit tests on each.

### Downloads

| Installer | For |
|---|---|
| `VotraxSpeakSetup-net10.exe` | 64-bit Windows. Installs the .NET 10 runtime if it is missing. |
| `VotraxSpeakSetup-net48.exe` | Any Windows with .NET Framework 4.8 (built into Windows 10 1903 and later). |

Both install to `Program Files\VotraxSpeak`, can add it to the PATH, and uninstall from "Installed apps".

### You need the phoneme ROM

The SC-01's ROM is copyrighted firmware and is **not** included. Obtain `votrsc01a.zip` (or `votrsc01.zip`)
from the MAME ROM sets available online, extract `sc01a.bin` (or `sc01.bin`), and pick it on the
installer's ROM page, or pass it silently with `/ROM=<path>`. See the
[README](https://github.com/ScottTunstall/votrax-sc01-emulator#the-phoneme-rom).

### Notes

- The installers are not code-signed, so Windows SmartScreen may warn on first run.
- Audio playback uses the Windows audio device; `-f` writes WAV files without it.

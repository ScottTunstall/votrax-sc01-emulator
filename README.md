# Votrax SC01 Emulator

A Windows console app that speaks English text through a sample-accurate emulation of the
**Votrax SC-01** speech chip, the voice of Q\*bert, Gorf, Wizard of Wor and many 1980s speech boards.

The chip emulation is a C# port of MAME's
[`votrax.cpp`](https://github.com/mamedev/mame/blob/master/src/devices/sound/votrax.cpp) by Olivier
Galibert. On top of it is an English text-to-phoneme converter based on the US Naval Research Lab's
letter-to-sound rules. The app builds for both **.NET Framework 4.8** and **.NET 10**.

```console
VotraxSpeak "I am a Votrax speech synthesizer."
```

---

## Contents

- [Quick setup](#quick-setup)
- [The phoneme ROM](#the-phoneme-rom)
- [Usage](#usage)
- [Speed and pitch](#speed-and-pitch)
- [Building](#building)
- [How it works](#how-it-works)
- [Licensing](#licensing)

---

## Quick setup

No programming tools needed. You download an installer, then point it at one ROM file.

1. **Download an installer** from the
   [latest release](https://github.com/ScottTunstall/votrax-sc01-emulator/releases/latest):

   | Installer                    | Choose it if |
   |------------------------------|--------------|
   | `VotraxSpeakSetup-net10.exe` | You have 64-bit Windows 10 or 11 (the usual case). It installs the .NET 10 runtime for you if it is missing. |
   | `VotraxSpeakSetup-net48.exe` | You have an older or 32-bit Windows with .NET Framework 4.8 (built into Windows 10 from the May 2019 update). |

   Both do the same thing. Windows SmartScreen may warn that the installer is unrecognised, because it
   is not code-signed; choose **More info**, then **Run anyway**.

2. **Get the speech chip's ROM.** It is copyrighted, so it can't be bundled. Download `votrsc01a.zip`
   from a MAME ROM set (see [The phoneme ROM](#the-phoneme-rom)) and extract `sc01a.bin` from it.

3. **Run the installer.** On the *Votrax SC-01 phoneme ROM* page, choose your `sc01a.bin`. Leave
   *Add VotraxSpeak to the PATH* ticked so the command works from any Command Prompt.

4. **Open a new Command Prompt and speak:**

   ```console
   VotraxSpeak "Hello, world!"
   ```

   To save the speech as a WAV file instead of playing it: `VotraxSpeak -f hello.wav "Hello, world!"`.
   See [Usage](#usage) for speed and pitch.

To remove it, uninstall *VotraxSpeak* from Windows Settings > Apps. More detail:
[`installer/README.md`](installer/README.md).

## The phoneme ROM

The SC-01 keeps its phoneme parameters in an internal 512-byte ROM. That ROM is copyrighted firmware, so
it is **not** in this repository or in the installers. The dumps are part of MAME's ROM sets, which
are available online (search for "MAME ROMs", or for the zip names below). Make sure you have a
MAME-compatible set and are entitled to use it. The two ROMs are tiny, standalone zips in that set:

| Zip to download   | File inside it | Chip revision | Size      | Needed? |
|-------------------|----------------|---------------|-----------|---------|
| `votrsc01a.zip`   | `sc01a.bin`    | SC-01-A       | 512 bytes | One of the two is enough; the SC-01-A is the default and the one most arcade boards used |
| `votrsc01.zip`    | `sc01.bin`     | SC-01         | 512 bytes | Optional; the original chip, selectable with `--rom` |

You do not need the arcade game sets, `votrax.zip`, or any other zip. Extract the `.bin` from the zip.
Either file works. The app checks the file's hash and refuses anything
that is not one of these dumps. [`roms/README.md`](roms/README.md) lists the hashes.

VotraxSpeak uses `--rom <path>` if given. Otherwise it looks for `sc01a.bin`, then `sc01.bin`, next to
the program and then in the current directory.

## Usage

```
VotraxSpeak [options] [--] <text>
```

| Option                 | Meaning |
|------------------------|---------|
| `<text>`               | **Required.** The text to speak. Quote it, or pass several words, which are joined with spaces. |
| `-f, --file <path>`    | Write the speech to a WAV file instead of playing it. |
| `-s, --speed <25-400>` | Chip clock as a percentage of the nominal 720 kHz. Default `100`. |
| `-p, --pitch <0-3>`    | Inflection level; higher is higher pitched. Default `0`. |
| `-r, --rom <path>`     | The phoneme ROM to use. |
| `-h, --help`           | Show help. |
| `--`                   | Treat everything after this as text, even if it starts with `-`. |

If an option is given twice, the last value wins.

### Examples

```console
:: Speak through the default audio device
VotraxSpeak "I am a Votrax speech synthesizer."

:: Several words without quotes work too
VotraxSpeak Hello there

:: Faster and higher
VotraxSpeak -s 130 -p 2 "Danger, danger!"

:: Save to a WAV file instead of playing
VotraxSpeak -f hello.wav "Hello, world!"

:: Use a ROM somewhere else
VotraxSpeak --rom D:\mame\roms\sc01.bin "Using the original SC-01"

:: Text that starts with a dash
VotraxSpeak -- "-5 degrees outside"
```

Numbers, decimals and thousands separators are read out ("1,905.5" becomes "one thousand nine hundred
five point five"), as are `&`, `%`, `+`, `=` and `@`. Commas and full stops make longer pauses.

### Exit codes

| Code | Meaning |
|------|---------|
| `0`  | Spoken, or written to the file. |
| `2`  | Invalid command line. |
| `3`  | Phoneme ROM missing, unreadable or not a known dump. |
| `4`  | The speech could not be played or the file could not be written. |

Playback uses the Windows audio device. `-f` works anywhere .NET runs.

## Speed and pitch

The real chip has two voice controls, and the switches drive them directly:

- **Speed** is the chip's master clock. Phoneme timing, filter frequencies and pitch all come from that
  clock, so a faster clock makes the voice faster *and* higher. Real arcade boards sound the same way
  when their clock is changed.
- **Pitch** is the chip's two-bit inflection input. It moves the pitch in four steps and leaves the
  timing alone.

## Building

Only needed if you want to change the code; everyone else should use the [installers](#quick-setup).

Needs the .NET 10 SDK. Building the .NET Framework 4.8 target also needs Windows. Building the
installers needs [Inno Setup 6](https://jrsoftware.org/isinfo.php).

To run a build from source, put your ROM in [`roms/`](roms/README.md) first; `dotnet build` copies it
next to the executable.

```console
dotnet build              :: both target frameworks
dotnet test               :: 66 tests on each framework
installer\build.ps1       :: VotraxSpeakSetup-net48.exe and VotraxSpeakSetup-net10.exe
```

All compiler, .NET analyzer (`AnalysisLevel` `latest-all`), IDE code-style and
[Roslynator](https://github.com/dotnet/roslynator) diagnostics are build errors. The house rules
(Clean Code and SOLID, command/query separation, one type per file, enums and constants in their own
files, principle of least surprise) are recorded at the top of [`.editorconfig`](.editorconfig).

The tests build synthetic ROM images, so they run without the real firmware.

## How it works

```
text ─► TextNormalizer ─► LetterToSoundTranslator ─► ArpabetToVotraxMapper ─► UtteranceShaper
        (numbers, case,    (NRL Report 7948 rules)    (rule phonemes →         (tidy pauses)
         symbols)                                      SC-01 phonemes)
                                                                │
     IAudioOutput ◄── PeakNormalizer ◄── VotraxSpeechRenderer ◄─┘
  (speakers / WAV)                       (drives VotraxSc01Chip like a host CPU:
                                          write phoneme, wait for A/R, repeat)
```

| Folder                    | Contents |
|---------------------------|----------|
| `src/VotraxSpeak/Synthesis` | The MAME port. `ControlLogic` holds the digital state (phoneme timing, interpolation, pitch and noise). `AnalogSignalPath` holds the switched-capacitor filters. `VotraxSc01Chip` is the pin-level facade. |
| `src/VotraxSpeak/Phonetics` | English text to SC-01 phonemes. |
| `src/VotraxSpeak/Speech`    | The rendering pipeline and voice settings. |
| `src/VotraxSpeak/Audio`     | Audio clips, WAV encoding, speaker and file outputs. |
| `src/VotraxSpeak/Cli`       | Command-line parsing and help. |
| `tests/VotraxSpeak.Tests`   | xUnit tests. |
| `installer`                 | Inno Setup installers for both frameworks. |
| `roms`                      | Where your ROM dump goes (not committed). |

## Licensing

- The emulation in `src/VotraxSpeak/Synthesis` is derived from MAME's `votrax.cpp`, BSD-3-Clause,
  copyright Olivier Galibert.
- The letter-to-sound rules come from NRL Report 7948 (1976), a US Government work in the public domain.
- The Votrax SC-01 ROM is not included.

See [`THIRD-PARTY-NOTICES.md`](THIRD-PARTY-NOTICES.md) for the full notices.

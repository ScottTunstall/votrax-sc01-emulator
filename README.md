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

- [Getting started](#getting-started)
- [The phoneme ROM](#the-phoneme-rom)
- [Usage](#usage)
- [Speed and pitch](#speed-and-pitch)
- [Building from source](#building-from-source)
- [How it works](#how-it-works)
- [Licensing](#licensing)

---

## Getting started

1. **Get the ROM.** The emulator needs the chip's internal phoneme ROM, which is not included (see
   [The phoneme ROM](#the-phoneme-rom)).
2. **Install or build VotraxSpeak.**
   - *With an installer:* build them with `installer\build.ps1`, then run
     `VotraxSpeakSetup-net10.exe` (needs 64-bit Windows; installs the .NET 10 runtime if it is missing) or
     `VotraxSpeakSetup-net48.exe` (any Windows with .NET Framework 4.8). Setup asks for your ROM, copies
     it into the install folder, and can add VotraxSpeak to the PATH. See
     [`installer/README.md`](installer/README.md).
   - *From source:* put the ROM in [`roms/`](roms/README.md) and run `dotnet build`; the ROM is copied
     next to the executable.
3. **Speak:**

   ```console
   VotraxSpeak "Hello, world!"
   ```

## The phoneme ROM

The SC-01 keeps its phoneme parameters in an internal 512-byte ROM. That ROM is copyrighted firmware, so
it is **not** in this repository or in the installers. Use the dump from your own MAME ROM set:

| File        | From MAME set   | Chip revision |
|-------------|-----------------|---------------|
| `sc01a.bin` | `votrsc01a.zip` | SC-01-A       |
| `sc01.bin`  | `votrsc01.zip`  | SC-01         |

Extract the `.bin` from the zip. Either file works. The app checks the file's hash and refuses anything
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

## Building from source

Needs the .NET 10 SDK. Building the .NET Framework 4.8 target also needs Windows. Building the
installers needs [Inno Setup 6](https://jrsoftware.org/isinfo.php).

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

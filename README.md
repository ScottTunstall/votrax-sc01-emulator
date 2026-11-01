# Votrax SC01 Emulator

A Windows program that speaks English text, or hand-written phonemes, through an accurate emulation of the
**Votrax SC-01** speech chip, the voice of Q\*bert, Gorf, Wizard of Wor and many 1980s speech boards.

The chip emulation is a C# port of MAME's
[`votrax.cpp`](https://github.com/mamedev/mame/blob/master/src/devices/sound/votrax.cpp) by Olivier
Galibert. On top of it is an English text-to-phoneme converter based on the US Naval Research Lab's
letter-to-sound rules. It builds for both **.NET Framework 4.8** and **.NET 10**, and has installers for each.

```console
VotraxSpeak "I am a Votrax speech synthesizer."
```

---

## Contents

- [Quick setup](#quick-setup)
- [The phoneme ROM](#the-phoneme-rom)
- [Usage](#usage)
  - [Speaking raw phonemes](#speaking-raw-phonemes)
- [Speed and pitch](#speed-and-pitch)
- [Building the Solution](#building-the-solution)
- [How it works](docs/how-it-works.md)
- [Licensing](#licensing)

---

## Quick setup

You don't need any programming tools.

1. **Download the installer.** Go to the
   [latest release](https://github.com/ScottTunstall/votrax-sc01-emulator/releases/latest) and click
   **`VotraxSpeakSetup-net10.exe`**. If you are not sure which one to pick, pick this one.

   *(Only if you have a very old or 32-bit Windows: use `VotraxSpeakSetup-net48.exe` instead.)*

2. **Get the chip's ROM file.** The program needs one small file, `sc01a.bin`, to sound like the real
   Votrax chip. We can't include it because it is copyrighted. It is in a free download called
   `votrsc01a.zip`, which is part of the MAME ROM sets you can find online (see
   [The phoneme ROM](#the-phoneme-rom)). Open the zip and take `sc01a.bin` out of it.

3. **Run the installer.** Double-click the file you downloaded.
   - If Windows says **"Windows protected your PC"**, click **More info**, then **Run anyway**. This
     happens with any new program that isn't from a big company.
   - When it asks for the chip ROM, click **Browse** and choose `sc01a.bin`.
   - Leave **Add VotraxSpeak to the PATH** ticked. Then click through to the end.

4. **Make it talk.** Press the Windows key, type `cmd`, and press Enter. In the black window, type this
   and press Enter:

   ```console
   VotraxSpeak "Hello, world!"
   ```

   You should hear the speech. Change the words between the quotes to say anything you like.

To save the speech as a sound file instead, type `VotraxSpeak -f hello.wav "Hello, world!"`. See
[Usage](#usage) for more. To remove the program, uninstall *VotraxSpeak* in Windows Settings > Apps.

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
| `--phonemes <names>`   | Speak a phoneme list exactly as written, instead of text (see [Speaking raw phonemes](#speaking-raw-phonemes)). |
| `--gorf <names>`       | The same, with the names spelled as in the Gorf arcade source. |
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

:: Exact phonemes instead of text (see Speaking raw phonemes)
VotraxSpeak --phonemes "PA1 I N S ER T PA1 K O1 UH3 I3 AY N N PA1"
```

Numbers, decimals and thousands separators are read out ("1,905.5" becomes "one thousand nine hundred
five point five"), as are `&`, `%`, `+`, `=` and `@`. Commas and full stops make longer pauses.

### Speaking raw phonemes

`--phonemes` (or `--gorf`, below) skips the text-to-phoneme step and sends the phonemes you list straight to the chip, so you
can reproduce hand-written speech such as the arcade games' own data. For example, Gorf's "Insert coin."
(from the [Gorf disassembly](https://github.com/tinymouse42/Gorf_Disassembly)):

```console
VotraxSpeak --phonemes "PA1 I N S ER T PA1 K O1 UH3 I3 AY N N PA1"
VotraxSpeak --gorf "PA1 I0 N S ER T PA1 K0 O1 UH3 I3 AY N N PA1"
```

- Names are the SC-01 data-sheet names, as used by MAME, separated by spaces or commas, in any case:
  `EH3 EH2 EH1 PA0 DT A1 A2 ZH AH2 I3 I2 I1 M N B V CH SH Z AW1 NG AH1 OO1 OO L K J H G F D S A AY Y1
  UH3 AH P O I U Y T R E W AE AE1 AW2 UH2 UH1 UH O2 O1 IU U1 THV TH ER EH E1 AW PA1 STOP`.
- `--gorf` takes the names as the Gorf source spells them (to avoid clashing with Forth numbers): `pA2` is `A1`,
  `pA1` is `A2`, `K0`/`J0`/`I0` are `K`/`J`/`I`, and the other `p` names (`pB pF pD pA pE pAE pAE1 pE1`)
  just drop the `p`. The codes are the same, so the two lines above sound identical. Gorf names are
  case-sensitive, because `pA1` (code `06`) and `PA1` (`3E`) differ.
- `HI` or `UP` after a name sets that phoneme's inflection bits (`$80` and `$40` in the Gorf data), as
  in `I1 UP Y UP AE`. Phonemes without one use `--pitch`.
- Nothing is added or tidied, so include the `PA1` pauses yourself. You cannot give both text and
  `--phonemes` or `--gorf`.

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

## Building the Solution

Only needed if you want to change the code; everyone else should use the [installers](#quick-setup).

Needs the .NET 10 SDK. Building the .NET Framework 4.8 target also needs Windows. Building the
installers needs [Inno Setup 6](https://jrsoftware.org/isinfo.php).

To run a build from source, put your ROM in [`roms/`](roms/README.md) first; `dotnet build` copies it
next to the executable.

```console
dotnet build              :: both target frameworks
dotnet test               :: 87 tests on each framework
installer\build.ps1       :: VotraxSpeakSetup-net48.exe and VotraxSpeakSetup-net10.exe
```

All compiler, .NET analyzer (`AnalysisLevel` `latest-all`), IDE code-style and
[Roslynator](https://github.com/dotnet/roslynator) diagnostics are build errors. The house rules
(Clean Code and SOLID, command/query separation, one type per file, enums and constants in their own
files, principle of least surprise) are recorded at the top of [`.editorconfig`](.editorconfig).

The tests build synthetic ROM images, so they run without the real firmware.

## How it works

The text-to-speech pipeline, the chip emulation and the project layout are described in
[`docs/how-it-works.md`](docs/how-it-works.md).

## Licensing

- The emulation in `src/VotraxSpeak/Synthesis` is derived from MAME's `votrax.cpp`, BSD-3-Clause,
  copyright Olivier Galibert.
- The letter-to-sound rules come from NRL Report 7948 (1976), a US Government work in the public domain.
- The Votrax SC-01 ROM is not included.

See [`THIRD-PARTY-NOTICES.md`](THIRD-PARTY-NOTICES.md) for the full notices.

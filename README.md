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
  - [Gorf speech, ready to paste](#gorf-speech-ready-to-paste)
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

### Gorf speech, ready to paste

Every speech phrase in the [Gorf disassembly](https://github.com/tinymouse42/Gorf_Disassembly), as a
`--gorf` command. They are generated from the disassembly's speech data bytes, so they play exactly what the
arcade board sent to the chip. Click a line's copy button, or select it, and paste it into a Command Prompt.

<details>
<summary>Show all 33 phrases</summary>

**"Insert coin."**

```console
VotraxSpeak --gorf "PA1 I0 N S ER T PA1 K0 O1 UH3 I3 AY N N PA1"
```

**"I am the Gorfian Empire."**

```console
VotraxSpeak --gorf "PA1 AH1 I1 UP Y UP pAE M UP THV UH1 UP G DT O1 R UP pF UP Y pA1 N EH1 UP M UP P AH1 I1 R PA1"
```

**"Space"**

```console
VotraxSpeak --gorf "S P pA1 I3 UP Y UP S UP"
```

**"Gorfians conquer another galaxy."**

```console
VotraxSpeak --gorf "G DT O1 UP R UP pF UP Y UP pA1 N S K0 AH1 N K0 UP ER UP AH2 N UP UH UP TH UP ER G UP pAE1 L UH1 K0 UP S Y PA1"
```

**"Try again; I devour your coins."**

```console
VotraxSpeak --gorf "T R UP AH2 UP I1 UP Y UP PA0 UH1 G UP EH1 UP I3 N PA1 PA1 AH1 I1 UP Y1 UP pD Y V AH1 UP U1 ER K0 UP O1 UP UH3 I3 pE1 N S PA1"
```

**"Long live Gorf!"**

```console
VotraxSpeak --gorf "PA1 L AW UP NG UP L I1 UP V UP G UP DT UP O1 UP O1 UP R R pF pF"
```

**"Gorfian robots... attack! attack!"**

```console
VotraxSpeak --gorf "G UP DT UP O1 UP R UP pF UP pE1 UP EH2 N R UP O1 UP U1 UP pB AH1 UH3 T S PA1 PA1 UH1 T pAE EH3 UP K0 UP PA0 UH1 T pAE EH3 UP K0 UP PA1"
```

**"Bad move"**

```console
VotraxSpeak --gorf "pB pAE EH1 UP pD UP M UP U UP U1 V PA1"
```

**"Ha ha ha ha!"**

```console
VotraxSpeak --gorf "H UP AH1 UP H AH1 UP H AH1 UP H AH1 PA1"
```

**"You cannot escape the Gorfian robots."**

```console
VotraxSpeak --gorf "Y1 I1 U1 UP U1 UP K0 pAE1 UP N UP UH1 T PA0 EH1 S K0 pA2 UP pE1 UP P UP THV UH1 G DT O1 UP R UP pF UP Y UP pA1 N PA0 R O1 UP U1 UP pB AH1 T S PA1"
```

**"Got you"**

```console
VotraxSpeak --gorf "G AH1 EH3 UP T UP Y1 UP I3 U1 PA1"
```

**"Nice shot"**

```console
VotraxSpeak --gorf "N UH3 AH2 UP Y UP S UP PA0 UP SH UP AH1 UP UH3 T PA1"
```

**"Too bad"**

```console
VotraxSpeak --gorf "T U UP pB UP pAE UP EH3 pD PA1"
```

**"Gorfians take no prisoners!"**

```console
VotraxSpeak --gorf "G DT O1 UP R UP pF Y pA1 N S T pA UP K0 UP N O UP P R UP I1 UP S UP I3 UP N EH3 R S PA1"
```

**"Cadet"**

```console
VotraxSpeak --gorf "K0 UH pD EH2 UP T UP PA1"
```

**"Captain"**

```console
VotraxSpeak --gorf "K0 pAE1 UP EH3 UP P UP T I3 N PA1"
```

**"Colonel"**

```console
VotraxSpeak --gorf "K0 ER UP N UP AH2 L PA1"
```

**"General"**

```console
VotraxSpeak --gorf "pD J0 EH2 UP N UP ER UH3 L PA1"
```

**"Warrior"**

```console
VotraxSpeak --gorf "W O UP R UP AY UP Y1 UP EH3 R PA1"
```

**"Avenger"**

```console
VotraxSpeak --gorf "UH1 V EH1 UP EH3 UP N UP N pD J0 ER PA1"
```

**"You have been promoted to"**

```console
VotraxSpeak --gorf "Y1 IU U1 UP U1 UP H pAE1 EH3 UP V UP pB EH3 EH1 UP N UP P R UH2 M O UP T UP EH3 pD PA0 T IU U1 PA0"
```

**"Some galactic defender you are"**

```console
VotraxSpeak --gorf "S UH M G UH1 L pAE K0 T I1 K0 PA0 pD pE1 pF UP EH1 UP N pD ER PA0 Y IU UP U1 UP U1 UP UH R PA1"
```

**"Bite the dust"**

```console
VotraxSpeak --gorf "pB AH2 I3 Y1 UP T UP THV UH UP pD UP UH UP S T PA1"
```

**"All hail the supreme Gorfian Empire!"**

```console
VotraxSpeak --gorf "AW L H pA1 UP AY UP L THV UH S U1 P UP R UP pE1 UP M PA1 G DT O1 UP R UP V Y pA1 N PA1 EH UP M UP P AH2 I3 Y R PA1"
```

**"Another enemy ship destroyed!"**

```console
VotraxSpeak --gorf "AH1 N UH THV R UP R UP PA0 EH1 N EH1 M Y SH I0 P PA0 pD UP pE1 UP S UP T R O1 I1 Y1 pD PA1"
```

**"Your end draws near"**

```console
VotraxSpeak --gorf "Y O UP R UP EH N pD PA0 pD R AW1 S N I0 UP R"
```

**"Push a player button."**

```console
VotraxSpeak --gorf "P OO1 IU UP SH UP pA1 AY Y1 P L pA2 UP AY UP ER PA0 pB UP UH1 UP DT T EH2 N N PA1"
```

**"You will meet a Gorfian doom"**

```console
VotraxSpeak --gorf "Y1 IU U1 UP U1 UP W I1 L UP M pE UP pE1 UP T UP pA1 pE1 G DT O1 UP R UP pF Y pA1 N pD UP U1 UP U UP M PA1"
```

**"Survival is impossible"**

```console
VotraxSpeak --gorf "S UP ER UP V AH2 I1 Y1 UP V UP UH3 L I1 S PA0 I1 M UP P UP AH1 S UP I1 UP pB L PA1"
```

**"Robot warriors, seek and destroy the"**

```console
VotraxSpeak --gorf "R O1 UP U1 UP pB AH1 T W O UP R UP AY UP Y1 UP EH3 R S PA1 S pE UP K0 UP pAE1 EH3 N pD pD pE1 S T R O1 I1 Y THV UH"
```

**"My Gorfian robots are unbeatable!"**

```console
VotraxSpeak --gorf "M AH2 I1 UP Y UP G DT O1 UP R UP pF Y pA1 N PA0 R O1 UP U1 UP pB AH1 T S PA0 AH1 UP R UP UH2 N pB UP pE1 UP AY UP T UP UH3 pB UH3 L PA1"
```

**"I am a Gorfian consciousness."**

```console
VotraxSpeak --gorf "AH1 I1 Y UP pAE1 EH2 UP M UP THV UH G DT O1 R pF UP Y pA1 N PA0 K0 UP AH1 UP N CH UP EH S N UP EH S PA1"
```

**"Prepare yourself for annihilation"**

```console
VotraxSpeak --gorf "P R pE1 UP P pAE1 UP ER UP Y U1 O1 UP R UP S UP EH1 L pF PA0 pF O1 R PA0 UH1 N AH2 I1 UP Y UP L pA1 UP SH UH2 N PA1"
```

</details>

Some phrases are only part of what the game says. It joins them end to end, so to hear the whole line,
paste the lists one after the other inside one pair of quotes. For example, "Your end draws near, Space
Cadet." is "Your end draws near", then "Space", then "Cadet":

```console
VotraxSpeak --gorf "Y O UP R UP EH N pD PA0 pD R AW1 S N I0 UP R S P pA1 I3 UP Y UP S UP K0 UH pD EH2 UP T UP PA1"
```

The three Flag Ship phrases ("Next time will be harder, but for now", "In the Gorfian chronicles" and
"For hitting my flagship") are in the game's upper ROM, which the disassembly lists by address only, so
their phonemes are not included.

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

## Votrax SC01 Emulator 1.1

### What's new

- **Speak exact phonemes.** `--phonemes "<names>"` skips the text conversion and sends your phonemes
  straight to the chip, using the standard SC-01 data-sheet names (the ones MAME uses). Nothing is added
  or tidied, so you control the pauses.
- **Gorf phoneme names.** `--gorf "<names>"` does the same with the names spelled as in the Gorf arcade
  source. The arcade's "Insert coin." plays as it does in the game:

  ```console
  VotraxSpeak --gorf "PA1 I0 N S ER T PA1 K0 O1 UH3 I3 AY N N PA1"
  ```

- `HI` and `UP` after a phoneme set its inflection bits, as in the Gorf data.
- A simpler, plain-language **Quick setup** in the README for people who aren't developers, and a new
  `docs/how-it-works.md`.
- 87 unit tests on each framework.

### Downloads

| Installer | For |
|---|---|
| `VotraxSpeakSetup-net10.exe` | 64-bit Windows. Installs the .NET 10 runtime if it is missing. |
| `VotraxSpeakSetup-net48.exe` | Any Windows with .NET Framework 4.8 (built into Windows 10 1903 and later). |

### You need the phoneme ROM

The SC-01's ROM is copyrighted firmware and is **not** included. Obtain `votrsc01a.zip` from the MAME ROM
sets available online, extract `sc01a.bin`, and pick it on the installer's ROM page. See the
[README](https://github.com/ScottTunstall/votrax-sc01-emulator#the-phoneme-rom).

The installers are not code-signed, so Windows SmartScreen may warn on first run.

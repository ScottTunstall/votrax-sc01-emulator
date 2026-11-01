## Votrax SC01 Emulator 1.2

### Changes you need to know about

- **`@` replaces `--`** as the marker meaning "treat everything after this as text", so text that starts with
  a dash can still be spoken: `VotraxSpeak @ "-5 degrees outside"`. `--` is no longer recognised.
- **Only `sc01a.bin` is accepted as the phoneme ROM.** `sc01.bin` (from `votrsc01.zip`) is now rejected, and
  is no longer searched for. Use `sc01a.bin` from `votrsc01a.zip`.

### Also in this release

- The installer's ROM page now says plainly where to get the ROM (the MAME ROM sets), which zip to download
  (`votrsc01a.zip`) and which file to choose (`sc01a.bin`).
- Documentation moved into pages under `docs/`: how it works, building, and a new page on speaking raw
  phonemes with `--phonemes` and `--gorf`, including ready-to-paste commands for all 33 phrases in the Gorf
  disassembly.
- 88 unit tests on each framework.

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

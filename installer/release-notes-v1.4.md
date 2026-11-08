## Votrax SC01 Emulator 1.4

### What changed

- **Save as Ogg Vorbis.** `-f` now picks the file format from the name: `VotraxSpeak -f hello.ogg "Hello"`
  writes an Ogg Vorbis file, and anything else writes a WAV file, as before.
- **`.wav` is added for you.** `-f hello` now writes `hello.wav`. Before, it wrote a WAV file called `hello`
  with no extension. A name that already has an extension is used as typed.
- **`--gorf` and `--phonemes` no longer need quotes.** `VotraxSpeak --gorf PA1 I0 N S ER T PA1 -f insert.ogg`
  works. The phoneme names are read up to the next option, so `-f` can go before or after them. Before,
  only the first name was taken, and the rest were treated as text and rejected.
- **A mistyped option now helps.** `-gorf` reports `Did you mean '--gorf'?` instead of just failing.

### Downloads

| Installer | For |
|---|---|
| `VotraxSpeakSetup-net10.exe` | 64-bit Windows. Needs the .NET 10 runtime and offers a link to it if it is missing. |
| `VotraxSpeakSetup-net48.exe` | Any Windows with .NET Framework 4.8 (built into Windows 10 1903 and later). |

### You need the phoneme ROM

The SC-01's ROM is copyrighted firmware and is **not** included. Obtain `votrsc01a.zip` from the MAME ROM
sets available online, extract `sc01a.bin`, and pick it on the installer's ROM page. See the
[README](https://github.com/ScottTunstall/votrax-sc01-emulator#the-phoneme-rom).

The installers are not code-signed, so Windows SmartScreen may still warn on first run.

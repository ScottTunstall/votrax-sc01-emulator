# The Windows installers

Builds two normal `setup.exe` installers for VotraxSpeak, one for each target framework:

| Installer                    | Program built for              | Runs on |
|------------------------------|--------------------------------|---------|
| `VotraxSpeakSetup-net48.exe` | .NET Framework 4.8             | Any Windows with .NET Framework 4.8 (built into Windows 10 1903 and later) |
| `VotraxSpeakSetup-net10.exe` | .NET 10, framework-dependent   | 64-bit Windows; fetches the .NET 10 runtime if it is missing |

Each installs the program under `Program Files`, adds it to the PATH so `VotraxSpeak` works from any
Command Prompt, and registers an entry in "Installed apps" / "Programs and Features" so it can be
removed like any other program. Both use the same app identity, so installing one replaces the other
instead of installing both into the same folder.

The installers are built with [Inno Setup](https://jrsoftware.org/isinfo.php), the same as
CDGFromYoutube: one script, one free compiler, and the uninstaller comes for free.

## Building them

Needs the .NET 10 SDK and [Inno Setup 6](https://jrsoftware.org/isinfo.php)
(`winget install JRSoftware.InnoSetup`). `build.ps1` looks for the compiler, `ISCC.exe`, on the `PATH`
and in the default install locations.

```console
installer\build.ps1                     # both installers
installer\build.ps1 -Framework net48    # just one
installer\build.ps1 -Version 1.2.0      # stamp a different version
```

For each framework, this publishes VotraxSpeak into `installer\publish\<framework>`, checks that no ROM
got in, and compiles `VotraxSpeak.iss` into `installer\Output\VotraxSpeakSetup-<suffix>.exe`. The version
comes from `<Version>` in `VotraxSpeak.csproj` unless `-Version` is given.

## The phoneme ROM

The Votrax SC-01 ROM is copyrighted firmware and is **never** bundled:

* the project does not publish `roms\*.bin`,
* `build.ps1` fails if a `.bin` file reaches a publish folder, and
* the script excludes `*.bin` from the files it installs.

Instead, setup has an optional **Votrax SC-01 phoneme ROM** page. There the user picks their own
`sc01a.bin` (`sc01.bin` is rejected), and setup copies it into the install folder, where VotraxSpeak finds it
automatically. For a silent install, pass `/ROM=<path>`:

```console
VotraxSpeakSetup-net10.exe /VERYSILENT /ROM="D:\roms\sc01a.bin"
```

The copied ROM is kept when a newer version (or the other framework's installer) replaces this one. It
is deleted on a normal uninstall.

## What it does

* Installs the published files, `README.md` and `THIRD-PARTY-NOTICES.md` (the BSD license of the MAME
  code requires its notice to go with the binaries) into `Program Files\VotraxSpeak`.
* Offers to add that folder to the machine `PATH` (checked by default).
* Adds a Start menu shortcut that opens a Command Prompt in the install folder, showing `--help`.
* **.NET Framework 4.8 installer:** stops with an explanation if .NET Framework 4.8 is not installed,
  since a script cannot install it.
* **.NET 10 installer:** runs Microsoft's `dotnet-install.ps1` to install the .NET 10 runtime if it is
  not already there, exactly as CDGFromYoutube's installer does. If that fails (for example with no
  internet), the program still installs, and Windows shows its own "you need .NET" prompt on first run.
* Removes any previously installed version first, so files it no longer ships are not left behind.

## What it does not do

* It does not sign the installers or the program, so Windows SmartScreen may warn about them on a
  machine that has not seen them before. Signing needs a code signing certificate.

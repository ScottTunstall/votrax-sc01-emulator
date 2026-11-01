## Votrax SC01 Emulator 1.3

### What changed

- **The .NET 10 installer no longer runs a hidden PowerShell download.** Earlier versions fetched and ran
  Microsoft's `dotnet-install.ps1` script behind the scenes to install the .NET 10 runtime. Antivirus
  software flags that behaviour as a downloader, so the installer was being reported as a virus. It now
  just checks whether the runtime is installed and, if it is not, asks whether to open Microsoft's download
  page. It does not download or run anything itself.
- If you have .NET 10 installed already, you will see no extra prompt.

If you need the runtime, pick the **.NET Runtime** for x64 under Windows from
https://dotnet.microsoft.com/download/dotnet/10.0.

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

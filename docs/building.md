# Building the Solution

[Back to the README](../README.md)

Only needed if you want to change the code; everyone else should use the
[installers](../README.md#quick-setup).

Needs the .NET 10 SDK. Building the .NET Framework 4.8 target also needs Windows. Building the
installers needs [Inno Setup 6](https://jrsoftware.org/isinfo.php); see
[`installer/README.md`](../installer/README.md).

To run a build from source, put your ROM in [`roms/`](../roms/README.md) first; `dotnet build` copies it
next to the executable.

```console
dotnet build              :: both target frameworks
dotnet test               :: 88 tests on each framework
installer\build.ps1       :: VotraxSpeakSetup-net48.exe and VotraxSpeakSetup-net10.exe
```

If PowerShell refuses to run `build.ps1` because script execution is disabled, run it for that one
command with `powershell -ExecutionPolicy Bypass -File installer\build.ps1`.

All compiler, .NET analyzer (`AnalysisLevel` `latest-all`), IDE code-style and
[Roslynator](https://github.com/dotnet/roslynator) diagnostics are build errors. The house rules
(Clean Code and SOLID, command/query separation, one type per file, enums and constants in their own
files, principle of least surprise) are recorded at the top of [`.editorconfig`](../.editorconfig).

The tests build synthetic ROM images, so they run without the real firmware.

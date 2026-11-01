; Inno Setup script for VotraxSpeak.
;
; Inno Setup is used, as in CDGFromYoutube, because it is the simplest way to get a normal Windows
; setup.exe: one script, one free compiler (https://jrsoftware.org/isinfo.php), and an "Installed apps"
; entry with a working uninstaller for free.
;
; One script builds both installers. TargetFramework picks which published build goes in:
;   /DTargetFramework=net48    -> Output\VotraxSpeakSetup-net48.exe  (.NET Framework 4.8)
;   /DTargetFramework=net10.0  -> Output\VotraxSpeakSetup-net10.exe  (.NET 10, framework-dependent)
;
; This script does not build the program. Run build.ps1, which publishes VotraxSpeak for both
; frameworks and then compiles this script once for each; see installer\README.md.
;
; The Votrax SC-01 phoneme ROM is copyrighted firmware and is never bundled. The installer can copy the
; user's own copy into the install folder instead (see the ROM page in [Code]).

#define MyAppName "VotraxSpeak"
#define MyAppExeName "VotraxSpeak.exe"
#ifndef MyAppVersion
  #define MyAppVersion "1.0.0"
#endif
#ifndef TargetFramework
  #error Pass /DTargetFramework=net48 or /DTargetFramework=net10.0 (build.ps1 does this).
#endif

#if TargetFramework == "net48"
  #define FrameworkName ".NET Framework 4.8"
  #define FileSuffix "net48"
#elif TargetFramework == "net10.0"
  #define FrameworkName ".NET 10"
  #define FileSuffix "net10"
#else
  #error TargetFramework must be net48 or net10.0.
#endif

#define MyPublishDir "publish\" + TargetFramework

[Setup]
; A fixed, random GUID that identifies VotraxSpeak across versions. Both installers share it, so
; installing either one replaces whichever is already there instead of installing side by side into
; the same folder. It must never change. The doubled "{{" is Inno's escape for a literal "{".
AppId={{D80EF121-95D2-484A-9FC1-D531A78FE449}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
; Shown in "Installed apps", so it is clear which of the two builds is installed.
AppVerName={#MyAppName} {#MyAppVersion} ({#FrameworkName})
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
; Keep the destination page showing even when a previous install exists, so the folder is always
; visible and changeable (the default, "auto", silently reuses the old location).
DisableDirPage=no
DisableProgramGroupPage=yes
UninstallDisplayIcon={app}\{#MyAppExeName}
OutputDir=Output
OutputBaseFilename=VotraxSpeakSetup-{#FileSuffix}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
#if TargetFramework == "net10.0"
; The .NET 10 build is published for win-x64.
ArchitecturesAllowed=x64compatible
#endif
; The .NET Framework build is AnyCPU, so it installs on any Windows; on 64-bit Windows it still goes
; into the 64-bit Program Files.
ArchitecturesInstallIn64BitMode=x64compatible
; Program Files and the machine PATH both need administrator rights to write to.
PrivilegesRequired=admin
; Tells Windows Explorer to notice the PATH change, so a newly opened Command Prompt sees it without a reboot.
ChangesEnvironment=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "addtopath"; Description: "Add {#MyAppName} to the PATH, so ""{#MyAppExeName}"" can be run from any Command Prompt"

[Files]
; publish\<framework> is created by build.ps1 (dotnet publish). ROM images are excluded as well as
; never being published, so this script cannot bundle one even if it is compiled by hand.
Source: "{#MyPublishDir}\*"; DestDir: "{app}"; Excludes: "*.bin"; Flags: recursesubdirs ignoreversion
Source: "..\README.md"; DestDir: "{app}"; Flags: ignoreversion
; The MAME port is BSD-licensed, which requires its notice to accompany binary distributions.
Source: "..\THIRD-PARTY-NOTICES.md"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
; VotraxSpeak takes the text to speak as an argument, so a plain shortcut would only flash a usage
; message and close. This one opens a prompt in the install folder instead, ready to type into.
Name: "{group}\{#MyAppName} Command Prompt"; Filename: "{cmd}"; \
    Parameters: "/K ""cd /d ""{app}"" && {#MyAppExeName} --help"""; WorkingDir: "{app}"; \
    IconFilename: "{app}\{#MyAppExeName}"
Name: "{group}\Uninstall {#MyAppName}"; Filename: "{uninstallexe}"

#if TargetFramework == "net10.0"
[Run]
; Fetches the .NET 10 runtime if it is missing, using Microsoft's own install script (the same approach
; as CDGFromYoutube). A failure here is only logged; the program still installs, and Windows offers the
; runtime download the first time it is run without one, as for any other .NET program.
Filename: "{sys}\WindowsPowerShell\v1.0\powershell.exe"; \
    Parameters: "-NoProfile -ExecutionPolicy Bypass -File ""{tmp}\install-dotnet-runtime.ps1"""; \
    StatusMsg: "Checking for the .NET 10 runtime..."; \
    Flags: runhidden waituntilterminated
#endif

[Code]
const
  EnvironmentKey = 'SYSTEM\CurrentControlSet\Control\Session Manager\Environment';
  NetFrameworkKey = 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full';
  { The "Release" value Microsoft documents for .NET Framework 4.8. }
  NetFramework48Release = 528040;
  RomImageSize = 512;
  RomFileNameA = 'sc01a.bin';
  RomFileName = 'sc01.bin';

var
  RomPage: TInputFileWizardPage;

function GetInstallDir(): string;
begin
  Result := ExpandConstant('{app}');
end;

{ Finds the uninstaller a previously installed version registered, by AppId, so it can be removed
  before this one is put down. Otherwise files an older version wrote but this one no longer includes
  would be left behind, since the same AppId makes Inno install over the old copy in place. }
function GetPreviousUninstallString(): string;
var
  UninstallKey: string;
  UninstallCommand: string;
begin
  UninstallKey := 'Software\Microsoft\Windows\CurrentVersion\Uninstall\{#SetupSetting("AppId")}_is1';
  if not RegQueryStringValue(HKLM, UninstallKey, 'UninstallString', UninstallCommand) then
    RegQueryStringValue(HKCU, UninstallKey, 'UninstallString', UninstallCommand);
  Result := UninstallCommand;
end;

{ Runs a previously installed version's uninstaller silently. Errors are ignored (best effort): a
  failed removal still leaves the new install to overwrite what it can. A silent uninstall keeps the
  user's ROM (see CurUninstallStepChanged), so it survives an upgrade or a switch between the two builds. }
procedure UninstallPreviousVersion();
var
  UninstallCommand: string;
  ExitCode: Integer;
begin
  UninstallCommand := GetPreviousUninstallString();
  if UninstallCommand = '' then
    Exit;

  UninstallCommand := RemoveQuotes(UninstallCommand);
  Exec(UninstallCommand, '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART', '', SW_HIDE,
      ewWaitUntilTerminated, ExitCode);
end;

#if TargetFramework == "net48"
{ The .NET Framework build cannot run without .NET Framework 4.8, and unlike .NET 10 it cannot be
  fetched by a script, so setup stops with an explanation instead of installing a program that will
  not start. Windows 10 (May 2019 Update) and later include it. }
function IsNetFramework48Installed(): Boolean;
var
  Release: Cardinal;
begin
  Result := RegQueryDWordValue(HKLM, NetFrameworkKey, 'Release', Release)
      and (Release >= NetFramework48Release);
end;
#endif

function InitializeSetup(): Boolean;
begin
#if TargetFramework == "net48"
  if not IsNetFramework48Installed() then
  begin
    SuppressibleMsgBox('This version of {#MyAppName} needs .NET Framework 4.8, which is not installed.' + #13#10#13#10 +
        'Install it from https://dotnet.microsoft.com/download/dotnet-framework/net48, or use ' +
        'VotraxSpeakSetup-net10.exe instead.', mbCriticalError, MB_OK, IDOK);
    Result := False;
    Exit;
  end;
#endif

  UninstallPreviousVersion();
  Result := True;
end;

{ Asks for the user's own copy of the phoneme ROM. It is optional: without it, VotraxSpeak still
  installs and can be pointed at a ROM with --rom. "/ROM=<path>" on the setup command line fills it in,
  for silent installs. }
procedure InitializeWizard();
begin
  RomPage := CreateInputFilePage(wpSelectTasks,
      'Votrax SC-01 phoneme ROM',
      'Where is your copy of the speech chip''s ROM?',
      '{#MyAppName} needs one small file from the speech chip, called sc01a.bin. It is copyrighted, so this ' +
      'installer does not include it.' + #13#10#13#10 +
      'You can get it from the MAME ROM sets, which are available online. Download the zip called ' +
      'votrsc01a.zip and open it: sc01a.bin is inside. (The original chip''s version is in votrsc01.zip, ' +
      'as sc01.bin.)' + #13#10#13#10 +
      'Then choose that .bin file below to have it copied into the install folder. You can leave this blank ' +
      'to keep a ROM that is already installed, or to give the file to {#MyAppName} later with --rom <path>.');
  RomPage.Add('&ROM file:', 'Votrax SC-01 ROM (sc01a.bin, sc01.bin)|sc01a.bin;sc01.bin|All files|*.*', '.bin');
  RomPage.Values[0] := ExpandConstant('{param:ROM|}');
end;

function IsRomFileName(const Path: string): Boolean;
var
  Name: string;
begin
  Name := ExtractFileName(Path);
  Result := (CompareText(Name, RomFileNameA) = 0) or (CompareText(Name, RomFileName) = 0);
end;

{ Checks the chosen ROM before leaving the page. The program verifies the ROM's hash when it runs;
  this only catches the obvious mistakes (wrong file, or the .zip instead of the .bin) early. }
function NextButtonClick(CurPageID: Integer): Boolean;
var
  RomPath: string;
  Size: Integer;
begin
  Result := True;
  if CurPageID <> RomPage.ID then
    Exit;

  RomPath := Trim(RomPage.Values[0]);
  if RomPath = '' then
    Exit;

  if not FileExists(RomPath) then
  begin
    SuppressibleMsgBox('The file "' + RomPath + '" does not exist.', mbError, MB_OK, IDOK);
    Result := False;
  end
  else if not IsRomFileName(RomPath) then
  begin
    SuppressibleMsgBox('Choose sc01a.bin or sc01.bin. If you have votrsc01a.zip or votrsc01.zip, ' +
        'extract the .bin file from it first.', mbError, MB_OK, IDOK);
    Result := False;
  end
  else if (not FileSize(RomPath, Size)) or (Size <> RomImageSize) then
  begin
    SuppressibleMsgBox('"' + RomPath + '" is not a Votrax SC-01 ROM image (those are exactly 512 bytes).',
        mbError, MB_OK, IDOK);
    Result := False;
  end;
end;

{ Copies the chosen ROM into the install folder, where VotraxSpeak looks for it by default. }
procedure InstallRom();
var
  RomPath: string;
begin
  RomPath := Trim(RomPage.Values[0]);
  if RomPath = '' then
    Exit;

  if not CopyFile(RomPath, AddBackslash(GetInstallDir()) + Lowercase(ExtractFileName(RomPath)), False) then
    SuppressibleMsgBox('The ROM could not be copied into ' + GetInstallDir() + '. Copy it there yourself, ' +
        'or pass --rom <path> when you run {#MyAppName}.', mbError, MB_OK, IDOK);
end;

{ Appends the install directory to the machine PATH, unless it is already there. }
procedure AddToPath();
var
  Path: string;
  InstallDir: string;
begin
  InstallDir := GetInstallDir();
  if not RegQueryStringValue(HKEY_LOCAL_MACHINE, EnvironmentKey, 'Path', Path) then
    Path := '';

  if (Pos(';' + Uppercase(InstallDir) + ';', ';' + Uppercase(Path) + ';') > 0) then
    Exit;

  if (Length(Path) > 0) and (Path[Length(Path)] <> ';') then
    Path := Path + ';';
  Path := Path + InstallDir;

  RegWriteExpandStringValue(HKEY_LOCAL_MACHINE, EnvironmentKey, 'Path', Path);
end;

{ Removes the install directory from the machine PATH, leaving everything else in it untouched. }
procedure RemoveFromPath();
var
  Path: string;
  InstallDir: string;
  MarkedPath: string;
  Position: Integer;
begin
  InstallDir := GetInstallDir();
  if not RegQueryStringValue(HKEY_LOCAL_MACHINE, EnvironmentKey, 'Path', Path) then
    Exit;

  MarkedPath := ';' + Path + ';';
  Position := Pos(';' + Uppercase(InstallDir) + ';', Uppercase(MarkedPath));
  if Position = 0 then
    Exit;

  Delete(MarkedPath, Position, Length(InstallDir) + 1);
  { Drop the leading and trailing markers added above; they are all that is left when the install
    directory was the only entry in the PATH. }
  if Length(MarkedPath) <= 2 then
    Path := ''
  else
    Path := Copy(MarkedPath, 2, Length(MarkedPath) - 2);

  RegWriteExpandStringValue(HKEY_LOCAL_MACHINE, EnvironmentKey, 'Path', Path);
end;

#if TargetFramework == "net10.0"
{ Writes the PowerShell script that [Run] executes, rather than an inline one-liner, so quoting and
  error reporting do not have to fight Inno's own command line escaping. }
procedure CreateDotNetInstallScript();
var
  Lines: TArrayOfString;
begin
  SetArrayLength(Lines, 12);
  Lines[0] := '$ErrorActionPreference = ''Stop''';
  Lines[1] := 'try {';
  Lines[2] := '    if (Get-Command dotnet -ErrorAction SilentlyContinue) {';
  Lines[3] := '        $has10 = & dotnet --list-runtimes 2>$null | Select-String ''Microsoft\.NETCore\.App 10\.''';
  Lines[4] := '        if ($has10) { exit 0 }';
  Lines[5] := '    }';
  Lines[6] := '    $script = Join-Path $env:TEMP ''dotnet-install.ps1''';
  Lines[7] := '    Invoke-WebRequest -Uri ''https://dot.net/v1/dotnet-install.ps1'' -OutFile $script -UseBasicParsing';
  Lines[8] := '    & $script -Channel 10.0 -Runtime dotnet -InstallDir "$env:ProgramFiles\dotnet"';
  Lines[9] := '} catch {';
  Lines[10] := '    Write-Warning "Could not install the .NET 10 runtime automatically: $_"';
  Lines[11] := '}';
  SaveStringsToFile(ExpandConstant('{tmp}\install-dotnet-runtime.ps1'), Lines, False);
end;
#endif

procedure CurStepChanged(CurStep: TSetupStep);
begin
#if TargetFramework == "net10.0"
  if CurStep = ssInstall then
    CreateDotNetInstallScript();
#endif

  if CurStep = ssPostInstall then
  begin
    InstallRom();
    if WizardIsTaskSelected('addtopath') then
      AddToPath();
  end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usUninstall then
    RemoveFromPath();

  { The ROM was copied in by [Code], not installed by [Files], so Inno would leave it behind. It is
    removed on a normal uninstall, but kept when a newer setup silently uninstalls this one first (see
    UninstallPreviousVersion), so an upgrade does not lose it. }
  if (CurUninstallStep = usPostUninstall) and not UninstallSilent() then
  begin
    DeleteFile(AddBackslash(GetInstallDir()) + RomFileNameA);
    DeleteFile(AddBackslash(GetInstallDir()) + RomFileName);
    { Only succeeds when empty, so nothing else the user put in the install folder is touched. }
    RemoveDir(GetInstallDir());
  end;
end;

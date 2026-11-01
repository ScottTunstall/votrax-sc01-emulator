<#
.SYNOPSIS
    Builds VotraxSpeakSetup-net48.exe and VotraxSpeakSetup-net10.exe: publishes the program for both
    frameworks, then compiles the Inno Setup script around each build.

.DESCRIPTION
    For each target framework, in order:
      1. dotnet publish into installer\publish\<framework>, which the .iss script picks up.
         .NET Framework 4.8 is published as is; .NET 10 is published framework-dependent for win-x64,
         so the installer (not the .exe) is what fetches the .NET 10 runtime.
      2. A check that no phoneme ROM (*.bin) reached the publish folder. The ROM is copyrighted
         firmware and must never be bundled.
      3. ISCC.exe (the Inno Setup compiler) turns VotraxSpeak.iss into Output\VotraxSpeakSetup-<suffix>.exe.

    Needs the .NET 10 SDK and Inno Setup 6 (https://jrsoftware.org/isinfo.php) installed.

.PARAMETER Version
    The version number written into the installed program's "Installed apps" entry. Defaults to the
    <Version> in VotraxSpeak.csproj, so the two stay in step.

.PARAMETER Framework
    Which installer to build: net48, net10.0, or both (the default).
#>
param(
    [string]$Version,
    [ValidateSet("net48", "net10.0", "both")]
    [string]$Framework = "both"
)

$ErrorActionPreference = "Stop"
$installerDir = $PSScriptRoot
$repoRoot = Split-Path $installerDir -Parent
$projectPath = Join-Path $repoRoot "src\VotraxSpeak\VotraxSpeak.csproj"
$scriptPath = Join-Path $installerDir "VotraxSpeak.iss"

if (-not $Version) {
    $Version = ([xml](Get-Content $projectPath)).Project.PropertyGroup.Version | Where-Object { $_ } | Select-Object -First 1
    if (-not $Version) {
        throw "No <Version> found in $projectPath, and -Version was not given."
    }
}

$isccPath = (Get-Command ISCC.exe -ErrorAction SilentlyContinue).Source
if (-not $isccPath) {
    foreach ($candidate in @(
        "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe",
        "${env:ProgramFiles}\Inno Setup 6\ISCC.exe",
        "${env:LOCALAPPDATA}\Programs\Inno Setup 6\ISCC.exe")) {
        if (Test-Path $candidate) { $isccPath = $candidate; break }
    }
}
if (-not $isccPath) {
    throw "Inno Setup 6 was not found. Install it with 'winget install JRSoftware.InnoSetup', or " +
        "from https://jrsoftware.org/isinfo.php, and run this script again."
}

$frameworks = if ($Framework -eq "both") { @("net48", "net10.0") } else { @($Framework) }

foreach ($targetFramework in $frameworks) {
    $publishDir = Join-Path $installerDir "publish\$targetFramework"

    Write-Output "Publishing VotraxSpeak $Version for $targetFramework..."
    if (Test-Path $publishDir) { Remove-Item $publishDir -Recurse -Force }

    $publishArguments = @($projectPath, "-c", "Release", "-f", $targetFramework, "-o", $publishDir, "-p:Version=$Version")
    if ($targetFramework -eq "net10.0") {
        $publishArguments += @("-r", "win-x64", "--self-contained", "false")
    }
    dotnet publish @publishArguments
    if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed for $targetFramework." }

    $bundledRoms = Get-ChildItem $publishDir -Recurse -Filter *.bin
    if ($bundledRoms) {
        throw "A phoneme ROM reached the publish folder ($($bundledRoms.FullName -join ', ')). " +
            "ROMs are copyrighted firmware and must not be bundled into the installer."
    }

    Write-Output "Compiling the $targetFramework installer with $isccPath..."
    & $isccPath "/DMyAppVersion=$Version" "/DTargetFramework=$targetFramework" $scriptPath
    if ($LASTEXITCODE -ne 0) { throw "ISCC.exe failed for $targetFramework." }
}

Write-Output "Done:"
Get-ChildItem (Join-Path $installerDir "Output") -Filter "VotraxSpeakSetup-*.exe" | ForEach-Object { Write-Output "  $($_.FullName)" }

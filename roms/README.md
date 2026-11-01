# Phoneme ROMs

VotraxSpeak emulates the Votrax SC-01 speech chip, and the emulation needs the chip's internal
512-byte phoneme ROM. That ROM is copyrighted firmware, so it is **not** in this repository. Copy your
own dump into this folder.

## Files to copy here

| File        | Extract it from | Chip revision | SHA-1 (as listed in MAME)                  |
|-------------|-----------------|---------------|--------------------------------------------|
| `sc01a.bin` | `votrsc01a.zip` | SC-01-A       | `1d6da90b1807a01b5e186ef08476119a862b5e6d` |

`sc01a.bin` is the only ROM VotraxSpeak accepts. MAME also has an older `sc01.bin` (in `votrsc01.zip`): **DO NOT USE `sc01.bin`.** The app rejects it.

## Where to get it

The zip is part of MAME's ROM sets, which are available online: search for "MAME ROMs", or for
`votrsc01a.zip`. Use a MAME-compatible set (the names above are the MAME device ROM
names) and make sure you are entitled to use it. You only need that one small zip (512 bytes of ROM);
none of the arcade game sets are required.
Copy the **.bin** file out of the zip; the app does not read zip files.

Check a dump with:

```powershell
Get-FileHash roms\sc01a.bin -Algorithm SHA1
```

The app refuses any file whose hash does not match one of the dumps above.

## What happens to them

* `dotnet build` copies `*.bin` from here next to `VotraxSpeak.exe`, so local builds work straight away.
* They are **never** published or put into the installers (`installer\build.ps1` checks this). The
  installer instead asks for the user's own copy and copies it into the install folder.
* `.gitignore` excludes `*.bin` and `*.zip` here, so a ROM cannot be committed by accident.

# Speaking raw phonemes

[Back to the README](../README.md)

`--phonemes` (or `--gorf`) skips the text-to-phoneme step and sends the phonemes you list straight to the chip, so you
can reproduce hand-written speech such as the arcade games' own data. For example, Gorf's "Insert coin."
(from the [Gorf disassembly](https://github.com/tinymouse42/Gorf_Disassembly)):

```console
VotraxSpeak --phonemes "PA1 I N S ER T PA1 K O1 UH3 I3 AY N N PA1"
VotraxSpeak --gorf "PA1 I0 N S ER T PA1 K0 O1 UH3 I3 AY N N PA1"
```

- Names are the SC-01 data-sheet names, as used by MAME, separated by spaces or commas, in any case:
  `EH3 EH2 EH1 PA0 DT A1 A2 ZH AH2 I3 I2 I1 M N B V CH SH Z AW1 NG AH1 OO1 OO L K J H G F D S A AY Y1
  UH3 AH P O I U Y T R E W AE AE1 AW2 UH2 UH1 UH O2 O1 IU U1 THV TH ER EH E1 AW PA1 STOP`.
- `--gorf` takes the names as the [Gorf arcade source](https://github.com/tinymouse42/Gorf_Disassembly/blob/main/src/Gorf_Disassembly.asm) spells them (to avoid clashing with Forth numbers): `pA2` is `A1`,
  `pA1` is `A2`, `K0`/`J0`/`I0` are `K`/`J`/`I`, and the other `p` names (`pB pF pD pA pE pAE pAE1 pE1`)
  just drop the `p`. The codes are the same, so the two lines above sound identical. Gorf names are
  case-sensitive, because `pA1` (code `06`) and `PA1` (`3E`) differ.
- `HI` or `UP` after a name sets that phoneme's inflection bits (`$80` and `$40` in the Gorf data), as
  in `I1 UP Y UP AE`. Phonemes without one use `--pitch`.
- Nothing is added or tidied, so include the `PA1` pauses yourself. You cannot give both text and
  `--phonemes` or `--gorf`.

## Gorf speech, ready to paste

Every speech phrase in the [Gorf disassembly](https://github.com/tinymouse42/Gorf_Disassembly), as a
`--gorf` command for VotraxSpeak. They are generated from the disassembly's speech data
bytes, so they play exactly what the arcade board sent to the chip. Click a block's copy button, or select
it, and paste it into a Command Prompt.

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

Some phrases are only part of what the game says. It joins them end to end, so to hear the whole line,
paste the lists one after the other inside one pair of quotes. For example, "Your end draws near, Space
Cadet." is "Your end draws near", then "Space", then "Cadet":

```console
VotraxSpeak --gorf "Y O UP R UP EH N pD PA0 pD R AW1 S N I0 UP R S P pA1 I3 UP Y UP S UP K0 UH pD EH2 UP T UP PA1"
```

The three Flag Ship phrases ("Next time will be harder, but for now", "In the Gorfian chronicles" and
"For hitting my flagship") are in the game's upper ROM, which the disassembly lists by address only, so
their phonemes are not included.


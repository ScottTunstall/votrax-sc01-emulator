# How it works

Part of the [Votrax SC01 Emulator](../README.md).

```
text ─► TextNormalizer ─► LetterToSoundTranslator ─► ArpabetToVotraxMapper ─► UtteranceShaper
        (numbers, case,    (NRL Report 7948 rules)    (rule phonemes →         (tidy pauses)
         symbols)                                      SC-01 phonemes)
                                                                │
     IAudioOutput ◄── PeakNormalizer ◄── VotraxSpeechRenderer ◄─┘
  (speakers / WAV)                       (drives VotraxSc01Chip like a host CPU:
                                          write phoneme, wait for A/R, repeat)
```

| Folder                    | Contents |
|---------------------------|----------|
| `src/VotraxSpeak/Synthesis` | The MAME port. `ControlLogic` holds the digital state (phoneme timing, interpolation, pitch and noise). `AnalogSignalPath` holds the switched-capacitor filters. `VotraxSc01Chip` is the pin-level facade. |
| `src/VotraxSpeak/Phonetics` | English text to SC-01 phonemes. |
| `src/VotraxSpeak/Speech`    | The rendering pipeline and voice settings. |
| `src/VotraxSpeak/Audio`     | Audio clips, WAV encoding, speaker and file outputs. |
| `src/VotraxSpeak/Cli`       | Command-line parsing and help. |
| `tests/VotraxSpeak.Tests`   | xUnit tests. |
| `installer`                 | Inno Setup installers for both frameworks. |
| `roms`                      | Where your ROM dump goes (not committed). |


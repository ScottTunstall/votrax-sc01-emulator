using System;
using System.IO;
using VotraxSpeak.Audio;
using VotraxSpeak.Cli;
using VotraxSpeak.Phonetics;
using VotraxSpeak.Speech;
using VotraxSpeak.Synthesis;

namespace VotraxSpeak;

/// <summary>
/// Entry point and composition root.
/// </summary>
internal static class Program
{
    private static int Main(string[] args)
    {
        CommandLineParseResult parsed = CommandLineParser.Parse(args);

        if (parsed.Options is null)
        {
            Console.Error.WriteLine(parsed.Error);
            Console.Error.WriteLine(UsageText.HelpHint);
            return (int)ExitCode.InvalidArguments;
        }

        if (parsed.Options.ShowHelp)
        {
            Console.Out.Write(UsageText.Text);
            return (int)ExitCode.Success;
        }

        return (int)Speak(parsed.Options);
    }

    private static ExitCode Speak(CommandLineOptions options)
    {
        try
        {
            TextToSpeechPipeline pipeline = CreatePipeline(options);

            if (options.Phonemes is null)
            {
                pipeline.Speak(options.Text, options.Voice);
            }
            else
            {
                pipeline.Speak(options.Phonemes, options.Voice);
            }

            return ExitCode.Success;
        }
        catch (Exception exception) when (exception is RomNotFoundException or InvalidRomException)
        {
            return Fail(exception, ExitCode.RomUnavailable);
        }
        catch (Exception exception) when (exception is AudioPlaybackException or IOException or UnauthorizedAccessException)
        {
            return Fail(exception, ExitCode.OutputFailed);
        }
    }

    private static TextToSpeechPipeline CreatePipeline(CommandLineOptions options)
    {
        var romLocator = new RomLocator([AppContext.BaseDirectory, Environment.CurrentDirectory]);
        PhonemeRom rom = PhonemeRomLoader.Load(romLocator.Locate(options.RomPath));

        IAudioOutput output = options.OutputFile is null
            ? new SpeakerOutput()
            : FileOutputFactory.Create(options.OutputFile);

        return new TextToSpeechPipeline(
            new EnglishTextToPhonemeConverter(new LetterToSoundTranslator(EnglishRules.All)),
            new VotraxSpeechRenderer(rom),
            output);
    }

    private static ExitCode Fail(Exception exception, ExitCode exitCode)
    {
        Console.Error.WriteLine(exception.Message);
        return exitCode;
    }
}

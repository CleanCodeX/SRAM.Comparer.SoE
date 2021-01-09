using System;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using Common.Shared.Min.Extensions;
using SramComparer.Helpers;
using SramComparer.SoE.Properties;
using SramComparer.SoE.Services;

namespace SramComparer.SoE
{
	public static class Program
	{
		private static readonly string DefaultConfigFileName = CommandHandlerSoE.DefaultConfigFileName;

		[ModuleInitializer]
		public static void InitializeServices()
		{
			ServiceCollection.ConsolePrinter = new ConsolePrinterSoE();
			ServiceCollection.CmdLineParser = new CmdLineParserSoE();
			ServiceCollection.CommandHandler = new CommandHandlerSoE();
		}

		public static int Main(string[] args)
		{
			ConsoleHelper.RedefineConsoleColors(bgColor: Color.FromArgb(17, 17, 17));

			var consolePrinter = ServiceCollection.ConsolePrinter;

			var cmdLineParser = ServiceCollection.CmdLineParser;
			cmdLineParser.ThrowIfNull(nameof(cmdLineParser));

			var loadedConfigFile = File.Exists(DefaultConfigFileName) ? DefaultConfigFileName : null;

			var options = cmdLineParser.Parse(args, GetDefaultConfigOrNew());
			if (options.ConfigFilePath is { } configFile && configFile != DefaultConfigFileName)
			{
				options = JsonFileSerializer.Deserialize<Options>(configFile);
				loadedConfigFile = configFile;
			}

			consolePrinter.ColorizeOutput = options.ColorizeOutput;

			if (loadedConfigFile is not null)
			{
				consolePrinter.PrintSectionHeader();
				consolePrinter.PrintColoredLine(ConsoleColor.Green,
					Resources.StatusConfigFileHasBeenLoadedTemplate.InsertArgs(loadedConfigFile));
				consolePrinter.ResetColor();
			}

			if (options.BatchCommands is null)
				CommandMenu.Instance.Show(options);
			else
				CommandQueue.Instance.Start(options);

			return 0;
		}
		
		private static IOptions GetDefaultConfigOrNew() => File.Exists(DefaultConfigFileName) ? JsonFileSerializer.Deserialize<Options>(DefaultConfigFileName) : new Options();
	}
}

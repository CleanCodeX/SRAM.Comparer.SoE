using System;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using Common.Shared.Min.Extensions;
using SRAM.Comparison.Helpers;
using SRAM.Comparison.Services;
using SRAM.Comparison.SoE.Services;
using Resources = SRAM.Comparison.Properties.Resources;

namespace SRAM.Comparison.SoE
{
	public static class Program
	{
		private static readonly string DefaultConfigFileName = CommandHandler.DefaultConfigFileName;

		[ModuleInitializer]
		public static void InitializeServices()
		{
			ServiceCollection.ConsolePrinter = new ConsolePrinterSoE();
			ServiceCollection.CmdLineParser = new CmdLineParserSoE();
			ServiceCollection.CommandHandler = new CommandHandlerSoE();
		}

		public static int Main(string[] args)
		{
			var consolePrinter = ServiceCollection.ConsolePrinter;

			try
			{
				ConsoleHelper.RedefineConsoleColors(bgColor: Color.FromArgb(17, 17, 17));

				var cmdLineParser = ServiceCollection.CmdLineParser;
				cmdLineParser.ThrowIfNull(nameof(cmdLineParser));

				IOptions options = null!;
				string? configToLoad = null;
				var cmdParser = new CmdLineParserSoE();

				if (File.Exists(DefaultConfigFileName))
					configToLoad = DefaultConfigFileName;
				else
				{
					options = cmdParser.Parse(args);

					if (options.ConfigPath is { } configFile)
						configToLoad = configFile;
				}

				if (configToLoad is not null)
				{
					try
					{
						var loadedConfig = JsonFileSerializer.Deserialize<Options>(configToLoad);
						options = cmdParser.Parse(args, loadedConfig);
					}
					catch (Exception ex)
					{
						consolePrinter.PrintError(ex);
						options = cmdParser.Parse(args);
					}
				}

				consolePrinter.ColorizeOutput = options.ColorizeOutput;

				if (configToLoad is not null)
				{
					consolePrinter.PrintSectionHeader();
					consolePrinter.PrintColoredLine(ConsoleColor.Green,
						Resources.StatusConfigFileLoadedTemplate.InsertArgs(configToLoad));
					consolePrinter.ResetColor();
				}

				if (options.BatchCommands is null)
					CommandMenu.Instance.Show(options);
				else
					CommandQueue.Instance.Start(options);

			}
			catch (Exception ex)
			{
				consolePrinter.PrintFatalError(ex.Message);
				Console.ReadKey();
			}
			
			return 0;
		}
	}
}

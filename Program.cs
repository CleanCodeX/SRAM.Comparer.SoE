using System;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using Common.Shared.Min.Extensions;
using SRAM.Comparison.Helpers;
using SRAM.Comparison.SoE.Properties;
using SRAM.Comparison.SoE.Services;

namespace SRAM.Comparison.SoE
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
			var consolePrinter = ServiceCollection.ConsolePrinter;

			try
			{
				ConsoleHelper.RedefineConsoleColors(bgColor: Color.FromArgb(17, 17, 17));

				var cmdLineParser = ServiceCollection.CmdLineParser;
				cmdLineParser.ThrowIfNull(nameof(cmdLineParser));

				var loadedConfigFile = File.Exists(DefaultConfigFileName) ? DefaultConfigFileName : null;

				var options = cmdLineParser.Parse(args, GetDefaultConfigOrNew());
				if (options.ConfigPath is { } configFile && configFile != DefaultConfigFileName)
				{
					options = JsonFileSerializer.Deserialize<Options>(configFile);
					loadedConfigFile = configFile;
				}

				consolePrinter.ColorizeOutput = options.ColorizeOutput;

				if (loadedConfigFile is not null)
				{
					consolePrinter.PrintSectionHeader();
					consolePrinter.PrintColoredLine(ConsoleColor.Green,
						Resources.StatusConfigFileLoadedTemplate.InsertArgs(loadedConfigFile));
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
		
		private static IOptions GetDefaultConfigOrNew() => File.Exists(DefaultConfigFileName) 
			? JsonFileSerializer.Deserialize<Options>(DefaultConfigFileName) 
			: new Options();
	}
}

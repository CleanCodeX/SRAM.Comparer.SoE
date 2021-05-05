using System;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using Common.Shared.Min.Extensions;
using SRAM.Comparison.Helpers;
using SRAM.Comparison.Properties;
using SRAM.Comparison.Services;
using SRAM.Comparison.SoE.Services;

namespace SRAM.Comparison.SoE
{
	public static class Program
	{
		private static readonly string DefaultConfigFileName = CommandHandler.DefaultConfigFileName;
		private static IConsolePrinter ConsolePrinter => ComparisonServices.ConsolePrinter;

		[ModuleInitializer]
		public static void InitializeServices()
		{
			ComparisonServices.ConsolePrinter = new ConsolePrinterSoE();
			ComparisonServices.CmdLineParser = new CmdLineParserSoE();
			ComparisonServices.CommandHandler = new CommandHandlerSoE();
		}

		public static int Main(string[] args)
		{
			try
			{
#pragma warning disable CA1416
				ConsoleHelper.RedefineConsoleColors(bgColor: Color.FromArgb(17, 17, 17));
#pragma warning restore CA1416 
				
				var cmdLineParser = ComparisonServices.CmdLineParser;
				cmdLineParser.ThrowIfNull(nameof(cmdLineParser));

				IOptions options = null!;
				string? configToLoad = null;
				CmdLineParserSoE cmdParser = new();

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
						ConsolePrinter.PrintError(ex);
						options = cmdParser.Parse(args);
					}
				}

				ConsolePrinter.ColorizeOutput = options.ColorizeOutput;
				
				if (configToLoad is not null)
				{
					ConsolePrinter.PrintSectionHeader();
					ConsolePrinter.PrintColoredLine(ConsoleColor.Green,
						Resources.StatusConfigFileLoadedTemplate.InsertArgs(configToLoad));
					ConsolePrinter.ResetColor();
				}

				if (options.BatchCommands is null)
					CommandMenu.Instance.Show(options);
				else
					CommandQueue.Instance.Start(options);

			}
			catch (Exception ex)
			{
				ConsolePrinter.PrintFatalError(ex.Message + Environment.NewLine + ex.StackTrace);

				try
				{
					Console.ReadKey();
				}
				catch
				{
					// Ignore
				}
			}
			
			return 0;
		}
	}
}

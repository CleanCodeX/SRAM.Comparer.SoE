using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Common.Shared.Min.Extensions;
using SramComparer.SoE.Services;

namespace SramComparer.SoE
{
	public static class Program
	{
		[ModuleInitializer]
		public static void InitializeServices()
		{
			ServiceCollection.ConsolePrinter = new ConsolePrinterSoE();
			ServiceCollection.CmdLineParser = new CmdLineParserSoE();
			ServiceCollection.CommandHandler = new CommandHandlerSoE();
		}

		public static int Main(string[] args)
		{
			var cmdLineParser = ServiceCollection.CmdLineParser;
			cmdLineParser.ThrowIfNull(nameof(cmdLineParser));
			
			var options = cmdLineParser.Parse(args);

			if (options.ConfigFilePath is not null && Path.GetExtension(options.ConfigFilePath).ToLower() == ".json")
			{
				var json = File.ReadAllText(options.ConfigFilePath);
				options = JsonSerializer.Deserialize<Options>(json)!;
			}

			ServiceCollection.ConsolePrinter.ColorizeOutput = options.ColorizeOutput;

			if (options.BatchCommands is null)
				CommandMenu.Instance.Show(options);
			else
				CommandQueue.Instance.Start(options);

			return 0;
		}
	}
}

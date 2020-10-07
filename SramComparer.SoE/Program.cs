using System.Runtime.CompilerServices;
using App.Commons.Extensions;
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
            if (options.Commands is null)
                CommandMenu.Instance.Run(options);
            else
                CommandQueue.Instance.Run(options);

            return 0;
		}
    }
}

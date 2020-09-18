using App.Commons.Extensions;
using SramComparer.SoE.Helpers;
using SramComparer.SoE.Services;
// ReSharper disable AccessToStaticMemberViaDerivedType
// ReSharper disable PossibleMultipleEnumeration

namespace SramComparer.SoE
{
    public static class Program
	{
        static Program()
        {
            ServiceCollection.CmdLineParser = new CmdLineParserSoE();
            ServiceCollection.CommandExecutor = new CommandExecutorSoE();
            ServiceCollection.ConsolePrinter = new ConsolePrinterSoE();
        }

		public static int Main(string[] args)
        {
            var cmdLineParser = ServiceCollection.CmdLineParser;
            cmdLineParser.ThrowIfNull(nameof(cmdLineParser));

            var options = cmdLineParser.Parse(args);
		    var isCommandMode = options.Commands is not null;

            if (isCommandMode)
                CommandQueue.Instance.Run(options);
            else
                CommandMenu.Instance.Run(options);

		    return 0;
		}
	}
}

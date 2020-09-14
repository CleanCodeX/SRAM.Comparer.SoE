using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Commons.Extensions;
using SramComparer.SoE.Helpers;
using SramComparer.SoE.Properties;
using Res = SramComparer.Properties.Resources;
// ReSharper disable AccessToStaticMemberViaDerivedType
// ReSharper disable PossibleMultipleEnumeration

namespace SramComparer.SoE
{
    public class Program
	{
		public static int Main(string[] args)
		{
		    var options = CmdLineParser.Parse(args);
		    var isCommandMode = options.Commands is not null;

			if (options.CurrentGameFilepath.IsNullOrEmpty())
			{
				ConsolePrinter.PrintFatalError(Res.ErrorMissingPathArguments);
				return 0;
			}

			var commands = options.Commands?.Split('-') ?? Enumerable.Empty<string>();
			var queuedCommands = new Queue<string>(commands);

			if (isCommandMode)
				Console.WriteLine(@$"{Resources.QueuedCommands}: {queuedCommands.Count} ({string.Join(", ", commands)})");
			else
			{
				ConsolePrinter.SetInitialConsoleSize();
				ConsolePrinter.PrintSettings(options);
				ConsolePrinter.PrintCommands();
			}

			while (true)
		    {
			    try
			    {
				    string? command;
				    if (isCommandMode && queuedCommands.Count > 0)
					    queuedCommands.TryDequeue(out command);
				    else
					    command = Console.ReadLine();

					if (CommandHelper.InternalRunCommand(command, options) == false)
					    break;

#if !DEBUG
					if (isCommandMode && queuedCommands.Count == 0)
					    break;
#endif
			    }
			    catch (IOException ex)
			    {
				    ConsolePrinter.PrintError(ex.Message);
			    }
			    catch (Exception ex)
			    {
				    ConsolePrinter.PrintError(ex);
			    }
            }

		    return 0;
		}
	}
}

using System;
using System.IO;
using App.Commons.Extensions;
using SramCommons.SoE.Helpers;
using SramComparer.SoE.Helpers;
using SramComparer.SoE.Helpers.Enums;
using SramComparer.Helpers.Enums;
using Res = SramComparer.Properties.Resources;
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable AccessToStaticMemberViaDerivedType

namespace SramComparer.SoE
{
	internal class Program
	{
		public static int Main(string[] args)
		{
		    Console.SetWindowSize(130, 50);
		    Console.BufferHeight = 1000;

		    var options = CmdLineParser.Parse(args);
		    ConsolePrinter.PrintSettings(options);

		    if (string.IsNullOrEmpty(options.CurrentGameFilepath) || string.IsNullOrEmpty(options.ComparisonGameFilepath))
		    {
				Console.WriteLine();
				Console.BackgroundColor = ConsoleColor.Red;
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine(Res.ErrorMissingPathArguments);
				Console.ResetColor();

				ConsolePrinter.PrintManual();

				Console.ReadKey();

				return 0;
		    }

		    ConsolePrinter.PrintCommands();

		    while (true)
		    {
			    try
			    {
				    var command = Console.ReadLine();

				    switch (command)
				    {
					    case nameof(Commands.cmd):
						    ConsolePrinter.PrintCommands();
						    break;
					    case nameof(Commands.s):
						    ConsolePrinter.PrintSettings(options);
						    break;
					    case nameof(Commands.m):
						    ConsolePrinter.PrintManual();
						    break;
					    case nameof(Commands.fwg):
						    CommandHelper.InvertIncludeFlag(ref options.Flags, ComparisonFlags.WholeGameBuffer);
						    break;
					    case nameof(Commands.fng):
						    CommandHelper.InvertIncludeFlag(ref options.Flags, ComparisonFlags.NonGameBuffer);
						    break;
					    case nameof(Commands.fu12b):
					    case nameof(Commands.fu12ba):
						    CommandHelper.InvertIncludeFlag(ref options.Flags, command == nameof(Commands.fu12ba) ? ComparisonFlags.AllUnknown12Bs : ComparisonFlags.Unknown12B);
						    break;
					    case nameof(Commands.fc):
					    case nameof(Commands.fca):
						    CommandHelper.InvertIncludeFlag(ref options.Flags, command == nameof(Commands.fca) ? ComparisonFlags.AllGameChecksums : ComparisonFlags.GameChecksum);
						    break;
					    case nameof(Commands.sg):
						    options.Game = CommandHelper.GetGameId();
						    if (options.Game == GameId.All)
							    options.ComparisonGame = GameId.All;

						    break;
					    case nameof(Commands.scg):
						    if (options.Game != GameId.All)
							    options.ComparisonGame = CommandHelper.GetGameId();
						    else
							    ConsolePrinter.PrintError(Res.ErrorComparisoGameSetButNotGame);

						    break;
					    case nameof(Commands.c):
						    CommandHelper.CompareFiles(options);
						    break;
					    case nameof(Commands.ow):
						    CommandHelper.OverwriteComparisonFileWithCurrentFile(options);
						    break;
					    case nameof(Commands.b):
						    CommandHelper.BackupSramFile(options, SramFileKind.Curr, false);
						    break;
					    case nameof(Commands.r):
						    CommandHelper.BackupSramFile(options, SramFileKind.Curr, true);
						    break;
					    case nameof(Commands.bc):
						    CommandHelper.BackupSramFile(options, SramFileKind.Comp, false);
						    break;
					    case nameof(Commands.rc):
						    CommandHelper.BackupSramFile(options, SramFileKind.Comp, true);
						    break;
					    case nameof(Commands.e):
						    CommandHelper.ExportCurrentComparison(options);
						    break;
					    case nameof(Commands.w):
						    Console.Clear();
						    ConsolePrinter.PrintCommands();
						    break;
					    case nameof(Commands.q):
						    return 0;
					    default:
						    ConsolePrinter.PrintError(Res.ErrorNoValidCommand.InsertArgs(command));

						    break;
				    }
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
        }
    }
}

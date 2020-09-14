using System;
using System.IO;
using App.Commons.Extensions;
using SramCommons.SoE;
using SramCommons.SoE.Models.Structs;
using SramComparer.Helpers;
using SramComparer.Helpers.Enums;
using SramComparer.Properties;
using SramComparer.SoE.Helpers.Enums;
// ReSharper disable RedundantArgumentDefaultValue

namespace SramComparer.SoE.Helpers
{
    public class CommandHelper: CommandHelperBase<SramFile, SramGame>
    {
        public static void CompareFiles(IOptions options) => CompareFiles<SramComparer>(options);
        public static void ExportCurrentComparison(IOptions options) => ExportCurrentComparison<SramComparer>(options);

        public static void InvertIncludeFlag(ref ComparisonFlags flags, ComparisonFlags flag) =>
            CommandHelperBase<SramFile, SramGame>.InvertIncludeFlag(ref flags, flag);

        public static bool RunCommand(string command, Options options, TextWriter? outStream = null)
        {
	        ConsolePrinter.SetInitialConsoleSize();

	        var defaultStream = Console.Out;

	        if (outStream is not null)
		        Console.SetOut(outStream);

	        if (options.CurrentGameFilepath.IsNullOrEmpty())
	        {
		        ConsolePrinter.PrintFatalError(Resources.ErrorMissingPathArguments);
		        return false;
	        }

	        try
	        {
		        return InternalRunCommand(command, options) is not null;
	        }
	        finally
	        {
		        if (outStream is not null)
			        Console.SetOut(defaultStream);
	        }
        }

		internal static bool? InternalRunCommand(string? command, Options options)
		{
			switch (command)
			{
				case "":
					return true;
				case nameof(Commands.cmd):
					ConsolePrinter.PrintCommands();
					return true;
				case nameof(Commands.s):
					ConsolePrinter.PrintSettings(options);
					return true;
				case nameof(Commands.m):
					ConsolePrinter.PrintManual();
					return true;
				case nameof(Commands.fwg):
					InvertIncludeFlag(ref options.Flags, ComparisonFlags.WholeGameBuffer);
					return true;
				case nameof(Commands.fng):
					InvertIncludeFlag(ref options.Flags, ComparisonFlags.NonGameBuffer);
					return true;
				case nameof(Commands.fu12b):
				case nameof(Commands.fu12ba):
					InvertIncludeFlag(ref options.Flags,
						command == nameof(Commands.fu12ba) ? ComparisonFlags.AllUnknown12Bs : ComparisonFlags.Unknown12B);
					return true;
				case nameof(Commands.fc):
				case nameof(Commands.fca):
					InvertIncludeFlag(ref options.Flags,
						command == nameof(Commands.fca) ? ComparisonFlags.AllGameChecksums : ComparisonFlags.GameChecksum);
					return true;
				case nameof(Commands.sg):
					options.Game = GetGameId(maxGameId: 4);
					if (options.Game == default)
						options.ComparisonGame = default;

					return true;
				case nameof(Commands.scg):
					if (options.Game != default)
						options.ComparisonGame = GetGameId(maxGameId: 4);
					else
						ConsolePrinterBase.PrintError(Resources.ErrorComparisoGameSetButNotGame);

					return true;
				case nameof(Commands.c):
					CompareFiles(options);
					return true;
				case nameof(Commands.ow):
					OverwriteComparisonFileWithCurrentFile(options);
					return true;
				case nameof(Commands.b):
					BackupSramFile(options, SramFileKind.Current, false);
					return true;
				case nameof(Commands.r):
					BackupSramFile(options, SramFileKind.Current, true);
					return true;
				case nameof(Commands.bc):
					BackupSramFile(options, SramFileKind.Comparison, false);
					return true;
				case nameof(Commands.rc):
					BackupSramFile(options, SramFileKind.Comparison, true);
					return true;
				case nameof(Commands.e):
					ExportCurrentComparison(options);
					return true;
				case nameof(Commands.w):
					Console.Clear();
					ConsolePrinter.PrintCommands();
					return true;
				case nameof(Commands.q):
					return false;
				default:
					ConsolePrinterBase.PrintError(Resources.ErrorNoValidCommand.InsertArgs(command));

					return true;
			}
		}
    }
}
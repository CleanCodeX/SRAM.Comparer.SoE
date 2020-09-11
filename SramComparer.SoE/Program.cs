using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using App.Commons.Extensions;
using SramComparer.Helpers.Enums;
using SramComparer.SoE.Helpers;
using SramComparer.SoE.Helpers.Enums;
using SramComparer.SoE.Properties;
using Res = SramComparer.Properties.Resources;
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable AccessToStaticMemberViaDerivedType

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

			if (!isCommandMode)
			{
				SetInitialConsoleSize();
				ConsolePrinter.PrintSettings(options);
				ConsolePrinter.PrintCommands();
			}

			var commands = options.Commands?.Split('-') ?? Enumerable.Empty<string>();
			var queuedCommands = new Queue<string>(commands);

			if(isCommandMode)
				Console.WriteLine($"{Resources.QueuedCommands}: {queuedCommands.Count} ({options.Commands})");

			while (true)
		    {
			    try
			    {
				    string? command;
				    if (isCommandMode)
					    queuedCommands.TryDequeue(out command);
				    else
					    command = Console.ReadLine();

					if (InternalRunCommand(command, options) == false)
					    break;

				    if (isCommandMode && queuedCommands.Count == 0)
					    break;
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

		public static bool RunCommand(string command, Options options, TextWriter? outStream = null)
		{
			SetInitialConsoleSize();

			var defaultStream = Console.Out;

			if (outStream is not null)
				Console.SetOut(outStream);

			if (options.CurrentGameFilepath.IsNullOrEmpty())
			{
				ConsolePrinter.PrintFatalError(Res.ErrorMissingPathArguments);
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

		private static bool? InternalRunCommand(string? command, Options options)
		{
			switch (command)
			{
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
					CommandHelper.InvertIncludeFlag(ref options.Flags, ComparisonFlags.WholeGameBuffer);
					return true;
				case nameof(Commands.fng):
					CommandHelper.InvertIncludeFlag(ref options.Flags, ComparisonFlags.NonGameBuffer);
					return true;
				case nameof(Commands.fu12b):
				case nameof(Commands.fu12ba):
					CommandHelper.InvertIncludeFlag(ref options.Flags,
						command == nameof(Commands.fu12ba) ? ComparisonFlags.AllUnknown12Bs : ComparisonFlags.Unknown12B);
					return true;
				case nameof(Commands.fc):
				case nameof(Commands.fca):
					CommandHelper.InvertIncludeFlag(ref options.Flags,
						command == nameof(Commands.fca) ? ComparisonFlags.AllGameChecksums : ComparisonFlags.GameChecksum);
					return true;
				case nameof(Commands.sg):
					options.Game = CommandHelper.GetGameId(maxGameId: 4);
					if (options.Game == default)
						options.ComparisonGame = default;

					return true;
				case nameof(Commands.scg):
					if (options.Game != default)
						options.ComparisonGame = CommandHelper.GetGameId(maxGameId: 4);
					else
						ConsolePrinter.PrintError(Res.ErrorComparisoGameSetButNotGame);

					return true;
				case nameof(Commands.c):
					CommandHelper.CompareFiles(options);
					return true;
				case nameof(Commands.ow):
					CommandHelper.OverwriteComparisonFileWithCurrentFile(options);
					return true;
				case nameof(Commands.b):
					CommandHelper.BackupSramFile(options, SramFileKind.Current, false);
					return true;
				case nameof(Commands.r):
					CommandHelper.BackupSramFile(options, SramFileKind.Current, true);
					return true;
				case nameof(Commands.bc):
					CommandHelper.BackupSramFile(options, SramFileKind.Comparison, false);
					return true;
				case nameof(Commands.rc):
					CommandHelper.BackupSramFile(options, SramFileKind.Comparison, true);
					return true;
				case nameof(Commands.e):
					CommandHelper.ExportCurrentComparison(options);
					return true;
				case nameof(Commands.w):
					Console.Clear();
					ConsolePrinter.PrintCommands();
					return true;
				case nameof(Commands.q):
					return false;
				default:
					ConsolePrinter.PrintError(Res.ErrorNoValidCommand.InsertArgs(command));

					return true;
			}
		}

		private static void SetInitialConsoleSize()
		{
			try
			{
				Console.SetWindowSize(130, 50);
				Console.BufferHeight = 1000;
			}
			catch
			{
				// Ignore
			}
		}
	}
}

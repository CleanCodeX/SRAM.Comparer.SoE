using System;
using System.IO;
using App.Commons.Extensions;
using SramCommons.SoE.Helpers;
using SramCommons.SoE.Models.Enums;
using SramComparer.Extensions;
using SramComparer.Helpers;
using SramComparer.SoE.Helpers.Enums;
using Res = SramComparer.Properties.Resources;
using ResSoE = SramComparer.SoE.Properties.Resources;

namespace SramComparer.SoE.Helpers
{
    public class ConsolePrinter : ConsolePrinterBase
    {
        public static void PrintSettings(IOptions options)
        {
            WriteNewSectionHeader();
            Console.WriteLine(Res.Settings + @":");

            WriteSettingName(Res.SettingCurrentGameFilepath, "{0}");
            WriteValue(Path.GetFileName(options.CurrentGameFilepath));

            WriteSettingName(Res.SettingComparisonGameFilepath, CmdOptions.ComparisonFile);
            WriteValue(Path.GetFileName(options.ComparisonGameFilepath));

            WriteSettingName(Res.SettingExportDirectory, CmdOptions.Exportdir);
            WriteValue(options.ExportDirectory);

            WriteSettingName(Res.SettingCurrentGameToCompare, $"{CmdOptions.Game} [1-4|{Res.All}]");
            WriteValue(Equals(options.Game, default(GameId)) ? Res.All : options.Game.ToInt().ToString());

            WriteSettingName(Res.SettingComparisonGameToCompare, $"{CmdOptions.ComparisonGame} [1-4|{Res.All}]");
            WriteValue(Equals(options.ComparisonGame, default(GameId)) ? Res.SameAsCurrentGame : options.ComparisonGame.ToInt().ToString());

            WriteSettingName(Res.SettingRegion, $"{CmdOptions.Region} [{string.Join("|", Enum.GetNames(typeof(FileRegion)))}]");
            WriteValue(options.Region.ToString());

            WriteSettingName(Res.ComparisonFlags, $"{CmdOptions.ComparisonFlags} [{string.Join(",", Enum.GetNames(typeof(ComparisonFlags)))}]");
            WriteValue(Environment.NewLine.PadRight(30) + @$": ""{options.Flags.ToFlagsString()}""");
        }

        public static void PrintCommands()
        {
            WriteNewSectionHeader();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine();
            Console.WriteLine("=".Repeat(50));
            Console.WriteLine(@"== " + Res.NewUserAdviceCommandTemplate.InsertArgs(nameof(Commands.m)));
            Console.WriteLine("=".Repeat(50));
            Console.WriteLine();

            PrintGroupName(Res.CmdGroupComparison);

            PrintCommandKey(Commands.c);
            Console.WriteLine(Res.CommandCompareFiles);

            PrintCommandKey(Commands.ow);
            Console.WriteLine(Res.CommandOverwriteComparisonFile);

            PrintGroupName(Res.CmdGroupSetGame);

            PrintCommandKey(Commands.sg);
            Console.WriteLine(Res.CommandSetGame);

            PrintCommandKey(Commands.scg);
            Console.WriteLine(Res.CommandSetComparisonGame);

            PrintGroupName(Res.CmdGroupSetFlags);

            PrintCommandKey(Commands.fwg);
            Console.WriteLine(Res.CommandIncludeWholeGameBufferComparison);

            PrintCommandKey(Commands.fng);
            Console.WriteLine(Res.CommandIncludeNonGameBufferComparison);

            PrintCommandKey(Commands.fc);
            Console.WriteLine(ResSoE.CommandIncludeChecksum);

            PrintCommandKey(Commands.fca);
            Console.WriteLine(ResSoE.CommandIncludeAllChecksums);

            PrintCommandKey(Commands.fu12b);
            Console.WriteLine(ResSoE.CommandIncludeUnknown12B);

            PrintCommandKey(Commands.fu12ba);
            Console.WriteLine(ResSoE.CommandIncludeAllUnknown12Bs);

            PrintGroupName(Res.CmdGroupBackup);

            PrintCommandKey(Commands.b);
            Console.WriteLine(Res.CommandBackupCurrentFile);

            PrintCommandKey(Commands.bc);
            Console.WriteLine(Res.CommandBackupComparisonFile);

            PrintCommandKey(Commands.r);
            Console.WriteLine(Res.CommandRestoreCurrentFile);

            PrintCommandKey(Commands.rc);
            Console.WriteLine(Res.CommandRestoreComparisonFile);

            PrintCommandKey(Commands.e);
            Console.WriteLine(Res.CommandExportComparisonResult);

            PrintGroupName(Res.CmdGroupDisplay);

            PrintCommandKey(Commands.m);
            Console.WriteLine(Res.CommandManual);

            PrintCommandKey(Commands.cmd);
            Console.WriteLine(Res.CommandDisplayCommands);

            PrintCommandKey(Commands.s);
            Console.WriteLine(Res.CommandDisplaySettings);

            PrintCommandKey(Commands.w);
            Console.WriteLine(Res.CommandWipeOutput);

            Console.WriteLine();
            PrintCommandKey(Commands.q);
            Console.WriteLine(Res.CommandQuit);
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Res.EnterCommand);
            Console.ResetColor();

            static void PrintCommandKey(Commands key)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(@$"{key,12}: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            static void PrintGroupName(string groupName)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(Environment.NewLine + groupName);
            }
        }

        public static void PrintManual()
        {
            WriteNewSectionHeader();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Res.AppDescription);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine();
            Console.WriteLine(ResSoE.AppManualCommandsTemplate.InsertArgs(
                Commands.ow, Commands.c, Commands.e, Commands.sg, Commands.scg, Commands.fwg, Commands.fng, 
                Commands.b, Commands.bc, Commands.r, Commands.rc));
            Console.ResetColor();
        }
    }
}
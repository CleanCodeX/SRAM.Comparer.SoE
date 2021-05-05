using System;
using Common.Shared.Min.Extensions;
using SoE.Models.Enums;
using SoE.Models.Structs;
using SRAM.Comparison.Services;
using SRAM.Comparison.SoE.Properties;
using ResSoE = SoE.Properties.Resources;

namespace SRAM.Comparison.SoE.Helpers
{
	public class TerminalCodePrinter
	{
		public const int EmptySaveSlotValue = 24672;

		public static void PrintTerminalCodes(IConsolePrinter consolePrinter, TerminalCode alarmCode, TerminalCode secretCode, ConsoleColor overridingColor = default)
		{
			consolePrinter.PrintSectionHeader();

			if (alarmCode.IsDefault)
				consolePrinter.PrintColoredLine(ConsoleColor.Cyan, ResSoE.StatusNoTerminalCodeSet);
			else if (alarmCode.Code1.ToUShort() == EmptySaveSlotValue)
				consolePrinter.PrintColoredLine(ConsoleColor.Yellow, Resources.StatusSaveslotIsEmpty);
			else
			{
				consolePrinter.PrintColored(ConsoleColor.White, $"{Resources.AlarmCode}: ");
				if (alarmCode.IsValid)
					PrintValidCode(consolePrinter, alarmCode, overridingColor);
				else
					PrintInvalidCode(alarmCode);

				consolePrinter.PrintColored(ConsoleColor.White, $"{Resources.SecretBossRoomCode}: ");
				if (secretCode.IsValid)
					PrintValidCode(consolePrinter, secretCode, overridingColor);
				else
					PrintInvalidCode(secretCode);
			}

			consolePrinter.ResetColor();

			void PrintInvalidCode(TerminalCode alarmCode1) =>
				consolePrinter.PrintColoredLine(ConsoleColor.Red,
					$"{ResSoE.StatusInvalidTerminalCode} ({alarmCode1.Code1}-{alarmCode1.Code2}-{alarmCode1.Code3})");
		}

		public static void PrintValidCode(IConsolePrinter consolePrinter, TerminalCode terminalCode, ConsoleColor overridingColor = default)
		{
			FormatCodeColor(consolePrinter, terminalCode.Code1, overridingColor);
			consolePrinter.PrintColored(ConsoleColor.White, "-");
			FormatCodeColor(consolePrinter, terminalCode.Code2, overridingColor);
			consolePrinter.PrintColored(ConsoleColor.White, "-");
			FormatCodeColor(consolePrinter, terminalCode.Code3, overridingColor);

			consolePrinter.PrintColored(ConsoleColor.White, " | ");

			FormatCodeNumber(consolePrinter, terminalCode.Code1, overridingColor);
			consolePrinter.PrintColored(ConsoleColor.White, "-");
			FormatCodeNumber(consolePrinter, terminalCode.Code2, overridingColor);
			consolePrinter.PrintColored(ConsoleColor.White, "-");
			FormatCodeNumber(consolePrinter, terminalCode.Code3, overridingColor);

			consolePrinter.PrintLine();
		}

		private static ConsoleColor GetTerminalCodeColor(TerminalCodeColor color) =>
			color switch
			{
				TerminalCodeColor.Blue => ConsoleColor.Cyan,
				TerminalCodeColor.Green => ConsoleColor.Green,
				_ => ConsoleColor.Red,
			};

		private static void FormatCodeColor(IConsolePrinter consolePrinter, TerminalCodeColor color, ConsoleColor overridingColor) =>
			consolePrinter.PrintColored(overridingColor != default ? overridingColor : GetTerminalCodeColor(color), color.GetDisplayName()!);

		private static void FormatCodeNumber(IConsolePrinter consolePrinter, TerminalCodeColor color, ConsoleColor overridingColor) =>
			consolePrinter.PrintColored(overridingColor != default ? overridingColor : GetTerminalCodeColor(color), color.ToUShort().ToString());
	}
}
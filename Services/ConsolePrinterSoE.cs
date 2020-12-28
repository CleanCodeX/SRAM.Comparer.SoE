using System;
using Common.Shared.Min.Extensions;
using SramComparer.Services;
using SramComparer.SoE.Enums;

namespace SramComparer.SoE.Services
{
	/// <summary>Console printer implementation for SoE</summary>
	/// <inheritdoc cref="ConsolePrinter"/>
	public class ConsolePrinterSoE : ConsolePrinter
	{
		protected override void PrintCustomCommands()
		{
			PrintGroupName(@"Secret Of Evermore");

			PrintCommandKey(CommandsSoE.cs);
			PrintColoredLine(ConsoleColor.Yellow, CommandsSoE.cs.GetDisplayName()!);

			PrintCommandKey(CommandsSoE.csa);
			PrintColoredLine(ConsoleColor.Yellow, CommandsSoE.csa.GetDisplayName()!);

			PrintCommandKey(CommandsSoE.u12b);
			PrintColoredLine(ConsoleColor.Yellow, CommandsSoE.u12b.GetDisplayName()!);

			PrintCommandKey(CommandsSoE.u12ba);
			PrintColoredLine(ConsoleColor.Yellow, CommandsSoE.u12ba.GetDisplayName()!);
		}
	}
}
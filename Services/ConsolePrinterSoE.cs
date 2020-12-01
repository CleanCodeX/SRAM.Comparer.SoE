using System;
using SramComparer.Services;
using SramComparer.SoE.Enums;
using SramComparer.SoE.Properties;

namespace SramComparer.SoE.Services
{
	/// <summary>Console printer implementation for SoE</summary>
	/// <inheritdoc cref="ConsolePrinter"/>
	public class ConsolePrinterSoE : ConsolePrinter
	{
		protected override void PrintCustomCommands()
		{
			PrintGroupName(@"Secret Of Evermore");

			PrintCommandKey(Commands.fc);
			PrintColoredLine(ConsoleColor.Yellow, Resources.CommandIncludeChecksum);

			PrintCommandKey(Commands.fca);
			PrintColoredLine(ConsoleColor.Yellow, Resources.CommandIncludeAllChecksums);

			PrintCommandKey(Commands.fu12b);
			PrintColoredLine(ConsoleColor.Yellow, Resources.CommandIncludeUnknown12B);

			PrintCommandKey(Commands.fu12ba);
			PrintColoredLine(ConsoleColor.Yellow, Resources.CommandIncludeAllUnknown12Bs);
		}
	}
}
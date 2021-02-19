using System;
using System.Globalization;
using System.IO;
using Common.Shared.Min.Extensions;
using SRAM.Comparison.Services;
using SRAM.Comparison.SoE.Enums;

namespace SRAM.Comparison.SoE.Services
{
	/// <summary>Console printer implementation for SoE</summary>
	/// <inheritdoc cref="ConsolePrinter"/>
	public class ConsolePrinterSoE : ConsolePrinter
	{
		protected override void PrintCustomCommands()
		{
			PrintGroupName(@"Secret Of Evermore");

			PrintCommandKey(CommandsSoE.Checksum);
			PrintColoredLine(ConsoleColor.Yellow, CommandsSoE.Checksum.GetDisplayName()!);

			PrintCommandKey(CommandsSoE.ChecksumDiff);
			PrintColoredLine(ConsoleColor.Yellow, CommandsSoE.ChecksumDiff.GetDisplayName()!);

			PrintCommandKey(CommandsSoE.EventTimer);
			PrintColoredLine(ConsoleColor.Yellow, CommandsSoE.EventTimer.GetDisplayName()!);

			PrintCommandKey(CommandsSoE.EventTimerDiff);
			PrintColoredLine(ConsoleColor.Yellow, CommandsSoE.EventTimerDiff.GetDisplayName()!);
		}

		protected override string GetAlternateCommands(in Enum cmd, Type alternateCommands) => base.GetAlternateCommands(cmd, 
			cmd.GetType() == typeof(CommandsSoE) 
			? typeof(AlternateCommandsSoe) 
			: alternateCommands);

		protected override string GetGuideText(string? guideName)
		{
			string? content = null;
			var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower();

			var fileName = guideName?.ToLower() switch
			{
				"savestate" => "guide-savestate.md",
				_ => "guide-srm.md"
			};

			string subfolder = "";
			if (lang != "en")
				subfolder = $"/{lang}";

			var filePath = $"Guides{subfolder}/{fileName}";
			if (File.Exists(filePath))
				return File.ReadAllText(filePath);

			filePath = $"Guides/{fileName}";
			if (File.Exists(filePath))
				return File.ReadAllText(filePath);

			return content ?? base.GetGuideText(fileName);
		}
	}
}
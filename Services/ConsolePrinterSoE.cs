using System;
using System.Globalization;
using System.IO;
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

			PrintCommandKey(CommandsSoE.Checksum);
			PrintColoredLine(ConsoleColor.Yellow, CommandsSoE.Checksum.GetDisplayName()!);

			PrintCommandKey(CommandsSoE.Checksum_Diff);
			PrintColoredLine(ConsoleColor.Yellow, CommandsSoE.Checksum_Diff.GetDisplayName()!);

			PrintCommandKey(CommandsSoE.U12b);
			PrintColoredLine(ConsoleColor.Yellow, CommandsSoE.U12b.GetDisplayName()!);

			PrintCommandKey(CommandsSoE.U12b_Diff);
			PrintColoredLine(ConsoleColor.Yellow, CommandsSoE.U12b_Diff.GetDisplayName()!);
		}

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
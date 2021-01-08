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

			PrintCommandKey(CommandsSoE.cs);
			PrintColoredLine(ConsoleColor.Yellow, CommandsSoE.cs.GetDisplayName()!);

			PrintCommandKey(CommandsSoE.csa);
			PrintColoredLine(ConsoleColor.Yellow, CommandsSoE.csa.GetDisplayName()!);

			PrintCommandKey(CommandsSoE.u12b);
			PrintColoredLine(ConsoleColor.Yellow, CommandsSoE.u12b.GetDisplayName()!);

			PrintCommandKey(CommandsSoE.u12ba);
			PrintColoredLine(ConsoleColor.Yellow, CommandsSoE.u12ba.GetDisplayName()!);
		}

		protected override string GetGuideText(string? guideName)
		{
			string? content = null;
			var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower();

			var fileName = guideName?.ToLower() switch
			{
				"savestate" => "guide-savestate.md",
				_ => "guide.md"
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
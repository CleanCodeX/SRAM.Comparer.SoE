using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Common.Shared.Min.Extensions;
using Common.Shared.Min.Helpers;
using IO.Extensions;
using SoE.Models.Enums;
using SoE.Models.Structs;
using SRAM.Comparison.Enums;
using SRAM.Comparison.Helpers;
using SRAM.Comparison.Services;
using SRAM.Comparison.SoE.Enums;
using SRAM.Comparison.SoE.Helpers;
using SRAM.SoE.Models;
using SRAM.SoE.Models.Structs;
using SRAM.Comparison.SoE.Properties;
using WRAM.Snes9x.SoE.Models;
using ResComp = SRAM.Comparison.Properties.Resources;
using Snes9x = SRAM.SoE.Extensions.StreamExtensions;

namespace SRAM.Comparison.SoE.Services
{
	/// <summary>Command handler implementation for SoE</summary>
	/// <inheritdoc cref="CommandHandler{TSramFile,TSaveSlot}"/>
	public class CommandHandlerSoE: CommandHandler<SramFileSoE, SaveSlotSoE>
	{
		private const string Snes9xId = "Snes9x";

		private new static readonly Uris Uris = new() 
		{
			Docu = "https://docu.xeth.de",
			Downloads = "https://releases.xeth.de",
			LatestUpdate = "https://xeth.de/Releases/SramComparer/LatestUpdate.json",
			Forum = "https://forum.xeth.de",
			Project = "https://evermore.azurewebsites.net",
			DiscordInvite = "https://discord.gg/s4wTHQgxae"
		};

		public override string? AppVersion { get; set; } = "035";

		public CommandHandlerSoE() {}
		public CommandHandlerSoE(IConsolePrinter consolePrinter) : base(consolePrinter) {}

		#region Command Handing

		/// <inheritdoc cref="CommandHandler{TSramFile,TSaveSlot}"/>
		protected override bool OnRunCommand(string command, IOptions options)
		{
			Requires.NotNull(command, nameof(command));
			Requires.NotNull(options, nameof(options));

			var cmd = command.ParseEnum<CommandsSoE>();
			if (cmd == default)
			{
				if (Enum.TryParse<AlternateCommandsSoe>(command, true, out var altSoECommand))
					command = ((CommandsSoE) altSoECommand).ToString();
				else if (Enum.TryParse<AlternateCommands>(command, true, out var altCommand))
					command = ((Commands) altCommand).ToString();
				else if (CheckCustomKeyBinding(command, out var boundCommand))
					command = boundCommand;

				cmd = command.ParseEnum<CommandsSoE>();
			}

			switch (cmd)
			{
				case CommandsSoE.Compare:
					Compare(options);
					break;
				case CommandsSoE.ExportCompResult:
				case CommandsSoE.ExportCompResultOpen:
				case CommandsSoE.ExportCompResultSelect:
					var localOptions = options.Copy();

					if (cmd == CommandsSoE.ExportCompResultOpen)
						localOptions.ExportFlags = localOptions.ExportFlags.SetUInt32Flags(ExportFlags.OpenFile);

					if (cmd == CommandsSoE.ExportCompResultSelect)
						localOptions.ExportFlags = localOptions.ExportFlags.SetUInt32Flags(ExportFlags.SelectFile);

					SaveCompResult(localOptions);
					break;
				case CommandsSoE.EventTimer:
				case CommandsSoE.EventTimerDiff:
					options.ComparisonFlags = InvertIncludeFlag(options.ComparisonFlags,
						cmd == CommandsSoE.EventTimer
							? ComparisonFlagsSoE.ScriptedEventTimer
							: ComparisonFlagsSoE.ScriptedEventTimerIfDifferent);

					break;
				case CommandsSoE.Checksum:
				case CommandsSoE.ChecksumDiff:
					options.ComparisonFlags = InvertIncludeFlag(options.ComparisonFlags,
						cmd == CommandsSoE.Checksum
							? ComparisonFlagsSoE.Checksum
							: ComparisonFlagsSoE.ChecksumIfDifferent);

					break;
				case CommandsSoE.ShowTerminalCodes:
					ShowTerminalCodes(options);

					break;
				default:
					return base.OnRunCommand(command, options);
			}

			return true;
		}

		private void ShowTerminalCodes(IOptions options)
		{
			options.CurrentFilePath.ThrowIfNull(nameof(options.CurrentFilePath));
			FileStream fileStream = new(options.CurrentFilePath, FileMode.Open, FileAccess.Read);
			var (AlarmCode, SecretCode) = GetTerminalCodes(options, fileStream);

			TerminalCodePrinter.PrintTerminalCodes(ConsolePrinter, AlarmCode, SecretCode);

			if (!AlarmCode.IsValid || !SecretCode.IsValid || !Equals(AlarmCode, SecretCode))
				return;

			ConsolePrinter.PrintColoredLine(ConsoleColor.Yellow, Resources.StatusBothTerminalCodesAreEqual);

			ConsolePrinter.PrintColoredLine(ConsoleColor.Cyan, Resources.PromptChangeSecretBossRoomCode);
			var cancel = Console.ReadLine()!.ToLower() != "1";
			ConsolePrinter.ResetColor();

			if (cancel) return;

			ChangeSecretCodeIfSame(options);
		}

		private void ChangeSecretCodeIfSame(IOptions options)
		{
			options.CurrentFilePath.ThrowIfNull(nameof(options.CurrentFilePath));

			TerminalCode newCode;

			using (FileStream inputStream = new(options.CurrentFilePath, FileMode.Open, FileAccess.Read))
			{
				var resultData = ChangeSecretDoorCodeIfSame(options, inputStream, out newCode);
				resultData.ThrowIfNull(nameof(resultData));

				var newCodeString = newCode.ToString().SubstringAfter("| ");
				var newFileName = Path.Join(options.CurrentFilePath + $".#{newCodeString}#" + Path.GetExtension(options.CurrentFilePath));
				
				File.WriteAllBytes(newFileName, resultData);
			}

			if (newCode.IsDefault)
				return;

			ConsolePrinter.PrintSectionHeader();
			ConsolePrinter.PrintColored(ConsoleColor.White, $"{Resources.StatusNewSecretBossRoomCode}: ");
			TerminalCodePrinter.PrintValidCode(ConsolePrinter, newCode);
			ConsolePrinter.ResetColor();
		}

		#endregion Command Handing

		#region Compare S-RAM

		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public int Compare(Stream currFile, Stream compFile, IOptions options) => Compare<SramComparerSoE>(currFile, compFile, options);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public int Compare(Stream currFile, Stream compFile, IOptions options, TextWriter output) => Compare<SramComparerSoE>(currFile, compFile, options, output);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public int Compare(IOptions options) => Compare<SramComparerSoE>(options);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public int Compare(IOptions options, TextWriter output) => Compare<SramComparerSoE>(options, output);

		public override byte[]? ConvertStreamIfSavestate(IOptions options, in Stream stream, string? filePath)
		{
			stream.ThrowIfNull(nameof(stream));

			if (base.ConvertStreamIfSavestate(options, in stream, filePath) is not { } convertedData)
				return null;

			if (convertedData.Length != SramSizes.Size && convertedData.Length != WramSizes.Size)
				throw new InvalidOperationException($"Copied stream has wrong size. Was {convertedData.Length}, but should be {SramSizes.Size} for SRM-files or {WramSizes.Size} for savestate-files");

			return convertedData;
		}
		
		#endregion Compare S-RAM

		#region Export

		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public string? SaveCompResult(IOptions options) => SaveCompResult<SramComparerSoE>(options);
		/// <summary>Convinience method for using the standard <see cref="SramComparerSoE"/></summary>
		public string? SaveCompResult(IOptions options, string filePath) => SaveCompResult<SramComparerSoE>(options, filePath); 

		#endregion Export

		#region Config

		protected override int GetMaxSaveSlotId() => 4;

		protected override void LoadConfig(IOptions options, string? configName = null)
		{
			ConsolePrinter.PrintSectionHeader();
			var filePath = base.GetConfigFilePath(options.ConfigPath, configName);
			Requires.FileExists(filePath, string.Empty, ResComp.ErrorConfigFileDoesNotExistTemplate.InsertArgs(filePath));

			try
			{
				var loadedOptions = JsonFileSerializer.Deserialize<Options>(filePath)!;
				
				foreach (var propertyInfo in typeof(IOptions).GetProperties().Where(e => e.CanWrite))
				{
					var newValue = propertyInfo.GetValue(loadedOptions);
					propertyInfo.SetValue(options, newValue);
				}
			}
			catch (Exception ex)
			{
				Debug.Print(ex.Message);
				throw;
			}

			ConsolePrinter.PrintColoredLine(ConsoleColor.Yellow, ResComp.StatusConfigFileLoadedTemplate.InsertArgs(filePath));
		}

		#endregion Config

		public (TerminalCode AlarmCode, TerminalCode SecretCode) GetTerminalCodes(IOptions options, Stream stream)
		{
			if (ConvertStreamIfSavestate(options, stream, options.CurrentFilePath) is { } convertedData)
				stream = convertedData.ToStream();

			SramFileSoE sramFile = new(stream, (GameRegion)options.GameRegion);
			var saveslotData = sramFile.GetSegment(options.CurrentFileSaveSlot);
			ref var chunk20 = ref saveslotData.Data.CurrentWeapon_LastLanding;
			return (chunk20.AlarmCode, chunk20.SecretCode);
		}

		public byte[]? ChangeSecretDoorCodeIfSame(IOptions options, Stream inputStream, out TerminalCode newCode)
		{
			if (ConvertStreamIfSavestate(options, inputStream, options.CurrentFilePath) is { } srmData)
				inputStream = srmData.ToStream();

			SramFileSoE sramFile = new(inputStream, (GameRegion)options.GameRegion);
			var slotIndex = options.CurrentFileSaveSlot;
			var saveslotData = sramFile.GetSegment(slotIndex);
			ref var chunk20 = ref saveslotData.Data.CurrentWeapon_LastLanding;

			newCode = default;

			if (chunk20.AlarmCode.IsDefault || chunk20.SecretCode.IsDefault)
				return default;

			if (!Equals(chunk20.AlarmCode, chunk20.SecretCode))
				throw new InvalidOperationException(Resources.ErrorBothTerminalCodesAreNotEqual);

			ref var code = ref chunk20.SecretCode;

			code.Code1 = code.Code1 switch
			{
				TerminalCodeColor.Blue => TerminalCodeColor.Green,
				TerminalCodeColor.Green => TerminalCodeColor.Blue,
				_ => TerminalCodeColor.Green,
			};

			newCode = code;
			sramFile.SetSegment(slotIndex, saveslotData);

			MemoryStream outputStream = new();
			sramFile.Save(outputStream);

			if (ConvertStreamToByteArrayIfSavestate(options, outputStream, options.CurrentFilePath) is { } savestateData)
				return savestateData;

			return outputStream.GetBuffer();
		}

		protected override byte[] LoadSramFromSavestate(IOptions options, in Stream stream)
		{
			var savestateType = options.SavestateType ?? Snes9xId;
			return savestateType switch
			{
				Snes9xId => Snes9x.ReadSramFromSavestate(stream, (GameRegion)options.GameRegion),
				_ => throw new NotSupportedException($"Savestate type {savestateType} is not supported.")
			};
		}

		protected override byte[] SaveSramToSavestate(IOptions options, in Stream savestateStream, in Stream srmStream)
		{
			var savestateType = options.SavestateType ?? Snes9xId;
			return savestateType switch
			{
				Snes9xId => Snes9x.WriteSramToSavestate(savestateStream, (GameRegion)options.GameRegion, srmStream.GetBytes()),
				_ => throw new NotSupportedException($"Savestate type {savestateType} is not supported.")
			};
		}

		protected override void CreateKeyBindingsFile<TEnum>() => base.CreateKeyBindingsFile<CommandsSoE>();

		protected override Uris GetUris() => Uris;
	}
}
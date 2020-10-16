using System;
using System.Linq;
using System.Text;
using Common.Shared.Min.Extensions;
using SramCommons.Extensions;
using SramCommons.Models;
using SramComparer.Helpers;
using SramComparer.Services;
using SramComparer.SoE.Enums;
using SramComparer.SoE.Properties;
using SramFormat.SoE;
using SramFormat.SoE.Constants;
using SramFormat.SoE.Models.Structs;
using static SramComparer.SoE.Helpers.UnkownBufferOffsetFinder;
using Res = SramComparer.Properties.Resources;
// ReSharper disable RedundantArgumentDefaultValue

namespace SramComparer.SoE.Services
{
	/// <summary>SRAM comparer implementation for SoE</summary>
	/// <inheritdoc cref="SramComparerBase{TSramFile,TSramGame}"/>
	public class SramComparerSoE : SramComparerBase<SramFileSoE, SramGame>
	{
		public SramComparerSoE() : base(ServiceCollection.ConsolePrinter) {}
		public SramComparerSoE(IConsolePrinter consolePrinter) :base(consolePrinter) {}

		/// <inheritdoc cref="SramComparerBase{TSramFile,TSramGame}"/>
		public override int CompareSram(SramFileSoE currFile, SramFileSoE compFile, IOptions options)
		{
			PrintGameValidationStatus(currFile, compFile);

			ConsolePrinter.PrintParagraph();

			var optionGameIndex = options.CurrentGame - 1;
			var optionCompGameIndex = options.ComparisonGame - 1;
			var optionFlags = (ComparisonFlagsSoE)options.Flags;

			ConsolePrinter.PrintColoredLine(ConsoleColor.Yellow, optionGameIndex > -1
				? string.Format(Res.StatusGameWillBeComparedTemplate, options.CurrentGame)
				: Res.StatusAllGamesWillBeCompared);

			Console.ForegroundColor = ConsoleColor.White;
			ConsolePrinter.PrintParagraph();

			var offset = GetSramOffset(nameof(compFile.Sram.Unknown1), out var bufferName);
			var sramDiffBytes = CompareByteArray(Res.Sram + ":" + bufferName, offset, compFile.Sram.Unknown1, currFile.Sram.Unknown1);
			if (sramDiffBytes > 0)
				ConsolePrinter.PrintColoredLine(ConsoleColor.Green, Res.StatusTotalDiffBytesTemplate.InsertArgs(sramDiffBytes));

			static int GetSramOffset(string bufferName, out string name)
			{
				name = bufferName;

				return GetSramBufferOffset(bufferName);
			}

			var allDiffBytes = sramDiffBytes;

			var checksums = new StringBuilder();
			checksums.AppendLine($"{Resources.GamesCurrentChecksumValues} (2 {Res.Bytes.ToLower()}): ({Resources.ChangesAtEveryInGameSave})");

			var timestamps = new StringBuilder();
			timestamps.AppendLine($"{Resources.GamesCurrentUnknown12BValues} (2+? {Res.Bytes.ToLower()}): ({Resources.ChangesAtEveryInGameSave})");

			if (optionGameIndex > -1 && optionCompGameIndex > -1)
				allDiffBytes = CompareGames(optionGameIndex, optionCompGameIndex);
			else
				for (var gameIndex = 0; gameIndex < 4; gameIndex++)
				{
					if (optionGameIndex > -1 && optionGameIndex != gameIndex) continue;

					allDiffBytes = CompareGames(gameIndex);
				}

			if (options.Flags.HasFlag(ComparisonFlagsSoE.NonGameBuffer))
			{
				var nonGameUnknownDiffBytes = CompareByteArray(nameof(currFile.Sram.Unknown1), 0, currFile.Sram.Unknown1, compFile.Sram.Unknown1, false);

				if (nonGameUnknownDiffBytes > 0)
				{
					ConsolePrinter.PrintParagraph();
					ConsolePrinter.PrintColoredLine(ConsoleColor.Magenta, " ".Repeat(2) + $@"[ {Res.SectionNonGameUnknowns} ].......................................");
					ConsolePrinter.ResetColor();

					nonGameUnknownDiffBytes = CompareByteArray(nameof(currFile.Sram.Unknown1), Offsets.SramUnknown1, currFile.Sram.Unknown1, compFile.Sram.Unknown1, true);

					ConsolePrinter.PrintParagraph();
					ConsolePrinter.PrintColoredLine(nonGameUnknownDiffBytes > 0 ? ConsoleColor.Green : ConsoleColor.White, " ".Repeat(4) + Res.StatusNonGameUnknownsBytesTemplate.InsertArgs(nonGameUnknownDiffBytes));
					ConsolePrinter.ResetColor();

					allDiffBytes += nonGameUnknownDiffBytes;
				}
			}

			ConsolePrinter.PrintParagraph();

			const int borderLength = 29;
			var color = allDiffBytes > 0 ? ConsoleColor.Yellow : ConsoleColor.Green;
			ConsolePrinter.PrintColoredLine(color, "=".Repeat(borderLength));
			if (allDiffBytes > 0)
				ConsolePrinter.PrintColoredLine(color, @$"== {Res.StatusSramChangedBytesTemplate} ".PadRight(borderLength + 1, '=').InsertArgs(allDiffBytes));
			else
				ConsolePrinter.PrintColoredLine(color, @$"== {Res.StatusNoSramBytesChanged} =".PadRight(borderLength, '='));

			ConsolePrinter.PrintColoredLine(color, "=".Repeat(borderLength));
			ConsolePrinter.ResetColor();

			if (optionFlags != default)
			{
				ConsolePrinter.PrintParagraph();

				if (optionFlags.HasFlag(ComparisonFlagsSoE.GameChecksum))
					ConsolePrinter.PrintColoredLine(ConsoleColor.Cyan, checksums.ToString());

				if (optionFlags.HasFlag(ComparisonFlagsSoE.Unknown12B))
					ConsolePrinter.PrintColoredLine(ConsoleColor.Cyan, timestamps.ToString());
			}

			return allDiffBytes;

			int CompareGames(int gameIndex, int compGameIndex = default)
			{
				if (compGameIndex == -1)
					compGameIndex = gameIndex;

				var currGame = currFile.GetGame(gameIndex);
				var currGameBytes = currFile.GetGameBytes(gameIndex);

				var compGame = compFile.GetGame(gameIndex);
				var compGameBytes = compFile.GetGameBytes(gameIndex);

				var gameId = gameIndex + 1;
				var compGameId = compGameIndex + 1;

				var gameIdString = gameId.ToString();
				if (gameId != compGameId)
					gameIdString += $" ({Resources.ComparedWithGameTemplated.InsertArgs(compGameId)})";

				if (optionFlags.HasFlag(ComparisonFlagsSoE.AllGameChecksums) || compGame.Checksum != currGame.Checksum)
					checksums.AppendLine(
						$"{" ".Repeat(2)}{Res.Game} {gameId}: {currGame.Checksum.PadLeft()}");

				if (optionFlags.HasFlag(ComparisonFlagsSoE.AllUnknown12Bs) || compGame.Unknown12B != currGame.Unknown12B)
					timestamps.AppendLine(
						$"{" ".Repeat(2)}{Res.Game} {gameId}: {currGame.Unknown12B.PadLeft()}");

				if (compGameBytes.SequenceEqual(currGameBytes)) return allDiffBytes;

				checksums.AppendLine(
					$"{" ".Repeat(2)}{Res.Game} {compGameId}: {compGame.Checksum.PadLeft()}");

				timestamps.AppendLine(
					$"{" ".Repeat(2)}{Res.Game} {compGameId}: {compGame.Unknown12B.PadLeft()}");

				ConsolePrinter.PrintColoredLine(ConsoleColor.Yellow, $@"[ {Res.Game} {gameIdString} ]---------------------------------------------");
				ConsolePrinter.ResetColor();

				ConsolePrinter.PrintParagraph();
				ConsolePrinter.PrintColoredLine(ConsoleColor.Magenta, " ".Repeat(2) + $@"[ {Res.UnknownsOnly} ].......................................");
				ConsolePrinter.ResetColor();

				var gameDiffBytes = CompareGame(currGame, compGame, options);
				if (gameDiffBytes > 0)
				{
					ConsolePrinter.PrintParagraph();
					ConsolePrinter.PrintColoredLine(ConsoleColor.Green, " ".Repeat(4) + Res.StatusGameUnknownsChangedBytesTemplate.InsertArgs(gameDiffBytes));
					ConsolePrinter.ResetColor();
				}

				if (!optionFlags.HasFlag(ComparisonFlagsSoE.WholeGameBuffer))
				{
					allDiffBytes += gameDiffBytes;
					return allDiffBytes;
				}

				ConsoleHelper.EnsureMinConsoleWidth(165);

				bufferName = $"{nameof(compFile.Sram.Game)} {gameId}";
				var bufferOffset = Offsets.FirstGame + gameId * Sizes.Game.All;
				var gameBufferDiffBytes = CompareByteArray(bufferName, bufferOffset, currGameBytes, compGameBytes, false);
				if (gameBufferDiffBytes > gameDiffBytes)
				{
					ConsolePrinter.PrintParagraph();
					ConsolePrinter.PrintColoredLine(ConsoleColor.Magenta, $@"{" ".Repeat(2)}[ {Res.SectionGameChangedTemplate} ]...................................".InsertArgs(gameIndex));
					// ReSharper disable once RedundantArgumentDefaultValue

					CompareByteArray(bufferName, bufferOffset, currGameBytes, compGameBytes, true, Offsets.Game.GetNameFromOffset);
					ConsolePrinter.PrintParagraph();
					ConsolePrinter.PrintColoredLine(gameDiffBytes > 0 ? ConsoleColor.Green : ConsoleColor.White, " ".Repeat(4) + Res.StatusGameChangedBytesTemplate.InsertArgs(gameIdString, gameBufferDiffBytes));
					ConsolePrinter.ResetColor();
				}

				allDiffBytes += gameBufferDiffBytes;
				return allDiffBytes;
			}
		}

		protected virtual void PrintGameValidationStatus(SramFileSoE currFile, SramFileSoE compFile)
		{
			ConsolePrinter.PrintSectionHeader();
			ConsolePrinter.PrintColoredLine(ConsoleColor.DarkYellow, $@"{Resources.ValidationStatus}:");

			OnPrintGameValidationStatus(Res.Current, currFile);
			OnPrintGameValidationStatus(Res.Comparison, compFile);
		}

		protected virtual void OnPrintGameValidationStatus(string name, ISramFile file)
		{
			ConsolePrinter.PrintColored(ConsoleColor.Gray, $@"{name}:".PadRight(15));
			ConsolePrinter.PrintColored(ConsoleColor.DarkYellow, $@" {Res.Game} (1-4)");
			ConsolePrinter.PrintColored(ConsoleColor.White, @" [ ");

			for (var i = 0; i <= 3; i++)
			{
				if (i > 0)
					ConsolePrinter.PrintColored(ConsoleColor.White, @" | ");

				var isValid = file.IsValid(i);
				ConsolePrinter.PrintColored(isValid ? ConsoleColor.DarkGreen : ConsoleColor.Red, isValid ? Resources.Valid : Resources.Invalid);
			}

			ConsolePrinter.PrintColoredLine(ConsoleColor.White, @" ]");
		}

		/// <inheritdoc cref="SramComparerBase{TSramFile,TSramGame}"/>
		public override int CompareGame(SramGame currGame, SramGame compGame, IOptions options)
		{
			var delimiter = StructDelimiter;

			// ReSharper disable once InlineOutVariableDeclaration
			string name;
			// ReSharper disable once JoinDeclarationAndInitializer
			int offset;

			offset = GetGameOffset(nameof(currGame.Unknown1), out name);
			var diffBytes = CompareByteArray(name, offset, currGame.Unknown1, compGame.Unknown1);

			offset = GetGameOffset(nameof(currGame.Unknown2), out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown2, compGame.Unknown2);

			offset = GetGameOffset(nameof(currGame.Unknown3), out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown3, compGame.Unknown3);

			//Unknown 4
			offset = GetGameOffset($"{nameof(currGame.Unknown4_BoyBuff)}{delimiter}{nameof(currGame.Unknown4_BoyBuff.BuffFlags)}", out name);
			diffBytes += CompareUShort(name, offset, currGame.Unknown4_BoyBuff.BuffFlags.ToUShort(), compGame.Unknown4_BoyBuff.BuffFlags.ToUShort());

			offset = GetGameOffset($"{nameof(currGame.Unknown4_BoyBuff)}{delimiter}{nameof(currGame.Unknown4_BoyBuff.Unknown1)}", out name);
			diffBytes += CompareUShort(name, offset, currGame.Unknown4_BoyBuff.Unknown1, compGame.Unknown4_BoyBuff.Unknown1);

			offset = GetGameOffset($"{nameof(currGame.Unknown4_BoyBuff)}{delimiter}{nameof(currGame.Unknown4_BoyBuff.Unknown2)}", out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown4_BoyBuff.Unknown2, compGame.Unknown4_BoyBuff.Unknown2);

			offset = GetGameOffset($"{nameof(currGame.Unknown4_BoyBuff)}{delimiter}{nameof(currGame.Unknown4_BoyBuff.Unknown3)}", out name);
			diffBytes += CompareUShort(name, offset, currGame.Unknown4_BoyBuff.Unknown3, compGame.Unknown4_BoyBuff.Unknown3);

			offset = GetGameOffset($"{nameof(currGame.Unknown4_BoyBuff)}{delimiter}{nameof(currGame.Unknown4_BoyBuff.Unknown4)}", out name);
			diffBytes += CompareUShort(name, offset, currGame.Unknown4_BoyBuff.Unknown4, compGame.Unknown4_BoyBuff.Unknown4);
			//Unknown 4

			offset = GetGameOffset(nameof(currGame.Unknown5), out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown5, compGame.Unknown5);

			offset = GetGameOffset(nameof(currGame.Unknown6), out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown6, compGame.Unknown6);

			//Unknown 7
			offset = GetGameOffset($"{nameof(currGame.Unknown7_DogBuff)}{delimiter}{nameof(currGame.Unknown7_DogBuff.BuffFlags)}", out name);
			diffBytes += CompareUShort(name, offset, currGame.Unknown7_DogBuff.BuffFlags.ToUShort(), compGame.Unknown7_DogBuff.BuffFlags.ToUShort());

			offset = GetGameOffset($"{nameof(currGame.Unknown7_DogBuff)}{delimiter}{nameof(currGame.Unknown7_DogBuff.Unknown1)}", out name);
			diffBytes += CompareUShort(name, offset, currGame.Unknown7_DogBuff.Unknown1, compGame.Unknown7_DogBuff.Unknown1);

			offset = GetGameOffset($"{nameof(currGame.Unknown7_DogBuff)}{delimiter}{nameof(currGame.Unknown7_DogBuff.Unknown2)}", out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown7_DogBuff.Unknown2, compGame.Unknown7_DogBuff.Unknown2);

			offset = GetGameOffset($"{nameof(currGame.Unknown7_DogBuff)}{delimiter}{nameof(currGame.Unknown7_DogBuff.Unknown3)}", out name);
			diffBytes += CompareUShort(name, offset, currGame.Unknown7_DogBuff.Unknown3, compGame.Unknown7_DogBuff.Unknown3);

			offset = GetGameOffset($"{nameof(currGame.Unknown7_DogBuff)}{delimiter}{nameof(currGame.Unknown7_DogBuff.Unknown4)}", out name);
			diffBytes += CompareUShort(name, offset, currGame.Unknown7_DogBuff.Unknown4, compGame.Unknown7_DogBuff.Unknown4);
			//Unknown 7

			offset = GetGameOffset(nameof(currGame.Unknown8), out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown8, compGame.Unknown8);

			offset = GetGameOffset(nameof(currGame.Unknown9), out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown9, compGame.Unknown9);

			offset = GetGameOffset(nameof(currGame.Unknown10), out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown10, compGame.Unknown10);

			offset = GetGameOffset(nameof(currGame.Unknown11), out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown11, compGame.Unknown11);

			// 12 A - C
			offset = GetGameOffset(nameof(currGame.Unknown12A), out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown12A, compGame.Unknown12A);

			if (options.Flags.HasFlag(ComparisonFlagsSoE.Unknown12B))
			{
				offset = GetGameOffset(nameof(currGame.Unknown12B), out name);
				diffBytes += CompareUShort(name, offset, currGame.Unknown12B,
					compGame.Unknown12B);
			}

			offset = GetGameOffset(nameof(currGame.Unknown12C), out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown12C, compGame.Unknown12C);
			// 

			offset = GetGameOffset(nameof(currGame.Unknown13), out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown13, compGame.Unknown13);

			offset = GetGameOffset(nameof(currGame.Unknown14_AntiquaFlags), out name);
			diffBytes += CompareByteArray(name, offset, 
				BitConverter.GetBytes(currGame.Unknown14_AntiquaFlags.ToUInt()), 
				BitConverter.GetBytes(compGame.Unknown14_AntiquaFlags.ToUInt()));

			//Unknown 15
			offset = GetGameOffset($"{nameof(currGame.Unknown15)}{delimiter}{nameof(currGame.Unknown15.Offset0To16)}", out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown15.Offset0To16, compGame.Unknown15.Offset0To16);

			offset = GetGameOffset($"{nameof(currGame.Unknown15)}{delimiter}{nameof(currGame.Unknown15.Offset17)}", out name);
			diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset17.ToByte(), compGame.Unknown15.Offset17.ToByte());

			offset = GetGameOffset($"{nameof(currGame.Unknown15)}{delimiter}{nameof(currGame.Unknown15.Offset18)}", out name);
			diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset18.ToByte(), compGame.Unknown15.Offset18.ToByte());

			offset = GetGameOffset($"{nameof(currGame.Unknown15)}{delimiter}{nameof(currGame.Unknown15.Offset19)}", out name);
			diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset19.ToByte(), compGame.Unknown15.Offset19.ToByte());

			offset = GetGameOffset($"{nameof(currGame.Unknown15)}{delimiter}{nameof(currGame.Unknown15.Offset20)}", out name);
			diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset20.ToByte(), compGame.Unknown15.Offset20.ToByte());

			offset = GetGameOffset($"{nameof(currGame.Unknown15)}{delimiter}{nameof(currGame.Unknown15.Offset21)}", out name);
			diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset21, compGame.Unknown15.Offset21);

			offset = GetGameOffset($"{nameof(currGame.Unknown15)}{delimiter}{nameof(currGame.Unknown15.Offset22)}", out name);
			diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset22, compGame.Unknown15.Offset22);

			offset = GetGameOffset($"{nameof(currGame.Unknown15)}{delimiter}{nameof(currGame.Unknown15.Offset22)}", out name);
			diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset22, compGame.Unknown15.Offset22);

			offset = GetGameOffset($"{nameof(currGame.Unknown15)}{delimiter}{nameof(currGame.Unknown15.Offset23)}", out name);
			diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset23, compGame.Unknown15.Offset23);

			offset = GetGameOffset($"{nameof(currGame.Unknown15)}{delimiter}{nameof(currGame.Unknown15.Offset24)}", out name);
			diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset24.ToByte(), compGame.Unknown15.Offset24.ToByte());

			offset = GetGameOffset($"{nameof(currGame.Unknown15)}{delimiter}{nameof(currGame.Unknown15.Offset25To117)}", out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown15.Offset25To117, compGame.Unknown15.Offset25To117);
			//Unknown 15

			// 16 A - C
			offset = GetGameOffset(nameof(currGame.Unknown16A), out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown16A, compGame.Unknown16A);

			offset = GetGameOffset(nameof(currGame.Unknown16B_GoticaFlags), out name);
			diffBytes += CompareByteArray(name, offset, 
				BitConverter.GetBytes(currGame.Unknown16B_GoticaFlags.ToUInt()), 
				BitConverter.GetBytes(compGame.Unknown16B_GoticaFlags.ToUInt()));

			offset = GetGameOffset(nameof(currGame.Unknown16C), out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown16C, compGame.Unknown16C);
			// 

			offset = GetGameOffset(nameof(currGame.Unknown17), out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown17, compGame.Unknown17);

			offset = GetGameOffset(nameof(currGame.Unknown18), out name);
			diffBytes += CompareByteArray(name, offset, currGame.Unknown18, compGame.Unknown18);

			return diffBytes;

			static int GetGameOffset(string bufferName, out string name)
			{
				name = bufferName;
				return  GetGameBufferOffset(bufferName);
			}
		}
	}
}
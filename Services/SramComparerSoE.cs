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
	/// <inheritdoc cref="SramComparerBase{TSramFile,TSaveSlot}"/>
	public class SramComparerSoE : SramComparerBase<SramFileSoE, SaveSlot>
	{
		public SramComparerSoE() : base(ServiceCollection.ConsolePrinter) {}
		public SramComparerSoE(IConsolePrinter consolePrinter) :base(consolePrinter) {}

		/// <inheritdoc cref="SramComparerBase{TSramFile,TSaveSlot}"/>
		public override int CompareSram(SramFileSoE currFile, SramFileSoE compFile, IOptions options)
		{
			PrintSaveSlotValidationStatus(currFile, compFile);

			ConsolePrinter.PrintParagraph();

			var optionSaveSlotIndex = options.CurrentSramFileSaveSlot - 1;
			var optionCompSaveSlotIndex = options.ComparisonSramFileSaveSlot - 1;
			var optionFlags = (ComparisonFlagsSoE)options.ComparisonFlags;

			ConsolePrinter.PrintColoredLine(ConsoleColor.Yellow, optionSaveSlotIndex > -1
				? string.Format(Res.StatusSingleSaveSlotWillBeComparedTemplate, options.CurrentSramFileSaveSlot)
				: Res.StatusAllSaveSlotsWillBeCompared);

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
			checksums.AppendLine($"{ComparisonFlagsSoE.ChecksumComparedSlots.GetDisplayName()} (2 {Res.Bytes.ToLower()}): ({Resources.ChangesAtEveryInSaveSlotSave})");

			var timestamps = new StringBuilder();
			timestamps.AppendLine($"{ComparisonFlagsSoE.Unknown12BComparedSlots.GetDisplayName()} (2 {Res.Bytes.ToLower()}): ({Resources.ChangesAtEveryInSaveSlotSave})");

			if (optionSaveSlotIndex > -1 && optionCompSaveSlotIndex > -1)
				allDiffBytes = CompareSaveSlots(optionSaveSlotIndex, optionCompSaveSlotIndex);
			else
				for (var slotIndex = 0; slotIndex < 4; slotIndex++)
				{
					if (optionSaveSlotIndex > -1 && optionSaveSlotIndex != slotIndex) continue;

					allDiffBytes = CompareSaveSlots(slotIndex);
				}

			if (options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.NonSlotByteByByteComparison))
			{
				var nonSaveSlotUnknownDiffBytes = CompareByteArray(nameof(currFile.Sram.Unknown1), 0, currFile.Sram.Unknown1, compFile.Sram.Unknown1, false);

				if (nonSaveSlotUnknownDiffBytes > 0)
				{
					ConsolePrinter.PrintParagraph();
					ConsolePrinter.PrintColoredLine(ConsoleColor.Magenta, " ".Repeat(2) + $@"[ {Res.SectionNonSaveSlotUnknowns} ].......................................");
					ConsolePrinter.ResetColor();

					nonSaveSlotUnknownDiffBytes = CompareByteArray(nameof(currFile.Sram.Unknown1), Offsets.SramUnknown1, currFile.Sram.Unknown1, compFile.Sram.Unknown1, true);

					ConsolePrinter.PrintParagraph();
					ConsolePrinter.PrintColoredLine(nonSaveSlotUnknownDiffBytes > 0 ? ConsoleColor.Green : ConsoleColor.White, " ".Repeat(4) + Res.StatusNonSaveSlotUnknownsBytesTemplate.InsertArgs(nonSaveSlotUnknownDiffBytes));
					ConsolePrinter.ResetColor();

					allDiffBytes += nonSaveSlotUnknownDiffBytes;
				}
			}

			ConsolePrinter.PrintParagraph();

			const int borderLength = 50;
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

				if (optionFlags.HasFlag(ComparisonFlagsSoE.ChecksumComparedSlots))
					ConsolePrinter.PrintColoredLine(ConsoleColor.Cyan, checksums.ToString());

				if (optionFlags.HasFlag(ComparisonFlagsSoE.Unknown12BComparedSlots))
					ConsolePrinter.PrintColoredLine(ConsoleColor.Cyan, timestamps.ToString());
			}

			return allDiffBytes;

			int CompareSaveSlots(int slotIndex, int compSaveSlotIndex = default)
			{
				if (compSaveSlotIndex == -1)
					compSaveSlotIndex = slotIndex;

				var currSaveSlot = currFile.GetSaveSlot(slotIndex);
				var currSaveSlotBytes = currFile.GetSaveSlotBytes(slotIndex);

				var compSaveSlot = compFile.GetSaveSlot(slotIndex);
				var compSaveSlotBytes = compFile.GetSaveSlotBytes(slotIndex);

				var gameId = slotIndex + 1;
				var compSaveSlotId = compSaveSlotIndex + 1;

				var gameIdString = gameId.ToString();
				if (gameId != compSaveSlotId)
					gameIdString += $" ({Resources.ComparedWithOtherSaveSlotTemplate.InsertArgs(compSaveSlotId)})";

				var currSaveSlotName = $"{Res.CurrentShort} {Res.SaveSlot} {gameId}";
				if (optionFlags.HasFlag(ComparisonFlagsSoE.ChecksumAllSlots) || compSaveSlot.Checksum != currSaveSlot.Checksum)
					checksums.AppendLine(
						$"{" ".Repeat(2)}{currSaveSlotName}: {currSaveSlot.Checksum.PadLeft()}");

				if (optionFlags.HasFlag(ComparisonFlagsSoE.Unknown12BAllSlots) || compSaveSlot.Unknown12B != currSaveSlot.Unknown12B)
					timestamps.AppendLine(
						$"{" ".Repeat(2)}{currSaveSlotName}: {currSaveSlot.Unknown12B.PadLeft()}");

				if (compSaveSlotBytes.SequenceEqual(currSaveSlotBytes)) return allDiffBytes;

				var compSaveSlotName = $"{Res.ComparisonShort} {Res.SaveSlot} {compSaveSlotId}";
				checksums.AppendLine(
					$"{" ".Repeat(2)}{compSaveSlotName}: {compSaveSlot.Checksum.PadLeft()}");

				timestamps.AppendLine(
					$"{" ".Repeat(2)}{compSaveSlotName}: {compSaveSlot.Unknown12B.PadLeft()}");

				ConsolePrinter.PrintColoredLine(ConsoleColor.Yellow, $@"[ {Res.SaveSlot} {gameIdString} ]---------------------------------------------");
				ConsolePrinter.ResetColor();

				ConsolePrinter.PrintParagraph();
				ConsolePrinter.PrintColoredLine(ConsoleColor.Magenta, " ".Repeat(2) + $@"[ {Res.UnknownAreasOnly} ].......................................");
				ConsolePrinter.ResetColor();

				var gameDiffBytes = CompareSaveSlot(currSaveSlot, compSaveSlot, options);
				if (gameDiffBytes > 0)
				{
					ConsolePrinter.PrintParagraph();
					ConsolePrinter.PrintColoredLine(ConsoleColor.Green, " ".Repeat(4) + Res.StatusUnknownsChangedBytesTemplate.InsertArgs(gameDiffBytes));
					ConsolePrinter.ResetColor();
				}

				if (!optionFlags.HasFlag(ComparisonFlagsSoE.SlotByteByByteComparison))
				{
					allDiffBytes += gameDiffBytes;
					return allDiffBytes;
				}

				ConsoleHelper.EnsureMinConsoleWidth(165);

				bufferName = $"{nameof(compFile.Sram.SaveSlots)} {gameId}";
				var bufferOffset = Offsets.FirstSaveSlot + gameId * Sizes.SaveSlot.All;
				var gameBufferDiffBytes = CompareByteArray(bufferName, bufferOffset, currSaveSlotBytes, compSaveSlotBytes, false);
				if (gameBufferDiffBytes > gameDiffBytes)
				{
					ConsolePrinter.PrintParagraph();
					ConsolePrinter.PrintColoredLine(ConsoleColor.Magenta, $@"{" ".Repeat(2)}[ {Res.SectionSaveSlotChangedTemplate} ]...................................".InsertArgs(slotIndex));
					// ReSharper disable once RedundantArgumentDefaultValue

					CompareByteArray(bufferName, bufferOffset, currSaveSlotBytes, compSaveSlotBytes, true, Offsets.SaveSlot.GetNameFromOffset);
					ConsolePrinter.PrintParagraph();
					ConsolePrinter.PrintColoredLine(gameDiffBytes > 0 ? ConsoleColor.Green : ConsoleColor.White, " ".Repeat(4) + Res.StatusSaveSlotChangedBytesTemplate.InsertArgs(gameIdString, gameBufferDiffBytes));
					ConsolePrinter.ResetColor();
				}

				allDiffBytes += gameBufferDiffBytes;
				return allDiffBytes;
			}
		}

		protected virtual void PrintSaveSlotValidationStatus(SramFileSoE currFile, SramFileSoE compFile)
		{
			ConsolePrinter.PrintSectionHeader();
			ConsolePrinter.PrintColoredLine(ConsoleColor.DarkYellow, $@"{Resources.ValidationStatus}:");

			OnPrintSaveSlotValidationStatus(Res.Current, currFile);
			OnPrintSaveSlotValidationStatus(Res.Comparison, compFile);
		}

		protected virtual void OnPrintSaveSlotValidationStatus(string name, ISramFile file)
		{
			ConsolePrinter.PrintColored(ConsoleColor.Gray, $@"{name}:".PadRight(15));
			ConsolePrinter.PrintColored(ConsoleColor.DarkYellow, $@" {Res.SaveSlot} (1-4)");
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

		/// <inheritdoc cref="SramComparerBase{TSramFile,TSaveSlot}"/>
		public override int CompareSaveSlot(SaveSlot currSaveSlot, SaveSlot compSaveSlot, IOptions options)
		{
			var delimiter = StructDelimiter;

			// ReSharper disable once InlineOutVariableDeclaration
			string name;
			// ReSharper disable once JoinDeclarationAndInitializer
			int offset;

			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown1), out name);
			var diffBytes = CompareByteArray(name, offset, currSaveSlot.Unknown1, compSaveSlot.Unknown1);

			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown2), out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown2, compSaveSlot.Unknown2);

			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown3), out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown3, compSaveSlot.Unknown3);

			//Unknown 4
			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown4_BoyBuff)}{delimiter}{nameof(currSaveSlot.Unknown4_BoyBuff.BuffFlags)}", out name);
			diffBytes += CompareUShort(name, offset, currSaveSlot.Unknown4_BoyBuff.BuffFlags.ToUShort(), compSaveSlot.Unknown4_BoyBuff.BuffFlags.ToUShort());

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown4_BoyBuff)}{delimiter}{nameof(currSaveSlot.Unknown4_BoyBuff.Unknown1)}", out name);
			diffBytes += CompareUShort(name, offset, currSaveSlot.Unknown4_BoyBuff.Unknown1, compSaveSlot.Unknown4_BoyBuff.Unknown1);

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown4_BoyBuff)}{delimiter}{nameof(currSaveSlot.Unknown4_BoyBuff.Unknown2)}", out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown4_BoyBuff.Unknown2, compSaveSlot.Unknown4_BoyBuff.Unknown2);

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown4_BoyBuff)}{delimiter}{nameof(currSaveSlot.Unknown4_BoyBuff.Unknown3)}", out name);
			diffBytes += CompareUShort(name, offset, currSaveSlot.Unknown4_BoyBuff.Unknown3, compSaveSlot.Unknown4_BoyBuff.Unknown3);

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown4_BoyBuff)}{delimiter}{nameof(currSaveSlot.Unknown4_BoyBuff.Unknown4)}", out name);
			diffBytes += CompareUShort(name, offset, currSaveSlot.Unknown4_BoyBuff.Unknown4, compSaveSlot.Unknown4_BoyBuff.Unknown4);
			//Unknown 4

			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown5), out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown5, compSaveSlot.Unknown5);

			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown6), out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown6, compSaveSlot.Unknown6);

			//Unknown 7
			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown7_DogBuff)}{delimiter}{nameof(currSaveSlot.Unknown7_DogBuff.BuffFlags)}", out name);
			diffBytes += CompareUShort(name, offset, currSaveSlot.Unknown7_DogBuff.BuffFlags.ToUShort(), compSaveSlot.Unknown7_DogBuff.BuffFlags.ToUShort());

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown7_DogBuff)}{delimiter}{nameof(currSaveSlot.Unknown7_DogBuff.Unknown1)}", out name);
			diffBytes += CompareUShort(name, offset, currSaveSlot.Unknown7_DogBuff.Unknown1, compSaveSlot.Unknown7_DogBuff.Unknown1);

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown7_DogBuff)}{delimiter}{nameof(currSaveSlot.Unknown7_DogBuff.Unknown2)}", out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown7_DogBuff.Unknown2, compSaveSlot.Unknown7_DogBuff.Unknown2);

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown7_DogBuff)}{delimiter}{nameof(currSaveSlot.Unknown7_DogBuff.Unknown3)}", out name);
			diffBytes += CompareUShort(name, offset, currSaveSlot.Unknown7_DogBuff.Unknown3, compSaveSlot.Unknown7_DogBuff.Unknown3);

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown7_DogBuff)}{delimiter}{nameof(currSaveSlot.Unknown7_DogBuff.Unknown4)}", out name);
			diffBytes += CompareUShort(name, offset, currSaveSlot.Unknown7_DogBuff.Unknown4, compSaveSlot.Unknown7_DogBuff.Unknown4);
			//Unknown 7

			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown8), out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown8, compSaveSlot.Unknown8);

			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown9), out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown9, compSaveSlot.Unknown9);

			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown10), out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown10, compSaveSlot.Unknown10);

			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown11), out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown11, compSaveSlot.Unknown11);

			// 12 A - C
			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown12A), out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown12A, compSaveSlot.Unknown12A);

			if (options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.Unknown12BComparedSlots))
			{
				offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown12B), out name);
				diffBytes += CompareUShort(name, offset, currSaveSlot.Unknown12B,
					compSaveSlot.Unknown12B);
			}

			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown12C), out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown12C, compSaveSlot.Unknown12C);
			// 

			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown13), out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown13, compSaveSlot.Unknown13);

			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown14), out name);
			diffBytes += CompareByteArray(name, offset, 
				BitConverter.GetBytes(currSaveSlot.Unknown14.ToUInt()), 
				BitConverter.GetBytes(compSaveSlot.Unknown14.ToUInt()));

			//Unknown 15
			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown15)}{delimiter}{nameof(currSaveSlot.Unknown15.Offset0To16)}", out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown15.Offset0To16, compSaveSlot.Unknown15.Offset0To16);

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown15)}{delimiter}{nameof(currSaveSlot.Unknown15.Offset17)}", out name);
			diffBytes += CompareByte(name, offset, currSaveSlot.Unknown15.Offset17.ToByte(), compSaveSlot.Unknown15.Offset17.ToByte());

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown15)}{delimiter}{nameof(currSaveSlot.Unknown15.Offset18)}", out name);
			diffBytes += CompareByte(name, offset, currSaveSlot.Unknown15.Offset18.ToByte(), compSaveSlot.Unknown15.Offset18.ToByte());

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown15)}{delimiter}{nameof(currSaveSlot.Unknown15.Offset19)}", out name);
			diffBytes += CompareByte(name, offset, currSaveSlot.Unknown15.Offset19.ToByte(), compSaveSlot.Unknown15.Offset19.ToByte());

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown15)}{delimiter}{nameof(currSaveSlot.Unknown15.Offset20)}", out name);
			diffBytes += CompareByte(name, offset, currSaveSlot.Unknown15.Offset20.ToByte(), compSaveSlot.Unknown15.Offset20.ToByte());

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown15)}{delimiter}{nameof(currSaveSlot.Unknown15.Offset21)}", out name);
			diffBytes += CompareByte(name, offset, currSaveSlot.Unknown15.Offset21, compSaveSlot.Unknown15.Offset21);

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown15)}{delimiter}{nameof(currSaveSlot.Unknown15.Offset22)}", out name);
			diffBytes += CompareByte(name, offset, currSaveSlot.Unknown15.Offset22, compSaveSlot.Unknown15.Offset22);

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown15)}{delimiter}{nameof(currSaveSlot.Unknown15.Offset22)}", out name);
			diffBytes += CompareByte(name, offset, currSaveSlot.Unknown15.Offset22, compSaveSlot.Unknown15.Offset22);

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown15)}{delimiter}{nameof(currSaveSlot.Unknown15.Offset23)}", out name);
			diffBytes += CompareByte(name, offset, currSaveSlot.Unknown15.Offset23, compSaveSlot.Unknown15.Offset23);

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown15)}{delimiter}{nameof(currSaveSlot.Unknown15.Offset24)}", out name);
			diffBytes += CompareByte(name, offset, currSaveSlot.Unknown15.Offset24.ToByte(), compSaveSlot.Unknown15.Offset24.ToByte());

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown15)}{delimiter}{nameof(currSaveSlot.Unknown15.Offset25To117)}", out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown15.Offset25To117, compSaveSlot.Unknown15.Offset25To117);
			//Unknown 15

			// 16 A - C
			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown16A), out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown16A, compSaveSlot.Unknown16A);

			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown16B_GothicaFlags), out name);
			diffBytes += CompareByteArray(name, offset, 
				BitConverter.GetBytes(currSaveSlot.Unknown16B_GothicaFlags.ToUInt()), 
				BitConverter.GetBytes(compSaveSlot.Unknown16B_GothicaFlags.ToUInt()));

			// 16 C
			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown16C), out name);
			diffBytes += CompareByte(name, offset,
				currSaveSlot.Unknown16C.Offset0.ToByte(),
				compSaveSlot.Unknown16C.Offset0.ToByte());

			offset = GetSaveSlotOffset($"{nameof(currSaveSlot.Unknown16C)}{delimiter}{nameof(currSaveSlot.Unknown16C.Offset1To5)}", out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown16C.Offset1To5, compSaveSlot.Unknown16C.Offset1To5);
			// 

			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown17), out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown17, compSaveSlot.Unknown17);

			offset = GetSaveSlotOffset(nameof(currSaveSlot.Unknown18), out name);
			diffBytes += CompareByteArray(name, offset, currSaveSlot.Unknown18, compSaveSlot.Unknown18);

			return diffBytes;

			static int GetSaveSlotOffset(string bufferName, out string name)
			{
				name = bufferName;
				return  GetSaveSlotBufferOffset(bufferName);
			}
		}
	}
}
using System;
using System.Linq;
using System.Text;
using Common.Shared.Min.Extensions;
using RosettaStone.Sram.SoE.Constants;
using RosettaStone.Sram.SoE.Models;
using RosettaStone.Sram.SoE.Models.Structs;
using SramCommons.Extensions;
using SramCommons.Models;
using SramComparer.Helpers;
using SramComparer.Services;
using SramComparer.SoE.Enums;
using SramComparer.SoE.Properties;
using static SramComparer.SoE.Helpers.UnkownBufferOffsetFinder;
using Res = SramComparer.Properties.Resources;
// ReSharper disable RedundantArgumentDefaultValue

namespace SramComparer.SoE.Services
{
	/// <summary>SRAM comparer implementation for SoE</summary>
	/// <inheritdoc cref="SramComparerBase{TSramFile,TSaveSlot}"/>
	public class SramComparerSoE : SramComparerBase<SramFileSoE, SaveSlotSoE>
	{
		public SramComparerSoE() : base(ServiceCollection.ConsolePrinter) {}
		public SramComparerSoE(IConsolePrinter consolePrinter) :base(consolePrinter) {}

		#region CompareSram

		/// <inheritdoc cref="SramComparerBase{TSramFile,TSaveSlot}"/>
		public override int CompareSram(SramFileSoE currFile, SramFileSoE compFile, IOptions options)
		{
			PrintSaveSlotValidationStatus(currFile, compFile);

			ConsolePrinter.PrintLine();

			var optionCurrSlotIndex = options.CurrentFileSaveSlot - 1;
			var optionCompSlotIndex = options.ComparisonFileSaveSlot - 1;
			var optionFlags = (ComparisonFlagsSoE)options.ComparisonFlags;

			var slotComparisonMode = optionCompSlotIndex > -1
				? Res.StatusDifferentSaveSlotComparisonTemplate.InsertArgs(options.CurrentFileSaveSlot,
					options.ComparisonFileSaveSlot)
				: optionCurrSlotIndex > -1
					? string.Format(Res.StatusSingleSaveSlotComparisonTemplate, options.CurrentFileSaveSlot)
					: Res.StatusAllSaveSlotsComparison;
			ConsolePrinter.PrintColoredLine(ConsoleColor.Yellow, slotComparisonMode);

			ConsolePrinter.PrintLine();

			var offset = GetSramOffset(nameof(compFile.Sram.Unknown1), out var bufferName);
			var sramDiffBytes = CompareByteArray(Res.CompSram + ":" + bufferName, offset, compFile.Sram.Unknown1, currFile.Sram.Unknown1);
			if (sramDiffBytes > 0)
				ConsolePrinter.PrintColoredLine(ConsoleColor.Green, Res.StatusTotalDiffBytesTemplate.InsertArgs(sramDiffBytes));

			static int GetSramOffset(string bufferName, out string name)
			{
				name = bufferName;

				return GetSramBufferOffset(bufferName);
			}

			var allDiffBytes = sramDiffBytes;

			var checksums = new StringBuilder();
			checksums.AppendLine($"{Resources.EnumChecksum} (2 {Res.Bytes} | {Resources.CompChangesAtEveryInSaveSlotSave}):");

			var timestamps = new StringBuilder();
			timestamps.AppendLine($"{nameof(SaveSlotDataSoE.Unknown12B)} (2 {Res.Bytes}):");

			if (optionCurrSlotIndex > -1 && optionCompSlotIndex > -1)
				allDiffBytes = CompareSaveSlots(optionCurrSlotIndex, optionCompSlotIndex);
			else
				for (var slotIndex = 0; slotIndex < 4; slotIndex++)
				{
					if (optionCurrSlotIndex > -1 && optionCurrSlotIndex != slotIndex) continue;

					allDiffBytes = CompareSaveSlots(slotIndex);
				}

			if (options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.NonSlotByteByByteComparison))
			{
				var nonSaveSlotUnknownDiffBytes = CompareByteArray(nameof(currFile.Sram.Unknown1), 0, currFile.Sram.Unknown1, compFile.Sram.Unknown1, false);

				if (nonSaveSlotUnknownDiffBytes > 0)
				{
					ConsolePrinter.PrintLine();
					ConsolePrinter.PrintColoredLine(ConsoleColor.Magenta, " ".Repeat(2) + $@"[ {Res.CompSectionNonSaveSlotUnknowns} ].......................................");
					ConsolePrinter.ResetColor();

					nonSaveSlotUnknownDiffBytes = CompareByteArray(nameof(currFile.Sram.Unknown1), SramOffsets.SramUnknown1, currFile.Sram.Unknown1, compFile.Sram.Unknown1, true);

					ConsolePrinter.PrintLine();
					ConsolePrinter.PrintColoredLine(nonSaveSlotUnknownDiffBytes > 0 ? ConsoleColor.Green : ConsoleColor.White, " ".Repeat(4) + Res.StatusNonSaveSlotUnknownsBytesTemplate.InsertArgs(nonSaveSlotUnknownDiffBytes));
					ConsolePrinter.ResetColor();

					allDiffBytes += nonSaveSlotUnknownDiffBytes;
				}
			}

			ConsolePrinter.PrintLine();

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
				ConsolePrinter.PrintLine();

				if (optionFlags.HasFlag(ComparisonFlagsSoE.ChecksumIfDifferent))
					ConsolePrinter.PrintColoredLine(ConsoleColor.Cyan, FormatAdditionalValues(nameof(checksums), checksums));

				if (optionFlags.HasFlag(ComparisonFlagsSoE.Unknown12BIfDifferent))
					ConsolePrinter.PrintColoredLine(ConsoleColor.Cyan, FormatAdditionalValues(nameof(timestamps), timestamps));
			}

			return allDiffBytes;

			int CompareSaveSlots(int currSlotIndex, int compSlotIndex = -1)
			{
				if (compSlotIndex == -1)
					compSlotIndex = currSlotIndex;

				var currSlot = currFile.GetSaveSlot(currSlotIndex);
				var currSlotBytes = currFile.GetSaveSlotBytes(currSlotIndex);

				var compSlot = compFile.GetSaveSlot(compSlotIndex);
				var compSlotBytes = compFile.GetSaveSlotBytes(compSlotIndex);

				var currSlotId = currSlotIndex + 1;
				var compSlotId = compSlotIndex + 1;

				var slotIdString = currSlotId.ToString();
				if (currSlotId != compSlotId)
					slotIdString += $" ({Resources.CompComparedWithOtherSaveSlotTemplate.InsertArgs(compSlotId)})";

				var padding = 25;
				var currSlotName = $"{$"({Res.EnumCurrentFile})".PadRight(padding)} {Res.CompSlot} {currSlotId}";
				var currSlotNameString = $"{" ".Repeat(2)}{currSlotName}";
			
				if (optionFlags.HasFlag(ComparisonFlagsSoE.Checksum) || compSlot.Checksum != currSlot.Checksum)
					checksums.AppendLine($"{currSlotNameString}: {currSlot.Checksum.PadLeft()}");

				if (optionFlags.HasFlag(ComparisonFlagsSoE.Unknown12B) || compSlot.Data.Unknown12B != currSlot.Data.Unknown12B)
					timestamps.AppendLine($"{currSlotNameString}: {currSlot.Data.Unknown12B.PadLeft()}");

				if (compSlotBytes.SequenceEqual(currSlotBytes)) return allDiffBytes;

				var compSlotName = $"{$"({Res.EnumComparisonFile})".PadRight(padding)} {Res.CompSlot} {compSlotId}";
				var compSlotNameString = $"{" ".Repeat(2)}{compSlotName}";

				checksums.AppendLine($"{compSlotNameString}: {compSlot.Checksum.PadLeft()}");
				timestamps.AppendLine($"{compSlotNameString}: {compSlot.Data.Unknown12B.PadLeft()}");

				ConsolePrinter.PrintColoredLine(ConsoleColor.Yellow, $@"[ {Res.CompSlot} {slotIdString} ]---------------------------------------------");
				ConsolePrinter.ResetColor();

				ConsolePrinter.PrintLine();
				ConsolePrinter.PrintColoredLine(ConsoleColor.Magenta, " ".Repeat(2) + $@"[ {Res.CompUnknownAreasOnly} ].......................................");
				ConsolePrinter.ResetColor();

				var slotDiffBytes = CompareSaveSlot(currSlot, compSlot, options);
				if (slotDiffBytes > 0)
				{
					ConsolePrinter.PrintLine();
					ConsolePrinter.PrintColoredLine(ConsoleColor.Green, " ".Repeat(4) + Res.StatusUnknownsChangedBytesTemplate.InsertArgs(slotDiffBytes));
					ConsolePrinter.ResetColor();
				}

				if (!optionFlags.HasFlag(ComparisonFlagsSoE.SlotByteByByteComparison))
				{
					allDiffBytes += slotDiffBytes;
					return allDiffBytes;
				}

				ConsoleHelper.EnsureMinConsoleWidth(165);

				bufferName = $"{nameof(compFile.Sram.SaveSlots)} {currSlotId}";
				var bufferOffset = SramOffsets.FirstSaveSlot + currSlotId * SramSizes.SaveSlot.All;
				var slotBufferDiffBytes = CompareByteArray(bufferName, bufferOffset, currSlotBytes, compSlotBytes, false);
				if (slotBufferDiffBytes > slotDiffBytes)
				{
					ConsolePrinter.PrintLine();
					ConsolePrinter.PrintColoredLine(ConsoleColor.Magenta, $@"{" ".Repeat(2)}[ {Res.CompSectionSaveSlotChangedTemplate} ]...................................".InsertArgs(currSlotIndex));
					// ReSharper disable once RedundantArgumentDefaultValue

					CompareByteArray(bufferName, bufferOffset, currSlotBytes, compSlotBytes, true, SramOffsets.SaveSlot.GetNameFromOffset);
					ConsolePrinter.PrintLine();
					ConsolePrinter.PrintColoredLine(slotDiffBytes > 0 ? ConsoleColor.Green : ConsoleColor.White, " ".Repeat(4) + Res.StatusSaveSlotChangedBytesTemplate.InsertArgs(slotIdString, slotBufferDiffBytes));
					ConsolePrinter.ResetColor();
				}

				allDiffBytes += slotBufferDiffBytes;
				return allDiffBytes;
			}
		}

		protected virtual string FormatAdditionalValues(string name, StringBuilder values) => values.Replace(Environment.NewLine, ConsolePrinter.NewLine).ToString();

		protected virtual void PrintSaveSlotValidationStatus(SramFileSoE currFile, SramFileSoE compFile)
		{
			ConsolePrinter.PrintSectionHeader();
			ConsolePrinter.PrintColoredLine(ConsoleColor.DarkYellow, $@"{Resources.CompValidationStatus}:");

			OnPrintSaveSlotValidationStatus(Res.EnumCurrentFile, currFile);
			OnPrintSaveSlotValidationStatus(Res.EnumComparisonFile, compFile);
		}

		protected virtual void OnPrintSaveSlotValidationStatus(string name, ISramFile file)
		{
			ConsolePrinter.PrintColored(ConsoleColor.Gray, $@"{name}:".PadRight(15));
			ConsolePrinter.PrintColored(ConsoleColor.DarkYellow, $@" {Res.CompSlot} (1-4)");
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

		#endregion CompareSram

		#region CompareSaveSlot

		/// <inheritdoc cref="SramComparerBase{TSramFile,TSaveSlot}"/>
		public override int CompareSaveSlot(SaveSlotSoE currSaveSlot, SaveSlotSoE compSaveSlot, IOptions options)
		{
			ref var currData = ref currSaveSlot.Data;
			ref var compData = ref compSaveSlot.Data;

			var delimiter = StructDelimiter;

			// ReSharper disable once InlineOutVariableDeclaration
			string name;
			// ReSharper disable once JoinDeclarationAndInitializer
			int diffBytes, offset;

			//Unknown 4
			offset = GetSaveSlotOffset($"{nameof(currData.Unknown4_BoyBuff)}{delimiter}{nameof(currData.Unknown4_BoyBuff.BuffFlags)}", out name);
			diffBytes = CompareUInt16(name, offset, currData.Unknown4_BoyBuff.BuffFlags.ToUShort(), compData.Unknown4_BoyBuff.BuffFlags.ToUShort());

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown4_BoyBuff)}{delimiter}{nameof(currData.Unknown4_BoyBuff.Unknown1)}", out name);
			diffBytes += CompareUInt16(name, offset, currData.Unknown4_BoyBuff.Unknown1, compData.Unknown4_BoyBuff.Unknown1);

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown4_BoyBuff)}{delimiter}{nameof(currData.Unknown4_BoyBuff.Unknown2)}", out name);
			diffBytes += CompareByteArray(name, offset, currData.Unknown4_BoyBuff.Unknown2, compData.Unknown4_BoyBuff.Unknown2);

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown4_BoyBuff)}{delimiter}{nameof(currData.Unknown4_BoyBuff.Unknown3)}", out name);
			diffBytes += CompareUInt16(name, offset, currData.Unknown4_BoyBuff.Unknown3, compData.Unknown4_BoyBuff.Unknown3);

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown4_BoyBuff)}{delimiter}{nameof(currData.Unknown4_BoyBuff.Unknown4)}", out name);
			diffBytes += CompareUInt16(name, offset, currData.Unknown4_BoyBuff.Unknown4, compData.Unknown4_BoyBuff.Unknown4);
			//Unknown 4

			offset = GetSaveSlotOffset(nameof(currData.Unknown5), out name);
			diffBytes += CompareByteArray(name, offset, currData.Unknown5, compData.Unknown5);

			offset = GetSaveSlotOffset(nameof(currData.Unknown6), out name);
			diffBytes += CompareByteArray(name, offset, currData.Unknown6, compData.Unknown6);

			//Unknown 7
			offset = GetSaveSlotOffset($"{nameof(currData.Unknown7_DogBuff)}{delimiter}{nameof(currData.Unknown7_DogBuff.BuffFlags)}", out name);
			diffBytes += CompareUInt16(name, offset, currData.Unknown7_DogBuff.BuffFlags.ToUShort(), compData.Unknown7_DogBuff.BuffFlags.ToUShort());

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown7_DogBuff)}{delimiter}{nameof(currData.Unknown7_DogBuff.Unknown1)}", out name);
			diffBytes += CompareUInt16(name, offset, currData.Unknown7_DogBuff.Unknown1, compData.Unknown7_DogBuff.Unknown1);

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown7_DogBuff)}{delimiter}{nameof(currData.Unknown7_DogBuff.Unknown2)}", out name);
			diffBytes += CompareByteArray(name, offset, currData.Unknown7_DogBuff.Unknown2, compData.Unknown7_DogBuff.Unknown2);

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown7_DogBuff)}{delimiter}{nameof(currData.Unknown7_DogBuff.Unknown3)}", out name);
			diffBytes += CompareUInt16(name, offset, currData.Unknown7_DogBuff.Unknown3, compData.Unknown7_DogBuff.Unknown3);

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown7_DogBuff)}{delimiter}{nameof(currData.Unknown7_DogBuff.Unknown4)}", out name);
			diffBytes += CompareUInt16(name, offset, currData.Unknown7_DogBuff.Unknown4, compData.Unknown7_DogBuff.Unknown4);
			//Unknown 7

			offset = GetSaveSlotOffset(nameof(currData.Unknown8), out name);
			diffBytes += CompareByteArray(name, offset, currData.Unknown8, compData.Unknown8);

			offset = GetSaveSlotOffset(nameof(currData.Unknown9), out name);
			diffBytes += CompareByteArray(name, offset, currData.Unknown9, compData.Unknown9);

			offset = GetSaveSlotOffset(nameof(currData.Unknown10), out name);
			diffBytes += CompareByteArray(name, offset, currData.Unknown10, compData.Unknown10);

			offset = GetSaveSlotOffset(nameof(currData.Unknown11), out name);
			diffBytes += CompareByteArray(name, offset, currData.Unknown11, compData.Unknown11);

			// 12 A - C
			offset = GetSaveSlotOffset(nameof(currData.Unknown12A), out name);
			diffBytes += CompareByteArray(name, offset, currData.Unknown12A, compData.Unknown12A);

			if (options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.Unknown12BIfDifferent))
			{
				offset = GetSaveSlotOffset(nameof(currData.Unknown12B), out name);
				diffBytes += CompareUInt16(name, offset, currData.Unknown12B,
					compData.Unknown12B);
			}

			offset = GetSaveSlotOffset(nameof(currData.Unknown12C), out name);
			diffBytes += CompareUInt32(name, offset, currData.Unknown12C, compData.Unknown12C);
			// 

			offset = GetSaveSlotOffset(nameof(currData.Unknown13), out name);
			diffBytes += CompareByteArray(name, offset, currData.Unknown13, compData.Unknown13);

			offset = GetSaveSlotOffset(nameof(currData.Unknown14), out name);
			diffBytes += CompareByteArray(name, offset, 
				BitConverter.GetBytes(currData.Unknown14.ToUInt()), 
				BitConverter.GetBytes(compData.Unknown14.ToUInt()));

			//Unknown 15
			offset = GetSaveSlotOffset($"{nameof(currData.Unknown15)}{delimiter}{nameof(currData.Unknown15.Offset0To16)}", out name);
			diffBytes += CompareByteArray(name, offset, currData.Unknown15.Offset0To16, compData.Unknown15.Offset0To16);

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown15)}{delimiter}{nameof(currData.Unknown15.Offset17)}", out name);
			diffBytes += CompareByte(name, offset, currData.Unknown15.Offset17.ToByte(), compData.Unknown15.Offset17.ToByte());

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown15)}{delimiter}{nameof(currData.Unknown15.Offset18)}", out name);
			diffBytes += CompareByte(name, offset, currData.Unknown15.Offset18.ToByte(), compData.Unknown15.Offset18.ToByte());

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown15)}{delimiter}{nameof(currData.Unknown15.Offset19)}", out name);
			diffBytes += CompareByte(name, offset, currData.Unknown15.Offset19.ToByte(), compData.Unknown15.Offset19.ToByte());

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown15)}{delimiter}{nameof(currData.Unknown15.Offset20)}", out name);
			diffBytes += CompareByte(name, offset, currData.Unknown15.Offset20.ToByte(), compData.Unknown15.Offset20.ToByte());

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown15)}{delimiter}{nameof(currData.Unknown15.Offset21)}", out name);
			diffBytes += CompareByte(name, offset, currData.Unknown15.Offset21, compData.Unknown15.Offset21);

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown15)}{delimiter}{nameof(currData.Unknown15.Offset22)}", out name);
			diffBytes += CompareByte(name, offset, currData.Unknown15.Offset22, compData.Unknown15.Offset22);

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown15)}{delimiter}{nameof(currData.Unknown15.Offset22)}", out name);
			diffBytes += CompareByte(name, offset, currData.Unknown15.Offset22, compData.Unknown15.Offset22);

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown15)}{delimiter}{nameof(currData.Unknown15.Offset23)}", out name);
			diffBytes += CompareByte(name, offset, currData.Unknown15.Offset23, compData.Unknown15.Offset23);

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown15)}{delimiter}{nameof(currData.Unknown15.Offset24)}", out name);
			diffBytes += CompareByte(name, offset, currData.Unknown15.Offset24.ToByte(), compData.Unknown15.Offset24.ToByte());

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown15)}{delimiter}{nameof(currData.Unknown15.Offset25To117)}", out name);
			diffBytes += CompareByteArray(name, offset, currData.Unknown15.Offset25To117, compData.Unknown15.Offset25To117);
			//Unknown 15

			// 16 A - C
			offset = GetSaveSlotOffset(nameof(currData.Unknown16A), out name);
			diffBytes += CompareByteArray(name, offset, currData.Unknown16A, compData.Unknown16A);

			offset = GetSaveSlotOffset(nameof(currData.Unknown16B_GothicaFlags), out name);
			diffBytes += CompareByteArray(name, offset, 
				BitConverter.GetBytes(currData.Unknown16B_GothicaFlags.ToUInt()), 
				BitConverter.GetBytes(compData.Unknown16B_GothicaFlags.ToUInt()));

			// 16 C
			offset = GetSaveSlotOffset(nameof(currData.Unknown16C), out name);
			diffBytes += CompareByte(name, offset,
				currData.Unknown16C.Offset0.ToByte(),
				compData.Unknown16C.Offset0.ToByte());

			offset = GetSaveSlotOffset($"{nameof(currData.Unknown16C)}{delimiter}{nameof(currData.Unknown16C.Offset1To5)}", out name);
			diffBytes += CompareByteArray(name, offset, currData.Unknown16C.Offset1To5, compData.Unknown16C.Offset1To5);
			// 

			offset = GetSaveSlotOffset(nameof(currData.Unknown17), out name);
			diffBytes += CompareByteArray(name, offset, currData.Unknown17, compData.Unknown17);

			offset = GetSaveSlotOffset(nameof(currData.Unknown18), out name);
			diffBytes += CompareByteArray(name, offset, currData.Unknown18, compData.Unknown18);

			return diffBytes;

			static int GetSaveSlotOffset(string bufferName, out string name)
			{
				name = bufferName;
				return  GetSaveSlotBufferOffset(bufferName);
			}
		}

		#endregion CompareSaveSlot
	}
}
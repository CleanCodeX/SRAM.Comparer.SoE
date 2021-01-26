using System;
using System.Linq;
using System.Text;
using Common.Shared.Min.Extensions;
using IO.Extensions;
using IO.Models;
using SRAM.Comparison.Helpers;
using SRAM.Comparison.Services;
using SRAM.Comparison.SoE.Enums;
using SRAM.Comparison.SoE.Properties;
using SRAM.SoE.Models;
using SRAM.SoE.Models.Structs;
using static SRAM.Comparison.SoE.Helpers.UnkownBufferOffsetFinder;
using Res = SRAM.Comparison.Properties.Resources;
// ReSharper disable RedundantArgumentDefaultValue

namespace SRAM.Comparison.SoE.Services
{
	/// <summary>S-RAM comparer implementation for SoE</summary>
	/// <inheritdoc cref="SramComparerBase{TSramFile,TSaveSlot}"/>
	public class SramComparerSoE : SramComparerBase<SramFileSoE, SaveSlotSoE>
	{
		public SramComparerSoE() : base(ServiceCollection.ConsolePrinter) {}
		public SramComparerSoE(IConsolePrinter consolePrinter) :base(consolePrinter) {}

		#region Compare S-RAM

		/// <inheritdoc cref="SramComparerBase{TSramFile,TSaveSlot}"/>
		protected override int OnCompareSram(SramFileSoE currFile, SramFileSoE compFile, IOptions options)
		{
			var optionFlags = (ComparisonFlagsSoE)options.ComparisonFlags;

			if(optionFlags.HasFlag(ComparisonFlagsSoE.ChecksumStatus))
				PrintSaveSlotChecksumValidation(currFile, compFile);

			ConsolePrinter.PrintLine();

			var optionCurrSlotIndex = options.CurrentFileSaveSlot - 1;
			var optionCompSlotIndex = options.ComparisonFileSaveSlot - 1;
			
			var slotComparisonMode = optionCompSlotIndex > -1
				? Res.StatusDifferentSaveSlotComparisonTemplate.InsertArgs(options.CurrentFileSaveSlot,
					options.ComparisonFileSaveSlot)
				: optionCurrSlotIndex > -1
					? string.Format(Res.StatusSingleSaveSlotComparisonTemplate, options.CurrentFileSaveSlot)
					: Res.StatusAllSaveSlotsComparison;
			ConsolePrinter.PrintColoredLine(ConsoleColor.Yellow, slotComparisonMode);

			ConsolePrinter.PrintLine();

			var offset = GetSramOffset(nameof(compFile.Struct.Unknown19), out var bufferName);
			var sramDiffBytes = CompareByteArray(Res.CompSram + ":" + bufferName, offset, compFile.Struct.Unknown19, currFile.Struct.Unknown19);
			if (sramDiffBytes > 0)
				ConsolePrinter.PrintColoredLine(ConsoleColor.Green, Res.StatusTotalDiffBytesTemplate.InsertArgs(sramDiffBytes));

			static int GetSramOffset(string bufferName, out string name)
			{
				name = bufferName;

				return GetSramBufferOffset(bufferName);
			}

			var allDiffBytes = sramDiffBytes;

			StringBuilder checksums = new(), timestamps = new();

			checksums.AppendLine($"{Resources.EnumChecksum} (2 {Res.Bytes} | {Resources.CompChangesAtEveryInSaveSlotSave}):");
			timestamps.AppendLine($"{nameof(SramSizes.SaveSlot.Unknown12B)} ({SramSizes.SaveSlot.Unknown12B} {Res.Bytes}):");

			if (optionCurrSlotIndex > -1 && optionCompSlotIndex > -1)
				allDiffBytes = CompareSaveSlots(optionCurrSlotIndex, optionCompSlotIndex);
			else
				for (var slotIndex = 0; slotIndex < 4; slotIndex++)
				{
					if (optionCurrSlotIndex > -1 && optionCurrSlotIndex != slotIndex) continue;

					allDiffBytes = CompareSaveSlots(slotIndex);
				}

			if (options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.NonSlotComparison))
			{
				var nonSaveSlotUnknownDiffBytes = CompareByteArray(nameof(currFile.Struct.Unknown19), 0, currFile.Struct.Unknown19, compFile.Struct.Unknown19, false);

				if (nonSaveSlotUnknownDiffBytes > 0)
				{
					ConsolePrinter.PrintLine();
					ConsolePrinter.PrintColoredLine(ConsoleColor.Magenta, " ".Repeat(2) + $@"[ {Res.CompSectionNonSaveSlotUnknowns} ]");
					ConsolePrinter.ResetColor();

					nonSaveSlotUnknownDiffBytes = CompareByteArray(nameof(currFile.Struct.Unknown19), SramOffsets.Unknown19, currFile.Struct.Unknown19, compFile.Struct.Unknown19, true);

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
			ConsolePrinter.PrintColoredLine(color,
				allDiffBytes > 0
					? @$"== {Res.StatusSramChangedBytesTemplate.InsertArgs(allDiffBytes)} ".PadRight(borderLength, '=')
					: @$"== {Res.StatusNoSramBytesChanged} =".PadRight(borderLength, '='));

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

				var currSlot = currFile.GetSegment(currSlotIndex);
				var currSlotBytes = currFile.GetSegmentBytes(currSlotIndex);

				var compSlot = compFile.GetSegment(compSlotIndex);
				var compSlotBytes = compFile.GetSegmentBytes(compSlotIndex);

				var currSlotId = currSlotIndex + 1;
				var compSlotId = compSlotIndex + 1;

				var slotIdString = currSlotId.ToString();
				if (currSlotId != compSlotId)
					slotIdString += $" ({Resources.CompComparedWithOtherSaveSlotTemplate.InsertArgs(compSlotId)})";

				const int padding = 25;
				var currSlotName = $"{$"({Res.EnumCurrentFile})".PadRight(padding)} {Res.CompSlot} {currSlotId}";
				var currSlotNameString = $"{" ".Repeat(2)}{currSlotName}";
			
				if (optionFlags.HasFlag(ComparisonFlagsSoE.Checksum) || compSlot.Checksum != currSlot.Checksum)
					checksums.AppendLine($"{currSlotNameString}: {currSlot.Checksum.PadLeft()}");

				if (optionFlags.HasFlag(ComparisonFlagsSoE.Unknown12B) || compSlot.Data.EquippedStuff_Moneys_Levels.Unknown12B != currSlot.Data.EquippedStuff_Moneys_Levels.Unknown12B)
					timestamps.AppendLine($"{currSlotNameString}: {currSlot.Data.EquippedStuff_Moneys_Levels.Unknown12B.PadLeft()}");

				if (compSlotBytes.SequenceEqual(currSlotBytes)) return allDiffBytes;

				var compSlotName = $"{$"({Res.EnumComparisonFile})".PadRight(padding)} {Res.CompSlot} {compSlotId}";
				var compSlotNameString = $"{" ".Repeat(2)}{compSlotName}";

				checksums.AppendLine($"{compSlotNameString}: {compSlot.Checksum.PadLeft()}");
				timestamps.AppendLine($"{compSlotNameString}: {compSlot.Data.EquippedStuff_Moneys_Levels.Unknown12B.PadLeft()}");

				ConsolePrinter.PrintColoredLine(ConsoleColor.Yellow, $@"[ {Res.CompSlot} {slotIdString} ]");
				ConsolePrinter.ResetColor();

				ConsolePrinter.PrintLine();
				ConsolePrinter.PrintColoredLine(ConsoleColor.Magenta, " ".Repeat(2) + $@"[ {Res.CompUnknownAreasOnly} ]");
				ConsolePrinter.ResetColor();

				var slotDiffBytes = OnCompareSaveSlot(currSlot, compSlot, options);
				if (slotDiffBytes > 0)
				{
					ConsolePrinter.PrintLine();
					ConsolePrinter.PrintColoredLine(ConsoleColor.Green, " ".Repeat(4) + Res.StatusUnknownsChangedBytesTemplate.InsertArgs(slotDiffBytes));
					ConsolePrinter.ResetColor();
				}

				if (!optionFlags.HasFlag(ComparisonFlagsSoE.SlotByteComparison))
				{
					allDiffBytes += slotDiffBytes;
					return allDiffBytes;
				}

				ConsoleHelper.EnsureMinConsoleWidth(ComparisonConsoleWidth);

				bufferName = $"{nameof(compFile.Struct.SaveSlots)} {currSlotId}";
				var bufferOffset = SramOffsets.FirstSaveSlot + currSlotId * SramSizes.SaveSlot.All;
				var slotBufferDiffBytes = CompareByteArray(bufferName, bufferOffset, currSlotBytes, compSlotBytes, false);
				if (slotBufferDiffBytes > slotDiffBytes)
				{
					ConsolePrinter.PrintLine();
					ConsolePrinter.PrintColoredLine(ConsoleColor.Magenta, $@"{" ".Repeat(2)}[ {Res.CompSectionSaveSlotChangedTemplate} ]".InsertArgs(currSlotIndex));
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

		protected virtual void PrintSaveSlotChecksumValidation(SramFileSoE currFile, SramFileSoE compFile)
		{
			ConsolePrinter.PrintSectionHeader();
			ConsolePrinter.PrintColoredLine(ConsoleColor.DarkYellow, $@"{Resources.CompChecksumValidation}:");

			OnPrintSaveSlotChecksumValidation(Res.EnumCurrentFile, currFile);
			OnPrintSaveSlotChecksumValidation(Res.EnumComparisonFile, compFile);
		}

		protected virtual void OnPrintSaveSlotChecksumValidation(string name, IMultiSegmentFile file)
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

		#region Compare SaveSlot

		/// <inheritdoc cref="SramComparerBase{TSramFile,TSaveSlot}"/>
		protected override int OnCompareSaveSlot(SaveSlotSoE currSaveSlot, SaveSlotSoE compSaveSlot, IOptions options)
		{
			ref var currData = ref currSaveSlot.Data;
			ref var compData = ref compSaveSlot.Data;

			const string delimiter = StructDelimiter;

			// ReSharper disable once InlineOutVariableDeclaration
			string name;
			// ReSharper disable once JoinDeclarationAndInitializer
			int diffBytes = 0, offset;

			//Unknown 4
			for (var i = 0; i < 4; ++i)
			{
				var parentName = nameof(currData.BoyStatusBuffs);
				offset = GetSaveSlotOffset(BuildBufferName(parentName, i, nameof(CharacterBuffStatus.Id)), out name);
				diffBytes += CompareUInt16(name, offset, currData.BoyStatusBuffs.Status[i].Id, compData.BoyStatusBuffs.Status[i].Id);

				offset = GetSaveSlotOffset(BuildBufferName(parentName, i, nameof(CharacterBuffStatus.Timer)), out name);
				diffBytes += CompareUInt16(name, offset, currData.BoyStatusBuffs.Status[i].Timer, compData.BoyStatusBuffs.Status[i].Timer);

				offset = GetSaveSlotOffset(BuildBufferName(parentName, i, nameof(CharacterBuffStatus.Boost)), out name);
				diffBytes += CompareUInt16(name, offset, currData.BoyStatusBuffs.Status[i].Boost, compData.BoyStatusBuffs.Status[i].Boost);
			}
			//Unknown 4

			//Unknown 7
			for (var i = 0; i < 4; ++i)
			{
				var parentName = nameof(currData.DogStatusBuffs);
				offset = GetSaveSlotOffset(BuildBufferName(parentName, i, nameof(CharacterBuffStatus.Id)), out name);
				diffBytes += CompareUInt16(name, offset, currData.DogStatusBuffs.Status[i].Id, compData.DogStatusBuffs.Status[i].Id);

				offset = GetSaveSlotOffset(BuildBufferName(parentName, i, nameof(CharacterBuffStatus.Timer)), out name);
				diffBytes += CompareUInt16(name, offset, currData.DogStatusBuffs.Status[i].Timer, compData.DogStatusBuffs.Status[i].Timer);

				offset = GetSaveSlotOffset(BuildBufferName(parentName, i, nameof(CharacterBuffStatus.Boost)), out name);
				diffBytes += CompareUInt16(name, offset, currData.DogStatusBuffs.Status[i].Boost, compData.DogStatusBuffs.Status[i].Boost);
			}
			//Unknown 7

			static string BuildBufferName(string parentName, int index, string bufferName) => $"{parentName}{index}{StructDelimiter}{bufferName}";

			offset = GetSaveSlotOffset(nameof(currData.EquippedStuff_Moneys_Levels.Unknown9), out name);
			diffBytes += CompareUInt16(name, offset, currData.EquippedStuff_Moneys_Levels.Unknown9, compData.EquippedStuff_Moneys_Levels.Unknown9);

			offset = GetSaveSlotOffset(nameof(currData.EquippedStuff_Moneys_Levels.Unknown10), out name);
			diffBytes += CompareByteArray(name, offset, currData.EquippedStuff_Moneys_Levels.Unknown10.AsArray(), compData.EquippedStuff_Moneys_Levels.Unknown10.AsArray());

			offset = GetSaveSlotOffset(nameof(currData.EquippedStuff_Moneys_Levels.Unknown11), out name);
			diffBytes += CompareByteArray(name, offset, currData.EquippedStuff_Moneys_Levels.Unknown11, compData.EquippedStuff_Moneys_Levels.Unknown11);

			//Unknown 12 A - C
			offset = GetSaveSlotOffset(nameof(currData.EquippedStuff_Moneys_Levels.Unknown12A), out name);
			diffBytes += CompareByteArray(name, offset, currData.EquippedStuff_Moneys_Levels.Unknown12A, compData.EquippedStuff_Moneys_Levels.Unknown12A);

			if (options.ComparisonFlags.HasFlag(ComparisonFlagsSoE.Unknown12BIfDifferent))
			{
				offset = GetSaveSlotOffset(nameof(currData.EquippedStuff_Moneys_Levels.Unknown12B), out name);
				diffBytes += CompareUInt32(name, offset, currData.EquippedStuff_Moneys_Levels.Unknown12B,
					compData.EquippedStuff_Moneys_Levels.Unknown12B);
			}

			offset = GetSaveSlotOffset(nameof(currData.EquippedStuff_Moneys_Levels.Unknown12C), out name);
			diffBytes += CompareUInt32(name, offset, currData.EquippedStuff_Moneys_Levels.Unknown12C, compData.EquippedStuff_Moneys_Levels.Unknown12C);
			// 

			offset = GetSaveSlotOffset(nameof(currData.AlchemyLevels.Unknown13), out name);
			diffBytes += CompareByteArray(name, offset, currData.AlchemyLevels.Unknown13, compData.AlchemyLevels.Unknown13);

			offset = GetSaveSlotOffset(nameof(currData.Collectables_Spots.Unknown14), out name);
			diffBytes += CompareByteArray(name, offset, 
				BitConverter.GetBytes(currData.Collectables_Spots.Unknown14.ToUShort()), 
				BitConverter.GetBytes(compData.Collectables_Spots.Unknown14.ToUShort()));

			offset = GetSaveSlotOffset(nameof(currData.Collectables_Spots.Unknown14B), out name);
			diffBytes += CompareByte(name, offset,
				currData.Collectables_Spots.Unknown14B,
				compData.Collectables_Spots.Unknown14B);

			//Unknown 15
			offset = GetSaveSlotOffset($"{nameof(currData.Collectables_Spots.Unknown15)}{delimiter}{nameof(currData.Collectables_Spots.Unknown15.Offset0To23)}", out name);
			diffBytes += CompareByteArray(name, offset, currData.Collectables_Spots.Unknown15.Offset0To23, compData.Collectables_Spots.Unknown15.Offset0To23);
			//Unknown 15

			#region Unknown 16

			offset = GetSaveSlotOffset(nameof(currData.Collectables_Spots.Unknown16A), out name);
			diffBytes += CompareByteArray(name, offset, currData.Collectables_Spots.Unknown16A, compData.Collectables_Spots.Unknown16A);

			offset = GetSaveSlotOffset(nameof(currData.Collectables_Spots.Unknown16B_GothicaFlags), out name);
			diffBytes += CompareByteArray(name, offset, 
				BitConverter.GetBytes(currData.Collectables_Spots.Unknown16B_GothicaFlags.ToUInt()), 
				BitConverter.GetBytes(compData.Collectables_Spots.Unknown16B_GothicaFlags.ToUInt()));

			offset = GetSaveSlotOffset(nameof(currData.Collectables_Spots.Unknown16C), out name);
			diffBytes += CompareByte(name, offset,
				currData.Collectables_Spots.Unknown16C.Offset0.ToByte(),
				compData.Collectables_Spots.Unknown16C.Offset0.ToByte());

			offset = GetSaveSlotOffset($"{nameof(currData.Collectables_Spots.Unknown16C)}{delimiter}{nameof(currData.Collectables_Spots.Unknown16C.Offset1To5)}", out name);
			diffBytes += CompareByteArray(name, offset, currData.Collectables_Spots.Unknown16C.Offset1To5, compData.Collectables_Spots.Unknown16C.Offset1To5);

			#endregion Unknown 16

			#region Unknown 17
			offset = GetSaveSlotOffset(nameof(currData.PossessedStuff.Unknown17A), out name);
			diffBytes += CompareByteArray(name, offset, currData.PossessedStuff.Unknown17A, compData.PossessedStuff.Unknown17A);

			offset = GetSaveSlotOffset(nameof(currData.PossessedStuff.Unknown17B), out name);
			diffBytes += CompareByteArray(name, offset, currData.PossessedStuff.Unknown17B, compData.PossessedStuff.Unknown17B);

			offset = GetSaveSlotOffset(nameof(currData.LastLanding_CurrentWeapon.Unknown17C), out name);
			diffBytes += CompareByteArray(name, offset, currData.LastLanding_CurrentWeapon.Unknown17C, compData.LastLanding_CurrentWeapon.Unknown17C);

			offset = GetSaveSlotOffset(nameof(currData.LastLanding_CurrentWeapon.Unknown17D), out name);
			diffBytes += CompareByteArray(name, offset, currData.LastLanding_CurrentWeapon.Unknown17D, compData.LastLanding_CurrentWeapon.Unknown17D);

			offset = GetSaveSlotOffset(nameof(currData.LastLanding_CurrentWeapon.Unknown17E), out name);
			diffBytes += CompareByteArray(name, offset, currData.LastLanding_CurrentWeapon.Unknown17E, compData.LastLanding_CurrentWeapon.Unknown17E);

			offset = GetSaveSlotOffset(nameof(currData.LastLanding_CurrentWeapon.Unknown17F), out name);
			diffBytes += CompareByteArray(name, offset, currData.LastLanding_CurrentWeapon.Unknown17F, compData.LastLanding_CurrentWeapon.Unknown17F);

			offset = GetSaveSlotOffset(nameof(currData.TradeGoods.Unknown17G), out name);
			diffBytes += CompareByteArray(name, offset, currData.TradeGoods.Unknown17G, compData.TradeGoods.Unknown17G);

			#endregion Unknown17

			offset = GetSaveSlotOffset(nameof(currData.TradeGoods.Unknown18), out name);
			diffBytes += CompareByteArray(name, offset, currData.TradeGoods.Unknown18, compData.TradeGoods.Unknown18);

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
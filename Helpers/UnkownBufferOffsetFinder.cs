using System;
using System.Runtime.InteropServices;
using Common.Shared.Min.Extensions;
using SRAM.SoE.Models;
using SRAM.SoE.Models.Structs;

// ReSharper disable InconsistentNaming

namespace SRAM.Comparison.SoE.Helpers
{
	/// <summary>List of buffers in sub-structures</summary>
	internal static class UnkownBufferOffsetFinder
	{
		internal const string StructDelimiter = "__";

		public static int GetSaveSlotBufferOffset(string bufferName) => bufferName.Contains(StructDelimiter)
			? (int) bufferName.ParseEnum<SaveSlotUnknownOffset>()
			: InternalGetBufferOffset<SaveSlotDataSoE>(bufferName) + SramSizes.SaveSlot.Checksum;

		public static int GetSramBufferOffset(string bufferName) => bufferName.Contains(StructDelimiter)
			? (int)bufferName.ParseEnum<SaveSlotUnknownOffset>()
			: InternalGetBufferOffset<SramSoE>(bufferName);

		private static int InternalGetBufferOffset<TParentBuffer>(string bufferName)
			where TParentBuffer: struct
		{
			var index = bufferName.IndexOf('.'); // check for path info
			if (index == -1)
				return FindFieldOffset<TParentBuffer>(bufferName);

			var parentFieldType = GetParentStructType<TParentBuffer>(bufferName, index, out var parentOffset);
			var fieldName = bufferName.Substring(index + 1);

			return parentOffset + (int) Marshal.OffsetOf(parentFieldType, fieldName);
		}

		private static Type GetParentStructType<TParentBuffer>(string fieldName, int index, out int parentOffset)
			where TParentBuffer : struct
		{
			var parentStructFieldName = fieldName.Substring(0, index);
			parentOffset = (int) Marshal.OffsetOf<TParentBuffer>(parentStructFieldName);

			return typeof(TParentBuffer).GetField(parentStructFieldName)!.FieldType;
		}

		private static int FindFieldOffset<TParentBuffer>(string fieldName)
			where TParentBuffer : struct
		{
			var parentOffset = 0;
			var parentType = typeof(TParentBuffer);
			if (parentType.GetField(fieldName) is null)
			{
				foreach (var fieldInfo in parentType.GetFields())
				{
					var fieldType = fieldInfo.FieldType;
					if (fieldType.GetField(fieldName) is not { } foundFieldInfo) continue;

					parentType = fieldType;
					parentOffset = (int) Marshal.OffsetOf<TParentBuffer>(fieldInfo.Name);

					break;
				}
			}

			return parentOffset + (int) Marshal.OffsetOf(parentType, fieldName);
		}
	}
}

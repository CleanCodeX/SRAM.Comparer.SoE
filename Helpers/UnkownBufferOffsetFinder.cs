using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Common.Shared.Min.Extensions;
using IO.Extensions;
using IO.Helpers;
using SRAM.SoE.Models;
using SRAM.SoE.Models.Structs;

// ReSharper disable InconsistentNaming

namespace SRAM.Comparison.SoE.Helpers
{
	/// <summary>List of buffers in sub-structures</summary>
	internal static class UnkownBufferOffsetFinder
	{
		private const string OffsetNotFoundTemplate = "Offset for {0} could not be found.";
		private const string NamedOffsetNotDefinedTemplate = "Named offset {0} is not defined.";

		internal const string StructDelimiter = "__";

		public static int GetSaveSlotBufferOffset(string fieldName) => 
			fieldName.Contains(StructDelimiter) 
			? (int)fieldName.ParseEnum<SaveSlotUnknownOffset>().GetOrThrowIfDefault(fieldName, OffsetNotFoundTemplate.InsertArgs(fieldName))
			: InternalGetBufferOffset(typeof(SaveSlotDataSoE), fieldName).GetOrThrowIfDefault(fieldName, NamedOffsetNotDefinedTemplate.InsertArgs(fieldName)) + SramSizes.Checksum;

		public static int GetSramBufferOffset(string fieldName) => 
			fieldName.Contains(StructDelimiter)
			? (int)fieldName.ParseEnum<SaveSlotUnknownOffset>().GetOrThrowIfDefault(fieldName, OffsetNotFoundTemplate.InsertArgs(fieldName))
			: InternalGetBufferOffset(typeof(SramSoE), fieldName).GetOrThrowIfDefault(fieldName, NamedOffsetNotDefinedTemplate.InsertArgs(fieldName));

		private static int InternalGetBufferOffset(Type type, string bufferName)
		{
			var index = bufferName.IndexOf('.'); // check for path info
			if (index == -1)
			{
				var offset = FindFieldOffset(type, bufferName);
				if (offset > -1)
					return offset;

				throw new ArgumentException($"Type [{type.Name}] does not contain field [{bufferName}]");
			}

			var parentFieldType = GetBaseFieldTypeAndOffset(type, bufferName[..index], out var parentOffset);
			var fieldName = bufferName[(index + 1)..];

			return parentOffset + InternalGetBufferOffset(parentFieldType, fieldName);
		}

		private static Type GetBaseFieldTypeAndOffset(Type type, string fieldName, out int parentOffset)
		{
			parentOffset = (int) Marshal.OffsetOf(type, fieldName);

			return type.GetField(fieldName)!.FieldType;
		}

		private static int FindFieldOffset(Type type, string fieldName)
		{
			var parentOffset = 0;

			var fieldInfo = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
			if (fieldInfo is null)
			{
				foreach (var childFieldInfo in type.GetPublicInstanceFields())
				{
					var fieldType = childFieldInfo.FieldType;
					if (fieldType.GetField(fieldName) is null) continue;

					parentOffset = (int) Marshal.OffsetOf(type, childFieldInfo.Name);
					type = fieldType;

					break;
				}
			}

			return parentOffset + (int)Marshal.OffsetOf(type, fieldName);
		}

		internal static string BuildFieldName([NotNull] in Type rootType, [NotNull] Type type, [NotNull] string fieldName)
		{
			rootType.ThrowIfNull(nameof(rootType));
			type.ThrowIfNull(nameof(type));
			fieldName.ThrowIfNull(nameof(fieldName));
			type.GetField(fieldName).ThrowIfNull(fieldName, $"Type [{type}] has no field named [{fieldName}].");

			List<string> list = new();

			if (!BuildFieldNameInternal(list, rootType, type, fieldName))
				throw new ArgumentException($"Field name {fieldName} could not be found.");

			return list.AsEnumerable().Reverse().ToArray().Join(".");
		}

		private static bool BuildFieldNameInternal(in List<string> list, in Type parentType, Type type, string fieldName)
		{
			var fields = parentType.GetPublicInstanceStructFields();
			foreach (var field in fields)
			{
				if (field.FieldType == type)
				{
					list.Add(fieldName);
					list.Add(field.Name);
					return true;
				}

				if (!field.FieldType.IsDefined<HasOffsetMembersAttribute>())
					continue;

				if (BuildFieldNameInternal(list, field.FieldType, type, fieldName))
				{
					list.Add(field.Name);
					return true;
				}
			}

			return false;
		}
	}
}

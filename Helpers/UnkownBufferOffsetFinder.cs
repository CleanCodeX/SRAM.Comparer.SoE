using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		internal class FindOffsetResult
		{
			public Type? ParentType;
			public string? Path;

			public override string? ToString() => ParentType is null ? Path : $"[{ParentType.Name}] {Path}";
		}

		public static int GetSaveSlotBufferOffset(string fieldName, out FindOffsetResult result)
		{
			result = new();
			int offset;

			if (fieldName.Contains(StructDelimiter))
				offset = (int)fieldName.ParseEnum<SaveSlotUnknownOffset>();
			else
				offset = InternalGetBufferOffset(typeof(SaveSlotDataSoE), fieldName, result);

			offset.ThrowIfDefault(fieldName, OffsetNotFoundTemplate.InsertArgs(fieldName));

			return SramSizes.Checksum + offset;
		}

		public static int GetSramBufferOffset(string fieldName, out FindOffsetResult result)
		{
			result = new();
			return fieldName.Contains(StructDelimiter)
				? (int) fieldName.ParseEnum<SaveSlotUnknownOffset>()
					.GetOrThrowIfDefault(fieldName, OffsetNotFoundTemplate.InsertArgs(fieldName))
				: InternalGetBufferOffset(typeof(SramSoE), fieldName, result)
					.GetOrThrowIfDefault(fieldName, NamedOffsetNotDefinedTemplate.InsertArgs(fieldName));
		}

		private static int InternalGetBufferOffset(Type type, string fieldNameOrPath, FindOffsetResult result)
		{
			var index = fieldNameOrPath.IndexOf('.'); // check for path info
			if (index == -1)
			{
				var offset = FindFieldOffset(type, fieldNameOrPath, result);
				if (offset > -1)
					return offset;

				throw new ArgumentException($"Type [{type.Name}] does not contain field [{fieldNameOrPath}]");
			}

			var fieldName = fieldNameOrPath[..index];
			var parentFieldType = GetBaseFieldTypeAndOffset(type, fieldName, out var parentOffset);

			result.Path += $"{fieldName}.";
			fieldNameOrPath = fieldNameOrPath[(index + 1)..];
			
			return parentOffset + InternalGetBufferOffset(parentFieldType, fieldNameOrPath, result);
		}

		private static Type GetBaseFieldTypeAndOffset(Type type, string fieldName, out int parentOffset)
		{
			parentOffset = (int) Marshal.OffsetOf(type, fieldName);

			return type.GetField(fieldName)!.FieldType;
		}

		private static int FindFieldOffset(Type type, string fieldName, FindOffsetResult result)
		{
			result.ParentType = type;

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
					result.ParentType = fieldType;
					Debug.Assert(result.Path is null);
					result.Path += $"{childFieldInfo.Name}.{fieldName}";

					break;
				}
			}
			else
				result.Path += fieldName;

			return parentOffset + (int) Marshal.OffsetOf(type, fieldName);
		}

		internal static string BuildFieldName([NotNull] in Type rootType, [NotNull] Type containingType, [NotNull] string fieldName)
		{
			rootType.ThrowIfNull(nameof(rootType));
			containingType.ThrowIfNull(nameof(containingType));
			fieldName.ThrowIfNull(nameof(fieldName));
			containingType.GetField(fieldName).ThrowIfNull(fieldName, $"Type [{containingType}] has no field named [{fieldName}].");

			List<string> list = new();

			if (!BuildFieldNameInternal(list, rootType, containingType, fieldName))
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

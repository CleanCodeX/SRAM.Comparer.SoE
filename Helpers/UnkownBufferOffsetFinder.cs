using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Common.Shared.Min.Extensions;
using RosettaStone.Sram.SoE.Models;
using RosettaStone.Sram.SoE.Models.Structs;

// ReSharper disable InconsistentNaming

namespace SramComparer.SoE.Helpers
{
	/// <summary>List of buffers in sub-structures</summary>
	internal static class UnkownBufferOffsetFinder
	{
		internal const string StructDelimiter = "__";

		public static int GetSaveSlotBufferOffset(string bufferName) => bufferName.Contains(StructDelimiter)
				? (int)bufferName.ParseEnum<SaveSlotUnknownOffset>()
				: (int)Marshal.OffsetOf<SaveSlotDataSoE>(bufferName);

		public static int GetSramBufferOffset(string bufferName) => SramOffsets.Unknown1;

		private const int Chunk3Size = SramSizes.SaveSlot.Chunk03; // 6 bytes
		private const int DogStatusBuffs1 = SramOffsets.SaveSlot.Chunk03; // Offset 112
		private const int DogStatusBuffs2 = DogStatusBuffs1 + Chunk3Size;
		private const int DogStatusBuffs3 = DogStatusBuffs1 + Chunk3Size * 2;
		private const int DogStatusBuffs4 = DogStatusBuffs1 + Chunk3Size * 3;

		[SuppressMessage("ReSharper", "UnusedMember.Local")]
		[SuppressMessage("ReSharper", "UnusedMember.Global")]
		private enum SaveSlotUnknownOffset
		{
			DogStatusBuffs1_Id = DogStatusBuffs1, // Offset 112
			DogStatusBuffs1_Timer = DogStatusBuffs1 + 2, 
			DogStatusBuffs1_Boost = DogStatusBuffs1 + 4, 

			DogStatusBuffs2_Id = DogStatusBuffs2, // Offset 118
			DogStatusBuffs2_Timer = DogStatusBuffs2 + 2, 
			DogStatusBuffs2_Boost = DogStatusBuffs2 + 4,

			DogStatusBuffs3_Id = DogStatusBuffs3,  // Offset 126
			DogStatusBuffs3_Timer = DogStatusBuffs3 + 2, 
			DogStatusBuffs3_Boost = DogStatusBuffs3 + 4,

			DogStatusBuffs4_Id = DogStatusBuffs4, // Offset 134
			DogStatusBuffs4_Timer = DogStatusBuffs4 + 2,
			DogStatusBuffs4_Boost = DogStatusBuffs4 + 4, 

			Unknown7_DogBuff = SramOffsets.SaveSlot.Chunk10, 
			Unknown7_DogBuff__BuffFlags = Unknown7_DogBuff, 
			Unknown7_DogBuff__Unknown1 = Unknown7_DogBuff + 2, 
			Unknown7_DogBuff__Unknown2 = Unknown7_DogBuff + 4, 
			Unknown7_DogBuff__Unknown3 = Unknown7_DogBuff + 28, 
			Unknown7_DogBuff__Unknown7 = Unknown7_DogBuff + 30, 

			Unknown15 = SramOffsets.SaveSlot.Unknown15, // Offset 609
			Unknown15_Offset0To16 = Unknown15, 
			Unknown15_Offset17 = Unknown15 + 17, 
			Unknown15_Offset18 = Unknown15 + 18, 
			Unknown15_Offset19 = Unknown15 + 19, 
			Unknown15_Offset20 = Unknown15 + 20, 
			Unknown15_Offset21 = Unknown15 + 21, 
			Unknown15_Offset22 = Unknown15 + 22, 
			Unknown15_Offset23 = Unknown15 + 23, 
			Unknown15_Offset24 = Unknown15 + 24, 
			Unknown15_Offset25To117 = Unknown15 + 25,

			Unknown16C = SramOffsets.SaveSlot.Unknown16C, // Offset 643
			Unknown16C__Offset1To5 = Unknown16C + 1, // Offset 644
		}
	}
}

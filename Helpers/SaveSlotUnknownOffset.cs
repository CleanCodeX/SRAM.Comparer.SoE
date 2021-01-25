using System.Diagnostics.CodeAnalysis;
using SRAM.SoE.Models;
using SRAM.SoE.Models.Structs;

namespace SRAM.Comparison.SoE.Helpers
{
	/// <summary>
	/// Base offsets for <see cref="SaveSlotUnknownOffset" /> members
	/// </summary>
	internal enum SaveSlotBaseOffset
	{

		Chunk3Size = SramSizes.SaveSlot.Chunk03, // 6 bytes
		Chunk10Size = SramSizes.SaveSlot.Chunk10, // 6 bytes

		BoyStatusBuffs1 = SramOffsets.SaveSlot.Chunk03, // Offset 112
		BoyStatusBuffs2 = BoyStatusBuffs1 + Chunk3Size,
		BoyStatusBuffs3 = BoyStatusBuffs1 + Chunk3Size * 2,
		BoyStatusBuffs4 = BoyStatusBuffs1 + Chunk3Size * 3,

		DogStatusBuffs1 = SramOffsets.SaveSlot.Chunk10, // Offset 112
		DogStatusBuffs2 = DogStatusBuffs1 + Chunk10Size,
		DogStatusBuffs3 = DogStatusBuffs1 + Chunk10Size * 2,
		DogStatusBuffs4 = DogStatusBuffs1 + Chunk10Size * 3,

		Unknown15 = SramOffsets.SaveSlot.Unknown15, // Offset 609
		Unknown16C = SramOffsets.SaveSlot.Unknown16C, // Offset 643
	}

	/// <summary>
	/// Offsets for possibly non-unique field names in <see cref="SaveSlotSoE" /> structure.
	/// </summary>
	[SuppressMessage("ReSharper", "UnusedMember.Local")]
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	internal enum SaveSlotUnknownOffset
	{
		BoyStatusBuffs1__Id = SaveSlotBaseOffset.BoyStatusBuffs1, // Offset 112
		BoyStatusBuffs1__Timer = SaveSlotBaseOffset.BoyStatusBuffs1 + 2,
		BoyStatusBuffs1__Boost = SaveSlotBaseOffset.BoyStatusBuffs1 + 4,

		BoyStatusBuffs2__Id = SaveSlotBaseOffset.BoyStatusBuffs2, // Offset 118
		BoyStatusBuffs2__Timer = SaveSlotBaseOffset.BoyStatusBuffs2 + 2,
		BoyStatusBuffs2__Boost = SaveSlotBaseOffset.BoyStatusBuffs2 + 4,

		BoyStatusBuffs3__Id = SaveSlotBaseOffset.BoyStatusBuffs3, // Offset 126
		BoyStatusBuffs3__Timer = SaveSlotBaseOffset.BoyStatusBuffs3 + 2,
		BoyStatusBuffs3__Boost = SaveSlotBaseOffset.BoyStatusBuffs3 + 4,

		BoyStatusBuffs4__Id = SaveSlotBaseOffset.BoyStatusBuffs4, // Offset 134
		BoyStatusBuffs4__Timer = SaveSlotBaseOffset.BoyStatusBuffs4 + 2,
		BoyStatusBuffs4__Boost = SaveSlotBaseOffset.BoyStatusBuffs4 + 4,

		DogStatusBuffs1__Id = SaveSlotBaseOffset.DogStatusBuffs1, // Offset 112
		DogStatusBuffs1__Timer = SaveSlotBaseOffset.DogStatusBuffs1 + 2,
		DogStatusBuffs1__Boost = SaveSlotBaseOffset.DogStatusBuffs1 + 4,

		DogStatusBuffs2__Id = SaveSlotBaseOffset.DogStatusBuffs2, // Offset 118
		DogStatusBuffs2__Timer = SaveSlotBaseOffset.DogStatusBuffs2 + 2,
		DogStatusBuffs2__Boost = SaveSlotBaseOffset.DogStatusBuffs2 + 4,

		DogStatusBuffs3__Id = SaveSlotBaseOffset.DogStatusBuffs3, // Offset 126
		DogStatusBuffs3__Timer = SaveSlotBaseOffset.DogStatusBuffs3 + 2,
		DogStatusBuffs3__Boost = SaveSlotBaseOffset.DogStatusBuffs3 + 4,

		DogStatusBuffs4__Id = SaveSlotBaseOffset.DogStatusBuffs4, // Offset 134
		DogStatusBuffs4__Timer = SaveSlotBaseOffset.DogStatusBuffs4 + 2,
		DogStatusBuffs4__Boost = SaveSlotBaseOffset.DogStatusBuffs4 + 4,

		Unknown15__Offset0To23 = SaveSlotBaseOffset.Unknown15,

		Unknown16C__Offset1To5 = SaveSlotBaseOffset.Unknown16C + 1, // Offset 644
	}
}

	
using System.Diagnostics.CodeAnalysis;
using SRAM.SoE.Models;
using SRAM.SoE.Models.Structs;
using WRAM.Snes9x.SoE.Models.Structs.Chunks;

namespace SRAM.Comparison.SoE.Helpers
{
	/// <summary>
	/// Base offsets for <see cref="SaveSlotUnknownOffset" /> members
	/// </summary>
	internal enum SaveSlotBaseOffset
	{

		Chunk3Size = Chunk03.Size, // 6 bytes
		Chunk10Size = Chunk10.Size, // 6 bytes

		BoyStatusBuffs0 = SramOffsets.SaveSlot.Chunk03, // Offset 112
		BoyStatusBuffs1 = BoyStatusBuffs0 + Chunk3Size,
		BoyStatusBuffs2 = BoyStatusBuffs0 + Chunk3Size * 2,
		BoyStatusBuffs3 = BoyStatusBuffs0 + Chunk3Size * 3,

		DogStatusBuffs0 = SramOffsets.SaveSlot.Chunk10, // Offset 112
		DogStatusBuffs1 = DogStatusBuffs0 + Chunk10Size,
		DogStatusBuffs2 = DogStatusBuffs0 + Chunk10Size * 2,
		DogStatusBuffs3 = DogStatusBuffs0 + Chunk10Size * 3,

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
		BoyStatusBuffs0__Id = SaveSlotBaseOffset.BoyStatusBuffs0, // Offset 112
		BoyStatusBuffs0__Timer = SaveSlotBaseOffset.BoyStatusBuffs0 + 2,
		BoyStatusBuffs0__Boost = SaveSlotBaseOffset.BoyStatusBuffs0 + 4,

		BoyStatusBuffs1__Id = SaveSlotBaseOffset.BoyStatusBuffs1, // Offset 118
		BoyStatusBuffs1__Timer = SaveSlotBaseOffset.BoyStatusBuffs1 + 2,
		BoyStatusBuffs1__Boost = SaveSlotBaseOffset.BoyStatusBuffs1 + 4,

		BoyStatusBuffs2__Id = SaveSlotBaseOffset.BoyStatusBuffs2, // Offset 126
		BoyStatusBuffs2__Timer = SaveSlotBaseOffset.BoyStatusBuffs2 + 2,
		BoyStatusBuffs2__Boost = SaveSlotBaseOffset.BoyStatusBuffs2 + 4,

		BoyStatusBuffs3__Id = SaveSlotBaseOffset.BoyStatusBuffs3, // Offset 134
		BoyStatusBuffs3__Timer = SaveSlotBaseOffset.BoyStatusBuffs3 + 2,
		BoyStatusBuffs3__Boost = SaveSlotBaseOffset.BoyStatusBuffs3 + 4,

		DogStatusBuffs0__Id = SaveSlotBaseOffset.DogStatusBuffs0, // Offset 112
		DogStatusBuffs0__Timer = SaveSlotBaseOffset.DogStatusBuffs0 + 2,
		DogStatusBuffs0__Boost = SaveSlotBaseOffset.DogStatusBuffs0 + 4,

		DogStatusBuffs1__Id = SaveSlotBaseOffset.DogStatusBuffs1, // Offset 118
		DogStatusBuffs1__Timer = SaveSlotBaseOffset.DogStatusBuffs1 + 2,
		DogStatusBuffs1__Boost = SaveSlotBaseOffset.DogStatusBuffs1 + 4,

		DogStatusBuffs2__Id = SaveSlotBaseOffset.DogStatusBuffs2, // Offset 126
		DogStatusBuffs2__Timer = SaveSlotBaseOffset.DogStatusBuffs2 + 2,
		DogStatusBuffs2__Boost = SaveSlotBaseOffset.DogStatusBuffs2 + 4,

		DogStatusBuffs3__Id = SaveSlotBaseOffset.DogStatusBuffs3, // Offset 134
		DogStatusBuffs3__Timer = SaveSlotBaseOffset.DogStatusBuffs3 + 2,
		DogStatusBuffs3__Boost = SaveSlotBaseOffset.DogStatusBuffs3 + 4,
	}
}

	
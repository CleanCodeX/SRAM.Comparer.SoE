//#define EXPLICIT

using System.Runtime.InteropServices;
using SramCommons.SoE.Models.Enums;
using SramCommons.Models.Structs;

// ReSharper disable InconsistentNaming

namespace SramCommons.SoE.Models.Structs
{
#if EXPLICIT
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = Sizes.Game.All)]
#else
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
#endif
    public struct SramGame
    {
        // Unknown game file bytes: 462 of 817
#if EXPLICIT
        [FieldOffset(Offsets.Game.Checksum)]
#endif
        public ushort Checksum; // 2 bytes

        // Unknown 1
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown1)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.Game.Unknown1)]
        public byte[] Unknown1; // 36 bytes

        // Boy & Dog character
#if EXPLICIT
        [FieldOffset(Offsets.Game.BoyName)]
#endif
        public CharacterName BoyName; // 38 (34 bytes) Null terminated

        // Unknown 2
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown2)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.Game.Unknown2)]
        public byte[] Unknown2; // 72 (2 bytes)

#if EXPLICIT
        [FieldOffset(Offsets.Game.DogName)]
#endif
        public CharacterName DogName; // 74 (34 bytes) Null terminated

        // Unknown 3
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown3)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.Game.Unknown3)]
        public byte[] Unknown3; // 108 (2 bytes)

#if EXPLICIT
        [FieldOffset(Offsets.Game.BoyCurrentHp)]
#endif
        public ushort BoyCurrentHp; // 110 (2 bytes)
        // Unknown 4
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown4)]
#endif
        public CharacterBuff Unknown4_BoyBuff;

#if EXPLICIT
        [FieldOffset(Offsets.Game.BoyMaxHp)]
#endif
        public ushort BoyMaxHp; // 142 (2 bytes)

        // Unknown 5
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown5)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.Game.Unknown5)]
        public byte[] Unknown5; // 144 (10 bytes)

#if EXPLICIT
        [FieldOffset(Offsets.Game.BoyExperience)]
#endif
        public ThreeByteUint BoyExperience; // 154 (3 Byte)
#if EXPLICIT
        [FieldOffset(Offsets.Game.BoyLevel)]
#endif
        public ushort BoyLevel; // 157 (2 bytes)

        // Unknown 6
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown6)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.Game.Unknown6)]
        public byte[] Unknown6; // 159 (16 bytes)

#if EXPLICIT
        [FieldOffset(Offsets.Game.DogCurrentHp)]
#endif
        public ushort DogCurrentHp; // 175 (2 bytes)

        // Unknown 7
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown7)]
#endif
        public CharacterBuff Unknown7_DogBuff; 

#if EXPLICIT
        [FieldOffset(Offsets.Game.DogMaxHp)]
#endif
        public ushort DogMaxHp; // 207 (2 bytes)

        // Unknown 8
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown8)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.Game.Unknown8)]
        public byte[] Unknown8; // 209 (10 bytes)

#if EXPLICIT
        [FieldOffset(Offsets.Game.DogExperience)] 
#endif
        public ThreeByteUint DogExperience; // 219 (3 Byte)
#if EXPLICIT
        [FieldOffset(Offsets.Game.DogLevel)]
#endif
        public ushort DogLevel; // 222 (2 bytes)

        // Unknown 9
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown9)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.Game.Unknown9)]
        public byte[] Unknown9; // 224 (28 bytes)

        // Money
#if EXPLICIT
        [FieldOffset(Offsets.Game.Money)]
#endif
        public Moneys Moneys; // 252 (12 bytes)

        // Unknown 10
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown10)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.Game.Unknown10)]
        public byte[] Unknown10; // 264 (13 bytes)

        // Weapon Levels
#if EXPLICIT
        [FieldOffset(Offsets.Game.WeaponLevels)]
#endif
        public WeaponLevels WeaponLevels; // 277 (26 bytes)

        // Unknown 11
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown11)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.Game.Unknown11)]
        public byte[] Unknown11; // 303 (14 bytes)

#if EXPLICIT
        [FieldOffset(Offsets.Game.DogAttackLevel)]
#endif
        public WeaponLevel DogAttackLevel; // 317 (2 bytes)

        // Unknown 12 A
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown12A)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.Game.Unknown12A)]
        public byte[] Unknown12A; // 319 (16 bytes)

        // Unknown 12 B
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown12B)]
#endif
        public ushort Unknown12B;// 335 (2 bytes) Maybe frame-counter, changes at every in-game save

        // Unknown 12 C
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown12C)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.Game.Unknown12C)]
        public byte[] Unknown12C; // 337 (4 bytes)

        // Alchemy Levels
#if EXPLICIT
        [FieldOffset(Offsets.Game.AlchemyMinorLevels)]
#endif
        public AlchemyLevels AlchemyMinorLevels; // 341 (70 bytes)

#if EXPLICIT
        [FieldOffset(Offsets.Game.AlchemyMajorLevels)] 
#endif
        public AlchemyLevels AlchemyMajorLevels; // 411 (70 bytes)

        // Unknown 13
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown13)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.Game.Unknown13)]
        public byte[] Unknown13; // 481 (22 bytes)

        // Weapons
#if EXPLICIT
        [FieldOffset(Offsets.Game.Alchemies)]
#endif
        public Alchemies Alchemies; // 503 (5 bytes)

        // Unknown 14
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown14_AntiquaFlags)]
#endif
        public Unknown14_AntiquaFlags Unknown14_AntiquaFlags; // 508 (4 bytes) 

        // Charms
#if EXPLICIT
        [FieldOffset(Offsets.Game.Charms)]
#endif
        public Charms Charms; // 512 (3 bytes)

        // Unknown 15
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown15)]
#endif
        public Unknown15 Unknown15; // 515 (118 bytes)

        // Weapons
#if EXPLICIT
        [FieldOffset(Offsets.Game.Weapons)]
#endif
        public Weapons Weapons; // 633 (2 bytes)

        // Unknown 16

#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown16A)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.Game.Unknown16A)]
        public byte[] Unknown16A; // 635 (4 bytes) 

#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown16B_GoticaFlags)]
#endif
        public Unknown16_GotikaFlags Unknown16B_GoticaFlags; // 639 (4 bytes)

#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown16A)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.Game.Unknown16C)]
        public byte[] Unknown16C; // 643 (6 bytes) 

        // Ingredients
#if EXPLICIT
        [FieldOffset(Offsets.Game.Ingredients)]
#endif
        public Ingredients Ingredients; // 649 (22 bytes)

        // Items
#if EXPLICIT
        [FieldOffset(Offsets.Game.Items)]
#endif
        public Items Items; // 671 (8 bytes)

#if EXPLICIT
        [FieldOffset(Offsets.Game.Armors)]
#endif
        public Armors Armors; // 679 (40 bytes)

#if EXPLICIT
        [FieldOffset(Offsets.Game.BazookaAmmunitions)]
#endif
        public BazookaAmmunitions BazookaAmmunitions; // 719 (3 bytes)

        // Unknown 17
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown17)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.Game.Unknown17)]
        public byte[] Unknown17; // 722 (67 bytes)

        // Trade Goods
#if EXPLICIT
        [FieldOffset(Offsets.Game.TradeGoods)]
#endif
        public TradeGoods TradeGoods; // 789 (26 bytes)

        // Unknown 18
#if EXPLICIT
        [FieldOffset(Offsets.Game.Unknown18)]
#endif
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Sizes.Game.Unknown18)]
        public byte[] Unknown18; // 816 (2 bytes)
    }
}
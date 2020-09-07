// ReSharper disable InconsistentNaming

using System.Linq;
using SramCommons.Extensions;

namespace SramCommons.SoE.Models
{
    public class Offsets
    {
        /// base offset of the game data in the SRAM
        public const int FirstGame = 2;
        public const int SramChecksum = 0;

        public class Game
        {
            public static string? GetNameFromOffset(int offset)
            {
                var constants = typeof(Game).GetPublicConstants<int>().OrderBy(e => e.Value);

                return (from kvp in constants 
                    where offset >= kvp.Value
                        select kvp.Key).LastOrDefault();
            }

            public const int Checksum = 0; // (2 bytes)

            public const int Unknown1 = 2; // (36 bytes)

            ///  of the boy's name
            public const int BoyName = 38; // (34 bytes)

            public const int Unknown2 = 72; // (2 bytes)

            ///  of the dog's name
            public const int DogName = 74; // (34 bytes)

            public const int Unknown3 = 108; // (2 bytes)

            ///  of the boy's current HP
            public const int BoyCurrentHp = 110; // (2 bytes)

            public const int Unknown4 = 112; // (30 bytes)

            public const int Unknown4_BoyBuff__BuffFlags = Unknown4; // Offset 112
            public const int Unknown4_BoyBuff__Unknown1 = Unknown4_BoyBuff__BuffFlags + 2; // Offset 114
            public const int Unknown4_BoyBuff__Unknown2 = Unknown4_BoyBuff__Unknown1 + 2; // Offset 116
            public const int Unknown4_BoyBuff__Unknown3 = Unknown4_BoyBuff__Unknown2 + 22; // Offset 138
            public const int Unknown4_BoyBuff__Unknown4 = Unknown4_BoyBuff__Unknown3 + 2; // Offset 140

            ///  of the boy's max HP
            public const int BoyMaxHp = 142; // (2 bytes)

            public const int Unknown5 = 144; // (10 bytes)

            ///  of the boy's experience
            public const int BoyExperience = 154; // (3 bytes)

            ///  of the boy's level
            public const int BoyLevel = 157; // (2 bytes)

            public const int Unknown6 = 159; // (16 bytes)

            ///  of the dog's current HP
            public const int DogCurrentHp = 175; // (2 bytes)

            public const int Unknown7 = 177; // (30 bytes)

            public const int Unknown7_BoyBuff__BuffFlags = Unknown7; // Offset 177
            public const int Unknown7_BoyBuff__Unknown1 = Unknown7_BoyBuff__BuffFlags + 2; // Offset 179
            public const int Unknown7_BoyBuff__Unknown2 = Unknown7_BoyBuff__Unknown1 + 2; // Offset 181
            public const int Unknown7_BoyBuff__Unknown3 = Unknown7_BoyBuff__Unknown2 + 22; // Offset 203
            public const int Unknown7_BoyBuff__Unknown7 = Unknown7_BoyBuff__Unknown3 + 2; // Offset 205

            ///  of the dog's max HP
            public const int DogMaxHp = 207; // (2 bytes)

            public const int Unknown8 = 209; // (10 bytes)

            ///  of the dog's experience
            public const int DogExperience = 219; // (3 bytes)

            ///  of the dog's level
            public const int DogLevel = 222; // (2 bytes)

            public const int Unknown9 = 224; // (28 bytes)

            /// base money offset
            public const int Money = 252; // (12 bytes)

            public const int Unknown10 = 264; // (13 bytes)

            /// base weapon Levels offset
            public const int WeaponLevels = 277; // (26 bytes)

            public const int Unknown11 = 303; // (14 bytes)

            ///  of the dog's attack level
            public const int DogAttackLevel = 317; // (2 bytes)

            public const int Unknown12A = 319; // (16 bytes)
            public const int Unknown12B = 335; // (2 bytes)
            public const int Unknown12C = 337; // (4 bytes)

            /// base minor alchemy Levels offset
            public const int AlchemyMinorLevels = 341; // (70 bytes)

            public const int AlchemyMajorLevels = 411; // (70 bytes)

            public const int Unknown13 = 481; // (23 bytes)

            ///  of boy's alchemy offset
            public const int Alchemies = 503; // (5 bytes)

            public const int Unknown14_AntiquaFlags = 508; // (4 bytes) 

            ///  of charms offset
            public const int Charms = 512; // (3 bytes)

            public const int Unknown15 = 515; // (118 bytes)

            ///  of boy's weapon offset
            public const int Weapons = 633; // (2 bytes)

            public const int Unknown16A = 635; // (4 bytes)
            public const int Unknown16B_GoticaFlags = 639; // (4 bytes)
            public const int Unknown16C = 643; // (6 bytes)

            /// base alchemy ingredient offset
            public const int Ingredients = 649; // (22 bytes)

            /// base item offset
            public const int Items = 671; // (8 bytes)

            /// base item offset
            public const int Armors = 679; // (40 bytes)

            /// base item offset
            public const int BazookaAmmunitions = 719; // (3 bytes)

            public const int Unknown17 = 722; // (67 bytes)

            /// base tradegood offset
            public const int TradeGoods = 789; // (26 bytes)

            public const int Unknown18 = 815; // (2 bytes)
        }

        public const int SramUnknown1 = 3_270;
    }
}
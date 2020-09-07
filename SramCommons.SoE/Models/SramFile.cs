using System;
using System.Diagnostics;
using SramCommons.SoE.Helpers;
using SramCommons.SoE.Models.Enums;
using SramCommons.SoE.Models.Structs;
using SramCommons.Exceptions;
using SramCommons.Extensions;
using SramCommons.Models;

namespace SramCommons.SoE.Models
{
    /// model of a Secret of Evermore SRAM file
    public class SramFile : SramFileBase<Sram, SramGame, FileRegion, GameId>
    {
		private readonly bool[] _validGames = new bool[4];
		
		public SramFile(string filename, FileRegion region) :base(filename, region, Sizes.Sram, Sizes.Game.All, Offsets.FirstGame)
        {
            Debug.Assert(Sizes.Game.All == Sizes.Game.AllKnown + Sizes.Game.AllUnknown);

            var anyGameIsValid = false;
			for (var game = 0; game < 4; ++game)
			{
				var gameId = (GameId)game + 1;
				var fileGameChecksum = GetChecksum(gameId);

                var calculatedChecksum = ChecksumHelper.CalcChecksum(SramBuffer, gameId, region);
				if (fileGameChecksum != calculatedChecksum) continue;

                anyGameIsValid = true;
                _validGames[game] = true;
            }

            if (!anyGameIsValid)
                throw new InvalidSramFileException(FileError.NoValidGames);
        }

        public override SramGame GetGame(GameId gameId)
        {
            CurrentGameId = gameId;
            CurrentGame = Sram.Game[gameId.ToIndex()];

            #region For debugging

#pragma warning disable IDE0059 // Unnötige Zuweisung eines Werts.
            // ReSharper disable UnusedVariable
            var boyLevel = CurrentGame.BoyLevel;
            var boyExperience = CurrentGame.BoyExperience;
            var boyCurrentHp = CurrentGame.BoyCurrentHp;
            var boyMaxHp = CurrentGame.BoyMaxHp;
            var boyName = CurrentGame.BoyName.StringValue;

            var dogLevel = CurrentGame.DogLevel;
            var dogExperience = CurrentGame.DogExperience;
            var dogCurrentHp = CurrentGame.DogCurrentHp;
            var dogMaxHp = CurrentGame.DogMaxHp;
            var dogName = CurrentGame.DogName.StringValue;

            var alchemies = CurrentGame.Alchemies.ToString();
            var alchemyMajorLevels = CurrentGame.AlchemyMajorLevels.ToString();
            var alchemyMinorLevels = CurrentGame.AlchemyMinorLevels.ToString();
            var charms = CurrentGame.Charms.ToString();
            var weapons = CurrentGame.Weapons.ToString();
            var weaponLevels = CurrentGame.WeaponLevels.ToString();
            var dogAttackLevel = CurrentGame.DogAttackLevel.ToString();
            var money = CurrentGame.Moneys.ToString();
            var items = CurrentGame.Items.ToString();
            var armors = CurrentGame.Armors.ToString();
            var ammunitions = CurrentGame.BazookaAmmunitions.ToString();
            var tradeGoods = CurrentGame.TradeGoods.ToString();

            var unknown1 = CurrentGame.Unknown1.FormatAsString();
            var unknown2 = CurrentGame.Unknown2.FormatAsString();
            var unknown3 = CurrentGame.Unknown3.FormatAsString();
            var unknown4 = CurrentGame.Unknown4_BoyBuff.FormatAsString();
            var unknown5 = CurrentGame.Unknown5.FormatAsString();
            var unknown6 = CurrentGame.Unknown6.FormatAsString();
            var unknown7 = CurrentGame.Unknown7_DogBuff.FormatAsString();
            var unknown8 = CurrentGame.Unknown8.FormatAsString();
            var unknown9 = CurrentGame.Unknown9.FormatAsString();
            var unknown10 = CurrentGame.Unknown10.FormatAsString();
            var unknown11 = CurrentGame.Unknown11.FormatAsString();
            var unknown12A = CurrentGame.Unknown12A.FormatAsString();
            var unknown12B = CurrentGame.Unknown12B.ToString();
            var unknown12C = CurrentGame.Unknown12C.FormatAsString();
            var unknown13 = CurrentGame.Unknown13.FormatAsString();
            var unknown14 = CurrentGame.Unknown14_AntiquaFlags.ToString();
            var unknown15 = CurrentGame.Unknown15.FormatAsString();
            var unknown16A = CurrentGame.Unknown16A.FormatAsString();
            var unknown16B = CurrentGame.Unknown16B_GoticaFlags.ToString();
            var unknown16C = CurrentGame.Unknown16C.FormatAsString();
            var unknown17 = CurrentGame.Unknown17.FormatAsString();
            var unknown18 = CurrentGame.Unknown18.FormatAsString();
            // ReSharper restore UnusedVariable
#pragma warning restore IDE0059 // Unnötige Zuweisung eines Werts.

            #endregion For debugging

            return CurrentGame;
        }

        public override bool IsValid(GameId gameId) => IsValid() && _validGames[gameId.ToIndex()];

        public override bool Save(string filename)
        {
            for (var game = 1; game <= 4; ++game)
            {
                var gameId = (GameId)game;
                if (IsValid(gameId))
                    SetChecksum(gameId, ChecksumHelper.CalcChecksum(SramBuffer, gameId, GameRegion));
            }

            return base.Save(filename);
        }

        public ushort GetChecksum(GameId game)
		{
			var offset = Offsets.FirstGame + game.ToIndex() * GameSize + Offsets.Game.Checksum;
			return BitConverter.ToUInt16(SramBuffer, offset);
		}

		public void SetChecksum(GameId game, ushort checksum)
		{
			var offset = Offsets.FirstGame + game.ToIndex() * GameSize + Offsets.Game.Checksum;
			var bytes = BitConverter.GetBytes(checksum);
			Array.Reverse(bytes);
			checksum = BitConverter.ToUInt16(bytes);

			CurrentGame.Checksum = checksum;

			bytes.CopyTo(SramBuffer, offset);
		}
    }
}



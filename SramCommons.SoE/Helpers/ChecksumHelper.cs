using System;
using System.Diagnostics;
using SramCommons.SoE.Models;
using SramCommons.SoE.Models.Enums;
using SramCommons.Extensions;

namespace SramCommons.SoE.Helpers
{
    public static class ChecksumHelper
    {
        [DebuggerStepThrough]
        public static ushort CalcChecksum(byte[] sramBuffer, GameId gameId, FileRegion region) =>
            CalcChecksum(sramBuffer, gameId, region == FileRegion.UnitedStates);

        public static ushort CalcChecksum(byte[] sramBuffer, GameId gameId, bool isUsVersion)
        {
            const int gameSize = Sizes.Game.All;
            const int sizeChecksum = 2;
            var checksum = isUsVersion ? ChecksumStartValues.US : ChecksumStartValues.Europe;
            var offset = Offsets.FirstGame + gameId.ToIndex() * gameSize;
            var temp = (byte)(checksum + sramBuffer[offset + sizeChecksum]);

            for (var i = 3; i < gameSize; ++i)
            {
                checksum &= 0xFF00;
                checksum |= temp;
                checksum <<= 1;

                if (checksum > 0xFFFF)
                    checksum -= 0xFFFF;

                temp = (byte)(checksum + sramBuffer[offset + i]);
            }

            return Convert.ToUInt16(checksum);
        }
    }
}
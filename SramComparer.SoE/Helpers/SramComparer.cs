using System;
using System.Text;
using Commons.Extensions;
using SramCommons.Extensions;
using SramCommons.SoE.Helpers;
using SramCommons.SoE.Models;
using SramCommons.SoE.Models.Structs;
using SramComparer.Helpers;
using SramComparer.SoE.Properties;
using static SramComparer.Helpers.ConsolePrinterBase;
using static SramComparer.SoE.Helpers.UnkownBufferOffsetFinder;
using Res = SramComparer.Properties.Resources;
// ReSharper disable RedundantArgumentDefaultValue

namespace SramComparer.SoE.Helpers
{
    public class SramComparer: SramComparerBase<SramFile, SramGame, GameId>
    {
        public override void CompareSram(SramFile currFile, SramFile compFile, IOptions options)
        {
            WriteNewSectionHeader();
            Console.ForegroundColor = ConsoleColor.Yellow;

            var optionGameId = (GameId)options.Game;
            var optionCompGameId = (GameId)options.ComparisonGame;
            var optionFlags = (ComparisonFlags)options.Flags;

            Console.WriteLine(optionGameId != default
                ? string.Format(Res.StatusGameWillBeComparedTemplate, options.Game)
                : Res.StatusAllGamesWillBeCompared);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            var offset = GetSramOffset(nameof(compFile.Sram.Unknown1), out var bufferName);
            var sramDiffBytes = CompareByteArray(Res.Sram + ":" + bufferName, offset, compFile.Sram.Unknown1, currFile.Sram.Unknown1);
            if (sramDiffBytes > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(Res.StatusTotalDiffBytesTemplate, sramDiffBytes);
                Console.ForegroundColor = ConsoleColor.White;
            }

            static int GetSramOffset(string bufferName, out string name)
            {
                name = bufferName;

                return GetSramBufferOffset(bufferName);
            }

            var allDiffBytes = sramDiffBytes;

            var checksums = new StringBuilder();
            checksums.AppendLine($"{Resources.GamesCurrentChecksumValues} (2 {Res.Bytes.ToLower()}): ({Resources.ChangesAtEveryInGameSave})");

            var timestamps = new StringBuilder();
            timestamps.AppendLine($"{Resources.GamesCurrentUnknown12BValues} (2+? {Res.Bytes.ToLower()}): ({Resources.ChangesAtEveryInGameSave})");

            if (optionGameId != default && optionCompGameId != default)
                allDiffBytes = CompareGames(optionGameId, optionCompGameId);
            else
                for (var i = 0; i < 4; i++)
                {
                    var gameId = (GameId) i + 1;

                    if (optionGameId != default && optionGameId != gameId) continue;

                    allDiffBytes = CompareGames(gameId);
                }

            if (options.Flags.HasFlag(ComparisonFlags.NonGameBuffer))
            {
                var nonGameUnknownDiffBytes = CompareByteArray(nameof(currFile.Sram.Unknown1), 0, currFile.Sram.Unknown1, compFile.Sram.Unknown1, false);

                if (nonGameUnknownDiffBytes > 0)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(" ".Repeat(2) + $@"[ {Res.SectionNonGameUnknowns} ].......................................");
                    Console.ResetColor();

                    nonGameUnknownDiffBytes = CompareByteArray(nameof(currFile.Sram.Unknown1), Offsets.SramUnknown1, currFile.Sram.Unknown1, compFile.Sram.Unknown1, true);

                    Console.WriteLine();
                    Console.ForegroundColor = nonGameUnknownDiffBytes > 0 ? ConsoleColor.Green : ConsoleColor.White;
                    Console.WriteLine(" ".Repeat(4) + Res.StatusNonGameUnknownsBytesTemplate, nonGameUnknownDiffBytes);
                    Console.ResetColor();

                    allDiffBytes += nonGameUnknownDiffBytes;
                }
            }

            Console.WriteLine();

            const int borderLength = 29;
            if (allDiffBytes > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("=".Repeat(borderLength));
                Console.WriteLine(@$"== {Res.StatusSramChangedBytesTemplate} ".PadRight(borderLength + 1, '='),  allDiffBytes);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("=".Repeat(borderLength));
                Console.WriteLine(@$"== {Res.StatusNoSramBytesChanged} =".PadRight(borderLength, '='));
            }

            Console.WriteLine("=".Repeat(borderLength));
            Console.ResetColor();

            if (optionFlags != default)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine();

                if (optionFlags.HasFlag(ComparisonFlags.GameChecksum))
                    Console.WriteLine(checksums.ToString());

                if (optionFlags.HasFlag(ComparisonFlags.Unknown12B))
                    Console.WriteLine(timestamps.ToString());
            }

            int CompareGames(GameId gameId, GameId compGameId = default)
            {
                if (compGameId == default)
                    compGameId = gameId;

                var currGame = currFile.GetGame(gameId);
                var currGameBytes = currFile.GetCurrentGameBytes();

                var compGame = compFile.GetGame(gameId);
                var compGameBytes = compFile.GetCurrentGameBytes();

                if (optionFlags.HasFlag(ComparisonFlags.AllGameChecksums) || compGame.Checksum != currGame.Checksum)
                    checksums.AppendLine(
                        $"{" ".Repeat(2)}{Res.Game} {gameId.ToInt()}: {currGame.Checksum.PadLeft()} = ({Res.ReversedByteOrder}) {currGame.Checksum.ReverseBytes().PadLeft()}");

                if (optionFlags.HasFlag(ComparisonFlags.AllUnknown12Bs) || compGame.Unknown12B != currGame.Unknown12B)
                    timestamps.AppendLine(
                        $"{" ".Repeat(2)}{Res.Game} {gameId.ToInt()}: {currGame.Unknown12B.PadLeft()} = ({Res.ReversedByteOrder}) {currGame.Unknown12B.ReverseBytes().PadLeft()}");

                if (compGameBytes.SequenceEqual(currGameBytes)) return allDiffBytes;

                checksums.AppendLine(
                    $"{" ".Repeat(2)}{Res.Game} {compGameId.ToInt()}: {compGame.Checksum.PadLeft()} = ({Res.ReversedByteOrder}) {compGame.Checksum.ReverseBytes().PadLeft()} ({Res.Comparison.ToLower()} {Res.Game.ToLower()})");

                timestamps.AppendLine(
                    $"{" ".Repeat(2)}{Res.Game} {compGameId.ToInt()}: {compGame.Unknown12B.PadLeft()} = ({Res.ReversedByteOrder}) {compGame.Unknown12B.ReverseBytes().PadLeft()} ({Res.Comparison.ToLower()} {Res.Game.ToLower()})");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($@"[ {Res.SectionGameHasChangedTemplate} ]---------------------------------------------", gameId.ToInt());
                Console.ResetColor();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(" ".Repeat(2) + $@"[ {Res.SectionGameUnknownsChangedTemplate} ].......................................", gameId.ToInt());
                Console.ResetColor();

                var gameDiffBytes = CompareGame(currGame, compGame, options);
                if (gameDiffBytes > 0)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" ".Repeat(4) + Res.StatusGameUnknownsChangedGameIdBytesTemplate, gameId.ToInt(), gameDiffBytes);
                    Console.ResetColor();
                }

                if (!optionFlags.HasFlag(ComparisonFlags.WholeGameBuffer))
                {
                    allDiffBytes += gameDiffBytes;
                    return allDiffBytes;
                }

                EnsureMinConsoleWidth(165);

                bufferName = $"{nameof(compFile.Sram.Game)} {gameId.ToInt()}";
                var bufferOffset = Offsets.FirstGame + gameId.ToInt() * Sizes.Game.All;
                var gameBufferDiffBytes = CompareByteArray(bufferName, bufferOffset, currGameBytes, compGameBytes, false);
                if (gameBufferDiffBytes > gameDiffBytes)
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($@"{" ".Repeat(2)}[ {Res.SectionGameChangedTemplate} ]...................................", gameId);
                    // ReSharper disable once RedundantArgumentDefaultValue

                    CompareByteArray(bufferName, bufferOffset, currGameBytes, compGameBytes, true, Offsets.Game.GetNameFromOffset);
                    Console.WriteLine();
                    Console.ForegroundColor = gameDiffBytes > 0 ? ConsoleColor.Green : ConsoleColor.White;
                    Console.WriteLine(" ".Repeat(4) + Res.StatusGameChangedBytesTemplate, gameId.ToInt(), gameBufferDiffBytes);
                    Console.ResetColor();
                }

                allDiffBytes += gameBufferDiffBytes;
                return allDiffBytes;
            }
        }

        protected override int CompareGame(SramGame currGame, SramGame compGame, IOptions options)
        {
            // ReSharper disable once InlineOutVariableDeclaration
            string name;
            // ReSharper disable once JoinDeclarationAndInitializer
            int offset;

            offset = GetGameOffset(nameof(currGame.Unknown1), out name);
            var diffBytes = CompareByteArray(name, offset, currGame.Unknown1, compGame.Unknown1);

            offset = GetGameOffset(nameof(currGame.Unknown2), out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown2, compGame.Unknown2);

            offset = GetGameOffset(nameof(currGame.Unknown3), out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown3, compGame.Unknown3);

            //Unknown 4
            offset = GetGameOffset($"{nameof(currGame.Unknown4_BoyBuff)}__{nameof(currGame.Unknown4_BoyBuff.BuffFlags)}", out name);
            diffBytes += CompareUShort(name, offset, currGame.Unknown4_BoyBuff.BuffFlags.ToUShort(), compGame.Unknown4_BoyBuff.BuffFlags.ToUShort());

            offset = GetGameOffset($"{nameof(currGame.Unknown4_BoyBuff)}__{nameof(currGame.Unknown4_BoyBuff.Unknown1)}", out name);
            diffBytes += CompareUShort(name, offset, currGame.Unknown4_BoyBuff.Unknown1, compGame.Unknown4_BoyBuff.Unknown1);

            offset = GetGameOffset($"{nameof(currGame.Unknown4_BoyBuff)}__{nameof(currGame.Unknown4_BoyBuff.Unknown2)}", out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown4_BoyBuff.Unknown2, compGame.Unknown4_BoyBuff.Unknown2);

            offset = GetGameOffset($"{nameof(currGame.Unknown4_BoyBuff)}__{nameof(currGame.Unknown4_BoyBuff.Unknown3)}", out name);
            diffBytes += CompareUShort(name, offset, currGame.Unknown4_BoyBuff.Unknown3, compGame.Unknown4_BoyBuff.Unknown3);

            offset = GetGameOffset($"{nameof(currGame.Unknown4_BoyBuff)}__{nameof(currGame.Unknown4_BoyBuff.Unknown4)}", out name);
            diffBytes += CompareUShort(name, offset, currGame.Unknown4_BoyBuff.Unknown4, compGame.Unknown4_BoyBuff.Unknown4);
            //Unknown 4

            offset = GetGameOffset(nameof(currGame.Unknown5), out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown5, compGame.Unknown5);

            offset = GetGameOffset(nameof(currGame.Unknown6), out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown6, compGame.Unknown6);

            //Unknown 7
            offset = GetGameOffset($"{nameof(currGame.Unknown7_DogBuff)}__{nameof(currGame.Unknown7_DogBuff.BuffFlags)}", out name);
            diffBytes += CompareUShort(name, offset, currGame.Unknown7_DogBuff.BuffFlags.ToUShort(), compGame.Unknown7_DogBuff.BuffFlags.ToUShort());

            offset = GetGameOffset($"{nameof(currGame.Unknown7_DogBuff)}__{nameof(currGame.Unknown7_DogBuff.Unknown1)}", out name);
            diffBytes += CompareUShort(name, offset, currGame.Unknown7_DogBuff.Unknown1, compGame.Unknown7_DogBuff.Unknown1);

            offset = GetGameOffset($"{nameof(currGame.Unknown7_DogBuff)}__{nameof(currGame.Unknown7_DogBuff.Unknown2)}", out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown7_DogBuff.Unknown2, compGame.Unknown7_DogBuff.Unknown2);

            offset = GetGameOffset($"{nameof(currGame.Unknown7_DogBuff)}__{nameof(currGame.Unknown7_DogBuff.Unknown3)}", out name);
            diffBytes += CompareUShort(name, offset, currGame.Unknown7_DogBuff.Unknown3, compGame.Unknown7_DogBuff.Unknown3);

            offset = GetGameOffset($"{nameof(currGame.Unknown7_DogBuff)}__{nameof(currGame.Unknown7_DogBuff.Unknown4)}", out name);
            diffBytes += CompareUShort(name, offset, currGame.Unknown7_DogBuff.Unknown4, compGame.Unknown7_DogBuff.Unknown4);
            //Unknown 7

            offset = GetGameOffset(nameof(currGame.Unknown8), out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown8, compGame.Unknown8);

            offset = GetGameOffset(nameof(currGame.Unknown9), out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown9, compGame.Unknown9);

            offset = GetGameOffset(nameof(currGame.Unknown10), out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown10, compGame.Unknown10);

            offset = GetGameOffset(nameof(currGame.Unknown11), out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown11, compGame.Unknown11);

            // 12 A - C
            offset = GetGameOffset(nameof(currGame.Unknown12A), out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown12A, compGame.Unknown12A);

            if (options.Flags.HasFlag(ComparisonFlags.Unknown12B))
            {
                offset = GetGameOffset(nameof(currGame.Unknown12B), out name);
                diffBytes += CompareUShort(name, offset, currGame.Unknown12B,
                    compGame.Unknown12B);
            }

            offset = GetGameOffset(nameof(currGame.Unknown12C), out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown12C, compGame.Unknown12C);
            // 

            offset = GetGameOffset(nameof(currGame.Unknown13), out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown13, compGame.Unknown13);

            offset = GetGameOffset(nameof(currGame.Unknown14_AntiquaFlags), out name);
            diffBytes += CompareByteArray(name, offset, 
                BitConverter.GetBytes(currGame.Unknown14_AntiquaFlags.ToUInt()), 
                BitConverter.GetBytes(compGame.Unknown14_AntiquaFlags.ToUInt()));

            //Unknown 15
            offset = GetGameOffset($"{nameof(currGame.Unknown15)}__{nameof(currGame.Unknown15.Offset0To16)}", out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown15.Offset0To16, compGame.Unknown15.Offset0To16);

            offset = GetGameOffset($"{nameof(currGame.Unknown15)}__{nameof(currGame.Unknown15.Offset17)}", out name);
            diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset17.ToByte(), compGame.Unknown15.Offset17.ToByte());

            offset = GetGameOffset($"{nameof(currGame.Unknown15)}__{nameof(currGame.Unknown15.Offset18)}", out name);
            diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset18.ToByte(), compGame.Unknown15.Offset18.ToByte());

            offset = GetGameOffset($"{nameof(currGame.Unknown15)}__{nameof(currGame.Unknown15.Offset19)}", out name);
            diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset19.ToByte(), compGame.Unknown15.Offset19.ToByte());

            offset = GetGameOffset($"{nameof(currGame.Unknown15)}__{nameof(currGame.Unknown15.Offset20)}", out name);
            diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset20.ToByte(), compGame.Unknown15.Offset20.ToByte());

            offset = GetGameOffset($"{nameof(currGame.Unknown15)}__{nameof(currGame.Unknown15.Offset21)}", out name);
            diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset21, compGame.Unknown15.Offset21);

            offset = GetGameOffset($"{nameof(currGame.Unknown15)}__{nameof(currGame.Unknown15.Offset22)}", out name);
            diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset22, compGame.Unknown15.Offset22);

            offset = GetGameOffset($"{nameof(currGame.Unknown15)}__{nameof(currGame.Unknown15.Offset22)}", out name);
            diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset22, compGame.Unknown15.Offset22);

            offset = GetGameOffset($"{nameof(currGame.Unknown15)}__{nameof(currGame.Unknown15.Offset23)}", out name);
            diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset23, compGame.Unknown15.Offset23);

            offset = GetGameOffset($"{nameof(currGame.Unknown15)}__{nameof(currGame.Unknown15.Offset24)}", out name);
            diffBytes += CompareByte(name, offset, currGame.Unknown15.Offset24.ToByte(), compGame.Unknown15.Offset24.ToByte());

            offset = GetGameOffset($"{nameof(currGame.Unknown15)}__{nameof(currGame.Unknown15.Offset25To117)}", out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown15.Offset25To117, compGame.Unknown15.Offset25To117);
            //Unknown 15

            // 16 A - C
            offset = GetGameOffset(nameof(currGame.Unknown16A), out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown16A, compGame.Unknown16A);

            offset = GetGameOffset(nameof(currGame.Unknown16B_GoticaFlags), out name);
            diffBytes += CompareByteArray(name, offset, 
                BitConverter.GetBytes(currGame.Unknown16B_GoticaFlags.ToUInt()), 
                BitConverter.GetBytes(compGame.Unknown16B_GoticaFlags.ToUInt()));

            offset = GetGameOffset(nameof(currGame.Unknown16C), out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown16C, compGame.Unknown16C);
            // 

            offset = GetGameOffset(nameof(currGame.Unknown17), out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown17, compGame.Unknown17);

            offset = GetGameOffset(nameof(currGame.Unknown18), out name);
            diffBytes += CompareByteArray(name, offset, currGame.Unknown18, compGame.Unknown18);

            return diffBytes;

            static int GetGameOffset(string bufferName, out string name)
            {
                name = bufferName;
                return  GetGameBufferOffset(bufferName);
            }
        }
    }
}
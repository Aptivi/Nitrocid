
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Speckles
    /// </summary>
    public static class SpecklesSettings
    {

        /// <summary>
        /// [Speckles] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SpecklesDelay
        {
            get
            {
                return Config.SaverConfig.SpecklesDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                Config.SaverConfig.SpecklesDelay = value;
            }
        }

    }

    /// <summary>
    /// Display code for Speckles
    /// </summary>
    public class SpecklesDisplay : BaseScreensaver, IScreensaver
    {

        private readonly List<(double, double, int, int, double, double, Color)> Blocks = new();

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Speckles";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Move the blocks to the direction, depending on the move direction
            for (int Block = 0; Block <= Blocks.Count - 1; Block++)
            {
                double BlockX = Blocks[Block].Item1 - Blocks[Block].Item5;
                double BlockY = Blocks[Block].Item2 - Blocks[Block].Item6;
                Blocks[Block] = (BlockX, BlockY, Blocks[Block].Item3, Blocks[Block].Item4, Blocks[Block].Item5, Blocks[Block].Item6, Blocks[Block].Item7);
            }

            // If any block is out of any range, delete it
            for (int BlockIndex = Blocks.Count - 1; BlockIndex >= 0; BlockIndex -= 1)
            {
                var Block = Blocks[BlockIndex];
                if (Block.Item1 < 0 || Block.Item1 >= ConsoleWrapper.WindowWidth ||
                    Block.Item2 < 0 || Block.Item2 >= ConsoleWrapper.WindowHeight)
                {
                    // The block went beyond. Remove it.
                    Blocks.RemoveAt(BlockIndex);
                }
            }

            // Add new block if guaranteed
            bool BlockShowGuaranteed = RandomDriver.RandomChance(25);
            int widthHalf = ConsoleWrapper.WindowWidth / 2;
            int heightHalf = ConsoleWrapper.WindowHeight / 2;
            if (BlockShowGuaranteed)
            {
                int BlockX = widthHalf;
                int BlockY = heightHalf;
                int stepsX = BlockX;
                int stepsY = BlockY;

                // Determine the end position based on the edge mode
                int blockEndX = 0;
                int blockEndY = 0;
                var edgeMode = (EdgeDirection)RandomDriver.Random(3);
                switch (edgeMode)
                {
                    case EdgeDirection.Left:
                        // (X = 0, Y = random)
                        blockEndY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);

                        // (BlockX = widthHalf - 5 .. widthHalf)
                        BlockX = RandomDriver.Random(widthHalf - 5, widthHalf);
                        break;
                    case EdgeDirection.Right:
                        // (X = max, Y = random)
                        blockEndX = ConsoleWrapper.WindowWidth - 1;
                        blockEndY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);

                        // (BlockX = widthHalf .. widthHalf + 5)
                        BlockX = RandomDriver.Random(widthHalf, widthHalf + 5);
                        break;
                    case EdgeDirection.Top:
                        // (X = random, Y = 0)
                        blockEndX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);

                        // (BlockY = heightHalf - 5 .. heightHalf)
                        BlockY = RandomDriver.Random(heightHalf - 5, heightHalf);
                        break;
                    case EdgeDirection.Bottom:
                        // (X = random, Y = max)
                        blockEndX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                        blockEndY = ConsoleWrapper.WindowHeight - 1;

                        // (BlockY = heightHalf .. heightHalf + 5)
                        BlockY = RandomDriver.Random(heightHalf, heightHalf + 5);
                        break;
                }

                // Determine the thresholds
                int blockDiffX = -(blockEndX - BlockX);
                int blockDiffY = -(blockEndY - BlockY);
                double blockThresholdX = (double)blockDiffX / stepsX;
                double blockThresholdY = (double)blockDiffY / stepsY;

                // Get the color
                var color = KernelColorTools.GetRandomColor(ColorType.TrueColor);
                Blocks.Add((BlockX, BlockY, blockEndX, blockEndY, blockThresholdX, blockThresholdY, color));
            }

            // Draw blocks
            if (!ConsoleResizeListener.WasResized(false))
            {
                for (int BlockIndex = Blocks.Count - 1; BlockIndex >= 0; BlockIndex -= 1)
                {
                    var Block = Blocks[BlockIndex];
                    char BlockSymbol = ' ';
                    int BlockX = (int)Block.Item1;
                    int BlockY = (int)Block.Item2;
                    var color = Block.Item7;
                    TextWriterWhereColor.WriteWhere(Convert.ToString(BlockSymbol), BlockX, BlockY, false, Color.Empty, color);
                }
            }
            else
            {
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...");
                Blocks.Clear();
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            ThreadManager.SleepNoBlock(SpecklesSettings.SpecklesDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            KernelColorTools.LoadBack();
        }

    }
}

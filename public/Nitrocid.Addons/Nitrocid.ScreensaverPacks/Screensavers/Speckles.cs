//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Kernel.Configuration;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Themes.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Speckles
    /// </summary>
    public class SpecklesDisplay : BaseScreensaver, IScreensaver
    {

        private readonly List<(double, double, int, int, double, double, Color)> Blocks = [];

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Speckles";

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
                var color = ColorTools.GetRandomColor(ColorType.TrueColor);
                Blocks.Add((BlockX, BlockY, blockEndX, blockEndY, blockThresholdX, blockThresholdY, color));
            }

            // Draw blocks
            if (!ConsoleResizeHandler.WasResized(false))
            {
                for (int BlockIndex = Blocks.Count - 1; BlockIndex >= 0; BlockIndex -= 1)
                {
                    var Block = Blocks[BlockIndex];
                    char BlockSymbol = ' ';
                    int BlockX = (int)Block.Item1;
                    int BlockY = (int)Block.Item2;
                    var color = Block.Item7;
                    TextWriterWhereColor.WriteWhereColorBack(Convert.ToString(BlockSymbol), BlockX, BlockY, false, Color.Empty, color);
                }
            }
            else
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.W, "Resize-syncing. Clearing...");
                Blocks.Clear();
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.SpecklesDelay);
            ThemeColorsTools.LoadBackground();
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            Blocks.Clear();
        }

    }
}

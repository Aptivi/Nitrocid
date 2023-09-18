
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

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using System;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Lightning
    /// </summary>
    public static class LightningSettings
    {

        /// <summary>
        /// [Lightning] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int LightningDelay
        {
            get
            {
                return Config.SaverConfig.LightningDelay;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                Config.SaverConfig.LightningDelay = value;
            }
        }
        /// <summary>
        /// [Lightning] Chance, in percent, to strike
        /// </summary>
        public static int LightningStrikeProbability
        {
            get
            {
                return Config.SaverConfig.LightningStrikeProbability;
            }
            set
            {
                if (value <= 0)
                    value = 5;
                Config.SaverConfig.LightningStrikeProbability = value;
            }
        }

    }

    /// <summary>
    /// Display code for Lightning
    /// </summary>
    public class LightningDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Lightning";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages { get; set; } = true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            KernelColorTools.LoadBack(0);
            ConsoleWrapper.Clear();
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            bool striking = RandomDriver.RandomChance(LightningSettings.LightningStrikeProbability);
            if (striking)
            {
                // First, determine the lightning bolt position
                int quarterWidth = ConsoleWrapper.WindowWidth / 4;
                int boltFirstHalfEndY = ConsoleWrapper.WindowHeight / 2 - 2;
                int boltSecondHalfEndY = ConsoleWrapper.WindowHeight / 2 + 2;
                int boltTopEdgeX = ConsoleWrapper.WindowWidth / 2 - RandomDriver.Random(-quarterWidth, quarterWidth);
                int boltBottomEdgeX = boltTopEdgeX;
                int boltFirstHalfEndX = boltTopEdgeX - 8;
                int boltSecondHalfEndX = boltBottomEdgeX + 8;

                // Determine the thresholds
                double boltFromTopToFirstHalfX = -((double)(boltTopEdgeX - boltFirstHalfEndX) / boltFirstHalfEndY);
                double boltFromFirstHalfToSecondHalfY = (double)(boltSecondHalfEndY - boltFirstHalfEndY) / (boltSecondHalfEndX - boltFirstHalfEndX);
                double boltFromSecondHalfToBottomX = (double)(boltBottomEdgeX - boltSecondHalfEndX) / boltSecondHalfEndY;

                for (int step = 1; step <= 5; step++)
                {
                    // If the step number is even, show the lightning. Else, show the white background.
                    bool showStrike = step % 2 == 0;
                    if (showStrike)
                    {
                        // Draw the flashes first
                        for (int y = 0; y < boltFirstHalfEndY; y++)
                        {
                            int x = (int)Math.Round(boltTopEdgeX + boltFromTopToFirstHalfX * y, MidpointRounding.ToZero);
                            TextWriterWhereColor.WriteWhere(" ", x + 1, y, Color.Empty, new Color(ConsoleColors.Yellow3_d7d700));
                            TextWriterWhereColor.WriteWhere(" ", x, y, Color.Empty, new Color(ConsoleColors.Yellow3_d7d700));
                            TextWriterWhereColor.WriteWhere(" ", x - 1, y, Color.Empty, new Color(ConsoleColors.Yellow3_d7d700));
                        }
                        int xCount = 0;
                        for (int x = boltFirstHalfEndX; x < boltSecondHalfEndX; x++)
                        {
                            int y = (int)Math.Round(boltFirstHalfEndY + boltFromFirstHalfToSecondHalfY * xCount, MidpointRounding.ToZero);
                            TextWriterWhereColor.WriteWhere(" ", x + 1, y + 1, Color.Empty, new Color(ConsoleColors.Yellow3_d7d700));
                            TextWriterWhereColor.WriteWhere(" ", x + 1, y, Color.Empty, new Color(ConsoleColors.Yellow3_d7d700));
                            TextWriterWhereColor.WriteWhere(" ", x + 1, y - 1, Color.Empty, new Color(ConsoleColors.Yellow3_d7d700));
                            TextWriterWhereColor.WriteWhere(" ", x, y + 1, Color.Empty, new Color(ConsoleColors.Yellow3_d7d700));
                            TextWriterWhereColor.WriteWhere(" ", x, y, Color.Empty, new Color(ConsoleColors.Yellow3_d7d700));
                            TextWriterWhereColor.WriteWhere(" ", x, y - 1, Color.Empty, new Color(ConsoleColors.Yellow3_d7d700));
                            TextWriterWhereColor.WriteWhere(" ", x - 1, y + 1, Color.Empty, new Color(ConsoleColors.Yellow3_d7d700));
                            TextWriterWhereColor.WriteWhere(" ", x - 1, y, Color.Empty, new Color(ConsoleColors.Yellow3_d7d700));
                            TextWriterWhereColor.WriteWhere(" ", x - 1, y - 1, Color.Empty, new Color(ConsoleColors.Yellow3_d7d700));
                            xCount++;
                        }
                        int yCount = 0;
                        for (int y = boltSecondHalfEndY; y < ConsoleWrapper.WindowHeight; y++)
                        {
                            int x = (int)Math.Round(boltSecondHalfEndX + boltFromSecondHalfToBottomX * yCount, MidpointRounding.ToZero);
                            TextWriterWhereColor.WriteWhere(" ", x + 1, y, Color.Empty, new Color(ConsoleColors.Yellow3_d7d700));
                            TextWriterWhereColor.WriteWhere(" ", x, y, Color.Empty, new Color(ConsoleColors.Yellow3_d7d700));
                            TextWriterWhereColor.WriteWhere(" ", x - 1, y, Color.Empty, new Color(ConsoleColors.Yellow3_d7d700));
                            yCount++;
                        }

                        // Draw the lightning!
                        for (int y = 0; y < boltFirstHalfEndY; y++)
                        {
                            int x = (int)Math.Round(boltTopEdgeX + boltFromTopToFirstHalfX * y, MidpointRounding.ToZero);
                            TextWriterWhereColor.WriteWhere(" ", x, y, Color.Empty, new Color(ConsoleColors.LightYellow3));
                        }
                        xCount = 0;
                        for (int x = boltFirstHalfEndX; x < boltSecondHalfEndX; x++)
                        {
                            int y = (int)Math.Round(boltFirstHalfEndY + boltFromFirstHalfToSecondHalfY * xCount, MidpointRounding.ToZero);
                            TextWriterWhereColor.WriteWhere(" ", x, y, Color.Empty, new Color(ConsoleColors.LightYellow3));
                            xCount++;
                        }
                        yCount = 0;
                        for (int y = boltSecondHalfEndY; y < ConsoleWrapper.WindowHeight; y++)
                        {
                            int x = (int)Math.Round(boltSecondHalfEndX + boltFromSecondHalfToBottomX * yCount, MidpointRounding.ToZero);
                            TextWriterWhereColor.WriteWhere(" ", x, y, Color.Empty, new Color(ConsoleColors.LightYellow3));
                            yCount++;
                        }
                        ThreadManager.SleepNoBlock(LightningSettings.LightningDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                    }
                    else
                    {
                        // Show only the white background
                        KernelColorTools.LoadBack(new Color(ConsoleColors.White));
                        ThreadManager.SleepNoBlock(LightningSettings.LightningDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        KernelColorTools.LoadBack(new Color(ConsoleColors.Black));
                    }
                }
                KernelColorTools.LoadBack(new Color(ConsoleColors.Black));
            }
            ThreadManager.SleepNoBlock(LightningSettings.LightningDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}

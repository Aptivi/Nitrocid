
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

using ColorSeq;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Drivers.RNG;
using KS.Kernel.Configuration;
using KS.Misc.Reflection;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using System;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Screensaver.Displays
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
        public override void ScreensaverLogic()
        {
            bool striking = RandomDriver.RandomChance(LightningSettings.LightningStrikeProbability);
            if (striking)
            {
                // First, determine the lightning bolt position
                int boltEdgeMinPos = 3 * (ConsoleWrapper.WindowWidth / 8);
                int boltEdgeMaxPos = 5 * (ConsoleWrapper.WindowWidth / 8);
                int boltFirstHalfEndY = (ConsoleWrapper.WindowHeight / 2) - 2;
                int boltSecondHalfEndY = (ConsoleWrapper.WindowHeight / 2) + 2;
                int boltTopEdgeX = RandomDriver.Random(boltEdgeMinPos, boltEdgeMaxPos);
                int boltBottomEdgeX = RandomDriver.Random(boltEdgeMinPos, boltEdgeMaxPos);
                int boltFirstHalfEndX = boltTopEdgeX - 2;
                int boltSecondHalfEndX = boltBottomEdgeX + 2;
                IntegerTools.SwapIfSourceLarger(ref boltFirstHalfEndX, ref boltSecondHalfEndX);
                bool swapHalf = boltTopEdgeX < boltBottomEdgeX;

                // Determine the thresholds
                double boltFromTopToFirstHalfX = -((double)(boltTopEdgeX - boltFirstHalfEndX) / boltTopEdgeX);
                double boltFromFirstHalfToSecondHalfY = (double)(boltSecondHalfEndY - boltFirstHalfEndY) / boltSecondHalfEndY;
                double boltFromSecondHalfToBottomX = -((double)(boltBottomEdgeX - boltSecondHalfEndX) / boltBottomEdgeX);

                for (int step = 1; step <= 5; step++)
                {
                    // If the step number is even, show the lightning. Else, show the white background.
                    bool showStrike = step % 2 == 0;
                    if (showStrike)
                    {
                        // Show the lightning!
                        for (int y = 0; y < boltFirstHalfEndY; y++)
                        {
                            int x = (int)Math.Round(boltTopEdgeX + (boltFromTopToFirstHalfX * y), MidpointRounding.ToZero);
                            TextWriterWhereColor.WriteWhere(" ", x, y, Color.Empty, new Color(ConsoleColors.White));
                        }
                        int xCount = 0;
                        if (swapHalf)
                        {
                            for (int x = boltFirstHalfEndX; x < boltSecondHalfEndX; x++)
                            {
                                int y = (int)Math.Round(boltFirstHalfEndY + (boltFromFirstHalfToSecondHalfY * xCount), MidpointRounding.ToZero);
                                TextWriterWhereColor.WriteWhere(" ", x, y, Color.Empty, new Color(ConsoleColors.White));
                                xCount++;
                            }
                        }
                        else
                        {
                            for (int x = boltSecondHalfEndX; x > boltFirstHalfEndX; x--)
                            {
                                int y = (int)Math.Round(boltFirstHalfEndY + (boltFromFirstHalfToSecondHalfY * xCount), MidpointRounding.ToZero);
                                TextWriterWhereColor.WriteWhere(" ", x, y, Color.Empty, new Color(ConsoleColors.White));
                                xCount++;
                            }
                        }
                        int yCount = 0;
                        for (int y = boltSecondHalfEndY; y < ConsoleWrapper.WindowHeight; y++)
                        {
                            int x = (int)Math.Round(boltBottomEdgeX + (boltFromSecondHalfToBottomX * yCount), MidpointRounding.ToZero);
                            TextWriterWhereColor.WriteWhere(" ", x, y, Color.Empty, new Color(ConsoleColors.White));
                            yCount++;
                        }
                        ThreadManager.SleepNoBlock(LightningSettings.LightningDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                    }
                    else
                    {
                        // Show only the white background
                        ColorTools.LoadBack(new Color(ConsoleColors.White), true);
                        ThreadManager.SleepNoBlock(LightningSettings.LightningDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
                        ColorTools.LoadBack(new Color(ConsoleColors.Black), true);
                    }
                }
                ColorTools.LoadBack(new Color(ConsoleColors.Black), true);
            }
            ThreadManager.SleepNoBlock(LightningSettings.LightningDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}

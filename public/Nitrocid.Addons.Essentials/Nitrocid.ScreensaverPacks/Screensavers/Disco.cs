//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Colors.Data;
using Nitrocid.Kernel.Configuration;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Disco
    /// </summary>
    public class DiscoDisplay : BaseScreensaver, IScreensaver
    {

        private int CurrentColor = 0;
        private int CurrentColorR, CurrentColorG, CurrentColorB;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Disco";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages =>
            true;

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int MaximumColors = ScreensaverPackInit.SaversConfig.DiscoMaximumColorLevel;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", vars: [MaximumColors]);
            int MaximumColorsR = ScreensaverPackInit.SaversConfig.DiscoMaximumRedColorLevel;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", vars: [MaximumColorsR]);
            int MaximumColorsG = ScreensaverPackInit.SaversConfig.DiscoMaximumGreenColorLevel;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", vars: [MaximumColorsG]);
            int MaximumColorsB = ScreensaverPackInit.SaversConfig.DiscoMaximumBlueColorLevel;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", vars: [MaximumColorsB]);

            // Select the background color
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Cycling colors: {0}", vars: [ScreensaverPackInit.SaversConfig.DiscoCycleColors]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "fed (future-eyes-destroyer) mode: {0}", vars: [ScreensaverPackInit.SaversConfig.DiscoEnableFedMode]);
            if (!ScreensaverPackInit.SaversConfig.DiscoEnableFedMode)
            {
                if (ScreensaverPackInit.SaversConfig.DiscoTrueColor)
                {
                    if (!ScreensaverPackInit.SaversConfig.DiscoCycleColors)
                    {
                        int RedColorNum = RandomDriver.Random(255);
                        int GreenColorNum = RandomDriver.Random(255);
                        int BlueColorNum = RandomDriver.Random(255);
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                        var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                        ConsoleColoring.SetConsoleColorDry(ColorStorage, true);
                    }
                    else
                    {
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [CurrentColorR, CurrentColorG, CurrentColorB]);
                        var ColorStorage = new Color(CurrentColorR, CurrentColorG, CurrentColorB);
                        ConsoleColoring.SetConsoleColorDry(ColorStorage, true);
                    }
                }
                else
                {
                    if (!ScreensaverPackInit.SaversConfig.DiscoCycleColors)
                    {
                        int color = RandomDriver.Random(255);
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [color]);
                        ConsoleColoring.SetConsoleColorDry(new Color(color), true);
                    }
                    else
                    {
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [CurrentColor]);
                        ConsoleColoring.SetConsoleColorDry(new Color(CurrentColor), true);
                    }
                }
            }
            else
            {
                if (CurrentColor == (int)ConsoleColors.Black)
                {
                    CurrentColor = (int)ConsoleColors.White;
                }
                else
                {
                    CurrentColor = (int)ConsoleColors.Black;
                }
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [CurrentColor]);
                ConsoleColoring.SetConsoleColorDry(new Color(CurrentColor), true);
            }

            // Make the disco effect!
            ConsoleWrapper.Clear();

            // Switch to the next color
            if (ScreensaverPackInit.SaversConfig.DiscoTrueColor)
            {
                if (CurrentColorR >= MaximumColorsR)
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Red level exceeded maximum color. {0} >= {1}", vars: [CurrentColorR, MaximumColorsR]);
                    CurrentColorR = 0;
                }
                else
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Stepping one (R)...");
                    CurrentColorR += 1;
                }
                if (CurrentColorG >= MaximumColorsG)
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Green level exceeded maximum color. {0} >= {1}", vars: [CurrentColorG, MaximumColorsG]);
                    CurrentColorG = 0;
                }
                else if (CurrentColorR == 0)
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Stepping one (G)...");
                    CurrentColorG += 1;
                }
                if (CurrentColorB >= MaximumColorsB)
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Blue level exceeded maximum color. {0} >= {1}", vars: [CurrentColorB, MaximumColorsB]);
                    CurrentColorB = 0;
                }
                else if (CurrentColorG == 0 & CurrentColorR == 0)
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Stepping one (B)...");
                    CurrentColorB += 1;
                }
                if (CurrentColorB == 0 & CurrentColorG == 0 & CurrentColorR == 0)
                {
                    CurrentColorB = 0;
                    CurrentColorG = 0;
                    CurrentColorR = 0;
                }
            }
            else if (CurrentColor >= MaximumColors)
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Color level exceeded maximum color. {0} >= {1}", vars: [CurrentColor, MaximumColors]);
                CurrentColor = 0;
            }
            else
            {
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Stepping one...");
                CurrentColor += 1;
            }

            // Check to see if we're dealing with beats per minute
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Using BPM: {0}", vars: [ScreensaverPackInit.SaversConfig.DiscoUseBeatsPerMinute]);
            if (ScreensaverPackInit.SaversConfig.DiscoUseBeatsPerMinute)
            {
                int BeatInterval = (int)Math.Round(60000d / ScreensaverPackInit.SaversConfig.DiscoDelay);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Beat interval from {0} BPM: {1} ms", vars: [ScreensaverPackInit.SaversConfig.DiscoDelay, BeatInterval]);
                ScreensaverManager.Delay(BeatInterval);
            }
            else
            {
                ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.DiscoDelay);
            }
        }

    }
}

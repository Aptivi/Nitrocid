//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Figletize;
using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.FancyWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Languages;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for CommitMilestone
    /// </summary>
    public static class CommitMilestoneSettings
    {

        /// <summary>
        /// [CommitMilestone] Enable truecolor support. Has a higher priority than 255 color support.
        /// </summary>
        public static bool CommitMilestoneTrueColor
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.CommitMilestoneTrueColor;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.CommitMilestoneTrueColor = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int CommitMilestoneDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.CommitMilestoneDelay;
            }
            set
            {
                if (value <= 0)
                    value = 1000;
                ScreensaverPackInit.SaversConfig.CommitMilestoneDelay = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] Enables the rainbow colors mode
        /// </summary>
        public static bool CommitMilestoneRainbowMode
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.CommitMilestoneRainbowMode;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.CommitMilestoneRainbowMode = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The minimum red color level (true color)
        /// </summary>
        public static int CommitMilestoneMinimumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumRedColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The minimum green color level (true color)
        /// </summary>
        public static int CommitMilestoneMinimumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumGreenColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The minimum blue color level (true color)
        /// </summary>
        public static int CommitMilestoneMinimumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumBlueColorLevel;
            }
            set
            {
                if (value <= 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The minimum color level (255 colors or 16 colors)
        /// </summary>
        public static int CommitMilestoneMinimumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumColorLevel;
            }
            set
            {
                int FinalMinimumLevel = 255;
                if (value <= 0)
                    value = 0;
                if (value > FinalMinimumLevel)
                    value = FinalMinimumLevel;
                ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The maximum red color level (true color)
        /// </summary>
        public static int CommitMilestoneMaximumRedColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.CommitMilestoneMaximumRedColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumRedColorLevel)
                    value = ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumRedColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.CommitMilestoneMaximumRedColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The maximum green color level (true color)
        /// </summary>
        public static int CommitMilestoneMaximumGreenColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.CommitMilestoneMaximumGreenColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumGreenColorLevel)
                    value = ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumGreenColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.CommitMilestoneMaximumGreenColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The maximum blue color level (true color)
        /// </summary>
        public static int CommitMilestoneMaximumBlueColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.CommitMilestoneMaximumBlueColorLevel;
            }
            set
            {
                if (value <= ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumBlueColorLevel)
                    value = ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumBlueColorLevel;
                if (value > 255)
                    value = 255;
                ScreensaverPackInit.SaversConfig.CommitMilestoneMaximumBlueColorLevel = value;
            }
        }
        /// <summary>
        /// [CommitMilestone] The maximum color level (255 colors or 16 colors)
        /// </summary>
        public static int CommitMilestoneMaximumColorLevel
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.CommitMilestoneMaximumColorLevel;
            }
            set
            {
                int FinalMaximumLevel = 255;
                if (value <= ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumColorLevel)
                    value = ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumColorLevel;
                if (value > FinalMaximumLevel)
                    value = FinalMaximumLevel;
                ScreensaverPackInit.SaversConfig.CommitMilestoneMaximumColorLevel = value;
            }
        }

    }

    /// <summary>
    /// Display code for CommitMilestone
    /// </summary>
    public class CommitMilestoneDisplay : BaseScreensaver, IScreensaver
    {

        private int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "CommitMilestone";

        /// <inheritdoc/>
        public override void ScreensaverPreparation() =>
            ColorTools.LoadBack();

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            var figFontUsed = FigletTools.GetFigletFont("Banner2");
            var figFontFallback = FigletTools.GetFigletFont("small");
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();

            // Set colors
            Color ColorStorage;
            if (CommitMilestoneSettings.CommitMilestoneTrueColor)
            {
                int RedColorNum = RandomDriver.Random(CommitMilestoneSettings.CommitMilestoneMinimumRedColorLevel, CommitMilestoneSettings.CommitMilestoneMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(CommitMilestoneSettings.CommitMilestoneMinimumGreenColorLevel, CommitMilestoneSettings.CommitMilestoneMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(CommitMilestoneSettings.CommitMilestoneMinimumBlueColorLevel, CommitMilestoneSettings.CommitMilestoneMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(CommitMilestoneSettings.CommitMilestoneMinimumColorLevel, CommitMilestoneSettings.CommitMilestoneMaximumColorLevel);
                DebugWriter.WriteDebugConditional(ScreensaverManager.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                ColorStorage = new Color(ColorNum);
            }
            if (CommitMilestoneSettings.CommitMilestoneRainbowMode)
            {
                ColorStorage = new($"hsl:{currentHueAngle};100;50");
                currentHueAngle++;
                if (currentHueAngle > 360)
                    currentHueAngle = 0;
            }

            // Prepare the figlet font for writing
            string text = "6,000!";
            string textDesc = Translate.DoTranslation("Celebrating the 6,000th commit since 0.0.1!");
            int figWidth = FigletTools.GetFigletWidth(text, figFontUsed) / 2;
            int figHeight = FigletTools.GetFigletHeight(text, figFontUsed) / 2;
            int figWidthFallback = FigletTools.GetFigletWidth(text, figFontFallback) / 2;
            int figHeightFallback = FigletTools.GetFigletHeight(text, figFontFallback) / 2;
            int width = ConsoleWrapper.WindowWidth;
            int height = ConsoleWrapper.WindowHeight;
            int consoleX = (width / 2) - figWidth;
            int consoleY = (height / 2) - figHeight;

            // Write it!
            if (!ConsoleResizeListener.WasResized(false))
            {
                ConsoleWrapper.Clear();
                if (consoleX < 0 || consoleY < 0)
                {
                    // The figlet won't fit, so use small text
                    consoleX = (width / 2) - figWidthFallback;
                    consoleY = (height / 2) - figHeightFallback;
                    if (consoleX < 0 || consoleY < 0)
                    {
                        // The fallback figlet also won't fit, so use smaller text
                        CenteredTextColor.WriteCenteredColor(consoleY, text, ColorStorage);
                        consoleY = height / 2;
                    }
                    else
                    {
                        CenteredFigletTextColor.WriteCenteredFigletColor(consoleY, figFontFallback, text, ColorStorage);
                        consoleY += figHeightFallback * 2;
                    }
                }
                else
                {
                    CenteredFigletTextColor.WriteCenteredFigletColor(consoleY, figFontUsed, text, ColorStorage);
                    consoleY += figHeight * 2;
                }
                CenteredTextColor.WriteCenteredColor(consoleY + 2, textDesc, ColorStorage);
            }

            // Reset resize sync
            ConsoleResizeListener.WasResized();
            int delay = CommitMilestoneSettings.CommitMilestoneRainbowMode ? 16 : CommitMilestoneSettings.CommitMilestoneDelay;
            ThreadManager.SleepNoBlock(delay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            currentHueAngle = 0;
        }

    }
}

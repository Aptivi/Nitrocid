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

using Textify.Data.Figlet;
using Terminaux.Writer.FancyWriters;
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Languages;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Kernel.Configuration;
using Terminaux.Colors;
using Terminaux.Base;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
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
            if (ScreensaverPackInit.SaversConfig.CommitMilestoneTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.CommitMilestoneMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.CommitMilestoneMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.CommitMilestoneMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum);
                ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumColorLevel, ScreensaverPackInit.SaversConfig.CommitMilestoneMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum);
                ColorStorage = new Color(ColorNum);
            }
            if (ScreensaverPackInit.SaversConfig.CommitMilestoneRainbowMode)
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
            if (!ConsoleResizeHandler.WasResized(false))
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
            ConsoleResizeHandler.WasResized();
            int delay = ScreensaverPackInit.SaversConfig.CommitMilestoneRainbowMode ? 16 : ScreensaverPackInit.SaversConfig.CommitMilestoneDelay;
            ThreadManager.SleepNoBlock(delay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            currentHueAngle = 0;
        }

    }
}

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

using Textify.Data.Figlet;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Simple;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for CommitMilestone
    /// </summary>
    public class CommitMilestoneDisplay : BaseScreensaver, IScreensaver
    {

        private int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "CommitMilestone";

        /// <inheritdoc/>
        public override void ScreensaverPreparation() =>
            ThemeColorsTools.LoadBackground();

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
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.CommitMilestoneMinimumColorLevel, ScreensaverPackInit.SaversConfig.CommitMilestoneMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
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
            string text = "7,000!";
            string textDesc = LanguageTools.GetLocalized("NKS_SCREENSAVERPACKS_COMMITMILESTONE_CONGRATS");
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
                var figletText = new FigletText(figFontUsed)
                {
                    Text = text,
                    ForegroundColor = ColorStorage
                };
                if (consoleX < 0 || consoleY < 0)
                {
                    // The figlet won't fit, so use small text
                    consoleX = (width / 2) - figWidthFallback;
                    consoleY = (height / 2) - figHeightFallback;
                    if (consoleX < 0 || consoleY < 0)
                    {
                        // The fallback figlet also won't fit, so use smaller text
                        consoleX = (width / 2) - (text.Length / 2);
                        consoleY = height / 2;
                        TextWriterWhereColor.WriteWhere(text, consoleX, consoleY, true);
                    }
                    else
                    {
                        // Write the figlet.
                        figletText.Font = figFontFallback;
                        RendererTools.WriteRenderable(figletText, new(consoleX, consoleY));
                        consoleY += figHeightFallback * 2;
                    }
                }
                else
                {
                    // Write the figlet.
                    RendererTools.WriteRenderable(figletText, new(consoleX, consoleY));
                    consoleY += figHeight * 2;
                }
                var descText = new AlignedText()
                {
                    Top = consoleY + 2,
                    Text = textDesc,
                    ForegroundColor = ColorStorage,
                    Settings = new()
                    {
                        Alignment = TextAlignment.Middle
                    }
                };
                TextWriterRaw.WriteRaw(descText.Render());
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            int delay = ScreensaverPackInit.SaversConfig.CommitMilestoneRainbowMode ? 16 : ScreensaverPackInit.SaversConfig.CommitMilestoneDelay;
            ScreensaverManager.Delay(delay);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            currentHueAngle = 0;
        }

    }
}

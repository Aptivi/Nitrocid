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

using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Base;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for MultiLines
    /// </summary>
    public class MultiLinesDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "MultiLines";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            ColorTools.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.MultiLinesBackgroundColor));

            // Draw few lines
            string lineString = !string.IsNullOrWhiteSpace(ScreensaverPackInit.SaversConfig.MultiLinesLineChar) ? ScreensaverPackInit.SaversConfig.MultiLinesLineChar : "-";
            string Line = new(lineString[0], ConsoleWrapper.WindowWidth);
            int linesCount = RandomDriver.Random(1, 10);
            for (int i = 0; i < linesCount; i++)
            {
                if (ScreensaverManager.Bailing)
                    return;

                // Select a color
                if (ScreensaverPackInit.SaversConfig.MultiLinesTrueColor)
                {
                    int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.MultiLinesMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.MultiLinesMaximumRedColorLevel);
                    int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.MultiLinesMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.MultiLinesMaximumGreenColorLevel);
                    int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.MultiLinesMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.MultiLinesMaximumBlueColorLevel);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                    var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                    ColorTools.SetConsoleColor(ColorStorage);
                }
                else
                {
                    int color = RandomDriver.Random(ScreensaverPackInit.SaversConfig.MultiLinesMinimumColorLevel, ScreensaverPackInit.SaversConfig.MultiLinesMaximumColorLevel);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [color]);
                    ColorTools.SetConsoleColor(new Color(color));
                }

                // Now, draw a line
                int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got top position ({0})", vars: [Top]);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ConsoleWrapper.SetCursorPosition(0, Top);
                    ConsoleWrapper.WriteLine(Line);
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.MultiLinesDelay);
        }

    }
}

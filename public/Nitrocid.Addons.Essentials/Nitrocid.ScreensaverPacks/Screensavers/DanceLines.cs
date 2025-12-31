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
using Terminaux.Base;
using Terminaux.Colors;
using Nitrocid.Base.Kernel.Configuration;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for DanceLines
    /// </summary>
    public class DanceLinesDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "DanceLines";

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
            ColorTools.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.DanceLinesBackgroundColor));

            // Draw few lines
            string lineString = !string.IsNullOrWhiteSpace(ScreensaverPackInit.SaversConfig.DanceLinesLineChar) ? ScreensaverPackInit.SaversConfig.DanceLinesLineChar : "-";
            for (int i = 0; i < ConsoleWrapper.WindowHeight; i++)
            {
                // Draw a randomly-sized line
                string line = new(lineString[0], RandomDriver.Random(ConsoleWrapper.WindowWidth));

                // Select a color
                if (ScreensaverPackInit.SaversConfig.DanceLinesTrueColor)
                {
                    int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DanceLinesMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.DanceLinesMaximumRedColorLevel);
                    int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DanceLinesMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.DanceLinesMaximumGreenColorLevel);
                    int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DanceLinesMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.DanceLinesMaximumBlueColorLevel);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                    var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                    ColorTools.SetConsoleColor(ColorStorage);
                }
                else
                {
                    int color = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DanceLinesMinimumColorLevel, ScreensaverPackInit.SaversConfig.DanceLinesMaximumColorLevel);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [color]);
                    ColorTools.SetConsoleColor(new Color(color));
                }

                // Now, draw a line
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got top position ({0})", vars: [i]);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ConsoleWrapper.SetCursorPosition(0, i);
                    ConsoleWrapper.Write(line);
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.DanceLinesDelay);
        }

    }
}

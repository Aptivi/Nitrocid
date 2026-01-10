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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for SnakeFill
    /// </summary>
    public class SnakeFillDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "SnakeFill";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Select a color
            if (ScreensaverPackInit.SaversConfig.SnakeFillTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.SnakeFillMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.SnakeFillMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.SnakeFillMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.SnakeFillMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.SnakeFillMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.SnakeFillMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                if (!ConsoleResizeHandler.WasResized(false))
                    ConsoleColoring.SetConsoleColorDry(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), true);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.SnakeFillMinimumColorLevel, ScreensaverPackInit.SaversConfig.SnakeFillMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                if (!ConsoleResizeHandler.WasResized(false))
                    ConsoleColoring.SetConsoleColorDry(new Color(ColorNum), true);
            }

            // Set max height
            int MaxWindowHeight = ConsoleWrapper.WindowHeight - 1;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Max height {0}", vars: [MaxWindowHeight]);

            // Fill the screen!
            bool reverseHeightAxis = false;
            for (int x = 0; x < ConsoleWrapper.WindowWidth; x++)
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;

                // Select the height and fill the entire screen
                if (reverseHeightAxis)
                {
                    for (int y = MaxWindowHeight; y >= 0; y--)
                    {
                        if (ConsoleResizeHandler.WasResized(false))
                            break;
                        if (ScreensaverManager.Bailing)
                            return;

                        TextWriterWhereColor.WriteWherePlain(" ", x, y);
                        ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.SnakeFillDelay);
                        reverseHeightAxis = false;
                    }
                }
                else
                {
                    for (int y = 0; y <= MaxWindowHeight; y++)
                    {
                        if (ConsoleResizeHandler.WasResized(false))
                            break;
                        if (ScreensaverManager.Bailing)
                            return;

                        TextWriterWhereColor.WriteWherePlain(" ", x, y);
                        ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.SnakeFillDelay);
                        reverseHeightAxis = true;
                    }
                }
            }
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.SnakeFillDelay);
        }

    }
}

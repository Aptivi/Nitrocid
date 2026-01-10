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
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for GlitterColor
    /// </summary>
    public class GlitterColorDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "GlitterColor";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Select position
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", vars: [Left, Top]);
            ConsoleWrapper.SetCursorPosition(Left, Top);

            // Make a glitter color
            if (ScreensaverPackInit.SaversConfig.GlitterColorTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.GlitterColorMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.GlitterColorMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.GlitterColorMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.GlitterColorMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.GlitterColorMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.GlitterColorMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ConsoleColoring.SetConsoleColorDry(ColorStorage, true);
                    ConsoleWrapper.Write(" ");
                }
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.GlitterColorMinimumColorLevel, ScreensaverPackInit.SaversConfig.GlitterColorMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ConsoleColoring.SetConsoleColorDry(new Color(ColorNum), true);
                    ConsoleWrapper.Write(" ");
                }
            }
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.GlitterColorDelay);
        }

    }
}

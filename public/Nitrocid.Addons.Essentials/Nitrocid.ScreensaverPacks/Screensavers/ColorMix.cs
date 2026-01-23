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
using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Nitrocid.Kernel.Configuration;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for ColorMix
    /// </summary>
    public class ColorMixDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "ColorMix";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleColoring.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.ColorMixBackgroundColor));
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int EndLeft = ConsoleWrapper.WindowWidth - 1;
            int EndTop = ConsoleWrapper.WindowHeight - 1;
            int Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
            int Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "End left: {0} | End top: {1}", vars: [EndLeft, EndTop]);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got left: {0} | Got top: {1}", vars: [Left, Top]);

            // Fill the color if not filled
            if (ConsoleWrapper.CursorLeft >= EndLeft && ConsoleWrapper.CursorTop >= EndTop)
                ConsoleWrapper.SetCursorPosition(0, 0);
            else
            {
                Color colorStorage;
                if (ScreensaverPackInit.SaversConfig.ColorMixTrueColor)
                {
                    int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ColorMixMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.ColorMixMaximumRedColorLevel);
                    int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ColorMixMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.ColorMixMaximumGreenColorLevel);
                    int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ColorMixMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.ColorMixMaximumBlueColorLevel);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                    colorStorage = new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}");
                }
                else
                {
                    int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ColorMixMinimumColorLevel, ScreensaverPackInit.SaversConfig.ColorMixMaximumColorLevel);
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                    colorStorage = new Color(ColorNum);
                }

                if (!ConsoleResizeHandler.WasResized(false))
                {
                    ConsoleColoring.SetConsoleColorDry(Color.Empty);
                    ConsoleColoring.SetConsoleColorDry(colorStorage, true);
                    TextWriterRaw.WritePlain(" ", false);
                }
                else
                {
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "We're refilling...");
                    ConsoleColoring.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.ColorMixBackgroundColor));
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.ColorMixDelay);
        }

    }
}

//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for FlashText
    /// </summary>
    public class FlashTextDisplay : BaseScreensaver, IScreensaver
    {

        private int Left, Top;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "FlashText";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.FlashTextBackgroundColor));
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", vars: [ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight]);

            // Select position
            Left = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth - ScreensaverPackInit.SaversConfig.FlashTextWrite.Length);
            Top = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", vars: [Left, Top]);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Make two delay halves to make up one half for screen with text and one half for screen with no text to make a flashing effect
            int HalfDelay = (int)Math.Round(ScreensaverPackInit.SaversConfig.FlashTextDelay / 2d);

            // Make a flashing text
            if (ScreensaverPackInit.SaversConfig.FlashTextTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FlashTextMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.FlashTextMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FlashTextMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.FlashTextMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FlashTextMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.FlashTextMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                var ColorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    TextWriterWhereColor.WriteWhereColorBack(ScreensaverPackInit.SaversConfig.FlashTextWrite, Left, Top, true, ColorStorage, ScreensaverPackInit.SaversConfig.FlashTextBackgroundColor);
                }
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.FlashTextMinimumColorLevel, ScreensaverPackInit.SaversConfig.FlashTextMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                if (!ConsoleResizeHandler.WasResized(false))
                {
                    TextWriterWhereColor.WriteWhereColorBack(ScreensaverPackInit.SaversConfig.FlashTextWrite, Left, Top, true, new Color(ColorNum), ScreensaverPackInit.SaversConfig.FlashTextBackgroundColor);
                }
            }
            ScreensaverManager.Delay(HalfDelay);
            ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
            ScreensaverManager.Delay(HalfDelay);

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
        }

    }
}

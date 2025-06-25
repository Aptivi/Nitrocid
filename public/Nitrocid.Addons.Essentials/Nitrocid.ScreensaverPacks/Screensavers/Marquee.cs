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
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Base.Extensions;
using Terminaux.Colors;
using Textify.General;
using Nitrocid.Base.Kernel.Configuration;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Marquee
    /// </summary>
    public class MarqueeDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Marquee";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ColorTools.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.MarqueeBackgroundColor));
            ScreensaverPackInit.SaversConfig.MarqueeWrite = ScreensaverPackInit.SaversConfig.MarqueeWrite.ReplaceAll([Convert.ToChar(13).ToString(), Convert.ToChar(10).ToString()], " - ");
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            ConsoleWrapper.Clear();

            // Ensure that the top position of the written text is always centered if AlwaysCentered is enabled. Else, select a random height.
            int TopPrinted = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            if (!ScreensaverPackInit.SaversConfig.MarqueeAlwaysCentered)
            {
                TopPrinted = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
            }
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Top position: {0}", vars: [TopPrinted]);

            // Start with the left position as the right position.
            int CurrentLeft = ConsoleWrapper.WindowWidth - 1;
            int CurrentLeftOtherEnd = ConsoleWrapper.WindowWidth - 1;
            int CurrentCharacterNum = 0;

            // We need to set colors for the text.
            if (ScreensaverPackInit.SaversConfig.MarqueeTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.MarqueeMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.MarqueeMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.MarqueeMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.MarqueeMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.MarqueeMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.MarqueeMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                ColorTools.SetConsoleColor(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"));
            }
            else
            {
                int color = RandomDriver.Random(ScreensaverPackInit.SaversConfig.MarqueeMinimumColorLevel, ScreensaverPackInit.SaversConfig.MarqueeMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [color]);
                ColorTools.SetConsoleColor(new Color(color));
            }

            // If the text is at the right and is longer than the console width, crop it until it's complete.
            while (CurrentLeftOtherEnd != 0)
            {
                ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.MarqueeDelay);
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverPackInit.SaversConfig.MarqueeUseConsoleAPI)
                    ConsoleWrapper.Clear();
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Current left: {0} | Current left on other end: {1}", vars: [CurrentLeft, CurrentLeftOtherEnd]);

                // Declare variable for written marquee text
                string MarqueeWritten = ScreensaverPackInit.SaversConfig.MarqueeWrite;
                bool Middle = ScreensaverPackInit.SaversConfig.MarqueeWrite.Length - (CurrentLeftOtherEnd - CurrentLeft) != CurrentCharacterNum - (CurrentLeftOtherEnd - CurrentLeft);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Middle of long text: {0}", vars: [Middle]);

                // If the current left position is not zero (not on the left), take the substring starting from the beginning of the string until the
                // written variable equals the base text variable. However, if we're on the left, take the substring so that the character which was
                // shown previously won't be shown again.
                if (CurrentLeft != 0)
                {
                    MarqueeWritten = MarqueeWritten[..(CurrentLeftOtherEnd - CurrentLeft)];
                }
                else if (CurrentLeft == 0 & Middle)
                {
                    MarqueeWritten = MarqueeWritten.Substring(CurrentCharacterNum - (CurrentLeftOtherEnd - CurrentLeft), CurrentLeftOtherEnd - CurrentLeft);
                }
                else
                {
                    MarqueeWritten = MarqueeWritten[(ScreensaverPackInit.SaversConfig.MarqueeWrite.Length - (CurrentLeftOtherEnd - CurrentLeft))..];
                }
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Written result: {0}", vars: [MarqueeWritten]);
                if (!ScreensaverPackInit.SaversConfig.MarqueeUseConsoleAPI)
                    MarqueeWritten += $"{ConsoleClearing.GetClearLineToRightSequence()}";

                // Set the appropriate cursor position and write the results
                ConsoleWrapper.SetCursorPosition(CurrentLeft, TopPrinted);
                ConsoleWrapper.Write(MarqueeWritten);
                if (Middle)
                    CurrentCharacterNum += 1;

                // If we're not on the left, decrement the current left position
                if (CurrentLeft != 0)
                {
                    CurrentLeft -= 1;
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Not on left. Decremented left position {0}", vars: [CurrentLeft]);
                }

                // If we're on the left or the entire text is written, decrement the current left other end position
                if (!Middle)
                {
                    CurrentLeftOtherEnd -= 1;
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "On left or entire text written. Decremented left other end position {0}", vars: [CurrentLeftOtherEnd]);
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.MarqueeDelay);
        }

    }
}

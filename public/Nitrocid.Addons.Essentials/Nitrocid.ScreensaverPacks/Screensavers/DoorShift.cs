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

using Nitrocid.Drivers.RNG;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Misc.Screensaver;
using Nitrocid.Kernel.Configuration;
using Terminaux.Base;
using Terminaux.Colors;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for DoorShift
    /// </summary>
    public class DoorShiftDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "DoorShift";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Variable preparations
            ConsoleColoring.LoadBackDry(new Color(ScreensaverPackInit.SaversConfig.DoorShiftBackgroundColor));
            ConsoleWrapper.CursorVisible = false;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Whether the door is closing or opening
            bool isClosing = RandomDriver.RandomChance(30);

            // Select a color
            if (ScreensaverPackInit.SaversConfig.DoorShiftTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DoorShiftMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.DoorShiftMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DoorShiftMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.DoorShiftMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DoorShiftMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.DoorShiftMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                if (!ConsoleResizeHandler.WasResized(false))
                    ConsoleColoring.SetConsoleColorDry(new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}"), true);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.DoorShiftMinimumColorLevel, ScreensaverPackInit.SaversConfig.DoorShiftMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                if (!ConsoleResizeHandler.WasResized(false))
                    ConsoleColoring.SetConsoleColorDry(new Color(ColorNum), true);
            }

            // Set max height and width
            int MaxWindowHeight = ConsoleWrapper.WindowHeight - 1;
            int halfWidth = ConsoleWrapper.WindowWidth / 2;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Max height {0}", vars: [MaxWindowHeight]);
            if (isClosing)
            {
                for (int column = 0; column <= halfWidth; column++)
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;
                    for (int Row = 0; Row <= MaxWindowHeight; Row++)
                    {
                        if (ConsoleResizeHandler.WasResized(false))
                            break;

                        // Check the positions
                        int leftDoorPos = column;
                        int rightDoorPos = ConsoleWrapper.WindowWidth - column - 1;
                        if (leftDoorPos > halfWidth)
                            leftDoorPos = halfWidth;
                        if (rightDoorPos < halfWidth)
                            rightDoorPos = halfWidth;

                        // Do the actual writing
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Setting position to {0}", vars: [column - 1, Row]);
                        ConsoleWrapper.SetCursorPosition(leftDoorPos, Row);
                        ConsoleWrapper.Write(" ");
                        ConsoleWrapper.SetCursorPosition(rightDoorPos, Row);
                        ConsoleWrapper.Write(" ");
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", vars: [ConsoleWrapper.WindowWidth - column + 1]);
                    }
                    ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.DoorShiftDelay);
                }
            }
            else
            {
                for (int column = 0; column <= halfWidth; column++)
                {
                    if (ConsoleResizeHandler.WasResized(false))
                        break;
                    for (int Row = 0; Row <= MaxWindowHeight; Row++)
                    {
                        if (ConsoleResizeHandler.WasResized(false))
                            break;

                        // Check the positions
                        int leftDoorPos = halfWidth - column;
                        int rightDoorPos = halfWidth + column;
                        if (leftDoorPos < 0)
                            leftDoorPos = 0;
                        if (rightDoorPos >= ConsoleWrapper.WindowWidth)
                            rightDoorPos = ConsoleWrapper.WindowWidth - 1;

                        // Do the actual writing
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Setting position to {0}", vars: [column - 1, Row]);
                        ConsoleWrapper.SetCursorPosition(leftDoorPos, Row);
                        ConsoleWrapper.Write(" ");
                        ConsoleWrapper.SetCursorPosition(rightDoorPos, Row);
                        ConsoleWrapper.Write(" ");
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Written blanks {0} times", vars: [ConsoleWrapper.WindowWidth - column + 1]);
                    }
                    ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.DoorShiftDelay);
                }
            }

            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.DoorShiftDelay);
        }

    }
}

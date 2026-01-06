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

using System;
using Terminaux.Colors;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Drivers.RNG;
using Terminaux.Base;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Writer.CyclicWriters.Graphical;
using System.Text;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Indeterminate
    /// </summary>
    public class IndeterminateDisplay : BaseScreensaver, IScreensaver
    {

        private readonly int RampFrameBlockStartWidth = 5;
        private readonly int RampFrameBlockWidth = 3;
        private int IndeterminateCurrentBlockStart;
        private int IndeterminateCurrentBlockEnd;
        private IndeterminateDirection IndeterminateCurrentBlockDirection = IndeterminateDirection.LeftToRight;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Indeterminate";

        /// <inheritdoc/>
        public IndeterminateDisplay()
        {
            IndeterminateCurrentBlockStart = RampFrameBlockStartWidth;
            IndeterminateCurrentBlockEnd = IndeterminateCurrentBlockStart + RampFrameBlockWidth;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.IndeterminateMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.IndeterminateMaximumRedColorLevel);
            int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.IndeterminateMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.IndeterminateMaximumGreenColorLevel);
            int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.IndeterminateMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.IndeterminateMaximumBlueColorLevel);
            int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.IndeterminateMinimumColorLevel, ScreensaverPackInit.SaversConfig.IndeterminateMaximumColorLevel);

            // Console resizing can sometimes cause the cursor to remain visible. This happens on Windows 10's terminal.
            ConsoleWrapper.CursorVisible = false;

            // Set start and end widths for the ramp frame
            int RampFrameStartWidth = 4;
            int RampFrameEndWidth = ConsoleWrapper.WindowWidth - RampFrameStartWidth;
            int RampFrameSpaces = RampFrameEndWidth - RampFrameStartWidth;
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Start width: {0}, End width: {1}, Spaces: {2}", vars: [RampFrameStartWidth, RampFrameEndWidth, RampFrameSpaces]);

            // Let the ramp be printed in the center
            int RampCenterPosition = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d);
            DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Center position: {0}", vars: [RampCenterPosition]);

            // Draw the frame
            if (!ConsoleResizeHandler.WasResized(false))
            {
                var border = new Border()
                {
                    Left = RampFrameStartWidth,
                    Top = RampCenterPosition - 2,
                    Width = RampFrameSpaces,
                    Height = 3,
                    Color =
                        ScreensaverPackInit.SaversConfig.IndeterminateUseBorderColors ?
                        new Color(ScreensaverPackInit.SaversConfig.IndeterminateLeftFrameColor) :
                        ColorTools.GetGray(),
                };
                TextWriterRaw.WriteRaw(border.Render());
            }

            // Draw the ramp
            int RampFrameBlockEndWidth = RampFrameEndWidth;
            Color RampCurrentColorInstance;
            if (ScreensaverPackInit.SaversConfig.IndeterminateTrueColor)
                // Set the current colors
                RampCurrentColorInstance = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            else
                // Set the current colors
                RampCurrentColorInstance = new Color(ColorNum);

            // Fill the ramp!
            while (
                (IndeterminateCurrentBlockEnd != RampFrameBlockEndWidth && IndeterminateCurrentBlockDirection == IndeterminateDirection.LeftToRight) ||
                (IndeterminateCurrentBlockStart != RampFrameBlockStartWidth && IndeterminateCurrentBlockDirection == IndeterminateDirection.RightToLeft)
            )
            {
                if (ConsoleResizeHandler.WasResized(false))
                    break;
                if (ScreensaverManager.Bailing)
                    return;
                var rampBuffer = new StringBuilder();

                // Clear the ramp
                if (IndeterminateCurrentBlockDirection == IndeterminateDirection.LeftToRight)
                {
                    int start = IndeterminateCurrentBlockStart == RampFrameStartWidth + 1 ? IndeterminateCurrentBlockStart : IndeterminateCurrentBlockStart - 1;
                    for (int BlockPos = start; BlockPos <= IndeterminateCurrentBlockEnd; BlockPos++)
                    {
                        rampBuffer.Append(
                            ConsolePositioning.RenderChangePosition(BlockPos, RampCenterPosition - 1) +
                            " " +
                            ConsolePositioning.RenderChangePosition(BlockPos, RampCenterPosition) +
                            " " +
                            ConsolePositioning.RenderChangePosition(BlockPos, RampCenterPosition + 1) +
                            " "
                        );
                    }
                }
                else
                {
                    int end = IndeterminateCurrentBlockEnd == RampFrameEndWidth ? IndeterminateCurrentBlockEnd : IndeterminateCurrentBlockEnd + 1;
                    for (int BlockPos = IndeterminateCurrentBlockStart; BlockPos <= end; BlockPos++)
                    {
                        rampBuffer.Append(
                            ConsolePositioning.RenderChangePosition(BlockPos, RampCenterPosition - 1) +
                            " " +
                            ConsolePositioning.RenderChangePosition(BlockPos, RampCenterPosition) +
                            " " +
                            ConsolePositioning.RenderChangePosition(BlockPos, RampCenterPosition + 1) +
                            " "
                        );
                    }
                }

                // Fill the ramp
                rampBuffer.Append(ColorTools.RenderSetConsoleColor(RampCurrentColorInstance, true));
                for (int BlockPos = IndeterminateCurrentBlockStart; BlockPos <= IndeterminateCurrentBlockEnd; BlockPos++)
                {
                    rampBuffer.Append(
                        ConsolePositioning.RenderChangePosition(BlockPos, RampCenterPosition - 1) +
                        " " +
                        ConsolePositioning.RenderChangePosition(BlockPos, RampCenterPosition) +
                        " " +
                        ConsolePositioning.RenderChangePosition(BlockPos, RampCenterPosition + 1) +
                        " "
                    );
                }
                TextWriterRaw.WriteRaw(rampBuffer.ToString());

                // Change the start and end positions
                switch (IndeterminateCurrentBlockDirection)
                {
                    case IndeterminateDirection.LeftToRight:
                        IndeterminateCurrentBlockStart += 1;
                        IndeterminateCurrentBlockEnd += 1;
                        break;
                    case IndeterminateDirection.RightToLeft:
                        IndeterminateCurrentBlockStart -= 1;
                        IndeterminateCurrentBlockEnd -= 1;
                        break;
                }

                // Delay writing
                ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.IndeterminateDelay);
            }

            // Change the direction enumeration
            switch (IndeterminateCurrentBlockDirection)
            {
                case IndeterminateDirection.LeftToRight:
                    IndeterminateCurrentBlockDirection = IndeterminateDirection.RightToLeft;
                    break;
                case IndeterminateDirection.RightToLeft:
                    IndeterminateCurrentBlockDirection = IndeterminateDirection.LeftToRight;
                    break;
            }

            // Reset the background
            ThemeColorsTools.LoadBackground();
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.IndeterminateDelay);
        }

        private enum IndeterminateDirection
        {
            LeftToRight,
            RightToLeft
        }

    }
}

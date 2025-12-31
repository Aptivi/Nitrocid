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
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display for WindowsLogo
    /// </summary>
    public class WindowsLogoDisplay : BaseScreensaver, IScreensaver
    {

        private bool Drawn;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "WindowsLogo";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            if (ConsoleResizeHandler.WasResized(false))
            {
                Drawn = false;

                // Reset resize sync
                ConsoleResizeHandler.WasResized();
            }
            else
            {
                // Get the required positions for the four boxes
                int UpperLeftBoxEndX = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - 1d);
                int UpperLeftBoxStartX = (int)Math.Round(UpperLeftBoxEndX / 2d);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Upper left box X position {0} -> {1}", vars: [UpperLeftBoxStartX, UpperLeftBoxEndX]);

                int UpperLeftBoxStartY = 2;
                int UpperLeftBoxEndY = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d - 1d);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Upper left box Y position {0} -> {1}", vars: [UpperLeftBoxStartY, UpperLeftBoxEndY]);

                int LowerLeftBoxEndX = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d - 1d);
                int LowerLeftBoxStartX = (int)Math.Round(LowerLeftBoxEndX / 2d);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Lower left box X position {0} -> {1}", vars: [LowerLeftBoxStartX, LowerLeftBoxEndX]);

                int LowerLeftBoxStartY = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d + 1d);
                int LowerLeftBoxEndY = ConsoleWrapper.WindowHeight - 2;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Lower left box X position {0} -> {1}", vars: [LowerLeftBoxStartX, LowerLeftBoxEndX]);

                int UpperRightBoxStartX = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d + 2d);
                int UpperRightBoxEndX = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d + UpperRightBoxStartX / 2d);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Upper right box X position {0} -> {1}", vars: [UpperRightBoxStartX, UpperRightBoxEndX]);

                int UpperRightBoxStartY = 2;
                int UpperRightBoxEndY = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d - 1d);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Upper right box X position {0} -> {1}", vars: [UpperRightBoxStartX, UpperRightBoxEndX]);

                int LowerRightBoxStartX = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d + 2d);
                int LowerRightBoxEndX = (int)Math.Round(ConsoleWrapper.WindowWidth / 2d + LowerRightBoxStartX / 2d);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Lower right box X position {0} -> {1}", vars: [LowerRightBoxStartX, LowerRightBoxEndX]);

                int LowerRightBoxStartY = (int)Math.Round(ConsoleWrapper.WindowHeight / 2d + 1d);
                int LowerRightBoxEndY = ConsoleWrapper.WindowHeight - 2;
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Lower right box X position {0} -> {1}", vars: [LowerRightBoxStartX, LowerRightBoxEndX]);

                // Draw the Windows 11 logo
                if (!Drawn)
                {
                    ColorTools.LoadBackDry(new Color(ConsoleColors.Black));
                    var windows11Color = new Color($"0;120;212");

                    // First, draw the upper left box
                    var upperLeftBox = new Box()
                    {
                        Left = UpperLeftBoxStartX,
                        Top = UpperLeftBoxStartY,
                        Width = UpperLeftBoxEndX - UpperLeftBoxStartX,
                        Height = UpperLeftBoxEndY - UpperLeftBoxStartY,
                        Color = windows11Color,
                    };
                    TextWriterRaw.WriteRaw(upperLeftBox.Render());

                    // Second, draw the lower left box
                    var lowerLeftBox = new Box()
                    {
                        Left = LowerLeftBoxStartX,
                        Top = LowerLeftBoxStartY,
                        Width = LowerLeftBoxEndX - LowerLeftBoxStartX,
                        Height = LowerLeftBoxEndY - LowerLeftBoxStartY,
                        Color = windows11Color,
                    };
                    TextWriterRaw.WriteRaw(lowerLeftBox.Render());

                    // Third, draw the upper right box
                    var upperRightBox = new Box()
                    {
                        Left = UpperRightBoxStartX,
                        Top = UpperRightBoxStartY,
                        Width = UpperRightBoxEndX - UpperRightBoxStartX,
                        Height = UpperRightBoxEndY - UpperRightBoxStartY,
                        Color = windows11Color,
                    };
                    TextWriterRaw.WriteRaw(upperRightBox.Render());

                    // Fourth, draw the lower right box
                    var lowerRightBox = new Box()
                    {
                        Left = LowerRightBoxStartX,
                        Top = LowerRightBoxStartY,
                        Width = LowerRightBoxEndX - LowerRightBoxStartX,
                        Height = LowerRightBoxEndY - LowerRightBoxStartY,
                        Color = windows11Color,
                    };
                    TextWriterRaw.WriteRaw(lowerRightBox.Render());

                    // Set drawn
                    Drawn = true;
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Drawn!");
                }
            }
            if (Drawn)
                ScreensaverManager.Delay(1000);
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            Drawn = false;
        }

    }
}

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
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Colors.Data;
using Nitrocid.Base.Kernel.Configuration;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.CyclicWriters.Graphical;
using Terminaux.Writer.CyclicWriters;
using Terminaux.Base.Extensions;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for StackBox
    /// </summary>
    public class StackBoxDisplay : BaseScreensaver, IScreensaver
    {

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "StackBox";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            if (ConsoleResizeHandler.WasResized(false))
            {
                ConsoleColoring.LoadBackDry(new Color(ConsoleColors.Black));
            }
            else
            {
                bool Drawable = true;

                // Get the required positions for the box
                int BoxStartX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                int BoxEndX = RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Box X position {0} -> {1}", vars: [BoxStartX, BoxEndX]);
                int BoxStartY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                int BoxEndY = RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Box Y position {0} -> {1}", vars: [BoxStartY, BoxEndY]);

                // Check to see if start is less than or equal to end
                BoxStartX.SwapIfSourceLarger(ref BoxEndX);
                BoxStartY.SwapIfSourceLarger(ref BoxEndY);
                if (BoxStartX == BoxEndX | BoxStartY == BoxEndY)
                {
                    // Don't draw; it won't be shown anyways
                    DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Asking StackBox not to draw. Consult above two lines.");
                    Drawable = false;
                }

                if (Drawable)
                {
                    Color color;

                    // Select color
                    if (ScreensaverPackInit.SaversConfig.StackBoxTrueColor)
                    {
                        int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.StackBoxMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.StackBoxMaximumRedColorLevel);
                        int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.StackBoxMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.StackBoxMaximumGreenColorLevel);
                        int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.StackBoxMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.StackBoxMaximumBlueColorLevel);
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                        color = new Color($"{RedColorNum};{GreenColorNum};{BlueColorNum}");
                    }
                    else
                    {
                        int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.StackBoxMinimumColorLevel, ScreensaverPackInit.SaversConfig.StackBoxMaximumColorLevel);
                        DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                        color = new Color(ColorNum);
                    }

                    // Draw the box
                    GraphicalCyclicWriter stackBox;
                    if (ScreensaverPackInit.SaversConfig.StackBoxFill)
                    {
                        stackBox = new Box()
                        {
                            Left = BoxStartX,
                            Top = BoxStartY,
                            Width = BoxEndX - BoxStartX,
                            Height = BoxEndY - BoxStartY,
                            Color = color,
                        };
                    }
                    else
                    {
                        stackBox = new BoxFrame()
                        {
                            Left = BoxStartX,
                            Top = BoxStartY,
                            Width = BoxEndX - BoxStartX,
                            Height = BoxEndY - BoxStartY,
                            FrameColor = color,
                        };
                    }
                    TextWriterRaw.WriteRaw(stackBox.Render());
                }
            }
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.StackBoxDelay);
        }

    }
}

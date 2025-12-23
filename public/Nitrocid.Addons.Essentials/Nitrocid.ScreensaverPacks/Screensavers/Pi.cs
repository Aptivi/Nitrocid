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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Drivers.RNG;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Kernel.Configuration;
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Graphical.Shapes;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Pi
    /// </summary>
    public class PiDisplay : BaseScreensaver, IScreensaver
    {
        // TODO: NKS_SCREENSAVERPACKS_PI_SETTINGS_DESC -> "Shows a circle arc that fills itself in the middle of the console"
        private static int currentStartAngle = 360;
        private static int currentEndAngle = 0;
        private static bool reverting = true;
        private static Color colorStorage = Color.Empty;
        private static Arc clearArc = new(0, 0, 0);

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Pi";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Reset values
            currentStartAngle = 360;
            currentEndAngle = 0;
            reverting = true;
            clearArc = new(0, 0, 0, shapeColor: Color.Empty);

            // Select colors
            if (ScreensaverPackInit.SaversConfig.PiTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.PiMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.PiMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.PiMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.PiMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.PiMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.PiMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                colorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.PiMinimumColorLevel, ScreensaverPackInit.SaversConfig.PiMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                colorStorage = new Color(ColorNum);
            }
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            var piBuffer = new StringBuilder();
            
            // Draw the arc based on start and end angles that were changed
            int piLeft = (ConsoleWrapper.WindowWidth / 2) - ConsoleWrapper.WindowHeight + 4;
            var arc = new Arc(ConsoleWrapper.WindowHeight - 4, piLeft, 2, shapeColor: colorStorage)
            {
                AngleStart = currentStartAngle,
                AngleEnd = currentEndAngle,
                OuterRadius = (ConsoleWrapper.WindowHeight / 2) - 4,
                InnerRadius = (ConsoleWrapper.WindowHeight / 2) - 6,
            };
            
            // Initialize the "eraser" arc to reduce flicker
            if (clearArc.Width != 0)
                piBuffer.Append(clearArc.Render());
            clearArc = new(arc.Height, arc.Left, arc.Top, ThemeColorsTools.GetColor(ThemeColorType.Background))
            {
                AngleStart = arc.AngleStart,
                AngleEnd = arc.AngleEnd,
                OuterRadius = arc.OuterRadius,
                InnerRadius = arc.InnerRadius,
            };

            // Render the arc
            piBuffer.Append(arc.Render());
            TextWriterRaw.WriteRaw(piBuffer.ToString());

            // Change the angle
            if (reverting)
            {
                currentEndAngle += 10;
                if (currentEndAngle == 360)
                {
                    reverting = false;
                    currentEndAngle = 0;
                    currentStartAngle = 0;
                }
            }
            else
            {
                currentStartAngle += 10;
                if (currentStartAngle == 360)
                {
                    reverting = true;
                    currentStartAngle = 360;
                }
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.PiDelay);
        }

    }
}

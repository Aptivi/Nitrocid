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
using System.Text;
using Terminaux.Colors;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Base;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Graphical.Shapes;
using Terminaux.Colors.Transformation;
using Terminaux.Base.Structures;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for ColorSpread
    /// </summary>
    public class ColorSpreadDisplay : BaseScreensaver, IScreensaver
    {
        // TODO: NKS_SCREENSAVERPACKS_COLORSPREAD_SETTINGS_DESC -> "Makes a spot somewhere in the screen and spreads it to the whole console (circle radius)"
        private static int currentOuterRadius = 0;
        private static Coordinate coords = new();
        private static Color colorStorage = Color.Empty;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "ColorSpread";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Reset values
            currentOuterRadius = 0;
            coords = new(RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth / 2), RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight / 2));

            // Select colors
            ResetColor();
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            var colorspreadBuffer = new StringBuilder();
            
            // Draw the arc based on start and end angles that were changed
            var arc = new Arc(ConsoleWrapper.WindowHeight, 0, 0, shapeColor: colorStorage)
            {
                AngleStart = 0,
                AngleEnd = 0,
                OuterRadius = currentOuterRadius,
                CenterPosX = coords.X,
                CenterPosY = coords.Y,
                Width = ConsoleWrapper.WindowWidth / 2 - 1,
                Height = ConsoleWrapper.WindowHeight,
            };

            // Render the arc
            colorspreadBuffer.Append(arc.Render());
            TextWriterRaw.WriteRaw(colorspreadBuffer.ToString());

            // Change the radius
            currentOuterRadius++;
            if (currentOuterRadius == ConsoleWrapper.WindowHeight + 5)
            {
                currentOuterRadius = 0;
                ResetColor();
                coords = new(RandomDriver.RandomIdx(ConsoleWrapper.WindowWidth / 2), RandomDriver.RandomIdx(ConsoleWrapper.WindowHeight / 2));
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.ColorSpreadDelay);
        }

        private static void ResetColor()
        {
            if (ScreensaverPackInit.SaversConfig.ColorSpreadTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ColorSpreadMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.ColorSpreadMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ColorSpreadMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.ColorSpreadMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ColorSpreadMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.ColorSpreadMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                colorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.ColorSpreadMinimumColorLevel, ScreensaverPackInit.SaversConfig.ColorSpreadMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                colorStorage = new Color(ColorNum);
            }
        }
    }
}

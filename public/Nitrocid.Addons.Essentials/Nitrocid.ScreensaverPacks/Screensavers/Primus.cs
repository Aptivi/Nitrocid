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
using Terminaux.Colors.Transformation;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Primus
    /// </summary>
    public class PrimusDisplay : BaseScreensaver, IScreensaver
    {
        // TODO: NKS_SCREENSAVERPACKS_PRIMUS_SETTINGS_DESC -> "Shows a growing circle that fills the whole screen before another circle grows over it"
        private static int currentOuterRadius = 0;
        private static int currentInnerRadius = 0;
        private static Color colorStorage = Color.Empty;
        private static Color backgroundColor = Color.Empty;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Primus";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Reset values
            currentOuterRadius = currentInnerRadius = 0;
            backgroundColor = ThemeColorsTools.GetColor(ThemeColorType.Background);

            // Select colors
            ResetColor();
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            var primusBuffer = new StringBuilder();
            
            // Draw the arc based on start and end angles that were changed
            var arc = new Arc(ConsoleWrapper.WindowHeight, 0, 0, shapeColor: colorStorage)
            {
                AngleStart = 0,
                AngleEnd = 0,
                OuterRadius = currentOuterRadius,
                Width = ConsoleWrapper.WindowWidth / 2,
                Height = ConsoleWrapper.WindowHeight,
            };

            // Render the arc
            primusBuffer.Append(arc.Render());
            TextWriterRaw.WriteRaw(primusBuffer.ToString());

            // Change the radius
            currentOuterRadius++;
            if (currentOuterRadius == ConsoleWrapper.WindowHeight + 5)
            {
                currentOuterRadius = 0;
                ResetColor();
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.PrimusDelay);
        }

        private static void ResetColor()
        {
            if (ScreensaverPackInit.SaversConfig.PrimusTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.PrimusMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.PrimusMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.PrimusMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.PrimusMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.PrimusMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.PrimusMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                colorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.PrimusMinimumColorLevel, ScreensaverPackInit.SaversConfig.PrimusMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                colorStorage = new Color(ColorNum);
            }
        }
    }
}

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

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for BackImplosion
    /// </summary>
    public class BackImplosionDisplay : BaseScreensaver, IScreensaver
    {
        // TODO: NKS_SCREENSAVERPACKS_BACKIMPLOSION_SETTINGS_DESC -> "Simulates the colored background fading out to black in circle starting from the middle"
        private static int currentOuterRadius = 0;
        private static Color colorStorage = Color.Empty;

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "BackImplosion";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Reset values
            currentOuterRadius = 0;

            // Select colors
            ResetColor();
            base.ScreensaverPreparation();
            ColorTools.LoadBackDry(colorStorage);
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;
            var backimplosionBuffer = new StringBuilder();
            
            // Draw the arc based on start and end angles that were changed
            var arc = new Arc(ConsoleWrapper.WindowHeight, 0, 0, shapeColor: ThemeColorsTools.GetColor(ThemeColorType.Background))
            {
                AngleStart = 0,
                AngleEnd = 0,
                OuterRadius = currentOuterRadius,
                Width = ConsoleWrapper.WindowWidth / 2,
                Height = ConsoleWrapper.WindowHeight,
            };

            // Render the arc
            backimplosionBuffer.Append(arc.Render());
            TextWriterRaw.WriteRaw(backimplosionBuffer.ToString());

            // Change the radius
            currentOuterRadius++;
            if (currentOuterRadius == ConsoleWrapper.WindowHeight + 5)
            {
                currentOuterRadius = 0;
                ResetColor();
                ColorTools.LoadBackDry(colorStorage);
            }

            // Reset resize sync
            ConsoleResizeHandler.WasResized();
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.BackImplosionDelay);
        }

        private static void ResetColor()
        {
            if (ScreensaverPackInit.SaversConfig.BackImplosionTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BackImplosionMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.BackImplosionMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BackImplosionMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.BackImplosionMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BackImplosionMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.BackImplosionMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                colorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.BackImplosionMinimumColorLevel, ScreensaverPackInit.SaversConfig.BackImplosionMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                colorStorage = new Color(ColorNum);
            }
        }
    }
}

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
using Terminaux.Base;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.CyclicWriters.Graphical.Shapes;
using Terminaux.Colors.Transformation;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Display code for Ripple
    /// </summary>
    public class RippleDisplay : BaseScreensaver, IScreensaver
    {
        // TODO: NKS_SCREENSAVERPACKS_RIPPLE_SETTINGS_DESC -> "Shows a live ripple effect from the middle"
        private static int currentOuterRadius = 0;
        private static int currentInnerRadius = 0;
        private static int currentOuterRadius2 = -6;
        private static int currentInnerRadius2 = -6;
        private static Color colorStorage = Color.Empty;
        private static Arc clearArc = new(0, 0, 0);
        private static Arc clearArc2 = new(0, 0, 0);

        /// <inheritdoc/>
        public override string ScreensaverName =>
            "Ripple";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            // Reset values
            currentOuterRadius = currentInnerRadius = 0;
            currentOuterRadius2 = currentInnerRadius2 = -6;
            clearArc = clearArc2 = new(0, 0, 0, shapeColor: Color.Empty);

            // Select colors
            if (ScreensaverPackInit.SaversConfig.RippleTrueColor)
            {
                int RedColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.RippleMinimumRedColorLevel, ScreensaverPackInit.SaversConfig.RippleMaximumRedColorLevel);
                int GreenColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.RippleMinimumGreenColorLevel, ScreensaverPackInit.SaversConfig.RippleMaximumGreenColorLevel);
                int BlueColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.RippleMinimumBlueColorLevel, ScreensaverPackInit.SaversConfig.RippleMaximumBlueColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", vars: [RedColorNum, GreenColorNum, BlueColorNum]);
                colorStorage = new Color(RedColorNum, GreenColorNum, BlueColorNum);
            }
            else
            {
                int ColorNum = RandomDriver.Random(ScreensaverPackInit.SaversConfig.RippleMinimumColorLevel, ScreensaverPackInit.SaversConfig.RippleMaximumColorLevel);
                DebugWriter.WriteDebugConditional(Config.MainConfig.ScreensaverDebug, DebugLevel.I, "Got color ({0})", vars: [ColorNum]);
                colorStorage = new Color(ColorNum);
            }
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            var rippleBuffer = new StringBuilder();
            
            // Draw the arc based on start and end angles that were changed
            int rippleLeft = (ConsoleWrapper.WindowWidth / 2) - ConsoleWrapper.WindowHeight + 4;
            var arc = new Arc(ConsoleWrapper.WindowHeight - 4, rippleLeft, 2, shapeColor: colorStorage)
            {
                AngleStart = 0,
                AngleEnd = 0,
                OuterRadius = currentOuterRadius,
                InnerRadius = currentInnerRadius,
            };
            var arc2 = new Arc(ConsoleWrapper.WindowHeight - 4, rippleLeft, 2, shapeColor: TransformationTools.GetDarkBackground(colorStorage))
            {
                AngleStart = 0,
                AngleEnd = 0,
                OuterRadius = currentOuterRadius2,
                InnerRadius = currentInnerRadius2,
            };

            // Initialize the "eraser" arc to reduce flicker
            if (clearArc.Width != 0)
                rippleBuffer.Append(clearArc.Render());
            clearArc = new(arc.Height, arc.Left, arc.Top, ThemeColorsTools.GetColor(ThemeColorType.Background))
            {
                AngleStart = arc.AngleStart,
                AngleEnd = arc.AngleEnd,
                OuterRadius = arc.OuterRadius,
                InnerRadius = arc.InnerRadius,
            };
            if (currentOuterRadius2 >= 0 && currentInnerRadius2 >= 0)
            {
                if (clearArc2.Width != 0)
                    rippleBuffer.Append(clearArc2.Render());
                clearArc2 = new(arc2.Height, arc2.Left, arc2.Top, ThemeColorsTools.GetColor(ThemeColorType.Background))
                {
                    AngleStart = arc2.AngleStart,
                    AngleEnd = arc2.AngleEnd,
                    OuterRadius = arc2.OuterRadius,
                    InnerRadius = arc2.InnerRadius,
                };
            }

            // Render the arc
            rippleBuffer.Append(arc.Render());
            if (currentOuterRadius2 >= 0 && currentInnerRadius2 >= 0)
                rippleBuffer.Append(arc2.Render());
            TextWriterRaw.WriteRaw(rippleBuffer.ToString());

            // Change the radius
            ChangeRadius(ref currentOuterRadius, ref currentInnerRadius);
            ChangeRadius(ref currentOuterRadius2, ref currentInnerRadius2);
            ScreensaverManager.Delay(ScreensaverPackInit.SaversConfig.RippleDelay);
        }

        private static void ChangeRadius(ref int outerRadius, ref int innerRadius)
        {
            if (outerRadius == (ConsoleWrapper.WindowHeight / 2) - 4)
            {
                innerRadius++;
                if (innerRadius >= (ConsoleWrapper.WindowHeight / 2))
                    innerRadius = outerRadius = 0;
            }
            else if (innerRadius == 0)
            {
                outerRadius++;
                if (outerRadius == 3)
                    innerRadius++;
            }
            else
            {
                outerRadius++;
                innerRadius++;
            }
        }
    }
}

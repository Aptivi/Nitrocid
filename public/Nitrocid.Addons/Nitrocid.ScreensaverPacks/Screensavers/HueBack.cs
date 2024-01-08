//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.Kernel.Threading;
using Nitrocid.Misc.Screensaver;
using Terminaux.Base;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for HueBack
    /// </summary>
    public static class HueBackSettings
    {

        /// <summary>
        /// [HueBack] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int HueBackDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.HueBackDelay;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                ScreensaverPackInit.SaversConfig.HueBackDelay = value;
            }
        }
        /// <summary>
        /// [HueBack] How intense is the color?
        /// </summary>
        public static int HueBackSaturation
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.HueBackSaturation;
            }
            set
            {
                if (value <= 0)
                    value = 100;
                if (value > 100)
                    value = 100;
                ScreensaverPackInit.SaversConfig.HueBackSaturation = value;
            }
        }
        /// <summary>
        /// [HueBack] How light is the color?
        /// </summary>
        public static int HueBackLuminance
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.HueBackLuminance;
            }
            set
            {
                if (value <= 0)
                    value = 50;
                if (value > 100)
                    value = 100;
                ScreensaverPackInit.SaversConfig.HueBackLuminance = value;
            }
        }

    }

    /// <summary>
    /// Display code for HueBack
    /// </summary>
    public class HueBackDisplay : BaseScreensaver, IScreensaver
    {

        // Hue angle is in degrees and not radians
        private static int currentHueAngle = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "HueBack";

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            ConsoleWrapper.CursorVisible = false;

            // Prepare the color
            var color = new Color($"hsl:{currentHueAngle};{HueBackSettings.HueBackSaturation};{HueBackSettings.HueBackLuminance}");

            // Now, change the background color accordingly
            ColorTools.LoadBack(color);
            ThreadManager.SleepNoBlock(HueBackSettings.HueBackDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
            currentHueAngle++;
            if (currentHueAngle > 360)
                currentHueAngle = 0;

            // Reset resize sync
            ConsoleResizeListener.WasResized();
        }

        /// <inheritdoc/>
        public override void ScreensaverOutro()
        {
            currentHueAngle = 0;
        }

    }
}

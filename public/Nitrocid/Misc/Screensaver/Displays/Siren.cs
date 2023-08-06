
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.Kernel.Configuration;
using KS.Kernel.Threading;
using Terminaux.Colors;

namespace KS.Misc.Screensaver.Displays
{
    /// <summary>
    /// Settings for Siren
    /// </summary>
    public static class SirenSettings
    {

        /// <summary>
        /// [Siren] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SirenDelay
        {
            get
            {
                return Config.SaverConfig.SirenDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                Config.SaverConfig.SirenDelay = value;
            }
        }

        /// <summary>
        /// [Siren] The siren style
        /// </summary>
        public static string SirenStyle
        {
            get
            {
                return Config.SaverConfig.SirenStyle;
            }
            set
            {
                Config.SaverConfig.SirenStyle = SirenDisplay.sirens.ContainsKey(value) ? value : "Cop";
            }
        }

    }

    /// <summary>
    /// Display code for Siren
    /// </summary>
    public class SirenDisplay : BaseScreensaver, IScreensaver
    {

        internal readonly static Dictionary<string, Color[]> sirens = new()
        {
            { "Cop", new Color[] { new(255, 0, 0), new(0, 0, 255) } },
            { "Ambulance", new Color[] { new(255, 0, 0), new(128, 0, 0) } }
        };
        private int step = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Siren";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages { get; set; } = true;

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get step color array from siren type
            Color[] sirenColors = sirens[SirenSettings.SirenStyle];

            // Step through the color
            step += 1;
            if (step >= sirenColors.Length)
                step = 0;

            // Set color
            KernelColorTools.LoadBack(sirenColors[step], true);

            // Delay
            ThreadManager.SleepNoBlock(SirenSettings.SirenDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}

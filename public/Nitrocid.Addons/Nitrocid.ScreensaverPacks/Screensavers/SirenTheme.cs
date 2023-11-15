//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Themes;
using KS.Kernel.Threading;
using KS.Misc.Screensaver;
using Terminaux.Colors;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for SirenTheme
    /// </summary>
    public static class SirenThemeSettings
    {

        /// <summary>
        /// [SirenTheme] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int SirenThemeDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SirenThemeDelay;
            }
            set
            {
                if (value <= 0)
                    value = 500;
                ScreensaverPackInit.SaversConfig.SirenThemeDelay = value;
            }
        }

        /// <summary>
        /// [SirenTheme] The siren style
        /// </summary>
        public static string SirenThemeStyle
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.SirenThemeStyle;
            }
            set
            {
                ScreensaverPackInit.SaversConfig.SirenThemeStyle = ThemeTools.GetInstalledThemes().ContainsKey(value) ? value : "Default";
            }
        }

    }

    /// <summary>
    /// Display code for SirenTheme
    /// </summary>
    public class SirenThemeDisplay : BaseScreensaver, IScreensaver
    {

        internal readonly static Dictionary<string, Color[]> sirens = [];
        private int step = 0;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "SirenTheme";

        /// <inheritdoc/>
        public override bool ScreensaverContainsFlashingImages { get; set; } = true;

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            base.ScreensaverPreparation();

            // Populate the sirens from all the available kernel themes
            if (sirens.Count > 0)
                return;
            foreach (var theme in ThemeTools.GetInstalledThemes().Keys)
            {
                // Get the colors
                var colors = ThemeTools.GetColorsFromTheme(theme);

                // Enumerate thorugh every type
                List<Color> colorList = [];
                foreach (var color in colors.Values)
                {
                    // Now, compare the colors and add them
                    if (!colorList.Contains(color))
                        colorList.Add(color);
                }

                // Now, add the list with the theme name to the list
                sirens.Add(theme, [.. colorList]);
            }
            step = 0;
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic()
        {
            // Get step color array from siren type
            Color[] sirenColors = sirens[SirenThemeSettings.SirenThemeStyle];

            // Step through the color
            step += 1;
            if (step >= sirenColors.Length)
                step = 0;

            // Set color
            KernelColorTools.LoadBack(sirenColors[step]);

            // Delay
            ThreadManager.SleepNoBlock(SirenThemeSettings.SirenThemeDelay, ScreensaverDisplayer.ScreensaverDisplayerThread);
        }

    }
}

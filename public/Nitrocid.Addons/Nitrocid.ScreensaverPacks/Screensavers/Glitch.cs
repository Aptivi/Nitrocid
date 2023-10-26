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

using KS.ConsoleBase;
using KS.Kernel.Debugging;
using KS.Misc.Screensaver;

namespace Nitrocid.ScreensaverPacks.Screensavers
{
    /// <summary>
    /// Settings for Glitch
    /// </summary>
    public static class GlitchSettings
    {

        /// <summary>
        /// [Glitch] How many milliseconds to wait before making the next write?
        /// </summary>
        public static int GlitchDelay
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GlitchDelay;
            }
            set
            {
                if (value <= 0)
                    value = 10;
                ScreensaverPackInit.SaversConfig.GlitchDelay = value;
            }
        }
        /// <summary>
        /// [Glitch] The Glitch density in percent
        /// </summary>
        public static int GlitchDensity
        {
            get
            {
                return ScreensaverPackInit.SaversConfig.GlitchDensity;
            }
            set
            {
                if (value < 0)
                    value = 40;
                if (value > 100)
                    value = 40;
                ScreensaverPackInit.SaversConfig.GlitchDensity = value;
            }
        }

    }

    /// <summary>
    /// Display code for Glitch
    /// </summary>
    public class GlitchDisplay : BaseScreensaver, IScreensaver
    {

        private Animations.Glitch.GlitchSettings GlitchSettingsInstance;

        /// <inheritdoc/>
        public override string ScreensaverName { get; set; } = "Glitch";

        /// <inheritdoc/>
        public override void ScreensaverPreparation()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Console geometry: {0}x{1}", ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight);
            GlitchSettingsInstance = new Animations.Glitch.GlitchSettings()
            {
                GlitchDelay = GlitchSettings.GlitchDelay,
                GlitchDensity = GlitchSettings.GlitchDensity
            };
            base.ScreensaverPreparation();
        }

        /// <inheritdoc/>
        public override void ScreensaverLogic() =>
            Animations.Glitch.Glitch.Simulate(GlitchSettingsInstance);

    }
}

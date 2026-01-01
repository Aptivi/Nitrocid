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

using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Misc.Splash;
using Nitrocid.SplashPacks.Settings;
using Nitrocid.SplashPacks.Splashes;

namespace Nitrocid.SplashPacks
{
    internal class SplashPackInit : IAddon
    {
        internal readonly static SplashInfo[] Splashes =
        [
            new SplashInfo("Simple", new SplashSimple()),
            new SplashInfo("Progress", new SplashProgress()),
            new SplashInfo("PowerLineProgress", new SplashPowerLineProgress()),
            new SplashInfo("Dots", new SplashDots()),
            new SplashInfo("TextBox", new SplashTextBox()),
            new SplashInfo("FigProgress", new SplashFigProgress()),
            new SplashInfo("Fader", new SplashFader()),
            new SplashInfo("FaderBack", new SplashFaderBack()),
            new SplashInfo("BeatFader", new SplashBeatFader()),
            new SplashInfo("Pulse", new SplashPulse()),
            new SplashInfo("BeatPulse", new SplashBeatPulse()),
            new SplashInfo("EdgePulse", new SplashEdgePulse()),
            new SplashInfo("BeatEdgePulse", new SplashBeatEdgePulse()),
            new SplashInfo("Spin", new SplashSpin()),
            new SplashInfo("SquareCorner", new SplashSquareCorner()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.AddonSplashPacks);

        internal static ExtraSplashesConfig SplashConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(ExtraSplashesConfig)) ? (ExtraSplashesConfig)Config.baseConfigurations[nameof(ExtraSplashesConfig)] : Config.GetFallbackKernelConfig<ExtraSplashesConfig>();

        void IAddon.StartAddon()
        {
            // Close the splash first
            SplashManager.BeginSplashOut();

            // First, initialize splashes
            foreach (var splash in Splashes)
                SplashManager.builtinSplashes.Add(splash);

            // Open the splash
            SplashManager.EndSplashOut();

            // Then, initialize configuration in a way that no mod can play with them
            var splashesConfig = new ExtraSplashesConfig();
            ConfigTools.RegisterBaseSetting(splashesConfig);
        }

        void IAddon.StopAddon()
        {
            // Close the splash first
            SplashManager.BeginSplashOut();

            // First, unload splashes
            foreach (var splash in Splashes)
                SplashManager.builtinSplashes.Remove(splash);

            // Open the splash
            SplashManager.EndSplashOut();

            // Then, unload the configuration
            ConfigTools.UnregisterBaseSetting(nameof(ExtraSplashesConfig));
        }

        void IAddon.FinalizeAddon()
        { }
    }
}

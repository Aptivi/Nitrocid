//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Misc.Splash;
using Nitrocid.Core.Languages;
using Nitrocid.Extras.Animated.Screensavers;
using Nitrocid.Extras.Animated.Settings;
using Nitrocid.Extras.Animated.Splashes;

namespace Nitrocid.Extras.Animated
{
    internal class AnimatedInit : IAddon
    {
        internal static Dictionary<string, BaseScreensaver> Screensavers = new()
        {
            { "beatfader", new BeatFaderDisplay() },
            { "beatpulse", new BeatPulseDisplay() },
            { "beatedgepulse", new BeatEdgePulseDisplay() },
            { "bsod", new BSODDisplay() },
            { "edgepulse", new EdgePulseDisplay() },
            { "excalibeats", new ExcaliBeatsDisplay() },
            { "fader", new FaderDisplay() },
            { "faderback", new FaderBackDisplay() },
            { "glitch", new GlitchDisplay() },
            { "ksx", new KSXDisplay() },
            { "ksx2", new KSX2Display() },
            { "ksx3", new KSX3Display() },
            { "ksxtheend", new KSXTheEndDisplay() },
            { "pulse", new PulseDisplay() },
            { "spin", new SpinDisplay() },
            { "squarecorner", new SquareCornerDisplay() },
            { "textreveal", new TextRevealDisplay() },
        };

        internal readonly static SplashInfo[] Splashes =
        [
            new SplashInfo("Fader", new SplashFader()),
            new SplashInfo("FaderBack", new SplashFaderBack()),
            new SplashInfo("BeatFader", new SplashBeatFader()),
            new SplashInfo("Pulse", new SplashPulse()),
            new SplashInfo("BeatPulse", new SplashBeatPulse()),
            new SplashInfo("EdgePulse", new SplashEdgePulse()),
            new SplashInfo("BeatEdgePulse", new SplashBeatEdgePulse()),
            new SplashInfo("Spin", new SplashSpin()),
            new SplashInfo("SquareCorner", new SplashSquareCorner()),
            new SplashInfo("TextReveal", new SplashTextReveal()),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.AddonScreensaverPacks);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.AddonScreensaverPacks);

        internal static AnimatedSaversConfig SaversConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(AnimatedSaversConfig)) ? (AnimatedSaversConfig)Config.baseConfigurations[nameof(AnimatedSaversConfig)] : Config.GetFallbackKernelConfig<AnimatedSaversConfig>();

        internal static AnimatedSplashesConfig SplashConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(AnimatedSplashesConfig)) ? (AnimatedSplashesConfig)Config.baseConfigurations[nameof(AnimatedSplashesConfig)] : Config.GetFallbackKernelConfig<AnimatedSplashesConfig>();

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.Animated.Resources.Languages.Output.Localizations", typeof(AnimatedInit).Assembly));

            // First, initialize screensavers
            foreach (var saver in Screensavers.Keys)
                ScreensaverManager.AddonSavers.Add(saver, Screensavers[saver]);

            // Next, initialize splashes
            lock (SplashManager.builtinSplashes)
            {
                foreach (var splash in Splashes)
                    SplashManager.builtinSplashes.Add(splash);
            }

            // Then, initialize configuration in a way that no mod can play with them
            var saversConfig = new AnimatedSaversConfig();
            var splashesConfig = new AnimatedSplashesConfig();
            ConfigTools.RegisterBaseSetting(saversConfig);
            ConfigTools.RegisterBaseSetting(splashesConfig);
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);

            // First, unload screensavers
            foreach (var saver in Screensavers.Keys)
                ScreensaverManager.AddonSavers.Remove(saver);

            // Next, unload splashes
            lock (SplashManager.builtinSplashes)
            {
                foreach (var splash in Splashes)
                    SplashManager.builtinSplashes.Remove(splash);
            }

            // Then, unload the configuration
            ConfigTools.UnregisterBaseSetting(nameof(AnimatedSaversConfig));
            ConfigTools.UnregisterBaseSetting(nameof(AnimatedSplashesConfig));
        }
    }
}

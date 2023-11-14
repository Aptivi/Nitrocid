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

using KS.Kernel.Configuration;
using KS.Kernel.Extensions;
using KS.Misc.Splash;
using Nitrocid.SplashPacks.Settings;
using Nitrocid.SplashPacks.Splashes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Nitrocid.SplashPacks
{
    internal class SplashPackInit : IAddon
    {
        internal readonly static Dictionary<string, SplashInfo> Splashes = new()
        {
            { "Simple", new SplashInfo("Simple", new SplashSimple()) },
            { "Progress", new SplashInfo("Progress", new SplashProgress()) },
            { "systemd", new SplashInfo("systemd", new SplashSystemd()) },
            { "sysvinit", new SplashInfo("sysvinit", new SplashSysvinit()) },
            { "openrc", new SplashInfo("openrc", new SplashOpenRC()) },
            { "PowerLine", new SplashInfo("PowerLine", new SplashPowerLine()) },
            { "PowerLineProgress", new SplashInfo("PowerLine", new SplashPowerLineProgress()) },
            { "Dots", new SplashInfo("Dots", new SplashDots()) },
            { "TextBox", new SplashInfo("TextBox", new SplashTextBox()) },
            { "FigProgress", new SplashInfo("FigProgress", new SplashFigProgress()) },
        };

        string IAddon.AddonName => "Extra Splashes Pack";

        AddonType IAddon.AddonType => AddonType.Important;

        internal static ExtraSplashesConfig SplashConfig =>
            (ExtraSplashesConfig)Config.baseConfigurations[nameof(ExtraSplashesConfig)];

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

        void IAddon.StartAddon()
        {
            // First, initialize splashes
            foreach (var splash in Splashes.Keys)
                SplashManager.InstalledSplashes.Add(splash, Splashes[splash]);

            // Then, initialize configuration in a way that no mod can play with them
            var splashesConfig = new ExtraSplashesConfig();
            ConfigTools.RegisterBaseSetting(splashesConfig);
        }

        void IAddon.StopAddon()
        {
            foreach (var splash in Splashes.Keys)
                SplashManager.InstalledSplashes.Remove(splash);

            // Then, unload the configuration
            ConfigTools.UnregisterBaseSetting(nameof(ExtraSplashesConfig));
        }

        void IAddon.FinalizeAddon()
        { }
    }
}

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

using Nitrocid.ConsoleBase.Themes;
using Newtonsoft.Json.Linq;
using Nitrocid.Kernel.Debugging;
using Nitrocid.ThemePacks.Resources;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Misc.Reflection;
using Nitrocid.Modifications;

namespace Nitrocid.ThemePacks
{
    internal class ThemePackInit : IAddon
    {
        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.AddonThemePacks);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

        void IAddon.StartAddon()
        {
            // Add them all!
            string[] themeResNames = GetThemeResourceNames();
            foreach (string key in themeResNames)
            {
                var themeToken = JToken.Parse(ThemesResources.ResourceManager.GetString(key));
                bool result = ThemeTools.themes.TryAdd(key, new ThemeInfo(themeToken));
                DebugWriter.WriteDebug(DebugLevel.I, "Added {0}: {1}", key, result);
            }
        }

        void IAddon.StopAddon()
        {
            // Remove them all!
            string[] themeResNames = GetThemeResourceNames();
            foreach (string key in themeResNames)
            {
                bool result = ThemeTools.themes.Remove(key);
                DebugWriter.WriteDebug(DebugLevel.I, "Removed {0}: {1}", key, result);
            }
        }

        void IAddon.FinalizeAddon()
        { }

        private string[] GetThemeResourceNames()
        {
            // Get all the themes provided by this pack
            string[] nonThemes =
            [
                nameof(ThemesResources.Culture),
                nameof(ThemesResources.ResourceManager)
            ];
            return PropertyManager.GetPropertiesNoEvaluation(typeof(ThemesResources)).Keys.Except(nonThemes).ToArray();
        }
    }
}

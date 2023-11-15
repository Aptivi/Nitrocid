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
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Shells;
using Nitrocid.Extras.JsonShell.Json;
using Nitrocid.Extras.JsonShell.Settings;
using Nitrocid.Extras.JsonShell.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace Nitrocid.Extras.JsonShell
{
    internal class JsonShellInit : IAddon
    {
        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasJsonShell);

        AddonType IAddon.AddonType => AddonType.Optional;

        internal static JsonConfig JsonConfig =>
            (JsonConfig)Config.baseConfigurations[nameof(JsonConfig)];

        ReadOnlyDictionary<string, Delegate> IAddon.PubliclyAvailableFunctions => null;

        ReadOnlyDictionary<string, PropertyInfo> IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo> IAddon.PubliclyAvailableFields => null;

        void IAddon.FinalizeAddon()
        {
            var config = new JsonConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.reservedShells.Add("JsonShell");
            ShellManager.RegisterShell("JsonShell", new JsonShellInfo());
        }

        void IAddon.StartAddon()
        { }

        void IAddon.StopAddon()
        {
            ShellManager.availableShells.Remove("JsonShell");
            PromptPresetManager.CurrentPresets.Remove("JsonShell");
            ShellManager.reservedShells.Remove("JsonShell");
            ConfigTools.UnregisterBaseSetting(nameof(JsonConfig));
        }
    }
}

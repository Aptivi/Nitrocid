//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Extras.Docking.Commands;
using Nitrocid.Extras.Docking.Dock;
using Nitrocid.Shell.ShellBase.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Modifications;
using System.Linq;
using Nitrocid.Users.Login.Widgets;

namespace Nitrocid.Extras.Docking
{
    internal class DockingInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("dock", /* Localizable */ "Shows you a full-screen overview about a selected dock view to be able to use it as an info panel",
                [
                    new CommandArgumentInfo(new[]
                    {
                        new CommandArgumentPart(true, "dockName", new()
                        {
                            AutoCompleter = (_) => DockTools.GetDockScreenNames(),
                            ArgumentDescription = /* Localizable */ "Dock name"
                        }),
                    })
                ], new DockCommand())
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasDocking);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        ReadOnlyDictionary<string, Delegate>? IAddon.PubliclyAvailableFunctions => new(new Dictionary<string, Delegate>()
        {
            { nameof(DockTools.DockScreen), new Action<string>(DockTools.DockScreen) },
            { nameof(DockTools.DockScreen) + "2", new Action<BaseWidget>(DockTools.DockScreen) },
            { nameof(DockTools.DoesDockScreenExist), new Func<string, (bool, BaseWidget?)>((target) =>
                {
                    bool result = DockTools.DoesDockScreenExist(target, out BaseWidget? instance);
                    return (result, instance);
                })
            },
            { nameof(DockTools.GetDockScreenNames), new Func<string[]>(DockTools.GetDockScreenNames) },
            { nameof(DockTools.GetDockScreens), new Func<ReadOnlyDictionary<string, BaseWidget>>(DockTools.GetDockScreens) },
        });

        ReadOnlyDictionary<string, PropertyInfo>? IAddon.PubliclyAvailableProperties => null;

        ReadOnlyDictionary<string, FieldInfo>? IAddon.PubliclyAvailableFields => null;

        void IAddon.FinalizeAddon()
        { }

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
    }
}

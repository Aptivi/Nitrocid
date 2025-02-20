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

using Nitrocid.Extras.Pastebin.Commands;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.ShellBase.Switches;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.Pastebin
{
    internal class PastebinInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("pastebin", /* Localizable */ "Pastes the content of either a file or a string to a text hosting provider",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file/string", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Path to a file or a string to write"
                        }),
                        new CommandArgumentPart(false, "arguments", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "Pastebin arguments"
                        }),
                    ],
                    [
                        new SwitchInfo("provider", /* Localizable */ "Specifies the URL to the Pastebin provider", new()
                        {
                            AcceptsValues = true,
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("type", /* Localizable */ "Specifies the Pastebin provider type", new()
                        {
                            AcceptsValues = true,
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("postpage", /* Localizable */ "Specifies the Pastebin provider post page", new()
                        {
                            AcceptsValues = true,
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("postformat", /* Localizable */ "Specifies the Pastebin provider post format", new()
                        {
                            AcceptsValues = true,
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("postfield", /* Localizable */ "Specifies the Pastebin provider post field name", new()
                        {
                            AcceptsValues = true,
                            ArgumentsRequired = true,
                        }),
                    ]),
                ], new PastebinCommand()),
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasPastebin);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        void IAddon.FinalizeAddon()
        { }

        void IAddon.StartAddon() =>
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);

        void IAddon.StopAddon() =>
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
    }
}

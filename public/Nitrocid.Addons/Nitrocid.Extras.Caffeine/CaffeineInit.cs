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

using Terminaux.Shell.Arguments;
using Terminaux.Shell.Switches;
using Nitrocid.Extras.Caffeine.Commands;
using Nitrocid.Extras.Caffeine.Localized;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using System.Linq;
using Nitrocid.Languages;

namespace Nitrocid.Extras.Caffeine
{
    internal class CaffeineInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("caffeine", /* Localizable */ "NKS_CAFFEINE_COMMAND_CAFFEINE_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "secondsOrName", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_CAFFEINE_COMMAND_CAFFEINE_ARGUMENT_SECSORNAME_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("abort", /* Localizable */ "NKS_CAFFEINE_COMMAND_CAFFEINE_SWITCH_ABORT_DESC", new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 1
                        })
                    ])
                ], new CaffeineCommand())
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasCaffeine);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        void IAddon.StartAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.Caffeine", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        void IAddon.StopAddon()
        {
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.Caffeine");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
        }

        void IAddon.FinalizeAddon()
        { }
    }
}

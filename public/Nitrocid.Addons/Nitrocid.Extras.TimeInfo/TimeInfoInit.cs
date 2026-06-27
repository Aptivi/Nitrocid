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

using Terminaux.Shell.Arguments;
using Terminaux.Shell.Switches;
using Nitrocid.Extras.TimeInfo.Commands;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Shells;
using System.Linq;
using Nitrocid.Languages;

namespace Nitrocid.Extras.TimeInfo
{
    internal class TimeInfoInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("gettimeinfo", LanguageTools.GetLocalized("NKS_DATES_COMMAND_GETTIMEINFO_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "date", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_DATES_COMMAND_GETTIMEINFO_ARGUMENT_DATE_DESC")
                        })
                    ],
                    [
                        new SwitchInfo("now", LanguageTools.GetLocalized("NKS_DATES_COMMAND_GETTIMEINFO_SWITCH_NOW_DESC"), new SwitchOptions()
                        {
                            OptionalizeLastRequiredArguments = 1,
                            AcceptsValues = false
                        })
                    ])
                ], new GetTimeInfoCommand(), CommandFlags.RedirectionSupported | CommandFlags.Wrappable),

            new CommandInfo("expiry", LanguageTools.GetLocalized("NKS_DATES_COMMAND_EXPIRY_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "production", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_DATES_COMMAND_EXPIRY_ARGUMENT_PRODUCTION_DESC")
                        }),
                        new CommandArgumentPart(true, "expiry", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_DATES_COMMAND_EXPIRY_ARGUMENT_EXPIRY_DESC")
                        })
                    ],
                    [
                        new SwitchInfo("implicit", LanguageTools.GetLocalized("NKS_DATES_COMMAND_EXPIRY_STATUS_IMPLICIT_DESC"), new SwitchOptions()
                        {
                            AcceptsValues = false
                        })
                    ])
                ], new ExpiryCommand()),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasTimeInfo);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasTimeInfo);

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.TimeInfo.Resources.Languages.Output.Localizations", typeof(TimeInfoInit).Assembly));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
        }

        public void FinalizeAddon()
        { }
    }
}

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

using Nitrocid.Extras.Pastebin.Commands;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Core.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.Pastebin
{
    internal class PastebinInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("pastebin", /* Localizable */ "NKS_PASTEBIN_COMMAND_PASTEBIN_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file/string", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_PASTEBIN_COMMAND_PASTEBIN_ARGUMENT_FILESTRING_DESC"
                        }),
                        new CommandArgumentPart(false, "arguments", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_PASTEBIN_COMMAND_PASTEBIN_ARGUMENT_ARGUMENTS_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("provider", /* Localizable */ "NKS_PASTEBIN_COMMAND_PASTEBIN_SWITCH_PROVIDER_DESC", new()
                        {
                            AcceptsValues = true,
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("type", /* Localizable */ "NKS_PASTEBIN_COMMAND_PASTEBIN_SWITCH_TYPE_DESC", new()
                        {
                            AcceptsValues = true,
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("postpage", /* Localizable */ "NKS_PASTEBIN_COMMAND_PASTEBIN_SWITCH_POSTPAGE_DESC", new()
                        {
                            AcceptsValues = true,
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("postformat", /* Localizable */ "NKS_PASTEBIN_COMMAND_PASTEBIN_SWITCH_POSTFORMAT_DESC", new()
                        {
                            AcceptsValues = true,
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("postfield", /* Localizable */ "NKS_PASTEBIN_COMMAND_PASTEBIN_SWITCH_POSTFIELD_DESC", new()
                        {
                            AcceptsValues = true,
                            ArgumentsRequired = true,
                        }),
                    ]),
                ], new PastebinCommand()),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasPastebin);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasPastebin);

        public void FinalizeAddon()
        { }

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.Pastebin.Resources.Languages.Output.Localizations", typeof(PastebinInit).Assembly));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
        }
    }
}

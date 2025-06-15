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
using Nitrocid.Extras.Pastebin.Localized;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Languages;
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
            new CommandInfo("pastebin", LanguageTools.GetLocalized("NKS_PASTEBIN_COMMAND_PASTEBIN_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "file/string", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_PASTEBIN_COMMAND_PASTEBIN_ARGUMENT_FILESTRING_DESC")
                        }),
                        new CommandArgumentPart(false, "arguments", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_PASTEBIN_COMMAND_PASTEBIN_ARGUMENT_ARGUMENTS_DESC")
                        }),
                    ],
                    [
                        new SwitchInfo("provider", LanguageTools.GetLocalized("NKS_PASTEBIN_COMMAND_PASTEBIN_SWITCH_PROVIDER_DESC"), new()
                        {
                            AcceptsValues = true,
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("type", LanguageTools.GetLocalized("NKS_PASTEBIN_COMMAND_PASTEBIN_SWITCH_TYPE_DESC"), new()
                        {
                            AcceptsValues = true,
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("postpage", LanguageTools.GetLocalized("NKS_PASTEBIN_COMMAND_PASTEBIN_SWITCH_POSTPAGE_DESC"), new()
                        {
                            AcceptsValues = true,
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("postformat", LanguageTools.GetLocalized("NKS_PASTEBIN_COMMAND_PASTEBIN_SWITCH_POSTFORMAT_DESC"), new()
                        {
                            AcceptsValues = true,
                            ArgumentsRequired = true,
                        }),
                        new SwitchInfo("postfield", LanguageTools.GetLocalized("NKS_PASTEBIN_COMMAND_PASTEBIN_SWITCH_POSTFIELD_DESC"), new()
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

        void IAddon.StartAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.Pastebin", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StopAddon()
        {
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.Pastebin");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
        }
    }
}

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
using Nitrocid.Extras.LanguageStudio.Commands;
using Nitrocid.Extras.LanguageStudio.Localized;
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using System.Linq;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Languages;

namespace Nitrocid.Extras.LanguageStudio
{
    internal class LanguageStudioInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("mklang", "NKS_LANGUAGESTUDIO_COMMAND_MKLANG_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "pathToTranslations", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_LANGUAGESTUDIO_COMMAND_MKLANG_ARGUMENT_PATH_DESC"
                        }),
                    ],
                    [
                        new SwitchInfo("tui", "NKS_LANGUAGESTUDIO_COMMAND_MKLANG_SWITCH_TUI_DESC")
                    ])
                ], new MkLangCommand())
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasLanguageStudio);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        void IAddon.StartAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.LanguageStudio", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StopAddon()
        {
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.LanguageStudio");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
        }

        void IAddon.FinalizeAddon()
        { }
    }
}

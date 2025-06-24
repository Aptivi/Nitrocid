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

using Nitrocid.Extras.Chemistry.Commands;
using Nitrocid.Extras.Chemistry.Screensavers;
using Nitrocid.Extras.Chemistry.Localized;
using Nitrocid.Base.Kernel.Extensions;
using Nitrocid.Base.Misc.Screensaver;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using System.Linq;
using Nitrocid.Base.Languages;

namespace Nitrocid.Extras.Chemistry
{
    internal class ChemistryInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("element", /* Localizable */ "NKS_CHEMISTRY_COMMAND_ELEMENT_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "name/symbol/atomicNumber", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = /* Localizable */ "NKS_CHEMISTRY_COMMAND_ELEMENT_ARGUMENT_SPECIFIER_DESC"
                        }),
                    ])
                ], new ElementCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported),

            new CommandInfo("elements", /* Localizable */ "NKS_CHEMISTRY_COMMAND_ELEMENTS_DESC", new ElementsCommand(), CommandFlags.Wrappable | CommandFlags.RedirectionSupported)
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasChemistry);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasChemistry);

        public ModLoadPriority AddonType => ModLoadPriority.Optional;

        public void StartAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
            ScreensaverManager.AddonSavers.Add("periodicpreview", new PeriodicPreviewDisplay());
        }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ScreensaverManager.AddonSavers.Remove("periodicpreview");
        }

        public void FinalizeAddon()
        { }
    }
}

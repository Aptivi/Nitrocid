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
using Nitrocid.Kernel.Extensions;
using Nitrocid.Misc.Screensaver;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using System.Collections.Generic;
using System.Linq;
using Nitrocid.Languages;

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

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasChemistry);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        void IAddon.StartAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.Chemistry", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
            ScreensaverManager.AddonSavers.Add("periodicpreview", new PeriodicPreviewDisplay());
        }

        void IAddon.StopAddon()
        {
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.Chemistry");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ScreensaverManager.AddonSavers.Remove("periodicpreview");
        }

        void IAddon.FinalizeAddon()
        { }
    }
}

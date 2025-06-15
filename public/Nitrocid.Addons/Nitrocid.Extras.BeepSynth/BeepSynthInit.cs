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
using Nitrocid.Shell.ShellBase.Commands;
using System.Collections.Generic;
using Nitrocid.Kernel.Extensions;
using Nitrocid.Shell.ShellBase.Shells;
using System.Linq;
using Nitrocid.Extras.BeepSynth.Commands;
using Nitrocid.Extras.BeepSynth.Localized;
using Nitrocid.Languages;

namespace Nitrocid.Extras.BeepSynth
{
    internal class BeepSynthInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("beepsynth", "NKS_BEEPSYNTH_COMMAND_BEEPSYNTH_DESC",
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(true, "synthFile", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = "NKS_BEEPSYNTH_COMMAND_BEEPSYNTH_ARGUMENT_SYNTHFILE_DESC"
                        }),
                    ])
                ], new BeepSynthCommand())
        ];

        string IAddon.AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasBeepSynth);

        ModLoadPriority IAddon.AddonType => ModLoadPriority.Optional;

        void IAddon.StartAddon()
        {
            LanguageTools.AddCustomAction("Nitrocid.Extras.BeepSynth", new(() => LocalStrings.Languages, () => LocalStrings.Localizations, LocalStrings.Translate, LocalStrings.CheckCulture, LocalStrings.ListLanguagesCulture, LocalStrings.Exists));
            CommandManager.RegisterAddonCommands(ShellType.Shell, [.. addonCommands]);
        }

        void IAddon.StopAddon()
        {
            LanguageTools.RemoveCustomAction("Nitrocid.Extras.BeepSynth");
            CommandManager.UnregisterAddonCommands(ShellType.Shell, [.. addonCommands.Select((ci) => ci.Command)]);
        }

        void IAddon.FinalizeAddon()
        { }
    }
}

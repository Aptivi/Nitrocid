//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Extras.MailShell.Settings;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using System.Linq;
using Nitrocid.Languages;
using Nitrocid.Extras.MailShell.Mail;

namespace Nitrocid.Extras.MailShell
{
    internal class MailShellInit : IAddon
    {
        private readonly BaseCommand[] addonCommands =
        [
            new MailCommandExec(),
            new PopMailCommandExec(),
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasMailShell);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasMailShell);

        internal static MailConfig MailConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(MailConfig)) ? (MailConfig)Config.baseConfigurations[nameof(MailConfig)] : Config.GetFallbackKernelConfig<MailConfig>();

        public void FinalizeAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.MailShell.Resources.Languages.Output.Localizations", typeof(MailShellInit).Assembly));
            var config = new MailConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterShell("MailShell", new MailShellInfo());
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StartAddon()
        { }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            ShellManager.UnregisterShell("MailShell");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(MailConfig));
        }
    }
}

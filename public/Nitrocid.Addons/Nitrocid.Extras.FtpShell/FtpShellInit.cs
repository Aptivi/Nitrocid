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

using Nitrocid.Extras.FtpShell.FTP;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Extensions;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using System.Collections.Generic;
using System.Linq;
using FtpConfig = Nitrocid.Extras.FtpShell.Settings.FtpConfig;
using Nitrocid.Languages;

namespace Nitrocid.Extras.FtpShell
{
    internal class FtpShellInit : IAddon
    {
        private readonly List<CommandInfo> addonCommands =
        [
            new CommandInfo("ftp", LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_FTP_DESC"),
                [
                    new CommandArgumentInfo(
                    [
                        new CommandArgumentPart(false, "server", new CommandArgumentPartOptions()
                        {
                            ArgumentDescription = LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_COMMAND_FTP_ARGUMENT_SERVER_DESC")
                        }),
                    ])
                ], new FtpCommandExec())
        ];

        public string AddonName =>
            InterAddonTranslations.GetAddonName(KnownAddons.ExtrasFtpShell);

        public string AddonTranslatedName =>
            InterAddonTranslations.GetLocalizedAddonName(KnownAddons.ExtrasFtpShell);

        internal static FtpConfig FtpConfig =>
            ConfigTools.IsCustomSettingBuiltin(nameof(FtpConfig)) ? (FtpConfig)Config.baseConfigurations[nameof(FtpConfig)] : Config.GetFallbackKernelConfig<FtpConfig>();

        public void FinalizeAddon()
        {
            LanguageTools.AddCustomAction(AddonName, new("Nitrocid.Extras.FtpShell.Resources.Languages.Output.Localizations", typeof(FtpShellInit).Assembly));
            var config = new FtpConfig();
            ConfigTools.RegisterBaseSetting(config);
            ShellManager.RegisterShell("FTPShell", new FTPShellInfo());
            CommandManager.RegisterCustomCommands("Shell", [.. addonCommands]);
        }

        public void StartAddon()
        { }

        public void StopAddon()
        {
            LanguageTools.RemoveCustomAction(AddonName);
            ShellManager.UnregisterShell("FTPShell");
            CommandManager.UnregisterCustomCommands("Shell", [.. addonCommands.Select((ci) => ci.Command)]);
            ConfigTools.UnregisterBaseSetting(nameof(FtpConfig));
        }
    }
}

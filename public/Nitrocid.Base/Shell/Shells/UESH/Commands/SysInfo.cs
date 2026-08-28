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

using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Hardware;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Users;
using Nitrocid.Base.Users.Login.Motd;
using Nitrocid.Base.Users.Windows;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.Tools.Placeholder;

namespace Nitrocid.Base.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows the system information
    /// </summary>
    class SysInfoCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "sysinfo";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_KERNEL_CONFIGURATION_SETTINGS_APP_SYSINFO");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo([],
                [
                    new SwitchInfo("s", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_S_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("h", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_H_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("u", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_U_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("m", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_M_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("l", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_L_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                    new SwitchInfo("a", /* Localizable */ "NKS_SHELL_SHELLS_UESH_COMMAND_SYSINFO_SWITCH_A_DESC", new SwitchOptions()
                    {
                        AcceptsValues = false
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            bool ShowSystemInfo = parameters.ContainsSwitch("-s");
            bool ShowHardwareInfo = parameters.ContainsSwitch("-h");
            bool ShowUserInfo = parameters.ContainsSwitch("-u");
            bool ShowMessageOfTheDay = parameters.ContainsSwitch("-m");
            bool ShowMal = parameters.ContainsSwitch("-l");
            if (parameters.ContainsSwitch("-a") || parameters.SwitchesList.Length == 0)
            {
                ShowSystemInfo = true;
                ShowHardwareInfo = true;
                ShowUserInfo = true;
                ShowMessageOfTheDay = true;
                ShowMal = true;
            }

            if (ShowSystemInfo)
            {
                // Kernel section
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_KERNEL_TITLE"), ThemeColorsTools.GetColor(ThemeColorType.Separator));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_KERNEL_VERSION"), KernelReleaseInfo.Version?.ToString() ?? "0.0.0.0");
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_KERNEL_DEBUG"), KernelEntry.DebugMode.ToString());
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_KERNEL_USUAL"), KernelPlatform.IsOnUsualEnvironment().ToString());
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_KERNEL_SAFE"), KernelEntry.SafeMode.ToString());
                TextWriterRaw.Write();
            }

            if (ShowHardwareInfo)
            {
                // Hardware section
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_HW_TITLE"), ThemeColorsTools.GetColor(ThemeColorType.Separator));
                HardwareList.ListHardware();

                if (!WindowsUserTools.IsAdministrator())
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DRIVERS_HARDWARE_BASE_NEEDSELEVATION"), true, ThemeColorType.Error);
                else
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_HW_TIP"), true, ThemeColorType.Tip);
                TextWriterRaw.Write();
            }

            if (ShowUserInfo)
            {
                // User section
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_USER_TITLE"), ThemeColorsTools.GetColor(ThemeColorType.Separator));
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_USER_USERNAME"), UserManagement.CurrentUser.Username);
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_USER_HOSTNAME"), Config.MainConfig.HostName);
                ListEntryWriterColor.WriteListEntry(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_USER_LISTING"), string.Join(", ", UserManagement.ListAllUsers()));
                TextWriterRaw.Write();
            }

            if (ShowMessageOfTheDay)
            {
                // Show MOTD
                SeparatorWriterColor.WriteSeparatorColor("MOTD", ThemeColorsTools.GetColor(ThemeColorType.Separator));
                TextWriterColor.Write(PlaceParse.ProbePlaces(MotdParse.MotdMessage), true, ThemeColorType.NeutralText);
                TextWriterRaw.Write();
            }

            if (ShowMal)
            {
                // Show MAL
                SeparatorWriterColor.WriteSeparatorColor("MAL", ThemeColorsTools.GetColor(ThemeColorType.Separator));
                TextWriterColor.Write(PlaceParse.ProbePlaces(MalParse.MalMessage), true, ThemeColorType.NeutralText);
            }
            return 0;
        }
    }
}

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

using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;
using Nitrocid.Languages;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Hardware;
using Nitrocid.Users;
using Textify.Tools.Placeholder;
using Nitrocid.Users.Login.Motd;
using Nitrocid.Users.Windows;
using Nitrocid.Kernel.Configuration;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Shows the system information
    /// </summary>
    class SysInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool ShowSystemInfo = false;
            bool ShowHardwareInfo = false;
            bool ShowUserInfo = false;
            bool ShowMessageOfTheDay = false;
            bool ShowMal = false;
            if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-s"))
                ShowSystemInfo = true;
            if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-h"))
                ShowHardwareInfo = true;
            if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-u"))
                ShowUserInfo = true;
            if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-m"))
                ShowMessageOfTheDay = true;
            if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-l"))
                ShowMal = true;
            if (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-a") || parameters.SwitchesList.Length == 0)
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
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_KERNEL_VERSION") + " ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(KernelMain.Version?.ToString() ?? "0.0.0.0", true, ThemeColorType.ListValue);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_KERNEL_DEBUG") + " ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(KernelEntry.DebugMode.ToString(), true, ThemeColorType.ListValue);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_KERNEL_USUAL") + " ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(KernelPlatform.IsOnUsualEnvironment().ToString(), true, ThemeColorType.ListValue);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_KERNEL_SAFE") + " ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(KernelEntry.SafeMode.ToString(), true, ThemeColorType.ListValue);
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
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_USER_USERNAME") + " ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(UserManagement.CurrentUser.Username, true, ThemeColorType.ListValue);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_USER_HOSTNAME") + " ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(Config.MainConfig.HostName, true, ThemeColorType.ListValue);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_SYSINFO_USER_LISTING") + " ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(string.Join(", ", UserManagement.ListAllUsers()), true, ThemeColorType.ListValue);
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

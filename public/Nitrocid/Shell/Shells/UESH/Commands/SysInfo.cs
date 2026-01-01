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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Hardware;
using Nitrocid.Users;
using Nitrocid.Misc.Text.Probers.Placeholder;
using Nitrocid.Users.Login.Motd;
using Nitrocid.Users.Windows;
using Nitrocid.Kernel.Configuration;
using Terminaux.Writer.FancyWriters;

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
                SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("Kernel settings"), KernelColorTools.GetColor(KernelColorType.Separator));
                TextWriters.Write(Translate.DoTranslation("Kernel Version:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write(KernelMain.Version?.ToString() ?? "0.0.0.0", true, KernelColorType.ListValue);
                TextWriters.Write(Translate.DoTranslation("Debug Mode:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write(KernelEntry.DebugMode.ToString(), true, KernelColorType.ListValue);
                TextWriters.Write(Translate.DoTranslation("Running on usual environment:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write(KernelPlatform.IsOnUsualEnvironment().ToString(), true, KernelColorType.ListValue);
                TextWriters.Write(Translate.DoTranslation("Safe Mode:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write(KernelEntry.SafeMode.ToString(), true, KernelColorType.ListValue);
                TextWriterRaw.Write();
            }

            if (ShowHardwareInfo)
            {
                // Hardware section
                SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("Hardware settings"), KernelColorTools.GetColor(KernelColorType.Separator));
                HardwareList.ListHardware();

                if (!WindowsUserTools.IsAdministrator())
                    TextWriters.Write(Translate.DoTranslation("You'll need to restart the kernel as elevated in order to be able to show hardware information."), true, KernelColorType.Error);
                else
                    TextWriters.Write(Translate.DoTranslation("Use \"hwinfo\" for extended information about hardware."), true, KernelColorType.Tip);
                TextWriterRaw.Write();
            }

            if (ShowUserInfo)
            {
                // User section
                SeparatorWriterColor.WriteSeparatorColor(Translate.DoTranslation("User settings"), KernelColorTools.GetColor(KernelColorType.Separator));
                TextWriters.Write(Translate.DoTranslation("Current user name:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write(UserManagement.CurrentUser.Username, true, KernelColorType.ListValue);
                TextWriters.Write(Translate.DoTranslation("Current host name:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write(Config.MainConfig.HostName, true, KernelColorType.ListValue);
                TextWriters.Write(Translate.DoTranslation("Available usernames:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write(string.Join(", ", UserManagement.ListAllUsers()), true, KernelColorType.ListValue);
                TextWriterRaw.Write();
            }

            if (ShowMessageOfTheDay)
            {
                // Show MOTD
                SeparatorWriterColor.WriteSeparatorColor("MOTD", KernelColorTools.GetColor(KernelColorType.Separator));
                TextWriters.Write(PlaceParse.ProbePlaces(MotdParse.MotdMessage), true, KernelColorType.NeutralText);
                TextWriterRaw.Write();
            }

            if (ShowMal)
            {
                // Show MAL
                SeparatorWriterColor.WriteSeparatorColor("MAL", KernelColorTools.GetColor(KernelColorType.Separator));
                TextWriters.Write(PlaceParse.ProbePlaces(MalParse.MalMessage), true, KernelColorType.NeutralText);
            }
            return 0;
        }
    }
}

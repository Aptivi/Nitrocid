//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using Terminaux.Writer.FancyWriters;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel;
using Nitrocid.Shell.ShellBase.Help;
using Nitrocid.Users.Login.Handlers;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Hardware;
using Nitrocid.Users;
using Nitrocid.Network.Base;
using Nitrocid.Misc.Text.Probers.Placeholder;
using Nitrocid.Users.Login.Motd;

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
                TextFancyWriters.WriteSeparator(Translate.DoTranslation("Kernel settings"), true, KernelColorType.Separator);
                TextWriters.Write(Translate.DoTranslation("Kernel Version:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write(KernelMain.Version.ToString(), true, KernelColorType.ListValue);
                TextWriters.Write(Translate.DoTranslation("Debug Mode:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write(KernelEntry.DebugMode.ToString(), true, KernelColorType.ListValue);
                TextWriters.Write(Translate.DoTranslation("Help command simplified:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write(HelpPrintTools.SimHelp.ToString(), true, KernelColorType.ListValue);
                TextWriters.Write(Translate.DoTranslation("MOTD on Login:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write(BaseLoginHandler.ShowMOTD.ToString(), true, KernelColorType.ListValue);
                TextWriters.Write(Translate.DoTranslation("Time/Date on corner:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write(TimeDateTools.CornerTimeDate.ToString(), true, KernelColorType.ListValue);
                TextWriterColor.Write();
            }

            if (ShowHardwareInfo)
            {
                // Hardware section
                TextFancyWriters.WriteSeparator(Translate.DoTranslation("Hardware settings"), true, KernelColorType.Separator);
                HardwareList.ListHardware();
                TextWriters.Write(Translate.DoTranslation("Use \"hwinfo\" for extended information about hardware."), true, KernelColorType.Tip);
                TextWriterColor.Write();
            }

            if (ShowUserInfo)
            {
                // User section
                TextFancyWriters.WriteSeparator(Translate.DoTranslation("User settings"), true, KernelColorType.Separator);
                TextWriters.Write(Translate.DoTranslation("Current user name:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write(UserManagement.CurrentUser.Username, true, KernelColorType.ListValue);
                TextWriters.Write(Translate.DoTranslation("Current host name:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write(NetworkTools.HostName, true, KernelColorType.ListValue);
                TextWriters.Write(Translate.DoTranslation("Available usernames:") + " ", false, KernelColorType.ListEntry);
                TextWriters.Write(string.Join(", ", UserManagement.ListAllUsers()), true, KernelColorType.ListValue);
                TextWriterColor.Write();
            }

            if (ShowMessageOfTheDay)
            {
                // Show MOTD
                TextFancyWriters.WriteSeparator("MOTD", true, KernelColorType.Separator);
                TextWriters.Write(PlaceParse.ProbePlaces(MotdParse.MotdMessage), true, KernelColorType.NeutralText);
                TextWriterColor.Write();
            }

            if (ShowMal)
            {
                // Show MAL
                TextFancyWriters.WriteSeparator("MAL", true, KernelColorType.Separator);
                TextWriters.Write(PlaceParse.ProbePlaces(MalParse.MalMessage), true, KernelColorType.NeutralText);
            }
            return 0;
        }
    }
}

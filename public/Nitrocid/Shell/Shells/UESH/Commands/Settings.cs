
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Settings;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lets you change kernel settings
    /// </summary>
    /// <remarks>
    /// This command starts up the Settings application, which allows you to change the kernel settings available to you. It's the successor to the defunct Nitrocid KS Configuration Tool application, and is native to the kernel.
    /// <br></br>
    /// It starts with the list of sections to start from. Once the user selects one, they'll be greeted with various options that are configurable. When they choose one, they'll be able to change the setting there.
    /// <br></br>
    /// If you just want to try out a setting without saving to the configuration file, you can change a setting and exit it immediately. It only survives the current session until you decide to save the changes to the configuration file.
    /// <br></br>
    /// Some settings allow you to specify a string, a number, or by the usage of another API, like the ColorWheel() tool.
    /// <br></br>
    /// In the string or long string values, if you used the /clear value, it will blank out the value. In some settings, if you just pressed ENTER, it'll use the same value that the kernel uses at the moment.
    /// <br></br>
    /// We've made sure that this application is user-friendly.
    /// <br></br>
    /// For the screensaver and splashes, refer to the command usage below.
    /// <br></br>
    /// <list type="table">
    /// <listheader>
    /// <term>Switches</term>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <term>-saver</term>
    /// <description>Opens the screensaver settings</description>
    /// </item>
    /// <item>
    /// <term>-splash</term>
    /// <description>Opens the splash settings</description>
    /// </item>
    /// </list>
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class SettingsCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var SettingsType = Misc.Settings.SettingsType.Normal;
            if (ListSwitchesOnly.Length > 0)
            {
                if (ListSwitchesOnly[0] == "-saver")
                    SettingsType = Misc.Settings.SettingsType.Screensaver;
                if (ListSwitchesOnly[0] == "-splash")
                    SettingsType = Misc.Settings.SettingsType.Splash;
            }
            SettingsApp.OpenMainPage(SettingsType);
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"));
            TextWriterColor.Write("  -saver: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Opens the screensaver settings"), true, KernelColorType.ListValue);
            TextWriterColor.Write("  -splash: ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Opens the splash settings"), true, KernelColorType.ListValue);
        }

    }
}
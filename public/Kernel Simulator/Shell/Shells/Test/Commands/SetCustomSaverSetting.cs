
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Screensaver.Customized;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Test.Commands
{
    /// <summary>
    /// It lets you set a setting from a custom saver. Load all the mods and screensavers first before using this command.
    /// </summary>
    class Test_SetCustomSaverSettingCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (CustomSaverTools.CustomSavers.ContainsKey(ListArgsOnly[0]))
            {
                if (CustomSaverTools.SetCustomSaverSettings(ListArgsOnly[0], ListArgsOnly[1], ListArgsOnly[2]))
                {
                    TextWriterColor.Write(Translate.DoTranslation("Settings set successfully for screensaver") + " {0}.", ListArgsOnly[0]);
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Failed to set a setting for screensaver") + " {0}.", true, ColorTools.ColTypes.Error, ListArgsOnly[0]);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Screensaver {0} not found."), true, ColorTools.ColTypes.Error, ListArgsOnly[0]);
            }
        }

    }
}
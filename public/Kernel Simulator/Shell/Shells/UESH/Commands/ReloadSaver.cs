
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
using KS.Kernel;
using KS.Languages;
using KS.Misc.Screensaver.Customized;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can reload screensaver files after making changes to them.
    /// </summary>
    /// <remarks>
    /// This command compiles the contents of your modfile to see if there is any errors, and reassign your screensaver mod files to the valid screensaver list if your screensaver doesn't contain any errors from the build system.
    /// <br></br>
    /// If you want to reflect changes on your screensaver, make some changes on your screensaver modfile and run this command pointing to the changed modfile.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class ReloadSaverCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (!Flags.SafeMode)
            {
                CustomSaverParser.ParseCustomSaver(ListArgsOnly[0]);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Reloading not allowed in safe mode."), true, KernelColorType.Error);
            }
        }

        public override void HelpHelper() =>
            // Print available screensavers
            TextWriterColor.Write(Translate.DoTranslation("where customsaver will be") + " {0}", string.Join(", ", CustomSaverTools.CustomSavers.Keys));

    }
}
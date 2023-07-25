
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

using System;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Interactive;
using KS.Misc.Writers.ConsoleWriters;
using KS.Modifications;
using KS.Modifications.ManPages;
using KS.Modifications.ManPages.Interactive;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Opens the mod manual
    /// </summary>
    /// <remarks>
    /// If the mod has a manual page which you can refer to, you can use them by this command.
    /// <br></br>
    /// </remarks>
    class ModManualCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string modName = ListArgsOnly[0];
            if (!ModManager.Mods.ContainsKey(modName))
                TextWriterColor.Write(Translate.DoTranslation("Tried to query the manuals for nonexistent mod {0}."), true, KernelColorType.Error, modName);
            var tuiInstance = new ManualViewer() { modName = modName };
            InteractiveTuiTools.OpenInteractiveTui(tuiInstance);
        }

    }
}

//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Interactive;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Modifications;
using KS.Misc.Interactives;
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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string modName = parameters.ArgumentsList[0];
            if (!ModManager.Mods.ContainsKey(modName))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("Tried to query the manuals for nonexistent mod {0}."), true, KernelColorType.Error, modName);
                return 10000 + (int)KernelExceptionType.NoSuchMod;
            }

            var tuiInstance = new ManualViewerCli() { modName = modName };
            InteractiveTuiTools.OpenInteractiveTui(tuiInstance);
            return 0;
        }

    }
}

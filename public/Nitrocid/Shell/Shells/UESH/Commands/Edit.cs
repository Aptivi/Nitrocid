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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Opens the text editor shell
    /// </summary>
    /// <remarks>
    /// If you want to edit a text document, this command will let you open the text editor shell to a specified document so you can edit it. Currently, it's on the basic stage, so it doesn't have advanced options yet.
    /// <br></br>
    /// It can also open binary files, but we don't recommend doing that, because it isn't a hex editor yet. Editing a binary file may or may not cause file corruptions. Use hexedit for such tasks.
    /// </remarks>
    class EditCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string path = FilesystemTools.NeutralizePath(parameters.ArgumentsList[0]);
            bool forceText = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-text");
            bool forceJson = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-json");
            bool forceHex = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-hex");
            bool forceSql = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-sql");
            if (FilesystemTools.FileExists(path))
                FilesystemTools.OpenEditor(path, forceText, forceJson, forceHex, forceSql);
            else
                TextWriters.Write(Translate.DoTranslation("Can't edit file {0} because it's not found."), true, KernelColorType.Error, path);
            return 0;
        }

    }
}

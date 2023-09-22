
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
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files.Editors.JsonShell;
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Json.Commands
{
    /// <summary>
    /// Deletes a property
    /// </summary>
    /// <remarks>
    /// You can use this command to remove a property from the end of the JSON file.
    /// </remarks>
    class JsonShell_DelPropertyCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            JsonTools.JsonShell_RemoveProperty(ListArgsOnly[0]);
            TextWriterColor.Write(Translate.DoTranslation("Removed property."), true, KernelColorType.Success);
            return 0;
        }

    }
}

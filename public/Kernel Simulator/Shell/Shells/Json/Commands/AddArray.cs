
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using KS.Misc.Editors.JsonShell;
using KS.Shell.ShellBase.Commands;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace KS.Shell.Shells.Json.Commands
{
    /// <summary>
    /// Adds a new array
    /// </summary>
    /// <remarks>
    /// You can use this command to add an array to the end of the parent property. Note that the parent property must exist.
    /// </remarks>
    class JsonShell_AddArrayCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string parent = SwitchManager.GetSwitchValue(ListSwitchesOnly, "-parentProperty");
            JsonTools.JsonShell_AddNewArray(parent, ListArgsOnly[0], JArray.Parse("[ \"" + string.Join("\", \"", ListArgsOnly.Skip(1).ToArray()) + "\" ]"));
        }
    }
}

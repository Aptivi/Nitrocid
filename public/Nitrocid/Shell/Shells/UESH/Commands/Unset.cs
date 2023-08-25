
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

using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Scripting;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Removes a UESH variable from the list
    /// </summary>
    /// <remarks>
    /// You can remove a UESH variable from the list or wipe its variable value
    /// </remarks>
    class UnsetCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            bool destructive = !SwitchManager.ContainsSwitch(ListSwitchesOnly, "-justwipe");
            UESHVariables.SetVariable(ListArgsOnly[0], "");
            if (destructive)
                UESHVariables.RemoveVariable(ListArgsOnly[0]);
            return 0;
        }
    }
}

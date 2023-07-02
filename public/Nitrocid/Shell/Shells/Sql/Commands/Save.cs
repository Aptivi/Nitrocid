
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

using KS.Misc.Editors.SqlEdit;
using KS.Misc.Editors.TextEdit;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Sql.Commands
{
    /// <summary>
    /// Saves the file
    /// </summary>
    /// <remarks>
    /// This command will save any changes made to the text file that is currently open
    /// </remarks>
    class Sql_SaveCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly) => SqlEditTools.SqlEdit_SaveSqlFile();

    }
}

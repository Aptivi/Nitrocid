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

using Nitrocid.Extras.SqlShell.Tools;
using Nitrocid.Files.Operations;
using Nitrocid.Shell.ShellBase.Commands;
using Terminaux.Inputs.Styles.Editor;

namespace Nitrocid.Extras.SqlShell.Sql.Commands
{
    /// <summary>
    /// Opens the SQL file in the interactive hex editor TUI
    /// </summary>
    /// <remarks>
    /// This command will open the currently open SQL database file in the interactive hex editor.
    /// </remarks>
    class TuiCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string path = SqlShellCommon.DatabasePath;
            byte[] bytes = Reading.ReadAllBytesNoBlock(path);
            HexEditInteractive.OpenInteractive(ref bytes);

            // Save the results
            SqlEditTools.SqlEdit_CloseSqlFile();
            Writing.WriteAllBytesNoBlock(path, bytes);
            SqlEditTools.SqlEdit_OpenSqlFile(path);
            return 0;
        }
    }
}

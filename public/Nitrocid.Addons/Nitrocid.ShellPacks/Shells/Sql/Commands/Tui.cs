//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Inputs.Styles.Editor;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.Sql.Commands
{
    /// <summary>
    /// Opens the SQL file in the interactive hex editor TUI
    /// </summary>
    /// <remarks>
    /// This command will open the currently open SQL database file in the interactive hex editor.
    /// </remarks>
    class TuiCommand : BaseCommand, ICommand
    {

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var sqlShell = (SqlShell?)shell ??
                throw new KernelException(KernelExceptionType.SqlEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            string path = sqlShell.DatabasePath;
            byte[] bytes = FilesystemTools.ReadAllBytesNoBlock(path);
            HexEditInteractive.OpenInteractive(ref bytes);

            // Save the results
            sqlShell.CloseSqlFile();
            FilesystemTools.WriteAllBytesNoBlock(path, bytes);
            sqlShell.OpenSqlFile(path);
            return 0;
        }
    }
}

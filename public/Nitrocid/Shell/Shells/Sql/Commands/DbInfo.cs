
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
using KS.Languages;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Sql.Commands
{
    /// <summary>
    /// Database information
    /// </summary>
    /// <remarks>
    /// This command prints database information.
    /// </remarks>
    class Sql_DbInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            var connection = SqlShellCommon.sqliteConnection;
            TextWriterColor.Write(Translate.DoTranslation("Database path:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(connection.DataSource, true, KernelColorType.ListValue);
            TextWriterColor.Write(Translate.DoTranslation("Server version:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(connection.ServerVersion, true, KernelColorType.ListValue);
            TextWriterColor.Write(Translate.DoTranslation("Connection state:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(connection.State.ToString(), true, KernelColorType.ListValue);
            TextWriterColor.Write(Translate.DoTranslation("Connection string:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(connection.ConnectionString, true, KernelColorType.ListValue);
            TextWriterColor.Write(Translate.DoTranslation("Database name:") + " ", false, KernelColorType.ListEntry);
            TextWriterColor.Write(connection.Database, true, KernelColorType.ListValue);
            return 0;
        }
    }
}

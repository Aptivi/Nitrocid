//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using Nitrocid.Languages;
using Terminaux.Shell.Commands;

namespace Nitrocid.ShellPacks.Shells.Sql.Commands
{
    /// <summary>
    /// Database information
    /// </summary>
    /// <remarks>
    /// This command prints database information.
    /// </remarks>
    class DbInfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var connection = SqlShellCommon.sqliteConnection;
            if (connection is null)
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_DBINFO_NOCONNECTION"), KernelColorType.Error);
                return 41;
            }
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_DBINFO_PATH") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(connection.DataSource, true, KernelColorType.ListValue);
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_DBINFO_VERSION") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(connection.ServerVersion, true, KernelColorType.ListValue);
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_DBINFO_STATE") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(connection.State.ToString(), true, KernelColorType.ListValue);
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_DBINFO_STRING") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(connection.ConnectionString, true, KernelColorType.ListValue);
            TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_DBINFO_NAME") + " ", false, KernelColorType.ListEntry);
            TextWriters.Write(connection.Database, true, KernelColorType.ListValue);
            return 0;
        }
    }
}

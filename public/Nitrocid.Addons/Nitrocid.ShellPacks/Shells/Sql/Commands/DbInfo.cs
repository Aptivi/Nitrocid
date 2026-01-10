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

using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Languages;
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
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_DBINFO_NOCONNECTION"), ThemeColorType.Error);
                return 41;
            }
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_DBINFO_PATH") + " ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write(connection.DataSource, true, ThemeColorType.ListValue);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_DBINFO_VERSION") + " ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write(connection.ServerVersion, true, ThemeColorType.ListValue);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_DBINFO_STATE") + " ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write(connection.State.ToString(), true, ThemeColorType.ListValue);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_DBINFO_STRING") + " ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write(connection.ConnectionString, true, ThemeColorType.ListValue);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_DBINFO_NAME") + " ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write(connection.Database, true, ThemeColorType.ListValue);
            return 0;
        }
    }
}

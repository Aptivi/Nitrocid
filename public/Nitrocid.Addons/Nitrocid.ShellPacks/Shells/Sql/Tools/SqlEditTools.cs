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

using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;

namespace Nitrocid.ShellPacks.Shells.Sql.Tools
{
    /// <summary>
    /// Sql editor tools module
    /// </summary>
    public static class SqlEditTools
    {
        /// <summary>
        /// Opens the SQL file
        /// </summary>
        /// <param name="File">Target file. We recommend you to use <see cref="FilesystemTools.NeutralizePath(string, bool)"></see> to neutralize path.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static SqliteConnection OpenSqlFile(string File)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to open SQL database file {0}...", vars: [File]);
            // TODO: NKS_SHELLPACKS_SQL_EXCEPTION_NOTSQL -> Not an SQL database file
            if (!FilesystemTools.IsSql(File))
                throw new KernelException(KernelExceptionType.SqlEditor, LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_EXCEPTION_NOTSQL"));
            var sqliteConnection = new SqliteConnection($"Data Source={File}");
            sqliteConnection.Open();
            return sqliteConnection;
        }

        /// <summary>
        /// Closes SQL file
        /// </summary>
        /// <param name="connection">SQL connection</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static void CloseSqlFile(SqliteConnection? connection)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to close SQL database file...");
            connection?.Close();
        }

        /// <summary>
        /// Executes an SQL command
        /// </summary>
        /// <param name="connection">SQL connection</param>
        /// <param name="query">An SQL query</param>
        /// <param name="replies">Replies array to be filled</param>
        /// <param name="error">Error during query (null if there are no errors)</param>
        /// <param name="parameters">SQL query parameters</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SqlCommand(SqliteConnection? connection, string query, out string[] replies, out Exception? error, params SqliteParameter[] parameters)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Trying to execute query {0}...", vars: [query]);
            List<string> replyList = [];
            using var sqlCommand = new SqliteCommand(query, connection);

            // Add parameters
            foreach (SqliteParameter parameter in parameters)
                sqlCommand.Parameters.Add(parameter);

            // Try to execute the command
            using var sqlReader = sqlCommand.ExecuteReader();
            while (sqlReader.Read())
            {
                for (int i = 0; i < sqlReader.FieldCount; i++)
                {
                    string reply = !sqlReader.IsDBNull(i) ? sqlReader.GetString(i) : "";
                    replyList.Add(reply);
                }
            }
            replies = [.. replyList];
            error = null;
            return true;
        }
    }
}

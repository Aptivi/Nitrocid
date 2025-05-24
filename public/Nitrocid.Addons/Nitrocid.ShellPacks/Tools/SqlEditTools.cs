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

using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Nitrocid.Files;
using Nitrocid.Kernel.Debugging;
using Nitrocid.ShellPacks.Shells.Sql;

namespace Nitrocid.ShellPacks.Tools
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
        public static bool SqlEdit_OpenSqlFile(string File)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to open file {0}...", vars: [File]);
                SqlShellCommon.sqliteConnection = new SqliteConnection($"Data Source={File}");
                SqlShellCommon.sqliteConnection.Open();
                SqlShellCommon.sqliteDatabasePath = File;
                return FilesystemTools.IsSql(File);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Open file {0} failed: {1}", vars: [File, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Closes SQL file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SqlEdit_CloseSqlFile()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to close file...");
                SqlShellCommon.sqliteConnection?.Close();
                SqlShellCommon.sqliteConnection = null;
                SqlShellCommon.sqliteDatabasePath = "";
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Closing file failed: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Executes an SQL command
        /// </summary>
        /// <param name="query">An SQL query</param>
        /// <param name="replies">Replies array to be filled</param>
        /// <param name="parameters">SQL query parameters</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SqlEdit_SqlCommand(string query, ref string[] replies, params SqliteParameter[] parameters)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to execute query {0}...", vars: [query]);
                List<string> replyList = [];
                using var sqlCommand = new SqliteCommand(query, SqlShellCommon.sqliteConnection);

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
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "SQL command failed: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

    }
}

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
using Microsoft.Data.Sqlite;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.ShellPacks.Shells.Sql.Tools;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.Sql
{
    /// <summary>
    /// Sql editor tools module
    /// </summary>
    public partial class SqlShell : BaseShell, IShell
    {
        /// <summary>
        /// Opens the SQL file
        /// </summary>
        /// <param name="File">Target file. We recommend you to use <see cref="FilesystemTools.NeutralizePath(string, bool)"></see> to neutralize path.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool OpenSqlFile(string File)
        {
            try
            {
                sqliteConnection = SqlEditTools.OpenSqlFile(File);
                sqliteDatabasePath = File;
                return true;
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
        public bool CloseSqlFile()
        {
            try
            {
                SqlEditTools.CloseSqlFile(Connection);
                sqliteConnection = null;
                sqliteDatabasePath = "";
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
        /// <param name="error">Error during query (null if there are no errors)</param>
        /// <param name="parameters">SQL query parameters</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public bool SqlCommand(string query, out string[] replies, out Exception? error, params SqliteParameter[] parameters)
        {
            try
            {
                return SqlEditTools.SqlCommand(Connection, query, out replies, out error, parameters);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "SQL command failed: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                replies = [];
                error = ex;
                return false;
            }
        }
    }
}

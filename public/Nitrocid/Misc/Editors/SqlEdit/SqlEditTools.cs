
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

using System;
using KS.Files;
using KS.Kernel.Debugging;
using KS.Shell.Shells.Sql;
using Microsoft.Data.Sqlite;

namespace KS.Misc.Editors.SqlEdit
{
    /// <summary>
    /// Sql editor tools module
    /// </summary>
    public static class SqlEditTools
    {

        /// <summary>
        /// Opens the SQL file
        /// </summary>
        /// <param name="File">Target file. We recommend you to use <see cref="Filesystem.NeutralizePath(string, bool)"></see> to neutralize path.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SqlEdit_OpenSqlFile(string File)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to open file {0}...", File);
                SqlShellCommon.sqliteConnection = new SqliteConnection($"Data Source={File}");
                SqlShellCommon.sqliteConnection.Open();
                SqlShellCommon.sqliteDatabasePath = File;
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Open file {0} failed: {1}", File, ex.Message);
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
                SqlShellCommon.sqliteConnection.Close();
                SqlShellCommon.sqliteConnection = null;
                SqlShellCommon.sqliteDatabasePath = "";
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Closing file failed: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        /// <summary>
        /// Saves SQL file
        /// </summary>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SqlEdit_SaveSqlFile()
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Trying to save file...");
                // TODO: Currently does nothing.
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Saving file failed: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

    }
}

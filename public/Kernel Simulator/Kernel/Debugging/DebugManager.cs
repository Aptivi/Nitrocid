
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Linq;
using KS.Files.Read;

namespace KS.Kernel.Debugging
{
    /// <summary>
    /// Debug management module
    /// </summary>
    public static class DebugManager
    {

        /// <summary>
        /// Debug maximum quota size before rotation
        /// </summary>
        public static double DebugQuota = 1073741824d; // 1073741824 bytes = 1 GiB (1 GB for Windows)

        internal static string DebugPath = "";

        /// <summary>
        /// Checks to see if the debug file exceeds the quota
        /// </summary>
        public static void CheckForDebugQuotaExceed()
        {
            try
            {
                string debugFilePath = DebugPath;

                // If, in rare cases, we don't have debug log file, create it
                if (!File.Exists(debugFilePath))
                    File.Create(debugFilePath).Close();

                // Now, get information from the currently rotating debug file
                var FInfo = new FileInfo(debugFilePath);
                double OldSize = FInfo.Length;
                if (OldSize > DebugQuota)
                {
                    var Lines = FileRead.ReadAllLinesNoBlock(debugFilePath).Skip(5).ToArray();
                    DebugWriter.DebugStreamWriter.Close();
                    DebugWriter.DebugStreamWriter = new StreamWriter(debugFilePath) { AutoFlush = true };
                    for (int l = 0; l <= Lines.Length - 1; l++)
                        // Remove the first 5 lines from stream.
                        DebugWriter.DebugStreamWriter.WriteLine(Lines[l]);
                    DebugWriter.WriteDebug(DebugLevel.W, "Max debug quota size exceeded, was {0} bytes.", OldSize);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

    }
}

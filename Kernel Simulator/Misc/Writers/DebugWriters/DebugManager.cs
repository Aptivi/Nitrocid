using System;
using System.IO;
using System.Linq;
using KS.Files;

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

using KS.Files.Read;

namespace KS.Misc.Writers.DebugWriters
{
    public static class DebugManager
    {

        public static double DebugQuota = 1073741824d; // 1073741824 bytes = 1 GiB (1 GB for Windows)

        /// <summary>
        /// Checks to see if the debug file exceeds the quota
        /// </summary>
        public static void CheckForDebugQuotaExceed()
        {
            try
            {
                var FInfo = new FileInfo(Paths.GetKernelPath(KernelPathType.Debugging));
                double OldSize = FInfo.Length;
                if (OldSize > DebugQuota)
                {
                    var Lines = FileRead.ReadAllLinesNoBlock(Paths.GetKernelPath(KernelPathType.Debugging)).Skip(5).ToArray();
                    DebugWriter.DebugStreamWriter.Close();
                    DebugWriter.DebugStreamWriter = new StreamWriter(Paths.GetKernelPath(KernelPathType.Debugging)) { AutoFlush = true };
                    for (int l = 0, loopTo = Lines.Length - 1; l <= loopTo; l++) // Remove the first 5 lines from stream.
                        DebugWriter.DebugStreamWriter.WriteLine(Lines[l]);
                    DebugWriter.Wdbg(DebugLevel.W, "Max debug quota size exceeded, was {0} bytes.", OldSize);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WStkTrc(ex);
            }
        }

    }
}
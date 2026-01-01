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

using Terminaux.Writer.ConsoleWriters;
using System;
using System.Threading;

namespace Nitrocid.Kernel.Debugging.Testing.Facades.FacadeData
{
    internal static class KernelThreadTestData
    {
        /// <summary>
        /// Write hello to console
        /// </summary>
        internal static void WriteHello()
        {
            try
            {
                TextWriterColor.Write("Hello world!");
                TextWriterColor.Write("- Writing from thread: {0} [{1}]", Thread.CurrentThread.Name ?? "<null>", Environment.CurrentManagedThreadId);
                while (true)
                    Thread.Sleep(1);
            }
            catch
            {
                TextWriterColor.Write("- Goodbye from thread: {0} [{1}]", Thread.CurrentThread.Name ?? "<null>", Environment.CurrentManagedThreadId);
            }
        }
    }
}

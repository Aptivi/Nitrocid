
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
using KS.Kernel.Threading;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// Gets backtrace for all threads
    /// </summary>
    /// <remarks>
    /// This command will print backtrace information for all kernel threads
    /// </remarks>
    class Debug_ThreadsBtCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            // Print the list
            Dictionary<string, string[]> result = ThreadManager.GetThreadBacktraces();
            foreach (var trace in result)
            {
                string threadAddress = trace.Key;
                string[] threadTrace = trace.Value;
                TextWriterColor.Write(Translate.DoTranslation("Thread stack trace information for {0}") + "\n", true, KernelColorType.ListTitle, threadAddress);
                ListWriterColor.WriteList(threadTrace);
                TextWriterColor.Write();
            }
        }
    }
}

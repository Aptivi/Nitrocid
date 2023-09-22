
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

using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging.Trace;
using KS.Shell.ShellBase.Commands;
using System.Diagnostics;

namespace KS.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// Gets current backtrace
    /// </summary>
    /// <remarks>
    /// This command will print a backtrace for the current thread
    /// </remarks>
    class Debug_CurrentBtCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            var trace = new StackTrace(true);
            for (int framenum = 0; framenum < trace.FrameCount; framenum++)
            {
                var frame = new DebugStackFrame(framenum);
                TextWriterColor.Write($"[{framenum + 1}] - at {frame.RoutineName}() [{frame.RoutineFileName} line {frame.RoutineLineNumber} column {frame.RoutineColumnNumber}]");
            }
            return 0;
        }
    }
}

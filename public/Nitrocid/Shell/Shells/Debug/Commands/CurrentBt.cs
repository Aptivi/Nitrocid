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
using Nitrocid.Kernel.Debugging.Trace;
using Terminaux.Shell.Commands;
using System.Diagnostics;

namespace Nitrocid.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// Gets current backtrace
    /// </summary>
    /// <remarks>
    /// This command will print a backtrace for the current thread
    /// </remarks>
    class CurrentBtCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
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

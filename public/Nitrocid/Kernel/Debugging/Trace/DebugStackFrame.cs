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

using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using System.Diagnostics;
using System.IO;

namespace Nitrocid.Kernel.Debugging.Trace
{
    internal class DebugStackFrame
    {
        public string RoutineName { get; } = "";
        public string RoutinePath { get; } = "";
        public int RoutineLineNumber { get; }
        public int RoutineColumnNumber { get; }
        public string RoutineFileName { get; } = "";

        internal DebugStackFrame() :
            this(2)
        { }

        internal DebugStackFrame(int frameNumber)
        {
            // Check the frame number
            var trace = new StackTrace(true);
            frameNumber += 1;
            if (frameNumber <= 0 || frameNumber > trace.FrameCount)
                throw new KernelException(KernelExceptionType.Debug, Translate.DoTranslation("Stack frame number shouldn't exceed current amount of frames or shouldn't be negative."));

            var frame = trace.GetFrame(frameNumber) ??
                throw new KernelException(KernelExceptionType.Debug, Translate.DoTranslation("Can't get frame"));
            string FrameFilePath = frame.GetFileName() ?? "";
            string Source = Path.GetFileName(FrameFilePath);
            int LineNum = frame.GetFileLineNumber();
            int ColNum = frame.GetFileColumnNumber();
            var Method = frame.GetMethod() ??
                throw new KernelException(KernelExceptionType.Debug, Translate.DoTranslation("Can't get method"));
            string Func = Method.Name;
            string FullFunc = Method.ReflectedType?.FullName ?? "";

            // Install values
            RoutineFileName = Source;
            RoutineLineNumber = LineNum;
            RoutineColumnNumber = ColNum;
            RoutineName = Func;
            RoutinePath = $"{FullFunc}.{Func}";
        }
    }
}

// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using KS.Files;
using KS.Languages;
using System;
using System.Diagnostics;
using System.IO;

namespace KS.Kernel.Debugging.Trace
{
    internal class DebugStackFrame
    {
        public string RoutineName { get; }
        public int RoutineLineNumber { get; }
        public int RoutineColumnNumber { get; }
        public string RoutineFileName { get; }
        
        internal DebugStackFrame() : 
            this(2) { }

        internal DebugStackFrame(int frameNumber)
        {
            // Check the frame number
            var trace = new StackTrace(true);
            frameNumber += 1;
            if (frameNumber <= 0 || frameNumber > trace.FrameCount)
                throw new ArgumentOutOfRangeException(nameof(frameNumber), frameNumber, Translate.DoTranslation("Stack frame number shouldn't exceed current amount of frames or shouldn't be negative."));

            string FrameFilePath = Filesystem.NeutralizePath(trace.GetFrame(frameNumber).GetFileName());
            string Source = Path.GetFileName(FrameFilePath);
            int LineNum = trace.GetFrame(frameNumber).GetFileLineNumber();
            int ColNum = trace.GetFrame(frameNumber).GetFileColumnNumber();
            string Func = trace.GetFrame(frameNumber).GetMethod().Name;

            // Install values
            RoutineFileName = Source;
            RoutineLineNumber = LineNum;
            RoutineColumnNumber = ColNum;
            RoutineName = Func;
        }
    }
}

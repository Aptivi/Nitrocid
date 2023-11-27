//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Kernel.Exceptions;
using KS.Languages;
using System.Diagnostics;

namespace KS.Kernel.Debugging.Trace
{
    internal class DebugStackFrameBasic
    {
        public string RoutineName { get; }
        public string RoutinePath { get; }

        internal DebugStackFrameBasic() :
            this(2)
        { }

        internal DebugStackFrameBasic(int frameNumber)
        {
            // Check the frame number
            var trace = new StackTrace();
            frameNumber += 1;
            if (frameNumber <= 0 || frameNumber > trace.FrameCount)
                throw new KernelException(KernelExceptionType.Debug, Translate.DoTranslation("Stack frame number shouldn't exceed current amount of frames or shouldn't be negative."));

            var Method = trace.GetFrame(frameNumber).GetMethod();
            string Func = Method.Name;
            string FullFunc = Method.ReflectedType.FullName;

            // Install values
            RoutineName = Func;
            RoutinePath = $"{FullFunc}.{Func}";
        }
    }
}

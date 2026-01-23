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
using Nitrocid.Kernel.Power;
using Nitrocid.Languages;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class Debug : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Enables the debugger");
        public override TestSection TestSection => TestSection.Kernel;
        public override void Run()
        {
            TextWriterColor.Write(Translate.DoTranslation("Previous value") + ": {0}", KernelEntry.DebugMode);
            if (!KernelEntry.DebugMode)
            {
                KernelEntry.DebugMode = true;
            }
            else
            {
                // This is to abort the remote debugger.
                PowerManager.RebootRequested = true;

                // Now, do the job!
                KernelEntry.DebugMode = false;
                PowerManager.RebootRequested = false;
            }
            TextWriterColor.Write(Translate.DoTranslation("Current value") + ": {0}", KernelEntry.DebugMode);
        }
    }
}

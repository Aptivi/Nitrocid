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

using Nitrocid.Kernel.Debugging.RemoteDebug;
using Nitrocid.Languages;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class RDebug : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Enables the remote debugger");
        public override TestSection TestSection => TestSection.Kernel;
        public override void Run()
        {
            if (!RemoteDebugger.RDebugThread.IsAlive)
            {
                RemoteDebugger.StartRDebugThread();
            }
            else
            {
                RemoteDebugger.StopRDebugThread();
            }
        }
    }
}

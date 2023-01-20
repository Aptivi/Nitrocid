
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

using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.Base;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class InternetCheck : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Checks for the connection to the Internet");
        public override bool TestInteractive => false;
        public override object TestExpectedValue => true;
        public override void Run()
        {
            bool netFound = NetworkTools.InternetAvailable;
            TextWriterColor.Write(Translate.DoTranslation("Internet availability is") + $": {netFound}");
            TestActualValue = netFound;
        }
    }
}

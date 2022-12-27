
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.FancyWriters;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class PrintSepColor : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Print a separator to console with color");
        public override void Run()
        {
            SeparatorWriterColor.WriteSeparator("Hello world!", true, KernelColorType.Success);
        }
    }
}

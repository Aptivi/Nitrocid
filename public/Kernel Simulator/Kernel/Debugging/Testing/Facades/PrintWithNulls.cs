
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

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class PrintWithNulls : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Print a string to console with null characters");
        public override void Run()
        {
            TextWriterColor.Write("Hello world!\nHow's your day going? \0Should be after this:\0\0\0", false, KernelColorType.Success);
            TextWriterColor.Write(" [{0}, {1}] ", true, KernelColorType.NeutralText, ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
        }
    }
}

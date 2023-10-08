
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

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class PrintWithNewLines : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Print a string to console with new lines in string");
        public override TestSection TestSection => TestSection.ConsoleBase;
        public override void Run()
        {
            TextWriterColor.WriteKernelColor("Hello world!\nHow's your day going?\nShould be directly after this:", false, KernelColorType.Success);
            TextWriterColor.WriteKernelColor(" [{0}, {1}] ", true, KernelColorType.NeutralText, ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
        }
    }
}

//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Terminaux.Colors.Themes.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Terminaux.Base;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class PrintWithNulls : TestFacade
    {
        public override string TestName => LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_PRINTWITHNULLS_DESC");
        public override TestSection TestSection => TestSection.ConsoleBase;
        public override void Run()
        {
            TextWriters.Write("Hello world!\nHow's your day going? \0Should be after this:\0\0\0", false, ThemeColorType.Success);
            TextWriters.Write(" [{0}, {1}] ", true, ThemeColorType.NeutralText, ConsoleWrapper.CursorLeft, ConsoleWrapper.CursorTop);
        }
    }
}

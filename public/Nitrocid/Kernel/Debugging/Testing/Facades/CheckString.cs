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
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class CheckString : TestFacade
    {
        public override string TestName => LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_CHECKSTRING_DESC");
        public override TestSection TestSection => TestSection.Languages;
        public override void Run()
        {
            string Text = "";
            if (string.IsNullOrEmpty(Text))
                Text = InputTools.ReadLine(LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_CHECKSTRING_PROMPT") + " ");
            var LocalizedStrings = LanguageManager.Languages["eng"].Strings;
            if (LocalizedStrings.ContainsKey(Text))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_CHECKSTRING_STRINGFOUND"), true, ThemeColorType.Success);
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_CHECKSTRING_STRINGNOTFOUND"));
            }
        }
    }
}

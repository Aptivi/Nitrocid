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

using Nitrocid.ConsoleBase.Inputs;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Terminaux.Colors;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class ColorTrueTest : TestFacade
    {
        public override string TestName => LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_COLORTRUETEST_DESC");
        public override TestSection TestSection => TestSection.ConsoleBase;
        public override void Run()
        {
            string TextR = "", TextG = "", TextB = "";
            if (string.IsNullOrEmpty(TextR))
                TextR = InputTools.ReadLine("R - " + LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_COLORTEST_PROMPT") + " ");
            if (string.IsNullOrEmpty(TextG))
                TextG = InputTools.ReadLine("G - " + LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_COLORTEST_PROMPT") + " ");
            if (string.IsNullOrEmpty(TextB))
                TextB = InputTools.ReadLine("B - " + LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_COLORTEST_PROMPT") + " ");
            if (int.TryParse(TextR, out int r) && int.TryParse(TextG, out int g) && int.TryParse(TextB, out int b))
            {
                var color = new Color(r, g, b);
                TextWriterColor.WriteColor("Color {0}", true, color, color.PlainSequence);
            }
        }
    }
}

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

using Nitrocid.ConsoleBase.Inputs;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Terminaux.Colors;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class ColorTrueTest : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests the VT sequence for true color");
        public override TestSection TestSection => TestSection.ConsoleBase;
        public override int TestOptionalParameters => 3;
        public override void Run(params string[] args)
        {
            string TextR = args.Length > 0 ? args[0] : "";
            string TextG = args.Length > 1 ? args[1] : "";
            string TextB = args.Length > 2 ? args[2] : "";
            if (string.IsNullOrEmpty(TextR))
                TextR = InputTools.ReadLine("R - " + Translate.DoTranslation("Write a color number ranging from 1 to 255:") + " ");
            if (string.IsNullOrEmpty(TextG))
                TextG = InputTools.ReadLine("G - " + Translate.DoTranslation("Write a color number ranging from 1 to 255:") + " ");
            if (string.IsNullOrEmpty(TextB))
                TextB = InputTools.ReadLine("B - " + Translate.DoTranslation("Write a color number ranging from 1 to 255:") + " ");
            if (int.TryParse(TextR, out int r) && int.TryParse(TextG, out int g) && int.TryParse(TextB, out int b))
            {
                var color = new Color(r, g, b);
                TextWriterColor.WriteColor("Color {0}", true, color, color.PlainSequence);
            }
        }
    }
}

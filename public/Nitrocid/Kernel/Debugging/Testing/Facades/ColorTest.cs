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
    internal class ColorTest : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests the VT sequence for 255 colors");
        public override TestSection TestSection => TestSection.ConsoleBase;
        public override void Run()
        {
            string Text = "";
            if (string.IsNullOrEmpty(Text))
                Text = InputTools.ReadLine(Translate.DoTranslation("Write a color number ranging from 1 to 255:") + " ");
            if (int.TryParse(Text, out int color))
            {
                var colorInstance = new Color(color);
                TextWriterColor.WriteColor("Color {0}", true, colorInstance, colorInstance.PlainSequence);
            }
        }
    }
}

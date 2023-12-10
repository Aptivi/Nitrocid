//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using KS.ConsoleBase.Inputs.Styles.Infobox;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;

namespace KS.Kernel.Debugging.Testing.Facades
{
    internal class TestInputInfoBoxButtons : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests the buttons in the informational box");
        public override TestSection TestSection => TestSection.ConsoleBase;
        public override void Run(params string[] args)
        {
            // Taken from https://en.wikipedia.org/wiki/Ubuntu_version_history
            var choices = new string[]
            {
                "20.04 (Focal Fossa)",
                "22.04 (Jammy Jellyfish)",
                "24.04 (Noble Numbat)",
            };
            int selected = InfoBoxButtonsColor.WriteInfoBoxButtons(choices, "Which Ubuntu version would you like to run?");
            TextWriterWhereColor.WriteWhere($"{selected}", 0, 0);
        }
    }
}

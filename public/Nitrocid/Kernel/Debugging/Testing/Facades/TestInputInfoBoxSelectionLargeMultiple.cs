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

using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using System.Collections.Generic;
using Terminaux.Inputs.Styles;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class TestInputInfoBoxSelectionLargeMultiple : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests the input multiple selection style in the informational box (large number of items to test scrolling and paging)");
        public override TestSection TestSection => TestSection.ConsoleBase;
        public override void Run()
        {
            var choices = new List<InputChoiceInfo>();
            for (int i = 0; i < 1000; i++)
                choices.Add(new InputChoiceInfo($"{i + 1}", $"Number #{i + 1}"));
            var selections = InfoBoxSelectionMultipleColor.WriteInfoBoxSelectionMultiple([.. choices], "Select a number");
            TextWriterWhereColor.WriteWhere(string.Join(", ", selections), 0, 0);
        }
    }
}

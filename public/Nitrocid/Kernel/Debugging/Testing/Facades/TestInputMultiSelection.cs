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

using Terminaux.Inputs.Styles.Selection;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using System.Collections.Generic;
using Terminaux.Inputs.Styles;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class TestInputMultiSelection : TestFacade
    {
        public override string TestName => LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_TESTFACADES_TESTINPUTMULTISELECTION_DESC");
        public override TestSection TestSection => TestSection.ConsoleBase;
        public override void Run()
        {
            // Taken from https://en.wikipedia.org/wiki/Ubuntu_version_history
            var choices = new List<InputChoiceInfo>()
            {
                new("focal", "20.04 (Focal Fossa)", "Ubuntu 20.04 LTS, codenamed Focal Fossa, is a long-term support release and was released on 23 April 2020."),
                new("jammy", "22.04 (Jammy Jellyfish)", "Ubuntu 22.04, codenamed Jammy Jellyfish, was released on 21 April 2022, and is a long-term support release, supported for five years, until April 2027."),
                new("noble", "24.04 (Noble Numbat)", "Ubuntu 24.04, codenamed Noble Numbat, is planned to be released on April 2024, and is a long-term support release, supported for five years, until April 2029."),
            };
            var altChoices = new List<InputChoiceInfo>()
            {
                new("lunar", "23.04 (Lunar Lobster)", "Ubuntu 23.04 Lunar Lobster is an interim release, released 20 April 2023."),
                new("mantic", "23.10 (Mantic Minotaur)", "Ubuntu 23.10 Mantic Minotaur is an interim release, scheduled 12 October 2023."),
            };
            var answers = SelectionMultipleStyle.PromptMultipleSelection("Which Ubuntu version would you like to run?", [.. choices], [.. altChoices]);
            TextWriterColor.Write(string.Join(", ", answers));
        }
    }
}

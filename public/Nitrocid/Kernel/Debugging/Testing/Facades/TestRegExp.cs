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
using System.Linq;
using System.Text.RegularExpressions;

namespace Nitrocid.Kernel.Debugging.Testing.Facades
{
    internal class TestRegExp : TestFacade
    {
        public override string TestName => Translate.DoTranslation("Tests the regular expression facility");
        public override TestSection TestSection => TestSection.Drivers;
        public override int TestOptionalParameters => 2;
        public override void Run(params string[] args)
        {
            string Text = args.Length > 0 ? args[0] : "";
            string Regex = args.Length > 1 ? args[1] : "";
            if (string.IsNullOrEmpty(Text))
                Text = InputTools.ReadLine(Translate.DoTranslation("Write a string to check:") + " ");
            if (string.IsNullOrEmpty(Regex))
                Regex = InputTools.ReadLine(Translate.DoTranslation("Write a regular expression:") + " ");
            var Reg = new Regex(Regex);
            var Matches = Reg.Matches(Text);
            int MatchNum = 1;
            foreach (Match Mat in Matches.Cast<Match>())
            {
                TextWriterColor.Write(Translate.DoTranslation("Match {0} ({1}): {2}"), MatchNum, Regex, Mat);
                MatchNum += 1;
            }
        }
    }
}

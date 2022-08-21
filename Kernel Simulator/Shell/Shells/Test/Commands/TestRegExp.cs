
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Text.RegularExpressions;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Test.Commands
{
    /// <summary>
    /// It lets you test the regular expression pattern on a specific string. It prints all matches.
    /// </summary>
    class Test_TestRegExpCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            string Exp = ListArgsOnly[0];
            var Reg = new Regex(Exp);
            var Matches = Reg.Matches(ListArgsOnly[1]);
            int MatchNum = 1;
            foreach (Match Mat in Matches)
            {
                TextWriterColor.Write(Translate.DoTranslation("Match {0} ({1}): {2}"), true, ColorTools.ColTypes.Neutral, MatchNum, Exp, Mat);
                MatchNum += 1;
            }
        }

    }
}
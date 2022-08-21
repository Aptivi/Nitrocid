using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

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

namespace KS.Shell.Shells.Test.Commands
{
    /// <summary>
    /// It lets you test the list writer using the Integer type.
    /// </summary>
    class Test_TestListWriterIntCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var NormalIntegerList = new List<int>() { 1, 2, 3 };
            var ArrayIntegerList = new List<int[]>() { { new int[] { 1, 2, 3 } }, { new int[] { 1, 2, 3 } }, { new int[] { 1, 2, 3 } } };
            TextWriterColor.Write(Translate.DoTranslation("Normal integer list:"), true, ColorTools.ColTypes.Neutral);
            ListWriterColor.WriteList(NormalIntegerList);
            TextWriterColor.Write(Translate.DoTranslation("Array integer list:"), true, ColorTools.ColTypes.Neutral);
            ListWriterColor.WriteList(ArrayIntegerList);
        }

    }
}

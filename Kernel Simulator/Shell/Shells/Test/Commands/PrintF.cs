
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

using System.Collections.Generic;
using KS.ConsoleBase.Colors;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Shell.Shells.Test.Commands
{
    /// <summary>
    /// It lets you test the <see cref="Write(string, bool, ColTypes, object[])"/> call to print every text, using the lines and colors that you need. It comes with format support.
    /// </summary>
    class Test_PrintFCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var Parts = new List<string>(ListArgsOnly);
            ColorTools.ColTypes Color = (ColorTools.ColTypes)Convert.ToInt32(ListArgsOnly[0]);
            bool Line = Convert.ToBoolean(ListArgsOnly[1]);
            object[] Vars = ListArgsOnly[2].Split(';');
            string Text = ListArgsOnly[3];
            TextWriterColor.Write(Text, Line, Color, Vars);
        }

    }
}


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

using System;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Test.Commands
{
    /// <summary>
    /// Checks how many milliseconds (or ticks if started with the -t switch) did it really take to sleep.
    /// </summary>
    class Test_SleepTookCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            int SleepMs = Convert.ToInt32(ListArgsOnly[0]);
            bool Ticks = ListSwitchesOnly.Contains("-t");
            if (Ticks)
            {
                TextWriterColor.Write("{0} ms => {1} ticks", SleepMs, ThreadManager.GetActualTicks(SleepMs));
            }
            else
            {
                TextWriterColor.Write("{0} ms => {1} ms", SleepMs, ThreadManager.GetActualMilliseconds(SleepMs));
            }
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("This command has the below switches that change how it works:"));
            TextWriterColor.Write("  -t: ", false, ColorTools.ColTypes.ListEntry);
            TextWriterColor.Write(Translate.DoTranslation("Unit in ticks"), true, ColorTools.ColTypes.ListValue);
        }

    }
}
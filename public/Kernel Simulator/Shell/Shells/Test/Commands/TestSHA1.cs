
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

using System.Diagnostics;
using KS.ConsoleBase.Colors;
using KS.Drivers.Encryption;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.Test.Commands
{
    /// <summary>
    /// It lets you estimate the time taken to encode a specified string on milliseconds using SHA1 algorithm.
    /// </summary>
    class Test_TestSHA1Command : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var spent = new Stopwatch();
            spent.Start(); // Time when you're on a breakpoint is counted
            TextWriterColor.Write(Encryption.GetEncryptedString(ListArgsOnly[0], Encryption.Algorithms.SHA1), true, ColorTools.ColTypes.NeutralText);
            TextWriterColor.Write(Translate.DoTranslation("Time spent: {0} milliseconds"), true, ColorTools.ColTypes.NeutralText, spent.ElapsedMilliseconds);
            spent.Stop();
        }

    }
}
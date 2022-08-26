
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
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Makes the PC speaker beep using specified time and frequency
    /// </summary>
    /// <remarks>
    /// This command lets you make a PC speaker beep using specified time in milliseconds and frequency in Hz. This requires that you have it installed.
    /// </remarks>
    class BeepCommand : CommandExecutor, ICommand
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "There is already a platform check in the command logic.")]
        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (StringQuery.IsStringNumeric(ListArgsOnly[0]) & Convert.ToInt32(ListArgsOnly[0]) >= 37 & Convert.ToInt32(ListArgsOnly[0]) <= 32767)
            {
                if (StringQuery.IsStringNumeric(ListArgsOnly[1]))
                {
                    if (KernelPlatform.IsOnWindows())
                    {
                        // TODO: Remove by Milestone 3
                        Console.Beep(Convert.ToInt32(ListArgsOnly[0]), Convert.ToInt32(ListArgsOnly[1]));
                    }
                    else
                    {
                        ConsoleBase.ConsoleWrapper.Beep();
                    }
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Time must be numeric."), true, ColorTools.ColTypes.Error);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Frequency must be numeric. If it's numeric, ensure that it is >= 37 and <= 32767."), true, ColorTools.ColTypes.Error);
            }
        }

    }
}

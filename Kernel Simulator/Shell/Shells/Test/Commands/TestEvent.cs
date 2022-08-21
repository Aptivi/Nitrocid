using System;
using KS.ConsoleBase.Colors;
using KS.Languages;
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

using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace KS.Shell.Shells.Test.Commands
{
    /// <summary>
    /// It lets you raise any event. If you have loaded mods, you can use this command for testing event raises.
    /// </summary>
    class Test_TestEventCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            try
            {
                string SubName = "Raise" + ListArgsOnly[0];
                Interaction.CallByName(new Kernel.Events.Events(), SubName, CallType.Method);
            }
            catch (Exception ex)
            {
                FileSystem.Write(Conversions.ToInteger(Translate.DoTranslation("Failure to raise event {0}: {1}")), true, ColorTools.ColTypes.Error, ListArgsOnly[0]);
            }
        }

    }
}
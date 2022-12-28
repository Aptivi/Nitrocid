
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Scripting.Conditions;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Tests the condition
    /// </summary>
    /// <remarks>
    /// Executes commands once the UESH conditions are satisfied
    /// </remarks>
    class IfCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            try
            {
                if (UESHConditional.ConditionSatisfied(ListArgsOnly[0]))
                {
                    string CommandString = string.Join(" ", ListArgsOnly.Skip(1).ToArray());
                    var AltThreads = ShellStart.ShellStack[ShellStart.ShellStack.Count - 1].AltCommandThreads;
                    if (AltThreads.Count == 0 || AltThreads[AltThreads.Count - 1].IsAlive)
                    {
                        var CommandThread = new KernelThread($"Alternative Shell Command Thread", false, (cmdThreadParams) => CommandExecutor.ExecuteCommand((CommandExecutor.ExecuteCommandParameters)cmdThreadParams));
                        ShellStart.ShellStack[ShellStart.ShellStack.Count - 1].AltCommandThreads.Add(CommandThread);
                    }
                    Shell.GetLine(CommandString);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to satisfy condition. See above for more information: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(Translate.DoTranslation("Failed to satisfy condition. More info here:") + " {0}", true, KernelColorType.Error, ex.Message);
            }
        }

    }
}

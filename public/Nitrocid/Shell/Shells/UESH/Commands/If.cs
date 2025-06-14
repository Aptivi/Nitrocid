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

using System;
using System.Linq;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Scripting.Conditions;
using Nitrocid.Shell.ShellBase.Shells;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Tests the condition
    /// </summary>
    /// <remarks>
    /// Executes commands once the UESH conditions are satisfied
    /// </remarks>
    class IfCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            try
            {
                if (UESHConditional.ConditionSatisfied(parameters.ArgumentsList[0]))
                {
                    string CommandString = string.Join(" ", parameters.ArgumentsList.Skip(1).ToArray());
                    var AltThreads = ShellManager.ShellStack[^1].AltCommandThreads;
                    if (AltThreads.Count == 0 || AltThreads[^1].IsAlive)
                    {
                        var CommandThread = new KernelThread($"Alternative Shell Command Thread", false, (cmdThreadParams) => CommandExecutor.ExecuteCommand((CommandExecutorParameters?)cmdThreadParams));
                        ShellManager.ShellStack[^1].AltCommandThreads.Add(CommandThread);
                    }
                    ShellManager.GetLine(CommandString);
                }
                return 0;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to satisfy condition. See above for more information: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_IF_CONDITIONUNSATISFIABLE") + " {0}", true, KernelColorType.Error, ex.Message);
                return ex.GetHashCode();
            }
        }

    }
}

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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Switches;
using System;
using System.Text;

namespace Nitrocid.Shell.ShellBase.Shells.Unified
{
    /// <summary>
    /// Pipes the command output to another command as parameters
    /// </summary>
    /// <remarks>
    /// If you want to redirect the command output as the other command's parameters, you can use this command.
    /// </remarks>
    class PipeUnifiedCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string sourceCommand = parameters.ArgumentsList[0];
            StringBuilder targetCommandBuilder = new(parameters.ArgumentsList[1] + " ");
            bool quoted = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-quoted");

            // First, get the source command output
            var currentShell = ShellManager.ShellStack[^1];
            var currentType = currentShell.ShellType;
            bool buildingTarget = true;
            DebugWriter.WriteDebug(DebugLevel.I, $"Writing piped output to the buffer for {sourceCommand}...");
            try
            {
                // Execute the source command
                DebugWriter.WriteDebug(DebugLevel.I, $"Executing {sourceCommand} to the buffer for {currentType}...");
                string contents = CommandExecutor.BufferCommand(sourceCommand);
                variableValue = contents;
                buildingTarget = false;

                // Build the command based on the output and execute the target command
                DebugWriter.WriteDebug(DebugLevel.I, $"Executing {targetCommandBuilder} for {currentType} with contents {contents}...");
                targetCommandBuilder.Append(quoted ? $"\"{contents}\"" : contents);
                ShellManager.GetLine($"{targetCommandBuilder}", "", currentType, true, false);
            }
            catch (Exception ex)
            {
                if (buildingTarget)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, $"Execution of {sourceCommand} to the buffer failed.");
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_WRAP_SOURCEFAILED"), KernelColorType.Error);
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, $"Execution of {targetCommandBuilder} failed.");
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_WRAP_TARGETFAILED") + $"\n    {targetCommandBuilder}", KernelColorType.Error);
                }
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_WRAP_FAILED"), KernelColorType.Error);
                DebugWriter.WriteDebug(DebugLevel.E, $"Reason for failure: {ex.Message}.");
                DebugWriter.WriteDebugStackTrace(ex);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.ShellOperation);
            }
            return 0;
        }

    }
}

//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Nitrocid.Kernel.Threading;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Arguments;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Shell.ShellBase.Shells.Unified
{
    /// <summary>
    /// Repeats a command
    /// </summary>
    /// <remarks>
    /// You can repeat either the last command entered or the specified command.
    /// </remarks>
    class RepeatUnifiedCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string timesString = parameters.ArgumentsList[0];
            bool valid = uint.TryParse(timesString, out uint times);
            if (!valid)
            {
                TextWriters.Write(Translate.DoTranslation("Invalid number of times."), true, KernelColorType.Error);
                return 18;
            }

            // Get the command to be executed
            string lastCommand = ShellManager.lastCommand;
            if (parameters.ArgumentsList.Length > 1)
                lastCommand = parameters.ArgumentsList[1];

            // Check to see if we're trying to call repeat
            var argumentInfo = ArgumentsParser.ParseShellCommandArguments(lastCommand, ShellManager.CurrentShellType).total[0];
            if (argumentInfo.Command == "repeat")
            {
                TextWriters.Write(Translate.DoTranslation("Can't repeat self."), true, KernelColorType.Error);
                return 19;
            }

            // First, initialize the alternative command thread
            var AltThreads = ShellManager.ShellStack[^1].AltCommandThreads;
            if (AltThreads.Count == 0 || AltThreads[^1].IsAlive)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Making alt thread for repeated command {0}...", lastCommand);
                var WrappedCommand = new KernelThread($"Repeated Shell Command Thread", false, (cmdThreadParams) => CommandExecutor.ExecuteCommand((CommandExecutorParameters?)cmdThreadParams));
                ShellManager.ShellStack[^1].AltCommandThreads.Add(WrappedCommand);
            }

            // Now, execute the command n times
            for (uint i = 1; i <= times; i++)
                ShellManager.GetLine(lastCommand);
            return 0;
        }

    }
}

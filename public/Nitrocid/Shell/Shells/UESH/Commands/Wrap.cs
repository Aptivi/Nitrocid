
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using KS.ConsoleBase.Colors;
using KS.Drivers;
using KS.Drivers.Console;
using KS.Drivers.Console.Consoles;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Wraps a command
    /// </summary>
    /// <remarks>
    /// You can wrap a command so it stops outputting until you press a key if the console has printed lines that exceed the console window height. Only the commands that are explicitly set to be wrappable can be used with this command.
    /// </remarks>
    class WrapCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            // TODO: Graduate this to a public API function if stability is proven
            var currentShell = ShellStart.ShellStack[^1];
            var currentType = currentShell.ShellType;
            var argumentInfo = new ProvidedCommandArgumentsInfo(StringArgs, currentType);
            string CommandToBeWrapped = argumentInfo.Command;

            // Check to see if the command is found
            if (!CommandManager.IsCommandFound(CommandToBeWrapped, currentType))
            {
                TextWriterColor.Write(Translate.DoTranslation("The wrappable command is not found."), true, KernelColorType.Error);
                return;
            }

            // Now, check to see if the command is wrappable
            if (!CommandManager.GetCommands(currentType)[CommandToBeWrapped].Flags.HasFlag(CommandFlags.Wrappable))
            {
                var WrappableCmds = new List<string>();
                foreach (CommandInfo CommandInfo in Shell.GetShellInfo(ShellType.Shell).Commands.Values)
                {
                    if (CommandInfo.Flags.HasFlag(CommandFlags.Wrappable))
                        WrappableCmds.Add(CommandInfo.Command);
                }
                TextWriterColor.Write(Translate.DoTranslation("The command is not wrappable. These commands are wrappable:"), true, KernelColorType.Error);
                ListWriterColor.WriteList(WrappableCmds);
                return;
            }

            bool buffered = false;
            try
            {
                // First, initialize the alternative command thread
                var AltThreads = ShellStart.ShellStack[^1].AltCommandThreads;
                if (AltThreads.Count == 0 || AltThreads[^1].IsAlive)
                {
                    var WrappedCommand = new KernelThread($"Wrapped Shell Command Thread", false, (cmdThreadParams) => CommandExecutor.ExecuteCommand((CommandExecutor.ExecuteCommandParameters)cmdThreadParams));
                    ShellStart.ShellStack[^1].AltCommandThreads.Add(WrappedCommand);
                }

                // Then, initialize the buffered writer and execute the commands
                DriverHandler.BeginLocalDriver<IConsoleDriver>("Buffered");
                Shell.GetLine(StringArgs, "", currentType, false);
                buffered = true;

                // Extract the buffer and then end the local driver
                var wrapBuffer = ((Buffered)DriverHandler.CurrentConsoleDriverLocal).consoleBuffer;
                var wrapOutput = wrapBuffer.ToString();
                DriverHandler.EndLocalDriver<IConsoleDriver>();

                // Now, print the output
                TextWriterWrappedColor.WriteWrapped(wrapOutput, false, KernelColorType.NeutralText);
                if (!wrapOutput.EndsWith(CharManager.NewLine))
                    TextWriterColor.Write();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to wrap command {0}: {1}", CommandToBeWrapped, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(Translate.DoTranslation("An error occurred while trying to wrap a command output") + ": {0}", true, KernelColorType.Error, ex.Message);
            }

            // In case error happens
            if (!buffered)
                DriverHandler.EndLocalDriver<IConsoleDriver>();
        }

        public override void HelpHelper()
        {
            // Get wrappable commands
            var WrappableCmds = new ArrayList();
            foreach (CommandInfo CommandInfo in Shell.GetShellInfo(ShellType.Shell).Commands.Values)
            {
                if (CommandInfo.Flags.HasFlag(CommandFlags.Wrappable))
                    WrappableCmds.Add(CommandInfo.Command);
            }

            // Print them along with help description
            TextWriterColor.Write(Translate.DoTranslation("Wrappable commands:") + " {0}", string.Join(", ", WrappableCmds.ToArray()));
        }

    }
}

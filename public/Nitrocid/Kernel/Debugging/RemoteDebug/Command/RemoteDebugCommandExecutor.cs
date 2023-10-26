//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Threading;
using KS.Languages;
using KS.Misc.Text;
using KS.Kernel.Events;
using KS.Kernel.Debugging.RemoteDebug.Command.BaseCommands;
using KS.Shell.ShellBase.Commands;
using KS.Kernel.Debugging.RemoteDebug.Command.Help;

namespace KS.Kernel.Debugging.RemoteDebug.Command
{
    /// <summary>
    /// Command parser module
    /// </summary>
    internal static class RemoteDebugCommandExecutor
    {

        internal static Dictionary<string, RemoteDebugCommandInfo> RemoteDebugCommands = new()
        {
            { "help", new RemoteDebugCommandInfo("help", /* Localizable */ "Help page", new RemoteDebugCommandArgumentInfo(new string[] { "[command]" }), new HelpCommand()) },
            { "register", new RemoteDebugCommandInfo("register", /* Localizable */ "Registers your name to your remote debug device", new RemoteDebugCommandArgumentInfo(new string[] { "<name>" }, true, 1), new RegisterCommand()) },
            { "exit", new RemoteDebugCommandInfo("exit", /* Localizable */ "Disconnects from the remote debugger", new RemoteDebugCommandArgumentInfo(), new ExitCommand()) },
            { "mutelogs", new RemoteDebugCommandInfo("mutelogs", /* Localizable */ "Mutes or unmutes the kernel logs", new RemoteDebugCommandArgumentInfo(), new MuteLogsCommand()) },
            { "trace", new RemoteDebugCommandInfo("trace", /* Localizable */ "Shows last stack trace on exception", new RemoteDebugCommandArgumentInfo(new string[] { "<tracenumber>" }, true, 1), new TraceCommand()) },
            { "username", new RemoteDebugCommandInfo("username", /* Localizable */ "Shows current username in the session", new RemoteDebugCommandArgumentInfo(), new UsernameCommand()) }
        };

        internal static void ExecuteCommand(string RequestedCommand, RemoteDebugDevice Device)
        {
            try
            {
                // Variables
                var ArgumentInfo = new RemoteDebugProvidedCommandArgumentInfo(RequestedCommand);
                string Command = ArgumentInfo.Command;
                var Args = ArgumentInfo.ArgumentsList;
                var Switches = ArgumentInfo.SwitchesList;
                string StrArgs = ArgumentInfo.ArgumentsText;
                bool RequiredArgumentsProvided = ArgumentInfo.RequiredArgumentsProvided;

                // Check to see if the command exists
                if (!RemoteDebugCommands.ContainsKey(Command))
                {
                    DebugWriter.WriteDebugDeviceOnly(DebugLevel.W, Translate.DoTranslation("Command not found."), true, Device);
                    return;
                }

                // If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                if (RemoteDebugCommands[Command].CommandArgumentInfo is not null)
                {
                    var ArgInfo = RemoteDebugCommands[Command].CommandArgumentInfo;
                    if (ArgInfo.ArgumentsRequired & RequiredArgumentsProvided | !ArgInfo.ArgumentsRequired)
                    {
                        var CommandBase = RemoteDebugCommands[Command].CommandBase;
                        CommandBase.Execute(StrArgs, Args, Switches, Device);
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "User hasn't provided enough arguments for {0}", Command);
                        DebugWriter.WriteDebugDeviceOnly(DebugLevel.W, Translate.DoTranslation("There were not enough arguments. See below for usage:"), true, Device);
                        RemoteDebugHelpPrint.ShowHelp(Command, Device);
                    }
                }
                else
                {
                    var CommandBase = RemoteDebugCommands[Command].CommandBase;
                    CommandBase.Execute(StrArgs, Args, Switches, Device);
                }
            }
            catch (ThreadInterruptedException)
            {
                CancellationHandlers.CancelRequested = false;
                return;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.RemoteDebugCommandError, RequestedCommand, ex);
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebugDeviceOnly(DebugLevel.E, Translate.DoTranslation("Error trying to execute command") + " {2}." + CharManager.NewLine + Translate.DoTranslation("Error {0}: {1}"), true, Device, ex.GetType().FullName, ex.Message, RequestedCommand);
            }
        }

    }
}

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
using System.Collections.Generic;
using System.Threading;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Kernel.Debugging.RemoteDebug.Command.BaseCommands;
using Textify.General;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Events;
using Nitrocid.Base.Kernel.Debugging.RemoteDebug.Command.Help;

namespace Nitrocid.Base.Kernel.Debugging.RemoteDebug.Command
{
    /// <summary>
    /// Command parser module
    /// </summary>
    internal static class RemoteDebugCommandExecutor
    {

        internal static Dictionary<string, RemoteDebugCommandInfo> RemoteDebugCommands = new()
        {
            { "help", new RemoteDebugCommandInfo("help", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_COMMAND_HELP_DESC"), new RemoteDebugCommandArgumentInfo(["[command]"]), new HelpCommand()) },
            { "register", new RemoteDebugCommandInfo("register", LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_REMOTEDEBUG_COMMAND_REGISTER_DESC"), new RemoteDebugCommandArgumentInfo(["<name>"], true, 1), new RegisterCommand()) },
            { "exit", new RemoteDebugCommandInfo("exit", LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_REMOTEDEBUG_COMMAND_EXIT_DESC"), new RemoteDebugCommandArgumentInfo(), new ExitCommand()) },
            { "mutelogs", new RemoteDebugCommandInfo("mutelogs", LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_REMOTEDEBUG_COMMAND_MUTELOGS_DESC"), new RemoteDebugCommandArgumentInfo(), new MuteLogsCommand()) },
            { "trace", new RemoteDebugCommandInfo("trace", LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_REMOTEDEBUG_COMMAND_TRACE_DESC"), new RemoteDebugCommandArgumentInfo(["<tracenumber>"], true, 1), new TraceCommand()) },
            { "username", new RemoteDebugCommandInfo("username", LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_REMOTEDEBUG_COMMAND_USERNAME_DESC"), new RemoteDebugCommandArgumentInfo(), new UsernameCommand()) }
        };

        internal static void ExecuteCommand(string RequestedCommand, RemoteDebugDevice Device)
        {
            try
            {
                // Variables
                var ArgumentInfo = new RemoteDebugProvidedCommandArgumentInfo(RequestedCommand);
                string Command = ArgumentInfo.Command;
                var Args = ArgumentInfo.ArgumentsList;
                var ArgsOrig = ArgumentInfo.ArgumentsListOrig;
                string StrArgs = ArgumentInfo.ArgumentsText;
                string StrArgsOrig = ArgumentInfo.ArgumentsTextOrig;
                var Switches = ArgumentInfo.SwitchesList;
                bool RequiredArgumentsProvided = ArgumentInfo.RequiredArgumentsProvided;

                // Check to see if the command exists
                if (!RemoteDebugCommands.TryGetValue(Command, out RemoteDebugCommandInfo? rdci))
                {
                    DebugWriter.WriteDebugDeviceOnly(DebugLevel.W, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_EXCEPTION_NOTFOUND"), true, Device);
                    return;
                }

                // Make the command parameters class
                var parameters = new RemoteDebugCommandParameters(StrArgs, Args, StrArgsOrig, ArgsOrig, Switches, Command);

                // If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                if (rdci.CommandArgumentInfo is not null)
                {
                    var ArgInfo = rdci.CommandArgumentInfo;
                    if (ArgInfo.ArgumentsRequired & RequiredArgumentsProvided | !ArgInfo.ArgumentsRequired)
                    {
                        var CommandBase = rdci.CommandBase;
                        CommandBase.Execute(parameters, Device);
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "User hasn't provided enough arguments for {0}", vars: [Command]);
                        DebugWriter.WriteDebugDeviceOnly(DebugLevel.W, LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_REMOTEDEBUG_NOTENOUGHARGS"), true, Device);
                        RemoteDebugHelpPrint.ShowHelp(Command, Device);
                    }
                }
                else
                {
                    var CommandBase = rdci.CommandBase;
                    CommandBase.Execute(parameters, Device);
                }
            }
            catch (ThreadInterruptedException)
            {
                CancellationHandlers.DismissRequest();
                return;
            }
            catch (Exception ex)
            {
                EventsManager.FireEvent(EventType.RemoteDebugCommandError, RequestedCommand, ex);
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebugDeviceOnly(DebugLevel.E, LanguageTools.GetLocalized("NKS_SHELL_BASE_COMMANDS_ERROREXECUTE1") + " {2}." + CharManager.NewLine + LanguageTools.GetLocalized("NKS_COMMON_ERRORDESC"), true, Device, vars: [ex.GetType().FullName ?? "<null>", ex.Message, RequestedCommand]);
            }
        }

    }
}

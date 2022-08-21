
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
using System.Collections.Generic;
using System.IO;
using KS.Languages;
using KS.Network.RemoteDebug.Commands;
using KS.Network.RemoteDebug.Interface;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Network.RemoteDebug
{
    static class RemoteDebugCmd
    {

        public readonly static Dictionary<string, CommandInfo> DebugCommands = new Dictionary<string, CommandInfo>() { { "exit", new CommandInfo("exit", ShellType.RemoteDebugShell, "Disconnects you from the debugger", new CommandArgumentInfo(), new Debug_ExitCommand()) }, { "help", new CommandInfo("help", ShellType.RemoteDebugShell, "Shows help screen", new CommandArgumentInfo(new[] { "[command]" }, false, 0), new Debug_HelpCommand()) }, { "register", new CommandInfo("register", ShellType.RemoteDebugShell, "Sets device username", new CommandArgumentInfo(new[] { "<username>" }, true, 1), new Debug_RegisterCommand()) }, { "trace", new CommandInfo("trace", ShellType.RemoteDebugShell, "Shows last stack trace on exception", new CommandArgumentInfo(new[] { "<tracenumber>" }, true, 1), new Debug_TraceCommand()) }, { "username", new CommandInfo("username", ShellType.RemoteDebugShell, "Shows current username in the session", new CommandArgumentInfo(), new Debug_UsernameCommand()) } };
        internal readonly static Dictionary<string, CommandInfo> DebugModCmds = new Dictionary<string, CommandInfo>();

        /// <summary>
        /// Client command parsing.
        /// </summary>
        /// <param name="CmdString">A specified command. It may contain arguments.</param>
        /// <param name="SocketStreamWriter">A socket stream writer</param>
        /// <param name="Address">An IP address</param>
        public static void ParseCmd(string CmdString, StreamWriter SocketStreamWriter, string Address)
        {
            Kernel.Kernel.KernelEventManager.RaiseRemoteDebugExecuteCommand(Address, CmdString);
            var ArgumentInfo = new ProvidedCommandArgumentsInfo(CmdString, ShellType.RemoteDebugShell);
            string Command = ArgumentInfo.Command;
            var Args = ArgumentInfo.ArgumentsList;
            var Switches = ArgumentInfo.SwitchesList;
            string StrArgs = ArgumentInfo.ArgumentsText;
            bool RequiredArgumentsProvided = ArgumentInfo.RequiredArgumentsProvided;

            try
            {
                RemoteDebugCommandExecutor DebugCommandBase = (RemoteDebugCommandExecutor)DebugCommands[Command].CommandBase;
                DebugCommandBase.Execute(StrArgs, Args, Switches, SocketStreamWriter, Address);
            }
            catch (Exception ex)
            {
                SocketStreamWriter.WriteLine(Translate.DoTranslation("Error executing remote debug command {0}: {1}"), Command, ex.Message);
                Kernel.Kernel.KernelEventManager.RaiseRemoteDebugCommandError(Address, CmdString, ex);
            }
        }
    }
}
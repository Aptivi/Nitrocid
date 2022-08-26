
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
using KS.Kernel.Debugging.RemoteDebug.Commands;
using KS.Kernel.Debugging.RemoteDebug.Interface;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Kernel.Debugging.RemoteDebug
{
    static class RemoteDebugCmd
    {

        public readonly static Dictionary<string, CommandInfo> DebugCommands = new()
        {
            { "exit", new CommandInfo("exit", ShellType.RemoteDebugShell, "Disconnects you from the debugger", new CommandArgumentInfo(), new Debug_ExitCommand()) },
            { "help", new CommandInfo("help", ShellType.RemoteDebugShell, "Shows help screen", new CommandArgumentInfo(new[] { "[command]" }, false, 0), new Debug_HelpCommand()) },
            { "register", new CommandInfo("register", ShellType.RemoteDebugShell, "Sets device username", new CommandArgumentInfo(new[] { "<username>" }, true, 1), new Debug_RegisterCommand()) },
            { "trace", new CommandInfo("trace", ShellType.RemoteDebugShell, "Shows last stack trace on exception", new CommandArgumentInfo(new[] { "<tracenumber>" }, true, 1), new Debug_TraceCommand()) },
            { "username", new CommandInfo("username", ShellType.RemoteDebugShell, "Shows current username in the session", new CommandArgumentInfo(), new Debug_UsernameCommand()) }
        };
        internal readonly static Dictionary<string, CommandInfo> DebugModCmds = new();
    }
}

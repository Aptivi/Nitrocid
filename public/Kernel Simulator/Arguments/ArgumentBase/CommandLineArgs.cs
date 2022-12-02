
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

using KS.Arguments.CommandLineArguments;
using KS.Shell.ShellBase.Commands;
using System.Collections.Generic;

namespace KS.Arguments.ArgumentBase
{
    /// <summary>
    /// Command line argument class
    /// </summary>
    public static class CommandLineArgs
    {

        /// <summary>
        /// Available command line arguments
        /// </summary>
        public readonly static Dictionary<string, ArgumentInfo> AvailableCMDLineArgs = new()
        {
            { "testInteractive", new ArgumentInfo("testInteractive", ArgumentType.CommandLineArgs, "Opens a test shell", new CommandArgumentInfo(), new CommandLine_TestInteractiveArgument()) },
            { "debug", new ArgumentInfo("debug", ArgumentType.CommandLineArgs, "Enables debug mode", new CommandArgumentInfo(), new CommandLine_DebugArgument()) },
            { "terminaldebug", new ArgumentInfo("terminaldebug", ArgumentType.CommandLineArgs, "Enables terminal debug mode", new CommandArgumentInfo(), new CommandLine_TerminalDebugArgument()) },
            { "args", new ArgumentInfo("args", ArgumentType.CommandLineArgs, "Prompts for arguments", new CommandArgumentInfo(), new CommandLine_ArgsArgument()) },
            { "reset", new ArgumentInfo("reset", ArgumentType.CommandLineArgs, "Resets the kernel to the factory settings", new CommandArgumentInfo(), new CommandLine_ResetArgument()) },
            { "newreader", new ArgumentInfo("newreader", ArgumentType.CommandLineArgs, "Opts in to new config reader", new CommandArgumentInfo(), new CommandLine_NewReaderArgument()) },
            { "newwriter", new ArgumentInfo("newwriter", ArgumentType.CommandLineArgs, "Opts in to new config writer", new CommandArgumentInfo(), new CommandLine_NewWriterArgument()) },
            { "newconfigpaths", new ArgumentInfo("newconfigpaths", ArgumentType.CommandLineArgs, "Opts in to new config paths", new CommandArgumentInfo(), new CommandLine_NewPathsArgument()) },
            { "bypasssizedetection", new ArgumentInfo("bypasssizedetection", ArgumentType.CommandLineArgs, "Bypasses the console size detection", new CommandArgumentInfo(), new CommandLine_BypassSizeDetectionArgument()) },
            { "help", new ArgumentInfo("help", ArgumentType.CommandLineArgs, "Help page", new CommandArgumentInfo(), new CommandLine_HelpArgument()) }
        };

    }
}

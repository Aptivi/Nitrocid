
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
            { "testInteractive", new ArgumentInfo("testInteractive", ArgumentType.CommandLineArgs, "Opens a test shell", "", false, 0, new CommandLine_TestInteractiveArgument()) },
            { "debug", new ArgumentInfo("debug", ArgumentType.CommandLineArgs, "Enables debug mode", "", false, 0, new CommandLine_DebugArgument()) },
            { "args", new ArgumentInfo("args", ArgumentType.CommandLineArgs, "Prompts for arguments", "", false, 0, new CommandLine_ArgsArgument()) },
            { "reset", new ArgumentInfo("reset", ArgumentType.CommandLineArgs, "Resets the kernel to the factory settings", "", false, 0, new CommandLine_ResetArgument()) },
            { "newreader", new ArgumentInfo("newreader", ArgumentType.CommandLineArgs, "Opts in to new config reader", "", false, 0, new CommandLine_NewReaderArgument()) },
            { "newwriter", new ArgumentInfo("newwriter", ArgumentType.CommandLineArgs, "Opts in to new config writer", "", false, 0, new CommandLine_NewWriterArgument()) },
            { "bypasssizedetection", new ArgumentInfo("bypasssizedetection", ArgumentType.CommandLineArgs, "Bypasses the console size detection", "", false, 0, new CommandLine_BypassSizeDetectionArgument()) },
            { "help", new ArgumentInfo("help", ArgumentType.CommandLineArgs, "Help page", "", false, 0, new CommandLine_HelpArgument()) }
        };

    }
}

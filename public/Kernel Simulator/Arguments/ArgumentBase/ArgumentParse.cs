
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
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;
using System;
using System.Collections.Generic;

namespace KS.Arguments.ArgumentBase
{
    /// <summary>
    /// Argument parser class
    /// </summary>
    public static class ArgumentParse
    {

        /// <summary>
        /// Available command line arguments
        /// </summary>
        public readonly static Dictionary<string, ArgumentInfo> AvailableCMDLineArgs = new()
        {
            { "quiet", new ArgumentInfo("quiet", "Starts the kernel quietly", new CommandArgumentInfo(), new QuietArgument()) },
            { "maintenance", new ArgumentInfo("maintenance", "Like safe mode, but also disables multi-user and some customization", new CommandArgumentInfo(), new MaintenanceArgument()) },
            { "safe", new ArgumentInfo("safe", "Starts the kernel in safe mode, disabling all mods", new CommandArgumentInfo(), new SafeArgument()) },
            { "testInteractive", new ArgumentInfo("testInteractive", "Opens a test shell", new CommandArgumentInfo(), new CommandLine_TestInteractiveArgument()) },
            { "debug", new ArgumentInfo("debug", "Enables debug mode", new CommandArgumentInfo(), new CommandLine_DebugArgument()) },
            { "terminaldebug", new ArgumentInfo("terminaldebug", "Enables terminal debug mode", new CommandArgumentInfo(), new CommandLine_TerminalDebugArgument()) },
            { "reset", new ArgumentInfo("reset", "Resets the kernel to the factory settings", new CommandArgumentInfo(), new CommandLine_ResetArgument()) },
            { "bypasssizedetection", new ArgumentInfo("bypasssizedetection", "Bypasses the console size detection", new CommandArgumentInfo(), new CommandLine_BypassSizeDetectionArgument()) },
            { "help", new ArgumentInfo("help", "Help page", new CommandArgumentInfo(), new CommandLine_HelpArgument()) }
        };

        /// <summary>
        /// Parses specified arguments
        /// </summary>
        /// <param name="ArgumentsInput">Input Arguments</param>
        public static void ParseArguments(List<string> ArgumentsInput)
        {
            // Check for the arguments written by the user
            try
            {
                var Arguments = AvailableCMDLineArgs;

                // Parse them now
                for (int i = 0; i <= ArgumentsInput.Count - 1; i++)
                {
                    string Argument = ArgumentsInput[i];
                    if (Arguments.ContainsKey(Argument))
                    {
                        // Variables
                        var ArgumentInfo = new ProvidedArgumentArgumentsInfo(Argument);
                        var Args = ArgumentInfo.ArgumentsList;
                        var Switches = ArgumentInfo.SwitchesList;
                        string strArgs = ArgumentInfo.ArgumentsText;
                        bool RequiredArgumentsProvided = ArgumentInfo.RequiredArgumentsProvided;

                        // If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                        if (Arguments[Argument].ArgArgumentInfo.ArgumentsRequired & RequiredArgumentsProvided | !Arguments[Argument].ArgArgumentInfo.ArgumentsRequired)
                        {
                            var ArgumentBase = Arguments[Argument].ArgumentBase;
                            ArgumentBase.Execute(strArgs, Args, Switches);
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "User hasn't provided enough arguments for {0}", Argument);
                            TextWriterColor.Write(Translate.DoTranslation("There was not enough arguments."));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                KernelPanic.KernelError(KernelErrorLevel.U, true, 5L, Translate.DoTranslation("Unrecoverable error in argument:") + " " + ex.Message, ex);
            }
        }

    }
}

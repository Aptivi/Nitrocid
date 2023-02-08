
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

using Extensification.StringExts;
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
            { "quiet", new ArgumentInfo("quiet", /* Localizable */ "Starts the kernel quietly", new CommandArgumentInfo(), new QuietArgument()) },
            { "maintenance", new ArgumentInfo("maintenance", /* Localizable */ "Like safe mode, but also disables multi-user and some customization", new CommandArgumentInfo(), new MaintenanceArgument()) },
            { "safe", new ArgumentInfo("safe", /* Localizable */ "Starts the kernel in safe mode, disabling all mods", new CommandArgumentInfo(), new SafeArgument()) },
            { "testInteractive", new ArgumentInfo("testInteractive", /* Localizable */ "Opens a test shell", new CommandArgumentInfo(), new TestInteractiveArgument()) },
            { "debug", new ArgumentInfo("debug", /* Localizable */ "Enables debug mode", new CommandArgumentInfo(), new DebugArgument()) },
            { "terminaldebug", new ArgumentInfo("terminaldebug", /* Localizable */ "Enables terminal debug mode", new CommandArgumentInfo(), new TerminalDebugArgument()) },
            { "reset", new ArgumentInfo("reset", /* Localizable */ "Resets the kernel to the factory settings", new CommandArgumentInfo(), new ResetArgument()) },
            { "bypasssizedetection", new ArgumentInfo("bypasssizedetection", /* Localizable */ "Bypasses the console size detection", new CommandArgumentInfo(), new BypassSizeDetectionArgument()) },
            { "noaltbuffer", new ArgumentInfo("noaltbuffer", /* Localizable */ "Prevents the kernel from using the alternative buffer", new CommandArgumentInfo(), new NoAltBufferArgument()) },
            { "lang", new ArgumentInfo("lang", /* Localizable */ "Sets the initial pre-boot environment language", new CommandArgumentInfo(new string[]{ "<lang>" }, true, 1), new LangArgument()) },
            { "help", new ArgumentInfo("help", /* Localizable */ "Help page", new CommandArgumentInfo(), new HelpArgument()) }
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
                    string ArgumentName = Argument.SplitSpacesEncloseDoubleQuotes()[0];
                    if (Arguments.ContainsKey(ArgumentName))
                    {
                        // Variables
                        var ArgumentInfo = new ProvidedArgumentArgumentsInfo(Argument);
                        var Arg = Arguments[ArgumentName];
                        var Args = ArgumentInfo.ArgumentsList;
                        var Switches = ArgumentInfo.SwitchesList;
                        string strArgs = ArgumentInfo.ArgumentsText;
                        bool RequiredArgumentsProvided = ArgumentInfo.RequiredArgumentsProvided;

                        // If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                        if (Arg.ArgArgumentInfo.ArgumentsRequired & RequiredArgumentsProvided | !Arg.ArgArgumentInfo.ArgumentsRequired)
                        {
                            var ArgumentBase = Arg.ArgumentBase;
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

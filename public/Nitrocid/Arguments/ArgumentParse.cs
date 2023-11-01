﻿//
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

using KS.Arguments.CommandLineArguments;
using KS.Arguments.Help;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using KS.Shell.ShellBase.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.Arguments
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
            { "quiet",
                new ArgumentInfo("quiet", /* Localizable */ "Starts the kernel quietly",
                    new[] {
                        new CommandArgumentInfo()
                    }, new QuietArgument())
            },

            { "maintenance",
                new ArgumentInfo("maintenance", /* Localizable */ "Like safe mode, but also disables multi-user and some customization",
                    new[] {
                        new CommandArgumentInfo()
                    }, new MaintenanceArgument())
            },

            { "safe",
                new ArgumentInfo("safe", /* Localizable */ "Starts the kernel in safe mode, disabling all mods",
                    new[] {
                        new CommandArgumentInfo()
                    }, new SafeArgument())
            },

            { "testInteractive",
                new ArgumentInfo("testInteractive", /* Localizable */ "Opens a test shell",
                    new[] {
                        new CommandArgumentInfo()
                    }, new TestInteractiveArgument())
            },

            { "debug",
                new ArgumentInfo("debug", /* Localizable */ "Enables debug mode",
                    new[] {
                        new CommandArgumentInfo()
                    }, new DebugArgument())
            },

            { "terminaldebug",
                new ArgumentInfo("terminaldebug", /* Localizable */ "Enables terminal debug mode",
                    new[] {
                        new CommandArgumentInfo()
                    }, new TerminalDebugArgument())
            },

            { "reset",
                new ArgumentInfo("reset", /* Localizable */ "Resets the kernel to the factory settings",
                    new[] {
                        new CommandArgumentInfo()
                    }, new ResetArgument())
            },

            { "bypasssizedetection",
                new ArgumentInfo("bypasssizedetection", /* Localizable */ "Bypasses the console size detection",
                    new[] {
                        new CommandArgumentInfo()
                    }, new BypassSizeDetectionArgument())
            },

            { "noaltbuffer",
                new ArgumentInfo("noaltbuffer", /* Localizable */ "Prevents the kernel from using the alternative buffer",
                    new[] {
                        new CommandArgumentInfo()
                    }, new NoAltBufferArgument())
            },

            { "lang",
                new ArgumentInfo("lang", /* Localizable */ "Sets the initial pre-boot environment language",
                    new[] {
                        new CommandArgumentInfo(new[]
                        {
                            new CommandArgumentPart(true, "lang")
                        })
                    }, new LangArgument())
            },

            { "attach",
                new ArgumentInfo("attach", /* Localizable */ "Attaches the Visual Studio debugger to this instance of Nitrocid",
                    new[] {
                        new CommandArgumentInfo()
                    }, new AttachArgument())
            },

            { "verbosepreboot",
                new ArgumentInfo("verbosepreboot", /* Localizable */ "Turns on verbose messages for pre-boot environment",
                    new[] {
                        new CommandArgumentInfo()
                    }, new VerbosePrebootArgument())
            },

            { "prebootsplash",
                new ArgumentInfo("prebootsplash", /* Localizable */ "Shows the pre-boot splash before configuration is loaded",
                    new[] {
                        new CommandArgumentInfo()
                    }, new PrebootSplashArgument())
            },
        };

        /// <summary>
        /// Parses specified arguments
        /// </summary>
        /// <param name="ArgumentsInput">Input Arguments</param>
        public static void ParseArguments(string[] ArgumentsInput)
        {
            // Check for the arguments written by the user
            try
            {
                ArgumentsInput = GetFilteredArguments(ArgumentsInput);
                var Arguments = AvailableCMDLineArgs;

                // Parse them now
                for (int i = 0; i <= ArgumentsInput.Length - 1; i++)
                {
                    string Argument = ArgumentsInput[i];
                    string ArgumentName = Argument.SplitEncloseDoubleQuotes()[0];
                    if (Arguments.ContainsKey(ArgumentName))
                    {
                        // Variables
                        var (satisfied, total) = ArgumentsParser.ParseArgumentArguments(Argument);
                        for (int j = 0; j < total.Length; j++)
                        {
                            var ArgumentInfo = total[j];
                            var Arg = Arguments[ArgumentName];
                            var Args = ArgumentInfo.ArgumentsList;
                            var Switches = ArgumentInfo.SwitchesList;
                            string strArgs = ArgumentInfo.ArgumentsText;
                            bool RequiredArgumentsProvided = ArgumentInfo.RequiredArgumentsProvided;

                            // If there are enough arguments provided, execute. Otherwise, fail with not enough arguments.
                            for (int idx = 0; idx < Arg.ArgArgumentInfo.Length; idx++)
                            {
                                var argInfo = Arg.ArgArgumentInfo[idx];
                                bool isLast = idx == Arg.ArgArgumentInfo.Length - 1 && j == total.Length - 1;
                                if (argInfo.ArgumentsRequired & RequiredArgumentsProvided | !argInfo.ArgumentsRequired)
                                {
                                    DebugWriter.WriteDebug(DebugLevel.I, "Executing argument {0} with args {1}...", Argument, strArgs);
                                    var ArgumentBase = Arg.ArgumentBase;
                                    ArgumentBase.Execute(strArgs, Args, Switches);
                                }
                                else if (isLast)
                                {
                                    DebugWriter.WriteDebug(DebugLevel.W, "User hasn't provided enough arguments for {0}", Argument);
                                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("There was not enough arguments."), true, KernelColorType.Error);
                                }
                            }
                        }
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "No such argument {0}", Argument);
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Unknown argument") + $" {Argument}", true, KernelColorType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                KernelPanic.KernelError(KernelErrorLevel.U, true, 5L, Translate.DoTranslation("Unrecoverable error in argument:") + " " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Checks to see if the specific argument name is passed
        /// </summary>
        /// <param name="ArgumentsInput">List of passed arguments</param>
        /// <param name="argumentName">Argument name to check</param>
        /// <returns>True if found in the arguments list and passed. False otherwise.</returns>
        public static bool IsArgumentPassed(string[] ArgumentsInput, string argumentName)
        {
            // Check for the arguments written by the user
            try
            {
                ArgumentsInput = GetFilteredArguments(ArgumentsInput);
                var Arguments = AvailableCMDLineArgs;

                // Parse them now
                bool found = false;
                for (int i = 0; i <= ArgumentsInput.Length - 1; i++)
                {
                    string Argument = ArgumentsInput[i];
                    string ArgumentName = Argument.SplitEncloseDoubleQuotes()[0];
                    found = ArgumentName == "help" || (ArgumentName == argumentName && Arguments.ContainsKey(ArgumentName));
                    if (found)
                        break;
                }
                return found;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Error trying to check for passed arguments: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                return false;
            }
        }

        private static string[] GetFilteredArguments(string[] ArgumentsInput)
        {
            if (ArgumentHelpPrint.acknowledged)
                ArgumentsInput = ArgumentsInput.Where((arg) => arg != "help").ToArray();
            return ArgumentsInput;
        }

    }
}
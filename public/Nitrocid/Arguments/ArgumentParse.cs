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

using Nitrocid.Arguments.CommandLineArguments;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Arguments;
using System;
using System.Collections.Generic;
using System.Text;
using Textify.General;

namespace Nitrocid.Arguments
{
    /// <summary>
    /// Argument parser class
    /// </summary>
    public static class ArgumentParse
    {

        internal readonly static Dictionary<string, ArgumentInfo> outArgs = new()
        {
            { "help",
                new ArgumentInfo("help", LanguageTools.GetLocalized("NKS_SHELL_SHELLS_COMMAND_HELP_DESC"),
                    [
                        new CommandArgumentInfo()
                    ], new HelpArgument())
            },

            { "version",
                new ArgumentInfo("version", LanguageTools.GetLocalized("NKS_ARGUMENTS_ARGUMENT_VERSION_DESC"),
                    [
                        new CommandArgumentInfo()
                    ], new VersionArgument())
            },

            { "apiversion",
                new ArgumentInfo("apiversion", LanguageTools.GetLocalized("NKS_ARGUMENTS_ARGUMENT_APIVERSION_DESC"),
                    [
                        new CommandArgumentInfo()
                    ], new ApiVersionArgument())
            },
        };

        private readonly static Dictionary<string, ArgumentInfo> args = new()
        {
            { "quiet",
                new ArgumentInfo("quiet", LanguageTools.GetLocalized("NKS_ARGUMENTS_ARGUMENT_QUIET_DESC"),
                    [
                        new CommandArgumentInfo()
                    ], new QuietArgument())
            },

            { "maintenance",
                new ArgumentInfo("maintenance", LanguageTools.GetLocalized("NKS_ARGUMENTS_ARGUMENT_MAINTENANCE_DESC"),
                    [
                        new CommandArgumentInfo()
                    ], new MaintenanceArgument())
            },

            { "safe",
                new ArgumentInfo("safe", LanguageTools.GetLocalized("NKS_ARGUMENTS_ARGUMENT_SAFE_DESC"),
                    [
                        new CommandArgumentInfo()
                    ], new SafeArgument())
            },

            { "testInteractive",
                new ArgumentInfo("testInteractive", LanguageTools.GetLocalized("NKS_ARGUMENTS_ARGUMENT_TESTINTERACTIVE_DESC"),
                    [
                        new CommandArgumentInfo()
                    ], new TestInteractiveArgument())
            },

            { "debug",
                new ArgumentInfo("debug", LanguageTools.GetLocalized("NKS_ARGUMENTS_ARGUMENT_DEBUG_DESC"),
                    [
                        new CommandArgumentInfo()
                    ], new DebugArgument())
            },

            { "terminaldebug",
                new ArgumentInfo("terminaldebug", LanguageTools.GetLocalized("NKS_ARGUMENTS_ARGUMENT_TERMINALDEBUG_DESC"),
                    [
                        new CommandArgumentInfo()
                    ], new TerminalDebugArgument())
            },

            { "reset",
                new ArgumentInfo("reset", LanguageTools.GetLocalized("NKS_ARGUMENTS_ARGUMENT_RESET_DESC"),
                    [
                        new CommandArgumentInfo()
                    ], new ResetArgument())
            },

            { "noaltbuffer",
                new ArgumentInfo("noaltbuffer", LanguageTools.GetLocalized("NKS_ARGUMENTS_ARGUMENT_NOALTBUFFER_DESC"),
                    [
                        new CommandArgumentInfo()
                    ], new NoAltBufferArgument())
            },

            { "lang",
                new ArgumentInfo("lang", LanguageTools.GetLocalized("NKS_ARGUMENTS_ARGUMENT_LANG_DESC"),
                    [
                        new CommandArgumentInfo(
                        [
                            new CommandArgumentPart(true, "lang")
                        ])
                    ], new LangArgument())
            },

            { "attach",
                new ArgumentInfo("attach", LanguageTools.GetLocalized("NKS_ARGUMENTS_ARGUMENT_ATTACH_DESC"),
                    [
                        new CommandArgumentInfo()
                    ], new AttachArgument())
            },

            { "verbosepreboot",
                new ArgumentInfo("verbosepreboot", LanguageTools.GetLocalized("NKS_ARGUMENTS_ARGUMENT_VERBOSEPREBOOT_DESC"),
                    [
                        new CommandArgumentInfo()
                    ], new VerbosePrebootArgument())
            },

            { "noprebootsplash",
                new ArgumentInfo("noprebootsplash", LanguageTools.GetLocalized("NKS_ARGUMENTS_ARGUMENT_NOPREBOOTSPLASH_DESC"),
                    [
                        new CommandArgumentInfo()
                    ], new NoPrebootSplashArgument())
            },
        };

        /// <summary>
        /// Available command line arguments
        /// </summary>
        public static Dictionary<string, ArgumentInfo> AvailableCMDLineArgs =>
            args;

        /// <summary>
        /// Parses specified arguments
        /// </summary>
        /// <param name="ArgumentsInput">Input Arguments</param>
        public static void ParseArguments(string[]? ArgumentsInput) =>
            ParseArguments(ArgumentsInput, false);

        internal static void ParseArguments(string[]? ArgumentsInput, bool earlyStage)
        {
            // Check for the arguments written by the user
            try
            {
                ArgumentsInput ??= [];
                var Arguments = earlyStage ? outArgs : AvailableCMDLineArgs;

                // Parse the arguments. Assume that unknown arguments are parameters of arguments
                string[] finalArguments = FinalizeArguments(ArgumentsInput, Arguments);

                // Parse them now
                for (int i = 0; i <= finalArguments.Length - 1; i++)
                {
                    string Argument = finalArguments[i];
                    string ArgumentName = Argument.SplitEncloseDoubleQuotes()[0];
                    if (Arguments.TryGetValue(ArgumentName, out ArgumentInfo? argInfoVal))
                    {
                        // Variables
                        var (satisfied, total) = ArgumentsParser.ParseArgumentArguments(Argument);
                        for (int j = 0; j < total.Length; j++)
                        {
                            var ArgumentInfo = total[j];
                            var Arg = argInfoVal;
                            var Args = ArgumentInfo.ArgumentsList;
                            var ArgOrig = ArgumentInfo.ArgumentsTextOrig;
                            var ArgsOrig = ArgumentInfo.ArgumentsListOrig;
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
                                    DebugWriter.WriteDebug(DebugLevel.I, "Executing argument {0} with args {1}...", vars: [Argument, strArgs]);

                                    // Prepare the argument parameter instance
                                    var parameters = new ArgumentParameters(strArgs, Args, ArgOrig, ArgsOrig, Switches, Argument);

                                    // Now, get the base command and execute it
                                    var ArgumentBase = Arg.ArgumentBase;
                                    ArgumentBase.Execute(parameters);
                                }
                                else if (isLast)
                                {
                                    DebugWriter.WriteDebug(DebugLevel.W, "User hasn't provided enough arguments for {0}", vars: [Argument]);
                                    TextWriters.Write(LanguageTools.GetLocalized("NKS_ARGUMENTS_NOTENOUGHARGS"), true, KernelColorType.Error);
                                }
                            }
                        }
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "No such argument {0}", vars: [Argument]);
                        TextWriters.Write(LanguageTools.GetLocalized("NKS_ARGUMENTS_UNKNOWNARGUMENT") + $" {Argument}", true, KernelColorType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                KernelPanic.KernelError(KernelErrorLevel.U, true, 5L, LanguageTools.GetLocalized("NKS_ARGUMENTS_UNRECOVERABLEERROR") + " " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Checks to see if the specific argument name is passed
        /// </summary>
        /// <param name="ArgumentsInput">List of passed arguments</param>
        /// <param name="argumentName">Argument name to check</param>
        /// <returns>True if found in the arguments list and passed. False otherwise.</returns>
        public static bool IsArgumentPassed(string[]? ArgumentsInput, string argumentName) =>
            IsArgumentPassed(ArgumentsInput, argumentName, false);

        internal static bool IsArgumentPassed(string[]? ArgumentsInput, string argumentName, bool earlyStage)
        {
            // Check for the arguments written by the user
            try
            {
                ArgumentsInput ??= [];
                var Arguments = earlyStage ? outArgs : AvailableCMDLineArgs;

                // Parse the arguments. Assume that unknown arguments are parameters of arguments
                string[] finalArguments = FinalizeArguments(ArgumentsInput, Arguments);

                // Parse them now
                bool found = false;
                for (int i = 0; i <= finalArguments.Length - 1; i++)
                {
                    string Argument = finalArguments[i];
                    string ArgumentName = Argument.SplitEncloseDoubleQuotes()[0];
                    found = ArgumentName == argumentName && Arguments.ContainsKey(ArgumentName);
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

        private static string[] FinalizeArguments(string[] argumentsInput, Dictionary<string, ArgumentInfo> arguments)
        {
            // Parse the arguments. Assume that unknown arguments are parameters of arguments
            List<string> finalArguments = [];
            StringBuilder builder = new();
            foreach (var argInput in argumentsInput)
            {
                if (arguments.TryGetValue(argInput, out ArgumentInfo? argInfoVal))
                {
                    // If we came across a valid argument, add the result and clear the builder
                    if (builder.Length > 0)
                    {
                        finalArguments.Add(builder.ToString().Trim());
                        builder.Clear();
                    }
                }

                // Add the argument name
                builder.Append(argInput + " ");
            }
            if (builder.Length > 0)
                finalArguments.Add(builder.ToString().Trim());
            return [.. finalArguments];
        }
    }
}

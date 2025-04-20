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

using Nitrocid.Arguments;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Text.Probers.Regexp;
using Nitrocid.Modifications;
using Nitrocid.Shell.ShellBase.Aliases;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Shell.ShellBase.Switches;
using System;
using System.Collections.Generic;
using System.Linq;
using Textify.General;

namespace Nitrocid.Shell.ShellBase.Arguments
{
    /// <summary>
    /// Argument parser tools
    /// </summary>
    public static class ArgumentsParser
    {
        /// <summary>
        /// Parses the shell command arguments
        /// </summary>
        /// <param name="CommandText">Command text that the user provided</param>
        /// <param name="CommandType">Shell command type. Consult the <see cref="ShellType"/> enum for information about supported shells.</param>
        /// <returns>An array of <see cref="ProvidedArgumentsInfo"/> that holds information about parsed command</returns>
        public static (ProvidedArgumentsInfo? satisfied, ProvidedArgumentsInfo[] total) ParseShellCommandArguments(string CommandText, ShellType CommandType) =>
            ParseShellCommandArguments(CommandText, null, ShellManager.GetShellTypeName(CommandType));

        /// <summary>
        /// Parses the shell command arguments
        /// </summary>
        /// <param name="CommandText">Command text that the user provided</param>
        /// <param name="CommandType">Shell command type.</param>
        /// <returns>An array of <see cref="ProvidedArgumentsInfo"/> that holds information about parsed command</returns>
        public static (ProvidedArgumentsInfo? satisfied, ProvidedArgumentsInfo[] total) ParseShellCommandArguments(string CommandText, string CommandType) =>
            ParseShellCommandArguments(CommandText, null, CommandType);

        /// <summary>
        /// Parses the shell command arguments
        /// </summary>
        /// <param name="CommandText">Command text that the user provided</param>
        /// <param name="cmdInfo">Command information</param>
        /// <param name="CommandType">Shell command type. Consult the <see cref="ShellType"/> enum for information about supported shells.</param>
        /// <returns>An array of <see cref="ProvidedArgumentsInfo"/> that holds information about parsed command</returns>
        public static (ProvidedArgumentsInfo? satisfied, ProvidedArgumentsInfo[] total) ParseShellCommandArguments(string CommandText, CommandInfo? cmdInfo, ShellType CommandType) =>
            ParseShellCommandArguments(CommandText, cmdInfo, ShellManager.GetShellTypeName(CommandType));

        /// <summary>
        /// Parses the shell command arguments
        /// </summary>
        /// <param name="CommandText">Command text that the user provided</param>
        /// <param name="cmdInfo">Command information</param>
        /// <param name="CommandType">Shell command type.</param>
        /// <returns>An array of <see cref="ProvidedArgumentsInfo"/> that holds information about parsed command</returns>
        public static (ProvidedArgumentsInfo? satisfied, ProvidedArgumentsInfo[] total) ParseShellCommandArguments(string CommandText, CommandInfo? cmdInfo, string CommandType)
        {
            string Command;
            CommandInfo[] ShellCommands;
            CommandInfo[] ModCommands;

            // Change the available commands list according to command type
            ShellCommands = CommandManager.GetCommands(CommandType);
            ModCommands = ModManager.ListModCommands(CommandType);

            // Split the requested command string into words
            var words = CommandText.SplitEncloseDoubleQuotes();
            var wordsOrig = CommandText.SplitEncloseDoubleQuotesNoRelease();
            string arguments = string.Join(' ', words.Skip(1));
            string argumentsOrig = string.Join(' ', wordsOrig.Skip(1));
            for (int i = 0; i <= words.Length - 1; i++)
                DebugWriter.WriteDebug(DebugLevel.I, "Word {0}: {1}", i + 1, words[i]);
            Command = words[0];

            // Check to see if the caller has provided a switch that subtracts the number of required arguments
            var aliases = AliasManager.GetEntireAliasListFromType(CommandType);
            var CommandInfo = ModCommands.Any((info) => info.Command == Command) ? ModCommands.Single((info) => info.Command == Command) :
                              ShellCommands.Any((info) => info.Command == Command) ? ShellCommands.Single((info) => info.Command == Command) :
                              aliases.Any((info) => info.Alias == Command) ? aliases.Single((info) => info.Alias == Command).TargetCommand :
                              cmdInfo;
            var fallback = new ProvidedArgumentsInfo(Command, arguments, words.Skip(1).ToArray(), argumentsOrig, wordsOrig.Skip(1).ToArray(), [], true, true, true, [], [], [], true, true, true, new());

            // Change the command if a command with no slash is entered on slash-enabled shells
            var shellInfo = ShellManager.GetShellInfo(CommandType);
            if (shellInfo.SlashCommand)
            {
                if (!CommandText.StartsWith('/'))
                {
                    // Change the command info to the non-slash one
                    CommandInfo = cmdInfo;
                }
                else
                {
                    // Strip the slash
                    CommandText = CommandText[1..].Trim();
                }
            }

            // Now, process the arguments
            if (CommandInfo != null)
                return ProcessArgumentOrShellCommandArguments(CommandText, CommandInfo, null);
            else
                return (fallback, new[] { fallback });
        }

        /// <summary>
        /// Parses the kernel argument arguments
        /// </summary>
        /// <param name="ArgumentText">Kernel argument text that the user provided</param>
        /// <returns>An array of <see cref="ProvidedArgumentsInfo"/> that holds information about parsed command</returns>
        public static (ProvidedArgumentsInfo? satisfied, ProvidedArgumentsInfo[] total) ParseArgumentArguments(string ArgumentText)
        {
            string Argument;
            var KernelArguments = ArgumentParse.AvailableCMDLineArgs;

            // Split the requested argument string into words
            var words = ArgumentText.SplitEncloseDoubleQuotes();
            var wordsOrig = ArgumentText.SplitEncloseDoubleQuotesNoRelease();
            string arguments = string.Join(' ', words.Skip(1));
            string argumentsOrig = string.Join(' ', wordsOrig.Skip(1));
            for (int i = 0; i <= words.Length - 1; i++)
                DebugWriter.WriteDebug(DebugLevel.I, "Word {0}: {1}", i + 1, words[i]);
            Argument = words[0];

            // Check to see if the caller has provided a switch that subtracts the number of required arguments
            var ArgumentInfo = KernelArguments.TryGetValue(Argument, out ArgumentInfo? argInfo) ? argInfo : null;
            var fallback = new ProvidedArgumentsInfo(Argument, arguments, words.Skip(1).ToArray(), argumentsOrig, wordsOrig.Skip(1).ToArray(), [], true, true, true, [], [], [], true, true, true, new());
            if (ArgumentInfo != null)
                return ProcessArgumentOrShellCommandArguments(ArgumentText, null, ArgumentInfo);
            else
                return (fallback, new[] { fallback });
        }

        private static (ProvidedArgumentsInfo? satisfied, ProvidedArgumentsInfo[] total) ProcessArgumentOrShellCommandArguments(string CommandText, CommandInfo? CommandInfo, ArgumentInfo? ArgumentInfo)
        {
            ProvidedArgumentsInfo? satisfiedArg = null;
            List<ProvidedArgumentsInfo> totalArgs = [];

            // Check the command and argument info
            bool isCommand = CommandInfo is not null;

            // Split the switches properly now
            string switchRegex =
                /* lang=regex */ @"((?<= )-\S+=((""(.+?)(?<![^\\]\\)"")|('(.+?)(?<![^\\]\\)')|(`(.+?)(?<![^\\]\\)`)|(?:[^\\\s]|\\.)+|\S+))|(?<= )-\S+";
            var EnclosedSwitches = RegexpTools
                .Matches(CommandText, switchRegex)
                .Select((match) => match.Value)
                .ToArray();
            CommandText = RegexpTools.Filter(CommandText, switchRegex);

            // Split the requested command string into words
            var words = CommandText.SplitEncloseDoubleQuotes();
            var wordsOrig = CommandText.SplitEncloseDoubleQuotesNoRelease();

            // Split the arguments with enclosed quotes
            var EnclosedArgMatches = words.Skip(1);
            var EnclosedArgMatchesOrig = wordsOrig.Skip(1);
            var EnclosedArgs = EnclosedArgMatches.ToArray();
            var EnclosedArgsOrig = EnclosedArgMatches.ToArray();
            DebugWriter.WriteDebug(DebugLevel.I, "{0} arguments parsed: {1}", EnclosedArgs.Length, string.Join(", ", EnclosedArgs));

            // Get the string of arguments
            string strArgs = words.Length > 0 ? string.Join(" ", EnclosedArgMatches) : "";
            string strArgsOrig = words.Length > 0 ? string.Join(" ", EnclosedArgMatchesOrig) : "";
            DebugWriter.WriteDebug(DebugLevel.I, "Finished strArgs: {0}", strArgs);
            DebugWriter.WriteDebug(DebugLevel.I, "Finished strArgsOrig: {0}", strArgsOrig);

            // Split the switches to their key-value counterparts
            var EnclosedSwitchKeyValuePairs = SwitchManager.GetSwitchValues(EnclosedSwitches, true);

            // Check to see if we're optionalizing some required arguments starting from the last required argument
            int minimumArgumentsOffset = 0;
            string[] unknownSwitchesList = [];
            string[] conflictingSwitchesList = [];
            string[] noValueSwitchesList = [];
            var argInfos = (isCommand ? CommandInfo?.CommandArgumentInfo : ArgumentInfo?.ArgArgumentInfo) ??
                throw new KernelException(KernelExceptionType.ShellOperation, Translate.DoTranslation("Can't get argument info for command or argument"));
            foreach (var argInfo in argInfos)
            {
                bool RequiredArgumentsProvided = true;
                bool RequiredSwitchesProvided = true;
                bool RequiredSwitchArgumentsProvided = true;
                bool numberProvided = true;
                bool switchNumberProvided = true;
                bool exactWordingProvided = true;

                // Check for argument info
                DebugWriter.WriteDebug(DebugLevel.I, "Argument info is full? {0}", argInfo is not null);

                // Optionalize some of the arguments if there are switches that optionalize them
                if (argInfo is not null)
                {
                    foreach (string enclosedSwitch in EnclosedSwitches)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Optionalizer is processing switch {0}...", enclosedSwitch);
                        var switches = argInfo.Switches.Where((switchInfo) => switchInfo.SwitchName == enclosedSwitch[1..]);
                        if (switches.Any())
                            foreach (var switchInfo in switches.Where(switchInfo => minimumArgumentsOffset < switchInfo.OptionalizeLastRequiredArguments))
                                minimumArgumentsOffset = switchInfo.OptionalizeLastRequiredArguments;
                        DebugWriter.WriteDebug(DebugLevel.I, "Minimum arguments offset is now {0}", minimumArgumentsOffset);
                    }
                }
                int finalRequiredArgs = argInfo is not null ? argInfo.MinimumArguments - minimumArgumentsOffset : 0;
                if (finalRequiredArgs < 0)
                    finalRequiredArgs = 0;
                DebugWriter.WriteDebug(DebugLevel.I, "Required arguments count is now {0}", finalRequiredArgs);

                // Check to see if the caller has provided required number of arguments
                if (argInfo is not null)
                    RequiredArgumentsProvided =
                        EnclosedArgs.Length >= finalRequiredArgs ||
                        !argInfo.ArgumentsRequired;
                else
                    RequiredArgumentsProvided = true;
                DebugWriter.WriteDebug(DebugLevel.I, "RequiredArgumentsProvided is {0}. Refer to the value of argument info.", RequiredArgumentsProvided);

                // Check to see if the caller has provided required number of switches
                if (argInfo is not null)
                    RequiredSwitchesProvided =
                        argInfo.Switches.Length == 0 ||
                        EnclosedSwitches.Length >= argInfo.Switches.Where((@switch) => @switch.IsRequired).Count() ||
                        !argInfo.Switches.Any((@switch) => @switch.IsRequired);
                else
                    RequiredSwitchesProvided = true;
                DebugWriter.WriteDebug(DebugLevel.I, "RequiredSwitchesProvided is {0}. Refer to the value of argument info.", RequiredSwitchesProvided);

                // Check to see if the caller has provided required number of switches that require arguments
                if (argInfo is not null)
                {
                    if (argInfo.Switches.Length == 0 || EnclosedSwitches.Length == 0 ||
                        !argInfo.Switches.Any((@switch) => @switch.ArgumentsRequired))
                        RequiredSwitchArgumentsProvided = true;
                    else
                    {
                        var allSwitches = argInfo.Switches.Where((@switch) => @switch.ArgumentsRequired).Select((@switch) => @switch.SwitchName).ToArray();
                        var allProvidedSwitches = EnclosedSwitches.Where((@switch) => allSwitches.Contains($"{@switch[1..]}")).ToArray();
                        foreach (var providedSwitch in allProvidedSwitches)
                        {
                            if (string.IsNullOrWhiteSpace(EnclosedSwitchKeyValuePairs.Single((kvp) => kvp.Item1 == providedSwitch).Item2))
                                RequiredSwitchArgumentsProvided = false;
                        }
                    }
                }
                else
                    RequiredSwitchArgumentsProvided = true;
                DebugWriter.WriteDebug(DebugLevel.I, "RequiredSwitchArgumentsProvided is {0}. Refer to the value of argument info.", RequiredSwitchArgumentsProvided);

                // Check to see if the caller has provided switches that don't accept values with the values
                if (argInfo is not null)
                {
                    var allSwitches = argInfo.Switches.Where((@switch) => !@switch.AcceptsValues).Select((@switch) => @switch.SwitchName).ToArray();
                    var allProvidedSwitches = EnclosedSwitches
                        .Where((@switch) => @switch.Contains('='))
                        .Where((@switch) => allSwitches.Contains($"{@switch[1..@switch.IndexOf('=')]}"))
                        .Select((@switch) => $"{@switch[..@switch.IndexOf('=')]}")
                        .ToArray();
                    List<string> rejected = [];
                    foreach (var providedSwitch in allProvidedSwitches)
                    {
                        if (!string.IsNullOrWhiteSpace(EnclosedSwitchKeyValuePairs.Single((kvp) => kvp.Item1 == providedSwitch).Item2))
                            rejected.Add(providedSwitch);
                    }
                    noValueSwitchesList = [.. rejected];
                }
                DebugWriter.WriteDebug(DebugLevel.I, "RequiredSwitchArgumentsProvided is {0}. Refer to the value of argument info.", RequiredSwitchArgumentsProvided);

                // Check to see if the caller has provided non-existent switches
                if (argInfo is not null)
                    unknownSwitchesList = EnclosedSwitchKeyValuePairs
                        .Select((kvp) => kvp.Item1)
                        .Where((key) => !argInfo.Switches.Any((switchInfo) => switchInfo.SwitchName == key[1..]))
                        .ToArray();
                DebugWriter.WriteDebug(DebugLevel.I, "Unknown switches: {0}", unknownSwitchesList.Length);

                // Check to see if the caller has provided conflicting switches
                if (argInfo is not null)
                {
                    List<string> processed = [];
                    List<string> conflicts = [];
                    foreach (var kvp in EnclosedSwitchKeyValuePairs)
                    {
                        // Check to see if the switch exists
                        string @switch = kvp.Item1;
                        if (unknownSwitchesList.Contains(@switch))
                            continue;
                        DebugWriter.WriteDebug(DebugLevel.I, "Processing switch: {0}", @switch);

                        // Get the switch and its conflicts list
                        var switchEnumerator = argInfo.Switches
                            .Where((switchInfo) => $"-{switchInfo.SwitchName}" == @switch);
                        if (switchEnumerator.Any())
                        {
                            // We have a switch! Now, process it.
                            var initialConflicts = switchEnumerator.First().ConflictsWith ?? [];
                            string[] switchConflicts = initialConflicts
                                .Select((conflicting) => $"-{conflicting}")
                                .ToArray();
                            DebugWriter.WriteDebug(DebugLevel.I, "Switch conflicts: {0} [{1}]", switchConflicts.Length, string.Join(", ", switchConflicts));

                            // Now, get the last switch and check to see if it's provided with the conflicting switch
                            string lastSwitch = processed.Count > 0 ? processed[^1] : "";
                            if (switchConflicts.Contains(lastSwitch))
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Conflict! {0} and {1} conflict with each other.", @switch, lastSwitch);
                                conflicts.Add($"{@switch} vs. {lastSwitch}");
                            }
                            processed.Add(@switch);
                            DebugWriter.WriteDebug(DebugLevel.I, "Marked conflicts: {0} [{1}]", conflicts.Count, string.Join(", ", conflicts));
                            DebugWriter.WriteDebug(DebugLevel.I, "Processed: {0} [{1}]", processed.Count, string.Join(", ", processed));
                        }
                    }
                    conflictingSwitchesList = [.. conflicts];
                }

                // Check to see if the caller has provided a non-numeric value to an argument that expects number
                if (argInfo is not null)
                {
                    for (int i = 0; i < argInfo.Arguments.Length && i < EnclosedArgs.Length; i++)
                    {
                        // Get the argument and the part
                        string arg = EnclosedArgs[i];
                        var argPart = argInfo.Arguments[i];

                        // Check to see if the argument expects a number and that the provided argument is numeric
                        // or if the argument allows string values
                        if (argPart.Options.IsNumeric && !double.TryParse(arg, out _))
                            numberProvided = false;
                    }
                }

                // Check to see if the caller has provided a wording other than the expected exact wording if found
                if (argInfo is not null)
                {
                    for (int i = 0; i < argInfo.Arguments.Length && i < EnclosedArgs.Length; i++)
                    {
                        // Get the argument and the part
                        string arg = EnclosedArgs[i];
                        var argPart = argInfo.Arguments[i];

                        // Check to see if the argument expects a number and that the provided argument is numeric
                        // or if the argument allows string values
                        if (argPart.Options.ExactWording.Length > 0 && !argPart.Options.ExactWording.Contains(arg))
                            exactWordingProvided = false;
                    }
                }

                // Check to see if the caller has provided a non-numeric value to a switch that expects numbers
                if (argInfo is not null)
                {
                    var switchesList = argInfo.Switches.Where((si) => si.SwitchName != "set").ToArray();
                    for (int i = 0; i < switchesList.Length && i < EnclosedSwitchKeyValuePairs.Count; i++)
                    {
                        // Get the switch and the part
                        var switches = EnclosedSwitchKeyValuePairs[i];
                        var switchPart = switchesList[i];

                        // Check to see if the switch expects a number and that the provided switch is numeric
                        // or if the switch allows string values
                        if (switchPart.IsNumeric && !double.TryParse(switches.Item2, out _))
                            switchNumberProvided = false;
                    }
                }

                // If all is well, bail.
                var paiInstance = new ProvidedArgumentsInfo
                (
                    words[0],
                    strArgs,
                    EnclosedArgs,
                    strArgsOrig,
                    EnclosedArgsOrig,
                    EnclosedSwitches,
                    RequiredArgumentsProvided,
                    RequiredSwitchesProvided,
                    RequiredSwitchArgumentsProvided,
                    unknownSwitchesList,
                    conflictingSwitchesList,
                    noValueSwitchesList,
                    numberProvided,
                    exactWordingProvided,
                    switchNumberProvided,
                    argInfo
                );
                if (RequiredArgumentsProvided &&
                    RequiredSwitchesProvided &&
                    RequiredSwitchArgumentsProvided &&
                    unknownSwitchesList.Length == 0 &&
                    conflictingSwitchesList.Length == 0 &&
                    numberProvided &&
                    exactWordingProvided &&
                    switchNumberProvided)
                    satisfiedArg = paiInstance;
                totalArgs.Add(paiInstance);
            }

            // Install the parsed values to the new class instance
            DebugWriter.WriteDebug(DebugLevel.I, "Finalizing...");
            return (satisfiedArg, totalArgs.ToArray());
        }
    }
}

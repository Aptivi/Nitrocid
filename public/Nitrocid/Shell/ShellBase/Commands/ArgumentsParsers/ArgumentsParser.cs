
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

using KS.Arguments.ArgumentBase;
using KS.Drivers;
using KS.Kernel.Debugging;
using KS.Misc.Text;
using KS.Modifications;
using KS.Shell.ShellBase.Aliases;
using KS.Shell.ShellBase.Shells;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KS.Shell.ShellBase.Commands.ArgumentsParsers
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
        /// <returns>An instance of <see cref="ProvidedArgumentsInfo"/> that holds information about parsed command</returns>
        public static ProvidedArgumentsInfo ParseShellCommandArguments(string CommandText, ShellType CommandType) =>
            ParseShellCommandArguments(CommandText, ShellManager.GetShellTypeName(CommandType));

        /// <summary>
        /// Parses the shell command arguments
        /// </summary>
        /// <param name="CommandText">Command text that the user provided</param>
        /// <param name="CommandType">Shell command type.</param>
        /// <returns>An instance of <see cref="ProvidedArgumentsInfo"/> that holds information about parsed command</returns>
        public static ProvidedArgumentsInfo ParseShellCommandArguments(string CommandText, string CommandType)
        {
            string Command;
            Dictionary<string, CommandInfo> ShellCommands;
            Dictionary<string, CommandInfo> ModCommands;

            // Change the available commands list according to command type
            ShellCommands = CommandManager.GetCommands(CommandType);
            ModCommands = ModManager.ListModCommands(CommandType);

            // Split the requested command string into words
            var words = CommandText.SplitEncloseDoubleQuotes();
            string arguments = string.Join(' ', words.Skip(1));
            for (int i = 0; i <= words.Length - 1; i++)
                DebugWriter.WriteDebug(DebugLevel.I, "Word {0}: {1}", i + 1, words[i]);
            Command = words[0];

            // Check to see if the caller has provided a switch that subtracts the number of required arguments
            var aliases = AliasManager.GetAliasesListFromType(CommandType);
            var CommandInfo = ModCommands.ContainsKey(Command) ? ModCommands[Command] :
                              ShellCommands.ContainsKey(Command) ? ShellCommands[Command] :
                              aliases.Any((kvp) => kvp.Key.Equals(Command)) ? ShellCommands[aliases.Single((kvp) => kvp.Key.Equals(Command)).Value] :
                              null;
            if (CommandInfo != null)
                return ProcessArgumentOrShellCommandArguments(CommandText, CommandInfo, null);
            else
                return new ProvidedArgumentsInfo(Command, arguments, words.Skip(1).ToArray(), Array.Empty<string>(), true, true, true, Array.Empty<string>(), Array.Empty<string>());
        }

        /// <summary>
        /// Parses the kernel argument arguments
        /// </summary>
        /// <param name="ArgumentText">Kernel argument text that the user provided</param>
        /// <returns>An instance of <see cref="ProvidedArgumentsInfo"/> that holds information about parsed command</returns>
        public static ProvidedArgumentsInfo ParseArgumentArguments(string ArgumentText)
        {
            string Argument;
            var KernelArguments = ArgumentParse.AvailableCMDLineArgs;

            // Split the requested argument string into words
            var words = ArgumentText.SplitEncloseDoubleQuotes();
            string arguments = string.Join(' ', words.Skip(1));
            for (int i = 0; i <= words.Length - 1; i++)
                DebugWriter.WriteDebug(DebugLevel.I, "Word {0}: {1}", i + 1, words[i]);
            Argument = words[0];

            // Check to see if the caller has provided a switch that subtracts the number of required arguments
            var ArgumentInfo = KernelArguments.ContainsKey(Argument) ? KernelArguments[Argument] : null;
            if (ArgumentInfo != null)
                return ProcessArgumentOrShellCommandArguments(ArgumentText, null, ArgumentInfo);
            else
                return new ProvidedArgumentsInfo(Argument, arguments, words.Skip(1).ToArray(), Array.Empty<string>(), true, true, true, Array.Empty<string>(), Array.Empty<string>());
        }

        private static ProvidedArgumentsInfo ProcessArgumentOrShellCommandArguments(string CommandText, CommandInfo CommandInfo, ArgumentInfo ArgumentInfo)
        {
            bool RequiredArgumentsProvided = true;
            bool RequiredSwitchesProvided = true;
            bool RequiredSwitchArgumentsProvided = true;

            // Check the command and argument info
            bool isCommand = CommandInfo is not null;

            // Split the switches properly now
            string switchRegex =
                /* lang=regex */ @"(-\S+=((""(.+?)(?<![^\\]\\)"")|('(.+?)(?<![^\\]\\)')|(`(.+?)(?<![^\\]\\)`)|(?:[^\\\s]|\\.)+|\S+))|(?<= )-\S+";
            var EnclosedSwitches = DriverHandler.CurrentRegexpDriverLocal
                .Matches(CommandText, switchRegex)
                .Select((match) => match.Value)
                .ToArray();
            CommandText = DriverHandler.CurrentRegexpDriverLocal.Filter(CommandText, switchRegex);

            // Split the requested command string into words
            var words = CommandText.SplitEncloseDoubleQuotes();

            // Split the arguments with enclosed quotes
            var EnclosedArgMatches = words.Skip(1);
            var EnclosedArgs = EnclosedArgMatches.ToArray();
            DebugWriter.WriteDebug(DebugLevel.I, "{0} arguments parsed: {1}", EnclosedArgs.Length, string.Join(", ", EnclosedArgs));

            // Get the string of arguments
            string strArgs = words.Length > 0 ? string.Join(" ", EnclosedArgMatches) : "";
            DebugWriter.WriteDebug(DebugLevel.I, "Finished strArgs: {0}", strArgs);

            // Split the switches to their key-value counterparts
            var EnclosedSwitchKeyValuePairs = SwitchManager.GetSwitchValues(EnclosedSwitches, true);

            // Check to see if we're optionalizing some required arguments starting from the last required argument
            int minimumArgumentsOffset = 0;
            string[] unknownSwitchesList = Array.Empty<string>();
            string[] conflictingSwitchesList = Array.Empty<string>();
            var argInfos = isCommand ? CommandInfo?.CommandArgumentInfo : ArgumentInfo?.ArgArgumentInfo;
            foreach (var argInfo in argInfos)
            {
                bool withArgInfo = argInfo is not null;
                DebugWriter.WriteDebug(DebugLevel.I, "Argument info is full? {0}", withArgInfo);
                if (withArgInfo)
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
                int finalRequiredArgs = withArgInfo ? argInfo.MinimumArguments - minimumArgumentsOffset : 0;
                if (finalRequiredArgs < 0)
                    finalRequiredArgs = 0;
                DebugWriter.WriteDebug(DebugLevel.I, "Required arguments count is now {0}", finalRequiredArgs);

                // Check to see if the caller has provided required number of arguments
                if (withArgInfo)
                    RequiredArgumentsProvided =
                        EnclosedArgs.Length >= finalRequiredArgs ||
                        !argInfo.ArgumentsRequired;
                else
                    RequiredArgumentsProvided = true;
                DebugWriter.WriteDebug(DebugLevel.I, "RequiredArgumentsProvided is {0}. Refer to the value of argument info.", RequiredArgumentsProvided);

                // Check to see if the caller has provided required number of switches
                if (withArgInfo)
                    RequiredSwitchesProvided =
                        argInfo.Switches.Length == 0 ||
                        EnclosedSwitches.Length >= argInfo.Switches.Where((@switch) => @switch.IsRequired).Count() ||
                        !argInfo.Switches.Any((@switch) => @switch.IsRequired);
                else
                    RequiredSwitchesProvided = true;
                DebugWriter.WriteDebug(DebugLevel.I, "RequiredSwitchesProvided is {0}. Refer to the value of argument info.", RequiredSwitchesProvided);

                // Check to see if the caller has provided required number of switches that require arguments
                if (withArgInfo)
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

                // Check to see if the caller has provided non-existent switches
                if (withArgInfo)
                    unknownSwitchesList = EnclosedSwitchKeyValuePairs
                        .Select((kvp) => kvp.Item1)
                        .Where((key) => !argInfo.Switches.Any((switchInfo) => switchInfo.SwitchName == key[1..]))
                        .ToArray();
                DebugWriter.WriteDebug(DebugLevel.I, "Unknown switches: {0}", unknownSwitchesList.Length);

                // Check to see if the caller has provided conflicting switches
                if (withArgInfo)
                {
                    List<string> processed = new();
                    List<string> conflicts = new();
                    foreach (var kvp in EnclosedSwitchKeyValuePairs)
                    {
                        // Check to see if the switch exists
                        string @switch = kvp.Item1;
                        if (unknownSwitchesList.Contains(@switch))
                            continue;
                        DebugWriter.WriteDebug(DebugLevel.I, "Processing switch: {0}", @switch);

                        // Get the switch and its conflicts list
                        string[] switchConflicts = argInfo.Switches
                            .Where((switchInfo) => $"-{switchInfo.SwitchName}" == @switch)
                            .First().ConflictsWith
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
                    conflictingSwitchesList = conflicts.ToArray();
                }

                // If all is well, bail.
                if (RequiredArgumentsProvided && RequiredSwitchesProvided && RequiredSwitchArgumentsProvided && unknownSwitchesList.Length == 0 && conflictingSwitchesList.Length == 0)
                    break;
            }

            // Install the parsed values to the new class instance
            DebugWriter.WriteDebug(DebugLevel.I, "Finalizing...");
            return new ProvidedArgumentsInfo(words[0], strArgs, EnclosedArgs, EnclosedSwitches, RequiredArgumentsProvided, RequiredSwitchesProvided, RequiredSwitchArgumentsProvided, unknownSwitchesList, conflictingSwitchesList);
        }
    }
}

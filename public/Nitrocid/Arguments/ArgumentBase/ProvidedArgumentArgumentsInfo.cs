
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

using System.Collections.Generic;
using KS.Drivers;
using System.Linq;
using KS.Kernel.Debugging;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;
using System;

namespace KS.Arguments.ArgumentBase
{
    /// <summary>
    /// Provided argument parameters information class
    /// </summary>
    public class ProvidedArgumentArgumentsInfo
    {

        internal string[] unknownSwitchesList;
        internal string[] conflictingSwitchesList;

        /// <summary>
        /// Target kernel argument that the user executed in shell
        /// </summary>
        public string Argument { get; private set; }
        /// <summary>
        /// Text version of the provided arguments and switches
        /// </summary>
        public string ArgumentsText { get; private set; }
        /// <summary>
        /// List version of the provided arguments and switches
        /// </summary>
        [Obsolete("To be removed in Beta 3.")]
        public string[] FullArgumentsList { get; private set; }
        /// <summary>
        /// List version of the provided arguments
        /// </summary>
        public string[] ArgumentsList { get; private set; }
        /// <summary>
        /// List version of the provided switches
        /// </summary>
        public string[] SwitchesList { get; private set; }
        /// <summary>
        /// Checks to see if the required arguments are provided
        /// </summary>
        public bool RequiredArgumentsProvided { get; private set; }

        /// <summary>
        /// Makes a new instance of the kernel argument argument info with the user-provided command text
        /// </summary>
        /// <param name="ArgumentText">Kernel argument text that the user provided</param>
        internal ProvidedArgumentArgumentsInfo(string ArgumentText)
        {
            string Argument;
            bool RequiredArgumentsProvided = true;
            bool RequiredSwitchesProvided = true;
            bool RequiredSwitchArgumentsProvided = true;
            var KernelArguments = ArgumentParse.AvailableCMDLineArgs;

            // Split the switches properly now
            string switchRegex =
                /* lang=regex */ @"(-\S+=((""(.+?)(?<![^\\]\\)"")|('(.+?)(?<![^\\]\\)')|(`(.+?)(?<![^\\]\\)`)|(?:[^\\\s]|\\.)+|\S+))|(?<= )-\S+";
            var EnclosedSwitches = DriverHandler.CurrentRegexpDriverLocal
                .Matches(ArgumentText, switchRegex)
                .Select((match) => match.Value)
                .ToArray();
            ArgumentText = DriverHandler.CurrentRegexpDriverLocal.Filter(ArgumentText, switchRegex);

            // Split the switches to their key-value counterparts
            var EnclosedSwitchKeyValuePairs = SwitchManager.GetSwitchValues(EnclosedSwitches, true);

            // Split the requested command string into words
            var words = ArgumentText.SplitEncloseDoubleQuotes();
            for (int i = 0; i <= words.Length - 1; i++)
                DebugWriter.WriteDebug(DebugLevel.I, "Word {0}: {1}", i + 1, words[i]);
            Argument = words[0];

            // Split the arguments with enclosed quotes
            var EnclosedArgMatches = words.Skip(1);
            var EnclosedArgs = EnclosedArgMatches.ToArray();
            DebugWriter.WriteDebug(DebugLevel.I, "{0} arguments parsed: {1}", EnclosedArgs.Length, string.Join(", ", EnclosedArgs));

            // Get the string of arguments
            string strArgs = words.Length > 0 ? string.Join(" ", EnclosedArgMatches) : "";
            DebugWriter.WriteDebug(DebugLevel.I, "Finished strArgs: {0}", strArgs);

            // Check to see if the caller has provided a switch that subtracts the number of required arguments
            var ArgumentInfo = KernelArguments.ContainsKey(Argument) ? KernelArguments[Argument] : null;
            int minimumArgumentsOffset = 0;
            if (ArgumentInfo?.ArgArgumentInfo is not null)
            {
                foreach (string enclosedSwitch in EnclosedSwitches)
                {
                    var switches = ArgumentInfo.ArgArgumentInfo.Switches.Where((switchInfo) => switchInfo.SwitchName == enclosedSwitch[1..]);
                    if (switches.Any())
                        foreach (var switchInfo in switches.Where(switchInfo => minimumArgumentsOffset < switchInfo.OptionalizeLastRequiredArguments))
                            minimumArgumentsOffset = switchInfo.OptionalizeLastRequiredArguments;
                }
            }
            int finalRequiredArgs = ArgumentInfo?.ArgArgumentInfo is not null ? ArgumentInfo.ArgArgumentInfo.MinimumArguments - minimumArgumentsOffset : 0;
            if (finalRequiredArgs < 0)
                finalRequiredArgs = 0;

            // Check to see if the caller has provided required number of arguments
            if (ArgumentInfo?.ArgArgumentInfo is not null)
                RequiredArgumentsProvided =
                    (EnclosedArgs.Length >= finalRequiredArgs) ||
                    !ArgumentInfo.ArgArgumentInfo.ArgumentsRequired;
            else
                RequiredArgumentsProvided = true;

            // Check to see if the caller has provided required number of switches
            if (ArgumentInfo?.ArgArgumentInfo is not null)
                RequiredSwitchesProvided =
                    ArgumentInfo.ArgArgumentInfo.Switches.Length == 0 ||
                    EnclosedSwitches.Length >= ArgumentInfo.ArgArgumentInfo.Switches.Where((@switch) => @switch.IsRequired).Count() ||
                    !ArgumentInfo.ArgArgumentInfo.Switches.Any((@switch) => @switch.IsRequired);
            else
                RequiredSwitchesProvided = true;

            // Check to see if the caller has provided required number of switches that require arguments
            if (ArgumentInfo?.ArgArgumentInfo is not null)
                RequiredSwitchArgumentsProvided =
                    ArgumentInfo.ArgArgumentInfo.Switches.Length == 0 ||
                    EnclosedSwitches.Length == 0 ||
                    EnclosedSwitchKeyValuePairs.Where((kvp) => !string.IsNullOrEmpty(kvp.Item2)).Count() >= ArgumentInfo.ArgArgumentInfo.Switches.Where((@switch) => @switch.ArgumentsRequired).Count() ||
                    !ArgumentInfo.ArgArgumentInfo.Switches.Any((@switch) => @switch.ArgumentsRequired);
            else
                RequiredSwitchArgumentsProvided = true;

            // Check to see if the caller has provided non-existent switches
            if (ArgumentInfo?.ArgArgumentInfo is not null)
                unknownSwitchesList = EnclosedSwitchKeyValuePairs
                    .Select((kvp) => kvp.Item1)
                    .Where((key) => !ArgumentInfo.ArgArgumentInfo.Switches.Any((switchInfo) => switchInfo.SwitchName == key[1..]))
                    .ToArray();

            // Check to see if the caller has provided conflicting switches
            if (ArgumentInfo?.ArgArgumentInfo is not null)
            {
                List<string> processed = new();
                List<string> conflicts = new();
                foreach (var kvp in EnclosedSwitchKeyValuePairs)
                {
                    // Check to see if the switch exists
                    string @switch = kvp.Item1;
                    if (unknownSwitchesList.Contains(@switch))
                        continue;

                    // Get the switch and its conflicts list
                    string[] switchConflicts = ArgumentInfo.ArgArgumentInfo.Switches
                        .Where((switchInfo) => $"-{switchInfo.SwitchName}" == @switch)
                        .First().ConflictsWith
                        .Select((conflicting) => $"-{conflicting}")
                        .ToArray();

                    // Now, get the last switch and check to see if it's provided with the conflicting switch
                    string lastSwitch = processed.Count > 0 ? processed[^1] : "";
                    if (switchConflicts.Contains(lastSwitch))
                        conflicts.Add($"{@switch} vs. {lastSwitch}");
                    processed.Add(@switch);
                }
                conflictingSwitchesList = conflicts.ToArray();
            }

            // Install the parsed values to the new class instance
            ArgumentsList = EnclosedArgs;
            SwitchesList = EnclosedSwitches;
            ArgumentsText = strArgs;
            this.Argument = Argument;
            this.RequiredArgumentsProvided = RequiredArgumentsProvided;
            // TODO: Implement these below on Beta 3.
            //this.RequiredSwitchesProvided = RequiredSwitchesProvided;
            //this.RequiredSwitchArgumentsProvided = RequiredSwitchArgumentsProvided;
        }

    }
}

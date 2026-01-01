//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using System.Collections.Generic;
using Textify.General;

namespace Nitrocid.Shell.ShellBase.Switches
{
    /// <summary>
    /// Switch management module
    /// </summary>
    public static class SwitchManager
    {
        /// <summary>
        /// Gets the switch values
        /// </summary>
        /// <returns>The list of switches, which start with a dash, with values supplied</returns>
        public static List<(string, string)> GetSwitchValues(string[] switches, bool includeNonValueSwitches = false)
        {
            List<(string, string)> switchValues = [];

            // Iterate through switches and check to see if there is an equal sign after the switch name
            foreach (string @switch in switches)
            {
                // Check the index to see if there is an equal sign
                int switchIndex = @switch.IndexOf('=');
                string switchName, switchValue;
                if (switchIndex == -1)
                {
                    if (includeNonValueSwitches)
                    {
                        // Assume that switch is the key name
                        switchName = @switch;
                        switchValue = "";
                    }
                    else
                        continue;
                }
                else
                {
                    // Get switch name and value. If the equal sign is at the end, the value is an empty value.
                    switchName = @switch[..switchIndex];
                    switchValue = switchIndex != @switch.Length - 1 ? @switch[(switchIndex + 1)..] : "";
                }

                // Add the values to the list
                switchValues.Add((switchName, switchValue));
            }

            // Return the final result
            return switchValues;
        }

        /// <summary>
        /// Gets the switch value
        /// </summary>
        /// <param name="switches">List of switches that start with the dash</param>
        /// <param name="switchKey">Switch key. Must begin with the dash before the switch name.</param>
        public static string GetSwitchValue(string[] switches, string switchKey)
        {
            var switchValues = GetSwitchValues(switches);
            return switchValues.Exists((tuple) => tuple.Item1 == switchKey) ?
                   switchValues.Find((tuple) => tuple.Item1 == switchKey).Item2 :
                   "";
        }

        /// <summary>
        /// Checks to see if the switch list contains a switch
        /// </summary>
        /// <param name="switches">List of switches that start with the dash</param>
        /// <param name="switchKey">Switch key. Must begin with the dash before the switch name.</param>
        public static bool ContainsSwitch(string[] switches, string switchKey)
        {
            var switchValues = GetSwitchValues(switches, true);
            return switchValues.Exists((tuple) => tuple.Item1 == switchKey);
        }

        /// <summary>
        /// Checks to see if the switch value of a specific switch is numeric
        /// </summary>
        /// <param name="switches">List of switches that start with the dash</param>
        /// <param name="switchKey">Switch key. Must begin with the dash before the switch name.</param>
        public static bool IsSwitchValueNumeric(string[] switches, string switchKey)
        {
            var switchValue = GetSwitchValue(switches, switchKey);
            return TextTools.IsStringNumeric(switchValue);
        }
    }
}

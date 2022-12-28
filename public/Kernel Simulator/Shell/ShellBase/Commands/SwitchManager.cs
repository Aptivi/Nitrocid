
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using System.Collections.Generic;

namespace KS.Shell.ShellBase.Commands
{
    /// <summary>
    /// Switch management module
    /// </summary>
    public static class SwitchManager
    {
        /// <summary>
        /// Gets the switch values
        /// </summary>
        /// <returns>The list of switches with values supplied</returns>
        public static List<(string, string)> GetSwitchValues(string[] switches)
        {
            List<(string, string)> switchValues = new();

            // Iterate through switches and check to see if there is an equal sign after the switch name
            foreach (string @switch in switches)
            {
                // Check the index to see if there is an equal sign
                int switchIndex = @switch.IndexOf('=');
                string switchName, switchValue;
                if (switchIndex == -1)
                    continue;

                // Get switch name and value. If the equal sign is at the end, the value is an empty value.
                switchName = @switch.Substring(0, switchIndex);
                switchValue = switchIndex != @switch.Length - 1 ? @switch.Substring(switchIndex + 1) : "";

                // Add the values to the list
                switchValues.Add((switchName, switchValue));
            }

            // Return the final result
            return switchValues;
        }

        /// <summary>
        /// Gets the switch value
        /// </summary>
        /// <param name="switches">List of switches</param>
        /// <param name="switchKey">Switch key</param>
        public static string GetSwitchValue(string[] switches, string switchKey)
        {
            var switchValues = GetSwitchValues(switches);
            return switchValues.Exists((tuple) => tuple.Item1 == switchKey) ?
                   switchValues.Find((tuple) => tuple.Item1 == switchKey).Item2 :
                   "";
        }
    }
}

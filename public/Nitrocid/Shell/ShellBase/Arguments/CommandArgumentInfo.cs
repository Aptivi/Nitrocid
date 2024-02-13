//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nitrocid.Shell.ShellBase.Switches;

namespace Nitrocid.Shell.ShellBase.Arguments
{
    /// <summary>
    /// Command argument info class
    /// </summary>
    public class CommandArgumentInfo
    {

        /// <summary>
        /// Does the command require arguments?
        /// </summary>
        public bool ArgumentsRequired =>
            Arguments.Any((part) => part.ArgumentRequired);
        /// <summary>
        /// User must specify at least this number of arguments
        /// </summary>
        public int MinimumArguments =>
            Arguments.Where((part) => part.ArgumentRequired).Count();
        /// <summary>
        /// Command arguments
        /// </summary>
        public CommandArgumentPart[] Arguments { get; private set; }
        /// <summary>
        /// Command switches
        /// </summary>
        public SwitchInfo[] Switches { get; private set; } = [
            new SwitchInfo("set", /* Localizable */ "Sets the value of the output to the selected UESH variable", false, true)
        ];
        /// <summary>
        /// Whether to accept the -set switch to set the UESH variable value
        /// </summary>
        public bool AcceptsSet { get; private set; }
        /// <summary>
        /// Rendered usage
        /// </summary>
        public string RenderedUsage
        {
            get
            {
                var usageBuilder = new StringBuilder();

                // Enumerate through the available switches first
                List<string> switchStrings = [];
                foreach (var Switch in Switches)
                {
                    bool required = Switch.IsRequired;
                    bool argRequired = Switch.ArgumentsRequired;
                    bool acceptsValue = Switch.AcceptsValues;
                    bool justNumeric = Switch.IsNumeric;
                    string switchName = Switch.SwitchName;

                    // If we're processing a conflicting switch, don't process it as we've already grouped them.
                    if (switchStrings.Contains(switchName))
                        continue;
                    else
                        switchStrings.Clear();

                    // Add the switch to the strings list in case we encounter a switch that conflicts with one or more switches.
                    switchStrings.Add(switchName);

                    // Check to see if there are any conflicts to put them to a group
                    string numericRender = justNumeric ? ":int" : "";
                    var conflicts = Switch.ConflictsWith;
                    if (conflicts.Length > 0)
                        switchStrings.AddRange(conflicts);
                    var switchLists = Switches
                        .Where((si) => switchStrings.Contains(si.SwitchName))
                        .ToArray();
                    var switchStringsUsages = switchLists
                        .Select((si) => $"-{si.SwitchName}{(si.IsRequired ? $"=value{numericRender}" : si.AcceptsValues ? $"[=value{numericRender}]" : "")}")
                        .ToArray();

                    // Now, render the switch usages
                    string renderedSwitchValue = argRequired ? $"=value{numericRender}" : acceptsValue ? $"[=value{numericRender}]" : "";
                    string requiredTagStart = required ? "<" : "[";
                    string requiredTagEnd = required ? ">" : "]";
                    string renderedSwitch =
                        conflicts.Length > 0 ?
                        $"{requiredTagStart}{string.Join("|", switchStringsUsages)}{requiredTagEnd} " :
                        $"{requiredTagStart}-{switchName}{renderedSwitchValue}{requiredTagEnd} ";
                    usageBuilder.Append(renderedSwitch);
                }

                // Enumerate through the available arguments
                foreach (var Argument in Arguments)
                {
                    bool required = Argument.ArgumentRequired;
                    bool justNumeric = Argument.IsNumeric;
                    bool hasWordingRequirement = Argument.ExactWording.Length > 0;
                    string requiredTagStart = hasWordingRequirement ? "" : required ? "<" : "[";
                    string requiredTagEnd = hasWordingRequirement ? "" : required ? ">" : "]";
                    string numericRender = justNumeric ? ":int" : "";
                    string renderedArgument =
                        $"{requiredTagStart}{Argument.ArgumentExpression}{numericRender}{requiredTagEnd} ";
                    usageBuilder.Append(renderedArgument);
                }

                return usageBuilder.ToString().Trim();
            }
        }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        public CommandArgumentInfo()
            : this([], [], false)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="AcceptsSet">Whether to accept the -set switch or not</param>
        public CommandArgumentInfo(bool AcceptsSet)
            : this([], [], AcceptsSet)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="Arguments">Command arguments</param>
        public CommandArgumentInfo(CommandArgumentPart[] Arguments)
            : this(Arguments, [], false)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="Arguments">Command arguments</param>
        /// <param name="AcceptsSet">Whether to accept the -set switch or not</param>
        public CommandArgumentInfo(CommandArgumentPart[] Arguments, bool AcceptsSet)
            : this(Arguments, [], AcceptsSet)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="Switches">Command switches</param>
        public CommandArgumentInfo(SwitchInfo[] Switches)
            : this([], Switches, false)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="Switches">Command switches</param>
        /// <param name="AcceptsSet">Whether to accept the -set switch or not</param>
        public CommandArgumentInfo(SwitchInfo[] Switches, bool AcceptsSet)
            : this([], Switches, AcceptsSet)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="Arguments">Command arguments</param>
        /// <param name="Switches">Command switches</param>
        public CommandArgumentInfo(CommandArgumentPart[] Arguments, SwitchInfo[] Switches)
            : this(Arguments, Switches, false)
        { }

        /// <summary>
        /// Installs a new instance of the command argument info class
        /// </summary>
        /// <param name="Arguments">Command arguments</param>
        /// <param name="Switches">Command switches</param>
        /// <param name="AcceptsSet">Whether to accept the -set switch or not</param>
        public CommandArgumentInfo(CommandArgumentPart[] Arguments, SwitchInfo[] Switches, bool AcceptsSet)
        {
            var finalArgs = Arguments ?? [];
            var finalSwitches = Switches ?? [];
            this.Arguments = finalArgs;
            if (AcceptsSet)
                this.Switches = this.Switches.Union(finalSwitches).ToArray();
            else
                this.Switches = finalSwitches;
            this.AcceptsSet = AcceptsSet;
        }

    }
}

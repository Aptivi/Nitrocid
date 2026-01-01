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

namespace Nitrocid.Shell.ShellBase.Switches
{
    /// <summary>
    /// Switch options for a switch
    /// </summary>
    public class SwitchOptions
    {
        private bool isRequiredSet;
        private bool argumentsRequiredSet;
        private bool acceptsValuesSet;
        private bool conflictsWithSet;
        private bool optionalizeLastRequiredArgumentsSet;
        private bool isNumericSet;
        private bool isRequired;
        private bool argumentsRequired;
        private bool acceptsValues = true;
        private string[] conflictsWith = [];
        private int optionalizeLastRequiredArguments;
        private bool isNumeric;

        /// <summary>
        /// Is the switch required?
        /// </summary>
        public bool IsRequired
        {
            get => isRequired;
            set
            {
                if (!isRequiredSet)
                {
                    isRequiredSet = true;
                    isRequired = value;
                }
            }
        }
        /// <summary>
        /// Does the switch require arguments?
        /// </summary>
        public bool ArgumentsRequired
        {
            get => argumentsRequired;
            set
            {
                if (!argumentsRequiredSet)
                {
                    argumentsRequiredSet = true;
                    argumentsRequired = value;
                }
            }
        }
        /// <summary>
        /// Does the switch accept values?
        /// </summary>
        public bool AcceptsValues
        {
            get => acceptsValues;
            set
            {
                if (!acceptsValuesSet)
                {
                    acceptsValuesSet = true;
                    acceptsValues = value;
                }
            }
        }
        /// <summary>
        /// Does the switch conflict with the provided switches?
        /// </summary>
        public string[]? ConflictsWith
        {
            get => conflictsWith;
            set
            {
                if (!conflictsWithSet)
                {
                    conflictsWithSet = true;
                    conflictsWith = value ?? [];
                }
            }
        }
        /// <summary>
        /// Whether to make the last N required arguments optional. This is useful for some switches, like -list.
        /// </summary>
        public int OptionalizeLastRequiredArguments
        {
            get => optionalizeLastRequiredArguments;
            set
            {
                if (!optionalizeLastRequiredArgumentsSet)
                {
                    optionalizeLastRequiredArgumentsSet = true;
                    optionalizeLastRequiredArguments = value;
                }
            }
        }
        /// <summary>
        /// Whether to make this switch only accept numbers
        /// </summary>
        public bool IsNumeric
        {
            get => isNumeric;
            set
            {
                if (!isNumericSet)
                {
                    isNumericSet = true;
                    isNumeric = value;
                }
            }
        }

        /// <summary>
        /// Makes a new instance of the switch options class
        /// </summary>
        public SwitchOptions()
        { }

    }
}

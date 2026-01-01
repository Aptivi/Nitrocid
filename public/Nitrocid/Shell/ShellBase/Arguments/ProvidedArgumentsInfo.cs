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

namespace Nitrocid.Shell.ShellBase.Arguments
{
    /// <summary>
    /// Provided arguments information class
    /// </summary>
    public class ProvidedArgumentsInfo
    {

        /// <summary>
        /// Target command or argument that the user executed in shell
        /// </summary>
        public string Command { get; private set; }
        /// <summary>
        /// Text version of the provided arguments and switches
        /// </summary>
        public string ArgumentsText { get; private set; }
        /// <summary>
        /// List version of the provided arguments
        /// </summary>
        public string[] ArgumentsList { get; private set; }
        /// <summary>
        /// Text version of the provided arguments and switches (original)
        /// </summary>
        public string ArgumentsTextOrig { get; private set; }
        /// <summary>
        /// List version of the provided arguments (original)
        /// </summary>
        public string[] ArgumentsListOrig { get; private set; }
        /// <summary>
        /// List version of the provided switches
        /// </summary>
        public string[] SwitchesList { get; private set; }
        /// <summary>
        /// Checks to see if the required arguments are provided
        /// </summary>
        public bool RequiredArgumentsProvided { get; private set; }
        /// <summary>
        /// Checks to see if the required switches are provided
        /// </summary>
        public bool RequiredSwitchesProvided { get; private set; }
        /// <summary>
        /// Checks to see if the required switch arguments for switches that require values are provided
        /// </summary>
        public bool RequiredSwitchArgumentsProvided { get; private set; }
        /// <summary>
        /// Checks to see if the number is provided for numeric argument. Also true if the argument doesn't expect a number.
        /// </summary>
        public bool NumberProvided { get; private set; }
        /// <summary>
        /// Checks to see if the exact wording is provided. Also true if the argument doesn't expect exact wording.
        /// </summary>
        public bool ExactWordingProvided { get; private set; }
        /// <summary>
        /// Checks to see if the number is provided for numeric switch. Also true if the switch doesn't expect a number.
        /// </summary>
        public bool SwitchNumberProvided { get; private set; }

        internal string[] UnknownSwitchesList { get; private set; }
        internal string[] ConflictingSwitchesList { get; private set; }
        internal string[] NoValueSwitchesList { get; private set; }
        internal CommandArgumentInfo? ArgumentInfo { get; private set; }

        internal ProvidedArgumentsInfo(string command, string argumentsText, string[] argumentsList, string argumentsTextOrig, string[] argumentsListOrig, string[] switchesList, bool requiredArgumentsProvided, bool requiredSwitchesProvided, bool requiredSwitchArgumentsProvided, string[] unknownSwitchesList, string[] conflictingSwitchesList, string[] noValueSwitchesList, bool numberProvided, bool exactWordingProvided, bool switchNumberProvided, CommandArgumentInfo? argumentInfo)
        {
            Command = command;
            ArgumentsText = argumentsText;
            ArgumentsList = argumentsList;
            ArgumentsTextOrig = argumentsTextOrig;
            ArgumentsListOrig = argumentsListOrig;
            SwitchesList = switchesList;
            RequiredArgumentsProvided = requiredArgumentsProvided;
            RequiredSwitchesProvided = requiredSwitchesProvided;
            RequiredSwitchArgumentsProvided = requiredSwitchArgumentsProvided;
            UnknownSwitchesList = unknownSwitchesList;
            ConflictingSwitchesList = conflictingSwitchesList;
            NoValueSwitchesList = noValueSwitchesList;
            NumberProvided = numberProvided;
            ExactWordingProvided = exactWordingProvided;
            ArgumentInfo = argumentInfo;
            SwitchNumberProvided = switchNumberProvided;
        }

    }
}

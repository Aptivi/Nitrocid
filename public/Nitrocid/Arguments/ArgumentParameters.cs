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

using System.Diagnostics;

namespace Nitrocid.Arguments
{
    /// <summary>
    /// Argument parameters that holds information about arguments and switches
    /// </summary>
    [DebuggerDisplay("Cmd = {ArgumentText}, Args = {ArgumentsText}")]
    public class ArgumentParameters
    {
        private readonly string stringArgument;
        private readonly string stringArgs;
        private readonly string[] listArgsOnly;
        private readonly string stringArgsOrig;
        private readonly string[] listArgsOnlyOrig;
        private readonly string[] listSwitchesOnly;

        /// <summary>
        /// Name of argument
        /// </summary>
        public string ArgumentText =>
            stringArgument;
        /// <summary>
        /// Text of arguments (filtered)
        /// </summary>
        public string ArgumentsText =>
            stringArgs;
        /// <summary>
        /// List of passed arguments (filtered)
        /// </summary>
        public string[] ArgumentsList =>
            listArgsOnly;
        /// <summary>
        /// Text of arguments (original)
        /// </summary>
        public string ArgumentsOriginalText =>
            stringArgsOrig;
        /// <summary>
        /// List of passed arguments (original)
        /// </summary>
        public string[] ArgumentsOriginalList =>
            listArgsOnlyOrig;
        /// <summary>
        /// List of passed switches
        /// </summary>
        public string[] SwitchesList =>
            listSwitchesOnly;

        internal ArgumentParameters(string stringArgs, string[] listArgsOnly, string stringArgsOrig, string[] listArgsOnlyOrig, string[] listSwitchesOnly, string argumentName)
        {
            this.stringArgs = stringArgs;
            this.listArgsOnly = listArgsOnly;
            this.stringArgsOrig = stringArgsOrig;
            this.listArgsOnlyOrig = listArgsOnlyOrig;
            this.listSwitchesOnly = listSwitchesOnly;
            stringArgument = argumentName;
        }
    }
}

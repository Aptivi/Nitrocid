
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

using KS.Shell.ShellBase.Help;

namespace KS.Arguments.Help
{
    /// <summary>
    /// Argument help system module
    /// </summary>
    public static class ArgumentHelpPrint
    {
        internal static bool acknowledged = false;

        /// <summary>
        /// Shows the help of an argument, or argument list if nothing is specified
        /// </summary>
        public static void ShowArgsHelp() =>
            ShowArgsHelp("");

        /// <summary>
        /// Shows the help of an argument, or argument list if nothing is specified
        /// </summary>
        /// <param name="Argument">A specified argument</param>
        public static void ShowArgsHelp(string Argument)
        {
            acknowledged = true;

            // Check to see if argument exists
            if (!string.IsNullOrWhiteSpace(Argument))
                ArgumentHelpPrintTools.ShowHelpUsage(Argument);
            else if (string.IsNullOrWhiteSpace(Argument))
            {
                // List the available arguments
                if (!HelpPrintTools.SimHelp)
                    ArgumentHelpPrintTools.ShowArgumentList();
                else
                    ArgumentHelpPrintTools.ShowArgumentListSimple();
            }
        }

    }
}

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

namespace Nitrocid.Shell.ShellBase.Commands
{
    /// <summary>
    /// Base command executor
    /// </summary>
    public interface ICommand
    {

        /// <summary>
        /// Executes the command with the given argument
        /// </summary>
        /// <param name="parameters">Command parameters including passed arguments and switches information</param>
        /// <param name="variableValue">Variable value to provide to target variable while -set is passed</param>
        /// <returns>Error code for the command</returns>
        int Execute(CommandParameters parameters, ref string variableValue);

        /// <summary>
        /// Executes the command with the given argument on dumb consoles
        /// </summary>
        /// <param name="parameters">Command parameters including passed arguments and switches information</param>
        /// <param name="variableValue">Variable value to provide to target variable while -set is passed</param>
        /// <returns>Error code for the command</returns>
        int ExecuteDumb(CommandParameters parameters, ref string variableValue);

        /// <summary>
        /// Shows additional information for the command when "help command" is invoked
        /// </summary>
        void HelpHelper();

    }
}

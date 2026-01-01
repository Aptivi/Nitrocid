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

using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Extras.JsonShell.Tools;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Extras.JsonShell.Json.Commands
{
    /// <summary>
    /// Prints a property or the whole file
    /// </summary>
    /// <remarks>
    /// You can use this command to print the contents of either the full JSON file or a property.
    /// </remarks>
    class PrintCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length > 0)
            {
                TextWriterColor.Write(JsonTools.SerializeToString(parameters.ArgumentsText));
            }
            else
            {
                TextWriterColor.Write(JsonTools.SerializeToString("$"));
            }
            return 0;
        }

    }
}

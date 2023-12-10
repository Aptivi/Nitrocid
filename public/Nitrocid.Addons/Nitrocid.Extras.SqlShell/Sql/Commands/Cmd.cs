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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;
using Microsoft.Data.Sqlite;
using Nitrocid.Extras.SqlShell.Tools;
using System.Collections.Generic;

namespace Nitrocid.Extras.SqlShell.Sql.Commands
{
    /// <summary>
    /// Executes a command
    /// </summary>
    /// <remarks>
    /// This command will execute any SQL query.
    /// </remarks>
    class CmdCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // First, check to see if we have parameters
            List<SqliteParameter> sqlParameters = [];
            foreach (string StringArg in parameters.ArgumentsList)
            {
                if (StringArg.StartsWith("@"))
                {
                    string paramValue = Input.ReadLine(TextTools.FormatString(Translate.DoTranslation("Enter parameter value for {0}:"), StringArg) + " ");
                    sqlParameters.Add(new SqliteParameter(StringArg, paramValue));
                }
            }

            // Now, get a group of replies and print them
            string[] replies = [];
            if (SqlEditTools.SqlEdit_SqlCommand(parameters.ArgumentsText, ref replies, [.. sqlParameters]))
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("SQL command succeeded. Here are the replies:"), true, KernelColorType.Success);
                foreach (string reply in replies)
                    TextWriterColor.WriteKernelColor(reply, true, KernelColorType.Success);
                return 0;
            }
            else
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("SQL command failed."), true, KernelColorType.Error);
                return 10000 + (int)KernelExceptionType.SqlEditor;
            }
        }
    }
}

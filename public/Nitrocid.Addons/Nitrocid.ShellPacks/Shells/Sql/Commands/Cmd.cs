//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Microsoft.Data.Sqlite;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.ShellPacks.Tools;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using System.Collections.Generic;
using Textify.General;
using Nitrocid.Base.ConsoleBase.Inputs;

namespace Nitrocid.ShellPacks.Shells.Sql.Commands
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
                    string paramValue = InputTools.ReadLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_PARAMVALUE_PROMPT").FormatString(StringArg) + " ");
                    sqlParameters.Add(new SqliteParameter(StringArg, paramValue));
                }
            }

            // Now, get a group of replies and print them
            string[] replies = [];
            if (SqlEditTools.SqlEdit_SqlCommand(parameters.ArgumentsText, ref replies, [.. sqlParameters]))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_COMMANDSUCCESS"), true, ThemeColorType.Success);
                foreach (string reply in replies)
                    TextWriterColor.Write(reply, true, ThemeColorType.Success);
                return 0;
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_COMMANDFAILURE"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.SqlEditor);
            }
        }
    }
}

//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Nitrocid.Extras.SqlShell.Tools;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Reader;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

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
        public override string Command =>
            "cmd";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_COMMAND_CMD_DESC");

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // First, check to see if we have parameters
            List<SqliteParameter> sqlParameters = [];
            foreach (string StringArg in parameters.ArgumentsList)
            {
                if (StringArg.StartsWith("@"))
                {
                    string paramValue = TermReader.Read(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_PARAMVALUE_PROMPT").FormatString(StringArg) + " ");
                    sqlParameters.Add(new SqliteParameter(StringArg, paramValue));
                }
            }

            // Now, get a group of replies and print them
            string[] replies = [];
            if (SqlEditTools.SqlEdit_SqlCommand(parameters.ArgumentsText, ref replies, out var error, [.. sqlParameters]))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_COMMANDSUCCESS"), true, ThemeColorType.Success);
                foreach (string reply in replies)
                    TextWriterColor.Write(reply, true, ThemeColorType.Success);
                return 0;
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SQL_COMMANDFAILURE"), true, ThemeColorType.Error);
                if (error is not null)
                    TextWriterColor.Write(error.Message, true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.SqlEditor);
            }
        }
    }
}

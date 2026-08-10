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

using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Journaling;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Base.Shell.Shells.Admin.Commands
{
    /// <summary>
    /// Gets the current kernel journal log
    /// </summary>
    /// <remarks>
    /// This command gets the current kernel journal log from the <see cref="KernelPathType.Journaling"/> path.
    /// </remarks>
    class JournalCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "journal";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_COMMAND_JOURNAL_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(false, "sessionNum", new()
                    {
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "NKS_SHELL_SHELLS_ADMIN_COMMAND_JOURNAL_ARGUMENT_SESSIONNUM_DESC"
                    }),
                ])
            ];

        public override CommandFlags Flags =>
            CommandFlags.Wrappable | CommandFlags.RedirectionSupported;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length > 0)
            {
                // Check to see if invalid number is provided
                string sessionNumStr = parameters.ArgumentsList[0];
                if (!int.TryParse(sessionNumStr, out int sessionNum))
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_JOURNAL_SESSIONNUMBERINVALID"), ThemeColorType.Error);
                    return KernelExceptionTools.GetErrorCode(KernelExceptionType.Journaling);
                }
                var entries = JournalManager.GetJournalEntries(sessionNum);
                JournalManager.PrintJournalLog(entries);
            }
            else
                JournalManager.PrintJournalLog();
            return 0;
        }
    }
}

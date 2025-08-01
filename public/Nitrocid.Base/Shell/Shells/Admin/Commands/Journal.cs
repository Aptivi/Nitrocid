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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Journaling;

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

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (parameters.ArgumentsList.Length > 0)
            {
                // Check to see if invalid number is provided
                if (!int.TryParse(parameters.ArgumentsList[0], out int sessionNum))
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

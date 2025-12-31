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

using System;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;

namespace Nitrocid.Base.Shell.Shells.Admin.Commands
{
    /// <summary>
    /// Clears debugging log
    /// </summary>
    /// <remarks>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class CdbgLogCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (KernelEntry.DebugMode)
            {
                try
                {
                    DebugWriter.RemoveDebugLogs();
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_CDBGLOG_SUCCESS"));
                    return 0;
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_CDBGLOG_FAILURE"), true, ThemeColorType.Error, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    return ex.GetHashCode();
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_CDBGLOG_NEEDSDEBUG"));
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Debug);
            }
        }

    }
}

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
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Shell.Shells.Admin.Commands
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
                    TextWriterColor.Write(Translate.DoTranslation("Debug log removed. All connected debugging devices may still view messages."));
                    return 0;
                }
                catch (Exception ex)
                {
                    TextWriters.Write(Translate.DoTranslation("Debug log removal failed: {0}"), true, KernelColorType.Error, ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    return ex.GetHashCode();
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("You must turn on debug mode before you can clear debug log."));
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Debug);
            }
        }

    }
}

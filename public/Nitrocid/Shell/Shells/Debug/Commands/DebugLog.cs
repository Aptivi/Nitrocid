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

using Terminaux.Shell.Commands;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Files.Paths;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Files;
using System;

namespace Nitrocid.Shell.Shells.Debug.Commands
{
    /// <summary>
    /// You can view the debug log of a session
    /// </summary>
    /// <remarks>
    /// This command lets you view the detailed debug log of a session by printing the contents of the whole file directly to the console. You'll most likely need to wrap the output.
    /// </remarks>
    class DebugLogCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Try to parse the session GUID.
            string sessionGuidStr = parameters.ArgumentsList[0];
            if (!Guid.TryParse(sessionGuidStr, out Guid sessionGuid))
            {
                // There is invalid session GUID being requested
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_DEBUGLOG_INVALIDGUID") + $" {sessionGuidStr}", true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Debug);
            }

            // Now, check to see if we have this session GUID. Get all the debug logs and compare.
            var debugs = FilesystemTools.GetFilesystemEntries(FilesystemTools.NeutralizePath(PathsManagement.AppDataPath + "/../Aptivi/Logs/") + "log_Nitrocid_*.txt");
            string finalDebug = "";
            foreach (string debug in debugs)
            {
                // Don't try to parse the rest of the files if we already have one.
                if (!string.IsNullOrEmpty(finalDebug))
                    break;

                // Check the debug path and compare it with the requested session GUID.
                if (debug.Contains($"{sessionGuid}"))
                    finalDebug = debug;
            }

            // Check to see if we really have the file path
            if (string.IsNullOrEmpty(finalDebug))
            {
                // There is no such session GUID being requested
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_DEBUG_DEBUGLOG_NOGUID") + $" {sessionGuidStr}", true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Debug);
            }

            // Get the contents of the file and write it to the console
            string contents = FilesystemTools.ReadContentsText(finalDebug);
            TextWriterColor.Write(contents);
            return 0;
        }

    }
}

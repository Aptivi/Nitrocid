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
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Threading;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using System;
using System.Collections.Generic;

namespace Nitrocid.Extras.Diagnostics.Commands
{
    /// <summary>
    /// Gets backtrace for all threads
    /// </summary>
    /// <remarks>
    /// This command will print backtrace information for all kernel threads
    /// </remarks>
    class ThreadsBtCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we're running on Windows 8.1 or later
            if (KernelPlatform.IsOnWindows() && !OperatingSystem.IsWindowsVersionAtLeast(6, 3))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DIAG_W8LOWER"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Debug);
            }

            // Print the list
            Dictionary<string, string[]> result = ThreadManager.GetThreadBacktraces();
            foreach (var trace in result)
            {
                string threadAddress = trace.Key;
                string[] threadTrace = trace.Value;
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DIAG_TRACEINFO") + "\n", true, ThemeColorType.ListTitle, threadAddress);
                ListWriterColor.WriteList(threadTrace);
            }
            return 0;
        }

        public override int ExecuteDumb(CommandParameters parameters, ref string variableValue)
        {
            // Check to see if we're running on Windows 8.1 or later
            if (KernelPlatform.IsOnWindows() && !OperatingSystem.IsWindowsVersionAtLeast(6, 3))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DIAG_W8LOWER"), true, ThemeColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Debug);
            }

            // Print the list in a dumb-friendly way
            Dictionary<string, string[]> result = ThreadManager.GetThreadBacktraces();
            foreach (var trace in result)
            {
                string threadAddress = trace.Key;
                string[] threadTrace = trace.Value;
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DIAG_TRACEINFO") + "\n", true, ThemeColorType.ListTitle, threadAddress);
                foreach (string threadTraceStr in threadTrace)
                    TextWriterColor.Write(threadTraceStr);
                TextWriterRaw.Write();
            }
            return 0;
        }

    }
}

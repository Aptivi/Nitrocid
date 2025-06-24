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

using FluentFTP;
using Nitrocid.Base.Kernel.Debugging;

namespace Nitrocid.ShellPacks.Tools
{
    /// <summary>
    /// FTP logger class
    /// </summary>
    public class FTPLogger : IFtpLogger
    {
        /// <summary>
        /// Translates a value of <see cref="FtpTraceLevel"/> to <see cref="DebugLevel"/>
        /// </summary>
        /// <param name="logLevel">Log level from logger</param>
        /// <returns>Debug level</returns>
        public static DebugLevel TranslateLogLevel(FtpTraceLevel logLevel)
        {
            return logLevel switch
            {
                FtpTraceLevel.Error => DebugLevel.E,
                FtpTraceLevel.Warn => DebugLevel.W,
                FtpTraceLevel.Info => DebugLevel.I,
                _ => DebugLevel.D,
            };
        }

        /// <inheritdoc/>
        public void Log(FtpLogEntry entry)
        {
            // Translate log level to debug level used by KS
            DebugLevel level = TranslateLogLevel(entry.Severity);

            // Render the string and print it
            DebugWriter.WriteDebug(level, entry.Message);

            // Optionally, check to see if we have an exception. Print its stack trace if found.
            if (entry.Exception is not null)
                DebugWriter.WriteDebugStackTrace(entry.Exception);
        }
    }
}

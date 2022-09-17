
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Kernel.Debugging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace KS.Network.FTP
{
    /// <summary>
    /// FTP logger class
    /// </summary>
    public class FTPLogger : ILogger
    {
        /// <summary>
        /// Enabled log levels
        /// </summary>
        public readonly List<LogLevel> enabledLevels = new()
        {
            LogLevel.Critical,
            LogLevel.Information,
            LogLevel.Warning,
            LogLevel.Error,
            LogLevel.Debug
        };

        /// <summary>
        /// Begins the logical logging scope
        /// </summary>
        /// <typeparam name="TState">State type</typeparam>
        /// <param name="state">State</param>
        public IDisposable BeginScope<TState>(TState state) => default!;

        /// <summary>
        /// Whether a specific log level is enabled
        /// </summary>
        /// <param name="logLevel">Log level to query</param>
        public bool IsEnabled(LogLevel logLevel) => enabledLevels.Contains(logLevel);

        /// <summary>
        /// Instructs the logger to write logging information to the debugger
        /// </summary>
        /// <typeparam name="TState">State type</typeparam>
        /// <param name="logLevel">Log level</param>
        /// <param name="eventId"></param>
        /// <param name="state">State</param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // Translate log level to debug level used by KS
            DebugLevel level = TranslateLogLevel(logLevel);

            // Render the string and print it
            string renderedString = formatter.Invoke(state, exception);
            DebugWriter.WriteDebug(level, renderedString);

            // Optionally, check to see if we have an exception. Print its stack trace if found.
            if (exception is not null)
                DebugWriter.WriteDebugStackTrace(exception);
        }

        /// <summary>
        /// Translates a value of <see cref="Microsoft.Extensions.Logging.LogLevel"/> to <see cref="DebugLevel"/>
        /// </summary>
        /// <param name="logLevel">Log level from logger</param>
        /// <returns>Debug level</returns>
        public static DebugLevel TranslateLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                case LogLevel.Error:
                    return DebugLevel.E;
                case LogLevel.Warning:
                    return DebugLevel.W;
                case LogLevel.Information:
                    return DebugLevel.I;
                case LogLevel.Debug:
                    return DebugLevel.D;
                default:
                    return DebugLevel.D;
            }
        }
    }
}

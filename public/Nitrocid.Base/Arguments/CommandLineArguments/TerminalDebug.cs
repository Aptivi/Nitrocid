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

using Aptivestigate.Logging;
using Aptivestigate.Serilog;
using Serilog;
using Terminaux.Base;
using Terminaux.Shell.Arguments.Base;

namespace Nitrocid.Base.Arguments.CommandLineArguments
{
    class TerminalDebugArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(ArgumentParameters parameters)
        {
            ConsoleLogger.AbstractLogger = new SerilogLogger(new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File(LogTools.GenerateLogFilePath(out _)));
            ConsoleLogger.EnableLogging = true;
        }
    }
}

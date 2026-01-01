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

using Nitrocid.Kernel.Debugging;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Drivers.DebugLogger.Bases
{
    internal class ConsoleDebugLogger : BaseDebugLoggerDriver, IDebugLoggerDriver
    {
        /// <inheritdoc/>
        public override string DriverName => "Console";

        /// <inheritdoc/>
        public override bool DriverInternal => true;

        /// <inheritdoc/>
        public override void Write(string text, DebugLevel level) =>
            TextWriterColor.Write(text, false);

        /// <inheritdoc/>
        public override void Write(string text, DebugLevel level, params object[] vars) =>
            TextWriterColor.Write(text, false, vars);
    }
}

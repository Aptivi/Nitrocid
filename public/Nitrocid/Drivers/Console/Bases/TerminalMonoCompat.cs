//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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

using Nitrocid.ConsoleBase;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using System;
using System.Threading;
using Terminaux.Base;

namespace Nitrocid.Drivers.Console.Bases
{
    internal class TerminalMonoCompat : BaseConsoleDriver, IConsoleDriver
    {

        public override string DriverName => "Default - Compatibility layer for Mono";

        public override DriverTypes DriverType => DriverTypes.Console;

        /// <inheritdoc/>
        public override void WritePlain(string Text, bool Line, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Get the filtered positions first.
                    int FilteredLeft = default, FilteredTop = default;
                    var pos = ConsoleExtensions.GetFilteredPositions(Text, Line, vars);
                    FilteredLeft = pos.Item1;
                    FilteredTop = pos.Item2;

                    // Actually write
                    if (Line)
                    {
                        if (vars.Length > 0)
                        {
                            WriteLine(Text, vars);
                        }
                        else
                        {
                            WriteLine(Text);
                        }
                    }
                    else if (vars.Length > 0)
                    {
                        Write(Text, vars);
                    }
                    else
                    {
                        Write(Text);
                    }

                    // Return to the processed position
                    ConsoleWrapper.SetCursorPosition(FilteredLeft, FilteredTop);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }
    }
}

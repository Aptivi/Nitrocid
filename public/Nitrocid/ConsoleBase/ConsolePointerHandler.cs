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
using Terminaux.Inputs;

namespace Nitrocid.ConsoleBase
{
    /// <summary>
    /// Console pointer handler
    /// </summary>
    public static class ConsolePointerHandler
    {
        internal static bool enableHandler = true;
        private static readonly object _lock = new();

        /// <summary>
        /// Starts the console pointer handler
        /// </summary>
        public static void StartHandler()
        {
            lock (_lock)
            {
                if (Input.EnableMouse)
                    DebugWriter.WriteDebug(DebugLevel.W, "Pointer handler is already listening while {0} is called...", vars: [nameof(StartHandler)]);
                else if (!enableHandler)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Pointer handler is not enabled while {0} is called...", vars: [nameof(StopHandler)]);
                    if (Input.EnableMouse)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Stopping the pointer handler anyway...");
                        Input.EnableMouse = false;
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Starting the pointer handler...");
                    Input.EnableMouse = true;
                }
            }
        }

        /// <summary>
        /// Starts the console pointer handler
        /// </summary>
        public static void StopHandler()
        {
            lock (_lock)
            {
                if (!Input.EnableMouse)
                    DebugWriter.WriteDebug(DebugLevel.W, "Pointer handler is not listening while {0} is called...", vars: [nameof(StopHandler)]);
                else if (!enableHandler)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Pointer handler is not enabled while {0} is called...", vars: [nameof(StopHandler)]);
                    if (Input.EnableMouse)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Stopping the pointer handler anyway...");
                        Input.EnableMouse = false;
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Stopping the pointer handler...");
                    Input.EnableMouse = false;
                }
            }
        }
    }
}

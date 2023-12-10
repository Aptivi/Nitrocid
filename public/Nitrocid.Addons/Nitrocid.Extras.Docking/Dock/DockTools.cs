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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs.Styles.Infobox;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Screensaver;
using Nitrocid.Extras.Docking.Dock.Docks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nitrocid.Extras.Docking.Dock
{
    /// <summary>
    /// Screen dock tools
    /// </summary>
    public static class DockTools
    {
        private static readonly Dictionary<string, IDock> docks = new()
        {
            { nameof(DigitalClock), new DigitalClock() }
        };

        /// <summary>
        /// Docks the screen using the given screen dock name
        /// </summary>
        /// <param name="dockName">Screen dock class name</param>
        /// <exception cref="KernelException"></exception>
        public static void DockScreen(string dockName)
        {
            // Check to see if there is a dock by this name
            if (!DoesDockScreenExist(dockName, out IDock dock))
                throw new KernelException(KernelExceptionType.Docking, Translate.DoTranslation("There is no screen dock by this name."));

            // Now, dock the screen
            DebugWriter.WriteDebug(DebugLevel.I, $"Docking screen with name: {dockName}");
            DockScreen(dock);
        }

        /// <summary>
        /// Docks the screen using the given screen dock
        /// </summary>
        /// <param name="dockInstance">Screen dock instance</param>
        /// <exception cref="KernelException"></exception>
        public static void DockScreen(IDock dockInstance)
        {
            // Check to see if there is a dock
            if (dockInstance is null)
                throw new KernelException(KernelExceptionType.Docking, Translate.DoTranslation("There is no screen dock."));

            // Now, dock the screen
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, $"Docking screen with name: [{dockInstance.DockName}]");

                // We need to prevent locking to avoid interference, because, most of the time, when you're docking your
                // screen, you're essentially idling because you've successfully converted your device to the information
                // center that displays continuously, and we don't want screensavers to interfere with the operation.
                ScreensaverManager.PreventLock();
                dockInstance.ScreenDock();
            }
            catch (Exception ex)
            {
                KernelColorTools.LoadBack();
                DebugWriter.WriteDebug(DebugLevel.E, $"Screen dock crashed [{dockInstance.DockName}]: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                InfoBoxColor.WriteInfoBoxKernelColor(Translate.DoTranslation("Screen dock has crashed") + $": {ex.Message}", KernelColorType.Error);
            }
            finally
            {
                KernelColorTools.LoadBack();
                ScreensaverManager.AllowLock();
            }
        }

        /// <summary>
        /// Checks to see if the dock screen by a specified dock class name exists
        /// </summary>
        /// <param name="dockName">Screen dock class name</param>
        /// <param name="dockInstance">Screen dock instance output</param>
        /// <returns>True if found; false otherwise.</returns>
        public static bool DoesDockScreenExist(string dockName, out IDock dockInstance)
        {
            bool result = docks.TryGetValue(dockName, out IDock dock);
            DebugWriter.WriteDebug(DebugLevel.I, $"Result: {dockName}, {result}");
            if (result)
                DebugWriter.WriteDebug(DebugLevel.I, $"Got dock: {dock.DockName}");
            dockInstance = dock;
            return result;
        }

        /// <summary>
        /// Gets the dock screen names
        /// </summary>
        /// <returns>An array containing dock screen class names that you can use with all the <see cref="DockTools"/> functions.</returns>
        public static string[] GetDockScreenNames()
        {
            string[] names = [.. docks.Keys];
            DebugWriter.WriteDebug(DebugLevel.I, $"Got {names.Length} docks: [{string.Join(", ", names)}]");
            return names;
        }

        /// <summary>
        /// Gets the dock screens
        /// </summary>
        /// <returns>A read-only dictionary containing dock screen names and their <see cref="IDock"/> instances</returns>
        public static ReadOnlyDictionary<string, IDock> GetDockScreens()
        {
            var dockScreens = new ReadOnlyDictionary<string, IDock>(docks);
            DebugWriter.WriteDebug(DebugLevel.I, $"Got {dockScreens.Count} docks");
            return dockScreens;
        }
    }
}

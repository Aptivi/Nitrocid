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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Screensaver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Nitrocid.Base.Misc.Widgets;
using Nitrocid.Base.Misc.Widgets.Implementations;
using Terminaux.Base;
using System.Threading;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles.Infobox.Tools;

namespace Nitrocid.Extras.Docking.Dock
{
    /// <summary>
    /// Screen dock tools
    /// </summary>
    public static class DockTools
    {
        private static readonly Dictionary<string, BaseWidget> docks = new()
        {
            { nameof(DigitalClock), new DigitalClock() },
            { nameof(AnalogClock), new AnalogClock() },
            { nameof(Emoji), new Emoji() },
        };

        /// <summary>
        /// Docks the screen using the given screen dock name
        /// </summary>
        /// <param name="dockName">Screen dock class name</param>
        /// <exception cref="KernelException"></exception>
        public static void DockScreen(string dockName)
        {
            // Check to see if there is a dock by this name
            if (!DoesDockScreenExist(dockName, out BaseWidget? dock))
                throw new KernelException(KernelExceptionType.Docking, LanguageTools.GetLocalized("NKS_DOCKING_NODOCKSCREEN2"));

            // Now, dock the screen
            DebugWriter.WriteDebug(DebugLevel.I, $"Docking screen with name: {dockName}");
            DockScreen(dock);
        }

        /// <summary>
        /// Docks the screen using the given screen dock
        /// </summary>
        /// <param name="dockInstance">Screen dock instance</param>
        /// <exception cref="KernelException"></exception>
        public static void DockScreen(BaseWidget? dockInstance)
        {
            // Check to see if there is a dock
            if (dockInstance is null)
                throw new KernelException(KernelExceptionType.Docking, LanguageTools.GetLocalized("NKS_DOCKING_NODOCK"));

            // Now, dock the screen
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, $"Docking screen with name: [{dockInstance.GetType().Name}]");

                // We need to prevent locking to avoid interference, because, most of the time, when you're docking your
                // screen, you're essentially idling because you've successfully converted your device to the information
                // center that displays continuously, and we don't want screensavers to interfere with the operation.
                ScreensaverManager.PreventLock();
                ThemeColorsTools.LoadBackground();
                TextWriterRaw.WriteRaw(dockInstance.Initialize());
                while (!ConsoleWrapper.KeyAvailable)
                {
                    ConsoleWrapper.CursorVisible = false;
                    TextWriterRaw.WriteRaw(dockInstance.Render());
                    SpinWait.SpinUntil(() => ConsoleWrapper.KeyAvailable, 1000);
                }
                TextWriterRaw.WriteRaw(dockInstance.Cleanup());
                Input.ReadKey();
            }
            catch (Exception ex)
            {
                ThemeColorsTools.LoadBackground();
                DebugWriter.WriteDebug(DebugLevel.E, $"Screen dock crashed [{dockInstance.GetType().Name}]: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_DOCKING_DOCKCRASHED") + $": {ex.Message}", new InfoBoxSettings()
                {
                    ForegroundColor = ThemeColorsTools.GetColor(ThemeColorType.Error)
                });
            }
            finally
            {
                ThemeColorsTools.LoadBackground();
                ScreensaverManager.AllowLock();
            }
        }

        /// <summary>
        /// Checks to see if the dock screen by a specified dock class name exists
        /// </summary>
        /// <param name="dockName">Screen dock class name</param>
        /// <param name="dockInstance">Screen dock instance output</param>
        /// <returns>True if found; false otherwise.</returns>
        public static bool DoesDockScreenExist(string dockName, out BaseWidget? dockInstance)
        {
            bool result = docks.TryGetValue(dockName, out BaseWidget? dock);
            DebugWriter.WriteDebug(DebugLevel.I, $"Result: {dockName}, {result}");
            if (result)
                DebugWriter.WriteDebug(DebugLevel.I, $"Got dock: {dock?.GetType().Name ?? "null"}");
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
        /// <returns>A read-only dictionary containing dock screen names and their <see cref="BaseWidget"/> instances</returns>
        public static ReadOnlyDictionary<string, BaseWidget> GetDockScreens()
        {
            var dockScreens = new ReadOnlyDictionary<string, BaseWidget>(docks);
            DebugWriter.WriteDebug(DebugLevel.I, $"Got {dockScreens.Count} docks");
            return dockScreens;
        }
    }
}

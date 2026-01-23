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

using Terminaux.Base.Buffered;
using Nitrocid.Drivers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Events;
using Nitrocid.Misc.Screensaver;
using Terminaux.Reader;
using ConsoleResizeListener = Terminaux.Base.ConsoleResizeHandler;
using System;

namespace Nitrocid.ConsoleBase
{
    /// <summary>
    /// The console resize listener module
    /// </summary>
    internal static class ConsoleResizeHandler
    {
        internal static bool inited = false;

        internal static void StartHandler()
        {
            // Handle window change
            if (inited)
                return;
            ConsoleResizeListener.RunEssentialHandler = false;
            ConsoleResizeListener.StartResizeListener(HandleResize);
            inited = true;
        }

        internal static void HandleResize(int oldX, int oldY, int newX, int newY)
        {
            // We need to call the WindowHeight and WindowWidth properties on the Terminal console driver, because
            // this polling works for all the terminals. Other drivers that don't use the terminal may not even
            // implement these two properties.
            DebugWriter.WriteDebug(DebugLevel.W, "Console resize detected! Terminaux reported old width x height: {0}x{1} | New width x height: {2}x{3}", vars: [oldX, oldY, newX, newY]);
            newX = Console.WindowWidth;
            newY = Console.WindowHeight;
            DebugWriter.WriteDebug(DebugLevel.W, "Final: Old width x height: {0}x{1} | New width x height: {2}x{3}", vars: [oldX, oldY, newX, newY]);
            DebugWriter.WriteDebug(DebugLevel.W, $"Userspace application will have to call {nameof(ConsoleResizeListener.WasResized)} to reset the state.");
            EventsManager.FireEvent(EventType.ResizeDetected, oldX, oldY, newX, newY);

            // Also, tell the screen-based apps to refresh themselves
            if (ScreenTools.CurrentScreen is not null && !ScreensaverManager.InSaver)
                ScreenTools.Render();

            // Also, tell the screensaver application to refresh itself
            if (ScreensaverManager.InSaver && ScreensaverDisplayer.displayingSaver is not null)
                ScreensaverDisplayer.displayingSaver.ScreensaverResizeSync();

            // Also, tell the input reader to reset
            if (TermReaderTools.Busy)
                TermReaderTools.Refresh();
        }
    }
}

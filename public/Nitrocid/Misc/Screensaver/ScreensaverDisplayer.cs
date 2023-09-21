
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Threading;
using KS.Kernel.Debugging;
using KS.Kernel.Events;
using KS.Kernel.Threading;
using KS.Kernel.Configuration;

namespace KS.Misc.Screensaver
{
    /// <summary>
    /// Screensaver display module
    /// </summary>
    internal static class ScreensaverDisplayer
    {

        internal readonly static KernelThread ScreensaverDisplayerThread = new("Screensaver display thread", false, (ss) => DisplayScreensaver((BaseScreensaver)ss));
        internal static bool OutOfSaver;
        internal static BaseScreensaver displayingSaver;

        /// <summary>
        /// Displays the screensaver from the screensaver base
        /// </summary>
        /// <param name="Screensaver">Screensaver base containing information about the screensaver</param>
        internal static void DisplayScreensaver(BaseScreensaver Screensaver)
        {
            try
            {
                // Preparations
                OutOfSaver = false;
                displayingSaver = Screensaver;
                Screensaver.ScreensaverPreparation();

                // Execute the actual screensaver logic
                while (!OutOfSaver)
                    Screensaver.ScreensaverLogic();
            }
            catch (ThreadInterruptedException)
            {
                ScreensaverManager.HandleSaverCancel();
            }
            catch (Exception ex)
            {
                ScreensaverManager.HandleSaverError(ex);
            }
            finally
            {
                OutOfSaver = true;
                Screensaver.ScreensaverOutro();
            }
        }

        internal static void BailFromScreensaver()
        {
            if (ScreensaverManager.InSaver)
            {
                ScreensaverDisplayerThread.Stop(false);
                ScreensaverManager.SaverAutoReset.WaitOne();

                // Raise event
                DebugWriter.WriteDebug(DebugLevel.I, "Screensaver really stopped.");
                EventsManager.FireEvent(EventType.PostShowScreensaver);
                ScreensaverManager.inSaver = false;
                KernelFlags.ScrnTimeReached = false;
                ScreensaverDisplayerThread.Regen();
            }
        }

    }
}

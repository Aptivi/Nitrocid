
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
using KS.Kernel;
using KS.Misc.Threading;
using KS.Kernel.Events;

namespace KS.Misc.Screensaver
{
    /// <summary>
    /// Screensaver display module
    /// </summary>
    public static class ScreensaverDisplayer
    {

        internal readonly static KernelThread ScreensaverDisplayerThread = new("Screensaver display thread", false, (ss) => DisplayScreensaver((BaseScreensaver)ss)) { isCritical = true };
        internal static bool OutOfRandom;

        /// <summary>
        /// Displays the screensaver from the screensaver base
        /// </summary>
        /// <param name="Screensaver">Screensaver base containing information about the screensaver</param>
        public static void DisplayScreensaver(BaseScreensaver Screensaver)
        {
            try
            {
                // Preparations
                if (Screensaver.ScreensaverContainsFlashingImages)
                    Screensaver.ScreensaverSeizureWarning();
                OutOfRandom = false;
                Screensaver.ScreensaverPreparation();

                // Execute the actual screensaver logic
                while (!OutOfRandom)
                    Screensaver.ScreensaverLogic();
            }
            catch (ThreadInterruptedException)
            {
                Misc.Screensaver.Screensaver.HandleSaverCancel();
            }
            catch (Exception ex)
            {
                Misc.Screensaver.Screensaver.HandleSaverError(ex);
            }
            finally
            {
                OutOfRandom = true;
                Screensaver.ScreensaverOutro();
            }
        }

        internal static void BailFromScreensaver()
        {
            if (Screensaver.InSaver)
            {
                ScreensaverDisplayerThread.Stop(false);
                Screensaver.SaverAutoReset.WaitOne();

                // Raise event
                DebugWriter.WriteDebug(DebugLevel.I, "Screensaver really stopped.");
                EventsManager.FireEvent(EventType.PostShowScreensaver);
                Screensaver.inSaver = false;
                Flags.ScrnTimeReached = false;
                ScreensaverDisplayerThread.Regen();
            }
        }

    }
}

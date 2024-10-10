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

using System;
using System.Threading;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Threading;
using Nitrocid.Kernel.Events;
using Terminaux.Base;
using Terminaux.Colors;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;

namespace Nitrocid.Misc.Screensaver
{
    /// <summary>
    /// Screensaver display module
    /// </summary>
    internal static class ScreensaverDisplayer
    {

        internal readonly static KernelThread ScreensaverDisplayerThread = new("Screensaver display thread", false, (ss) => DisplayScreensaver((BaseScreensaver?)ss));
        internal static bool OutOfSaver;
        internal static BaseScreensaver? displayingSaver;

        /// <summary>
        /// Displays the screensaver from the screensaver base
        /// </summary>
        /// <param name="Screensaver">Screensaver base containing information about the screensaver</param>
        internal static void DisplayScreensaver(BaseScreensaver? Screensaver)
        {
            if (Screensaver is null)
                throw new KernelException(KernelExceptionType.ScreensaverManagement, Translate.DoTranslation("Screensaver instance is not specified"));
            bool initialVisible = ConsoleWrapper.CursorVisible;
            bool initialBack = ColorTools.AllowBackground;
            bool initialPalette = ColorTools.GlobalSettings.UseTerminalPalette;
            try
            {
                // Preparations
                OutOfSaver = false;
                displayingSaver = Screensaver;
                ColorTools.AllowBackground = true;
                ColorTools.GlobalSettings.UseTerminalPalette = false;
                Screensaver.ScreensaverPreparation();

                // Execute the actual screensaver logic
                while (!OutOfSaver)
                {
                    if (ConsoleWrapper.CursorVisible)
                        ConsoleWrapper.CursorVisible = false;
                    Screensaver.ScreensaverLogic();
                }
            }
            catch (ThreadInterruptedException)
            {
                ScreensaverManager.HandleSaverCancel(initialVisible);
            }
            catch (Exception ex)
            {
                ScreensaverManager.HandleSaverError(ex, initialVisible);
            }
            finally
            {
                OutOfSaver = true;
                Screensaver.ScreensaverOutro();
                ColorTools.AllowBackground = initialBack;
                ColorTools.GlobalSettings.UseTerminalPalette = initialPalette;
                ColorTools.LoadBack();
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
                ScreensaverManager.ScrnTimeReached = false;
                ScreensaverDisplayerThread.Regen();
            }
        }

    }
}

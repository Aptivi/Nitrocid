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

using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Starting.Bootloader.Apps;
using Nitrocid.Kernel.Starting.Bootloader.KeyHandler;
using Nitrocid.Kernel.Starting.Bootloader.Style;
using Nitrocid.Kernel.Starting.Environment;
using System;
using System.Linq;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Colors;
using Terminaux.Reader;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Kernel.Starting.Bootloader
{
    internal class BootloaderMain
    {
        internal static Screen bootloaderScreen = new();

        internal static void StartBootloader()
        {
            try
            {
                // Preload bootloader
                ConsoleWrapper.CursorVisible = false;

                // Now, enter the main loop.
                MainLoop();
            }
            catch (Exception ex)
            {
                // Failed trying to preload the bootloader or failure in the bootloader (after preloading)
                DebugWriter.WriteDebug(DebugLevel.E, "Bootloader has failed: {0}", ex.Message);
                TextWriterColor.Write("GRILO has experienced an internal error: {0}", ex.Message);

                DebugWriter.WriteDebug(DebugLevel.E, "Stack trace:\n{0}", ex.StackTrace);
                TextWriterColor.Write(ex.StackTrace);
                TextWriterColor.Write("Press any key to exit.");
                TermReader.ReadKey();
            }
            finally
            {
                // Clean up
                ColorTools.LoadBack();
                ConsoleWrapper.CursorVisible = true;
            }
        }

        internal static void MainLoop()
        {
            // Get the boot apps
            var bootApps = BootManager.GetBootApps();
            int chosenBootEntry = Config.MainConfig.BootSelect;

            // Reset the bootloader state
            BootloaderState.waitingForBootKey = true;
            BootloaderState.waitingForFirstBootKey = true;

            // Set the bootloader screen as a default
            ScreenTools.SetCurrent(bootloaderScreen);

            // Now, draw the boot menu. Note that the chosen boot entry counts from zero.
            var bootloaderBuffer = new ScreenPart();
            var postBootloaderBuffer = new ScreenPart();
            var postBootBuffer = new ScreenPart();
            bootloaderScreen.AddBufferedPart("Bootloader Screen", bootloaderBuffer);

            // Wait for a boot key
            while (BootloaderState.WaitingForBootKey)
            {
                // Render the menu
                DebugWriter.WriteDebug(DebugLevel.I, "Rendering menu...");
                bootloaderBuffer.AddDynamicText(() =>
                {
                    ConsoleWrapper.CursorVisible = false;
                    return BootStyleManager.RenderMenu(chosenBootEntry);
                });

                // Actually render the thing
                ScreenTools.Render();

                // Wait for a key and parse it
                int timeout = Config.MainConfig.BootSelectTimeoutSeconds;
                BootStyleManager.RenderSelectTimeout(timeout);
                ConsoleKeyInfo cki;
                if (timeout > 0 && BootloaderState.WaitingForFirstBootKey)
                {
                    var result = TermReader.ReadKeyTimeout(true, TimeSpan.FromSeconds(Config.MainConfig.BootSelectTimeoutSeconds));
                    if (!result.provided)
                        cki = new('\x0A', ConsoleKey.Enter, false, false, false);
                    else
                        cki = result.result;
                }
                else
                    cki = TermReader.ReadKey();
                BootloaderState.waitingForFirstBootKey = false;
                DebugWriter.WriteDebug(DebugLevel.I, "Key pressed: {0}", cki.Key.ToString());
                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow:
                        DebugWriter.WriteDebug(DebugLevel.I, "Decrementing boot entry...");
                        chosenBootEntry--;

                        // If we reached the beginning of the boot menu, go to the ending
                        if (chosenBootEntry < 0)
                        {
                            chosenBootEntry = bootApps.Count - 1;
                            DebugWriter.WriteDebug(DebugLevel.I, "We're at the beginning! Chosen boot entry is now {0}", chosenBootEntry);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        DebugWriter.WriteDebug(DebugLevel.I, "Incrementing boot entry...");
                        chosenBootEntry++;

                        // If we reached the ending of the boot menu, go to the beginning
                        if (chosenBootEntry > bootApps.Count - 1)
                        {
                            chosenBootEntry = 0;
                            DebugWriter.WriteDebug(DebugLevel.I, "We're at the ending! Chosen boot entry is now {0}", chosenBootEntry);
                        }
                        break;
                    case ConsoleKey.Home:
                        DebugWriter.WriteDebug(DebugLevel.I, "Decrementing boot entry to the first entry...");
                        chosenBootEntry = 0;
                        break;
                    case ConsoleKey.End:
                        DebugWriter.WriteDebug(DebugLevel.I, "Decrementing boot entry to the last entry...");
                        chosenBootEntry = bootApps.Count - 1;
                        break;
                    case ConsoleKey.H:
                        if (cki.Modifiers.HasFlag(ConsoleModifiers.Shift))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Opening controls page...");
                            var style = BootStyleManager.GetCurrentBootStyle();
                            bootloaderBuffer.Clear();
                            bootloaderBuffer.AddDynamicText(() =>
                            {
                                return BootStyleManager.RenderDialog(
                                    $"""
                                    Standard controls
                                    -----------------

                                    [UP ARROW]   | Selects the previous boot entry
                                    [DOWN ARROW] | Selects the next boot entry
                                    [HOME]       | Selects the first boot entry
                                    [END]        | Selects the last boot entry
                                    [SHIFT + H]  | Opens this help page
                                    [ENTER]      | Boots the selected entry

                                    Controls defined by custom boot style
                                    -------------------------------------

                                    {(style.CustomKeys is not null && style.CustomKeys.Count > 0 ?
                                    string.Join("\n", style.CustomKeys
                                        .Select((cki) => $"[{string.Join(" + ", cki.Key.Modifiers)} + {cki.Key.Key}]")) :
                                    "No controls defined by custom boot style")}
                                    """
                                );
                            });

                            // Wait for input
                            DebugWriter.WriteDebug(DebugLevel.I, "Waiting for user to press any key...");
                            ScreenTools.Render();
                            TermReader.ReadKey();
                            bootloaderScreen.RequireRefresh();
                        }
                        break;
                    case ConsoleKey.Enter:
                        // We're no longer waiting for boot key
                        DebugWriter.WriteDebug(DebugLevel.I, "Booting...");
                        BootloaderState.waitingForBootKey = false;
                        break;
                    default:
                        Handler.HandleKey(cki);
                        break;
                }
                bootloaderBuffer.Clear();
            }

            // Remove the bootloader buffer
            bootloaderScreen.RemoveBufferedPart("Bootloader Screen");

            // Add the post-bootloader screen buffer
            bootloaderScreen.AddBufferedPart("Post-Bootloader Screen", postBootloaderBuffer);
            bootloaderScreen.RequireRefresh();

            // Reset the states
            BootloaderState.waitingForBootKey = true;

            // Boot the system
            try
            {
                string chosenBootName = BootManager.GetBootAppNameByIndex(chosenBootEntry);
                var chosenBootApp = BootManager.GetBootApp(chosenBootName);
                DebugWriter.WriteDebug(DebugLevel.I, "Boot name {0} at index {1}", chosenBootName, chosenBootEntry);

                // Check the environment
                if (chosenBootApp == EnvironmentTools.mainEnvironment)

                // Render the booting message
                postBootloaderBuffer.AddDynamicText(() => BootStyleManager.RenderBootingMessage(chosenBootName));
                ScreenTools.Render();
                bootloaderScreen.RequireRefresh();
                bootloaderScreen.RemoveBufferedPart("Post-Bootloader Screen");

                // Now, set the environment
                EnvironmentTools.SetEnvironment(chosenBootApp);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Unknown boot failure: {0}", ex.Message);
                DebugWriter.WriteDebug(DebugLevel.E, "Stack trace:\n{0}", ex.StackTrace);
            }
        }
    }
}

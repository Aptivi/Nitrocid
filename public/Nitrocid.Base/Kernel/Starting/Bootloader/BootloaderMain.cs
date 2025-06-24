//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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
using System;
using System.Linq;
using Terminaux.Base;
using Terminaux.Base.Buffered;
using Terminaux.Base.Extensions;
using Terminaux.Inputs;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Starting.Bootloader.Style;
using Nitrocid.Base.Kernel.Starting.Environment;
using Nitrocid.Base.Kernel.Starting.Bootloader.KeyHandler;
using Nitrocid.Base.Kernel.Starting.Bootloader.Apps;

namespace Nitrocid.Base.Kernel.Starting.Bootloader
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
                // Failure in the bootloader
                DebugWriter.WriteDebug(DebugLevel.E, "Bootloader has failed: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_ERROR") + ": {0}", ex.Message);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_EXIT"));
                Input.ReadKey();
            }
            finally
            {
                // Clean up
                ThemeColorsTools.LoadBackground();
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
                    var result = Input.ReadKeyTimeout(TimeSpan.FromSeconds(Config.MainConfig.BootSelectTimeoutSeconds));
                    if (!result.provided)
                        cki = new('\x0A', ConsoleKey.Enter, false, false, false);
                    else
                        cki = result.result;
                }
                else
                    cki = Input.ReadKey();
                BootloaderState.waitingForFirstBootKey = false;
                DebugWriter.WriteDebug(DebugLevel.I, "Key pressed: {0}", vars: [cki.Key.ToString()]);
                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow:
                        DebugWriter.WriteDebug(DebugLevel.I, "Decrementing boot entry...");
                        chosenBootEntry--;

                        // If we reached the beginning of the boot menu, go to the ending
                        if (chosenBootEntry < 0)
                        {
                            chosenBootEntry = bootApps.Count - 1;
                            DebugWriter.WriteDebug(DebugLevel.I, "We're at the beginning! Chosen boot entry is now {0}", vars: [chosenBootEntry]);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        DebugWriter.WriteDebug(DebugLevel.I, "Incrementing boot entry...");
                        chosenBootEntry++;

                        // If we reached the ending of the boot menu, go to the beginning
                        if (chosenBootEntry > bootApps.Count - 1)
                        {
                            chosenBootEntry = 0;
                            DebugWriter.WriteDebug(DebugLevel.I, "We're at the ending! Chosen boot entry is now {0}", vars: [chosenBootEntry]);
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
                                string section1 = LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_STDCONTROLS");
                                string section2 = LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_CUSTOMCONTROLS");
                                string renderedCustomKeys =
                                    style.CustomKeys is not null && style.CustomKeys.Count > 0 ?
                                    string.Join("\n", style.CustomKeys
                                        .Select((cki) => $"[{string.Join(" + ", cki.Key.Modifiers)} + {cki.Key.Key}]")) :
                                    LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_NOCUSTOMCONTROLS");
                                return BootStyleManager.RenderDialog(
                                    $"""
                                    {section1}
                                    {new string('=', ConsoleChar.EstimateCellWidth(section1))}

                                    [UP ARROW]   | {LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_KEYBINDING_PREVENTRY")}
                                    [DOWN ARROW] | {LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_KEYBINDING_NEXTENTRY")}
                                    [HOME]       | {LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_KEYBINDING_FIRSTENTRY")}
                                    [END]        | {LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_KEYBINDING_LASTENTRY")}
                                    [SHIFT + H]  | {LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_KEYBINDING_HELP")}
                                    [ENTER]      | {LanguageTools.GetLocalized("NKS_KERNEL_STARTING_BOOTLOADER_KEYBINDING_BOOT")}

                                    {section2}
                                    {new string('=', ConsoleChar.EstimateCellWidth(section2))}

                                    {renderedCustomKeys}
                                    """
                                );
                            });

                            // Wait for input
                            DebugWriter.WriteDebug(DebugLevel.I, "Waiting for user to press any key...");
                            ScreenTools.Render();
                            Input.ReadKey();
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
                DebugWriter.WriteDebug(DebugLevel.I, "Boot name {0} at index {1}", vars: [chosenBootName, chosenBootEntry]);

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
                DebugWriter.WriteDebug(DebugLevel.E, "Unknown boot failure: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebug(DebugLevel.E, "Stack trace:\n{0}", vars: [ex.StackTrace]);
            }
        }
    }
}

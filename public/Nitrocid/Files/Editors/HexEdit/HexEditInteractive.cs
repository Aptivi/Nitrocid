//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Interactive;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Files.Operations.Printing;
using KS.Files.Operations.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Screensaver;
using KS.Shell.Shells.Hex;
using System;
using System.Linq;

namespace KS.Files.Editors.HexEdit
{
    /// <summary>
    /// Interactive hex editor
    /// </summary>
    public static class HexEditInteractive
    {
        private static string status;
        private static bool bail;
        private static bool refresh = true;
        private static int byteIdx = 0;
        private static readonly HexEditorBinding[] bindings = new[]
        {
            new HexEditorBinding(/* Localizable */ "Exit", ConsoleKey.Escape, default, () => bail = true, true),
            new HexEditorBinding(/* Localizable */ "Move Left", ConsoleKey.LeftArrow, default, MoveBackward, true),
            new HexEditorBinding(/* Localizable */ "Move Right", ConsoleKey.RightArrow, default, MoveForward, true),
            new HexEditorBinding(/* Localizable */ "Move Up", ConsoleKey.UpArrow, default, MoveUp, true),
            new HexEditorBinding(/* Localizable */ "Move Down", ConsoleKey.DownArrow, default, MoveDown, true),
        };

        /// <summary>
        /// Opens an interactive hex editor
        /// </summary>
        /// <param name="file">Target file to open</param>
        public static void OpenInteractive(string file) =>
            OpenInteractive(file, false);

        /// <summary>
        /// Opens an interactive hex editor
        /// </summary>
        /// <param name="file">Target file to open</param>
        /// <param name="fromShell">Whether it's open from the hex shell</param>
        internal static void OpenInteractive(string file, bool fromShell)
        {
            // Check to see if the file exists
            if (!Checking.FileExists(file))
                throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("File not found.") + $" {file}");

            // Open the file
            if (!fromShell && !HexEditTools.OpenBinaryFile(file))
                throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("Failed to open the binary file.") + $" {file}");

            // Set status
            status = Translate.DoTranslation("Ready");
            refresh = true;
            bail = false;

            // Main loop
            byteIdx = 0;
            while (!bail)
            {
                // Check to see if we need to refresh
                if (refresh)
                {
                    refresh = false;

                    // Clear the screen
                    ConsoleWrapper.CursorVisible = false;
                    KernelColorTools.LoadBack();

                    // Now, render the keybindings
                    RenderKeybindings();

                    // Render the status
                    RenderStatus();

                    // Render the box
                    RenderHexViewBox();
                }

                // Now, render the visual hex with the current selection
                RenderContentsInHexWithSelection(byteIdx);

                // Wait for a keypress
                var keypress = Input.DetectKeypress();
                HandleKeypress(keypress);

                // Finally, set the refresh requirement
                if (!refresh)
                    refresh = ScreensaverManager.ScreenRefreshRequired || ConsoleResizeListener.WasResized(false);
            }

            // Close the file and clean up
            KernelColorTools.LoadBack();
            if (!fromShell)
                HexEditTools.CloseBinaryFile();
        }

        private static void RenderKeybindings()
        {
            foreach (HexEditorBinding binding in bindings)
            {
                // First, check to see if the rendered binding info is going to exceed the console window width
                string renderedBinding = $" {binding.Key} {Translate.DoTranslation(binding.Name)}  ";
                bool canDraw = renderedBinding.Length + ConsoleWrapper.CursorLeft < ConsoleWrapper.WindowWidth - 3;
                if (canDraw)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Drawing binding {0} with description {1}...", binding.Key.ToString(), binding.Name);
                    TextWriterWhereColor.WriteWhereColorBack($" {binding.Key} ", ConsoleWrapper.CursorLeft + 0, ConsoleWrapper.WindowHeight - 1, BaseInteractiveTui.KeyBindingOptionColor, BaseInteractiveTui.OptionBackgroundColor);
                    TextWriterWhereColor.WriteWhereColorBack($"{(binding._localizable ? Translate.DoTranslation(binding.Name) : binding.Name)}  ", ConsoleWrapper.CursorLeft + 1, ConsoleWrapper.WindowHeight - 1, BaseInteractiveTui.OptionForegroundColor, KernelColorTools.GetColor(KernelColorType.Background));
                }
                else
                {
                    // We can't render anymore, so just break and write a binding to show more
                    DebugWriter.WriteDebug(DebugLevel.I, "Bailing because of no space...");
                    TextWriterWhereColor.WriteWhereColorBack($" K ", ConsoleWrapper.WindowWidth - 3, ConsoleWrapper.WindowHeight - 1, BaseInteractiveTui.KeyBindingOptionColor, BaseInteractiveTui.OptionBackgroundColor);
                    break;
                }
            }
        }

        private static void RenderStatus() =>
            TextWriterWhereColor.WriteWhereColorBack(status, 0, 0, BaseInteractiveTui.ForegroundColor, KernelColorTools.GetColor(KernelColorType.Background));

        private static void RenderHexViewBox()
        {
            // Get the widths and heights
            int SeparatorConsoleWidthInterior = ConsoleWrapper.WindowWidth - 2;
            int SeparatorMinimumHeight = 1;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

            // Render the box
            BorderColor.WriteBorder(0, SeparatorMinimumHeight, SeparatorConsoleWidthInterior, SeparatorMaximumHeightInterior, BaseInteractiveTui.PaneSeparatorColor, KernelColorTools.GetColor(KernelColorType.Background));
        }

        private static void RenderContentsInHexWithSelection(int byteIdx)
        {
            int byteLinesPerPage = ConsoleWrapper.WindowHeight - 4;
            int paneCurrentSelection = byteIdx / 16;
            int currentPage = (paneCurrentSelection - 1) / byteLinesPerPage;
            int startIndex = byteLinesPerPage * currentPage;
            int endIndex = byteLinesPerPage * (currentPage + 1);
            int startByte = (startIndex * 16) + 1;
            int endByte = endIndex * 16;
            string rendered = FileContentPrinter.RenderContentsInHex(byteIdx + 1, startByte, endByte, HexEditShellCommon.FileBytes);
            TextWriterWhereColor.WriteWhereColorBack(rendered, 1, 2, BaseInteractiveTui.ForegroundColor, KernelColorTools.GetColor(KernelColorType.Background));
        }

        private static void HandleKeypress(ConsoleKeyInfo key)
        {
            if (!bindings.Any((heb) => heb.Key == key.Key && heb.KeyModifiers == key.Modifiers))
                return;

            var bind = bindings
                .First((heb) => heb.Key == key.Key && heb.KeyModifiers == key.Modifiers);
            bind.Action();
        }

        private static void MoveBackward()
        {
            byteIdx--;
            if (byteIdx < 0)
                byteIdx = 0;
        }

        private static void MoveForward()
        {
            byteIdx++;
            if (byteIdx > HexEditShellCommon.FileBytes.Length - 1)
                byteIdx = HexEditShellCommon.FileBytes.Length - 1;
        }

        private static void MoveUp()
        {
            byteIdx -= 16;
            if (byteIdx < 0)
                byteIdx = 0;
        }

        private static void MoveDown()
        {
            byteIdx += 16;
            if (byteIdx > HexEditShellCommon.FileBytes.Length - 1)
                byteIdx = HexEditShellCommon.FileBytes.Length - 1;
        }
    }
}

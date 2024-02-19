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
using System.Globalization;
using System.Linq;
using System.Text;
using Terminaux.Sequences.Builder.Types;
using Terminaux.Sequences;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Shell.Shells.Hex;
using Nitrocid.Files.Operations;
using Nitrocid.Languages;
using Nitrocid.Files.Operations.Printing;
using Nitrocid.Kernel.Exceptions;
using Terminaux.Writer.FancyWriters;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Inputs.Interactive;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Base.Buffered;
using Nitrocid.Files.Operations.Querying;
using Textify.General;
using Terminaux.Colors;
using Terminaux.Base;
using Terminaux.Reader;
using Terminaux.Base.Extensions;

namespace Nitrocid.Files.Editors.HexEdit
{
    /// <summary>
    /// Interactive hex editor
    /// </summary>
    public static class HexEditInteractive
    {
        private static string status;
        private static bool bail;
        private static int byteIdx = 0;
        private static readonly HexEditorBinding[] bindings =
        [
            new HexEditorBinding( /* Localizable */ "Exit", ConsoleKey.Escape, default, (b) => { bail = true; return b; }, true),
            new HexEditorBinding( /* Localizable */ "Keybindings", ConsoleKey.K, default, RenderKeybindingsBox, true),
            new HexEditorBinding( /* Localizable */ "Insert", ConsoleKey.F1, default, Insert, true),
            new HexEditorBinding( /* Localizable */ "Remove", ConsoleKey.F2, default, Remove, true),
            new HexEditorBinding( /* Localizable */ "Replace", ConsoleKey.F3, default, Replace, true),
            new HexEditorBinding( /* Localizable */ "Replace All", ConsoleKey.F3, ConsoleModifiers.Shift, ReplaceAll, true),
            new HexEditorBinding( /* Localizable */ "Replace All What", ConsoleKey.F3, ConsoleModifiers.Shift | ConsoleModifiers.Alt, ReplaceAllWhat, true),
            new HexEditorBinding( /* Localizable */ "Number Info", ConsoleKey.F4, default, NumInfo, true),
        ];

        /// <summary>
        /// Opens an interactive hex editor
        /// </summary>
        /// <param name="file">Target file to open</param>
        public static void OpenInteractive(string file) =>
            OpenInteractive(file, ref HexEditShellCommon.FileBytes);

        /// <summary>
        /// Opens an interactive hex editor
        /// </summary>
        /// <param name="file">Target file to open</param>
        /// <param name="bytes">Resulting byte array</param>
        internal static void OpenInteractive(string file, ref byte[] bytes)
        {
            // Check to see if the file exists
            if (!Checking.FileExists(file))
                throw new KernelException(KernelExceptionType.HexEditor, Translate.DoTranslation("File not found.") + $" {file}");

            // Try to open the file
            bytes ??= Reading.ReadAllBytesNoBlock(file);

            // Set status
            status = Translate.DoTranslation("Ready");
            bail = false;

            // Main loop
            byteIdx = 0;
            var screen = new Screen();
            ScreenTools.SetCurrent(screen);
            ConsoleWrapper.CursorVisible = false;
            ColorTools.LoadBackDry(InteractiveTuiStatus.OptionBackgroundColor);
            try
            {
                while (!bail)
                {
                    // Now, render the keybindings
                    RenderKeybindings(ref screen);

                    // Render the box
                    RenderHexViewBox(ref screen);

                    // Now, render the visual hex with the current selection
                    RenderContentsInHexWithSelection(byteIdx, ref screen, bytes);

                    // Render the status
                    RenderStatus(ref screen);

                    // Wait for a keypress
                    ScreenTools.Render(screen);
                    var keypress = TermReader.ReadKey();
                    HandleKeypress(keypress, ref bytes);

                    // Reset, in case selection changed
                    screen.RemoveBufferedParts();
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Hex editor failed: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                InfoBoxColor.WriteInfoBoxColor(Translate.DoTranslation("The hex editor failed:") + $" {ex.Message}", KernelColorTools.GetColor(KernelColorType.Error));
            }
            bail = false;
            ScreenTools.UnsetCurrent(screen);

            // Close the file and clean up
            ColorTools.LoadBack();
        }

        private static void RenderKeybindings(ref Screen screen)
        {
            // Make a screen part
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var bindingsBuilder = new StringBuilder(CsiSequences.GenerateCsiCursorPosition(1, ConsoleWrapper.WindowHeight));
                foreach (HexEditorBinding binding in bindings)
                {
                    // First, check to see if the rendered binding info is going to exceed the console window width
                    string renderedBinding = $"{GetBindingKeyShortcut(binding, false)} {(binding._localizable ? Translate.DoTranslation(binding.Name) : binding.Name)}  ";
                    int actualLength = VtSequenceTools.FilterVTSequences(bindingsBuilder.ToString()).Length;
                    bool canDraw = renderedBinding.Length + actualLength < ConsoleWrapper.WindowWidth - 3;
                    if (canDraw)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Drawing binding {0} with description {1}...", GetBindingKeyShortcut(binding, false), binding.Name);
                        bindingsBuilder.Append(
                            $"{InteractiveTuiStatus.KeyBindingOptionColor.VTSequenceForeground}" +
                            $"{InteractiveTuiStatus.OptionBackgroundColor.VTSequenceBackground}" +
                            GetBindingKeyShortcut(binding, false) +
                            $"{InteractiveTuiStatus.OptionForegroundColor.VTSequenceForeground}" +
                            $"{InteractiveTuiStatus.BackgroundColor.VTSequenceBackground}" +
                            $" {(binding._localizable ? Translate.DoTranslation(binding.Name) : binding.Name)}  " +
                            ConsoleClearing.GetClearLineToRightSequence()
                        );
                    }
                    else
                    {
                        // We can't render anymore, so just break and write a binding to show more
                        DebugWriter.WriteDebug(DebugLevel.I, "Bailing because of no space...");
                        bindingsBuilder.Append(
                            $"{CsiSequences.GenerateCsiCursorPosition(ConsoleWrapper.WindowWidth - 2, ConsoleWrapper.WindowHeight)}" +
                            $"{InteractiveTuiStatus.KeyBindingOptionColor.VTSequenceForeground}" +
                            $"{InteractiveTuiStatus.OptionBackgroundColor.VTSequenceBackground}" +
                            " K " +
                            ConsoleClearing.GetClearLineToRightSequence()
                        );
                        break;
                    }
                }
                return bindingsBuilder.ToString();
            });
            screen.AddBufferedPart("Hex editor interactive - Keybindings", part);
        }

        private static void RenderStatus(ref Screen screen)
        {
            // Make a screen part
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var builder = new StringBuilder();
                builder.Append(
                    $"{InteractiveTuiStatus.ForegroundColor.VTSequenceForeground}" +
                    $"{InteractiveTuiStatus.BackgroundColor.VTSequenceBackground}" +
                    $"{TextWriterWhereColor.RenderWhere(status + ConsoleClearing.GetClearLineToRightSequence(), 0, 0)}"
                );
                return builder.ToString();
            });
            screen.AddBufferedPart("Hex editor interactive - Status", part);
        }

        private static void RenderHexViewBox(ref Screen screen)
        {
            // Make a screen part
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var builder = new StringBuilder();

                // Get the widths and heights
                int SeparatorConsoleWidthInterior = ConsoleWrapper.WindowWidth - 2;
                int SeparatorMinimumHeight = 1;
                int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

                // Render the box
                builder.Append(
                    $"{InteractiveTuiStatus.PaneSeparatorColor.VTSequenceForeground}" +
                    $"{InteractiveTuiStatus.BackgroundColor.VTSequenceBackground}" +
                    $"{BorderColor.RenderBorderPlain(0, SeparatorMinimumHeight, SeparatorConsoleWidthInterior, SeparatorMaximumHeightInterior)}"
                );
                return builder.ToString();
            });
            screen.AddBufferedPart("Hex editor interactive - Hex view box", part);
        }

        private static void RenderContentsInHexWithSelection(int byteIdx, ref Screen screen, byte[] bytes)
        {
            // First, update the status
            StatusNumInfo(bytes);

            // Then, render the contents with the selection indicator
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var builder = new StringBuilder();
                int byteLinesPerPage = ConsoleWrapper.WindowHeight - 4;
                int currentSelection = byteIdx / 16;
                int currentPage = currentSelection / byteLinesPerPage;
                int startIndex = byteLinesPerPage * currentPage;
                int endIndex = byteLinesPerPage * (currentPage + 1);
                int startByte = startIndex * 16 + 1;
                int endByte = endIndex * 16;
                if (startByte > bytes.Length)
                    startByte = bytes.Length;
                if (endByte > bytes.Length)
                    endByte = bytes.Length;
                string rendered = FileContentPrinter.RenderContentsInHex(byteIdx + 1, startByte, endByte, bytes);

                // Render the box
                builder.Append(
                    $"{InteractiveTuiStatus.ForegroundColor.VTSequenceForeground}" +
                    $"{InteractiveTuiStatus.BackgroundColor.VTSequenceBackground}" +
                    $"{TextWriterWhereColor.RenderWhere(rendered, 1, 2)}"
                );
                return builder.ToString();
            });
            screen.AddBufferedPart("Hex editor interactive - Contents", part);
        }

        private static void HandleKeypress(ConsoleKeyInfo key, ref byte[] bytes)
        {
            // Check to see if we have this binding
            if (!bindings.Any((heb) => heb.Key == key.Key && heb.KeyModifiers == key.Modifiers))
            {
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        MoveBackward();
                        return;
                    case ConsoleKey.RightArrow:
                        MoveForward(bytes);
                        return;
                    case ConsoleKey.UpArrow:
                        MoveUp();
                        return;
                    case ConsoleKey.DownArrow:
                        MoveDown(bytes);
                        return;
                    case ConsoleKey.PageUp:
                        PreviousPage(bytes);
                        return;
                    case ConsoleKey.PageDown:
                        NextPage(bytes);
                        return;
                    case ConsoleKey.Home:
                        Beginning();
                        return;
                    case ConsoleKey.End:
                        End(bytes);
                        return;
                }
                return;
            }

            // Now, get the first binding and execute it.
            var bind = bindings
                .First((heb) => heb.Key == key.Key && heb.KeyModifiers == key.Modifiers);
            bytes = bind.Action(bytes);
        }

        private static byte[] RenderKeybindingsBox(byte[] bytes)
        {
            // Show the available keys list
            if (bindings.Length == 0)
                return bytes;

            // User needs an infobox that shows all available keys
            string section = Translate.DoTranslation("Available keys");
            int maxBindingLength = bindings
                .Max((heb) => GetBindingKeyShortcut(heb).Length);
            string[] bindingRepresentations = bindings
                .Select((heb) => $"{GetBindingKeyShortcut(heb) + new string(' ', maxBindingLength - GetBindingKeyShortcut(heb).Length) + $" | {(heb._localizable ? Translate.DoTranslation(heb.Name) : heb.Name)}"}")
                .ToArray();
            InfoBoxColor.WriteInfoBoxColorBack(
                $"{section}{CharManager.NewLine}" +
                $"{new string('=', section.Length)}{CharManager.NewLine}{CharManager.NewLine}" +
                $"{string.Join('\n', bindingRepresentations)}"
            , InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            return bytes;
        }

        private static string GetBindingKeyShortcut(HexEditorBinding bind, bool mark = true)
        {
            string markStart = mark ? "[" : " ";
            string markEnd = mark ? "]" : " ";
            return $"{markStart}{(bind.KeyModifiers != 0 ? $"{bind.KeyModifiers} + " : "")}{bind.Key}{markEnd}";
        }

        private static void MoveBackward()
        {
            byteIdx--;
            if (byteIdx < 0)
                byteIdx = 0;
        }

        private static void MoveForward(byte[] bytes)
        {
            byteIdx++;
            if (byteIdx > bytes.Length - 1)
                byteIdx = bytes.Length - 1;
        }

        private static void MoveUp()
        {
            byteIdx -= 16;
            if (byteIdx < 0)
                byteIdx = 0;
        }

        private static void MoveDown(byte[] bytes)
        {
            byteIdx += 16;
            if (byteIdx > bytes.Length - 1)
                byteIdx = bytes.Length - 1;
        }

        private static byte[] Insert(byte[] bytes)
        {
            // Prompt and parse the number
            byte byteNum = default;
            string byteNumHex = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Write the byte number with the hexadecimal value.") + " 00 -> FF.", InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            if (byteNumHex.Length != 2 ||
                byteNumHex.Length == 2 && !byte.TryParse(byteNumHex, NumberStyles.AllowHexSpecifier, null, out byteNum))
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("The byte number specified is not valid."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            else
                bytes = HexEditTools.AddNewByte(bytes, byteNum, byteIdx + 1);
            return bytes;
        }

        private static byte[] Remove(byte[] bytes)
        {
            bytes = HexEditTools.DeleteByte(bytes, byteIdx + 1);
            return bytes;
        }

        private static byte[] Replace(byte[] bytes)
        {
            // Get the current byte number and its hex
            byte byteNum = bytes[byteIdx];
            string byteNumHex = byteNum.ToString("X2");

            // Now, prompt for the replacement byte
            byte byteNumReplaced = default;
            string byteNumReplacedHex = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Write the byte number with the hexadecimal value to replace {0} with.") + " 00 -> FF.", InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor, byteNumHex);
            if (byteNumReplacedHex.Length != 2 ||
                byteNumReplacedHex.Length == 2 && !byte.TryParse(byteNumReplacedHex, NumberStyles.AllowHexSpecifier, null, out byteNumReplaced))
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("The byte number specified is not valid."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);

            // Do the replacement!
            bytes = HexEditTools.Replace(bytes, byteNum, byteNumReplaced, byteIdx + 1, byteIdx + 1);
            return bytes;
        }

        private static byte[] ReplaceAll(byte[] bytes)
        {
            // Get the current byte number and its hex
            byte byteNum = bytes[byteIdx];
            string byteNumHex = byteNum.ToString("X2");

            // Now, prompt for the replacement byte
            byte byteNumReplaced = default;
            string byteNumReplacedHex = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Write the byte number with the hexadecimal value to replace {0} with.") + " 00 -> FF.", InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor, byteNumHex);
            if (byteNumReplacedHex.Length != 2 ||
                byteNumReplacedHex.Length == 2 && !byte.TryParse(byteNumReplacedHex, NumberStyles.AllowHexSpecifier, null, out byteNumReplaced))
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("The byte number specified is not valid."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);

            // Do the replacement!
            bytes = HexEditTools.Replace(bytes, byteNum, byteNumReplaced);
            return bytes;
        }

        private static byte[] ReplaceAllWhat(byte[] bytes)
        {
            // Prompt and parse the number
            byte byteNum = default;
            string byteNumHex = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Write the byte number with the hexadecimal value to be replaced.") + " 00 -> FF.", InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            if (byteNumHex.Length != 2 ||
                byteNumHex.Length == 2 && !byte.TryParse(byteNumHex, NumberStyles.AllowHexSpecifier, null, out byteNum))
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("The byte number specified is not valid."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);

            // Now, prompt for the replacement byte
            byte byteNumReplaced = default;
            string byteNumReplacedHex = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Write the byte number with the hexadecimal value to replace {0} with.") + " 00 -> FF.", InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor, byteNumHex);
            if (byteNumReplacedHex.Length != 2 ||
                byteNumReplacedHex.Length == 2 && !byte.TryParse(byteNumReplacedHex, NumberStyles.AllowHexSpecifier, null, out byteNumReplaced))
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("The byte number specified is not valid."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);

            // Do the replacement!
            bytes = HexEditTools.Replace(bytes, byteNum, byteNumReplaced);
            return bytes;
        }

        private static byte[] NumInfo(byte[] bytes)
        {
            // Get the hex number in different formats
            byte byteNum = bytes[byteIdx];
            string byteNumHex = byteNum.ToString("X2");
            string byteNumOctal = Convert.ToString(byteNum, 8);
            string byteNumNumber = Convert.ToString(byteNum);
            string byteNumBinary = Convert.ToString(byteNum, 2);

            // Print the number information
            string header = Translate.DoTranslation("Number information:");
            int maxLength = header.Length > ConsoleWrapper.WindowWidth - 4 ? ConsoleWrapper.WindowWidth - 4 : header.Length;
            InfoBoxColor.WriteInfoBoxColorBack(
                header + CharManager.NewLine +
                new string('=', maxLength) + CharManager.NewLine + CharManager.NewLine +
                Translate.DoTranslation("Hexadecimal") + $": {byteNumHex}" + CharManager.NewLine +
                Translate.DoTranslation("Octal") + $": {byteNumOctal}" + CharManager.NewLine +
                Translate.DoTranslation("Number") + $": {byteNumNumber}" + CharManager.NewLine +
                Translate.DoTranslation("Binary") + $": {byteNumBinary}"
                , InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            return bytes;
        }

        private static void StatusNumInfo(byte[] bytes)
        {
            // Get the hex number in different formats
            byte byteNum = bytes[byteIdx];
            string byteNumHex = byteNum.ToString("X2");
            string byteNumOctal = Convert.ToString(byteNum, 8);
            string byteNumNumber = Convert.ToString(byteNum);
            string byteNumBinary = Convert.ToString(byteNum, 2);

            // Change the status to the number information
            status =
                Translate.DoTranslation("Hexadecimal") + $": {byteNumHex} | " +
                Translate.DoTranslation("Octal") + $": {byteNumOctal} | " +
                Translate.DoTranslation("Number") + $": {byteNumNumber} | " +
                Translate.DoTranslation("Binary") + $": {byteNumBinary}";
        }

        private static void PreviousPage(byte[] bytes)
        {
            int byteLinesPerPage = ConsoleWrapper.WindowHeight - 4;
            int currentSelection = byteIdx / 16;
            int currentPage = currentSelection / byteLinesPerPage;
            int startIndex = byteLinesPerPage * currentPage;
            int startByte = startIndex * 16;
            if (startByte > bytes.Length)
                startByte = bytes.Length;
            byteIdx = startByte - 1 < 0 ? 0 : startByte - 1;
        }

        private static void NextPage(byte[] bytes)
        {
            int byteLinesPerPage = ConsoleWrapper.WindowHeight - 4;
            int currentSelection = byteIdx / 16;
            int currentPage = currentSelection / byteLinesPerPage;
            int endIndex = byteLinesPerPage * (currentPage + 1);
            int startByte = endIndex * 16;
            if (startByte > bytes.Length - 1)
                startByte = bytes.Length - 1;
            byteIdx = startByte;
        }

        private static void Beginning() =>
            byteIdx = 0;

        private static void End(byte[] bytes) =>
            byteIdx = bytes.Length - 1;
    }
}

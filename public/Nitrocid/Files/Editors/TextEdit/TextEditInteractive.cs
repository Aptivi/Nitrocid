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
using KS.Files.Editors.TextEdit;
using KS.Files.Operations.Printing;
using KS.Files.Operations.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Screensaver;
using KS.Misc.Text;
using KS.Shell.Shells.Text;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Terminaux.Sequences.Tools;
using Terminaux.Sequences.Builder.Types;

namespace KS.Files.Editors.TextEdit
{
    /// <summary>
    /// Interactive text editor
    /// </summary>
    public static class TextEditInteractive
    {
        private static string status;
        private static bool bail;
        private static bool refresh = true;
        private static bool entering;
        private static int lineIdx = 0;
        private static int lineColIdx = 0;
        private static readonly TextEditorBinding[] bindings = new[]
        {
            new TextEditorBinding( /* Localizable */ "Exit", ConsoleKey.Escape, default, () => bail = true, true),
            new TextEditorBinding( /* Localizable */ "Keybindings", ConsoleKey.K, default, RenderKeybindingsBox, true),
            new TextEditorBinding( /* Localizable */ "Enter...", ConsoleKey.I, default, SwitchEnter, true),
            new TextEditorBinding( /* Localizable */ "Insert", ConsoleKey.F1, default, Insert, true),
            new TextEditorBinding( /* Localizable */ "Remove Line", ConsoleKey.F2, default, RemoveLine, true),
            new TextEditorBinding( /* Localizable */ "Insert", ConsoleKey.F1, ConsoleModifiers.Shift, InsertNoMove, true),
            new TextEditorBinding( /* Localizable */ "Remove Line", ConsoleKey.F2, ConsoleModifiers.Shift, RemoveLineNoMove, true),
            new TextEditorBinding( /* Localizable */ "Replace", ConsoleKey.F3, default, Replace, true),
            new TextEditorBinding( /* Localizable */ "Replace All", ConsoleKey.F3, ConsoleModifiers.Shift, ReplaceAll, true),
            new TextEditorBinding( /* Localizable */ "Left", ConsoleKey.LeftArrow, default, MoveBackward, true),
            new TextEditorBinding( /* Localizable */ "Right", ConsoleKey.RightArrow, default, MoveForward, true),
            new TextEditorBinding( /* Localizable */ "Up", ConsoleKey.UpArrow, default, MoveUp, true),
            new TextEditorBinding( /* Localizable */ "Down", ConsoleKey.DownArrow, default, MoveDown, true),
            new TextEditorBinding( /* Localizable */ "Previous page", ConsoleKey.PageUp, default, PreviousPage, true),
            new TextEditorBinding( /* Localizable */ "Next page", ConsoleKey.PageDown, default, NextPage, true),
            new TextEditorBinding( /* Localizable */ "Beginning", ConsoleKey.Home, default, Beginning, true),
            new TextEditorBinding( /* Localizable */ "End", ConsoleKey.End, default, End, true),
        };
        private static readonly TextEditorBinding[] bindingsEntering = new[]
        {
            new TextEditorBinding( /* Localizable */ "Stop Entering", ConsoleKey.Escape, default, SwitchEnter, true),
            new TextEditorBinding( /* Localizable */ "Insert", ConsoleKey.Enter, default, Insert, true),
            new TextEditorBinding( /* Localizable */ "Left", ConsoleKey.LeftArrow, default, MoveBackward, true),
            new TextEditorBinding( /* Localizable */ "Right", ConsoleKey.RightArrow, default, MoveForward, true),
            new TextEditorBinding( /* Localizable */ "Up", ConsoleKey.UpArrow, default, MoveUp, true),
            new TextEditorBinding( /* Localizable */ "Down", ConsoleKey.DownArrow, default, MoveDown, true),
            new TextEditorBinding( /* Localizable */ "Previous page", ConsoleKey.PageUp, default, PreviousPage, true),
            new TextEditorBinding( /* Localizable */ "Next page", ConsoleKey.PageDown, default, NextPage, true),
            new TextEditorBinding( /* Localizable */ "Beginning", ConsoleKey.Home, default, Beginning, true),
            new TextEditorBinding( /* Localizable */ "End", ConsoleKey.End, default, End, true),
        };

        /// <summary>
        /// Opens an interactive text editor
        /// </summary>
        /// <param name="file">Target file to open</param>
        public static void OpenInteractive(string file) =>
            OpenInteractive(file, false);

        /// <summary>
        /// Opens an interactive text editor
        /// </summary>
        /// <param name="file">Target file to open</param>
        /// <param name="fromShell">Whether it's open from the text shell</param>
        internal static void OpenInteractive(string file, bool fromShell)
        {
            // Check to see if the file exists
            if (!Checking.FileExists(file))
                throw new KernelException(KernelExceptionType.TextEditor, Translate.DoTranslation("File not found.") + $" {file}");

            // Open the file
            if (!fromShell && !TextEditTools.OpenTextFile(file))
                throw new KernelException(KernelExceptionType.TextEditor, Translate.DoTranslation("Failed to open the text file.") + $" {file}");

            // Set status
            status = Translate.DoTranslation("Ready");
            refresh = true;
            bail = false;

            // Main loop
            lineIdx = 0;
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
                }

                // Render the box
                RenderTextViewBox();

                // Now, render the visual text with the current selection
                RenderContentsWithSelection(lineIdx);

                // Render the status
                RenderStatus();

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
                TextEditTools.CloseTextFile();
        }

        private static void RenderKeybindings()
        {
            var binds = entering ? bindingsEntering : bindings;
            foreach (TextEditorBinding binding in binds)
            {
                // First, check to see if the rendered binding info is going to exceed the console window width
                string renderedBinding = $" {binding.Key} {Translate.DoTranslation(binding.Name)}  ";
                bool canDraw = renderedBinding.Length + ConsoleWrapper.CursorLeft < ConsoleWrapper.WindowWidth - 3;
                if (canDraw)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Drawing binding {0} with description {1}...", binding.Key.ToString(), binding.Name);
                    TextWriterWhereColor.WriteWhereColorBack(GetBindingKeyShortcut(binding, false), ConsoleWrapper.CursorLeft + 0, ConsoleWrapper.WindowHeight - 1, BaseInteractiveTui.KeyBindingOptionColor, BaseInteractiveTui.OptionBackgroundColor);
                    TextWriterWhereColor.WriteWhereColorBack($"{(binding._localizable ? Translate.DoTranslation(binding.Name) : binding.Name)}  ", ConsoleWrapper.CursorLeft + 1, ConsoleWrapper.WindowHeight - 1, BaseInteractiveTui.OptionForegroundColor, KernelColorTools.GetColor(KernelColorType.Background));
                }
                else
                {
                    // We can't render anymore, so just break and write a binding to show more
                    DebugWriter.WriteDebug(DebugLevel.I, "Bailing because of no space...");
                    break;
                }
            }
        }

        private static void RenderStatus() =>
            TextWriterWhereColor.WriteWhereColorBack(status + ConsoleExtensions.GetClearLineToRightSequence(), 0, 0, BaseInteractiveTui.ForegroundColor, KernelColorTools.GetColor(KernelColorType.Background));

        private static void RenderTextViewBox()
        {
            // Get the widths and heights
            int SeparatorConsoleWidthInterior = ConsoleWrapper.WindowWidth - 2;
            int SeparatorMinimumHeight = 1;
            int SeparatorMaximumHeightInterior = ConsoleWrapper.WindowHeight - 4;

            // Render the box
            BorderColor.WriteBorder(0, SeparatorMinimumHeight, SeparatorConsoleWidthInterior, SeparatorMaximumHeightInterior, BaseInteractiveTui.PaneSeparatorColor, KernelColorTools.GetColor(KernelColorType.Background));
        }

        private static void RenderContentsWithSelection(int lineIdx)
        {
            // First, update the status
            StatusTextInfo();

            // Check the lines
            if (TextEditShellCommon.FileLines.Count == 0)
                return;

            // Get the widths and heights
            int SeparatorConsoleWidthInterior = ConsoleWrapper.WindowWidth - 2;
            int SeparatorMinimumHeightInterior = 2;

            // Get the colors
            var unhighlightedColorBackground = KernelColorTools.GetColor(KernelColorType.Background);
            var highlightedColorBackground = KernelColorTools.GetColor(KernelColorType.Success);

            // Get the start and the end indexes for lines
            int lineLinesPerPage = ConsoleWrapper.WindowHeight - 4;
            int currentPage = lineIdx / lineLinesPerPage;
            int startIndex = (lineLinesPerPage * currentPage) + 1;
            int endIndex = lineLinesPerPage * (currentPage + 1);
            if (startIndex > TextEditShellCommon.FileLines.Count)
                startIndex = TextEditShellCommon.FileLines.Count;
            if (endIndex > TextEditShellCommon.FileLines.Count)
                endIndex = TextEditShellCommon.FileLines.Count;

            // Get the lines and highlight the selection
            int count = 0;
            var sels = new StringBuilder();
            for (int i = startIndex; i <= endIndex; i++)
            {
                // Get a line
                string source = TextEditShellCommon.FileLines[i - 1];
                if (source.Length == 0)
                    source = " ";

                // Highlight the selection
                var lineBuilder = new StringBuilder(source);
                if (i == lineIdx + 1)
                {
                    lineBuilder.Insert(lineColIdx + 1, unhighlightedColorBackground.VTSequenceBackground);
                    lineBuilder.Insert(lineColIdx + 1, highlightedColorBackground.VTSequenceForeground);
                    lineBuilder.Insert(lineColIdx, unhighlightedColorBackground.VTSequenceForeground);
                    lineBuilder.Insert(lineColIdx, highlightedColorBackground.VTSequenceBackground);
                }

                // Now, get the line range
                string line = lineBuilder.ToString();
                if (source.Length > 0)
                {
                    int charsPerPage = SeparatorConsoleWidthInterior;
                    int currentCharPage = lineColIdx / charsPerPage;
                    int startLineIndex = (charsPerPage * currentCharPage);
                    int endLineIndex = charsPerPage * (currentCharPage + 1);
                    if (startLineIndex > source.Length)
                        startLineIndex = source.Length;
                    if (endLineIndex > source.Length)
                        endLineIndex = source.Length;
                    int vtSeqLength = 0;
                    var vtSeqMatches = VtSequenceTools.MatchVTSequences(line);
                    foreach (var match in vtSeqMatches)
                        vtSeqLength += match.Sum((mat) => mat.Length);
                    source = source[startLineIndex..endLineIndex];
                    line = line[startLineIndex..(endLineIndex + vtSeqLength)];
                }
                line += new string(' ', SeparatorConsoleWidthInterior - source.Length);

                // Change the color depending on the highlighted line and column
                sels.Append(
                    $"{CsiSequences.GenerateCsiCursorPosition(2, SeparatorMinimumHeightInterior + count + 1)}" +
                    $"{highlightedColorBackground.VTSequenceForeground}" +
                    $"{unhighlightedColorBackground.VTSequenceBackground}" +
                    line
                );
                count++;
            }

            // Render the selections
            TextWriterColor.WritePlain(sels.ToString());
        }

        private static void HandleKeypress(ConsoleKeyInfo key)
        {
            var binds = entering ? bindingsEntering : bindings;

            // Check to see if we have this binding
            if (!binds.Any((heb) => heb.Key == key.Key && heb.KeyModifiers == key.Modifiers))
            {
                if (entering)
                {
                    // Insert a new character or delete it if backspace is entered
                    if (key.Key == ConsoleKey.Backspace)
                        RuboutChar();
                    else if (key.Key == ConsoleKey.Delete)
                        DeleteChar();
                    else
                        InsertChar(key.KeyChar);
                }
                return;
            }

            // Now, get the first binding and execute it.
            var bind = binds
                .First((heb) => heb.Key == key.Key && heb.KeyModifiers == key.Modifiers);
            bind.Action();
        }

        private static void InsertChar(char keyChar)
        {
            // Check the lines
            if (TextEditShellCommon.FileLines.Count == 0)
                return;

            // Insert a character
            TextEditShellCommon.FileLines[lineIdx] = TextEditShellCommon.FileLines[lineIdx].Insert(TextEditShellCommon.FileLines[lineIdx].Length == 0 ? 0 : lineColIdx, $"{keyChar}");
            MoveForward();
        }

        private static void RuboutChar()
        {
            // Check the lines
            if (TextEditShellCommon.FileLines.Count == 0)
                return;
            if (TextEditShellCommon.FileLines[lineIdx].Length == 0)
                return;
            if (lineColIdx == 0)
                return;

            // Delete a character
            TextEditShellCommon.FileLines[lineIdx] = TextEditShellCommon.FileLines[lineIdx].Remove(lineColIdx - 1, 1);
            MoveBackward();
        }

        private static void DeleteChar()
        {
            // Check the lines
            if (TextEditShellCommon.FileLines.Count == 0)
                return;
            if (TextEditShellCommon.FileLines[lineIdx].Length == 0)
                return;

            // Delete a character
            TextEditShellCommon.FileLines[lineIdx] = TextEditShellCommon.FileLines[lineIdx].Remove(lineColIdx, 1);
            UpdateLineIndex(lineIdx);
        }

        private static void RenderKeybindingsBox()
        {
            var binds = entering ? bindingsEntering : bindings;

            // Show the available keys list
            if (binds.Length == 0)
                return;

            // User needs an infobox that shows all available keys
            string section = Translate.DoTranslation("Available keys");
            int maxBindingLength = binds
                .Max((heb) => GetBindingKeyShortcut(heb).Length);
            string[] bindingRepresentations = binds
                .Select((heb) => $"{GetBindingKeyShortcut(heb) + new string(' ', maxBindingLength - GetBindingKeyShortcut(heb).Length) + $" | {(heb._localizable ? Translate.DoTranslation(heb.Name) : heb.Name)}"}")
                .ToArray();
            InfoBoxColor.WriteInfoBoxColorBack(
                $"{section}{CharManager.NewLine}" +
                $"{new string('=', section.Length)}{CharManager.NewLine}{CharManager.NewLine}" +
                $"{string.Join('\n', bindingRepresentations)}"
            , BaseInteractiveTui.BoxForegroundColor, BaseInteractiveTui.BoxBackgroundColor);
            refresh = true;
        }

        private static string GetBindingKeyShortcut(TextEditorBinding bind, bool mark = true)
        {
            string markStart = mark ? "[" : " ";
            string markEnd = mark ? "]" : " ";
            return $"{markStart}{(bind.KeyModifiers != 0 ? $"{bind.KeyModifiers} + " : "")}{bind.Key}{markEnd}";
        }

        private static void MoveBackward() =>
            UpdateColumnIndex(lineColIdx - 1);

        private static void MoveForward() =>
            UpdateColumnIndex(lineColIdx + 1);

        private static void MoveUp() =>
            UpdateLineIndex(lineIdx - 1);

        private static void MoveDown() =>
            UpdateLineIndex(lineIdx + 1);

        private static void Insert()
        {
            // Insert a line
            TextEditShellCommon.FileLines.Insert(TextEditShellCommon.FileLines.Count == 0 ? 0 : lineIdx + 1, "");
            MoveDown();
        }

        private static void RemoveLine()
        {
            // Check the lines
            if (TextEditShellCommon.FileLines.Count == 0)
                return;

            // Remove a line
            TextEditTools.RemoveLine(lineIdx + 1);
            MoveUp();
        }

        private static void InsertNoMove()
        {
            // Insert a line
            TextEditShellCommon.FileLines.Insert(TextEditShellCommon.FileLines.Count == 0 ? 0 : lineIdx + 1, "");
            UpdateLineIndex(lineIdx);
        }

        private static void RemoveLineNoMove()
        {
            // Check the lines
            if (TextEditShellCommon.FileLines.Count == 0)
                return;

            // Remove a line
            TextEditTools.RemoveLine(lineIdx + 1);
            UpdateLineIndex(lineIdx);
        }

        private static void Replace()
        {
            // Check the lines
            if (TextEditShellCommon.FileLines.Count == 0)
                return;

            // Now, prompt for the replacement line
            string replacementText = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Write the string to find"), BaseInteractiveTui.BoxForegroundColor, BaseInteractiveTui.BoxBackgroundColor);
            string replacedText = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Write the replacement string"), BaseInteractiveTui.BoxForegroundColor, BaseInteractiveTui.BoxBackgroundColor);

            // Do the replacement!
            TextEditTools.Replace(replacementText, replacedText, lineIdx + 1);
            refresh = true;
        }

        private static void ReplaceAll()
        {
            // Check the lines
            if (TextEditShellCommon.FileLines.Count == 0)
                return;

            // Now, prompt for the replacement line
            string replacementText = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Write the string to find"), BaseInteractiveTui.BoxForegroundColor, BaseInteractiveTui.BoxBackgroundColor);
            string replacedText = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Write the replacement string"), BaseInteractiveTui.BoxForegroundColor, BaseInteractiveTui.BoxBackgroundColor);

            // Do the replacement!
            TextEditTools.Replace(replacementText, replacedText);
            refresh = true;
        }

        private static void StatusTextInfo()
        {
            // Get the status
            status =
                Translate.DoTranslation("Lines") + $": {TextEditShellCommon.FileLines.Count} | " +
                Translate.DoTranslation("Column") + $": {lineColIdx + 1} | " +
                Translate.DoTranslation("Row") + $": {lineIdx + 1}";
        }

        private static void PreviousPage()
        {
            int lineLinesPerPage = ConsoleWrapper.WindowHeight - 4;
            int currentPage = lineIdx / lineLinesPerPage;
            int startIndex = lineLinesPerPage * currentPage;
            if (startIndex > TextEditShellCommon.FileLines.Count)
                startIndex = TextEditShellCommon.FileLines.Count;
            UpdateLineIndex(startIndex - 1 < 0 ? 0 : startIndex - 1);
        }

        private static void NextPage()
        {
            int lineLinesPerPage = ConsoleWrapper.WindowHeight - 4;
            int currentPage = lineIdx / lineLinesPerPage;
            int endIndex = lineLinesPerPage * (currentPage + 1);
            if (endIndex > TextEditShellCommon.FileLines.Count - 1)
                endIndex = TextEditShellCommon.FileLines.Count - 1;
            UpdateLineIndex(endIndex);
        }

        private static void Beginning() =>
            UpdateLineIndex(0);

        private static void End() =>
            UpdateLineIndex(TextEditShellCommon.FileLines.Count - 1);

        private static void UpdateLineIndex(int lnIdx)
        {
            lineIdx = lnIdx;
            if (lineIdx > TextEditShellCommon.FileLines.Count - 1)
                lineIdx = TextEditShellCommon.FileLines.Count - 1;
            if (lineIdx < 0)
                lineIdx = 0;
            UpdateColumnIndex(lineColIdx);
        }

        private static void UpdateColumnIndex(int clIdx)
        {
            lineColIdx = clIdx;
            if (TextEditShellCommon.FileLines.Count == 0)
            {
                lineColIdx = 0;
                return;
            }
            if (lineColIdx > TextEditShellCommon.FileLines[lineIdx].Length - 1)
                lineColIdx = TextEditShellCommon.FileLines[lineIdx].Length - 1;
            if (lineColIdx < 0)
                lineColIdx = 0;
        }

        private static void SwitchEnter()
        {
            entering = !entering;
            refresh = true;
        }
    }
}

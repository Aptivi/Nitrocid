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
using System.Linq;
using System.Text;
using Textify.Sequences.Tools;
using Textify.Sequences.Builder.Types;
using System.Collections.Generic;
using Nitrocid.Shell.Shells.Text;
using Nitrocid.Kernel.Debugging;
using Nitrocid.ConsoleBase;
using Nitrocid.ConsoleBase.Inputs;
using Nitrocid.Languages;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.ConsoleBase.Writers.FancyWriters;
using Nitrocid.Misc.Text;
using Nitrocid.ConsoleBase.Inputs.Styles.Infobox;
using Nitrocid.ConsoleBase.Writers.ConsoleWriters;
using Nitrocid.ConsoleBase.Interactive;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Buffered;
using Nitrocid.Files.Operations.Querying;

namespace Nitrocid.Files.Editors.TextEdit
{
    /// <summary>
    /// Interactive text editor
    /// </summary>
    public static class TextEditInteractive
    {
        private static string status;
        private static bool bail;
        private static bool entering;
        private static int lineIdx = 0;
        private static int lineColIdx = 0;
        private static readonly TextEditorBinding[] bindings =
        [
            new TextEditorBinding( /* Localizable */ "Exit", ConsoleKey.Escape, default, (l) => { bail = true; return l; }, true),
            new TextEditorBinding( /* Localizable */ "Keybindings", ConsoleKey.K, default, RenderKeybindingsBox, true),
            new TextEditorBinding( /* Localizable */ "Enter...", ConsoleKey.I, default, SwitchEnter, true),
            new TextEditorBinding( /* Localizable */ "Insert", ConsoleKey.F1, default, Insert, true),
            new TextEditorBinding( /* Localizable */ "Remove Line", ConsoleKey.F2, default, RemoveLine, true),
            new TextEditorBinding( /* Localizable */ "Insert", ConsoleKey.F1, ConsoleModifiers.Shift, InsertNoMove, true),
            new TextEditorBinding( /* Localizable */ "Remove Line", ConsoleKey.F2, ConsoleModifiers.Shift, RemoveLineNoMove, true),
            new TextEditorBinding( /* Localizable */ "Replace", ConsoleKey.F3, default, Replace, true),
            new TextEditorBinding( /* Localizable */ "Replace All", ConsoleKey.F3, ConsoleModifiers.Shift, ReplaceAll, true),
        ];
        private static readonly TextEditorBinding[] bindingsEntering =
        [
            new TextEditorBinding( /* Localizable */ "Stop Entering", ConsoleKey.Escape, default, SwitchEnter, true),
            new TextEditorBinding( /* Localizable */ "New Line", ConsoleKey.Enter, default, Insert, true),
        ];

        /// <summary>
        /// Opens an interactive text editor
        /// </summary>
        /// <param name="file">Target file to open</param>
        public static void OpenInteractive(string file) =>
            OpenInteractive(file, ref TextEditShellCommon.fileLines);

        /// <summary>
        /// Opens an interactive text editor
        /// </summary>
        /// <param name="file">Target file to open</param>
        /// <param name="lines">Target number of lines</param>
        internal static void OpenInteractive(string file, ref List<string> lines)
        {
            // Check to see if the file exists
            if (!Checking.FileExists(file))
                throw new KernelException(KernelExceptionType.TextEditor, Translate.DoTranslation("File not found.") + $" {file}");

            // Set status
            status = Translate.DoTranslation("Ready");
            bail = false;

            // Main loop
            lineIdx = 0;
            var screen = new Screen();
            ScreenTools.SetCurrent(screen);
            ConsoleWrapper.CursorVisible = false;
            KernelColorTools.LoadBack();
            try
            {
                while (!bail)
                {
                    // Now, render the keybindings
                    RenderKeybindings(ref screen);

                    // Render the box
                    RenderTextViewBox(ref screen);

                    // Now, render the visual hex with the current selection
                    RenderContentsWithSelection(lineIdx, ref screen, lines);

                    // Render the status
                    RenderStatus(ref screen);

                    // Wait for a keypress
                    ScreenTools.Render(screen);
                    var keypress = Input.DetectKeypress();
                    HandleKeypress(keypress, ref lines);

                    // Reset, in case selection changed
                    screen.RemoveBufferedParts();
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, $"Text editor failed: {ex.Message}");
                DebugWriter.WriteDebugStackTrace(ex);
                InfoBoxColor.WriteInfoBoxKernelColor(Translate.DoTranslation("The text editor failed:") + $" {ex.Message}", KernelColorType.Error);
            }
            bail = false;
            ScreenTools.UnsetCurrent(screen);

            // Close the file and clean up
            KernelColorTools.LoadBack();
        }

        private static void RenderKeybindings(ref Screen screen)
        {
            // Make a screen part
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var binds = entering ? bindingsEntering : bindings;
                var bindingsBuilder = new StringBuilder(CsiSequences.GenerateCsiCursorPosition(1, ConsoleWrapper.WindowHeight));
                foreach (TextEditorBinding binding in binds)
                {
                    // First, check to see if the rendered binding info is going to exceed the console window width
                    string renderedBinding = $"{GetBindingKeyShortcut(binding, false)} {(binding._localizable ? Translate.DoTranslation(binding.Name) : binding.Name)}  ";
                    int actualLength = VtSequenceTools.FilterVTSequences(bindingsBuilder.ToString()).Length;
                    bool canDraw = renderedBinding.Length + actualLength < ConsoleWrapper.WindowWidth - 3;
                    if (canDraw)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Drawing binding {0} with description {1}...", GetBindingKeyShortcut(binding, false), binding.Name);
                        bindingsBuilder.Append(
                            $"{BaseInteractiveTui.KeyBindingOptionColor.VTSequenceForeground}" +
                            $"{BaseInteractiveTui.OptionBackgroundColor.VTSequenceBackground}" +
                            GetBindingKeyShortcut(binding, false) +
                            $"{BaseInteractiveTui.OptionForegroundColor.VTSequenceForeground}" +
                            $"{BaseInteractiveTui.BackgroundColor.VTSequenceBackground}" +
                            $" {(binding._localizable ? Translate.DoTranslation(binding.Name) : binding.Name)}  "
                        );
                    }
                    else
                    {
                        // We can't render anymore, so just break and write a binding to show more
                        DebugWriter.WriteDebug(DebugLevel.I, "Bailing because of no space...");
                        bindingsBuilder.Append(
                            $"{CsiSequences.GenerateCsiCursorPosition(ConsoleWrapper.WindowWidth - 2, ConsoleWrapper.WindowHeight)}" +
                            $"{BaseInteractiveTui.KeyBindingOptionColor.VTSequenceForeground}" +
                            $"{BaseInteractiveTui.OptionBackgroundColor.VTSequenceBackground}" +
                            " K "
                        );
                        break;
                    }
                }
                return bindingsBuilder.ToString();
            });
            screen.AddBufferedPart("Text editor interactive - Keybindings", part);
        }

        private static void RenderStatus(ref Screen screen)
        {
            // Make a screen part
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                var builder = new StringBuilder();
                builder.Append(
                    $"{BaseInteractiveTui.ForegroundColor.VTSequenceForeground}" +
                    $"{KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground}" +
                    $"{TextWriterWhereColor.RenderWherePlain(status + ConsoleExtensions.GetClearLineToRightSequence(), 0, 0)}"
                );
                return builder.ToString();
            });
            screen.AddBufferedPart("Text editor interactive - Status", part);
        }

        private static void RenderTextViewBox(ref Screen screen)
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
                    $"{BaseInteractiveTui.PaneSeparatorColor.VTSequenceForeground}" +
                    $"{KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground}" +
                    $"{BorderColor.RenderBorderPlain(0, SeparatorMinimumHeight, SeparatorConsoleWidthInterior, SeparatorMaximumHeightInterior)}"
                );
                return builder.ToString();
            });
            screen.AddBufferedPart("Text editor interactive - Text view box", part);
        }

        private static void RenderContentsWithSelection(int lineIdx, ref Screen screen, List<string> lines)
        {
            // First, update the status
            StatusTextInfo(lines);

            // Check the lines
            if (lines.Count == 0)
                return;

            // Now, make a dynamic text
            var part = new ScreenPart();
            part.AddDynamicText(() =>
            {
                // Get the widths and heights
                int SeparatorConsoleWidthInterior = ConsoleWrapper.WindowWidth - 2;
                int SeparatorMinimumHeightInterior = 2;

                // Get the colors
                var unhighlightedColorBackground = KernelColorTools.GetColor(KernelColorType.Background);
                var highlightedColorBackground = KernelColorTools.GetColor(KernelColorType.Success);

                // Get the start and the end indexes for lines
                int lineLinesPerPage = ConsoleWrapper.WindowHeight - 4;
                int currentPage = lineIdx / lineLinesPerPage;
                int startIndex = lineLinesPerPage * currentPage + 1;
                int endIndex = lineLinesPerPage * (currentPage + 1);
                if (startIndex > lines.Count)
                    startIndex = lines.Count;
                if (endIndex > lines.Count)
                    endIndex = lines.Count;

                // Get the lines and highlight the selection
                int count = 0;
                var sels = new StringBuilder();
                for (int i = startIndex; i <= endIndex; i++)
                {
                    // Get a line
                    string source = lines[i - 1].Replace("\t", ">");
                    if (source.Length == 0)
                        source = " ";

                    // Seek through the whole string to find unprintable characters
                    var sourceBuilder = new StringBuilder();
                    for (int l = 0; l < source.Length; l++)
                    {
                        bool unprintable = CharManager.IsControlChar(source[l]) || source[l] == '\0' || source[l] == (char)0xAD;
                        string rendered = unprintable ? "." : source[l].ToString();
                        sourceBuilder.Append(rendered);
                    }
                    source = sourceBuilder.ToString();

                    // Highlight the selection
                    var lineBuilder = new StringBuilder(source);
                    if (i == lineIdx + 1)
                    {
                        if (lineColIdx + 1 > lineBuilder.Length)
                        {
                            lineBuilder.Append(unhighlightedColorBackground.VTSequenceForeground);
                            lineBuilder.Append(highlightedColorBackground.VTSequenceBackground);
                            lineBuilder.Append(' ');
                            lineBuilder.Append(unhighlightedColorBackground.VTSequenceBackground);
                            lineBuilder.Append(highlightedColorBackground.VTSequenceForeground);
                        }
                        else
                        {
                            lineBuilder.Insert(lineColIdx + 1, unhighlightedColorBackground.VTSequenceBackground);
                            lineBuilder.Insert(lineColIdx + 1, highlightedColorBackground.VTSequenceForeground);
                            lineBuilder.Insert(lineColIdx, unhighlightedColorBackground.VTSequenceForeground);
                            lineBuilder.Insert(lineColIdx, highlightedColorBackground.VTSequenceBackground);
                        }
                    }

                    // Now, get the line range
                    string line = lineBuilder.ToString();
                    if (source.Length > 0)
                    {
                        int charsPerPage = SeparatorConsoleWidthInterior;
                        int currentCharPage = lineColIdx / charsPerPage;
                        int startLineIndex = charsPerPage * currentCharPage;
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
                return sels.ToString();
            });
            screen.AddBufferedPart("Text editor interactive - Contents", part);
        }

        private static void HandleKeypress(ConsoleKeyInfo key, ref List<string> lines)
        {
            var binds = entering ? bindingsEntering : bindings;

            // Check to see if we have this binding
            if (!binds.Any((heb) => heb.Key == key.Key && heb.KeyModifiers == key.Modifiers))
            {
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        MoveBackward(lines);
                        return;
                    case ConsoleKey.RightArrow:
                        MoveForward(lines);
                        return;
                    case ConsoleKey.UpArrow:
                        MoveUp(lines);
                        return;
                    case ConsoleKey.DownArrow:
                        MoveDown(lines);
                        return;
                    case ConsoleKey.PageUp:
                        PreviousPage(lines);
                        return;
                    case ConsoleKey.PageDown:
                        NextPage(lines);
                        return;
                    case ConsoleKey.Home:
                        Beginning(lines);
                        return;
                    case ConsoleKey.End:
                        End(lines);
                        return;
                }
                if (entering)
                {
                    // Handle the entering keys apppropriately
                    if (key.Key == ConsoleKey.Backspace)
                        RuboutChar(lines);
                    else if (key.Key == ConsoleKey.Delete)
                        DeleteChar(lines);
                    else
                        InsertChar(key.KeyChar, lines);
                }
                return;
            }

            // Now, get the first binding and execute it.
            var bind = binds
                .First((heb) => heb.Key == key.Key && heb.KeyModifiers == key.Modifiers);
            lines = bind.Action(lines);
        }

        private static List<string> InsertChar(char keyChar, List<string> lines)
        {
            // Check the lines
            if (lines.Count == 0)
                return lines;

            // Insert a character
            lines[lineIdx] = lines[lineIdx].Insert(lines[lineIdx].Length == 0 ? 0 : lineColIdx, $"{keyChar}");
            MoveForward(lines);
            return lines;
        }

        private static List<string> RuboutChar(List<string> lines)
        {
            // Check the lines
            if (lines.Count == 0)
                return lines;
            if (lines[lineIdx].Length == 0)
                return lines;
            if (lineColIdx == 0)
                return lines;

            // Delete a character
            lines[lineIdx] = lines[lineIdx].Remove(lineColIdx - 1, 1);
            MoveBackward(lines);
            return lines;
        }

        private static List<string> DeleteChar(List<string> lines)
        {
            // Check the lines
            if (lines.Count == 0)
                return lines;
            if (lines[lineIdx].Length == 0 ||
                lines[lineIdx].Length == lineColIdx)
                return lines;

            // Delete a character
            lines[lineIdx] = lines[lineIdx].Remove(lineColIdx, 1);
            UpdateLineIndex(lineIdx, lines);
            return lines;
        }

        private static List<string> RenderKeybindingsBox(List<string> lines)
        {
            var binds = entering ? bindingsEntering : bindings;

            // Show the available keys list
            if (binds.Length == 0)
                return lines;

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
            return lines;
        }

        private static string GetBindingKeyShortcut(TextEditorBinding bind, bool mark = true)
        {
            string markStart = mark ? "[" : " ";
            string markEnd = mark ? "]" : " ";
            return $"{markStart}{(bind.KeyModifiers != 0 ? $"{bind.KeyModifiers} + " : "")}{bind.Key}{markEnd}";
        }

        private static void MoveBackward(List<string> lines) =>
            UpdateColumnIndex(lineColIdx - 1, lines);

        private static void MoveForward(List<string> lines) =>
            UpdateColumnIndex(lineColIdx + 1, lines);

        private static void MoveUp(List<string> lines) =>
            UpdateLineIndex(lineIdx - 1, lines);

        private static void MoveDown(List<string> lines) =>
            UpdateLineIndex(lineIdx + 1, lines);

        private static List<string> Insert(List<string> lines)
        {
            // Insert a line
            if (lines.Count == 0)
                lines.Add("");
            else
                lines.Insert(lineIdx + 1, "");
            MoveDown(lines);
            return lines;
        }

        private static List<string> RemoveLine(List<string> lines)
        {
            // Check the lines
            if (lines.Count == 0)
                return lines;

            // Remove a line
            lines = TextEditTools.RemoveLine(lines, lineIdx + 1);
            MoveUp(lines);
            return lines;
        }

        private static List<string> InsertNoMove(List<string> lines)
        {
            // Insert a line
            if (lines.Count == 0)
                lines.Add("");
            else
                lines.Insert(lineIdx + 1, "");
            UpdateLineIndex(lineIdx, lines);
            return lines;
        }

        private static List<string> RemoveLineNoMove(List<string> lines)
        {
            // Check the lines
            if (lines.Count == 0)
                return lines;

            // Remove a line
            lines = TextEditTools.RemoveLine(lines, lineIdx + 1);
            UpdateLineIndex(lineIdx, lines);
            return lines;
        }

        private static List<string> Replace(List<string> lines)
        {
            // Check the lines
            if (lines.Count == 0)
                return lines;

            // Now, prompt for the replacement line
            string replacementText = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Write the string to find"), BaseInteractiveTui.BoxForegroundColor, BaseInteractiveTui.BoxBackgroundColor);
            string replacedText = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Write the replacement string"), BaseInteractiveTui.BoxForegroundColor, BaseInteractiveTui.BoxBackgroundColor);

            // Do the replacement!
            lines = TextEditTools.Replace(lines, replacementText, replacedText, lineIdx + 1);
            return lines;
        }

        private static List<string> ReplaceAll(List<string> lines)
        {
            // Check the lines
            if (lines.Count == 0)
                return lines;

            // Now, prompt for the replacement line
            string replacementText = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Write the string to find"), BaseInteractiveTui.BoxForegroundColor, BaseInteractiveTui.BoxBackgroundColor);
            string replacedText = InfoBoxInputColor.WriteInfoBoxInputColorBack(Translate.DoTranslation("Write the replacement string"), BaseInteractiveTui.BoxForegroundColor, BaseInteractiveTui.BoxBackgroundColor);

            // Do the replacement!
            lines = TextEditTools.Replace(lines, replacementText, replacedText);
            return lines;
        }

        private static void StatusTextInfo(List<string> lines)
        {
            // Get the status
            status =
                Translate.DoTranslation("Lines") + $": {lines.Count} | " +
                Translate.DoTranslation("Column") + $": {lineColIdx + 1} | " +
                Translate.DoTranslation("Row") + $": {lineIdx + 1}";

            // Check to see if the current character is unprintable
            if (lines.Count == 0)
                return;
            if (lines[lineIdx].Length == 0)
                return;
            if (entering)
                return;
            var currChar = lines[lineIdx][lineColIdx];
            if (CharManager.IsControlChar(currChar) || currChar == '\0' || currChar == (char)0xAD)
                status += " | " + Translate.DoTranslation("Bin") + $": {(int)currChar}";
            if (currChar == '\t')
                status += " | " + Translate.DoTranslation("Tab") + $": {(int)currChar}";
        }

        private static void PreviousPage(List<string> lines)
        {
            int lineLinesPerPage = ConsoleWrapper.WindowHeight - 4;
            int currentPage = lineIdx / lineLinesPerPage;
            int startIndex = lineLinesPerPage * currentPage;
            if (startIndex > lines.Count)
                startIndex = lines.Count;
            UpdateLineIndex(startIndex - 1 < 0 ? 0 : startIndex - 1, lines);
        }

        private static void NextPage(List<string> lines)
        {
            int lineLinesPerPage = ConsoleWrapper.WindowHeight - 4;
            int currentPage = lineIdx / lineLinesPerPage;
            int endIndex = lineLinesPerPage * (currentPage + 1);
            if (endIndex > lines.Count - 1)
                endIndex = lines.Count - 1;
            UpdateLineIndex(endIndex, lines);
        }

        private static void Beginning(List<string> lines) =>
            UpdateLineIndex(0, lines);

        private static void End(List<string> lines) =>
            UpdateLineIndex(lines.Count - 1, lines);

        private static void UpdateLineIndex(int lnIdx, List<string> lines)
        {
            lineIdx = lnIdx;
            if (lineIdx > lines.Count - 1)
                lineIdx = lines.Count - 1;
            if (lineIdx < 0)
                lineIdx = 0;
            UpdateColumnIndex(lineColIdx, lines);
        }

        private static void UpdateColumnIndex(int clIdx, List<string> lines)
        {
            lineColIdx = clIdx;
            if (lines.Count == 0)
            {
                lineColIdx = 0;
                return;
            }
            int maxLen = lines[lineIdx].Length;
            maxLen -= entering ? 0 : 1;
            if (lineColIdx > maxLen)
                lineColIdx = maxLen;
            if (lineColIdx < 0)
                lineColIdx = 0;
        }

        private static List<string> SwitchEnter(List<string> lines)
        {
            entering = !entering;
            UpdateLineIndex(lineIdx, lines);
            return lines;
        }
    }
}

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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using KS.ConsoleBase.Inputs;
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Languages;
using Terminaux.Colors;
using System.Text;
using KS.Misc.Text;

namespace KS.ConsoleBase.Writers.ConsoleWriters
{
    /// <summary>
    /// List writer with color support
    /// </summary>
    public static class ListWriterColor
    {
        #region Dictionary
        /// <summary>
        /// Outputs the list entries into the terminal prompt plainly. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        public static void WriteListPlain<TKey, TValue>(Dictionary<TKey, TValue> List) =>
            WriteListPlain(List, ConsoleExtensions.WrapListOutputs);

        /// <summary>
        /// Outputs the list entries into the terminal prompt plainly, and wraps output if needed.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteListPlain<TKey, TValue>(Dictionary<TKey, TValue> List, bool Wrap)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Variables
                    var LinesMade = 0;

                    // Try to write list to console
                    string buffered = RenderList(List);
                    string[] bufferedLines = TextTools.GetWrappedSentences(buffered, ConsoleWrapper.WindowWidth);
                    var buffer = new StringBuilder();
                    for (int idx = 0; idx < bufferedLines.Length; idx++)
                    {
                        string bufferedLine = bufferedLines[idx];
                        var Values = new List<object>();
                        buffer.Append(bufferedLine);
                        if (idx < bufferedLines.Length - 1)
                            buffer.AppendLine();

                        if (Wrap)
                        {
                            LinesMade += 1;
                            if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                            {
                                TextWriterColor.WritePlain(buffer.ToString(), false);
                                buffer.Clear();
                                if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                    break;
                                LinesMade = 0;
                            }
                        }
                        else if (ConsoleWrapper.KeyAvailable)
                        {
                            TextWriterColor.WritePlain(buffer.ToString(), false);
                            buffer.Clear();
                            if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                break;
                        }
                    }
                    TextWriterColor.WritePlain(buffer.ToString(), false);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the list entries into the terminal prompt. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List) =>
            WriteList(List, ConsoleExtensions.WrapListOutputs);

        /// <summary>
        /// Outputs the list entries into the terminal prompt, and wraps output if needed.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, bool Wrap) =>
            WriteList(List, KernelColorTools.GetColor(KernelColorType.ListEntry), KernelColorTools.GetColor(KernelColorType.ListValue), Wrap);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, KernelColorType ListKeyColor, KernelColorType ListValueColor) =>
            WriteList(List, ListKeyColor, ListValueColor, ConsoleExtensions.WrapListOutputs);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, KernelColorType ListKeyColor, KernelColorType ListValueColor, bool Wrap) =>
            WriteList(List, KernelColorTools.GetColor(ListKeyColor), KernelColorTools.GetColor(ListValueColor), Wrap);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, ConsoleColors ListKeyColor, ConsoleColors ListValueColor) =>
            WriteList(List, ListKeyColor, ListValueColor, ConsoleExtensions.WrapListOutputs);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, ConsoleColors ListKeyColor, ConsoleColors ListValueColor, bool Wrap) =>
            WriteList(List, new Color(ListKeyColor), new Color(ListValueColor), Wrap);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, Color ListKeyColor, Color ListValueColor) =>
            WriteList(List, ListKeyColor, ListValueColor, ConsoleExtensions.WrapListOutputs);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<TKey, TValue>(Dictionary<TKey, TValue> List, Color ListKeyColor, Color ListValueColor, bool Wrap)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Variables
                    var LinesMade = 0;

                    // Try to write list to console
                    string buffered = RenderList(List, ListKeyColor, ListValueColor);
                    string[] bufferedLines = TextTools.GetWrappedSentences(buffered, ConsoleWrapper.WindowWidth);
                    var buffer = new StringBuilder();
                    for (int idx = 0; idx < bufferedLines.Length; idx++)
                    {
                        string bufferedLine = bufferedLines[idx];
                        var Values = new List<object>();
                        buffer.Append(bufferedLine);
                        if (idx < bufferedLines.Length - 1)
                            buffer.AppendLine();

                        if (Wrap)
                        {
                            LinesMade += 1;
                            if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                            {
                                TextWriterColor.WritePlain(buffer.ToString(), false);
                                buffer.Clear();
                                if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                    break;
                                LinesMade = 0;
                            }
                        }
                        else if (ConsoleWrapper.KeyAvailable)
                        {
                            TextWriterColor.WritePlain(buffer.ToString(), false);
                            buffer.Clear();
                            if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                break;
                        }
                    }
                    TextWriterColor.WritePlain(buffer.ToString(), false);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        public static string RenderList<TKey, TValue>(Dictionary<TKey, TValue> List)
        {
            var listBuilder = new StringBuilder();
            foreach (TKey ListEntry in List.Keys)
            {
                var Values = new List<object>();
                var value = List[ListEntry];
                if (value as IEnumerable is not null & value as string is null)
                {
                    foreach (var Value in (IEnumerable)value)
                        Values.Add(Value);
                    string valuesString = string.Join(", ", Values);
                    listBuilder.AppendLine($"- {ListEntry}: {valuesString}");
                }
                else
                    listBuilder.AppendLine($"- {ListEntry}: {value}");
            }
            return listBuilder.ToString();
        }

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static string RenderList<TKey, TValue>(Dictionary<TKey, TValue> List, Color ListKeyColor, Color ListValueColor)
        {
            var listBuilder = new StringBuilder();
            foreach (TKey ListEntry in List.Keys)
            {
                var Values = new List<object>();
                var value = List[ListEntry];
                if (value as IEnumerable is not null & value as string is null)
                {
                    foreach (var Value in (IEnumerable)value)
                        Values.Add(Value);
                    string valuesString = string.Join(", ", Values);
                    listBuilder.AppendLine(
                        $"{ListKeyColor.VTSequenceForeground}" +
                        $"- {ListEntry}: " +
                        $"{ListValueColor.VTSequenceForeground}" +
                        $"{valuesString}"
                    );
                }
                else
                    listBuilder.AppendLine(
                        $"{ListKeyColor.VTSequenceForeground}" +
                        $"- {ListEntry}: " +
                        $"{ListValueColor.VTSequenceForeground}" +
                        $"{value}"
                    );
            }
            listBuilder.Append(KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground);
            return listBuilder.ToString();
        }
        #endregion

        #region Enumerables
        /// <summary>
        /// Outputs the list entries into the terminal prompt plainly. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        public static void WriteListPlain<T>(IEnumerable<T> List) =>
            WriteListPlain(List, ConsoleExtensions.WrapListOutputs);

        /// <summary>
        /// Outputs the list entries into the terminal prompt plainly, and wraps output if needed.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteListPlain<T>(IEnumerable<T> List, bool Wrap)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Variables
                    var LinesMade = 0;

                    // Try to write list to console
                    string buffered = RenderList(List);
                    string[] bufferedLines = TextTools.GetWrappedSentences(buffered, ConsoleWrapper.WindowWidth);
                    var buffer = new StringBuilder();
                    for (int idx = 0; idx < bufferedLines.Length; idx++)
                    {
                        string bufferedLine = bufferedLines[idx];
                        var Values = new List<object>();
                        buffer.Append(bufferedLine);
                        if (idx < bufferedLines.Length - 1)
                            buffer.AppendLine();

                        if (Wrap)
                        {
                            LinesMade += 1;
                            if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                            {
                                TextWriterColor.WritePlain(buffer.ToString(), false);
                                buffer.Clear();
                                if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                    break;
                                LinesMade = 0;
                            }
                        }
                        else if (ConsoleWrapper.KeyAvailable)
                        {
                            TextWriterColor.WritePlain(buffer.ToString(), false);
                            buffer.Clear();
                            if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                break;
                        }
                    }
                    TextWriterColor.WritePlain(buffer.ToString(), false);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the list entries into the terminal prompt. It wraps output depending on the kernel settings.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        public static void WriteList<T>(IEnumerable<T> List) =>
            WriteList(List, ConsoleExtensions.WrapListOutputs);

        /// <summary>
        /// Outputs the list entries into the terminal prompt, and wraps output if needed.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<T>(IEnumerable<T> List, bool Wrap) =>
            WriteList(List, KernelColorTools.GetColor(KernelColorType.ListEntry), KernelColorTools.GetColor(KernelColorType.ListValue), Wrap);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<T>(IEnumerable<T> List, KernelColorType ListKeyColor, KernelColorType ListValueColor) =>
            WriteList(List, ListKeyColor, ListValueColor, ConsoleExtensions.WrapListOutputs);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<T>(IEnumerable<T> List, KernelColorType ListKeyColor, KernelColorType ListValueColor, bool Wrap) =>
            WriteList(List, KernelColorTools.GetColor(ListKeyColor), KernelColorTools.GetColor(ListValueColor), Wrap);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<T>(IEnumerable<T> List, ConsoleColors ListKeyColor, ConsoleColors ListValueColor) =>
            WriteList(List, ListKeyColor, ListValueColor, ConsoleExtensions.WrapListOutputs);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<T>(IEnumerable<T> List, ConsoleColors ListKeyColor, ConsoleColors ListValueColor, bool Wrap) =>
            WriteList(List, new Color(ListKeyColor), new Color(ListValueColor), Wrap);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteList<T>(IEnumerable<T> List, Color ListKeyColor, Color ListValueColor) =>
            WriteList(List, ListKeyColor, ListValueColor, ConsoleExtensions.WrapListOutputs);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="List">A dictionary that will be listed to the terminal prompt.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <param name="Wrap">Wraps the output as needed.</param>
        public static void WriteList<T>(IEnumerable<T> List, Color ListKeyColor, Color ListValueColor, bool Wrap)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Variables
                    var LinesMade = 0;

                    // Try to write list to console
                    string buffered = RenderList(List, ListKeyColor, ListValueColor);
                    string[] bufferedLines = TextTools.GetWrappedSentences(buffered, ConsoleWrapper.WindowWidth);
                    var buffer = new StringBuilder();
                    for (int idx = 0; idx < bufferedLines.Length; idx++)
                    {
                        string bufferedLine = bufferedLines[idx];
                        var Values = new List<object>();
                        buffer.Append(bufferedLine);
                        if (idx < bufferedLines.Length - 1)
                            buffer.AppendLine();

                        if (Wrap)
                        {
                            LinesMade += 1;
                            if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                            {
                                TextWriterColor.WritePlain(buffer.ToString(), false);
                                buffer.Clear();
                                if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                    break;
                                LinesMade = 0;
                            }
                        }
                        else if (ConsoleWrapper.KeyAvailable)
                        {
                            TextWriterColor.WritePlain(buffer.ToString(), false);
                            buffer.Clear();
                            if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                                break;
                        }
                    }
                    TextWriterColor.WritePlain(buffer.ToString(), false);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        public static string RenderList<T>(IEnumerable<T> List)
        {
            var listBuilder = new StringBuilder();
            int EntryNumber = 1;
            foreach (T ListEntry in List)
            {
                var Values = new List<object>();
                if (ListEntry as IEnumerable is not null & ListEntry as string is null)
                {
                    foreach (var Value in (IEnumerable)ListEntry)
                        Values.Add(Value);
                    string valuesString = string.Join(", ", Values);
                    listBuilder.AppendLine($"- {EntryNumber}: {valuesString}");
                }
                else
                    listBuilder.AppendLine($"- {EntryNumber}: {ListEntry}");
                EntryNumber += 1;
            }
            return listBuilder.ToString();
        }

        /// <summary>
        /// Renders the list entries.
        /// </summary>
        /// <param name="List">A dictionary that will be listed.</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static string RenderList<T>(IEnumerable<T> List, Color ListKeyColor, Color ListValueColor)
        {
            var listBuilder = new StringBuilder();
            int EntryNumber = 1;
            foreach (T ListEntry in List)
            {
                var Values = new List<object>();
                if (ListEntry as IEnumerable is not null & ListEntry as string is null)
                {
                    foreach (var Value in (IEnumerable)ListEntry)
                        Values.Add(Value);
                    string valuesString = string.Join(", ", Values);
                    listBuilder.AppendLine(
                        $"{ListKeyColor.VTSequenceForeground}" +
                        $"- {EntryNumber}: " +
                        $"{ListValueColor.VTSequenceForeground}" +
                        $"{valuesString}"
                    );
                }
                else
                    listBuilder.AppendLine(
                        $"{ListKeyColor.VTSequenceForeground}" +
                        $"- {EntryNumber}: " +
                        $"{ListValueColor.VTSequenceForeground}" +
                        $"{ListEntry}"
                    );
                EntryNumber += 1;
            }
            listBuilder.Append(KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground);
            return listBuilder.ToString();
        }
        #endregion
    }
}

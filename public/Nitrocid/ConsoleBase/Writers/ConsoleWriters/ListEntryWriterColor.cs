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
using System.Text;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Kernel.Debugging;
using KS.Languages;
using Terminaux.Colors;

namespace KS.ConsoleBase.Writers.ConsoleWriters
{
    /// <summary>
    /// List entry writer with color support
    /// </summary>
    public static class ListEntryWriterColor
    {
        /// <summary>
        /// Outputs a list entry and value into the terminal prompt plainly.
        /// </summary>
        /// <param name="entry">A list entry that will be listed to the terminal prompt.</param>
        /// <param name="value">A list value that will be listed to the terminal prompt.</param>
        /// <param name="indent">Indentation level</param>
        public static void WriteListEntryPlain(string entry, string value, int indent = 0)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Write the list entry
                    string buffered = RenderListEntry(entry, value, indent);
                    TextWriterColor.WritePlain(buffered);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs a list entry and value into the terminal prompt.
        /// </summary>
        /// <param name="entry">A list entry that will be listed to the terminal prompt.</param>
        /// <param name="value">A list value that will be listed to the terminal prompt.</param>
        /// <param name="indent">Indentation level</param>
        public static void WriteListEntry(string entry, string value, int indent = 0) =>
            WriteListEntry(entry, value, KernelColorTools.GetColor(KernelColorType.ListEntry), KernelColorTools.GetColor(KernelColorType.ListValue), indent);

        /// <summary>
        /// Outputs a list entry and value into the terminal prompt.
        /// </summary>
        /// <param name="entry">A list entry that will be listed to the terminal prompt.</param>
        /// <param name="value">A list value that will be listed to the terminal prompt.</param>
        /// <param name="indent">Indentation level</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteListEntry(string entry, string value, KernelColorType ListKeyColor, KernelColorType ListValueColor, int indent = 0) =>
            WriteListEntry(entry, value, KernelColorTools.GetColor(ListKeyColor), KernelColorTools.GetColor(ListValueColor), indent);

        /// <summary>
        /// Outputs a list entry and value into the terminal prompt.
        /// </summary>
        /// <param name="entry">A list entry that will be listed to the terminal prompt.</param>
        /// <param name="value">A list value that will be listed to the terminal prompt.</param>
        /// <param name="indent">Indentation level</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteListEntry(string entry, string value, ConsoleColors ListKeyColor, ConsoleColors ListValueColor, int indent = 0) =>
            WriteListEntry(entry, value, new Color(ListKeyColor), new Color(ListValueColor), indent);

        /// <summary>
        /// Outputs a list entry and value into the terminal prompt.
        /// </summary>
        /// <param name="entry">A list entry that will be listed to the terminal prompt.</param>
        /// <param name="value">A list value that will be listed to the terminal prompt.</param>
        /// <param name="indent">Indentation level</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteListEntry(string entry, string value, Color ListKeyColor, Color ListValueColor, int indent = 0)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Write the list entry
                    string buffered = RenderListEntry(entry, value, ListKeyColor, ListValueColor, indent);
                    TextWriterColor.WritePlain(buffered);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Renders a list entry and value.
        /// </summary>
        /// <param name="entry">A list entry that will be listed.</param>
        /// <param name="value">A list value that will be listed.</param>
        /// <param name="indent">Indentation level</param>
        /// <returns>A list entry without the new line at the end</returns>
        public static string RenderListEntry(string entry, string value, int indent = 0)
        {
            // First, get the spaces count to indent
            if (indent < 0)
                indent = 0;
            string spaces = new(' ', indent * 2);

            // Then, write the list entry
            var listBuilder = new StringBuilder();
            listBuilder.Append($"{spaces}- {entry}: {value}");
            return listBuilder.ToString();
        }

        /// <summary>
        /// Renders a list entry and value.
        /// </summary>
        /// <param name="entry">A list entry that will be listed.</param>
        /// <param name="value">A list value that will be listed.</param>
        /// <param name="indent">Indentation level</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        /// <returns>A list entry without the new line at the end</returns>
        public static string RenderListEntry(string entry, string value, Color ListKeyColor, Color ListValueColor, int indent = 0)
        {
            // First, get the spaces count to indent
            if (indent < 0)
                indent = 0;
            string spaces = new(' ', indent * 2);

            // Then, write the list entry
            var listBuilder = new StringBuilder();
            listBuilder.Append(
                $"{ListKeyColor.VTSequenceForeground}{spaces}- {entry}: " +
                $"{ListValueColor.VTSequenceForeground}{value}"
            );
            return listBuilder.ToString();
        }
    }
}

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

using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Debugging;
using System;
using System.Threading;
using Nitrocid.Drivers.Console;
using Nitrocid.Drivers;

namespace Nitrocid.ConsoleBase.Writers
{
    /// <summary>
    /// Text writer wrapper for writing with <see cref="KernelColorType"/> (<see cref="Terminaux.Writer.ConsoleWriters"/>)
    /// </summary>
    public static class TextWriters
    {
        /// <summary>
        /// Outputs a list entry and value into the terminal prompt.
        /// </summary>
        /// <param name="entry">A list entry that will be listed to the terminal prompt.</param>
        /// <param name="value">A list value that will be listed to the terminal prompt.</param>
        /// <param name="indent">Indentation level</param>
        /// <param name="ListKeyColor">A key color.</param>
        /// <param name="ListValueColor">A value color.</param>
        public static void WriteListEntry(string entry, string value, KernelColorType ListKeyColor, KernelColorType ListValueColor, int indent = 0) =>
            ListEntryWriterColor.WriteListEntry(entry, value, KernelColorTools.GetColor(ListKeyColor), KernelColorTools.GetColor(ListValueColor), indent);

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, KernelColorType colorType, params object[] vars) =>
            Write(Text, true, false, colorType, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, KernelColorType colorType, params object[] vars) =>
            Write(Text, Line, false, colorType, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, KernelColorType colorType, params object[] vars)
        {
            lock (BaseConsoleDriver.WriteLock)
            {
                try
                {
                    // Get the colors
                    var foreground = KernelColorTools.GetColor(colorType);
                    var background = KernelColorTools.GetColor(KernelColorType.Background);

                    // Write the text to console
                    if (Highlight)
                        TextWriterHighlightedColor.WriteColorBack(Text, Line, foreground, background, vars);
                    else
                        TextWriterColor.WriteColorBack(Text, Line, foreground, background, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, "There is a serious error when printing text. {0}", vars: [ex.Message]);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] vars) =>
            Write(Text, true, false, colorTypeForeground, colorTypeBackground, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] vars) =>
            Write(Text, Line, false, colorTypeForeground, colorTypeBackground, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] vars)
        {
            lock (BaseConsoleDriver.WriteLock)
            {
                try
                {
                    // Get the colors
                    var foreground = KernelColorTools.GetColor(colorTypeForeground);
                    var background = KernelColorTools.GetColor(colorTypeBackground);

                    // Write the text to console
                    if (Highlight)
                        TextWriterHighlightedColor.WriteColorBack(Text, Line, foreground, background, vars);
                    else
                        TextWriterColor.WriteColorBack(Text, Line, foreground, background, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, "There is a serious error when printing text. {0}", vars: [ex.Message]);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, KernelColorType colorType, params object[] vars) =>
            WriteWhere(msg, Left, Top, false, 0, colorType, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, bool Return, KernelColorType colorType, params object[] vars) =>
            WriteWhere(msg, Left, Top, Return, 0, colorType, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, bool Return, int RightMargin, KernelColorType colorType, params object[] vars)
        {
            lock (BaseConsoleDriver.WriteLock)
            {
                try
                {
                    // Get the colors
                    var foreground = KernelColorTools.GetColor(colorType);
                    var background = KernelColorTools.GetColor(KernelColorType.Background);

                    // Write the text to console
                    TextWriterWhereColor.WriteWhereColorBack(msg, Left, Top, Return, RightMargin, foreground, background, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, "There is a serious error when printing text. {0}", vars: [ex.Message]);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] vars) =>
            WriteWhere(msg, Left, Top, false, 0, colorTypeForeground, colorTypeBackground, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, bool Return, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] vars) =>
            WriteWhere(msg, Left, Top, Return, 0, colorTypeForeground, colorTypeBackground, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="RightMargin">The right margin</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, bool Return, int RightMargin, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] vars)
        {
            lock (BaseConsoleDriver.WriteLock)
            {
                try
                {
                    // Get the colors
                    var foreground = KernelColorTools.GetColor(colorTypeForeground);
                    var background = KernelColorTools.GetColor(colorTypeBackground);

                    // Write the text to console
                    TextWriterWhereColor.WriteWhereColorBack(msg, Left, Top, Return, RightMargin, foreground, background, vars);
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, "There is a serious error when printing text. {0}", vars: [ex.Message]);
                }
            }
        }
    }
}

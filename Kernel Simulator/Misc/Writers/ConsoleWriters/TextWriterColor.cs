
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using ColorSeq;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Writers.WriterBase;

namespace KS.Misc.Writers.ConsoleWriters
{
    public static class TextWriterColor
    {

        internal static object WriteLock = new();

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, ColorTools.ColTypes colorType, params object[] vars)
        {
            Write(Text, Line, false, colorType, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, ColorTools.ColTypes colorType, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    ColorTools.SetConsoleColor(colorType, Highlight);

                    // Write the text to console
                    if (Highlight)
                    {
                        WriterPlainManager.currentPlain.WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(colorType);
                        ColorTools.SetConsoleColor(ColorTools.BackgroundColor, true);
                        WriterPlainManager.currentPlain.WritePlain("", Line);
                    }
                    else
                    {
                        WriterPlainManager.currentPlain.WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, ColorTools.ColTypes colorTypeForeground, ColorTools.ColTypes colorTypeBackground, params object[] vars)
        {
            Write(Text, Line, false, colorTypeForeground, colorTypeBackground, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, ColorTools.ColTypes colorTypeForeground, ColorTools.ColTypes colorTypeBackground, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    ColorTools.SetConsoleColor(colorTypeForeground, Highlight);
                    ColorTools.SetConsoleColor(colorTypeBackground, !Highlight);

                    // Write the text to console
                    if (Highlight)
                    {
                        WriterPlainManager.currentPlain.WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(colorTypeForeground);
                        ColorTools.SetConsoleColor(colorTypeBackground, true);
                        WriterPlainManager.currentPlain.WritePlain("", Line);
                    }
                    else
                    {
                        WriterPlainManager.currentPlain.WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, ConsoleColor color, params object[] vars)
        {
            Write(Text, Line, false, color, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, ConsoleColor color, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    if (Highlight)
                    {
                        ConsoleWrapper.ForegroundColor = (ConsoleColor)Convert.ToInt32(StringQuery.IsStringNumeric(ColorTools.BackgroundColor.PlainSequence) && Convert.ToDouble(ColorTools.BackgroundColor.PlainSequence) <= 15d ? Enum.Parse(typeof(ConsoleColor), ColorTools.BackgroundColor.PlainSequence) : ConsoleColor.Black);
                        ConsoleWrapper.BackgroundColor = color;
                    }
                    else
                    {
                        ConsoleWrapper.BackgroundColor = (ConsoleColor)Convert.ToInt32(StringQuery.IsStringNumeric(ColorTools.BackgroundColor.PlainSequence) && Convert.ToDouble(ColorTools.BackgroundColor.PlainSequence) <= 15d ? Enum.Parse(typeof(ConsoleColor), ColorTools.BackgroundColor.PlainSequence) : ConsoleColor.Black);
                        ConsoleWrapper.ForegroundColor = color;
                    }

                    // Write the text to console
                    if (Highlight)
                    {
                        WriterPlainManager.currentPlain.WritePlain(Text, false, vars);
                        ConsoleWrapper.BackgroundColor = (ConsoleColor)Convert.ToInt32(StringQuery.IsStringNumeric(ColorTools.BackgroundColor.PlainSequence) && Convert.ToDouble(ColorTools.BackgroundColor.PlainSequence) <= 15d ? Enum.Parse(typeof(ConsoleColor), ColorTools.BackgroundColor.PlainSequence) : ConsoleColor.Black);
                        ConsoleWrapper.ForegroundColor = color;
                        WriterPlainManager.currentPlain.WritePlain("", Line);
                    }
                    else
                    {
                        WriterPlainManager.currentPlain.WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor, params object[] vars)
        {
            Write(Text, Line, false, ForegroundColor, BackgroundColor, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    if (Highlight)
                    {
                        ConsoleWrapper.BackgroundColor = ForegroundColor;
                        ConsoleWrapper.ForegroundColor = BackgroundColor;
                    }
                    else
                    {
                        ConsoleWrapper.BackgroundColor = BackgroundColor;
                        ConsoleWrapper.ForegroundColor = ForegroundColor;
                    }

                    // Write the text to console
                    if (Highlight)
                    {
                        WriterPlainManager.currentPlain.WritePlain(Text, false, vars);
                        ConsoleWrapper.BackgroundColor = BackgroundColor;
                        ConsoleWrapper.ForegroundColor = ForegroundColor;
                        WriterPlainManager.currentPlain.WritePlain("", Line);
                    }
                    else
                    {
                        WriterPlainManager.currentPlain.WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, Color color, params object[] vars)
        {
            Write(Text, Line, false, color, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, Color color, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    if (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, ConsoleWrapper.Out))
                    {
                        ColorTools.SetConsoleColor(color, Highlight, Highlight);
                        ColorTools.SetConsoleColor(ColorTools.BackgroundColor, !Highlight, !Highlight);
                    }

                    // Write the text to console
                    if (Highlight)
                    {
                        WriterPlainManager.currentPlain.WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(color);
                        ColorTools.SetConsoleColor(ColorTools.BackgroundColor, true);
                        WriterPlainManager.currentPlain.WritePlain("", Line);
                    }
                    else
                    {
                        WriterPlainManager.currentPlain.WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            Write(Text, Line, false, ForegroundColor, BackgroundColor, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    if (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, ConsoleWrapper.Out))
                    {
                        ColorTools.SetConsoleColor(ForegroundColor, Highlight, Highlight);
                        ColorTools.SetConsoleColor(BackgroundColor, !Highlight, !Highlight);
                    }

                    // Write the text to console
                    if (Highlight)
                    {
                        WriterPlainManager.currentPlain.WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(ForegroundColor);
                        ColorTools.SetConsoleColor(BackgroundColor, true);
                        WriterPlainManager.currentPlain.WritePlain("", Line);
                    }
                    else
                    {
                        WriterPlainManager.currentPlain.WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

    }
}

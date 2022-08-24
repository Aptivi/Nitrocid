
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
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Writers.ConsoleWriters
{
    public static class TextWriterWhereColor
    {

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWherePlain(string msg, int Left, int Top, params object[] vars)
        {
            WriteWherePlain(msg, Left, Top, false, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWherePlain(string msg, int Left, int Top, bool Return, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Format the message as necessary
                    if (!(vars.Length == 0))
                        msg = StringManipulate.FormatString(msg, vars);

                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    int OldLeft = Console.CursorLeft;
                    int OldTop = Console.CursorTop;
                    var Paragraphs = msg.SplitNewLines();
                    Console.SetCursorPosition(Left, Top);
                    for (int MessageParagraphIndex = 0, loopTo = Paragraphs.Length - 1; MessageParagraphIndex <= loopTo; MessageParagraphIndex++)
                    {
                        // We can now check to see if we're writing a letter past the console window width
                        string MessageParagraph = Paragraphs[MessageParagraphIndex];
                        foreach (char ParagraphChar in MessageParagraph)
                        {
                            if (Console.CursorLeft == Console.WindowWidth)
                            {
                                if (Console.CursorTop == Console.BufferHeight - 1)
                                {
                                    // We've reached the end of buffer. Write the line to scroll.
                                    Console.WriteLine();
                                }
                                else
                                {
                                    Console.CursorTop += 1;
                                }
                                Console.CursorLeft = Left;
                            }
                            Console.Write(ParagraphChar);
                        }

                        // We're starting with the new paragraph, so we increase the CursorTop value by 1.
                        if (!(MessageParagraphIndex == Paragraphs.Length - 1))
                        {
                            if (Console.CursorTop == Console.BufferHeight - 1)
                            {
                                // We've reached the end of buffer. Write the line to scroll.
                                Console.WriteLine();
                            }
                            else
                            {
                                Console.CursorTop += 1;
                            }
                            Console.CursorLeft = Left;
                        }
                    }
                    if (Return)
                        Console.SetCursorPosition(OldLeft, OldTop);
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
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
        public static void WriteWhere(string msg, int Left, int Top, ColorTools.ColTypes colorType, params object[] vars)
        {
            WriteWhere(msg, Left, Top, false, colorType, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, bool Return, ColorTools.ColTypes colorType, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    ColorTools.SetConsoleColor(colorType);

                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    WriteWherePlain(msg, Left, Top, Return, vars);
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
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
        public static void WriteWhere(string msg, int Left, int Top, ColorTools.ColTypes colorTypeForeground, ColorTools.ColTypes colorTypeBackground, params object[] vars)
        {
            WriteWhere(msg, Left, Top, false, colorTypeForeground, colorTypeBackground, vars);
        }

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
        public static void WriteWhere(string msg, int Left, int Top, bool Return, ColorTools.ColTypes colorTypeForeground, ColorTools.ColTypes colorTypeBackground, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    ColorTools.SetConsoleColor(colorTypeForeground);
                    ColorTools.SetConsoleColor(colorTypeBackground, true);

                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    WriteWherePlain(msg, Left, Top, Return, vars);
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, ConsoleColor color, params object[] vars)
        {
            WriteWhere(msg, Left, Top, false, color, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, bool Return, ConsoleColor color, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    Console.BackgroundColor = (ConsoleColor)Convert.ToInt32(StringQuery.IsStringNumeric(ColorTools.BackgroundColor.PlainSequence) && Convert.ToDouble(ColorTools.BackgroundColor.PlainSequence) <= 15d ? Enum.Parse(typeof(ConsoleColor), ColorTools.BackgroundColor.PlainSequence) : ConsoleColor.Black);
                    Console.ForegroundColor = color;

                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    WriteWherePlain(msg, Left, Top, Return, vars);
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor, params object[] vars)
        {
            WriteWhere(msg, Left, Top, false, ForegroundColor, BackgroundColor, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, bool Return, ConsoleColor ForegroundColor, ConsoleColor BackgroundColor, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    Console.BackgroundColor = BackgroundColor;
                    Console.ForegroundColor = ForegroundColor;

                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    WriteWherePlain(msg, Left, Top, Return, vars);
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, Color color, params object[] vars)
        {
            WriteWhere(msg, Left, Top, false, color, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, bool Return, Color color, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    if (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out))
                    {
                        ColorTools.SetConsoleColor(color);
                        ColorTools.SetConsoleColor(ColorTools.BackgroundColor, true);
                    }

                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    WriteWherePlain(msg, Left, Top, Return, vars);
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            WriteWhere(msg, Left, Top, false, ForegroundColor, BackgroundColor, vars);
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with location support, and sets colors as needed.
        /// </summary>
        /// <param name="msg">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteWhere(string msg, int Left, int Top, bool Return, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    if (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, Console.Out))
                    {
                        ColorTools.SetConsoleColor(ForegroundColor);
                        ColorTools.SetConsoleColor(BackgroundColor, true);
                    }

                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    WriteWherePlain(msg, Left, Top, Return, vars);
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

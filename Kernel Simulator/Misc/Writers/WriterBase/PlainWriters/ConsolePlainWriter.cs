
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

using Extensification.StringExts;
using KS.ConsoleBase;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using System;
using System.Linq;
using System.Threading;

namespace KS.Misc.Writers.WriterBase.PlainWriters
{
    internal class ConsolePlainWriter : IWriterPlain
    {
        /// <inheritdoc/>
        public void WritePlain(string Text, bool Line, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Get the filtered positions first.
                    int FilteredLeft = default, FilteredTop = default;
                    if (!Line & (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, ConsoleWrapper.Out)))
                        ConsoleExtensions.GetFilteredPositions(Text, ref FilteredLeft, ref FilteredTop, vars);

                    // Actually write
                    if (Line)
                    {
                        if (!(vars.Length == 0))
                        {
                            ConsoleWrapper.WriteLine(Text, vars);
                        }
                        else
                        {
                            ConsoleWrapper.WriteLine(Text);
                        }
                    }
                    else if (!(vars.Length == 0))
                    {
                        ConsoleWrapper.Write(Text, vars);
                    }
                    else
                    {
                        ConsoleWrapper.Write(Text);
                    }

                    // Return to the processed position
                    if (!Line & (Kernel.Kernel.DefConsoleOut is null | Equals(Kernel.Kernel.DefConsoleOut, ConsoleWrapper.Out)))
                        ConsoleWrapper.SetCursorPosition(FilteredLeft, FilteredTop);
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <inheritdoc/>
        public void WriteSlowlyPlain(string msg, bool Line, double MsEachLetter, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Format string as needed
                    if (!(vars.Length == 0))
                        msg = StringManipulate.FormatString(msg, vars);

                    // Write text slowly
                    var chars = msg.ToCharArray().ToList();
                    foreach (char ch in chars)
                    {
                        Thread.Sleep((int)Math.Round(MsEachLetter));
                        ConsoleWrapper.Write(ch);
                    }
                    if (Line)
                    {
                        ConsoleWrapper.WriteLine();
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <inheritdoc/>
        public void WriteWherePlain(string msg, int Left, int Top, params object[] vars) => WriteWherePlain(msg, Left, Top, false, vars);

        /// <inheritdoc/>
        public void WriteWherePlain(string msg, int Left, int Top, bool Return, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Format the message as necessary
                    if (!(vars.Length == 0))
                        msg = StringManipulate.FormatString(msg, vars);

                    // Write text in another place. By the way, we check the text for newlines and console width excess
                    int OldLeft = ConsoleWrapper.CursorLeft;
                    int OldTop = ConsoleWrapper.CursorTop;
                    var Paragraphs = msg.SplitNewLines();
                    ConsoleWrapper.SetCursorPosition(Left, Top);
                    for (int MessageParagraphIndex = 0, loopTo = Paragraphs.Length - 1; MessageParagraphIndex <= loopTo; MessageParagraphIndex++)
                    {
                        // We can now check to see if we're writing a letter past the console window width
                        string MessageParagraph = Paragraphs[MessageParagraphIndex];
                        foreach (char ParagraphChar in MessageParagraph)
                        {
                            if (ConsoleWrapper.CursorLeft == ConsoleWrapper.WindowWidth)
                            {
                                if (ConsoleWrapper.CursorTop == ConsoleWrapper.BufferHeight - 1)
                                {
                                    // We've reached the end of buffer. Write the line to scroll.
                                    ConsoleWrapper.WriteLine();
                                }
                                else
                                {
                                    ConsoleWrapper.CursorTop += 1;
                                }
                                ConsoleWrapper.CursorLeft = Left;
                            }
                            ConsoleWrapper.Write(ParagraphChar);
                        }

                        // We're starting with the new paragraph, so we increase the CursorTop value by 1.
                        if (!(MessageParagraphIndex == Paragraphs.Length - 1))
                        {
                            if (ConsoleWrapper.CursorTop == ConsoleWrapper.BufferHeight - 1)
                            {
                                // We've reached the end of buffer. Write the line to scroll.
                                ConsoleWrapper.WriteLine();
                            }
                            else
                            {
                                ConsoleWrapper.CursorTop += 1;
                            }
                            ConsoleWrapper.CursorLeft = Left;
                        }
                    }
                    if (Return)
                        ConsoleWrapper.SetCursorPosition(OldLeft, OldTop);
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <inheritdoc/>
        public void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, params object[] vars) => WriteWhereSlowlyPlain(msg, Line, Left, Top, MsEachLetter, false, vars);

        /// <inheritdoc/>
        public void WriteWhereSlowlyPlain(string msg, bool Line, int Left, int Top, double MsEachLetter, bool Return, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                try
                {
                    // Format string as needed
                    if (!(vars.Length == 0))
                        msg = StringManipulate.FormatString(msg, vars);

                    // Write text in another place slowly
                    int OldLeft = ConsoleWrapper.CursorLeft;
                    int OldTop = ConsoleWrapper.CursorTop;
                    var Paragraphs = msg.SplitNewLines();
                    ConsoleWrapper.SetCursorPosition(Left, Top);
                    for (int MessageParagraphIndex = 0, loopTo = Paragraphs.Length - 1; MessageParagraphIndex <= loopTo; MessageParagraphIndex++)
                    {
                        // We can now check to see if we're writing a letter past the console window width
                        string MessageParagraph = Paragraphs[MessageParagraphIndex];
                        foreach (char ParagraphChar in MessageParagraph)
                        {
                            Thread.Sleep((int)Math.Round(MsEachLetter));
                            if (ConsoleWrapper.CursorLeft == ConsoleWrapper.WindowWidth)
                            {
                                ConsoleWrapper.CursorTop += 1;
                                ConsoleWrapper.CursorLeft = Left;
                            }
                            ConsoleWrapper.Write(ParagraphChar);
                            if (Line)
                                ConsoleWrapper.WriteLine();
                        }

                        // We're starting with the new paragraph, so we increase the CursorTop value by 1.
                        if (!(MessageParagraphIndex == Paragraphs.Length - 1))
                        {
                            ConsoleWrapper.CursorTop += 1;
                            ConsoleWrapper.CursorLeft = Left;
                        }
                    }
                    if (Return)
                        ConsoleWrapper.SetCursorPosition(OldLeft, OldTop);
                }
                catch (Exception ex) when (!(ex.GetType().Name == "ThreadInterruptedException"))
                {
                    DebugWriter.WStkTrc(ex);
                    KernelTools.KernelError(KernelErrorLevel.C, false, 0L, Translate.DoTranslation("There is a serious error when printing text."), ex);
                }
            }
        }

        /// <inheritdoc/>
        public void WriteWrappedPlain(string Text, bool Line, params object[] vars)
        {
            lock (TextWriterColor.WriteLock)
            {
                var LinesMade = default(int);
                int OldTop;
                try
                {
                    // Format string as needed
                    if (!(vars.Length == 0))
                        Text = StringManipulate.FormatString(Text, vars);

                    OldTop = ConsoleWrapper.CursorTop;
                    foreach (char TextChar in Text.ToString().ToCharArray())
                    {
                        ConsoleWrapper.Write(TextChar);
                        LinesMade += ConsoleWrapper.CursorTop - OldTop;
                        OldTop = ConsoleWrapper.CursorTop;
                        if (LinesMade == ConsoleWrapper.WindowHeight - 1)
                        {
                            if (ConsoleWrapper.ReadKey(true).Key == ConsoleKey.Escape)
                                break;
                            LinesMade = 0;
                        }
                    }
                    if (Line)
                        ConsoleWrapper.WriteLine();
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

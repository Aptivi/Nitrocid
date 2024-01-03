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
using System.Threading;
using Terminaux.Colors;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.ConsoleBase.Writers.ConsoleWriters
{
    /// <summary>
    /// Console text writer with color support
    /// </summary>
    public static class TextWriterColor
    {

        internal static object WriteLock = new();

        /// <summary>
        /// Outputs the new line into the terminal prompt, and sets colors as needed.
        /// </summary>
        public static void Write()
        {
            lock (WriteLock)
            {
                ConsoleWrapper.WriteLine();
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt plainly.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WritePlain(string Text, params object[] vars) =>
            WritePlain(Text, true, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt plainly.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WritePlain(string Text, bool Line, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Actually write
                    if (Line)
                    {
                        if (vars.Length > 0)
                        {
                            ConsoleWrapper.WriteLine(Text, vars);
                        }
                        else
                        {
                            ConsoleWrapper.WriteLine(Text);
                        }
                    }
                    else if (vars.Length > 0)
                    {
                        ConsoleWrapper.Write(Text, vars);
                    }
                    else
                    {
                        ConsoleWrapper.Write(Text);
                    }
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, params object[] vars) =>
            Write(Text, true, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, params object[] vars) =>
            Write(Text, Line, false, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, params object[] vars) =>
            WriteColor(Text, Line, Highlight, KernelColorTools.GetColor(KernelColorType.NeutralText), vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColor(string Text, ConsoleColors color, params object[] vars) =>
            WriteColor(Text, true, false, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColor(string Text, bool Line, ConsoleColors color, params object[] vars) =>
            WriteColor(Text, Line, false, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColor(string Text, bool Line, bool Highlight, ConsoleColors color, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    KernelColorTools.SetConsoleColor(new Color(color), Highlight);
                    KernelColorTools.SetConsoleColor(KernelColorType.Background, !Highlight, false);

                    // Write the text to console
                    if (Highlight)
                    {
                        WritePlain(Text, false, vars);
                        KernelColorTools.SetConsoleColor(new Color(color));
                        KernelColorTools.SetConsoleColor(KernelColorType.Background, true);
                        WritePlain("", Line);
                    }
                    else
                    {
                        WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColorBack(string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteColorBack(Text, true, false, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColorBack(string Text, bool Line, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteColorBack(Text, Line, false, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColorBack(string Text, bool Line, bool Highlight, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    KernelColorTools.SetConsoleColor(new Color(ForegroundColor), Highlight);
                    KernelColorTools.SetConsoleColor(new Color(BackgroundColor), !Highlight);

                    // Write the text to console
                    if (Highlight)
                    {
                        WritePlain(Text, false, vars);
                        KernelColorTools.SetConsoleColor(new Color(ForegroundColor));
                        KernelColorTools.SetConsoleColor(new Color(BackgroundColor), true);
                        WritePlain("", Line);
                    }
                    else
                    {
                        WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColor(string Text, Color color, params object[] vars) =>
            WriteColor(Text, true, false, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColor(string Text, bool Line, Color color, params object[] vars) =>
            WriteColor(Text, Line, false, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColor(string Text, bool Line, bool Highlight, Color color, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    KernelColorTools.SetConsoleColor(color, Highlight);
                    KernelColorTools.SetConsoleColor(KernelColorType.Background, !Highlight, false);

                    // Write the text to console
                    if (Highlight)
                    {
                        WritePlain(Text, false, vars);
                        KernelColorTools.SetConsoleColor(color);
                        KernelColorTools.SetConsoleColor(KernelColorType.Background, true);
                        WritePlain("", Line);
                    }
                    else
                    {
                        WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColorBack(string Text, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            WriteColorBack(Text, true, false, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColorBack(string Text, bool Line, Color ForegroundColor, Color BackgroundColor, params object[] vars) =>
            WriteColorBack(Text, Line, false, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void WriteColorBack(string Text, bool Line, bool Highlight, Color ForegroundColor, Color BackgroundColor, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    KernelColorTools.SetConsoleColor(ForegroundColor, Highlight);
                    KernelColorTools.SetConsoleColor(BackgroundColor, !Highlight);

                    // Write the text to console
                    if (Highlight)
                    {
                        WritePlain(Text, false, vars);
                        KernelColorTools.SetConsoleColor(ForegroundColor);
                        KernelColorTools.SetConsoleColor(BackgroundColor, true);
                        WritePlain("", Line);
                    }
                    else
                    {
                        WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

    }
}

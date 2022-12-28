
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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
using System.Threading;
using ColorSeq;
using KS.Kernel.Debugging;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Drivers;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Writers.ConsoleWriters
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
                DriverHandler.CurrentConsoleDriver.WritePlain();
            }
        }

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, params object[] vars) => Write(Text, true, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, params object[] vars) => Write(Text, Line, false, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, params object[] vars) => Write(Text, Line, Highlight, KernelColorType.NeutralText, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt, and sets colors as needed.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="colorType">A type of colors that will be changed.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, KernelColorType colorType, params object[] vars) => Write(Text, Line, false, colorType, vars);

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
            lock (WriteLock)
            {
                try
                {
                    // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                    ColorTools.SetConsoleColor(colorType, Highlight);

                    // Write the text to console
                    if (Highlight)
                    {
                        DriverHandler.CurrentConsoleDriver.WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(colorType);
                        ColorTools.SetConsoleColor(KernelColorType.Background, true);
                        DriverHandler.CurrentConsoleDriver.WritePlain("", Line);
                    }
                    else
                    {
                        DriverHandler.CurrentConsoleDriver.WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
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
        public static void Write(string Text, bool Line, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] vars) => Write(Text, Line, false, colorTypeForeground, colorTypeBackground, vars);

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
                        DriverHandler.CurrentConsoleDriver.WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(colorTypeForeground);
                        ColorTools.SetConsoleColor(colorTypeBackground, true);
                        DriverHandler.CurrentConsoleDriver.WritePlain("", Line);
                    }
                    else
                    {
                        DriverHandler.CurrentConsoleDriver.WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
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
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, ConsoleColors color, params object[] vars) => Write(Text, Line, false, color, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, ConsoleColors color, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    ColorTools.SetConsoleColor(new Color(Convert.ToInt32(color)), Highlight);

                    // Write the text to console
                    if (Highlight)
                    {
                        DriverHandler.CurrentConsoleDriver.WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(new Color(Convert.ToInt32(color)));
                        ColorTools.SetConsoleColor(KernelColorType.Background, true);
                        DriverHandler.CurrentConsoleDriver.WritePlain("", Line);
                    }
                    else
                    {
                        DriverHandler.CurrentConsoleDriver.WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
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
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] vars) => Write(Text, Line, false, ForegroundColor, BackgroundColor, vars);

        /// <summary>
        /// Outputs the text into the terminal prompt with custom color support.
        /// </summary>
        /// <param name="Text">A sentence that will be written to the terminal prompt. Supports {0}, {1}, ...</param>
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="Highlight">Highlight the text written</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, bool Highlight, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] vars)
        {
            lock (WriteLock)
            {
                try
                {
                    // Try to write to console
                    ColorTools.SetConsoleColor(new Color(Convert.ToInt32(ForegroundColor)), Highlight);
                    ColorTools.SetConsoleColor(new Color(Convert.ToInt32(BackgroundColor)), !Highlight);

                    // Write the text to console
                    if (Highlight)
                    {
                        DriverHandler.CurrentConsoleDriver.WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(new Color(Convert.ToInt32(ForegroundColor)));
                        ColorTools.SetConsoleColor(new Color(Convert.ToInt32(BackgroundColor)), true);
                        DriverHandler.CurrentConsoleDriver.WritePlain("", Line);
                    }
                    else
                    {
                        DriverHandler.CurrentConsoleDriver.WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
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
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="color">A color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, Color color, params object[] vars) => Write(Text, Line, false, color, vars);

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
                    ColorTools.SetConsoleColor(color, Highlight, Highlight);
                    ColorTools.SetConsoleColor(KernelColorType.Background, !Highlight, !Highlight);

                    // Write the text to console
                    if (Highlight)
                    {
                        DriverHandler.CurrentConsoleDriver.WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(color);
                        ColorTools.SetConsoleColor(KernelColorType.Background, true);
                        DriverHandler.CurrentConsoleDriver.WritePlain("", Line);
                    }
                    else
                    {
                        DriverHandler.CurrentConsoleDriver.WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
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
        /// <param name="Line">Whether to print a new line or not</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        public static void Write(string Text, bool Line, Color ForegroundColor, Color BackgroundColor, params object[] vars) => Write(Text, Line, false, ForegroundColor, BackgroundColor, vars);

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
                    ColorTools.SetConsoleColor(ForegroundColor, Highlight, Highlight);
                    ColorTools.SetConsoleColor(BackgroundColor, !Highlight, !Highlight);

                    // Write the text to console
                    if (Highlight)
                    {
                        DriverHandler.CurrentConsoleDriver.WritePlain(Text, false, vars);
                        ColorTools.SetConsoleColor(ForegroundColor);
                        ColorTools.SetConsoleColor(BackgroundColor, true);
                        DriverHandler.CurrentConsoleDriver.WritePlain("", Line);
                    }
                    else
                    {
                        DriverHandler.CurrentConsoleDriver.WritePlain(Text, Line, vars);
                    }
                }
                catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
                }
            }
        }

    }
}

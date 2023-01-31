
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Threading;
using ColorSeq;
using Figgle;
using KS.Kernel.Debugging;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.FancyWriters.Tools;
using KS.Misc.Writers.ConsoleWriters;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Writers.FancyWriters
{
    /// <summary>
    /// Figlet writer (positional)
    /// </summary>
    public static class FigletWhereColor
    {

        /// <summary>
        /// Writes the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletWherePlain(string Text, int Left, int Top, bool Return, FiggleFont FigletFont, params object[] Vars)
        {
            try
            {
                Text = FigletTools.RenderFiglet(Text, FigletFont, Vars);
                TextWriterWhereColor.WriteWhere(Text, Left, Top, Return, Vars);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletWhere(string Text, int Left, int Top, bool Return, FiggleFont FigletFont, KernelColorType ColTypes, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                ColorTools.SetConsoleColor(ColTypes);

                // Actually write
                WriteFigletWherePlain(Text, Left, Top, Return, FigletFont, Vars);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletWhere(string Text, int Left, int Top, bool Return, FiggleFont FigletFont, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                ColorTools.SetConsoleColor(colorTypeForeground);
                ColorTools.SetConsoleColor(colorTypeBackground, true);

                // Actually write
                WriteFigletWherePlain(Text, Left, Top, Return, FigletFont, Vars);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletWhere(string Text, int Left, int Top, bool Return, FiggleFont FigletFont, ConsoleColors Color, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                ColorTools.SetConsoleColor(new Color((int)Color));

                // Actually write
                WriteFigletWherePlain(Text, Left, Top, Return, FigletFont, Vars);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletWhere(string Text, int Left, int Top, bool Return, FiggleFont FigletFont, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                ColorTools.SetConsoleColor(new Color((int)ForegroundColor));
                ColorTools.SetConsoleColor(new Color((int)BackgroundColor), true);

                // Actually write
                WriteFigletWherePlain(Text, Left, Top, Return, FigletFont, Vars);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletWhere(string Text, int Left, int Top, bool Return, FiggleFont FigletFont, Color Color, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                ColorTools.SetConsoleColor(Color);

                // Actually write
                WriteFigletWherePlain(Text, Left, Top, Return, FigletFont, Vars);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the figlet text with position support
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="Left">Column number in console</param>
        /// <param name="Top">Row number in console</param>
        /// <param name="Return">Whether or not to return to old position</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletWhere(string Text, int Left, int Top, bool Return, FiggleFont FigletFont, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                ColorTools.SetConsoleColor(ForegroundColor);
                ColorTools.SetConsoleColor(BackgroundColor, true);

                // Actually write
                WriteFigletWherePlain(Text, Left, Top, Return, FigletFont, Vars);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

    }
}

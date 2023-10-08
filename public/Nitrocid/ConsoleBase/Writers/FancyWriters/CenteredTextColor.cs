
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using System;
using System.Threading;
using Terminaux.Colors;

namespace KS.ConsoleBase.Writers.FancyWriters
{
    /// <summary>
    /// Centered writer
    /// </summary>
    public static class CenteredTextColor
    {

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(int top, string Text, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = TextTools.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth);
                ConsoleWrapper.CursorTop = top;
                for (int i = 0; i < sentences.Length; i++)
                {
                    string sentence = sentences[i];
                    int consoleInfoX = ConsoleWrapper.WindowWidth / 2 - sentence.Length / 2;
                    consoleInfoX = consoleInfoX < 0 ? 0 : consoleInfoX;
                    TextWriterWhereColor.WriteWhere(sentence + "\n", consoleInfoX, ConsoleWrapper.CursorTop, Vars);
                }
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredKernelColor(int top, string Text, KernelColorType ColTypes, params object[] Vars) =>
            WriteCenteredColorBack(top, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredKernelColor(int top, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            WriteCenteredColorBack(top, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredColor(int top, string Text, ConsoleColors Color, params object[] Vars) =>
            WriteCenteredColorBack(top, Text, new Color(Color), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredColorBack(int top, string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteCenteredColorBack(top, Text, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredColor(int top, string Text, Color Color, params object[] Vars) =>
            WriteCenteredColorBack(top, Text, Color, KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredColorBack(int top, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = TextTools.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth);
                ConsoleWrapper.CursorTop = top;
                for (int i = 0; i < sentences.Length; i++)
                {
                    string sentence = sentences[i];
                    int consoleInfoX = ConsoleWrapper.WindowWidth / 2 - sentence.Length / 2;
                    consoleInfoX = consoleInfoX < 0 ? 0 : consoleInfoX;
                    TextWriterWhereColor.WriteWhereColorBack(sentence + "\n", consoleInfoX, ConsoleWrapper.CursorTop, ForegroundColor, BackgroundColor, Vars);
                }
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(int top, string Text, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = TextTools.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth);
                WriteCentered(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLineKernelColor(int top, string Text, KernelColorType ColTypes, params object[] Vars) =>
            WriteCenteredOneLineColorBack(top, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLineKernelColor(int top, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            WriteCenteredOneLineColorBack(top, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLineColor(int top, string Text, ConsoleColors Color, params object[] Vars) =>
            WriteCenteredOneLineColorBack(top, Text, new Color(Color), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLineColorBack(int top, string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteCenteredOneLineColorBack(top, Text, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLineColor(int top, string Text, Color Color, params object[] Vars) =>
            WriteCenteredOneLineColorBack(top, Text, Color, KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLineColorBack(int top, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                string[] sentences = TextTools.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth);
                WriteCentered(top, sentences[0].Truncate(ConsoleWrapper.WindowWidth - 4));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

    }
}

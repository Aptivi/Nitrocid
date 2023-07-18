
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

using ColorSeq;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using ColorTools = KS.ConsoleBase.Colors.ColorTools;

namespace KS.Misc.Writers.FancyWriters
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
            Text = TextTools.FormatString(Text, Vars);
            string[] sentences = TextTools.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth);
            ConsoleWrapper.CursorTop = top;
            for (int i = 0; i < sentences.Length; i++)
            {
                string sentence = sentences[i];
                int consoleInfoX = (ConsoleWrapper.WindowWidth / 2) - (sentence.Length / 2);
                consoleInfoX = consoleInfoX < 0 ? 0 : consoleInfoX;
                TextWriterWhereColor.WriteWhere(sentence + "\n", consoleInfoX, ConsoleWrapper.CursorTop, Vars);
            }
        }

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(int top, string Text, KernelColorType ColTypes, params object[] Vars) =>
            WriteCentered(top, Text, ColorTools.GetColor(ColTypes), ColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(int top, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            WriteCentered(top, Text, ColorTools.GetColor(colorTypeForeground), ColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(int top, string Text, ConsoleColors Color, params object[] Vars) =>
            WriteCentered(top, Text, new Color(Color), ColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(int top, string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteCentered(top, Text, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(int top, string Text, Color Color, params object[] Vars) =>
            WriteCentered(top, Text, Color, ColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(int top, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            Text = TextTools.FormatString(Text, Vars);
            string[] sentences = TextTools.GetWrappedSentences(Text, ConsoleWrapper.WindowWidth);
            ConsoleWrapper.CursorTop = top;
            for (int i = 0; i < sentences.Length; i++)
            {
                string sentence = sentences[i];
                int consoleInfoX = (ConsoleWrapper.WindowWidth / 2) - (sentence.Length / 2);
                consoleInfoX = consoleInfoX < 0 ? 0 : consoleInfoX;
                TextWriterWhereColor.WriteWhere(sentence + "\n", consoleInfoX, ConsoleWrapper.CursorTop, ForegroundColor, BackgroundColor, Vars);
            }
        }

    }
}

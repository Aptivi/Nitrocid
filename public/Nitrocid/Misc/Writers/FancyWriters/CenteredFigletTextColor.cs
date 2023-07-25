
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
using Figgle;
using KS.ConsoleBase;
using KS.ConsoleBase.Colors;
using KS.Misc.Text;
using KS.Misc.Writers.FancyWriters.Tools;

namespace KS.Misc.Writers.FancyWriters
{
    /// <summary>
    /// Centered Figlet writer
    /// </summary>
    public static class CenteredFigletTextColor
    {

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FiggleFont FigletFont, string Text, params object[] Vars)
        {
            Text = TextTools.FormatString(Text, Vars);
            int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
            int consoleX = (ConsoleWrapper.WindowWidth / 2) - figWidth;
            FigletWhereColor.WriteFigletWhere(Text, consoleX, top, true, FigletFont, KernelColorType.NeutralText, Vars);
        }

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FiggleFont FigletFont, string Text, KernelColorType ColTypes, params object[] Vars) =>
            WriteCenteredFiglet(top, FigletFont, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FiggleFont FigletFont, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            WriteCenteredFiglet(top, FigletFont, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FiggleFont FigletFont, string Text, ConsoleColors Color, params object[] Vars) =>
            WriteCenteredFiglet(top, FigletFont, Text, new Color(Color), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FiggleFont FigletFont, string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteCenteredFiglet(top, FigletFont, Text, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FiggleFont FigletFont, string Text, Color Color, params object[] Vars) =>
            WriteCenteredFiglet(top, FigletFont, Text, Color, KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FiggleFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            Text = TextTools.FormatString(Text, Vars);
            int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
            int consoleX = (ConsoleWrapper.WindowWidth / 2) - figWidth;
            FigletWhereColor.WriteFigletWhere(Text, consoleX, top, true, FigletFont, ForegroundColor, BackgroundColor, Vars);
        }

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FiggleFont FigletFont, string Text, params object[] Vars)
        {
            Text = TextTools.FormatString(Text, Vars);
            int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
            int figHeight = FigletTools.GetFigletHeight(Text, FigletFont) / 2;
            int consoleX = (ConsoleWrapper.WindowWidth / 2) - figWidth;
            int consoleY = (ConsoleWrapper.WindowHeight / 2) - figHeight;
            FigletWhereColor.WriteFigletWhere(Text, consoleX, consoleY, true, FigletFont, KernelColorType.NeutralText, Vars);
        }

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FiggleFont FigletFont, string Text, KernelColorType ColTypes, params object[] Vars) =>
            WriteCenteredFiglet(FigletFont, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FiggleFont FigletFont, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            WriteCenteredFiglet(FigletFont, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FiggleFont FigletFont, string Text, ConsoleColors Color, params object[] Vars) =>
            WriteCenteredFiglet(FigletFont, Text, new Color(Color), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FiggleFont FigletFont, string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteCenteredFiglet(FigletFont, Text, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FiggleFont FigletFont, string Text, Color Color, params object[] Vars) =>
            WriteCenteredFiglet(FigletFont, Text, Color, KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FiggleFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            Text = TextTools.FormatString(Text, Vars);
            int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
            int figHeight = FigletTools.GetFigletHeight(Text, FigletFont) / 2;
            int consoleX = (ConsoleWrapper.WindowWidth / 2) - figWidth;
            int consoleY = (ConsoleWrapper.WindowHeight / 2) - figHeight;
            FigletWhereColor.WriteFigletWhere(Text, consoleX, consoleY, true, FigletFont, ForegroundColor, BackgroundColor, Vars);
        }

    }
}

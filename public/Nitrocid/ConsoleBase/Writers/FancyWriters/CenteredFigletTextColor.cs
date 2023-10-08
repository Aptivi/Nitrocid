
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

using Figletize;
using Figletize.Utilities;
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
        public static void WriteCenteredFiglet(int top, FigletizeFont FigletFont, string Text, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                var figFontFallback = FigletTools.GetFigletFont("small");
                int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
                int figHeight = FigletTools.GetFigletHeight(Text, FigletFont);
                int figWidthFallback = FigletTools.GetFigletWidth(Text, figFontFallback) / 2;
                int figHeightFallback = FigletTools.GetFigletHeight(Text, figFontFallback);
                int consoleX = ConsoleWrapper.WindowWidth / 2 - figWidth;
                int consoleMaxY = top + figHeight;
                if (consoleX < 0 || consoleMaxY > ConsoleWrapper.WindowHeight)
                {
                    // The figlet won't fit, so use small text
                    consoleX = (ConsoleWrapper.WindowWidth / 2) - figWidthFallback;
                    consoleMaxY = top + figHeightFallback;
                    if (consoleX < 0 || consoleMaxY > ConsoleWrapper.WindowHeight)
                    {
                        // The fallback figlet also won't fit, so use smaller text
                        consoleX = (ConsoleWrapper.WindowWidth / 2) - (Text.Length / 2);
                        TextWriterWhereColor.WriteWhereKernelColor(Text, consoleX, top, true, KernelColorType.NeutralText, Vars);
                    }
                    else
                    {
                        // Write the figlet.
                        FigletWhereColor.WriteFigletWhereKernelColor(Text, consoleX, top, true, figFontFallback, KernelColorType.NeutralText, Vars);
                    }
                }
                else
                {
                    // Write the figlet.
                    FigletWhereColor.WriteFigletWhereKernelColor(Text, consoleX, top, true, FigletFont, KernelColorType.NeutralText, Vars);
                }
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletKernelColor(int top, FigletizeFont FigletFont, string Text, KernelColorType ColTypes, params object[] Vars) =>
            WriteCenteredFigletColorBack(top, FigletFont, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletKernelColor(int top, FigletizeFont FigletFont, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            WriteCenteredFigletColorBack(top, FigletFont, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColor(int top, FigletizeFont FigletFont, string Text, ConsoleColors Color, params object[] Vars) =>
            WriteCenteredFigletColorBack(top, FigletFont, Text, new Color(Color), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColorBack(int top, FigletizeFont FigletFont, string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteCenteredFigletColorBack(top, FigletFont, Text, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColor(int top, FigletizeFont FigletFont, string Text, Color Color, params object[] Vars) =>
            WriteCenteredFigletColorBack(top, FigletFont, Text, Color, KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColorBack(int top, FigletizeFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                var figFontFallback = FigletTools.GetFigletFont("small");
                int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
                int figHeight = FigletTools.GetFigletHeight(Text, FigletFont);
                int figWidthFallback = FigletTools.GetFigletWidth(Text, figFontFallback) / 2;
                int figHeightFallback = FigletTools.GetFigletHeight(Text, figFontFallback);
                int consoleX = ConsoleWrapper.WindowWidth / 2 - figWidth;
                int consoleMaxY = top + figHeight;
                if (consoleX < 0 || consoleMaxY > ConsoleWrapper.WindowHeight)
                {
                    // The figlet won't fit, so use small text
                    consoleX = (ConsoleWrapper.WindowWidth / 2) - figWidthFallback;
                    consoleMaxY = top + figHeightFallback;
                    if (consoleX < 0 || consoleMaxY > ConsoleWrapper.WindowHeight)
                    {
                        // The fallback figlet also won't fit, so use smaller text
                        consoleX = (ConsoleWrapper.WindowWidth / 2) - (Text.Length / 2);
                        TextWriterWhereColor.WriteWhereColorBack(Text, consoleX, top, true, ForegroundColor, BackgroundColor, Vars);
                    }
                    else
                    {
                        // Write the figlet.
                        FigletWhereColor.WriteFigletWhereColorBack(Text, consoleX, top, true, figFontFallback, ForegroundColor, BackgroundColor, Vars);
                    }
                }
                else
                {
                    // Write the figlet.
                    FigletWhereColor.WriteFigletWhereColorBack(Text, consoleX, top, true, FigletFont, ForegroundColor, BackgroundColor, Vars);
                }
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FigletizeFont FigletFont, string Text, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                var figFontFallback = FigletTools.GetFigletFont("small");
                int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
                int figHeight = FigletTools.GetFigletHeight(Text, FigletFont) / 2;
                int figWidthFallback = FigletTools.GetFigletWidth(Text, figFontFallback) / 2;
                int figHeightFallback = FigletTools.GetFigletHeight(Text, figFontFallback) / 2;
                int consoleX = ConsoleWrapper.WindowWidth / 2 - figWidth;
                int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                if (consoleX < 0 || consoleY < 0)
                {
                    // The figlet won't fit, so use small text
                    consoleX = (ConsoleWrapper.WindowWidth / 2) - figWidthFallback;
                    consoleY = (ConsoleWrapper.WindowHeight / 2) - figHeightFallback;
                    if (consoleX < 0 || consoleY < 0)
                    {
                        // The fallback figlet also won't fit, so use smaller text
                        consoleX = (ConsoleWrapper.WindowWidth / 2) - (Text.Length / 2);
                        consoleY = ConsoleWrapper.WindowHeight / 2;
                        TextWriterWhereColor.WriteWhereKernelColor(Text, consoleX, consoleY, true, KernelColorType.NeutralText, Vars);
                    }
                    else
                    {
                        // Write the figlet.
                        FigletWhereColor.WriteFigletWhereKernelColor(Text, consoleX, consoleY, true, figFontFallback, KernelColorType.NeutralText, Vars);
                    }
                }
                else
                {
                    // Write the figlet.
                    FigletWhereColor.WriteFigletWhereKernelColor(Text, consoleX, consoleY, true, FigletFont, KernelColorType.NeutralText, Vars);
                }
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletKernelColor(FigletizeFont FigletFont, string Text, KernelColorType ColTypes, params object[] Vars) =>
            WriteCenteredFigletColorBack(FigletFont, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletKernelColor(FigletizeFont FigletFont, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            WriteCenteredFigletColorBack(FigletFont, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColor(FigletizeFont FigletFont, string Text, ConsoleColors Color, params object[] Vars) =>
            WriteCenteredFigletColorBack(FigletFont, Text, new Color(Color), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColorBack(FigletizeFont FigletFont, string Text, ConsoleColors ForegroundColor, ConsoleColors BackgroundColor, params object[] Vars) =>
            WriteCenteredFigletColorBack(FigletFont, Text, new Color(ForegroundColor), new Color(BackgroundColor), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Color">A color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColor(FigletizeFont FigletFont, string Text, Color Color, params object[] Vars) =>
            WriteCenteredFigletColorBack(FigletFont, Text, Color, KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFigletColorBack(FigletizeFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                var figFontFallback = FigletTools.GetFigletFont("small");
                int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
                int figHeight = FigletTools.GetFigletHeight(Text, FigletFont) / 2;
                int figWidthFallback = FigletTools.GetFigletWidth(Text, figFontFallback) / 2;
                int figHeightFallback = FigletTools.GetFigletHeight(Text, figFontFallback) / 2;
                int consoleX = ConsoleWrapper.WindowWidth / 2 - figWidth;
                int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
                if (consoleX < 0 || consoleY < 0)
                {
                    // The figlet won't fit, so use small text
                    consoleX = (ConsoleWrapper.WindowWidth / 2) - figWidthFallback;
                    consoleY = (ConsoleWrapper.WindowHeight / 2) - figHeightFallback;
                    if (consoleX < 0 || consoleY < 0)
                    {
                        // The fallback figlet also won't fit, so use smaller text
                        consoleX = (ConsoleWrapper.WindowWidth / 2) - (Text.Length / 2);
                        consoleY = ConsoleWrapper.WindowHeight / 2;
                        TextWriterWhereColor.WriteWhereColorBack(Text, consoleX, consoleY, true, ForegroundColor, BackgroundColor, Vars);
                    }
                    else
                    {
                        // Write the figlet.
                        FigletWhereColor.WriteFigletWhereColorBack(Text, consoleX, consoleY, true, figFontFallback, ForegroundColor, BackgroundColor, Vars);
                    }
                }
                else
                {
                    // Write the figlet.
                    FigletWhereColor.WriteFigletWhereColorBack(Text, consoleX, consoleY, true, FigletFont, ForegroundColor, BackgroundColor, Vars);
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

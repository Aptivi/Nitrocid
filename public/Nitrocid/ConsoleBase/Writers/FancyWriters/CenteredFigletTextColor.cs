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

using Figletize;
using Figletize.Utilities;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers.ConsoleWriters;
using Nitrocid.Drivers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using System;
using System.Text;
using System.Threading;
using Terminaux.Colors;
using Textify.General;

namespace Nitrocid.ConsoleBase.Writers.FancyWriters
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
                TextWriterColor.WritePlain(RenderCenteredFiglet(top, FigletFont, Text, Vars), false);
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
                TextWriterColor.WritePlain(RenderCenteredFiglet(top, FigletFont, Text, ForegroundColor, BackgroundColor, Vars), false);
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
                TextWriterColor.WritePlain(RenderCenteredFiglet(FigletFont, Text, Vars), false);
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
                TextWriterColor.WritePlain(RenderCenteredFiglet(FigletFont, Text, ForegroundColor, BackgroundColor, Vars), false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Renders a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCenteredFiglet(FigletizeFont FigletFont, string Text, params object[] Vars)
        {
            Text = TextTools.FormatString(Text, Vars);
            var figBuilder = new StringBuilder();
            var figFontFallback = FigletTools.GetFigletFont("small");
            int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
            int figHeight = FigletTools.GetFigletHeight(Text, FigletFont) / 2;
            int figWidthFallback = FigletTools.GetFigletWidth(Text, figFontFallback) / 2;
            int figHeightFallback = FigletTools.GetFigletHeight(Text, figFontFallback) / 2;
            int consoleX = ConsoleWrapper.WindowWidth / 2 - figWidth;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            if (consoleX < 0 || consoleY > ConsoleWrapper.WindowHeight)
            {
                // The figlet won't fit, so use small text
                consoleX = ConsoleWrapper.WindowWidth / 2 - figWidthFallback;
                consoleY = ConsoleWrapper.WindowHeight / 2 - figHeightFallback;
                if (consoleX < 0 || consoleY > ConsoleWrapper.WindowHeight)
                {
                    // The fallback figlet also won't fit, so use smaller text
                    consoleX = ConsoleWrapper.WindowWidth / 2 - Text.Length / 2;
                    figBuilder.Append(
                        DriverHandler.CurrentConsoleDriverLocal.RenderWherePlain(Text, consoleX, consoleY, true, Vars)
                    );
                }
                else
                {
                    // Write the figlet.
                    figBuilder.Append(
                        FigletWhereColor.RenderFigletWherePlain(Text, consoleX, consoleY, true, figFontFallback, Vars)
                    );
                }
            }
            else
            {
                // Write the figlet.
                figBuilder.Append(
                    FigletWhereColor.RenderFigletWherePlain(Text, consoleX, consoleY, true, FigletFont, Vars)
                );
            }
            return figBuilder.ToString();
        }

        /// <summary>
        /// Renders a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCenteredFiglet(FigletizeFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            Text = TextTools.FormatString(Text, Vars);
            var figBuilder = new StringBuilder();
            var figFontFallback = FigletTools.GetFigletFont("small");
            int figWidth = FigletTools.GetFigletWidth(Text, FigletFont) / 2;
            int figHeight = FigletTools.GetFigletHeight(Text, FigletFont) / 2;
            int figWidthFallback = FigletTools.GetFigletWidth(Text, figFontFallback) / 2;
            int figHeightFallback = FigletTools.GetFigletHeight(Text, figFontFallback) / 2;
            int consoleX = ConsoleWrapper.WindowWidth / 2 - figWidth;
            int consoleY = ConsoleWrapper.WindowHeight / 2 - figHeight;
            if (consoleX < 0 || consoleY > ConsoleWrapper.WindowHeight)
            {
                // The figlet won't fit, so use small text
                consoleX = ConsoleWrapper.WindowWidth / 2 - figWidthFallback;
                consoleY = ConsoleWrapper.WindowHeight / 2 - figHeightFallback;
                if (consoleX < 0 || consoleY > ConsoleWrapper.WindowHeight)
                {
                    // The fallback figlet also won't fit, so use smaller text
                    consoleX = ConsoleWrapper.WindowWidth / 2 - Text.Length / 2;
                    figBuilder.Append(
                        ForegroundColor.VTSequenceForeground +
                        BackgroundColor.VTSequenceBackground +
                        DriverHandler.CurrentConsoleDriverLocal.RenderWherePlain(Text, consoleX, consoleY, true, Vars)
                    );
                }
                else
                {
                    // Write the figlet.
                    figBuilder.Append(
                        ForegroundColor.VTSequenceForeground +
                        BackgroundColor.VTSequenceBackground +
                        FigletWhereColor.RenderFigletWherePlain(Text, consoleX, consoleY, true, figFontFallback, Vars)
                    );
                }
            }
            else
            {
                // Write the figlet.
                figBuilder.Append(
                    ForegroundColor.VTSequenceForeground +
                    BackgroundColor.VTSequenceBackground +
                    FigletWhereColor.RenderFigletWherePlain(Text, consoleX, consoleY, true, FigletFont, Vars)
                );
            }

            // Write the resulting buffer
            figBuilder.Append(
                KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground +
                KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground
            );
            return figBuilder.ToString();
        }

        /// <summary>
        /// Renders a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCenteredFiglet(int top, FigletizeFont FigletFont, string Text, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                var figBuilder = new StringBuilder();
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
                    consoleX = ConsoleWrapper.WindowWidth / 2 - figWidthFallback;
                    consoleMaxY = top + figHeightFallback;
                    if (consoleX < 0 || consoleMaxY > ConsoleWrapper.WindowHeight)
                    {
                        // The fallback figlet also won't fit, so use smaller text
                        consoleX = ConsoleWrapper.WindowWidth / 2 - Text.Length / 2;
                        figBuilder.Append(
                            DriverHandler.CurrentConsoleDriverLocal.RenderWherePlain(Text, consoleX, top, true, Vars)
                        );
                    }
                    else
                    {
                        // Write the figlet.
                        figBuilder.Append(
                            FigletWhereColor.RenderFigletWherePlain(Text, consoleX, top, true, figFontFallback, Vars)
                        );
                    }
                }
                else
                {
                    // Write the figlet.
                    figBuilder.Append(
                        FigletWhereColor.RenderFigletWherePlain(Text, consoleX, top, true, FigletFont, Vars)
                    );
                }
                return figBuilder.ToString();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
            return "";
        }

        /// <summary>
        /// Renders a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ForegroundColor">A foreground color that will be changed to.</param>
        /// <param name="BackgroundColor">A background color that will be changed to.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static string RenderCenteredFiglet(int top, FigletizeFont FigletFont, string Text, Color ForegroundColor, Color BackgroundColor, params object[] Vars)
        {
            try
            {
                Text = TextTools.FormatString(Text, Vars);
                var figBuilder = new StringBuilder();
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
                    consoleX = ConsoleWrapper.WindowWidth / 2 - figWidthFallback;
                    consoleMaxY = top + figHeightFallback;
                    if (consoleX < 0 || consoleMaxY > ConsoleWrapper.WindowHeight)
                    {
                        // The fallback figlet also won't fit, so use smaller text
                        consoleX = ConsoleWrapper.WindowWidth / 2 - Text.Length / 2;
                        figBuilder.Append(
                            ForegroundColor.VTSequenceForeground +
                            BackgroundColor.VTSequenceBackground +
                            DriverHandler.CurrentConsoleDriverLocal.RenderWherePlain(Text, consoleX, top, true, Vars)
                        );
                    }
                    else
                    {
                        // Write the figlet.
                        figBuilder.Append(
                            ForegroundColor.VTSequenceForeground +
                            BackgroundColor.VTSequenceBackground +
                            FigletWhereColor.RenderFigletWherePlain(Text, consoleX, top, true, figFontFallback, Vars)
                        );
                    }
                }
                else
                {
                    // Write the figlet.
                    figBuilder.Append(
                        ForegroundColor.VTSequenceForeground +
                        BackgroundColor.VTSequenceBackground +
                        FigletWhereColor.RenderFigletWherePlain(Text, consoleX, top, true, FigletFont, Vars)
                    );
                }

                // Write the resulting buffer
                figBuilder.Append(
                    KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground +
                    KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground
                );
                return figBuilder.ToString();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
            return "";
        }

    }
}

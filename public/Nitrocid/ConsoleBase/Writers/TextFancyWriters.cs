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

using Textify.Figlet.Utilities.Lines;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.FancyWriters.Tools;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using System;
using System.Collections.Generic;
using System.Threading;
using Terminaux.Colors;
using Terminaux.Base;

namespace Nitrocid.ConsoleBase.Writers
{
    /// <summary>
    /// Fancy text writer wrapper for writing with <see cref="KernelColorType"/> (<see cref="Terminaux.Writer.FancyWriters"/>)
    /// </summary>
    public static class TextFancyWriters
    {
        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxBorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxBorderColor, KernelColorType.Background);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxBorderColor, KernelColorType BackgroundColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxBorderColor, BackgroundColor);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, KernelColorType BoxBorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, settings, BoxBorderColor, KernelColorType.Background);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, KernelColorType BoxBorderColor, KernelColorType BackgroundColor)
        {
            try
            {
                // StringBuilder to put out the final rendering text
                string rendered = BorderColor.RenderBorderPlain(Left, Top, InteriorWidth, InteriorHeight, settings);
                TextWriterWhereColor.WriteWhereColorBack(rendered, Left, Top, false, KernelColorTools.GetColor(BoxBorderColor), KernelColorTools.GetColor(BackgroundColor));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxBorderColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxBorderColor, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxBorderColor, KernelColorType BackgroundColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxBorderColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, KernelColorType BoxBorderColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight, settings, BoxBorderColor, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings</param>
        /// <param name="BoxBorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, KernelColorType BoxBorderColor, KernelColorType BackgroundColor, params object[] vars)
        {
            try
            {
                // StringBuilder to put out the final rendering text
                string rendered = BorderColor.RenderBorderPlain(text, Left, Top, InteriorWidth, InteriorHeight, settings, vars);
                TextWriterWhereColor.WriteWhereColorBack(rendered, Left, Top, false, KernelColorTools.GetColor(BoxBorderColor), KernelColorTools.GetColor(BackgroundColor));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the box plainly
        /// </summary>
        /// <param name="Left">Where to place the box horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxColorType">Box color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBox(int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxColorType)
        {
            try
            {
                // Fill the box with spaces inside it
                TextWriterWhereColor.WriteWhereColorBack(BoxColor.RenderBox(Left, Top, InteriorWidth, InteriorHeight), Left, Top, false, KernelColorTools.GetColor(KernelColorType.NeutralText), KernelColorTools.GetColor(BoxColorType));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, KernelColorType.Background);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxFrameColor, KernelColorType BackgroundColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, BackgroundColor);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, KernelColorType BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, settings, BoxFrameColor, KernelColorType.Background);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings</param>
        /// <param name="FrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, KernelColorType FrameColor, KernelColorType BackgroundColor)
        {
            try
            {
                // Render the box frame
                string frame = BoxFrameColor.RenderBoxFrame(Left, Top, InteriorWidth, InteriorHeight, settings);
                TextWriterWhereColor.WriteWhereColorBack(frame, Left, Top, false, KernelColorTools.GetColor(FrameColor), KernelColorTools.GetColor(BackgroundColor));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxFrameColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxFrameColor, KernelColorType BackgroundColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, BorderSettings.GlobalSettings, BoxFrameColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, KernelColorType BoxFrameColor, params object[] vars) =>
            WriteBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, BoxFrameColor, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="settings">Border settings</param>
        /// <param name="FrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, BorderSettings settings, KernelColorType FrameColor, KernelColorType BackgroundColor, params object[] vars)
        {
            try
            {
                // Render the box frame
                string frame = BoxFrameColor.RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, settings, vars);
                TextWriterWhereColor.WriteWhereColorBack(frame, Left, Top, false, KernelColorTools.GetColor(FrameColor), KernelColorTools.GetColor(BackgroundColor));
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
        public static void WriteCenteredFiglet(int top, FigletFont FigletFont, string Text, KernelColorType ColTypes, params object[] Vars) =>
            CenteredFigletTextColor.WriteCenteredFigletColorBack(top, FigletFont, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FigletFont FigletFont, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            CenteredFigletTextColor.WriteCenteredFigletColorBack(top, FigletFont, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FigletFont FigletFont, string Text, KernelColorType ColTypes, params object[] Vars) =>
            CenteredFigletTextColor.WriteCenteredFigletColorBack(FigletFont, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars);

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FigletFont FigletFont, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            CenteredFigletTextColor.WriteCenteredFigletColorBack(FigletFont, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(int top, string Text, KernelColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredTextColor.WriteCenteredColorBack(top, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(int top, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredTextColor.WriteCenteredColorBack(top, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(string Text, KernelColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredTextColor.WriteCenteredColorBack(Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredTextColor.WriteCenteredColorBack(Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(int top, string Text, KernelColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredTextColor.WriteCenteredOneLineColorBack(top, Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(int top, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredTextColor.WriteCenteredOneLineColorBack(top, Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(string Text, KernelColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredTextColor.WriteCenteredOneLineColorBack(Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars) =>
            CenteredTextColor.WriteCenteredOneLineColorBack(Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), leftMargin, rightMargin, Vars);

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFiglet(string Text, FigletFont FigletFont, KernelColorType ColTypes, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WritePlain(FigletColor.RenderFigletPlain(Text, FigletFont, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), Vars), false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFiglet(string Text, FigletFont FigletFont, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars)
        {
            try
            {
                TextWriterRaw.WritePlain(FigletColor.RenderFigletPlain(Text, FigletFont, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), Vars), false);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
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
        public static void WriteFigletWhere(string Text, int Left, int Top, bool Return, FigletFont FigletFont, KernelColorType ColTypes, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                KernelColorTools.SetConsoleColorDry(ColTypes);

                // Actually write
                FigletWhereColor.WriteFigletWherePlain(Text, Left, Top, Return, FigletFont, Vars);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
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
        public static void WriteFigletWhere(string Text, int Left, int Top, bool Return, FigletFont FigletFont, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars)
        {
            try
            {
                // Check if default console output equals the new console output text writer. If it does, write in color, else, suppress the colors.
                KernelColorTools.SetConsoleColorDry(colorTypeForeground);
                KernelColorTools.SetConsoleColorDry(colorTypeBackground, true);

                // Actually write
                FigletWhereColor.WriteFigletWherePlain(Text, Left, Top, Return, FigletFont, Vars);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the PowerLine text
        /// </summary>
        /// <param name="Segments">List of PowerLine segments</param>
        /// <param name="EndingColor">A type of colors that will be changed at the end of the transition</param>
        /// <param name="Line">Write new line after writing the segments</param>
        public static void WritePowerLine(List<PowerLineSegment> Segments, KernelColorType EndingColor, bool Line = false) =>
            PowerLineColor.WritePowerLine(Segments, KernelColorTools.GetColor(EndingColor), Line);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, KernelColorType ProgressColor, bool DrawBorder = true) =>
            ProgressBarColor.WriteProgress(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, KernelColorTools.GetColor(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, int width, KernelColorType ProgressColor, bool DrawBorder = true) =>
            ProgressBarColor.WriteProgress(Progress, Left, Top, width, KernelColorTools.GetColor(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, KernelColorType ProgressColor, KernelColorType FrameColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, int width, KernelColorType ProgressColor, KernelColorType FrameColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, width, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, KernelColorType ProgressColor, KernelColorType FrameColor, KernelColorType BackgroundColor, bool DrawBorder = true) =>
            WriteProgress(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, ProgressColor, FrameColor, BackgroundColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="width">Progress bar width</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteProgress(double Progress, int Left, int Top, int width, KernelColorType ProgressColor, KernelColorType FrameColor, KernelColorType BackgroundColor, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WritePlain(ProgressBarColor.RenderProgress(Progress, Left, Top, width, KernelColorTools.GetColor(ProgressColor), KernelColorTools.GetColor(FrameColor), KernelColorTools.GetColor(BackgroundColor), DrawBorder));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, KernelColorType ProgressColor, bool DrawBorder = true) =>
            ProgressBarVerticalColor.WriteVerticalProgress(Progress, Left, Top, ConsoleWrapper.WindowHeight - 2, KernelColorTools.GetColor(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="height">Progress bar height</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int height, KernelColorType ProgressColor, bool DrawBorder = true) =>
            ProgressBarVerticalColor.WriteVerticalProgress(Progress, Left, Top, height, KernelColorTools.GetColor(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, KernelColorType ProgressColor, KernelColorType FrameColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, ConsoleWrapper.WindowHeight - 2, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="height">Progress bar height</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int height, KernelColorType ProgressColor, KernelColorType FrameColor, bool DrawBorder = true)
        {
            try
            {
                TextWriterRaw.WritePlain(ProgressBarVerticalColor.RenderVerticalProgress(Progress, Left, Top, height, KernelColorTools.GetColor(ProgressColor), KernelColorTools.GetColor(FrameColor), DrawBorder));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, KernelColorType ColTypes, params object[] Vars) =>
            SeparatorWriterColor.WriteSeparatorColorBack(Text, KernelColorTools.GetColor(ColTypes), KernelColorTools.GetColor(KernelColorType.Background), true, Vars);

        /// <summary>
        /// Draw a separator with text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteSeparator(string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, params object[] Vars) =>
            SeparatorWriterColor.WriteSeparatorColorBack(Text, KernelColorTools.GetColor(colorTypeForeground), KernelColorTools.GetColor(colorTypeBackground), true, Vars);

        /// <summary>
        /// Draw a table with text
        /// </summary>
        /// <param name="Headers">Headers to insert to the table.</param>
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="Margin">Margin offset</param>
        /// <param name="SeparateRows">Separate the rows?</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="colorTypeSeparatorForeground">A type of colors that will be changed for the separator foreground color.</param>
        /// <param name="colorTypeHeaderForeground">A type of colors that will be changed for the header foreground color.</param>
        /// <param name="colorTypeValueForeground">A type of colors that will be changed for the value foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        public static void WriteTable(string[] Headers, string[,] Rows, int Margin, KernelColorType colorTypeSeparatorForeground, KernelColorType colorTypeHeaderForeground, KernelColorType colorTypeValueForeground, KernelColorType colorTypeBackground, bool SeparateRows = true, List<CellOptions> CellOptions = null) =>
            TableColor.WriteTable(Headers, Rows, Margin, KernelColorTools.GetColor(colorTypeSeparatorForeground), KernelColorTools.GetColor(colorTypeHeaderForeground), KernelColorTools.GetColor(colorTypeValueForeground), KernelColorTools.GetColor(colorTypeBackground), SeparateRows, CellOptions);
    }
}

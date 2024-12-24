//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Textify.Data.Figlet.Utilities.Lines;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Writer.FancyWriters;
using Terminaux.Writer.CyclicWriters.Renderer.Tools;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using System;
using System.Collections.Generic;
using System.Threading;
using Terminaux.Base;
using Terminaux.Writer.CyclicWriters;
using Textify.General;
using Terminaux.Writer.CyclicWriters.Renderer;
using Terminaux.Colors.Transformation;

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
                var border = new Border()
                {
                    Left = Left,
                    Top = Top,
                    InteriorWidth = InteriorWidth,
                    InteriorHeight = InteriorHeight,
                    Settings = settings,
                    Color = KernelColorTools.GetColor(BoxBorderColor),
                    BackgroundColor = KernelColorTools.GetColor(BackgroundColor)
                };
                TextWriterRaw.WriteRaw(border.Render());
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
                var border = new Border()
                {
                    Text = text.FormatString(vars),
                    Left = Left,
                    Top = Top,
                    InteriorWidth = InteriorWidth,
                    InteriorHeight = InteriorHeight,
                    Settings = settings,
                    Color = KernelColorTools.GetColor(BoxBorderColor),
                    TextColor = KernelColorTools.GetColor(BoxBorderColor),
                    BackgroundColor = KernelColorTools.GetColor(BackgroundColor)
                };
                TextWriterRaw.WriteRaw(border.Render());
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
                var box = new Box()
                {
                    Left = Left,
                    Top = Top,
                    InteriorWidth = InteriorWidth,
                    InteriorHeight = InteriorHeight,
                    Color = KernelColorTools.GetColor(BoxColorType),
                };
                TextWriterRaw.WriteRaw(box.Render());
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
                var boxFrame = new BoxFrame()
                {
                    Left = Left,
                    Top = Top,
                    InteriorWidth = InteriorWidth,
                    InteriorHeight = InteriorHeight,
                    Settings = settings,
                    FrameColor = KernelColorTools.GetColor(FrameColor),
                    BackgroundColor = KernelColorTools.GetColor(BackgroundColor),
                };
                TextWriterRaw.WriteRaw(boxFrame.Render());
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
                var boxFrame = new BoxFrame()
                {
                    Text = text.FormatString(vars),
                    Left = Left,
                    Top = Top,
                    InteriorWidth = InteriorWidth,
                    InteriorHeight = InteriorHeight,
                    Settings = settings,
                    FrameColor = KernelColorTools.GetColor(FrameColor),
                    TitleColor = KernelColorTools.GetColor(FrameColor),
                    BackgroundColor = KernelColorTools.GetColor(BackgroundColor),
                };
                TextWriterRaw.WriteRaw(boxFrame.Render());
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
        /// <param name="leftMargin">Left margin</param>
        /// <param name="rightMargin">Right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FigletFont FigletFont, string Text, KernelColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            var figlet = new AlignedFigletText(FigletFont)
            {
                Top = top,
                Text = Text.FormatString(Vars),
                ForegroundColor = KernelColorTools.GetColor(ColTypes),
                BackgroundColor = KernelColorTools.GetColor(KernelColorType.Background),
                LeftMargin = leftMargin,
                RightMargin = rightMargin,
            };
            TextWriterRaw.WriteRaw(figlet.Render());
        }

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="top">Top position to write centered figlet text to</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">Left margin</param>
        /// <param name="rightMargin">Right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(int top, FigletFont FigletFont, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            var figlet = new AlignedFigletText(FigletFont)
            {
                Top = top,
                Text = Text.FormatString(Vars),
                ForegroundColor = KernelColorTools.GetColor(colorTypeForeground),
                BackgroundColor = KernelColorTools.GetColor(colorTypeBackground),
                LeftMargin = leftMargin,
                RightMargin = rightMargin,
            };
            TextWriterRaw.WriteRaw(figlet.Render());
        }

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">Left margin</param>
        /// <param name="rightMargin">Right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FigletFont FigletFont, string Text, KernelColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            var figlet = new AlignedFigletText(FigletFont)
            {
                Text = Text.FormatString(Vars),
                ForegroundColor = KernelColorTools.GetColor(ColTypes),
                BackgroundColor = KernelColorTools.GetColor(KernelColorType.Background),
                LeftMargin = leftMargin,
                RightMargin = rightMargin,
            };
            TextWriterRaw.WriteRaw(figlet.Render());
        }

        /// <summary>
        /// Draw a centered figlet with text
        /// </summary>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered figlet.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">Left margin</param>
        /// <param name="rightMargin">Right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredFiglet(FigletFont FigletFont, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            var figlet = new AlignedFigletText(FigletFont)
            {
                Text = Text.FormatString(Vars),
                ForegroundColor = KernelColorTools.GetColor(colorTypeForeground),
                BackgroundColor = KernelColorTools.GetColor(colorTypeBackground),
                LeftMargin = leftMargin,
                RightMargin = rightMargin,
            };
            TextWriterRaw.WriteRaw(figlet.Render());
        }

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(int top, string Text, KernelColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            var text = new AlignedText()
            {
                Top = top,
                Text = Text.FormatString(Vars),
                ForegroundColor = KernelColorTools.GetColor(ColTypes),
                BackgroundColor = KernelColorTools.GetColor(KernelColorType.Background),
                LeftMargin = leftMargin,
                RightMargin = rightMargin,
            };
            TextWriterRaw.WriteRaw(text.Render());
        }

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
        public static void WriteCentered(int top, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            var text = new AlignedText()
            {
                Top = top,
                Text = Text.FormatString(Vars),
                ForegroundColor = KernelColorTools.GetColor(colorTypeForeground),
                BackgroundColor = KernelColorTools.GetColor(colorTypeBackground),
                LeftMargin = leftMargin,
                RightMargin = rightMargin,
            };
            TextWriterRaw.WriteRaw(text.Render());
        }

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(string Text, KernelColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            var text = new AlignedText()
            {
                Text = Text.FormatString(Vars),
                ForegroundColor = KernelColorTools.GetColor(ColTypes),
                BackgroundColor = KernelColorTools.GetColor(KernelColorType.Background),
                LeftMargin = leftMargin,
                RightMargin = rightMargin,
            };
            TextWriterRaw.WriteRaw(text.Render());
        }

        /// <summary>
        /// Draws a centered text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCentered(string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            var text = new AlignedText()
            {
                Text = Text.FormatString(Vars),
                ForegroundColor = KernelColorTools.GetColor(colorTypeForeground),
                BackgroundColor = KernelColorTools.GetColor(colorTypeBackground),
                LeftMargin = leftMargin,
                RightMargin = rightMargin,
            };
            TextWriterRaw.WriteRaw(text.Render());
        }

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="top">Top position to write centered text to</param>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(int top, string Text, KernelColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            var text = new AlignedText()
            {
                Top = top,
                Text = Text.FormatString(Vars),
                ForegroundColor = KernelColorTools.GetColor(ColTypes),
                BackgroundColor = KernelColorTools.GetColor(KernelColorType.Background),
                LeftMargin = leftMargin,
                RightMargin = rightMargin,
                OneLine = true,
            };
            TextWriterRaw.WriteRaw(text.Render());
        }

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
        public static void WriteCenteredOneLine(int top, string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            var text = new AlignedText()
            {
                Top = top,
                Text = Text.FormatString(Vars),
                ForegroundColor = KernelColorTools.GetColor(colorTypeForeground),
                BackgroundColor = KernelColorTools.GetColor(colorTypeBackground),
                LeftMargin = leftMargin,
                RightMargin = rightMargin,
                OneLine = true,
            };
            TextWriterRaw.WriteRaw(text.Render());
        }

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(string Text, KernelColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            var text = new AlignedText()
            {
                Text = Text.FormatString(Vars),
                ForegroundColor = KernelColorTools.GetColor(ColTypes),
                BackgroundColor = KernelColorTools.GetColor(KernelColorType.Background),
                LeftMargin = leftMargin,
                RightMargin = rightMargin,
                OneLine = true,
            };
            TextWriterRaw.WriteRaw(text.Render());
        }

        /// <summary>
        /// Draws a centered text (just the first line)
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the centered.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">The left margin</param>
        /// <param name="rightMargin">The right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteCenteredOneLine(string Text, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            var text = new AlignedText()
            {
                Text = Text.FormatString(Vars),
                ForegroundColor = KernelColorTools.GetColor(colorTypeForeground),
                BackgroundColor = KernelColorTools.GetColor(colorTypeBackground),
                LeftMargin = leftMargin,
                RightMargin = rightMargin,
                OneLine = true,
            };
            TextWriterRaw.WriteRaw(text.Render());
        }

        /// <summary>
        /// Writes the figlet text
        /// </summary>
        /// <param name="Text">Text to be written. If nothing, the entire line is filled with the separator.</param>
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">Left margin</param>
        /// <param name="rightMargin">Right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFiglet(string Text, FigletFont FigletFont, KernelColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                var figlet = new FigletText(FigletFont)
                {
                    Text = Text.FormatString(Vars),
                    ForegroundColor = KernelColorTools.GetColor(ColTypes),
                    BackgroundColor = KernelColorTools.GetColor(KernelColorType.Background),
                    LeftMargin = leftMargin,
                    RightMargin = rightMargin,
                };
                TextWriterRaw.WriteRaw(figlet.Render());
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
        /// <param name="leftMargin">Left margin</param>
        /// <param name="rightMargin">Right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFiglet(string Text, FigletFont FigletFont, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                var figlet = new FigletText(FigletFont)
                {
                    Text = Text.FormatString(Vars),
                    ForegroundColor = KernelColorTools.GetColor(colorTypeForeground),
                    BackgroundColor = KernelColorTools.GetColor(colorTypeBackground),
                    LeftMargin = leftMargin,
                    RightMargin = rightMargin,
                };
                TextWriterRaw.WriteRaw(figlet.Render());
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
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="ColTypes">A type of colors that will be changed.</param>
        /// <param name="leftMargin">Left margin</param>
        /// <param name="rightMargin">Right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletWhere(string Text, int Left, int Top, FigletFont FigletFont, KernelColorType ColTypes, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                var figlet = new FigletText(FigletFont)
                {
                    Text = Text.FormatString(Vars),
                    ForegroundColor = KernelColorTools.GetColor(ColTypes),
                    BackgroundColor = KernelColorTools.GetColor(KernelColorType.Background),
                    LeftMargin = leftMargin,
                    RightMargin = rightMargin,
                };
                TextWriterRaw.WriteRaw(ContainerTools.RenderRenderable(figlet, new(Left, Top)));
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
        /// <param name="FigletFont">Figlet font to use in the text.</param>
        /// <param name="colorTypeForeground">A type of colors that will be changed for the foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        /// <param name="leftMargin">Left margin</param>
        /// <param name="rightMargin">Right margin</param>
        /// <param name="Vars">Variables to format the message before it's written.</param>
        public static void WriteFigletWhere(string Text, int Left, int Top, FigletFont FigletFont, KernelColorType colorTypeForeground, KernelColorType colorTypeBackground, int leftMargin = 0, int rightMargin = 0, params object[] Vars)
        {
            try
            {
                var figlet = new FigletText(FigletFont)
                {
                    Text = Text.FormatString(Vars),
                    ForegroundColor = KernelColorTools.GetColor(colorTypeForeground),
                    BackgroundColor = KernelColorTools.GetColor(colorTypeBackground),
                    LeftMargin = leftMargin,
                    RightMargin = rightMargin,
                };
                TextWriterRaw.WriteRaw(ContainerTools.RenderRenderable(figlet, new(Left, Top)));
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
        public static void WritePowerLine(List<PowerLineSegment> Segments, KernelColorType EndingColor, bool Line = false)
        {
            var powerLine = new PowerLine()
            {
                EndingColor = KernelColorTools.GetColor(EndingColor),
                Segments = Segments
            };
            TextWriterRaw.WriteRaw(powerLine.Render());
            if (Line)
                TextWriterRaw.Write();
        }

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        public static void WriteProgress(double Progress, int Left, int Top, KernelColorType ProgressColor) =>
            WriteProgress(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, ProgressColor, KernelColorType.Background);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="width">Progress bar width</param>
        public static void WriteProgress(double Progress, int Left, int Top, int width, KernelColorType ProgressColor) =>
            WriteProgress(Progress, Left, Top, width, ProgressColor, KernelColorType.Background);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        public static void WriteProgress(double Progress, int Left, int Top, KernelColorType ProgressColor, KernelColorType BackgroundColor) =>
            WriteProgress(Progress, Left, Top, ConsoleWrapper.WindowWidth - 10, ProgressColor, BackgroundColor);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="width">Progress bar width</param>
        public static void WriteProgress(double Progress, int Left, int Top, int width, KernelColorType ProgressColor, KernelColorType BackgroundColor)
        {
            try
            {
                var progress = new ProgressBarNoText((int)Progress, 100)
                {
                    LeftMargin = ConsoleWrapper.WindowWidth - width,
                    ProgressActiveForegroundColor = KernelColorTools.GetColor(ProgressColor),
                    ProgressForegroundColor = TransformationTools.GetDarkBackground(KernelColorTools.GetColor(ProgressColor)),
                    ProgressBackgroundColor = KernelColorTools.GetColor(BackgroundColor),
                };
                TextWriterRaw.WriteRaw(ContainerTools.RenderRenderable(progress, new(Left, Top)));
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
        public static void WriteVerticalProgress(double Progress, int Left, int Top, KernelColorType ProgressColor) =>
            WriteVerticalProgress(Progress, Left, Top, ConsoleWrapper.WindowHeight - 2, ProgressColor, KernelColorType.Background);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="height">Progress bar height</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int height, KernelColorType ProgressColor) =>
            WriteVerticalProgress(Progress, Left, Top, height, ProgressColor, KernelColorType.Background);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, KernelColorType ProgressColor, KernelColorType BackgroundColor) =>
            WriteVerticalProgress(Progress, Left, Top, ConsoleWrapper.WindowHeight - 2, ProgressColor, BackgroundColor);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="BackgroundColor">The progress bar background color</param>
        /// <param name="height">Progress bar height</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int height, KernelColorType ProgressColor, KernelColorType BackgroundColor)
        {
            try
            {
                var progress = new SimpleProgress((int)Progress, 100)
                {
                    Vertical = true,
                    Height = height,
                    ProgressActiveForegroundColor = KernelColorTools.GetColor(ProgressColor),
                    ProgressForegroundColor = TransformationTools.GetDarkBackground(KernelColorTools.GetColor(ProgressColor)),
                    ProgressBackgroundColor = KernelColorTools.GetColor(BackgroundColor),
                };
                TextWriterRaw.WriteRaw(ContainerTools.RenderRenderable(progress, new(Left, Top)));
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
        /// <param name="Rows">Rows to insert to the table.</param>
        /// <param name="left">Left position of the upper-left corner</param>
        /// <param name="top">Top position of the upper-left corner</param>
        /// <param name="width">Table interior width</param>
        /// <param name="height">Table interior height</param>
        /// <param name="enableHeader">Whether to enable the header or no</param>
        /// <param name="borderSettings">Specifies the table border settings</param>
        /// <param name="CellOptions">Specifies the cell options</param>
        /// <param name="colorTypeSeparatorForeground">A type of colors that will be changed for the separator foreground color.</param>
        /// <param name="colorTypeHeaderForeground">A type of colors that will be changed for the header foreground color.</param>
        /// <param name="colorTypeValueForeground">A type of colors that will be changed for the value foreground color.</param>
        /// <param name="colorTypeBackground">A type of colors that will be changed for the background color.</param>
        public static void WriteTable(string[,] Rows, int left, int top, int width, int height, bool enableHeader, KernelColorType colorTypeSeparatorForeground, KernelColorType colorTypeHeaderForeground, KernelColorType colorTypeValueForeground, KernelColorType colorTypeBackground, List<CellOptions>? CellOptions = null, BorderSettings? borderSettings = null)
        {
            var table = new Table()
            {
                Rows = Rows,
                Left = left,
                Top = top,
                InteriorWidth = width,
                InteriorHeight = height,
                Header = enableHeader,
                SeparatorColor = KernelColorTools.GetColor(colorTypeSeparatorForeground),
                HeaderColor = KernelColorTools.GetColor(colorTypeHeaderForeground),
                ValueColor = KernelColorTools.GetColor(colorTypeValueForeground),
                BackgroundColor = KernelColorTools.GetColor(colorTypeBackground),
                Settings = CellOptions ?? [],
                BorderSettings = borderSettings ?? new(),
            };
            TextWriterRaw.WriteRaw(table.Render());
        }

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, KernelColorType sliderColor, int minPos = 0) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, sliderColor, KernelColorType.Separator, minPos);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="width">Slider width</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, KernelColorType sliderColor, int minPos = 0) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, width, sliderColor, KernelColorType.Separator, minPos);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, KernelColorType sliderColor, KernelColorType BackgroundColor, int minPos = 0) =>
            WriteSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, sliderColor, BackgroundColor, minPos);

        /// <summary>
        /// Writes the slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="width">Slider width</param>
        public static void WriteSliderAbsolute(int currPos, int maxPos, int Left, int Top, int width, KernelColorType sliderColor, KernelColorType BackgroundColor, int minPos = 0)
        {
            var slider = new Slider(width * ((currPos - minPos) / (maxPos - minPos)), minPos, width + 1)
            {
                Width = width,
                SliderActiveForegroundColor = KernelColorTools.GetColor(sliderColor),
                SliderForegroundColor = TransformationTools.GetDarkBackground(KernelColorTools.GetColor(sliderColor)),
                SliderBackgroundColor = KernelColorTools.GetColor(BackgroundColor),
            };
            TextWriterRaw.WriteRaw(ContainerTools.RenderRenderable(slider, new(Left, Top)));
        }

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, KernelColorType sliderColor) =>
            WriteSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, sliderColor, KernelColorType.Separator);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="width">Slider width</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int width, KernelColorType sliderColor) =>
            WriteSlider(currPos, maxPos, Left, Top, width, sliderColor, KernelColorType.Separator);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, KernelColorType sliderColor, KernelColorType BackgroundColor) =>
            WriteSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowWidth - 10, sliderColor, BackgroundColor);

        /// <summary>
        /// Writes the slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="sliderColor">The slider color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="width">Slider width</param>
        public static void WriteSlider(int currPos, int maxPos, int Left, int Top, int width, KernelColorType sliderColor, KernelColorType BackgroundColor)
        {
            var slider = new Slider(currPos, 0, maxPos)
            {
                Width = width,
                SliderActiveForegroundColor = KernelColorTools.GetColor(sliderColor),
                SliderForegroundColor = TransformationTools.GetDarkBackground(KernelColorTools.GetColor(sliderColor)),
                SliderBackgroundColor = KernelColorTools.GetColor(BackgroundColor),
            };
            TextWriterRaw.WriteRaw(ContainerTools.RenderRenderable(slider, new(Left, Top)));
        }

        /// <summary>
        /// Writes the vertical slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        public static void WriteVerticalSliderAbsolute(int currPos, int maxPos, int Left, int Top, KernelColorType SliderColor, int minPos = 0) =>
            WriteVerticalSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowHeight - 2, SliderColor, KernelColorType.Separator, minPos);

        /// <summary>
        /// Writes the vertical slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="height">Slider height</param>
        public static void WriteVerticalSliderAbsolute(int currPos, int maxPos, int Left, int Top, int height, KernelColorType SliderColor, int minPos = 0) =>
            WriteVerticalSliderAbsolute(currPos, maxPos, Left, Top, height, SliderColor, KernelColorType.Separator, minPos);

        /// <summary>
        /// Writes the vertical slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        public static void WriteVerticalSliderAbsolute(int currPos, int maxPos, int Left, int Top, KernelColorType SliderColor, KernelColorType BackgroundColor, int minPos = 0) =>
            WriteVerticalSliderAbsolute(currPos, maxPos, Left, Top, ConsoleWrapper.WindowHeight - 2, SliderColor, BackgroundColor, minPos);

        /// <summary>
        /// Writes the vertical slider (absolute)
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="minPos">Minimum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="height">Slider height</param>
        public static void WriteVerticalSliderAbsolute(int currPos, int maxPos, int Left, int Top, int height, KernelColorType SliderColor, KernelColorType BackgroundColor, int minPos = 0)
        {
            var slider = new Slider(height * ((currPos - minPos) / (maxPos - minPos)), minPos, height + 1)
            {
                Vertical = true,
                Height = height,
                SliderActiveForegroundColor = KernelColorTools.GetColor(SliderColor),
                SliderForegroundColor = TransformationTools.GetDarkBackground(KernelColorTools.GetColor(SliderColor)),
                SliderBackgroundColor = KernelColorTools.GetColor(BackgroundColor),
            };
            TextWriterRaw.WriteRaw(ContainerTools.RenderRenderable(slider, new(Left, Top)));
        }

        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, KernelColorType SliderColor) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowHeight - 2, SliderColor, KernelColorType.Separator);

        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="height">Slider height</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, int height, KernelColorType SliderColor) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, height, SliderColor, KernelColorType.Separator);

        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, KernelColorType SliderColor, KernelColorType BackgroundColor) =>
            WriteVerticalSlider(currPos, maxPos, Left, Top, ConsoleWrapper.WindowHeight - 2, SliderColor, BackgroundColor);

        /// <summary>
        /// Writes the vertical slider
        /// </summary>
        /// <param name="currPos">Current position out of maximum position</param>
        /// <param name="maxPos">Maximum position</param>
        /// <param name="Left">The slider position from the upper left corner</param>
        /// <param name="Top">The slider position from the top</param>
        /// <param name="SliderColor">The slider color</param>
        /// <param name="BackgroundColor">The slider background color</param>
        /// <param name="height">Slider height</param>
        public static void WriteVerticalSlider(int currPos, int maxPos, int Left, int Top, int height, KernelColorType SliderColor, KernelColorType BackgroundColor)
        {
            var slider = new Slider(currPos, 0, maxPos)
            {
                Vertical = true,
                Height = height,
                SliderActiveForegroundColor = KernelColorTools.GetColor(SliderColor),
                SliderForegroundColor = TransformationTools.GetDarkBackground(KernelColorTools.GetColor(SliderColor)),
                SliderBackgroundColor = KernelColorTools.GetColor(BackgroundColor),
            };
            TextWriterRaw.WriteRaw(ContainerTools.RenderRenderable(slider, new(Left, Top)));
        }
    }
}

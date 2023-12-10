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
using KS.Kernel.Debugging;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.ConsoleBase.Writers.FancyWriters.Tools;
using Terminaux.Colors;
using System.Text;
using KS.ConsoleBase.Writers.ConsoleWriters;

namespace KS.ConsoleBase.Writers.FancyWriters
{
    /// <summary>
    /// Border writer with color support
    /// </summary>
    public static class BorderTextColor
    {
        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBorderTextPlain(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, params object[] vars) =>
            WriteBorderTextPlain(text, Left, Top, InteriorWidth, InteriorHeight,
                             BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                             BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                             BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                             BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for border</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for border</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for border</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for border</param>
        /// <param name="UpperFrameChar">Upper frame character for border</param>
        /// <param name="LowerFrameChar">Lower frame character for border</param>
        /// <param name="LeftFrameChar">Left frame character for border</param>
        /// <param name="RightFrameChar">Right frame character for border</param>
        public static void WriteBorderTextPlain(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars)
        {
            try
            {
                // StringBuilder to put out the final rendering text
                string rendered = RenderBorderTextPlain(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, vars);
                TextWriterWhereColor.WriteWhere(rendered, Left, Top);
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
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        KernelColorType.Separator, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BorderColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BorderColor, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BorderColor, KernelColorType BackgroundColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BorderColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, ConsoleColors BorderColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(BorderColor), KernelColorTools.GetColor(KernelColorType.Background), vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="Color"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, ConsoleColors BorderColor, ConsoleColors BackgroundColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(BorderColor), new Color(BackgroundColor), vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, Color BorderColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BorderColor, KernelColorTools.GetColor(KernelColorType.Background), vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="Color"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, Color BorderColor, Color BackgroundColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BorderColor, BackgroundColor, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for border</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for border</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for border</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for border</param>
        /// <param name="UpperFrameChar">Upper frame character for border</param>
        /// <param name="LowerFrameChar">Lower frame character for border</param>
        /// <param name="LeftFrameChar">Left frame character for border</param>
        /// <param name="RightFrameChar">Right frame character for border</param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, KernelColorType.Separator, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for border</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for border</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for border</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for border</param>
        /// <param name="UpperFrameChar">Upper frame character for border</param>
        /// <param name="LowerFrameChar">Lower frame character for border</param>
        /// <param name="LeftFrameChar">Left frame character for border</param>
        /// <param name="RightFrameChar">Right frame character for border</param>
        /// <param name="BorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       KernelColorType BorderColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BorderColor, KernelColorType.Background, vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for border</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for border</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for border</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for border</param>
        /// <param name="UpperFrameChar">Upper frame character for border</param>
        /// <param name="LowerFrameChar">Lower frame character for border</param>
        /// <param name="LeftFrameChar">Left frame character for border</param>
        /// <param name="RightFrameChar">Right frame character for border</param>
        /// <param name="BorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       KernelColorType BorderColor, KernelColorType BackgroundColor, params object[] vars)
        {
            try
            {
                // StringBuilder to put out the final rendering text
                string rendered = RenderBorderTextPlain(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, vars);
                TextWriterWhereColor.WriteWhereKernelColor(rendered, Left, Top, false, BorderColor, BackgroundColor);
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
        /// <param name="UpperLeftCornerChar">Upper left corner character for border</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for border</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for border</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for border</param>
        /// <param name="UpperFrameChar">Upper frame character for border</param>
        /// <param name="LowerFrameChar">Lower frame character for border</param>
        /// <param name="LeftFrameChar">Left frame character for border</param>
        /// <param name="RightFrameChar">Right frame character for border</param>
        /// <param name="BorderColor">Border color</param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color BorderColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BorderColor, KernelColorTools.GetColor(KernelColorType.Background), vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for border</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for border</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for border</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for border</param>
        /// <param name="UpperFrameChar">Upper frame character for border</param>
        /// <param name="LowerFrameChar">Lower frame character for border</param>
        /// <param name="LeftFrameChar">Left frame character for border</param>
        /// <param name="RightFrameChar">Right frame character for border</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       Color BorderColor, Color BackgroundColor, params object[] vars)
        {
            try
            {
                // StringBuilder to put out the final rendering text
                string rendered = RenderBorderTextPlain(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, vars);
                TextWriterWhereColor.WriteWhereColorBack(rendered, Left, Top, false, BorderColor, BackgroundColor);
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
        /// <param name="UpperLeftCornerChar">Upper left corner character for border</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for border</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for border</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for border</param>
        /// <param name="UpperFrameChar">Upper frame character for border</param>
        /// <param name="LowerFrameChar">Lower frame character for border</param>
        /// <param name="LeftFrameChar">Left frame character for border</param>
        /// <param name="RightFrameChar">Right frame character for border</param>
        /// <param name="BorderColor">Border color</param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors BorderColor, params object[] vars) =>
            WriteBorderText(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(BorderColor), KernelColorTools.GetColor(KernelColorType.Background), vars);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for border</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for border</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for border</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for border</param>
        /// <param name="UpperFrameChar">Upper frame character for border</param>
        /// <param name="LowerFrameChar">Lower frame character for border</param>
        /// <param name="LeftFrameChar">Left frame character for border</param>
        /// <param name="RightFrameChar">Right frame character for border</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        public static void WriteBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                       ConsoleColors BorderColor, ConsoleColors BackgroundColor, params object[] vars)
        {
            try
            {
                // StringBuilder to put out the final rendering text
                string rendered = RenderBorderTextPlain(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, vars);
                TextWriterWhereColor.WriteWhereColorBack(rendered, Left, Top, false, BorderColor, BackgroundColor);
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static string RenderBorderTextPlain(string text, int Left, int Top, int InteriorWidth, int InteriorHeight, params object[] vars) =>
            RenderBorderTextPlain(text, Left, Top, InteriorWidth, InteriorHeight,
                             BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                             BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                             BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                             BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar, vars);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for border</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for border</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for border</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for border</param>
        /// <param name="UpperFrameChar">Upper frame character for border</param>
        /// <param name="LowerFrameChar">Lower frame character for border</param>
        /// <param name="LeftFrameChar">Left frame character for border</param>
        /// <param name="RightFrameChar">Right frame character for border</param>
        public static string RenderBorderTextPlain(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                               char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                               char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, params object[] vars)
        {
            StringBuilder border = new();
            try
            {
                // StringBuilder to put out the final rendering text
                border.Append(
                    BoxFrameTextColor.RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, vars) +
                    BoxColor.RenderBox(Left + 1, Top, InteriorWidth, InteriorHeight)
                );
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
            return border.ToString();
        }

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        public static string RenderBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                          Color BorderColor, Color BackgroundColor, params object[] vars) =>
            RenderBorderText(text, Left, Top, InteriorWidth, InteriorHeight,
                             BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                             BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                             BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                             BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                             BorderColor, BackgroundColor, vars);

        /// <summary>
        /// Renders the border plainly
        /// </summary>
        /// <param name="text">Text to be written.</param>
        /// <param name="vars">Variables to format the message before it's written.</param>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for border</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for border</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for border</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for border</param>
        /// <param name="UpperFrameChar">Upper frame character for border</param>
        /// <param name="LowerFrameChar">Lower frame character for border</param>
        /// <param name="LeftFrameChar">Left frame character for border</param>
        /// <param name="RightFrameChar">Right frame character for border</param>
        /// <param name="BorderColor">Border color</param>
        /// <param name="BackgroundColor">Border background color</param>
        public static string RenderBorderText(string text, int Left, int Top, int InteriorWidth, int InteriorHeight,
                                               char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar,
                                               char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar,
                                               Color BorderColor, Color BackgroundColor, params object[] vars)
        {
            StringBuilder border = new();
            try
            {
                // StringBuilder to put out the final rendering text
                border.Append(
                    BorderColor.VTSequenceForeground +
                    BackgroundColor.VTSequenceBackground +
                    BoxFrameTextColor.RenderBoxFrame(text, Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, vars) +
                    BoxColor.RenderBox(Left + 1, Top, InteriorWidth, InteriorHeight) +
                    KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground +
                    KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground
                );
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
            return border.ToString();
        }
    }
}

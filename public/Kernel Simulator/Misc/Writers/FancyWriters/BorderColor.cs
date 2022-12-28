
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

using Extensification.StringExts;
using KS.Misc.Writers.ConsoleWriters;
using System;
using System.Threading;
using static KS.ConsoleBase.Colors.ColorTools;
using KS.Kernel.Debugging;
using KS.ConsoleBase.Colors;
using KS.Languages;
using ColorSeq;
using KS.Misc.Writers.FancyWriters.Tools;

namespace KS.Misc.Writers.FancyWriters
{
    /// <summary>
    /// Border writer with color support
    /// </summary>
    public static class BorderColor
    {
        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBorderPlain(int Left, int Top, int InteriorWidth, int InteriorHeight) =>
            WriteBorderPlain(Left, Top, InteriorWidth, InteriorHeight,
                             BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                             BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                             BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                             BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
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
        public static void WriteBorderPlain(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                            string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                            string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar)
        {
            try 
            {
                // First, draw the border
                TextWriterWhereColor.WriteWhere(UpperLeftCornerChar + UpperFrameChar.Repeat(InteriorWidth) + UpperRightCornerChar, Left, Top, true);
                for (int i = 1; i <= InteriorHeight; i++)
                {
                    TextWriterWhereColor.WriteWhere(LeftFrameChar, Left, Top + i, true);
                    TextWriterWhereColor.WriteWhere(RightFrameChar, Left + InteriorWidth + 1, Top + i, true);
                }
                TextWriterWhereColor.WriteWhere(LowerLeftCornerChar + LowerFrameChar.Repeat(InteriorWidth) + LowerRightCornerChar, Left, Top + InteriorHeight + 1, true);

                // Then, fill the border with spaces inside it
                for (int x = 1; x <= InteriorWidth; x++)
                    for (int y = 1; y <= InteriorHeight; y++)
                        TextWriterWhereColor.WriteWhere(" ", Left + x, Top + y, true);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        KernelColorType.Separator, KernelColorType.Background);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Kernel Simulator's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BorderColor, KernelColorType.Background);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Kernel Simulator's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Kernel Simulator's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BorderColor, KernelColorType BackgroundColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BorderColor, BackgroundColor);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Kernel Simulator's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, ConsoleColors BorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(Convert.ToInt32(BorderColor)), GetColor(KernelColorType.Background));

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Kernel Simulator's <see cref="Color"/></param>
        /// <param name="BackgroundColor">Border background color from Kernel Simulator's <see cref="Color"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, ConsoleColors BorderColor, ConsoleColors BackgroundColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(Convert.ToInt32(BorderColor)), new Color(Convert.ToInt32(BackgroundColor)));

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Kernel Simulator's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BorderColor, GetColor(KernelColorType.Background));

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Kernel Simulator's <see cref="Color"/></param>
        /// <param name="BackgroundColor">Border background color from Kernel Simulator's <see cref="Color"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BorderColor, Color BackgroundColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BorderColor, BackgroundColor);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
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
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                       string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                       string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, KernelColorType.Separator, KernelColorType.Background);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
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
        /// <param name="BorderColor">Border color from Kernel Simulator's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                       string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                       string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar, 
                                       KernelColorType BorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BorderColor, KernelColorType.Background);

        /// <summary>
        /// Writes the border plainly
        /// </summary>
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
        /// <param name="BorderColor">Border color from Kernel Simulator's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Kernel Simulator's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                       string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                       string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar, 
                                       KernelColorType BorderColor, KernelColorType BackgroundColor)
        {
            try
            {
                SetConsoleColor(BorderColor, false);
                SetConsoleColor(BackgroundColor, true, true);
                WriteBorderPlain(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the border plainly
        /// </summary>
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
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                       string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                       string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar, 
                                       Color BorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BorderColor, GetColor(KernelColorType.Background));

        /// <summary>
        /// Writes the border plainly
        /// </summary>
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
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                       string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                       string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar, 
                                       Color BorderColor, Color BackgroundColor)
        {
            try
            {
                SetConsoleColor(BorderColor, false);
                SetConsoleColor(BackgroundColor, true, true);
                WriteBorderPlain(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Writes the border plainly
        /// </summary>
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
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                       string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                       string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar, 
                                       ConsoleColors BorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(Convert.ToInt32(BorderColor)), GetColor(KernelColorType.Background));

        /// <summary>
        /// Writes the border plainly
        /// </summary>
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
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                       string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                       string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar, 
                                       ConsoleColors BorderColor, ConsoleColors BackgroundColor)
        {
            try
            {
                SetConsoleColor(new Color(Convert.ToInt32(BorderColor)), false);
                SetConsoleColor(new Color(Convert.ToInt32(BackgroundColor)), true, true);
                WriteBorderPlain(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }
    }
}

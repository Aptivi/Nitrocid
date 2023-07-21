
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
                                            char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar, 
                                            char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar)
        {
            try 
            {
                // First, draw the border
                BoxFrameColor.WriteBoxFramePlain(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);

                // Then, fill the border with spaces inside it
                BoxColor.WriteBoxPlain(Left + 1, Top, InteriorWidth, InteriorHeight);
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
        /// <param name="BorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
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
        /// <param name="BorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="KernelColorType"/></param>
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
        /// <param name="BorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, ConsoleColors BorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(BorderColor), GetColor(KernelColorType.Background));

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="Color"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, ConsoleColors BorderColor, ConsoleColors BackgroundColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(BorderColor), new Color(BackgroundColor));

        /// <summary>
        /// Writes the border plainly
        /// </summary>
        /// <param name="Left">Where to place the border horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the border vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
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
        /// <param name="BorderColor">Border color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="Color"/></param>
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
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar, 
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar) =>
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
        /// <param name="BorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar, 
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, 
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
        /// <param name="BorderColor">Border color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">Border background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBorder(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar, 
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, 
                                       KernelColorType BorderColor, KernelColorType BackgroundColor)
        {
            try
            {
                // First, draw the border
                BoxFrameColor.WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BorderColor, BackgroundColor);

                // Then, fill the border with spaces inside it
                BoxColor.WriteBox(Left + 1, Top, InteriorWidth, InteriorHeight, BackgroundColor);
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
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar, 
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, 
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
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar, 
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, 
                                       Color BorderColor, Color BackgroundColor)
        {
            try
            {
                // First, draw the border
                BoxFrameColor.WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BorderColor, BackgroundColor);

                // Then, fill the border with spaces inside it
                BoxColor.WriteBox(Left + 1, Top, InteriorWidth, InteriorHeight, BackgroundColor);
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
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar, 
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, 
                                       ConsoleColors BorderColor) =>
            WriteBorder(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(BorderColor), GetColor(KernelColorType.Background));

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
                                       char UpperLeftCornerChar, char LowerLeftCornerChar, char UpperRightCornerChar, char LowerRightCornerChar, 
                                       char UpperFrameChar, char LowerFrameChar, char LeftFrameChar, char RightFrameChar, 
                                       ConsoleColors BorderColor, ConsoleColors BackgroundColor)
        {
            try
            {
                // First, draw the border
                BoxFrameColor.WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BorderColor, BackgroundColor);

                // Then, fill the border with spaces inside it
                BoxColor.WriteBox(Left + 1, Top, InteriorWidth, InteriorHeight, BackgroundColor);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }
    }
}

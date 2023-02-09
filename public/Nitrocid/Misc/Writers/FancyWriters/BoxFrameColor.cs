
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
    /// BoxFrame writer with color support
    /// </summary>
    public static class BoxFrameColor
    {
        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        public static void WriteBoxFramePlain(int Left, int Top, int InteriorWidth, int InteriorHeight) =>
            WriteBoxFramePlain(Left, Top, InteriorWidth, InteriorHeight,
                             BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                             BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                             BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                             BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        public static void WriteBoxFramePlain(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                            string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                            string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar)
        {
            try 
            {
                // Draw the box frame
                TextWriterWhereColor.WriteWhere(UpperLeftCornerChar + UpperFrameChar.Repeat(InteriorWidth) + UpperRightCornerChar, Left, Top, true);
                for (int i = 1; i <= InteriorHeight; i++)
                {
                    TextWriterWhereColor.WriteWhere(LeftFrameChar, Left, Top + i, true);
                    TextWriterWhereColor.WriteWhere(RightFrameChar, Left + InteriorWidth + 1, Top + i, true);
                }
                TextWriterWhereColor.WriteWhere(LowerLeftCornerChar + LowerFrameChar.Repeat(InteriorWidth) + LowerRightCornerChar, Left, Top + InteriorHeight + 1, true);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
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
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        KernelColorType.Separator, KernelColorType.Background);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, KernelColorType BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BoxFrameColor, KernelColorType.Background);

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
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BoxFrameColor, BackgroundColor);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, ConsoleColors BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(Convert.ToInt32(BoxFrameColor)), GetColor(KernelColorType.Background));

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Nitrocid KS's <see cref="Color"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, ConsoleColors BoxFrameColor, ConsoleColors BackgroundColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        new Color(Convert.ToInt32(BoxFrameColor)), new Color(Convert.ToInt32(BackgroundColor)));

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BoxFrameColor, GetColor(KernelColorType.Background));

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="Color"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Nitrocid KS's <see cref="Color"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, Color BoxFrameColor, Color BackgroundColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight,
                        BorderTools.BorderUpperLeftCornerChar, BorderTools.BorderLowerLeftCornerChar,
                        BorderTools.BorderUpperRightCornerChar, BorderTools.BorderLowerRightCornerChar,
                        BorderTools.BorderUpperFrameChar, BorderTools.BorderLowerFrameChar,
                        BorderTools.BorderLeftFrameChar, BorderTools.BorderRightFrameChar,
                        BoxFrameColor, BackgroundColor);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                       string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                       string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, KernelColorType.Separator, KernelColorType.Background);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                       string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                       string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar, 
                                       KernelColorType BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BoxFrameColor, KernelColorType.Background);

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        /// <param name="BoxFrameColor">BoxFrame color from Nitrocid KS's <see cref="KernelColorType"/></param>
        /// <param name="BackgroundColor">BoxFrame background color from Nitrocid KS's <see cref="KernelColorType"/></param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                       string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                       string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar, 
                                       KernelColorType BoxFrameColor, KernelColorType BackgroundColor)
        {
            try
            {
                SetConsoleColor(BoxFrameColor, false);
                SetConsoleColor(BackgroundColor, true, true);
                WriteBoxFramePlain(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
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
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                       string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                       string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar, 
                                       Color BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, BoxFrameColor, GetColor(KernelColorType.Background));

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                       string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                       string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar, 
                                       Color BoxFrameColor, Color BackgroundColor)
        {
            try
            {
                SetConsoleColor(BoxFrameColor, false);
                SetConsoleColor(BackgroundColor, true, true);
                WriteBoxFramePlain(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
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
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                       string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                       string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar, 
                                       ConsoleColors BoxFrameColor) =>
            WriteBoxFrame(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar, new Color(Convert.ToInt32(BoxFrameColor)), GetColor(KernelColorType.Background));

        /// <summary>
        /// Writes the box frame plainly
        /// </summary>
        /// <param name="Left">Where to place the box frame horizontally? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="Top">Where to place the box frame vertically? Please note that this value comes from the upper left corner, which is an exterior position.</param>
        /// <param name="InteriorWidth">The width of the interior window, excluding the two console columns for left and right frames</param>
        /// <param name="InteriorHeight">The height of the interior window, excluding the two console columns for upper and lower frames</param>
        /// <param name="UpperLeftCornerChar">Upper left corner character for box frame</param>
        /// <param name="LowerLeftCornerChar">Lower left corner character for box frame</param>
        /// <param name="UpperRightCornerChar">Upper right corner character for box frame</param>
        /// <param name="LowerRightCornerChar">Lower right corner character for box frame</param>
        /// <param name="UpperFrameChar">Upper frame character for box frame</param>
        /// <param name="LowerFrameChar">Lower frame character for box frame</param>
        /// <param name="LeftFrameChar">Left frame character for box frame</param>
        /// <param name="RightFrameChar">Right frame character for box frame</param>
        /// <param name="BoxFrameColor">BoxFrame color</param>
        /// <param name="BackgroundColor">BoxFrame background color</param>
        public static void WriteBoxFrame(int Left, int Top, int InteriorWidth, int InteriorHeight, 
                                       string UpperLeftCornerChar, string LowerLeftCornerChar, string UpperRightCornerChar, string LowerRightCornerChar, 
                                       string UpperFrameChar, string LowerFrameChar, string LeftFrameChar, string RightFrameChar, 
                                       ConsoleColors BoxFrameColor, ConsoleColors BackgroundColor)
        {
            try
            {
                SetConsoleColor(new Color(Convert.ToInt32(BoxFrameColor)), false);
                SetConsoleColor(new Color(Convert.ToInt32(BackgroundColor)), true, true);
                WriteBoxFramePlain(Left, Top, InteriorWidth, InteriorHeight, UpperLeftCornerChar, LowerLeftCornerChar, UpperRightCornerChar, LowerRightCornerChar, UpperFrameChar, LowerFrameChar, LeftFrameChar, RightFrameChar);
            }
            catch (Exception ex) when (!(ex.GetType().Name == nameof(ThreadInterruptedException)))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }
    }
}

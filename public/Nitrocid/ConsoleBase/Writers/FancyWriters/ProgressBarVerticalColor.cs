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
using System.Text;
using System.Threading;
using Terminaux.Colors;
using Textify.Sequences.Builder.Types;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Writers.FancyWriters.Tools;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers.ConsoleWriters;

namespace Nitrocid.ConsoleBase.Writers.FancyWriters
{
    /// <summary>
    /// Vertical progress bar writer with color support
    /// </summary>
    public static class ProgressBarVerticalColor
    {

        /// <summary>
        /// Writes the vertical progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgressPlain(double Progress, int Left, int Top, bool DrawBorder = true) =>
            WriteVerticalProgressPlain(Progress, Left, Top, 2, 0, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="HeightOffset">Height offset</param>
        public static void WriteVerticalProgressPlain(double Progress, int Left, int Top, int HeightOffset, bool DrawBorder = true) =>
            WriteVerticalProgressPlain(Progress, Left, Top, HeightOffset, 0, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        public static void WriteVerticalProgressPlain(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, bool DrawBorder = true)
        {
            try
            {
                TextWriterColor.WritePlain(RenderVerticalProgressPlain(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, DrawBorder));
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
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, 2, 0, KernelColorTools.GetColor(KernelColorType.Progress), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int HeightOffset, bool DrawBorder = true) =>
             WriteVerticalProgress(Progress, Left, Top, HeightOffset, 0, KernelColorTools.GetColor(KernelColorType.Progress), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, KernelColorTools.GetColor(KernelColorType.Progress), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, ConsoleColors ProgressColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, 2, 0, new Color(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int HeightOffset, ConsoleColors ProgressColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, HeightOffset, 0, new Color(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, ConsoleColors ProgressColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, new Color(ProgressColor), ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, ConsoleColors ProgressColor, ConsoleColors FrameColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, 2, 0, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int HeightOffset, ConsoleColors ProgressColor, ConsoleColors FrameColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, HeightOffset, 0, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, ConsoleColors ProgressColor, ConsoleColors FrameColor, bool DrawBorder = true)
        {
            try
            {
                TextWriterColor.WritePlain(RenderVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, ProgressColor, FrameColor, DrawBorder));
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
        public static void WriteVerticalProgress(double Progress, int Left, int Top, Color ProgressColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, 2, 0, ProgressColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int HeightOffset, Color ProgressColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, HeightOffset, 0, ProgressColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color ProgressColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, ProgressColor, ColorTools.GetGray(), DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, Color ProgressColor, Color FrameColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, 2, 0, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="HeightOffset">Height offset</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int HeightOffset, Color ProgressColor, Color FrameColor, bool DrawBorder = true) =>
            WriteVerticalProgress(Progress, Left, Top, HeightOffset, 0, ProgressColor, FrameColor, DrawBorder);

        /// <summary>
        /// Writes the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        public static void WriteVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color ProgressColor, Color FrameColor, bool DrawBorder = true)
        {
            try
            {
                TextWriterColor.WritePlain(RenderVerticalProgress(Progress, Left, Top, TopHeightOffset, BottomHeightOffset, ProgressColor, FrameColor, DrawBorder));
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
        }

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        public static string RenderVerticalProgressPlain(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, bool DrawBorder = true)
        {
            try
            {
                // Get the final height offset
                int FinalHeightOffset = TopHeightOffset + BottomHeightOffset;

                // Check the progress value
                if (Progress > 100)
                    Progress = 100;
                if (Progress < 0)
                    Progress = 0;

                // Fill the progress
                int MaximumHeight = ConsoleWrapper.WindowHeight - FinalHeightOffset;
                int ProgressFilled = ConsoleExtensions.PercentRepeatTargeted((int)Math.Round(Progress), 100, MaximumHeight);

                // Draw the border
                StringBuilder borderBuilder = new();
                if (DrawBorder)
                {
                    borderBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + 1)}");
                    borderBuilder.Append($"{ProgressTools.ProgressUpperLeftCornerChar}{ProgressTools.ProgressUpperFrameChar}{ProgressTools.ProgressUpperRightCornerChar}");
                    for (int i = 0; i < ConsoleWrapper.WindowHeight - FinalHeightOffset; i++)
                    {
                        borderBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + i + 2)}");
                        borderBuilder.Append(ProgressTools.ProgressLeftFrameChar + " " + ProgressTools.ProgressRightFrameChar);
                    }
                    borderBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + MaximumHeight + 2)}");
                    borderBuilder.Append(ProgressTools.ProgressLowerLeftCornerChar.ToString() + ProgressTools.ProgressLowerFrameChar + ProgressTools.ProgressLowerRightCornerChar);
                }

                // Draw the progress bar
                for (int i = ProgressFilled; i < MaximumHeight; i++)
                {
                    borderBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + MaximumHeight - i + 1)}");
                    borderBuilder.Append(' ');
                }
                for (int i = 0; i < ProgressFilled; i++)
                {
                    borderBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + MaximumHeight - i + 1)}");
                    borderBuilder.Append('*');
                }

                // Render to the console
                return borderBuilder.ToString();
            }
            catch (Exception ex) when (ex.GetType().Name != nameof(ThreadInterruptedException))
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, Translate.DoTranslation("There is a serious error when printing text.") + " {0}", ex.Message);
            }
            return "";
        }

        /// <summary>
        /// Renders the progress bar
        /// </summary>
        /// <param name="Progress">The progress percentage</param>
        /// <param name="Left">The progress position from the upper left corner</param>
        /// <param name="Top">The progress position from the top</param>
        /// <param name="DrawBorder">Whether to draw the border or not</param>
        /// <param name="TopHeightOffset">Height offset from the top</param>
        /// <param name="BottomHeightOffset">Height offset from the bottom</param>
        /// <param name="ProgressColor">The progress bar color</param>
        /// <param name="FrameColor">The progress bar frame color</param>
        public static string RenderVerticalProgress(double Progress, int Left, int Top, int TopHeightOffset, int BottomHeightOffset, Color ProgressColor, Color FrameColor, bool DrawBorder = true)
        {
            try
            {
                // Get the final height offset
                int FinalHeightOffset = TopHeightOffset + BottomHeightOffset;

                // Check the progress value
                if (Progress > 100)
                    Progress = 100;
                if (Progress < 0)
                    Progress = 0;

                // Fill the progress
                int MaximumHeight = ConsoleWrapper.WindowHeight - FinalHeightOffset;
                int ProgressFilled = ConsoleExtensions.PercentRepeatTargeted((int)Math.Round(Progress), 100, MaximumHeight);

                // Draw the border
                StringBuilder borderBuilder = new();
                if (DrawBorder)
                {
                    borderBuilder.Append($"{FrameColor.VTSequenceForeground}");
                    borderBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + 1)}");
                    borderBuilder.Append($"{ProgressTools.ProgressUpperLeftCornerChar}{ProgressTools.ProgressUpperFrameChar}{ProgressTools.ProgressUpperRightCornerChar}");
                    for (int i = 0; i < ConsoleWrapper.WindowHeight - FinalHeightOffset; i++)
                    {
                        borderBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + i + 2)}");
                        borderBuilder.Append(ProgressTools.ProgressLeftFrameChar + " " + ProgressTools.ProgressRightFrameChar);
                    }
                    borderBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(Left + 1, Top + MaximumHeight + 2)}");
                    borderBuilder.Append(ProgressTools.ProgressLowerLeftCornerChar.ToString() + ProgressTools.ProgressLowerFrameChar + ProgressTools.ProgressLowerRightCornerChar);
                }

                // Draw the progress bar
                for (int i = ProgressFilled; i < MaximumHeight; i++)
                {
                    borderBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + MaximumHeight - i + 1)}");
                    borderBuilder.Append(' ');
                }
                borderBuilder.Append($"{ProgressColor.VTSequenceBackground}");
                for (int i = 0; i < ProgressFilled; i++)
                {
                    borderBuilder.Append($"{CsiSequences.GenerateCsiCursorPosition(Left + 2, Top + MaximumHeight - i + 1)}");
                    borderBuilder.Append(' ');
                }
                borderBuilder.Append(KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground);
                borderBuilder.Append(KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground);

                // Render to the console
                return borderBuilder.ToString();
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

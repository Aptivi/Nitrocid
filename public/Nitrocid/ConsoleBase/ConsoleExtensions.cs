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
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Threading;
using Textify.General;
using Textify.Sequences.Builder;
using Textify.Sequences.Tools;
using TerminauxExts = Terminaux.Base.ConsoleExtensions;

namespace Nitrocid.ConsoleBase
{
    /// <summary>
    /// Additional routines for the console
    /// </summary>
    public static class ConsoleExtensions
    {

        internal static bool UseAltBuffer = true;
        internal static bool HasSetAltBuffer;

        /// <summary>
        /// Does your console support true color?
        /// </summary>
        public static bool ConsoleSupportsTrueColor =>
            Config.MainConfig.ConsoleSupportsTrueColor;

        /// <summary>
        /// Whether the input history is enabled
        /// </summary>
        public static bool InputHistoryEnabled =>
            Config.MainConfig.InputHistoryEnabled;

        /// <summary>
        /// Enables the scroll bar in selection screens
        /// </summary>
        public static bool EnableScrollBarInSelection =>
            Config.MainConfig.EnableScrollBarInSelection;

        /// <summary>
        /// Opts in to the new color selector
        /// </summary>
        public static bool UseNewColorSelector =>
            Config.MainConfig.UseNewColorSelector;

        /// <summary>
        /// Wraps the list outputs
        /// </summary>
        public static bool WrapListOutputs =>
            Config.MainConfig.WrapListOutputs;

        /// <summary>
        /// Clears the console buffer, but keeps the current cursor position
        /// </summary>
        public static void ClearKeepPosition() =>
            TerminauxExts.ClearKeepPosition();

        /// <summary>
        /// Clears the line to the right
        /// </summary>
        public static string GetClearLineToRightSequence() =>
            TerminauxExts.GetClearLineToRightSequence();

        /// <summary>
        /// Clears the line to the right
        /// </summary>
        public static void ClearLineToRight() =>
            TerminauxExts.ClearLineToRight();

        /// <summary>
        /// Gets how many times to repeat the character to represent the appropriate percentage level for the specified number.
        /// </summary>
        /// <param name="CurrentNumber">The current number that is less than or equal to the maximum number.</param>
        /// <param name="MaximumNumber">The maximum number.</param>
        /// <param name="WidthOffset">The console window width offset. It's usually a multiple of 2.</param>
        /// <returns>How many times to repeat the character</returns>
        public static int PercentRepeat(int CurrentNumber, int MaximumNumber, int WidthOffset) =>
            TerminauxExts.PercentRepeat(CurrentNumber, MaximumNumber, WidthOffset);

        /// <summary>
        /// Filters the VT sequences that matches the regex
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <returns>The text that doesn't contain the VT sequences</returns>
        public static string FilterVTSequences(string Text) =>
            TerminauxExts.FilterVTSequences(Text);

        /// <summary>
        /// Get the filtered cursor positions (by filtered means filtered from the VT escape sequences that matches the regex in the routine)
        /// </summary>
        /// <param name="Text">The text that contains the VT sequences</param>
        /// <param name="line">Whether to simulate the new line at the end of text or not</param>
        /// <param name="Vars">Variables to be formatted in the text</param>
        public static (int, int) GetFilteredPositions(string Text, bool line, params object[] Vars) =>
            TerminauxExts.GetFilteredPositions(Text, line, Vars);

        /// <summary>
        /// Sets the console title
        /// </summary>
        /// <param name="Text">The text to be set</param>
        public static void SetTitle(string Text) =>
            TerminauxExts.SetTitle(Text);

        /// <summary>
        /// Resets the entire console
        /// </summary>
        public static void ResetAll() =>
            TerminauxExts.ResetAll();

        /// <summary>
        /// Resets the console colors without clearing screen
        /// </summary>
        /// <param name="useKernelColors">Whether to use the kernel colors or to use the default terminal colors</param>
        public static void ResetColors(bool useKernelColors = false)
        {
            ResetBackground(useKernelColors);
            ResetForeground(useKernelColors);
        }

        /// <summary>
        /// Resets the background console color without clearing screen
        /// </summary>
        /// <param name="useKernelColors">Whether to use the kernel colors or to use the default terminal colors</param>
        public static void ResetBackground(bool useKernelColors = false)
        {
            if (useKernelColors)
            {
                ConsoleWrapper.Write(
                    KernelColorTools.GetColor(KernelColorType.Background).VTSequenceBackground
                );
            }
            else
            {
                TerminauxExts.ResetBackground();
            }
        }

        /// <summary>
        /// Resets the foreground console color without clearing screen
        /// </summary>
        /// <param name="useKernelColors">Whether to use the kernel colors or to use the default terminal colors</param>
        public static void ResetForeground(bool useKernelColors = false)
        {
            if (useKernelColors)
            {
                ConsoleWrapper.Write(
                    KernelColorTools.GetColor(KernelColorType.NeutralText).VTSequenceForeground
                );
            }
            else
            {
                TerminauxExts.ResetForeground();
            }
        }

        internal static void PreviewMainBuffer()
        {
            if (KernelPlatform.IsOnWindows())
                return;
            if (!(HasSetAltBuffer && UseAltBuffer))
                return;

            // Show the main buffer
            ShowMainBuffer();

            // Sleep for five seconds
            ThreadManager.SleepNoBlock(5000);

            // Show the alternative buffer
            ShowAltBuffer();
        }

        internal static void ShowMainBuffer()
        {
            if (KernelPlatform.IsOnWindows())
                return;
            if (!UseAltBuffer)
                return;

            TextWriterColor.Write("\u001b[?1049l");
        }

        internal static void ShowAltBuffer()
        {
            if (KernelPlatform.IsOnWindows())
                return;
            if (!UseAltBuffer)
                return;

            TextWriterColor.Write("\u001b[?1049h");
            ConsoleWrapper.SetCursorPosition(0, 0);
            ConsoleWrapper.CursorVisible = false;
        }

        #region Windows-specific
        private const string winKernel = "kernel32.dll";

        [DllImport(winKernel, SetLastError = true)]
        private static extern bool SetConsoleMode(nint hConsoleHandle, int mode);

        [DllImport(winKernel, SetLastError = true)]
        private static extern bool GetConsoleMode(nint handle, out int mode);

        [DllImport(winKernel, SetLastError = true)]
        private static extern nint GetStdHandle(int handle);

        internal static bool InitializeSequences()
        {
            nint stdHandle = GetStdHandle(-11);
            int mode = CheckForConHostSequenceSupport();
            if (mode != 7)
                return SetConsoleMode(stdHandle, mode | 4);
            return true;
        }

        internal static int CheckForConHostSequenceSupport()
        {
            nint stdHandle = GetStdHandle(-11);
            GetConsoleMode(stdHandle, out int mode);
            return mode;
        }
        #endregion

    }
}

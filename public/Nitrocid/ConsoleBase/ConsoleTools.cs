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
using Terminaux.Base;

namespace Nitrocid.ConsoleBase
{
    /// <summary>
    /// Additional routines for the console
    /// </summary>
    public static class ConsoleTools
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
                KernelColorTools.SetConsoleColor(KernelColorType.Background, Background: true);
            else
                ConsoleExtensions.ResetBackground();
        }

        /// <summary>
        /// Resets the foreground console color without clearing screen
        /// </summary>
        /// <param name="useKernelColors">Whether to use the kernel colors or to use the default terminal colors</param>
        public static void ResetForeground(bool useKernelColors = false)
        {
            if (useKernelColors)
                KernelColorTools.SetConsoleColor(KernelColorType.NeutralText);
            else
                ConsoleExtensions.ResetForeground();
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

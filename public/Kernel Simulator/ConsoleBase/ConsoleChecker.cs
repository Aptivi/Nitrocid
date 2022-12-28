
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

using System;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Drivers;
using KS.Drivers.Console.Consoles;
using KS.ConsoleBase.Inputs;

namespace KS.ConsoleBase
{
    /// <summary>
    /// Console sanity checking module
    /// </summary>
    public static class ConsoleChecker
    {

        /// <summary>
        /// Checks the running console for sanity, like the incompatible consoles, insane console types, etc.
        /// <br></br>
        /// The severity of the checks can be describes in two categories:
        /// <br></br>
        /// <br></br>
        /// 1. Error: We'll throw <see cref="KernelException"/> with type <see cref="KernelExceptionType.InsaneConsoleDetected"/> when we
        ///           detect that the console is not conforming to the standards.
        /// <br></br>
        /// 2. Warning: We'll just issue a warning in yellow text using the plain 16-color writer when we detect that the console is not
        ///             supporting optional features, like 256-colors.
        /// </summary>
        public static void CheckConsole()
        {
            string TerminalType = KernelPlatform.GetTerminalType();

            // Check if the terminal type is "dumb".
            // Severity: Error
            // Explanation below:
            // ---
            // The "dumb" terminals usually are not useful for interactive applications, since they only provide the stdout and stderr streams without
            // support for console cursor manipulation, which Kernel Simulator heavily depends on. These terminals are used for streaming output to the
            // appropriate variable, like the frontend applications that rely on console applications and their outputs to do their job (for example,
            // Brasero, a disk burning program, uses wodim, xorriso, and such applications to do its very intent of burning a blank CD-ROM. All these
            // backend applications are console programs).
            if (BaseConsoleDriver.IsDumb)
            {
                throw new KernelException(KernelExceptionType.InsaneConsoleDetected,
                                          "Kernel Simulator makes use of inputs and cursor manipulation, but the \"dumb\" terminals have no support for such tasks." + CharManager.NewLine + 
                                          "Possible solution: Use an appropriate terminal emulator or consult your terminal settings to set the terminal type into something other than \"dumb\"." + CharManager.NewLine +
                                          "                   We recommend using the \"vt100\" terminal emulators to get the most out of Kernel Simulator.");
            }

            // Check if the terminal supports 256 colors
            // Severity: Warning
            // Explanation below
            // ---
            // Kernel Simulator makes use of the 256 colors to print its own text by default. Even if we specify the 16-color compatibility values, we
            // still use the VT sequence to print colored text, but this will be changed later.
            if (!IsConsole256Colors())
            {
                ConsoleWrapper.ForegroundColor = ConsoleColor.Yellow;
                DriverHandler.CurrentConsoleDriver.WritePlain("Warning: Kernel Simulator makes use of the 256 colors. Make sure that your terminal is set to run on 256 color mode. Your terminal is {0}. Press any key to continue.", true, TerminalType);
                Input.DetectKeypress();
            }
        }

        /// <summary>
        /// Does the console support 256 colors? Always true on Windows
        /// </summary>
        public static bool IsConsole256Colors()
        {
            string TerminalType = KernelPlatform.GetTerminalType();
            return TerminalType.Contains("-256col") || KernelPlatform.IsOnWindows();
        }

        /// <summary>
        /// Checks the console size with edge cases
        /// </summary>
        internal static void CheckConsoleSize()
        {
            // If we're being run on TMUX, the status bar might mess up our interpretation of the window height.
            int MinimumWidth =  80;
            int MinimumHeight = 24;
            if (KernelPlatform.IsRunningFromTmux())
                // Assume that status bar is 1 row long
                MinimumHeight -= 1;

            // Check for the minimum console window requirements (80x24)
            while (ConsoleWrapper.WindowWidth < MinimumWidth | ConsoleWrapper.WindowHeight < MinimumHeight)
            {
                TextWriterColor.Write(Translate.DoTranslation("Your console is too small to run properly:") + " {0}x{1} | buff: {2}x{3}", true, KernelColorType.Warning, ConsoleWrapper.WindowWidth, ConsoleWrapper.WindowHeight, ConsoleWrapper.BufferWidth, ConsoleWrapper.BufferHeight);
                TextWriterColor.Write(Translate.DoTranslation("To have a better experience, resize your console window while still being on this screen. Press any key to continue..."), true, KernelColorType.Warning);
                Input.DetectKeypress();
            }
        }

    }
}

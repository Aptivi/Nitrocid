
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
using KS.Kernel.Exceptions;
using KS.Misc.Platform;
using KS.Misc.Writers.ConsoleWriters;

namespace KS.ConsoleBase
{
    public static class ConsoleSanityChecker
    {

        /// <summary>
        /// Checks the running console for sanity, like the incompatible consoles, insane console types, etc.
        /// <br></br>
        /// The severity of the checks can be describes in two categories:
        /// <br></br>
        /// <br></br>
        /// 1. Error: We'll throw <see cref="InsaneConsoleDetectedException"/> when we detect that the console is not conforming to the
        ///           standards.
        /// <br></br>
        /// 2. Warning: We'll just issue a warning in yellow text using the plain 16-color writer when we detect that the console is not
        ///             supporting optional features, like 256-colors.
        /// </summary>
        public static void CheckConsole()
        {
            string TerminalType = KernelPlatform.GetTerminalType();
            string TerminalEmulator = KernelPlatform.GetTerminalEmulator();

            // First: Check if the console is running on Apple_Terminal (terminal.app).
            // Severity: Error
            // Explanation below:
            // ---
            // This check is needed because we have the stock Terminal.app (Apple_Terminal according to $TERM_PROGRAM) that has incompatibilities with
            // VT sequences, causing broken display. It claims it supports XTerm, yet it isn't fully XTerm-compliant, so we exit the program early when
            // this stock terminal is spotted.
            // ---
            // More information regarding this check: The blacklisted terminals will not be able to run Kernel Simulator properly, because they have
            // broken support for colors and possibly more features. For example, we have Apple_Terminal that has no support for 255 and true colors;
            // it only supports 16 colors setting by VT sequences and nothing can change that, although it's fully XTerm compliant.
            if (KernelPlatform.IsOnMacOS())
            {
                if (TerminalEmulator == "Apple_Terminal")
                {
                    throw new InsaneConsoleDetectedException("Kernel Simulator makes use of VT escape sequences, but Terminal.app has broken support for 255 and true colors." + Kernel.Kernel.NewLine + "Possible solution: Download iTerm2 here: https://iterm2.com/downloads.html");
                }
            }

            // Second: Check if the terminal type is "dumb".
            // Severity: Error
            // Explanation below:
            // ---
            // The "dumb" terminals usually are not useful for interactive applications, since they only provide the stdout and stderr streams without
            // support for console cursor manipulation, which Kernel Simulator heavily depends on. These terminals are used for streaming output to the
            // appropriate variable, like the frontend applications that rely on console applications and their outputs to do their job (for example,
            // Brasero, a disk burning program, uses wodim, xorriso, and such applications to do its very intent of burning a blank CD-ROM. All these
            // backend applications are console programs).
            if (TerminalType == "dumb")
            {
                throw new InsaneConsoleDetectedException("Kernel Simulator makes use of inputs and cursor manipulation, but the \"dumb\" terminals have no support for such tasks." + Kernel.Kernel.NewLine + "Possible solution: Use an appropriate terminal emulator or consult your terminal settings to set the terminal type into something other than \"dumb\"." + Kernel.Kernel.NewLine + "                   We recommend using the \"vt100\" terminal emulators to get the most out of Kernel Simulator.");
            }

            // Third: Check if the terminal supports 256 colors
            // Severity: Warning
            // Explanation below
            // ---
            // Kernel Simulator makes use of the 256 colors to print its own text by default. Even if we specify the 16-color compatibility values, we
            // still use the VT sequence to print colored text, but this will be changed later.
            if (!TerminalType.Contains("-256col") & !KernelPlatform.IsOnWindows())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                TextWriterColor.WritePlain("Warning: Kernel Simulator makes use of the 256 colors. Make sure that your terminal is set to run on 256 color mode. Your terminal is {0}. Press any key to continue.", true, TerminalType);
                Console.ReadKey(true);
            }
        }

    }
}
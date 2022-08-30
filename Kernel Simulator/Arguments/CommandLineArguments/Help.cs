
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

using KS.Arguments.ArgumentBase;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using System;

namespace KS.Arguments.CommandLineArguments
{
    class CommandLine_HelpArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            // Command-line arguments
            TextWriterColor.Write(Translate.DoTranslation("Command-line arguments:"), true, ColorTools.ColTypes.ListTitle);
            ArgumentHelpSystem.ShowArgsHelp(ArgumentType.CommandLineArgs);
            TextWriterColor.Write();

            // Pre-boot command-line arguments
            TextWriterColor.Write(Translate.DoTranslation("Pre-boot command-line arguments:"), true, ColorTools.ColTypes.ListTitle);
            ArgumentHelpSystem.ShowArgsHelp(ArgumentType.PreBootCommandLineArgs);
            TextWriterColor.Write();

            // Either start the kernel or exit it
            TextWriterColor.Write(Translate.DoTranslation("* Press any key to start the kernel or ESC to exit."), true, ColorTools.ColTypes.Tip);
            if (ConsoleBase.ConsoleWrapper.ReadKey(true).Key == ConsoleKey.Escape)
            {
                // Clear the console and reset the colors
                ConsoleBase.ConsoleWrapper.ResetColor();
                ConsoleBase.ConsoleWrapper.Clear();
                Environment.Exit(0);
            }
        }

    }
}


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

using KS.Arguments.ArgumentBase;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.ConsoleBase.Inputs;
using System;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel;

namespace KS.Arguments.CommandLineArguments
{
    class HelpArgument : ArgumentExecutor, IArgument
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            // Kernel arguments
            TextWriterColor.Write(Translate.DoTranslation("Available kernel arguments:"), true, KernelColorType.ListTitle);
            ArgumentHelpSystem.ShowArgsHelp();
            TextWriterColor.Write();

            // Either start the kernel or exit it
            TextWriterColor.Write(Translate.DoTranslation("* Press any key to start the kernel or ESC to exit."), true, KernelColorType.Tip);
            if (Input.DetectKeypress().Key == ConsoleKey.Escape)
                Flags.KernelShutdown = true;
        }

    }
}

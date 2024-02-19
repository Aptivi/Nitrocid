﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System.Collections.Generic;
using System.Linq;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;

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

using KS.Scripting.Interaction;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class SelectCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            var Titles = new List<string>();

            // Add the provided working titles
            if (ListArgsOnly.Length > 3)
            {
                Titles.AddRange(ListArgsOnly.Skip(3));
            }

            // Prompt for selection
            UESHCommands.PromptSelectionAndSet(ListArgsOnly[2], ListArgsOnly[0], ListArgsOnly[1], [.. Titles]);
        }

        public override void HelpHelper()
        {
            TextWriterColor.Write(Translate.DoTranslation("where <$variable> is any variable that will be used to store response") + Kernel.Kernel.NewLine + Translate.DoTranslation("where <answers> are one-lettered answers of the question separated in slashes"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
        }

    }
}
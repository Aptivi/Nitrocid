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

using KS.Network;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Commands
{
    class ChHostNameCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (string.IsNullOrEmpty(ListArgs[0]))
            {
                TextWriterColor.Write(Translate.DoTranslation("Blank host name."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
            }
            else if (ListArgs[0].IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray()) != -1)
            {
                TextWriterColor.Write(Translate.DoTranslation("Special characters are not allowed."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Changing from: {0} to {1}..."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), Kernel.Kernel.HostName, ListArgs[0]);
                NetworkTools.ChangeHostname(ListArgs[0]);
            }
        }

    }
}
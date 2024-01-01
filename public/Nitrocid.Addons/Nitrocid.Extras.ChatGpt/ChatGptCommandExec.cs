﻿//
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace Nitrocid.Extras.ChatGpt
{
    internal class ChatGptCommandExec : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string apikey = parameters.ArgumentsList[0];
            TextWriters.Write("Warning: The ChatGPT shell isn't done yet, so chatting doesn't work.", KernelColorType.Warning);
            ShellManager.StartShell("ChatGptShell", apikey);
            return 0;
        }

    }
}
//
// Nitrocid KS  Copyright (C) 2018-2025  Aptivi
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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Extras.Docking.Dock;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Nitrocid.Users.Login.Widgets;

namespace Nitrocid.Extras.Docking.Commands
{
    class DockCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Check the dock screen for existence
            if (!DockTools.DoesDockScreenExist(parameters.ArgumentsList[0], out BaseWidget? dock))
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_DOCKING_NODOCKSCREEN1"), KernelColorType.Error);
                return 34;
            }

            // Now, dock the screen
            DockTools.DockScreen(dock);
            return 0;
        }

    }
}

//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Widgets;
using Nitrocid.Extras.Docking.Dock;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Extras.Docking.Commands
{
    class DockCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "dock";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_DOCKING_COMMAND_DOCK_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "dockName", new()
                    {
                        AutoCompleter = (_) => DockTools.GetDockScreenNames(),
                        ArgumentDescription = /* Localizable */ "NKS_DOCKING_COMMAND_DOCK_ARGUMENT_DOCKNAME_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            // Check the dock screen for existence
            string dockName = parameters.ArgumentsList[0];
            if (!DockTools.DoesDockScreenExist(dockName, out BaseWidget? dock))
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_DOCKING_NODOCKSCREEN2"), ThemeColorType.Error);
                return 34;
            }

            // Now, dock the screen
            DockTools.DockScreen(dock);
            return 0;
        }

    }
}

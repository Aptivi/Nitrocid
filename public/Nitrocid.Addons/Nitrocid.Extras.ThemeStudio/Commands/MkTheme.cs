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

using Nitrocid.Extras.ThemeStudio.Studio;
using Terminaux.Shell.Commands;

namespace Nitrocid.Extras.ThemeStudio.Commands
{
    /// <summary>
    /// Makes a new theme
    /// </summary>
    /// <remarks>
    /// This opens up a theme studio to manage the newly-created theme colors that you can adjust. This will allow you to create your own themes for Nitrocid KS.
    /// <br></br>
    /// If you want your theme to be included in the default Nitrocid KS themes, let us know.
    /// </remarks>
    class MkThemeCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool tui = parameters.ContainsSwitch("-tui");
            ThemeStudioApp.StartThemeStudio(parameters.ArgumentsList[0], tui);
            return 0;
        }
    }
}

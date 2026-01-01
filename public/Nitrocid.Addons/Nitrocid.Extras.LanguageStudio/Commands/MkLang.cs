//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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

using Nitrocid.Extras.LanguageStudio.Studio;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;

namespace Nitrocid.Extras.LanguageStudio.Commands
{
    /// <summary>
    /// Makes a new language
    /// </summary>
    /// <remarks>
    /// This opens up the language studio to let you provide translations to strings. This will allow you to create your own languages for Nitrocid KS.
    /// </remarks>
    class MkLangCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            bool tui = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-tui");
            LanguageStudioApp.StartLanguageStudio(parameters.ArgumentsList[0], tui);
            return 0;
        }

    }
}

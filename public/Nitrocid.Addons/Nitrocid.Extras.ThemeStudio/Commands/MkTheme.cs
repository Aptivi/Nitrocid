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

using Nitrocid.Languages;
using Nitrocid.Extras.ThemeStudio.Studio;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Shell.Switches;

namespace Nitrocid.Extras.ThemeStudio.Commands
{
    /// <summary>
    /// Makes a new theme
    /// </summary>
    /// <remarks>
    /// This opens up a theme studio to manage the newly-created theme colors that you can adjust. This will allow you to create your own themes for Nitrocid.
    /// <br></br>
    /// If you want your theme to be included in the default Nitrocid themes, let us know.
    /// </remarks>
    class MkThemeCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "mktheme";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_THEMESTUDIO_COMMAND_MKTHEME_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "themeName", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_THEMESTUDIO_COMMAND_MKTHEME_ARGUMENT_THEMENAME_DESC"
                    }),
                ],
                [
                    new SwitchInfo("tui", /* Localizable */ "NKS_THEMESTUDIO_COMMAND_MKTHEME_SWITCH_TUI_DESC")
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            bool tui = parameters.ContainsSwitch("-tui");
            ThemeStudioApp.StartThemeStudio(parameters.ArgumentsList[0], tui);
            return 0;
        }
    }
}

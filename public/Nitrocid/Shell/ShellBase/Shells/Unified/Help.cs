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

using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Help;
using Nitrocid.Shell.ShellBase.Switches;

namespace Nitrocid.Shell.ShellBase.Shells.Unified
{
    /// <summary>
    /// Opens the help page
    /// </summary>
    /// <remarks>
    /// This command allows you to get help for any specific command, including its usage. If no command is specified, all commands are listed.
    /// </remarks>
    class HelpUnifiedCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Determine which type to show
            bool useSimplified = SwitchManager.ContainsSwitch(parameters.SwitchesList, "-simplified");
            bool showGeneral = parameters.SwitchesList.Length == 0 ||
                SwitchManager.ContainsSwitch(parameters.SwitchesList, "-general") || SwitchManager.ContainsSwitch(parameters.SwitchesList, "-all");
            bool showMod = parameters.SwitchesList.Length > 0 &&
                (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-mod") || SwitchManager.ContainsSwitch(parameters.SwitchesList, "-all"));
            bool showAlias = parameters.SwitchesList.Length > 0 &&
                (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-alias") || SwitchManager.ContainsSwitch(parameters.SwitchesList, "-all"));
            bool showUnified = parameters.SwitchesList.Length > 0 &&
                (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-unified") || SwitchManager.ContainsSwitch(parameters.SwitchesList, "-all"));
            bool showAddon = parameters.SwitchesList.Length > 0 &&
                (SwitchManager.ContainsSwitch(parameters.SwitchesList, "-addon") || SwitchManager.ContainsSwitch(parameters.SwitchesList, "-all"));

            // Now, show the help
            if (string.IsNullOrWhiteSpace(parameters.ArgumentsText))
                HelpPrint.ShowHelpExtended(useSimplified, showGeneral, showMod, showAlias, showUnified, showAddon);
            else
                HelpPrint.ShowHelpExtended(parameters.ArgumentsList[0], useSimplified);
            return 0;
        }

    }
}


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

using KS.Shell.ShellBase.Switches;

namespace KS.Shell.ShellBase.Commands.UnifiedCommands
{
    /// <summary>
    /// Opens the help page
    /// </summary>
    /// <remarks>
    /// This command allows you to get help for any specific command, including its usage. If no command is specified, all commands are listed.
    /// </remarks>
    class HelpUnifiedCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string StringArgsOrig, string[] ListArgsOnlyOrig, string[] ListSwitchesOnly, ref string variableValue)
        {
            // Determine which type to show
            bool showGeneral = ListSwitchesOnly.Length == 0 ||
                SwitchManager.ContainsSwitch(ListSwitchesOnly, "-general") || SwitchManager.ContainsSwitch(ListSwitchesOnly, "-all");
            bool showMod = ListSwitchesOnly.Length > 0 &&
                (SwitchManager.ContainsSwitch(ListSwitchesOnly, "-mod") || SwitchManager.ContainsSwitch(ListSwitchesOnly, "-all"));
            bool showAlias = ListSwitchesOnly.Length > 0 &&
                (SwitchManager.ContainsSwitch(ListSwitchesOnly, "-alias") || SwitchManager.ContainsSwitch(ListSwitchesOnly, "-all"));
            bool showUnified = ListSwitchesOnly.Length > 0 &&
                (SwitchManager.ContainsSwitch(ListSwitchesOnly, "-unified") || SwitchManager.ContainsSwitch(ListSwitchesOnly, "-all"));
            bool showAddon = ListSwitchesOnly.Length > 0 &&
                (SwitchManager.ContainsSwitch(ListSwitchesOnly, "-addon") || SwitchManager.ContainsSwitch(ListSwitchesOnly, "-all"));
            
            // Now, show the help
            if (string.IsNullOrWhiteSpace(StringArgs))
                HelpSystem.ShowHelp(showGeneral, showMod, showAlias, showUnified, showAddon);
            else
                HelpSystem.ShowHelp(ListArgsOnly[0]);
            return 0;
        }

    }
}

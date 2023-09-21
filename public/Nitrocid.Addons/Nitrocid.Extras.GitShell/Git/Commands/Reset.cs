
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using LibGit2Sharp;
using GitCommand = LibGit2Sharp.Commands;
using System.Linq;
using KS.Shell.ShellBase.Switches;

namespace Nitrocid.Extras.GitShell.Git.Commands
{
    /// <summary>
    /// Resets the local repo
    /// </summary>
    /// <remarks>
    /// This command lets you reset your local Git repository to the most recent change.
    /// </remarks>
    class Git_ResetCommand : BaseCommand, ICommand
    {

        public override int Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly, ref string variableValue)
        {
            // Assume that we want to do a soft reset
            var resetMode = ResetMode.Soft;
            if (ListSwitchesOnly.Length > 0)
            {
                // Determine the reset mode by switch
                bool useSoft = SwitchManager.ContainsSwitch(ListSwitchesOnly, "-soft");
                bool useMixed = SwitchManager.ContainsSwitch(ListSwitchesOnly, "-mixed");
                bool useHard = SwitchManager.ContainsSwitch(ListSwitchesOnly, "-hard");
                if (useSoft)
                    resetMode = ResetMode.Soft;
                else if (useMixed)
                    resetMode = ResetMode.Mixed;
                else if (useHard)
                    resetMode = ResetMode.Hard;
            }

            // Now, reset.
            GitShellCommon.Repository.Reset(resetMode);
            return 0;
        }

    }
}

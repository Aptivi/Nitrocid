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

using LibGit2Sharp;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Switches;

namespace Nitrocid.ShellPacks.Shells.Git.Commands
{
    /// <summary>
    /// Resets the local repo
    /// </summary>
    /// <remarks>
    /// This command lets you reset your local Git repository to the most recent change.
    /// </remarks>
    class ResetCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            // Assume that we want to do a soft reset
            var resetMode = ResetMode.Soft;
            if (parameters.SwitchesList.Length > 0)
            {
                // Determine the reset mode by switch
                bool useSoft = parameters.ContainsSwitch("-soft");
                bool useMixed = parameters.ContainsSwitch("-mixed");
                bool useHard = parameters.ContainsSwitch("-hard");
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

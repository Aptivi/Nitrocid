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
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Extras.GitShell.Git.Commands
{
    /// <summary>
    /// Lists all branches
    /// </summary>
    /// <remarks>
    /// This command lets you list all branches in your Git repository.
    /// </remarks>
    class LsBranchesCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "lsbranches";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_LSBRANCHES_DESC");

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported | CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            if (GitShellCommon.Repository is null)
                return 43;
            var branches = GitShellCommon.Repository.Branches;
            foreach (var branch in branches)
            {
                TextWriterColor.Write($"- [{(branch.IsRemote ? "R" : " ")}-{(branch.IsTracking ? "T" : " ")}-{(branch.IsCurrentRepositoryHead ? "H" : " ")}] {branch.CanonicalName} [{branch.FriendlyName}]", true, ThemeColorType.ListEntry);
                TextWriterColor.Write($"  {branch.Tip.Sha[..7]}: {branch.Tip.MessageShort}", true, ThemeColorType.ListValue);
            }
            return 0;
        }

    }
}

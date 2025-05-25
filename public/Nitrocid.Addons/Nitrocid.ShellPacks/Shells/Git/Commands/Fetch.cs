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
using GitCommand = LibGit2Sharp.Commands;
using System.Linq;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Colors;

namespace Nitrocid.ShellPacks.Shells.Git.Commands
{
    /// <summary>
    /// Fetch updates
    /// </summary>
    /// <remarks>
    /// This command lets you fetch all the updates from the remote.
    /// </remarks>
    class FetchCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var status = GitShellCommon.Repository.RetrieveStatus();

            // Check to see if the repo has been modified
            if (status.IsDirty)
            {
                TextWriters.Write(Translate.DoTranslation("Save your work first by creating a commit before checking out a branch."), true, KernelColorType.Error);
                return 11;
            }

            // Check for existence if the remote is provided, or check for remotes and select the default one
            if (GitShellCommon.Repository is null)
                return 43;
            var remotes = GitShellCommon.Repository.Network.Remotes;
            var remoteNames = remotes.Select((remote) => remote.Name).ToArray();
            string selectedRemote = "origin";
            if (parameters.ArgumentsList.Length > 0)
            {
                string requestedRemote = parameters.ArgumentsList[0];
                if (!remoteNames.Contains(requestedRemote))
                {
                    TextWriters.Write(Translate.DoTranslation("Remote doesn't exist.") + $" {requestedRemote}", true, KernelColorType.Error);
                    return 12;
                }
            }
            else
            {
                // Check for the "origin" remote
                if (!remoteNames.Contains(selectedRemote))
                {
                    // We don't have origin! Let's select the first remote
                    if (remoteNames.Length == 0)
                    {
                        TextWriters.Write(Translate.DoTranslation("No remotes found to pull updates from."), true, KernelColorType.Error);
                        return 13;
                    }
                    selectedRemote = remoteNames[0];
                }
            }

            // Now, checkout the branch.
            var remoteRefSpecs = remotes[selectedRemote].FetchRefSpecs.Select((refspec) => refspec.Specification);
            var remoteFetchOptions = new FetchOptions()
            {
                Prune = true,
                TagFetchMode = TagFetchMode.All
            };
            GitCommand.Fetch(GitShellCommon.Repository, selectedRemote, remoteRefSpecs, remoteFetchOptions, $"GitShell is fetching from {selectedRemote}...");
            return 0;
        }

    }
}

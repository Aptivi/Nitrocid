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

using System;
using LibGit2Sharp;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using GitCommand = LibGit2Sharp.Commands;

namespace Nitrocid.ShellPacks.Shells.Git.Commands
{
    /// <summary>
    /// Stages all unstaged files
    /// </summary>
    /// <remarks>
    /// This command lets you stage all unstaged files in your Git repository.
    /// </remarks>
    class UnstageAllCommand : BaseCommand, ICommand
    {

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var gitShell = (GitShell?)shell ??
                throw new KernelException(KernelExceptionType.Git, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            var status = gitShell.Repository.RetrieveStatus();

            // Check to see if the repo has been modified
            if (!status.IsDirty)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_UNSTAGE_NOCHANGES"), true, ThemeColorType.Success);
                return 0;
            }

            // Stage all unstaged changes...
            var modified = status.Staged;
            foreach (var item in modified)
            {
                try
                {
                    GitCommand.Unstage(gitShell.Repository, item.FilePath);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_UNSTAGE_SUCCESS"), true, ThemeColorType.Success, item.FilePath);
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_UNSTAGE_FAILURE") + "{1}", true, ThemeColorType.Error, item.FilePath, ex.Message);
                }
            }
            return 0;
        }

    }
}

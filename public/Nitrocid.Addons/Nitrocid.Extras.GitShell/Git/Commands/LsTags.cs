﻿//
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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Terminaux.Shell.Commands;

namespace Nitrocid.Extras.GitShell.Git.Commands
{
    /// <summary>
    /// Lists all tags
    /// </summary>
    /// <remarks>
    /// This command lets you list all tags in your Git repository.
    /// </remarks>
    class LsTagsCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (GitShellCommon.Repository is null)
                return 43;
            var tags = GitShellCommon.Repository.Tags;
            foreach (var tag in tags)
            {
                TextWriters.Write($"- [{(tag.IsAnnotated ? "A" : " ")}] {tag.CanonicalName} [{tag.FriendlyName}]", true, KernelColorType.ListEntry);
                TextWriters.Write($"  {tag.Target.Sha}", true, KernelColorType.ListValue);
            }
            return 0;
        }

    }
}

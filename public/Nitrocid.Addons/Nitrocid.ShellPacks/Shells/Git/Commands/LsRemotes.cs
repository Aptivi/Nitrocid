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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;

namespace Nitrocid.ShellPacks.Shells.Git.Commands
{
    /// <summary>
    /// Lists all remotes
    /// </summary>
    /// <remarks>
    /// This command lets you list all remotes in your Git repository.
    /// </remarks>
    class LsRemotesCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (GitShellCommon.Repository is null)
                return 43;
            var remotes = GitShellCommon.Repository.Network.Remotes;
            foreach (var remote in remotes)
            {
                TextWriterColor.Write($"- {remote.Name}:", true, ThemeColorType.ListEntry);
                TextWriterColor.Write($"  - R: {remote.Url}", true, ThemeColorType.ListValue);
                TextWriterColor.Write($"  - P: {remote.PushUrl}", true, ThemeColorType.ListValue);
            }
            return 0;
        }

    }
}

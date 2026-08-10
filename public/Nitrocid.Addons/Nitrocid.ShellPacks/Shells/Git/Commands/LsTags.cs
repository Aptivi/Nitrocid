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

using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ShellPacks.Shells.Git.Commands
{
    /// <summary>
    /// Lists all tags
    /// </summary>
    /// <remarks>
    /// This command lets you list all tags in your Git repository.
    /// </remarks>
    class LsTagsCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "lstags";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_LSTAGS_DESC");

        public override CommandFlags Flags =>
            CommandFlags.RedirectionSupported | CommandFlags.Wrappable;

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            var gitShell = (GitShell?)shell ??
                throw new KernelException(KernelExceptionType.Git, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_LASTSHELLTYPEMISMATCH"));
            if (gitShell.Repository is null)
                return 43;
            var tags = gitShell.Repository.Tags;
            foreach (var tag in tags)
            {
                TextWriterColor.Write($"- [{(tag.IsAnnotated ? "A" : " ")}] {tag.CanonicalName} [{tag.FriendlyName}]", true, ThemeColorType.ListEntry);
                TextWriterColor.Write($"  {tag.Target.Sha}", true, ThemeColorType.ListValue);
            }
            return 0;
        }

    }
}

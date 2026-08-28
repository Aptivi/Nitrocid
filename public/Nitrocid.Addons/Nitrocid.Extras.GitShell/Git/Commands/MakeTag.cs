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

using LibGit2Sharp;
using Nitrocid.Kernel.Time;
using Nitrocid.Kernel.Time.Timezones;
using Nitrocid.Languages;
using Terminaux.Shell.Arguments;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.Extras.GitShell.Git.Commands
{
    /// <summary>
    /// Makes a tag
    /// </summary>
    /// <remarks>
    /// This command lets you make a tag.
    /// </remarks>
    class MakeTagCommand : BaseCommand, ICommand
    {
        public override string Command => 
            "maketag";

        public override string HelpDefinition => 
            LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_COMMAND_MAKETAG_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "tagname", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_GIT_COMMAND_MAKETAG_ARGUMENT_TAGNAME_DESC"
                    }),
                    new CommandArgumentPart(false, "message", new CommandArgumentPartOptions()
                    {
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_GIT_COMMAND_MAKETAG_ARGUMENT_MESSAGE_DESC"
                    }),
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            if (!GitShellCommon.isIdentified)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_NEEDSIDENTIFICATION_1") + " 'setid' " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_NEEDSIDENTIFICATION_2"), true, ThemeColorType.Error);
                return 15;
            }

            string tagName = parameters.ArgumentsList[0];
            string message = parameters.ArgumentsList.Length > 1 ? parameters.ArgumentsList[1] : "";
            var author = new Signature(GitShellCommon.name, GitShellCommon.email, new(TimeDateTools.KernelDateTime, TimeZoneRenderers.ShowTimeZoneUtcOffsetLocal()));
            var tag = GitShellCommon.Repository.ApplyTag(tagName, author, message);
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_MAKETAG_CREATED"));
            TextWriterColor.Write($"- [{(tag.IsAnnotated ? "A" : " ")}] {tag.CanonicalName} [{tag.FriendlyName}]", true, ThemeColorType.ListEntry);
            TextWriterColor.Write($"  {tag.Target.Sha}", true, ThemeColorType.ListValue);
            return 0;
        }

    }
}

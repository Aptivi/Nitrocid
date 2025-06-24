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
using System.IO;
using System.Linq;
using Terminaux.Shell.Commands;
using Nitrocid.Base.Languages;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Colors.Themes.Colors;

namespace Nitrocid.ShellPacks.Shells.Git.Commands
{
    /// <summary>
    /// Lists changes line by line
    /// </summary>
    /// <remarks>
    /// This command prints a list of changes line by line and who made it and in which commit.
    /// </remarks>
    class BlameCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            string file = parameters.ArgumentsList[0];
            int start = 0;
            int end = 0;
            if (parameters.ArgumentsList.Length > 1)
                start = int.Parse(parameters.ArgumentsList[1]);
            if (parameters.ArgumentsList.Length > 2)
                end = int.Parse(parameters.ArgumentsList[2]);

            // Get the list of blame hunks
            if (GitShellCommon.Repository is null)
                return 43;
            int hunkNum = 1;
            var blameHunks = GitShellCommon.Repository.Blame(file, new BlameOptions()
            {
                MinLine = start,
                MaxLine = end,
            });
            SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_TITLE") + $" {Path.GetFileName(file)}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
            foreach (var hunk in blameHunks)
            {
                int lines = hunk.LineCount;
                int initialStart = hunk.InitialStartLineNumber;
                int finalStart = hunk.FinalStartLineNumber;
                var initialCommit = hunk.InitialCommit;
                var finalCommit = hunk.FinalCommit;

                // Display some info about the blame hunk
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_HUNKNUM") + $" {hunkNum}/{blameHunks.Count()}", ThemeColorsTools.GetColor(ThemeColorType.ListTitle));
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_LINENUM") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{lines}", true, ThemeColorType.ListValue);
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_INITIALLINE") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{initialStart}", true, ThemeColorType.ListValue);
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_FINALLINE") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{finalStart}", true, ThemeColorType.ListValue);
                TextWriterRaw.Write();

                // Initial commit info
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_INITIALCOMMIT_TITLE"), true, ThemeColorType.ListEntry);
                TextWriterColor.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_SHA") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{initialCommit.Sha}", true, ThemeColorType.ListValue);
                TextWriterColor.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_MESSAGE") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{initialCommit.MessageShort}", true, ThemeColorType.ListValue);
                TextWriterColor.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_AUTHOR") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{initialCommit.Author.Name} <{initialCommit.Author.Email}>", true, ThemeColorType.ListValue);
                TextWriterColor.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_COMMITTER") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{initialCommit.Committer.Name} <{initialCommit.Committer.Email}>", true, ThemeColorType.ListValue);
                TextWriterColor.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_PARENTNUM") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{initialCommit.Parents.Count()}", true, ThemeColorType.ListValue);
                TextWriterRaw.Write();

                // Final commit info
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_FINALCOMMIT_TITLE"), true, ThemeColorType.ListEntry);
                TextWriterColor.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_SHA") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{finalCommit.Sha}", true, ThemeColorType.ListValue);
                TextWriterColor.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_MESSAGE") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{finalCommit.MessageShort}", true, ThemeColorType.ListValue);
                TextWriterColor.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_AUTHOR") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{finalCommit.Author.Name} <{finalCommit.Author.Email}>", true, ThemeColorType.ListValue);
                TextWriterColor.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_COMMITTER") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{finalCommit.Committer.Name} <{finalCommit.Committer.Email}>", true, ThemeColorType.ListValue);
                TextWriterColor.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_PARENTNUM") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{finalCommit.Parents.Count()}", true, ThemeColorType.ListValue);
                TextWriterRaw.Write();

                // Increment the hunk number for display
                hunkNum++;
            }
            return 0;
        }

    }
}

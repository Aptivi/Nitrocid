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
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.ConsoleBase.Colors;
using Terminaux.Writer.ConsoleWriters;

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
            SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_TITLE") + $" {Path.GetFileName(file)}", KernelColorTools.GetColor(KernelColorType.ListTitle));
            foreach (var hunk in blameHunks)
            {
                int lines = hunk.LineCount;
                int initialStart = hunk.InitialStartLineNumber;
                int finalStart = hunk.FinalStartLineNumber;
                var initialCommit = hunk.InitialCommit;
                var finalCommit = hunk.FinalCommit;

                // Display some info about the blame hunk
                SeparatorWriterColor.WriteSeparatorColor(LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_HUNKNUM") + $" {hunkNum}/{blameHunks.Count()}", KernelColorTools.GetColor(KernelColorType.ListTitle));
                TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_LINENUM") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{lines}", true, KernelColorType.ListValue);
                TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_INITIALLINE") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{initialStart}", true, KernelColorType.ListValue);
                TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_FINALLINE") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{finalStart}", true, KernelColorType.ListValue);
                TextWriterRaw.Write();

                // Initial commit info
                TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_INITIALCOMMIT_TITLE"), true, KernelColorType.ListEntry);
                TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_SHA") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{initialCommit.Sha}", true, KernelColorType.ListValue);
                TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_MESSAGE") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{initialCommit.MessageShort}", true, KernelColorType.ListValue);
                TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_AUTHOR") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{initialCommit.Author.Name} <{initialCommit.Author.Email}>", true, KernelColorType.ListValue);
                TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_COMMITTER") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{initialCommit.Committer.Name} <{initialCommit.Committer.Email}>", true, KernelColorType.ListValue);
                TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_PARENTNUM") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{initialCommit.Parents.Count()}", true, KernelColorType.ListValue);
                TextWriterRaw.Write();

                // Final commit info
                TextWriters.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_FINALCOMMIT_TITLE"), true, KernelColorType.ListEntry);
                TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_SHA") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{finalCommit.Sha}", true, KernelColorType.ListValue);
                TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_MESSAGE") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{finalCommit.MessageShort}", true, KernelColorType.ListValue);
                TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_AUTHOR") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{finalCommit.Author.Name} <{finalCommit.Author.Email}>", true, KernelColorType.ListValue);
                TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_COMMITTER") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{finalCommit.Committer.Name} <{finalCommit.Committer.Email}>", true, KernelColorType.ListValue);
                TextWriters.Write("  - " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_BLAMESTATUS_COMMIT_PARENTNUM") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{finalCommit.Parents.Count()}", true, KernelColorType.ListValue);
                TextWriterRaw.Write();

                // Increment the hunk number for display
                hunkNum++;
            }
            return 0;
        }

    }
}

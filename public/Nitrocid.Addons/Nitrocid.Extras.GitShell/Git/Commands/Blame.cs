//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
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

using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.ConsoleBase.Writers.FancyWriters;
using KS.Languages;
using KS.Shell.ShellBase.Commands;
using LibGit2Sharp;
using System.IO;
using System.Linq;

namespace Nitrocid.Extras.GitShell.Git.Commands
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
            int hunkNum = 1;
            var blameHunks = GitShellCommon.Repository.Blame(file, new BlameOptions()
            {
                MinLine = start,
                MaxLine = end,
            });
            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Blame status for file") + $" {Path.GetFileName(file)}", true);
            foreach (var hunk in blameHunks)
            {
                int lines = hunk.LineCount;
                int initialStart = hunk.InitialStartLineNumber;
                int finalStart = hunk.FinalStartLineNumber;
                var initialCommit = hunk.InitialCommit;
                var finalCommit = hunk.FinalCommit;

                // Display some info about the blame hunk
                SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Hunk number") + $" {hunkNum}/{blameHunks.Count()}", true);
                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Number of lines") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{lines}", true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Initial line number") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{initialStart}", true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Final line number") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{finalStart}", true, KernelColorType.ListValue);
                TextWriterColor.Write();

                // Initial commit info
                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Initial commit"), true, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor("  - " + Translate.DoTranslation("Commit SHA") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{initialCommit.Sha}", true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor("  - " + Translate.DoTranslation("Commit message") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{initialCommit.MessageShort}", true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor("  - " + Translate.DoTranslation("Commit author") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{initialCommit.Author.Name} <{initialCommit.Author.Email}>", true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor("  - " + Translate.DoTranslation("Commit committer") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{initialCommit.Committer.Name} <{initialCommit.Committer.Email}>", true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor("  - " + Translate.DoTranslation("Number of parents") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{initialCommit.Parents.Count()}", true, KernelColorType.ListValue);
                TextWriterColor.Write();

                // Final commit info
                TextWriterColor.WriteKernelColor("- " + Translate.DoTranslation("Final commit"), true, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor("  - " + Translate.DoTranslation("Commit SHA") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{finalCommit.Sha}", true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor("  - " + Translate.DoTranslation("Commit message") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{finalCommit.MessageShort}", true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor("  - " + Translate.DoTranslation("Commit author") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{finalCommit.Author.Name} <{finalCommit.Author.Email}>", true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor("  - " + Translate.DoTranslation("Commit committer") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{finalCommit.Committer.Name} <{finalCommit.Committer.Email}>", true, KernelColorType.ListValue);
                TextWriterColor.WriteKernelColor("  - " + Translate.DoTranslation("Number of parents") + ": ", false, KernelColorType.ListEntry);
                TextWriterColor.WriteKernelColor($"{finalCommit.Parents.Count()}", true, KernelColorType.ListValue);
                TextWriterColor.Write();

                // Increment the hunk number for display
                hunkNum++;
            }
            return 0;
        }

    }
}

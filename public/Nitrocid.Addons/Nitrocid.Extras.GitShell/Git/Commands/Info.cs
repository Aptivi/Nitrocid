//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Extras.GitShell.Git.Commands
{
    /// <summary>
    /// Current repository information
    /// </summary>
    /// <remarks>
    /// This command lets you get information about the current Git repository.
    /// </remarks>
    class InfoCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (GitShellCommon.Repository is null)
                return 43;
            var info = GitShellCommon.Repository.Info;
            TextWriters.Write("- " + Translate.DoTranslation("Bare repository") + ": ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{info.IsBare}", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Head detached") + ": ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{info.IsHeadDetached}", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("No reference") + ": ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{info.IsHeadUnborn}", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Shallow clone") + ": ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{info.IsShallow}", true, KernelColorType.ListValue);
            TextWriters.Write("- " + Translate.DoTranslation("Path to .git") + ": ", false, KernelColorType.ListEntry);
            TextWriters.Write($"{info.Path}", true, KernelColorType.ListValue);
            if (!info.IsBare)
            {
                TextWriters.Write("- " + Translate.DoTranslation("Path to working directory") + ": ", false, KernelColorType.ListEntry);
                TextWriters.Write($"{info.WorkingDirectory}", true, KernelColorType.ListValue);
            }
            return 0;
        }

    }
}

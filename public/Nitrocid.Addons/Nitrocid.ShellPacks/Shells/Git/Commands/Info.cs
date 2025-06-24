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

using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Base.Languages;
using Terminaux.Shell.Commands;

namespace Nitrocid.ShellPacks.Shells.Git.Commands
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
            TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_INFO_BARE") + ": ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write($"{info.IsBare}", true, ThemeColorType.ListValue);
            TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_INFO_DETACHED") + ": ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write($"{info.IsHeadDetached}", true, ThemeColorType.ListValue);
            TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_INFO_NOREF") + ": ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write($"{info.IsHeadUnborn}", true, ThemeColorType.ListValue);
            TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_INFO_SHALLOW") + ": ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write($"{info.IsShallow}", true, ThemeColorType.ListValue);
            TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_INFO_PATHTODOTGIT") + ": ", false, ThemeColorType.ListEntry);
            TextWriterColor.Write($"{info.Path}", true, ThemeColorType.ListValue);
            if (!info.IsBare)
            {
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_GIT_INFO_PATHTOWORKINGDIR") + ": ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write($"{info.WorkingDirectory}", true, ThemeColorType.ListValue);
            }
            return 0;
        }

    }
}

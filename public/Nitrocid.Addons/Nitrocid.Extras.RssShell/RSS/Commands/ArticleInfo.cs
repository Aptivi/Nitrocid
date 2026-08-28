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
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Textify.General;
using Terminaux.Shell.Arguments;

namespace Nitrocid.Extras.RssShell.RSS.Commands
{
    /// <summary>
    /// Gets article information
    /// </summary>
    /// <remarks>
    /// If you want to know more about the article, you can use this command to get extensive information about the article. Some feeds provide extra arguments to the article to make getting information even more vital to those who need it.
    /// <br></br>
    /// It shows you the article title, the link, the description, and the extra arguments and their values, if available.
    /// </remarks>
    class ArticleInfoCommand : BaseCommand, ICommand
    {
        public override string Command =>
            "articleinfo";

        public override string HelpDefinition =>
            LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_COMMAND_ARTICLEINFO_DESC");

        public override CommandArgumentInfo[] CommandArgumentInfo =>
            [
                new CommandArgumentInfo(
                [
                    new CommandArgumentPart(true, "feednum", new CommandArgumentPartOptions()
                    {
                        IsNumeric = true,
                        ArgumentDescription = /* Localizable */ "NKS_SHELLPACKS_RSS_COMMAND_ARGUMENT_FEEDNUM_DESC"
                    })
                ])
            ];

        public override int Execute(IShell? shell, CommandParameters parameters, ref string variableValue)
        {
            int ArticleIndex = (int)Math.Round(Convert.ToDouble(parameters.ArgumentsList[0]) - 1d);
            if (ArticleIndex > RSSShellCommon.RSSFeedInstance?.FeedArticles.Length - 1)
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_ARTICLENUMOUTOFRANGE"), true, ThemeColorType.Error);
                DebugWriter.WriteDebug(DebugLevel.E, "Tried to access article number {0}, but count is {1}.", vars: [ArticleIndex, RSSShellCommon.RSSFeedInstance?.FeedArticles.Length - 1]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.RSSShell);
            }
            else
            {
                var Article = RSSShellCommon.RSSFeedInstance?.FeedArticles[ArticleIndex] ??
                    throw new KernelException(KernelExceptionType.RSSShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_ARTICLEINFO_NOARTICLE"));
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_ARTICLEINFO_TITLE") + " ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(Article.ArticleTitle, true, ThemeColorType.ListValue);
                TextWriterColor.Write("- " + LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_ARTICLEINFO_LINK") + " ", false, ThemeColorType.ListEntry);
                TextWriterColor.Write(Article.ArticleLink, true, ThemeColorType.ListValue);
                foreach (string Variable in Article.ArticleVariables.Keys)
                {
                    if (Variable != "title" &&
                        Variable != "link" &&
                        Variable != "summary" &&
                        Variable != "description" &&
                        Variable != "content")
                    {
                        TextWriterColor.Write("- {0}: ", false, ThemeColorType.ListEntry, Variable);
                        TextWriterColor.Write(Article.ArticleVariables[Variable].InnerText, true, ThemeColorType.ListValue);
                    }
                }
                TextWriterColor.Write(CharManager.NewLine + Article.ArticleDescription);
                return 0;
            }
        }

    }
}

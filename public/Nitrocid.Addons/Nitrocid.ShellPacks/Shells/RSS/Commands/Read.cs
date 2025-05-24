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

using System;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.ShellPacks.Shells.RSS;
using SpecProbe.Software.Platform;

namespace Nitrocid.ShellPacks.Shells.RSS.Commands
{
    /// <summary>
    /// Opens a web browser to an article
    /// </summary>
    /// <remarks>
    /// If you want to read more about the article, you can use this command to open your current web browser to the article link, if one is found.
    /// </remarks>
    class ReadCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            var articles = RSSShellCommon.RSSFeedInstance?.FeedArticles ?? [];
            int ArticleIndex = (int)Math.Round(Convert.ToDouble(parameters.ArgumentsList[0]) - 1d);
            int articleCount = articles.Length;
            var article = articles[ArticleIndex];
            if (ArticleIndex > articleCount - 1)
            {
                TextWriters.Write(Translate.DoTranslation("Article number couldn't be bigger than the available articles."), true, KernelColorType.Error);
                DebugWriter.WriteDebug(DebugLevel.E, "Tried to access article number {0}, but count is {1}.", vars: [ArticleIndex, articleCount - 1]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.RSSShell);
            }
            else if (!string.IsNullOrWhiteSpace(article.ArticleLink))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Opening web browser to {0}...", vars: [article.ArticleLink]);
                PlatformHelper.PlatformOpen(article.ArticleLink);
                return 0;
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("Article doesn't have a link!"), true, KernelColorType.Error);
                DebugWriter.WriteDebug(DebugLevel.E, "Tried to open a web browser to link of article number {0}, but it's empty. \"{1}\"", vars: [ArticleIndex, article.ArticleLink]);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.RSSShell);
            }
        }

    }
}

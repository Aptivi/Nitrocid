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

using Nettify.Rss.Instance;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Interactive;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Textify.General;
using Nitrocid.Network.Connections;
using SpecProbe.Software.Platform;

namespace Nitrocid.ShellPacks.Shells.RSS.Interactive
{
    /// <summary>
    /// RSS Reader TUI class
    /// </summary>
    public class RssReaderCli : BaseInteractiveTui<RSSArticle>, IInteractiveTui<RSSArticle>
    {
        internal NetworkConnection? rssConnection;

        private RSSFeed Feed
        {
            get
            {
                if (rssConnection is null || rssConnection.ConnectionInstance is not RSSFeed feed)
                    throw new KernelException(KernelExceptionType.RSSNetwork, Translate.DoTranslation("This connection contains an invalid RSS feed instance."));
                return feed;
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<RSSArticle> PrimaryDataSource =>
            Feed.FeedArticles;

        /// <inheritdoc/>
        public override string GetInfoFromItem(RSSArticle item)
        {
            // Get some info from the article
            RSSArticle selectedArticle = item;
            bool hasTitle = !string.IsNullOrEmpty(selectedArticle.ArticleTitle);
            bool hasDescription = !string.IsNullOrEmpty(selectedArticle.ArticleDescription);

            // Generate the rendered text
            string finalRenderedArticleTitle =
                hasTitle ?
                $"{selectedArticle.ArticleTitle}" :
                Translate.DoTranslation("Unknown article title") + $" -> {selectedArticle.ArticleLink}";
            string finalRenderedArticleBody =
                hasDescription ?
                selectedArticle.ArticleDescription :
                Translate.DoTranslation("Unfortunately, this article doesn't have any contents. You can still follow the article at") + $" {selectedArticle.ArticleLink}.";

            // Render them to the second pane
            return
                finalRenderedArticleTitle + CharManager.NewLine +
                new string('-', finalRenderedArticleTitle.Length) + CharManager.NewLine + CharManager.NewLine +
                finalRenderedArticleBody;
            ;
        }

        /// <inheritdoc/>
        public override string GetStatusFromItem(RSSArticle item)
        {
            RSSArticle article = item;
            return article.ArticleTitle;
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(RSSArticle item)
        {
            RSSArticle article = item;
            return article.ArticleTitle;
        }

        internal void ShowArticleInfo(RSSArticle? item)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            if (item is null)
                return;
            bool hasTitle = !string.IsNullOrEmpty(item.ArticleTitle);
            bool hasDescription = !string.IsNullOrEmpty(item.ArticleDescription);
            bool hasVars = item.ArticleVariables.Count > 0;

            string finalRenderedArticleTitle =
                hasTitle ?
                $"{item.ArticleTitle}" :
                Translate.DoTranslation("Unknown article title") + $" -> {item.ArticleLink}";
            string finalRenderedArticleBody =
                hasDescription ?
                item.ArticleDescription :
                Translate.DoTranslation("Unfortunately, this article doesn't have any contents. You can still follow the article at") + $" {item.ArticleLink}.";
            string finalRenderedArticleVars =
                hasVars ?
                $"  - {string.Join("\n  - ", item.ArticleVariables.Select((kvp) => $"{kvp.Key} [{kvp.Value.InnerText}]"))}" :
                Translate.DoTranslation("No revision.");
            finalInfoRendered.AppendLine(finalRenderedArticleTitle);
            finalInfoRendered.AppendLine(finalRenderedArticleBody);
            finalInfoRendered.AppendLine(finalRenderedArticleVars);

            // Now, render the info box
            InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
        }

        internal void OpenArticleLink(RSSArticle? item)
        {
            // Check to see if we have a link
            if (item is null)
                return;
            bool hasLink = !string.IsNullOrEmpty(item.ArticleLink);
            if (!hasLink)
            {
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("This article doesn't have a link."), Settings.InfoBoxSettings);
                return;
            }

            // Now, open the host browser
            try
            {
                PlatformHelper.PlatformOpen(item.ArticleLink);
            }
            catch (Exception e)
            {
                InfoBoxModalColor.WriteInfoBoxModal(Translate.DoTranslation("Can't open the host browser to the article link.") + $" {e.Message}", Settings.InfoBoxSettings);
            }
        }

        internal void RefreshFeed() =>
            Feed.Refresh();

    }
}

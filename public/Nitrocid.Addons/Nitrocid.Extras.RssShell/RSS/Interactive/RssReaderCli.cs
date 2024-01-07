//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using Nitrocid.Network.Base.Connections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Textify.General;

namespace Nitrocid.Extras.RssShell.RSS.Interactive
{
    /// <summary>
    /// RSS Reader TUI class
    /// </summary>
    public class RssReaderCli : BaseInteractiveTui, IInteractiveTui
    {
        internal static NetworkConnection rssConnection;

        private static RSSFeed Feed
        {
            get
            {
                if (rssConnection.ConnectionInstance is not RSSFeed feed)
                    throw new KernelException(KernelExceptionType.RSSNetwork, Translate.DoTranslation("This connection contains an invalid RSS feed instance."));
                return feed;
            }
        }

        /// <summary>
        /// Article viewer keybindings
        /// </summary>
        public override List<InteractiveTuiBinding> Bindings { get; set; } =
        [
            // Operations
            new InteractiveTuiBinding("Info", ConsoleKey.F1, (article, _) => ShowArticleInfo(article)),
            new InteractiveTuiBinding("Read More", ConsoleKey.F2, (article, _) => OpenArticleLink(article)),
            new InteractiveTuiBinding("Refresh", ConsoleKey.F3, (article, _) => RefreshFeed())
        ];

        /// <inheritdoc/>
        public override IEnumerable PrimaryDataSource =>
            Feed.FeedArticles;

        /// <inheritdoc/>
        public override string GetInfoFromItem(object item)
        {
            // Get some info from the article
            RSSArticle selectedArticle = (RSSArticle)item;
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
        public override void RenderStatus(object item)
        {
            RSSArticle article = (RSSArticle)item;
            InteractiveTuiStatus.Status = article.ArticleTitle;
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(object item)
        {
            RSSArticle article = (RSSArticle)item;
            return article.ArticleTitle;
        }

        private static void ShowArticleInfo(object item)
        {
            // Render the final information string
            var finalInfoRendered = new StringBuilder();
            RSSArticle article = (RSSArticle)item;
            bool hasTitle = !string.IsNullOrEmpty(article.ArticleTitle);
            bool hasDescription = !string.IsNullOrEmpty(article.ArticleDescription);
            bool hasVars = article.ArticleVariables.Count > 0;

            string finalRenderedArticleTitle =
                hasTitle ?
                $"{article.ArticleTitle}" :
                Translate.DoTranslation("Unknown article title") + $" -> {article.ArticleLink}";
            string finalRenderedArticleBody =
                hasDescription ?
                article.ArticleDescription :
                Translate.DoTranslation("Unfortunately, this article doesn't have any contents. You can still follow the article at") + $" {article.ArticleLink}.";
            string finalRenderedArticleVars =
                hasVars ?
                $"  - {string.Join("\n  - ", article.ArticleVariables.Select((kvp) => $"{kvp.Key} [{kvp.Value}]"))}" :
                Translate.DoTranslation("No revision.");
            finalInfoRendered.AppendLine(finalRenderedArticleTitle);
            finalInfoRendered.AppendLine(finalRenderedArticleBody);
            finalInfoRendered.AppendLine(finalRenderedArticleVars);
            finalInfoRendered.AppendLine("\n" + Translate.DoTranslation("Press any key to close this window."));

            // Now, render the info box
            InfoBoxColor.WriteInfoBoxColorBack(finalInfoRendered.ToString(), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
        }

        private static void OpenArticleLink(object item)
        {
            // Check to see if we have a link
            RSSArticle article = (RSSArticle)item;
            bool hasLink = !string.IsNullOrEmpty(article.ArticleLink);
            if (!hasLink)
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("This article doesn't have a link."), InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);

            // Now, open the host browser
            try
            {
                Process.Start(article.ArticleLink);
            }
            catch (Exception e)
            {
                InfoBoxColor.WriteInfoBoxColorBack(Translate.DoTranslation("Can't open the host browser to the article link.") + $" {e.Message}", InteractiveTuiStatus.BoxForegroundColor, InteractiveTuiStatus.BoxBackgroundColor);
            }
        }

        private static void RefreshFeed() =>
            Feed.Refresh();

    }
}

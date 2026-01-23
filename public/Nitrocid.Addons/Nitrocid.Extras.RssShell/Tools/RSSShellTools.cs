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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Nettify.Rss.Instance;
using Nitrocid.Extras.RssShell.RSS;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Notifications;

namespace Nitrocid.Extras.RssShell.Tools
{
    /// <summary>
    /// RSS tools module
    /// </summary>
    public static class RSSShellTools
    {
        /// <summary>
        /// Gets the first article
        /// </summary>
        /// <param name="url">RSS feed URL</param>
        /// <returns>A tuple that contains feed title and article title</returns>
        /// <exception cref="KernelException"></exception>
        public static (string feedTitle, string articleTitle) GetFirstArticle(string url)
        {
            try
            {
                var Feed = new RSSFeed(url, RSSFeedType.Infer);
                Feed.Refresh();
                if (Feed.FeedArticles.Length > 0)
                    return (Feed.FeedTitle, Feed.FeedArticles[0].ArticleTitle);
                if (!string.IsNullOrEmpty(Feed.FeedDescription))
                    return (Feed.FeedTitle, Feed.FeedDescription);
                return (Feed.FeedTitle, Translate.DoTranslation("No description"));
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news, throwing to the kernel: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.RSSNetwork, ex);
            }
        }

        /// <summary>
        /// Gets the articles
        /// </summary>
        /// <param name="url">RSS feed URL</param>
        /// <returns>A list of tuples that contain feed titles and article titles</returns>
        /// <exception cref="KernelException"></exception>
        public static (string feedTitle, string articleTitle)[] GetArticles(string url)
        {
            try
            {
                var Feed = new RSSFeed(url, RSSFeedType.Infer);
                Feed.Refresh();
                var articles = new List<(string feedTitle, string articleTitle)>();
                foreach (var article in Feed.FeedArticles)
                    articles.Add((Feed.FeedTitle, article.ArticleTitle));
                return [.. articles];
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news, throwing to the kernel: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.RSSNetwork, ex);
            }
        }

        /// <summary>
        /// Searches for articles
        /// </summary>
        /// <param name="phrase">Phrase to look for</param>
        /// <param name="searchTitle">Whether to search the title or not</param>
        /// <param name="searchDescription">Whether to search the description or not</param>
        /// <param name="caseSensitive">Case sensitivity</param>
        /// <returns>List of articles containing the phrase</returns>
        public static List<RSSArticle> SearchArticles(string phrase, bool searchTitle = true, bool searchDescription = false, bool caseSensitive = false)
        {
            var articles = RSSShellCommon.RSSFeedInstance?.FeedArticles ?? [];
            var foundArticles = new List<RSSArticle>();
            var feedArticles = articles;

            // If not searching title and description, assume that we're searching for title
            if (!searchTitle && !searchDescription)
                searchTitle = true;

            // Search through the entire article list
            foreach (var article in feedArticles)
            {
                bool titleFound = caseSensitive ? article.ArticleTitle.Contains(phrase) : article.ArticleTitle.ToLower().Contains(phrase);
                bool descriptionFound = caseSensitive ? article.ArticleDescription.Contains(phrase) : article.ArticleDescription.ToLower().Contains(phrase);

                if (searchTitle && titleFound)
                {
                    foundArticles.Add(article);
                    continue;
                }

                if (searchDescription && descriptionFound)
                {
                    foundArticles.Add(article);
                    continue;
                }
            }

            return foundArticles;
        }

        /// <summary>
        /// Refreshes the feeds
        /// </summary>
        internal static void RefreshFeeds()
        {
            try
            {
                var articles = RSSShellCommon.RSSFeedInstance?.FeedArticles ?? [];
                var OldFeedsList = new List<RSSArticle>(articles);
                List<RSSArticle> NewFeedsList;
                while (RSSShellCommon.RSSFeedInstance is not null)
                {
                    if (RSSShellCommon.RSSFeedInstance is not null)
                    {
                        // Refresh the feed
                        RSSShellCommon.RSSFeedInstance.Refresh();

                        // Check for new feeds
                        NewFeedsList = articles.Except(OldFeedsList).ToList();
                        string OldFeedTitle = OldFeedsList.Count == 0 ? "" : OldFeedsList[0].ArticleTitle;
                        if (NewFeedsList.Count > 0 && NewFeedsList[0].ArticleTitle != OldFeedTitle)
                        {
                            // Update the list
                            DebugWriter.WriteDebug(DebugLevel.W, "Feeds received! Recents count was {0}, Old count was {1}", vars: [articles.Length, OldFeedsList.Count]);
                            OldFeedsList = new List<RSSArticle>(articles);
                            foreach (RSSArticle NewFeed in NewFeedsList)
                            {
                                var FeedNotif = new Notification(NewFeed.ArticleTitle, NewFeed.ArticleDescription, NotificationPriority.Low, NotificationType.Normal);
                                NotificationManager.NotifySend(FeedNotif);
                            }
                        }
                    }
                    Thread.Sleep(RSSShellCommon.RSSRefreshInterval);
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Aborting refresher...");
            }
        }
    }
}

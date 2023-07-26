
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
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentFTP.Helpers;
using KS.ConsoleBase.Colors;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Notifications;
using Syndian.Instance;
using KS.Shell.Shells.RSS;
using KS.ConsoleBase.Writers.ConsoleWriters;

namespace KS.Network.RSS
{
    /// <summary>
    /// RSS tools module
    /// </summary>
    public static class RSSTools
    {
        /// <summary>
        /// Whether to show the RSS headline each login
        /// </summary>
        public static bool ShowHeadlineOnLogin =>
            Config.MainConfig.ShowHeadlineOnLogin;
        /// <summary>
        /// RSS headline URL
        /// </summary>
        public static string RssHeadlineUrl =>
            Config.MainConfig.RssHeadlineUrl;

        /// <summary>
        /// Refreshes the feeds
        /// </summary>
        internal static void RefreshFeeds()
        {
            try
            {
                var OldFeedsList = new List<RSSArticle>(RSSShellCommon.RSSFeedInstance.FeedArticles);
                List<RSSArticle> NewFeedsList;
                while (RSSShellCommon.RSSFeedInstance is not null)
                {
                    if (RSSShellCommon.RSSFeedInstance is not null)
                    {
                        // Refresh the feed
                        RSSShellCommon.RSSFeedInstance.Refresh();

                        // Check for new feeds
                        NewFeedsList = RSSShellCommon.RSSFeedInstance.FeedArticles.Except(OldFeedsList).ToList();
                        string OldFeedTitle = OldFeedsList.Count == 0 ? "" : OldFeedsList[0].ArticleTitle;
                        if (NewFeedsList.Count > 0 && NewFeedsList[0].ArticleTitle != OldFeedTitle)
                        {
                            // Update the list
                            DebugWriter.WriteDebug(DebugLevel.W, "Feeds received! Recents count was {0}, Old count was {1}", RSSShellCommon.RSSFeedInstance.FeedArticles.Count, OldFeedsList.Count);
                            OldFeedsList = new List<RSSArticle>(RSSShellCommon.RSSFeedInstance.FeedArticles);
                            foreach (RSSArticle NewFeed in NewFeedsList)
                            {
                                var FeedNotif = new Notification(NewFeed.ArticleTitle, NewFeed.ArticleDescription, NotificationManager.NotifPriority.Low, NotificationManager.NotifType.Normal);
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

        /// <summary>
        /// Show a headline on login
        /// </summary>
        public static void ShowHeadlineLogin()
        {
            if (ShowHeadlineOnLogin)
            {
                try
                {
                    var Feed = new RSSFeed(RssHeadlineUrl, RSSFeedType.Infer);
                    if (Feed.FeedArticles.Count > 0)
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Latest news from") + " {0}: ", false, KernelColorType.ListEntry, Feed.FeedTitle);
                        TextWriterColor.Write(Feed.FeedArticles[0].ArticleTitle, true, KernelColorType.ListValue);
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get latest news: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriterColor.Write(Translate.DoTranslation("Failed to get the latest news."), true, KernelColorType.Error);
                }
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
            var foundArticles = new List<RSSArticle>();
            var feedArticles = RSSShellCommon.RSSFeedInstance.FeedArticles;

            // If not searching title and description, assume that we're searching for title
            if (!searchTitle && !searchDescription)
                searchTitle = true;

            // Search through the entire article list
            foreach (var article in feedArticles)
            {
                bool titleFound = caseSensitive ? article.ArticleTitle.Contains(phrase) : article.ArticleTitle.ContainsCI(phrase);
                bool descriptionFound = caseSensitive ? article.ArticleDescription.Contains(phrase) : article.ArticleDescription.ContainsCI(phrase);

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

    }
}

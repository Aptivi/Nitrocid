
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
using System.Xml;
using FluentFTP.Helpers;
using HtmlAgilityPack;
using KS.ConsoleBase.Colors;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.RSS.Instance;
using KS.Shell.Shells.RSS;

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
        /// Make instances of RSS Article from feed node and type
        /// </summary>
        /// <param name="FeedNode">Feed XML node</param>
        /// <param name="FeedType">Feed type</param>
        public static List<RSSArticle> MakeRssArticlesFromFeed(XmlNodeList FeedNode, RSSFeedType FeedType)
        {
            var Articles = new List<RSSArticle>();
            switch (FeedType)
            {
                case RSSFeedType.RSS2:
                    {
                        foreach (XmlNode Node in FeedNode[0]) // <channel>
                        {
                            foreach (XmlNode Child in Node.ChildNodes) // <item>
                            {
                                if (Child.Name == "item")
                                {
                                    var Article = MakeArticleFromFeed(Child);
                                    Articles.Add(Article);
                                }
                            }
                        }

                        break;
                    }
                case RSSFeedType.RSS1:
                    {
                        foreach (XmlNode Node in FeedNode[0]) // <channel> or <item>
                        {
                            if (Node.Name == "item")
                            {
                                var Article = MakeArticleFromFeed(Node);
                                Articles.Add(Article);
                            }
                        }

                        break;
                    }
                case RSSFeedType.Atom:
                    {
                        foreach (XmlNode Node in FeedNode[0]) // <feed>
                        {
                            if (Node.Name == "entry")
                            {
                                var Article = MakeArticleFromFeed(Node);
                                Articles.Add(Article);
                            }
                        }

                        break;
                    }

                default:
                    {
                        throw new KernelException(KernelExceptionType.InvalidFeedType, Translate.DoTranslation("Invalid RSS feed type."));
                    }
            }
            return Articles;
        }

        /// <summary>
        /// Generates an instance of article from feed
        /// </summary>
        /// <param name="Article">The child node which holds the entire article</param>
        /// <returns>An article</returns>
        public static RSSArticle MakeArticleFromFeed(XmlNode Article)
        {
            // Variables
            var Parameters = new Dictionary<string, XmlNode>();
            string Title = default, Link = default, Description = default;

            // Parse article
            foreach (XmlNode ArticleNode in Article.ChildNodes)
            {
                // Check the title
                if (ArticleNode.Name == "title")
                {
                    // Trimming newlines and spaces is necessary, since some RSS feeds (GitHub commits) might return string with trailing and leading spaces and newlines.
                    Title = ArticleNode.InnerText.Trim(Convert.ToChar(Convert.ToChar(13)), Convert.ToChar(Convert.ToChar(10)), ' ');
                }

                // Check the link
                if (ArticleNode.Name == "link")
                {
                    // Links can be in href attribute, so check that.
                    if (ArticleNode.Attributes.Count != 0 & ArticleNode.Attributes.GetNamedItem("href") is not null)
                    {
                        Link = ArticleNode.Attributes.GetNamedItem("href").InnerText;
                    }
                    else
                    {
                        Link = ArticleNode.InnerText;
                    }
                }

                // Check the summary
                if (ArticleNode.Name == "summary" | ArticleNode.Name == "content" | ArticleNode.Name == "description")
                {
                    // It can be of HTML type, or plain text type.
                    if (ArticleNode.Attributes.Count != 0 & ArticleNode.Attributes.GetNamedItem("type") is not null)
                    {
                        if (ArticleNode.Attributes.GetNamedItem("type").Value == "html")
                        {
                            // Extract plain text from HTML
                            var HtmlContent = new HtmlDocument();
                            HtmlContent.LoadHtml(ArticleNode.InnerText.Trim(Convert.ToChar(Convert.ToChar(13)), Convert.ToChar(Convert.ToChar(10)), ' '));

                            // Some feeds have no node called "pre," so work around this...
                            var PreNode = HtmlContent.DocumentNode.SelectSingleNode("pre");
                            if (PreNode is null)
                            {
                                Description = HtmlContent.DocumentNode.InnerText;
                            }
                            else
                            {
                                Description = PreNode.InnerText;
                            }
                        }
                        else
                        {
                            Description = ArticleNode.InnerText.Trim(Convert.ToChar(Convert.ToChar(13)), Convert.ToChar(Convert.ToChar(10)), ' ');
                        }
                    }
                    else
                    {
                        Description = ArticleNode.InnerText.Trim(Convert.ToChar(Convert.ToChar(13)), Convert.ToChar(Convert.ToChar(10)), ' ');
                    }
                }
                if (!Parameters.ContainsKey(ArticleNode.Name))
                    Parameters.Add(ArticleNode.Name, ArticleNode);
            }
            return new RSSArticle(Title, Link, Description, Parameters);
        }

        /// <summary>
        /// Gets a feed property
        /// </summary>
        /// <param name="FeedProperty">Feed property name</param>
        /// <param name="FeedNode">Feed XML node</param>
        /// <param name="FeedType">Feed type</param>
        public static object GetFeedProperty(string FeedProperty, XmlNodeList FeedNode, RSSFeedType FeedType)
        {
            switch (FeedType)
            {
                case RSSFeedType.RSS2:
                    {
                        foreach (XmlNode Node in FeedNode[0]) // <channel>
                        {
                            foreach (XmlNode Child in Node.ChildNodes)
                            {
                                if (Child.Name == FeedProperty)
                                {
                                    return Child.InnerXml;
                                }
                            }
                        }

                        break;
                    }
                case RSSFeedType.RSS1:
                    {
                        foreach (XmlNode Node in FeedNode[0]) // <channel> or <item>
                        {
                            foreach (XmlNode Child in Node.ChildNodes)
                            {
                                if (Child.Name == FeedProperty)
                                {
                                    return Child.InnerXml;
                                }
                            }
                        }

                        break;
                    }
                case RSSFeedType.Atom:
                    {
                        foreach (XmlNode Node in FeedNode[0]) // Children of <feed>
                        {
                            if (Node.Name == FeedProperty)
                            {
                                return Node.InnerXml;
                            }
                        }

                        break;
                    }

                default:
                    {
                        throw new KernelException(KernelExceptionType.InvalidFeedType, Translate.DoTranslation("Invalid RSS feed type."));
                    }
            }
            return "";
        }

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

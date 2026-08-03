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
using System.Collections.Generic;
using Nettify.Rss.Instance;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.ShellPacks.Shells.RSS.Interactive;
using Terminaux.Inputs.Interactive;

namespace Nitrocid.ShellPacks.Shells.RSS.Tools
{
    /// <summary>
    /// RSS tools module
    /// </summary>
    public static class RSSTools
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
                return (Feed.FeedTitle, LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_FEEDNODESC"));
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
        /// <param name="feedArticles">Feed articles to search within the list</param>
        /// <param name="phrase">Phrase to look for</param>
        /// <param name="searchTitle">Whether to search the title or not</param>
        /// <param name="searchDescription">Whether to search the description or not</param>
        /// <param name="caseSensitive">Case sensitivity</param>
        /// <returns>List of articles containing the phrase</returns>
        public static List<RSSArticle> SearchArticles(RSSArticle[] feedArticles, string phrase, bool searchTitle = true, bool searchDescription = false, bool caseSensitive = false)
        {
            var foundArticles = new List<RSSArticle>();

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
        /// Opens the RSS feed interactive TUI
        /// </summary>
        /// <param name="feedAddress">RSS feed URL</param>
        /// <exception cref="KernelException"></exception>
        public static void OpenFeedTui(string feedAddress)
        {
            var feed = new RSSFeed(feedAddress, RSSFeedType.Infer);
            OpenFeedTui(feed);
        }

        /// <summary>
        /// Opens the RSS feed interactive TUI
        /// </summary>
        /// <param name="feedClass"></param>
        /// <exception cref="KernelException"></exception>
        public static void OpenFeedTui(RSSFeed? feedClass)
        {
            // Check the feed class
            if (feedClass is null)
                throw new KernelException(KernelExceptionType.RSSNetwork, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_1"));

            // Initialize the RSS reader TUI
            // TODO: NKS_SHELLPACKS_RSS_TUI_KEYBINDING_ADDFEED -> Adds a feed
            // TODO: NKS_SHELLPACKS_RSS_TUI_KEYBINDING_REFRESHALL -> Refresh all
            // TODO: NKS_SHELLPACKS_RSS_TUI_KEYBINDING_REFRESHFEED -> Refresh feed
            // TODO: NKS_SHELLPACKS_RSS_TUI_KEYBINDING_FILTERARTICLES -> Filter articles
            // TODO: NKS_SHELLPACKS_RSS_TUI_KEYBINDING_FILTERRESET -> Reset filter
            var tui = new RssReaderCli();
            tui.BindingsSecondPane.Add(new InteractiveTuiBinding<RSSFeed, RSSArticle>(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_KEYBINDING_INFO"), ConsoleKey.Enter, (_, _, article, _) => tui.ShowArticleInfo(article)));
            tui.BindingsSecondPane.Add(new InteractiveTuiBinding<RSSFeed, RSSArticle>(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_TUI_KEYBINDING_READMORE"), ConsoleKey.Enter, ConsoleModifiers.Shift, (_, _, article, _) => tui.OpenArticleLink(article)));
            tui.BindingsFirstPane.Add(new InteractiveTuiBinding<RSSFeed, RSSArticle>(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_TUI_KEYBINDING_REFRESH"), ConsoleKey.Enter, (feed, _, _, _) => tui.RefreshFeed(feed)));
            tui.BindingsFirstPane.Add(new InteractiveTuiBinding<RSSFeed, RSSArticle>(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_TUI_KEYBINDING_REFRESHFEED"), ConsoleKey.Enter, ConsoleModifiers.Shift, (feed, _, _, _) => tui.OpenFeedLink(feed)));
            tui.Bindings.Add(new InteractiveTuiBinding<RSSFeed, RSSArticle>(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_TUI_KEYBINDING_REFRESHALL"), ConsoleKey.F1, (_, _, _, _) => tui.RefreshAllFeeds()));
            tui.BindingsFirstPane.Add(new InteractiveTuiBinding<RSSFeed, RSSArticle>(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_TUI_KEYBINDING_ADDFEED"), ConsoleKey.F2, (_, _, _, _) => tui.AddFeedPrompt()));
            tui.BindingsSecondPane.Add(new InteractiveTuiBinding<RSSFeed, RSSArticle>(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_TUI_KEYBINDING_FILTERARTICLES"), ConsoleKey.F3, (_, _, _, _) => tui.FilterArticlesPrompt()));
            tui.BindingsSecondPane.Add(new InteractiveTuiBinding<RSSFeed, RSSArticle>(LanguageTools.GetLocalized("NKS_SHELLPACKS_RSS_TUI_KEYBINDING_FILTERRESET"), ConsoleKey.F3, (_, _, _, _) => tui.ResetFilter()));

            // Set the feed and run auto refresh
            tui.feeds.Add(feedClass);
            tui.timer = new((_) => tui.RefreshAllFeeds(), null, 0, ShellsInit.ShellsConfig.RSSRefreshInterval);
            tui.mre.WaitOne();

            // Open the TUI
            InteractiveTuiTools.OpenInteractiveTui(tui);
            tui.timer.Dispose();
        }
    }
}

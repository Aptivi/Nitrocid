﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Xml;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using HtmlAgilityPack;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Languages;
using KS.Misc.Notifiers;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Network.RSS.Instance;
using KS.Network.Transfer;
using Newtonsoft.Json.Linq;
using Terminaux.Base;

namespace KS.Network.RSS
{
    public static class RSSTools
    {

        /// <summary>
        /// Whether to show the RSS headline each login
        /// </summary>
        public static bool ShowHeadlineOnLogin;
        /// <summary>
        /// RSS headline URL
        /// </summary>
        public static string RssHeadlineUrl = "https://www.techrepublic.com/rssfeeds/articles/";
        /// <summary>
        /// Cached feed list JSON
        /// </summary>
        private static string FeedListJsonText = "";

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
                        throw new Kernel.Exceptions.InvalidFeedTypeException(Translate.DoTranslation("Invalid RSS feed type."));
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
                    Title = ArticleNode.InnerText.Trim(Convert.ToChar("\r"), Convert.ToChar("\n"), ' ');
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
                            HtmlContent.LoadHtml(ArticleNode.InnerText.Trim(Convert.ToChar("\r"), Convert.ToChar("\n"), ' '));

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
                            Description = ArticleNode.InnerText.Trim(Convert.ToChar("\r"), Convert.ToChar("\n"), ' ');
                        }
                    }
                    else
                    {
                        Description = ArticleNode.InnerText.Trim(Convert.ToChar("\r"), Convert.ToChar("\n"), ' ');
                    }
                }
                if (Parameters.ContainsKey(ArticleNode.Name))
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
        public static string GetFeedProperty(string FeedProperty, XmlNodeList FeedNode, RSSFeedType FeedType)
        {
            switch (FeedType)
            {
                case RSSFeedType.RSS2:
                    {
                        foreach (XmlNode Node in FeedNode[0]) // <channel>
                        {
                            foreach (XmlNode Child in Node.ChildNodes)
                            {
                                if ((Child.Name ?? "") == (FeedProperty ?? ""))
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
                                if ((Child.Name ?? "") == (FeedProperty ?? ""))
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
                            if ((Node.Name ?? "") == (FeedProperty ?? ""))
                            {
                                return Node.InnerXml;
                            }
                        }

                        break;
                    }

                default:
                    {
                        throw new Kernel.Exceptions.InvalidFeedTypeException(Translate.DoTranslation("Invalid RSS feed type."));
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
                        if (NewFeedsList.Count > 0 && (NewFeedsList[0].ArticleTitle ?? "") != (OldFeedTitle ?? ""))
                        {
                            // Update the list
                            DebugWriter.Wdbg(DebugLevel.W, "Feeds received! Recents count was {0}, Old count was {1}", RSSShellCommon.RSSFeedInstance.FeedArticles.Count, OldFeedsList.Count);
                            OldFeedsList = new List<RSSArticle>(RSSShellCommon.RSSFeedInstance.FeedArticles);
                            foreach (RSSArticle NewFeed in NewFeedsList)
                            {
                                var FeedNotif = new Notification(NewFeed.ArticleTitle, NewFeed.ArticleDescription, Notifications.NotifPriority.Low, Notifications.NotifType.Normal);
                                Notifications.NotifySend(FeedNotif);
                            }
                        }
                    }
                    Thread.Sleep(RSSShellCommon.RSSRefreshInterval);
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.Wdbg(DebugLevel.W, "Aborting refresher...");
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
                    if (!(Feed.FeedArticles.Count == 0))
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Latest news:") + " ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListEntry));
                        TextWriterColor.Write(Feed.FeedArticles[0].ArticleTitle, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.ListValue));
                    }
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Failed to get latest news: {0}", ex.Message);
                    DebugWriter.WStkTrc(ex);
                    TextWriterColor.Write(Translate.DoTranslation("Failed to get the latest news."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                }
            }
        }

        /// <summary>
        /// Opens the feed selector
        /// </summary>
        public static void OpenFeedSelector()
        {
            int StepNumber = 1;
            JToken FeedListJson;
            JToken[] FeedListJsonCountries = [];
            JToken[] FeedListJsonNewsSources = [];
            JToken[] FeedListJsonNewsSourceFeeds = [];
            int SelectedCountryIndex = 0;
            var SelectedNewsSourceIndex = default(int);
            int SelectedNewsSourceFeedIndex;
            string FinalFeedUrl = "";

            // Try to get the feed list
            try
            {
                TextWriterColor.Write(Translate.DoTranslation("Downloading feed list..."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Progress));
                if (string.IsNullOrEmpty(FeedListJsonText))
                    FeedListJsonText = NetworkTransfer.DownloadString("https://cdn.jsdelivr.net/gh/yavuz/news-feed-list-of-countries@master/news-feed-list-of-countries.json");
                FeedListJson = JToken.Parse(FeedListJsonText);
                FeedListJsonCountries = FeedListJson.SelectTokens("*").Where(c => c["newSources"].Any()).ToArray();
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed to get feed list: {0}", ex.Message);
                DebugWriter.WStkTrc(ex);
                TextWriterColor.Write(Translate.DoTranslation("Failed to download feed list."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
            }

            // Country selection
            while (StepNumber == 1)
            {
                // If the JSON token is actually full, show the list of countries
                ConsoleWrapper.Clear();
                TextWriterWhereColor.WriteWhere(Translate.DoTranslation("Select your country by pressing the arrow left or arrow right keys. Press ENTER to confirm your selection."), 0, 1, false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
                TextWriterColor.Write(Kernel.Kernel.NewLine + Kernel.Kernel.NewLine + "   < ", false, KernelColorTools.ColTypes.Gray);

                // The cursor positions for the arrow elements
                int MaxLength = FeedListJsonCountries.Max(x => x["name"].ToString().Length);
                string ItemName = $"{FeedListJsonCountries[SelectedCountryIndex]["name"]} [{FeedListJsonCountries[SelectedCountryIndex]["iso"]}]";
                int ArrowLeftXPosition = ConsoleWrapper.CursorLeft + MaxLength + $" [{FeedListJsonCountries[SelectedCountryIndex]["iso"]}]".Length;
                int ItemNameXPosition = (int)Math.Round(ConsoleWrapper.CursorLeft + (ArrowLeftXPosition - ConsoleWrapper.CursorLeft) / 2d - ItemName.Length / 2d);
                TextWriterWhereColor.WriteWhere(ItemName, ItemNameXPosition, ConsoleWrapper.CursorTop, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option));
                TextWriterWhereColor.WriteWhere(" >", ArrowLeftXPosition, ConsoleWrapper.CursorTop, false, KernelColorTools.ColTypes.Gray);
                TextWriterColor.Write(Kernel.Kernel.NewLine + Kernel.Kernel.NewLine + Translate.DoTranslation("This country has {0} news sources."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), FeedListJsonCountries[SelectedCountryIndex]["newSources"].Count());

                // Read and get response
                var ConsoleResponse = Input.DetectKeypress();
                DebugWriter.Wdbg(DebugLevel.I, "Keypress: {0}", ConsoleResponse.Key.ToString());
                if (ConsoleResponse.Key == ConsoleKey.LeftArrow)
                {
                    // Decrement country index by 1
                    DebugWriter.Wdbg(DebugLevel.I, "Decrementing number...");
                    if (SelectedCountryIndex == 0)
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Reached zero! Back to country index {0}.", FeedListJsonCountries.Length - 1);
                        SelectedCountryIndex = FeedListJsonCountries.Length - 1;
                    }
                    else
                    {
                        SelectedCountryIndex -= 1;
                        DebugWriter.Wdbg(DebugLevel.I, "Decremented to {0}", SelectedCountryIndex);
                    }
                }
                else if (ConsoleResponse.Key == ConsoleKey.RightArrow)
                {
                    // Increment country index by 1
                    if (SelectedCountryIndex == FeedListJsonCountries.Length - 1)
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Reached maximum country number! Back to zero.");
                        SelectedCountryIndex = 0;
                    }
                    else
                    {
                        SelectedCountryIndex += 1;
                        DebugWriter.Wdbg(DebugLevel.I, "Incremented to {0}", SelectedCountryIndex);
                    }
                }
                else if (ConsoleResponse.Key == ConsoleKey.Enter)
                {
                    // Go to the next step
                    DebugWriter.Wdbg(DebugLevel.I, "Selected country: {0}", FeedListJsonCountries[SelectedCountryIndex]["name"]);
                    FeedListJsonNewsSources = [.. FeedListJsonCountries[SelectedCountryIndex]["newSources"]];
                    TextWriterColor.WritePlain("", true);
                    StepNumber += 1;
                }
            }

            // News source selection
            TextWriterColor.Write(Translate.DoTranslation("Select your favorite news source by writing the number. Press ENTER to confirm your selection.") + Kernel.Kernel.NewLine, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
            for (int SourceIndex = 0, loopTo = FeedListJsonNewsSources.Length - 1; SourceIndex <= loopTo; SourceIndex++)
            {
                var NewsSource = FeedListJsonNewsSources[SourceIndex];
                string NewsSourceTitle = NewsSource["site"]["title"].ToString().Trim();
                TextWriterColor.Write("{0}) {1}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), SourceIndex + 1, NewsSourceTitle);
            }
            TextWriterColor.WritePlain("", true);
            while (StepNumber == 2)
            {
                // Print input
                DebugWriter.Wdbg(DebugLevel.W, "{0} news sources.", FeedListJsonNewsSources.Length);
                TextWriterColor.Write(">> ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));

                // Read and parse the answer
                string AnswerStr = Input.ReadLine();
                if (StringQuery.IsStringNumeric(AnswerStr))
                {
                    // Got a numeric string! Check to see if we're in range before parsing it to index
                    int AnswerInt = Convert.ToInt32(AnswerStr);
                    DebugWriter.Wdbg(DebugLevel.W, "Got answer {0}.", AnswerInt);
                    if (AnswerInt > 0 & AnswerInt <= FeedListJsonNewsSources.Length)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Answer is in range.");
                        SelectedNewsSourceIndex = AnswerInt - 1;
                        FeedListJsonNewsSourceFeeds = [.. FeedListJsonNewsSources[SelectedNewsSourceIndex]["feedUrls"]];
                        TextWriterColor.WritePlain("", true);
                        StepNumber += 1;
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Answer is out of range.");
                        TextWriterColor.Write(Translate.DoTranslation("The selection is out of range. Select between 1-{0}. Try again."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), FeedListJsonNewsSources.Length);
                    }
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Answer is not numeric.");
                    TextWriterColor.Write(Translate.DoTranslation("The answer must be numeric."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                }
            }

            // News feed selection
            TextWriterColor.Write(Translate.DoTranslation("Select a feed for your favorite news source. Press ENTER to confirm your selection.") + Kernel.Kernel.NewLine, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
            for (int SourceFeedIndex = 0, loopTo1 = FeedListJsonNewsSourceFeeds.Length - 1; SourceFeedIndex <= loopTo1; SourceFeedIndex++)
            {
                var NewsSourceFeed = FeedListJsonNewsSourceFeeds[SourceFeedIndex];
                string NewsSourceTitle = (string)NewsSourceFeed["title"];
                if (string.IsNullOrEmpty(NewsSourceTitle))
                {
                    // Some feeds like the French nouvelobs.com (Obs) don't have their feed title, so take it from the site title instead
                    NewsSourceTitle = (string)FeedListJsonNewsSources[SelectedNewsSourceIndex]["site"]["title"];
                }
                NewsSourceTitle = NewsSourceTitle.Trim();
                TextWriterColor.Write("{0}) {1}: {2}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Option), SourceFeedIndex + 1, NewsSourceTitle, NewsSourceFeed["url"]);
            }
            TextWriterColor.WritePlain("", true);
            while (StepNumber == 3)
            {
                // Print input
                DebugWriter.Wdbg(DebugLevel.W, "{0} news source feeds.", FeedListJsonNewsSourceFeeds.Length);
                TextWriterColor.Write(">> ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));

                // Read and parse the answer
                string AnswerStr = Input.ReadLine();
                if (StringQuery.IsStringNumeric(AnswerStr))
                {
                    // Got a numeric string! Check to see if we're in range before parsing it to index
                    int AnswerInt = Convert.ToInt32(AnswerStr);
                    DebugWriter.Wdbg(DebugLevel.W, "Got answer {0}.", AnswerInt);
                    if (AnswerInt > 0 & AnswerInt <= FeedListJsonNewsSourceFeeds.Length)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Answer is in range.");
                        SelectedNewsSourceFeedIndex = AnswerInt - 1;
                        FinalFeedUrl = (string)FeedListJsonNewsSourceFeeds[SelectedNewsSourceFeedIndex]["url"];
                        StepNumber += 1;
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Answer is out of range.");
                        TextWriterColor.Write(Translate.DoTranslation("The selection is out of range. Select between 1-{0}. Try again."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), FeedListJsonNewsSourceFeeds.Length);
                    }
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Answer is not numeric.");
                    TextWriterColor.Write(Translate.DoTranslation("The answer must be numeric."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                }
            }

            // Actually change the feed
            RSSShellCommon.RSSFeedLink = FinalFeedUrl;
        }

    }
}
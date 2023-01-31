
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

using System.Collections.Generic;
using System.Xml;
using System;
using KS.Languages;
using KS.Kernel.Debugging;
using KS.Network.Base.Transfer;
using KS.Kernel.Exceptions;

namespace KS.Network.RSS.Instance
{
    /// <summary>
    /// RSS feed class instance
    /// </summary>
    public class RSSFeed
    {

        private string _FeedUrl;
        private RSSFeedType _FeedType;
        private string _FeedTitle;
        private string _FeedDescription;
        private List<RSSArticle> _FeedArticles = new();

        /// <summary>
        /// A URL to RSS feed
        /// </summary>
        public string FeedUrl => _FeedUrl;

        /// <summary>
        /// RSS feed type
        /// </summary>
        public RSSFeedType FeedType => _FeedType;

        /// <summary>
        /// RSS feed type
        /// </summary>
        public string FeedTitle => _FeedTitle;

        /// <summary>
        /// RSS feed description (Atom feeds not supported and always return an empty string)
        /// </summary>
        public string FeedDescription => _FeedDescription;

        /// <summary>
        /// Feed articles
        /// </summary>
        public List<RSSArticle> FeedArticles => _FeedArticles;

        /// <summary>
        /// Makes a new instance of an RSS feed class
        /// </summary>
        /// <param name="FeedUrl">A URL to RSS feed</param>
        /// <param name="FeedType">A feed type to parse. If set to Infer, it will automatically detect the type based on contents.</param>
        public RSSFeed(string FeedUrl, RSSFeedType FeedType) => Refresh(FeedUrl, FeedType);

        /// <summary>
        /// Refreshes the RSS class instance
        /// </summary>
        public void Refresh() => Refresh(_FeedUrl, _FeedType);

        /// <summary>
        /// Refreshes the RSS class instance
        /// </summary>
        /// <param name="FeedUrl">A URL to RSS feed</param>
        /// <param name="FeedType">A feed type to parse. If set to Infer, it will automatically detect the type based on contents.</param>
        public void Refresh(string FeedUrl, RSSFeedType FeedType)
        {
            // Make a web request indicator
            DebugWriter.WriteDebug(DebugLevel.I, "Refreshing feed {0}...", FeedUrl);
            var FeedWebRequest = NetworkTransfer.WClient.GetAsync(FeedUrl).Result;

            // Load the RSS feed and get the feed XML document
            var FeedStream = FeedWebRequest.Content.ReadAsStreamAsync().Result;
            var FeedDocument = new XmlDocument();
            FeedDocument.Load(FeedStream);

            // Infer feed type
            var FeedNodeList = default(XmlNodeList);
            if (FeedType == RSSFeedType.Infer)
            {
                if (FeedDocument.GetElementsByTagName("rss").Count != 0)
                {
                    FeedNodeList = FeedDocument.GetElementsByTagName("rss");
                    _FeedType = RSSFeedType.RSS2;
                }
                else if (FeedDocument.GetElementsByTagName("rdf:RDF").Count != 0)
                {
                    FeedNodeList = FeedDocument.GetElementsByTagName("rdf:RDF");
                    _FeedType = RSSFeedType.RSS1;
                }
                else if (FeedDocument.GetElementsByTagName("feed").Count != 0)
                {
                    FeedNodeList = FeedDocument.GetElementsByTagName("feed");
                    _FeedType = RSSFeedType.Atom;
                }
            }
            else if (FeedType == RSSFeedType.RSS2)
            {
                FeedNodeList = FeedDocument.GetElementsByTagName("rss");
                if (FeedNodeList.Count == 0)
                    throw new KernelException(KernelExceptionType.InvalidFeedType, Translate.DoTranslation("Invalid RSS2 feed."));
            }
            else if (FeedType == RSSFeedType.RSS1)
            {
                FeedNodeList = FeedDocument.GetElementsByTagName("rdf:RDF");
                if (FeedNodeList.Count == 0)
                    throw new KernelException(KernelExceptionType.InvalidFeedType, Translate.DoTranslation("Invalid RSS1 feed."));
            }
            else if (FeedType == RSSFeedType.Atom)
            {
                FeedNodeList = FeedDocument.GetElementsByTagName("feed");
                if (FeedNodeList.Count == 0)
                    throw new KernelException(KernelExceptionType.InvalidFeedType, Translate.DoTranslation("Invalid Atom feed."));
            }

            // Populate basic feed properties
            string FeedTitle = Convert.ToString(RSSTools.GetFeedProperty("title", FeedNodeList, _FeedType));
            string FeedDescription = Convert.ToString(RSSTools.GetFeedProperty("description", FeedNodeList, _FeedType));

            // Populate articles
            var Articles = RSSTools.MakeRssArticlesFromFeed(FeedNodeList, _FeedType);

            // Install the variables to a new instance
            _FeedUrl = FeedUrl;
            _FeedTitle = FeedTitle.Trim();
            _FeedDescription = FeedDescription.Trim();
            if (_FeedArticles.Count != 0 & Articles.Count != 0)
            {
                if (!_FeedArticles[0].Equals(Articles[0]))
                {
                    _FeedArticles = Articles;
                }
            }
            else
            {
                _FeedArticles = Articles;
            }
        }

    }

    /// <summary>
    /// Enumeration for RSS feed type
    /// </summary>
    public enum RSSFeedType
    {
        /// <summary>
        /// The RSS format is RSS 2.0
        /// </summary>
        RSS2,
        /// <summary>
        /// The RSS format is RSS 1.0
        /// </summary>
        RSS1,
        /// <summary>
        /// The RSS format is Atom
        /// </summary>
        Atom,
        /// <summary>
        /// Try to infer RSS type
        /// </summary>
        Infer = 1024
    }
}

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
using System.Net.Http;
using Nettify.Rss.Instance;
using Nitrocid.ShellPacks.Tools;
using Nitrocid.Kernel.Threading;
using Nitrocid.Network.Connections;

namespace Nitrocid.ShellPacks.Shells.RSS
{
    /// <summary>
    /// Common RSS shell module
    /// </summary>
    public static class RSSShellCommon
    {

        internal static NetworkConnection? clientConnection;
        internal static RSSFeed? feedInstance;
        internal static int fetchTimeout = 60000;
        internal static int refreshInterval = 60000;
        internal static KernelThread RSSRefresher = new("RSS Feed Refresher", false, RSSShellTools.RefreshFeeds);
        internal static HttpClient RSSRefresherClient = new() { Timeout = TimeSpan.FromMilliseconds(RSSFetchTimeout) };
        internal static string rssFeedLink = "";

        /// <summary>
        /// RSS feed instance
        /// </summary>
        public static RSSFeed? RSSFeedInstance =>
            feedInstance;
        /// <summary>
        /// RSS feed URL prompt style
        /// </summary>
        public static string RSSFeedUrlPromptStyle =>
            ShellsInit.ShellsConfig.RSSFeedUrlPromptStyle;
        /// <summary>
        /// RSS fetch timeout in milliseconds
        /// </summary>
        public static int RSSFetchTimeout =>
            ShellsInit.ShellsConfig.RSSFetchTimeout;
        /// <summary>
        /// Whether to refresh RSS feeds or not
        /// </summary>
        public static bool RSSRefreshFeeds =>
            ShellsInit.ShellsConfig.RSSRefreshFeeds;
        /// <summary>
        /// RSS refresh interval in milliseconds
        /// </summary>
        public static int RSSRefreshInterval =>
            ShellsInit.ShellsConfig.RSSRefreshInterval;
        /// <summary>
        /// Whether to keep the connection alive or not
        /// </summary>
        public static bool RSSKeepAlive { get; set; }
        /// <summary>
        /// RSS feed URL prompt style
        /// </summary>
        public static string RSSFeedLink =>
            rssFeedLink;
    }
}

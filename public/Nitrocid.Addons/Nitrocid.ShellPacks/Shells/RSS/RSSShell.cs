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
using System.Linq;
using System.Net.Http;
using System.Threading;
using Nettify.Rss.Instance;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Notifications;
using Nitrocid.Base.Network.Connections;
using Nitrocid.Base.Network.SpeedDial;
using Terminaux.Inputs;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Threadify.Manager;

namespace Nitrocid.ShellPacks.Shells.RSS
{
    /// <summary>
    /// The RSS shell
    /// </summary>
    public class RSSShell : BaseShell, IShell
    {
        internal NetworkInstanceConnection<RSSFeed>? clientConnection;
        internal RSSFeed? feedInstance;
        internal int fetchTimeout = 60000;
        internal int refreshInterval = 60000;
        internal ThreadInstance RSSRefresher = new("RSS Feed Refresher", false, new ParameterizedThreadStart((shell) => ((RSSShell?)shell)?.RefreshFeeds()));
        internal HttpClient RSSRefresherClient = new() { Timeout = TimeSpan.FromMilliseconds(ShellsInit.ShellsConfig.RSSFetchTimeout) };

        /// <summary>
        /// RSS feed instance
        /// </summary>
        public RSSFeed? RSSFeedInstance =>
            feedInstance;

        /// <summary>
        /// Whether to keep the connection alive or not
        /// </summary>
        public bool RSSKeepAlive { get; set; }

        /// <inheritdoc/>
        public override string ShellType => "RSSShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        internal bool detaching = false;

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Parse shell arguments
            var rssConnection = (NetworkInstanceConnection<RSSFeed>)ShellArgs[0];
            RSSFeed? rssFeed = rssConnection.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.RSSShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOCLIENT"));
            feedInstance = rssFeed;

            // Send ping to keep the connection alive
            if (!RSSKeepAlive & !RSSRefresher.IsAlive & ShellsInit.ShellsConfig.RSSRefreshFeeds)
            {
                RSSRefresher.Start(this);
                DebugWriter.WriteDebug(DebugLevel.I, "Made new thread about RefreshFeeds()");
            }

            // Write connection information to Speed Dial file if it doesn't exist there
            SpeedDialTools.TryAddEntryToSpeedDial(rssFeed.FeedUrl, rssConnection.ConnectionUri.Port, NetworkConnectionType.RSS, "", "", false);

            while (!Bail)
            {
                try
                {
                    // Prompt for the command
                    ShellManager.GetLine();
                }
                catch (ThreadInterruptedException)
                {
                    CancellationHandlers.DismissRequest();
                    Bail = true;
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_SHELL_ERROR") + " {0}", true, ThemeColorType.Error, ex.Message);
                    DebugWriter.WriteDebug(DebugLevel.E, "Shell will have to exit: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    Input.ReadKey();
                    Bail = true;
                }

                // Exiting, so reset the site
                if (Bail)
                {
                    if (!detaching)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Exit requested. Disconnecting host...");
                        if (ShellsInit.ShellsConfig.RSSRefreshFeeds)
                            RSSRefresher.Stop();
                        int connectionIndex = NetworkConnectionTools.GetConnectionIndex(rssConnection);
                        NetworkConnectionTools.CloseConnection(connectionIndex);
                        clientConnection = null;
                    }
                    detaching = false;
                    feedInstance = null;
                }
            }
        }

        /// <summary>
        /// Refreshes the feeds
        /// </summary>
        internal void RefreshFeeds()
        {
            try
            {
                var articles = RSSFeedInstance?.FeedArticles ?? [];
                var OldFeedsList = new List<RSSArticle>(articles);
                List<RSSArticle> NewFeedsList;
                while (RSSFeedInstance is not null)
                {
                    if (RSSFeedInstance is not null)
                    {
                        // Refresh the feed
                        RSSFeedInstance.Refresh();

                        // Check for new feeds
                        NewFeedsList = [.. articles.Except(OldFeedsList)];
                        string OldFeedTitle = OldFeedsList.Count == 0 ? "" : OldFeedsList[0].ArticleTitle;
                        if (NewFeedsList.Count > 0 && NewFeedsList[0].ArticleTitle != OldFeedTitle)
                        {
                            // Update the list
                            DebugWriter.WriteDebug(DebugLevel.W, "Feeds received! Recents count was {0}, Old count was {1}", vars: [articles.Length, OldFeedsList.Count]);
                            OldFeedsList = [.. articles];
                            foreach (RSSArticle NewFeed in NewFeedsList)
                            {
                                var FeedNotif = new Notification(NewFeed.ArticleTitle, NewFeed.ArticleDescription, NotificationPriority.Low, NotificationType.Normal);
                                NotificationManager.NotifySend(FeedNotif);
                            }
                        }
                    }
                    Thread.Sleep(ShellsInit.ShellsConfig.RSSRefreshInterval);
                }
            }
            catch (ThreadInterruptedException)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Aborting refresher...");
            }
        }
    }
}

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
using System.Threading;
using Nettify.Rss.Instance;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Nitrocid.Kernel.Debugging;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Terminaux.Colors.Themes.Colors;
using Textify.General;
using Nitrocid.Network.SpeedDial;
using Nitrocid.Network.Connections;
using Nitrocid.Kernel.Exceptions;

namespace Nitrocid.ShellPacks.Shells.RSS
{
    /// <summary>
    /// The RSS shell
    /// </summary>
    public class RSSShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "RSSShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        internal bool detaching = false;

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Parse shell arguments
            NetworkConnection rssConnection = (NetworkConnection)ShellArgs[0];
            RSSFeed? rssFeed = (RSSFeed?)rssConnection.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.RSSShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOCLIENT"));
            RSSShellCommon.feedInstance = rssFeed;
            RSSShellCommon.rssFeedLink = rssFeed.FeedUrl;

            // Send ping to keep the connection alive
            if (!RSSShellCommon.RSSKeepAlive & !RSSShellCommon.RSSRefresher.IsAlive & ShellsInit.ShellsConfig.RSSRefreshFeeds)
            {
                RSSShellCommon.RSSRefresher.Start();
                DebugWriter.WriteDebug(DebugLevel.I, "Made new thread about RefreshFeeds()");
            }

            // Write connection information to Speed Dial file if it doesn't exist there
            SpeedDialTools.TryAddEntryToSpeedDial(rssFeed.FeedUrl, rssConnection.ConnectionUri.Port, NetworkConnectionType.RSS, false);

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
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_SHELL_ERROR") + CharManager.NewLine + "Error {0}: {1}", true, ThemeColorType.Error, ex.GetType().FullName ?? "<null>", ex.Message);
                    continue;
                }

                // Exiting, so reset the site
                if (Bail)
                {
                    if (!detaching)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Exit requested. Disconnecting host...");
                        if (ShellsInit.ShellsConfig.RSSRefreshFeeds)
                            RSSShellCommon.RSSRefresher.Stop();
                        int connectionIndex = NetworkConnectionTools.GetConnectionIndex(rssConnection);
                        NetworkConnectionTools.CloseConnection(connectionIndex);
                        RSSShellCommon.clientConnection = null;
                    }
                    detaching = false;
                    RSSShellCommon.rssFeedLink = "";
                    RSSShellCommon.feedInstance = null;
                }
            }
        }

    }
}

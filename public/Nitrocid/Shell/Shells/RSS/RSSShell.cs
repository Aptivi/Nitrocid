
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
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.Base.Connections;
using Syndian.Instance;
using KS.Shell.ShellBase.Shells;
using KS.Shell.Shells.HTTP;

namespace KS.Shell.Shells.RSS
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
            RSSFeed rssFeed = (RSSFeed)rssConnection.ConnectionInstance;
            RSSShellCommon.feedInstance = rssFeed;
            RSSShellCommon.rssFeedLink = rssFeed.FeedUrl;

            // Send ping to keep the connection alive
            if (!RSSShellCommon.RSSKeepAlive & !RSSShellCommon.RSSRefresher.IsAlive & RSSShellCommon.RSSRefreshFeeds)
            {
                RSSShellCommon.RSSRefresher.Start();
                DebugWriter.WriteDebug(DebugLevel.I, "Made new thread about RefreshFeeds()");
            }

            while (!Bail)
            {
                try
                {
                    // Prompt for the command
                    ShellManager.GetLine();
                }
                catch (ThreadInterruptedException)
                {
                    Flags.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriterColor.Write(Translate.DoTranslation("There was an error in the shell.") + CharManager.NewLine + "Error {0}: {1}", true, KernelColorType.Error, ex.GetType().FullName, ex.Message);
                    continue;
                }

                // Exiting, so reset the site
                if (Bail)
                {
                    if (!detaching)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Exit requested. Disconnecting host...");
                        if (RSSShellCommon.RSSRefreshFeeds)
                            RSSShellCommon.RSSRefresher.Stop();
                        int connectionIndex = NetworkConnectionTools.GetConnectionIndex(HTTPShellCommon.ClientHTTP);
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

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
using System.Net.Http;
using System.Threading;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Network.Connections;
using Nitrocid.Base.Network.SpeedDial;
using Terminaux.Themes.Colors;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Inputs;

namespace Nitrocid.ShellPacks.Shells.HTTP
{
    /// <summary>
    /// The HTTP shell
    /// </summary>
    public partial class HTTPShell : BaseShell, IShell
    {
        internal bool detaching = false;
        internal NetworkConnection? clientConnection;

        /// <summary>
        /// HTTP site URL
        /// </summary>
        public string HTTPSite { get; set; } = "";

        /// <summary>
        /// HTTP shell prompt style
        /// </summary>
        public string HTTPShellPromptStyle { get; set; } = "";

        /// <summary>
        /// An HTTP client
        /// </summary>
        public NetworkConnection? ClientHTTP =>
            clientConnection;

        /// <inheritdoc/>
        public override string ShellType => "HTTPShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Parse shell arguments
            NetworkConnection httpConnection = (NetworkConnection)ShellArgs[0];
            clientConnection = httpConnection;
            HTTPSite = httpConnection.ConnectionUri.OriginalString;

            // Write connection information to Speed Dial file if it doesn't exist there
            SpeedDialTools.TryAddEntryToSpeedDial(HTTPSite, httpConnection.ConnectionUri.Port, NetworkConnectionType.HTTP, "", "", false);

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
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_EXCEPTION_SHELLERROR") + " {0}", true, ThemeColorType.Error, ex.Message);
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
                        ((HttpClient?)ClientHTTP?.ConnectionInstance)?.Dispose();
                        int connectionIndex = NetworkConnectionTools.GetConnectionIndex(ClientHTTP);
                        NetworkConnectionTools.CloseConnection(connectionIndex);
                        clientConnection = null;
                    }
                    detaching = false;
                    HTTPSite = "";
                }
            }
        }

    }
}

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
using System.Threading;
using Nitrocid.Base.ConsoleBase.Inputs;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Network.Connections;
using Nitrocid.Base.Network.SpeedDial;
using Terminaux.Colors.Themes.Colors;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ShellPacks.Shells.HTTP
{
    /// <summary>
    /// The HTTP shell
    /// </summary>
    public class HTTPShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "HTTPShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        internal bool detaching = false;

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Parse shell arguments
            NetworkConnection httpConnection = (NetworkConnection)ShellArgs[0];
            HTTPShellCommon.clientConnection = httpConnection;
            HTTPShellCommon.HTTPSite = httpConnection.ConnectionUri.OriginalString;

            // Write connection information to Speed Dial file if it doesn't exist there
            SpeedDialTools.TryAddEntryToSpeedDial(HTTPShellCommon.HTTPSite, httpConnection.ConnectionUri.Port, NetworkConnectionType.HTTP, false);

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
                    InputTools.DetectKeypress();
                    Bail = true;
                }

                // Exiting, so reset the site
                if (Bail)
                {
                    if (!detaching)
                    {
                        ((HttpClient?)HTTPShellCommon.ClientHTTP?.ConnectionInstance)?.Dispose();
                        int connectionIndex = NetworkConnectionTools.GetConnectionIndex(HTTPShellCommon.ClientHTTP);
                        NetworkConnectionTools.CloseConnection(connectionIndex);
                        HTTPShellCommon.clientConnection = null;
                    }
                    detaching = false;
                    HTTPShellCommon.HTTPSite = "";
                }
            }
        }

    }
}

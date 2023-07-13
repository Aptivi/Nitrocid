
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
using System.Net.Http;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Network.Base.Connections;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.HTTP.Commands
{
    /// <summary>
    /// Sets a URL for the current shell instance
    /// </summary>
    /// <remarks>
    /// If you want to be able to execute any HTTP command, you must issue this command first. It sets the target URL that you want to send commands to.
    /// <br></br>
    /// You must set the target URL to the one that supports the commands.
    /// </remarks>
    class HTTP_SetSiteCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            try
            {
                var SiteUri = new Uri(StringArgs);
                TextWriterColor.Write(Translate.DoTranslation("Setting site to") + " {0}...", true, KernelColorType.Progress, SiteUri.ToString());
                HTTPShellCommon.clientConnection = NetworkConnectionTools.EstablishConnection("HTTP connection", SiteUri, NetworkConnectionType.HTTP, new HttpClient());
                HTTPShellCommon.HTTPSite = SiteUri.ToString();
            }
            catch (Exception)
            {
                TextWriterColor.Write(Translate.DoTranslation("The site URI format is invalid."), true, KernelColorType.Error);
            }
        }

    }
}

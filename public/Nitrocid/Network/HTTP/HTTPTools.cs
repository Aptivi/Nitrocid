
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
using System.Threading.Tasks;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.Shells.HTTP;

namespace KS.Network.HTTP
{
    /// <summary>
    /// HTTP tools
    /// </summary>
    public static class HTTPTools
    {

        /// <summary>
        /// Deletes the specified content from HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetodelete.html")</param>
        public async static Task HttpDelete(string ContentUri)
        {
            if (HTTPShellCommon.HTTPConnected)
            {
                var TargetUri = new Uri(NeutralizeUri(ContentUri));
                await HTTPShellCommon.ClientHTTP.DeleteAsync(TargetUri);
            }
            else
            {
                throw new KernelException(KernelExceptionType.HTTPShell, Translate.DoTranslation("You must connect to server with administrative privileges before performing the deletion."));
            }
        }

        /// <summary>
        /// Gets the specified content string from HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        public async static Task<string> HttpGetString(string ContentUri)
        {
            if (HTTPShellCommon.HTTPConnected)
            {
                var TargetUri = new Uri(NeutralizeUri(ContentUri));
                return await HTTPShellCommon.ClientHTTP.GetStringAsync(TargetUri);
            }
            else
            {
                throw new KernelException(KernelExceptionType.HTTPShell, Translate.DoTranslation("You must connect to server before performing transmission."));
            }
        }

        /// <summary>
        /// Gets the specified content from HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        public async static Task<HttpResponseMessage> HttpGet(string ContentUri)
        {
            if (HTTPShellCommon.HTTPConnected)
            {
                var TargetUri = new Uri(NeutralizeUri(ContentUri));
                return await HTTPShellCommon.ClientHTTP.GetAsync(TargetUri);
            }
            else
            {
                throw new KernelException(KernelExceptionType.HTTPShell, Translate.DoTranslation("You must connect to server before performing transmission."));
            }
        }

        /// <summary>
        /// Neutralize the URI so the host name, <see cref="HTTPShellCommon.HTTPSite"/>, doesn't appear twice.
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        public static string NeutralizeUri(string ContentUri)
        {
            string NeutralizedUri = "";
            if (!ContentUri.StartsWith(HTTPShellCommon.HTTPSite))
                NeutralizedUri += HTTPShellCommon.HTTPSite;
            NeutralizedUri += ContentUri;
            return NeutralizedUri;
        }

    }
}

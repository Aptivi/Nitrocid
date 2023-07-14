
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
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using KS.Files;
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
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            await ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).DeleteAsync(TargetUri);
        }

        /// <summary>
        /// Gets the specified content string from HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        public async static Task<string> HttpGetString(string ContentUri)
        {
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            return await ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).GetStringAsync(TargetUri);
        }

        /// <summary>
        /// Gets the specified content from HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        public async static Task<HttpResponseMessage> HttpGet(string ContentUri)
        {
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            return await ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).GetAsync(TargetUri);
        }

        /// <summary>
        /// Puts the specified content string to the HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentString">String to put to the HTTP server</param>
        public async static Task<HttpResponseMessage> HttpPutString(string ContentUri, string ContentString)
        {
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            var stringContent = new StringContent(ContentString);
            return await ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).PutAsync(TargetUri, stringContent);
        }

        /// <summary>
        /// Puts the specified file to the HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentPath">Path to the file to open a stream and put it to the HTTP server</param>
        public async static Task<HttpResponseMessage> HttpPutFile(string ContentUri, string ContentPath)
        {
            ContentPath = Filesystem.NeutralizePath(ContentPath);
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            var TargetStream = new FileStream(ContentPath, FileMode.Open, FileAccess.Read);
            var stringContent = new StreamContent(TargetStream);
            return await ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).PutAsync(TargetUri, stringContent);
        }

        /// <summary>
        /// Posts the specified content string to the HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentString">String to post to the HTTP server</param>
        public async static Task<HttpResponseMessage> HttpPostString(string ContentUri, string ContentString)
        {
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            var stringContent = new StringContent(ContentString);
            return await ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).PostAsync(TargetUri, stringContent);
        }

        /// <summary>
        /// Posts the specified file to the HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentPath">Path to the file to open a stream and post it to the HTTP server</param>
        public async static Task<HttpResponseMessage> HttpPostFile(string ContentUri, string ContentPath)
        {
            ContentPath = Filesystem.NeutralizePath(ContentPath);
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            var TargetStream = new FileStream(ContentPath, FileMode.Open, FileAccess.Read);
            var stringContent = new StreamContent(TargetStream);
            return await ((HttpClient)HTTPShellCommon.ClientHTTP.ConnectionInstance).PostAsync(TargetUri, stringContent);
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

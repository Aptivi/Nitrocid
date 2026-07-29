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

using System.Net.Http;
using System.Threading.Tasks;
using Nitrocid.Base.Network.Types.HTTP;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.HTTP
{
    /// <summary>
    /// HTTP tools
    /// </summary>
    public partial class HTTPShell : BaseShell, IShell
    {
        /// <summary>
        /// Deletes the specified content from HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetodelete.html")</param>
        public async Task HttpDelete(string ContentUri) =>
            await HTTPTools.HttpDelete(HTTPClient, ContentUri, HTTPSite);

        /// <summary>
        /// Gets the specified content string from HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        public async Task<string> HttpGetString(string ContentUri) =>
            await HTTPTools.HttpGetString(HTTPClient, ContentUri, HTTPSite);

        /// <summary>
        /// Gets the specified content from HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        public async Task<HttpResponseMessage> HttpGet(string ContentUri) =>
            await HTTPTools.HttpGet(HTTPClient, ContentUri, HTTPSite);

        /// <summary>
        /// Puts the specified content string to the HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentString">String to put to the HTTP server</param>
        public async Task<HttpResponseMessage> HttpPutString(string ContentUri, string ContentString) =>
            await HTTPTools.HttpPutString(HTTPClient, ContentUri, ContentString, HTTPSite);

        /// <summary>
        /// Puts the specified file to the HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentPath">Path to the file to open a stream and put it to the HTTP server</param>
        public async Task<HttpResponseMessage> HttpPutFile(string ContentUri, string ContentPath) =>
            await HTTPTools.HttpPutFile(HTTPClient, ContentUri, ContentPath, HTTPSite);

        /// <summary>
        /// Posts the specified content string to the HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentString">String to post to the HTTP server</param>
        public async Task<HttpResponseMessage> HttpPostString(string ContentUri, string ContentString) =>
            await HTTPTools.HttpPostString(HTTPClient, ContentUri, ContentString, HTTPSite);

        /// <summary>
        /// Posts the specified file to the HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentPath">Path to the file to open a stream and post it to the HTTP server</param>
        public async Task<HttpResponseMessage> HttpPostFile(string ContentUri, string ContentPath) =>
            await HTTPTools.HttpPostFile(HTTPClient, ContentUri, ContentPath, HTTPSite);

        /// <summary>
        /// Adds a request header to the future requests
        /// </summary>
        /// <param name="key">Key to assign a value to</param>
        /// <param name="value">Value to assign to this key</param>
        public void HttpAddHeader(string key, string value) =>
            HTTPTools.HttpAddHeader(HTTPClient, key, value);

        /// <summary>
        /// Adds a request header to the future requests
        /// </summary>
        /// <param name="key">Key to remove</param>
        public void HttpRemoveHeader(string key) =>
            HTTPTools.HttpRemoveHeader(HTTPClient, key);

        /// <summary>
        /// Modifies a request header key for the future requests
        /// </summary>
        /// <param name="key">Key to assign a value to</param>
        /// <param name="value">Value to assign to this key</param>
        public void HttpEditHeader(string key, string value) =>
            HTTPTools.HttpEditHeader(HTTPClient, key, value);

        /// <summary>
        /// Makes a list of headers
        /// </summary>
        /// <returns>An array of tuples containing keys and values from the HTTP request headers</returns>
        public (string, string)[] HttpListHeaders() =>
            HTTPTools.HttpListHeaders(HTTPClient);

        /// <summary>
        /// Checks to see if the specified key from the header exists
        /// </summary>
        /// <param name="key">Key to query</param>
        /// <returns>True if found; false otherwise.</returns>
        public bool HttpHeaderExists(string key) =>
            HTTPTools.HttpHeaderExists(HTTPClient, key);

        /// <summary>
        /// Gets the current user agent
        /// </summary>
        /// <returns>
        /// The current user agent. If there are two or more user agents set in the same header (by somehow adding the same
        /// key with different UA), returns the last user agent value.
        /// </returns>
        public string HttpGetCurrentUserAgent() =>
            HTTPTools.HttpGetCurrentUserAgent(HTTPClient);

        /// <summary>
        /// Sets the current user agent
        /// </summary>
        /// <param name="userAgent">Target user agent</param>
        public void HttpSetUserAgent(string userAgent) =>
            HTTPTools.HttpSetUserAgent(HTTPClient, userAgent);
    }
}

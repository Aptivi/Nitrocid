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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;

namespace Nitrocid.Base.Network.Types.HTTP
{
    /// <summary>
    /// HTTP tools
    /// </summary>
    public static class HTTPTools
    {
        /// <summary>
        /// Deletes the specified content from HTTP server
        /// </summary>
        /// <param name="client">HTTP client</param>
        /// <param name="httpSite">HTTP hostname (https://...)</param>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetodelete.html")</param>
        public static async Task HttpDelete(HttpClient client, string ContentUri, string httpSite)
        {
            var TargetUri = new Uri(NeutralizeUri(ContentUri, httpSite));
            await client.DeleteAsync(TargetUri);
        }

        /// <summary>
        /// Gets the specified content string from HTTP server
        /// </summary>
        /// <param name="client">HTTP client</param>
        /// <param name="httpSite">HTTP hostname (https://...)</param>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        public static async Task<string> HttpGetString(HttpClient client, string ContentUri, string httpSite)
        {
            var TargetUri = new Uri(NeutralizeUri(ContentUri, httpSite));
            return await client.GetStringAsync(TargetUri);
        }

        /// <summary>
        /// Gets the specified content from HTTP server
        /// </summary>
        /// <param name="client">HTTP client</param>
        /// <param name="httpSite">HTTP hostname (https://...)</param>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        public static async Task<HttpResponseMessage> HttpGet(HttpClient client, string ContentUri, string httpSite)
        {
            var TargetUri = new Uri(NeutralizeUri(ContentUri, httpSite));
            return await client.GetAsync(TargetUri);
        }

        /// <summary>
        /// Puts the specified content string to the HTTP server
        /// </summary>
        /// <param name="client">HTTP client</param>
        /// <param name="httpSite">HTTP hostname (https://...)</param>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentString">String to put to the HTTP server</param>
        public static async Task<HttpResponseMessage> HttpPutString(HttpClient client, string ContentUri, string ContentString, string httpSite)
        {
            var TargetUri = new Uri(NeutralizeUri(ContentUri, httpSite));
            var stringContent = new StringContent(ContentString);
            return await client.PutAsync(TargetUri, stringContent);
        }

        /// <summary>
        /// Puts the specified file to the HTTP server
        /// </summary>
        /// <param name="client">HTTP client</param>
        /// <param name="httpSite">HTTP hostname (https://...)</param>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentPath">Path to the file to open a stream and put it to the HTTP server</param>
        public static async Task<HttpResponseMessage> HttpPutFile(HttpClient client, string ContentUri, string ContentPath, string httpSite)
        {
            ContentPath = FilesystemTools.NeutralizePath(ContentPath);
            var TargetUri = new Uri(NeutralizeUri(ContentUri, httpSite));
            var TargetStream = new FileStream(ContentPath, FileMode.Open, FileAccess.Read);
            var stringContent = new StreamContent(TargetStream);
            return await client.PutAsync(TargetUri, stringContent);
        }

        /// <summary>
        /// Posts the specified content string to the HTTP server
        /// </summary>
        /// <param name="client">HTTP client</param>
        /// <param name="httpSite">HTTP hostname (https://...)</param>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentString">String to post to the HTTP server</param>
        public static async Task<HttpResponseMessage> HttpPostString(HttpClient client, string ContentUri, string ContentString, string httpSite)
        {
            var TargetUri = new Uri(NeutralizeUri(ContentUri, httpSite));
            var stringContent = new StringContent(ContentString);
            return await client.PostAsync(TargetUri, stringContent);
        }

        /// <summary>
        /// Posts the specified file to the HTTP server
        /// </summary>
        /// <param name="client">HTTP client</param>
        /// <param name="httpSite">HTTP hostname (https://...)</param>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentPath">Path to the file to open a stream and post it to the HTTP server</param>
        public static async Task<HttpResponseMessage> HttpPostFile(HttpClient client, string ContentUri, string ContentPath, string httpSite)
        {
            ContentPath = FilesystemTools.NeutralizePath(ContentPath);
            var TargetUri = new Uri(NeutralizeUri(ContentUri, httpSite));
            var TargetStream = new FileStream(ContentPath, FileMode.Open, FileAccess.Read);
            var stringContent = new StreamContent(TargetStream);
            return await client.PostAsync(TargetUri, stringContent);
        }

        /// <summary>
        /// Adds a request header to the future requests
        /// </summary>
        /// <param name="client">HTTP client</param>
        /// <param name="key">Key to assign a value to</param>
        /// <param name="value">Value to assign to this key</param>
        public static void HttpAddHeader(HttpClient client, string key, string value)
        {
            if (!HttpHeaderExists(client, key))
                client.DefaultRequestHeaders.Add(key, value);
            else
                throw new KernelException(KernelExceptionType.HTTPNetwork, LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_EXCEPTION_HEADEREXISTS"));
        }

        /// <summary>
        /// Adds a request header to the future requests
        /// </summary>
        /// <param name="client">HTTP client</param>
        /// <param name="key">Key to remove</param>
        public static void HttpRemoveHeader(HttpClient client, string key)
        {
            if (HttpHeaderExists(client, key))
                client.DefaultRequestHeaders.Remove(key);
            else
                throw new KernelException(KernelExceptionType.HTTPNetwork, LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_EXCEPTION_HEADERNOTEXISTS_REMOVE"));
        }

        /// <summary>
        /// Modifies a request header key for the future requests
        /// </summary>
        /// <param name="client">HTTP client</param>
        /// <param name="key">Key to assign a value to</param>
        /// <param name="value">Value to assign to this key</param>
        public static void HttpEditHeader(HttpClient client, string key, string value)
        {
            if (HttpHeaderExists(client, key))
            {
                // We can't just index a key from the request header collection and expect it to set to another value. We need to
                // remove the key and re-add the same key with different value
                HttpRemoveHeader(client, key);
                HttpAddHeader(client, key, value);
            }
            else
                throw new KernelException(KernelExceptionType.HTTPNetwork, LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_EXCEPTION_HEADERNOTEXISTS_EDIT"));
        }

        /// <summary>
        /// Makes a list of headers
        /// </summary>
        /// <param name="client">HTTP client</param>
        /// <returns>An array of tuples containing keys and values from the HTTP request headers</returns>
        public static (string, string)[] HttpListHeaders(HttpClient client)
        {
            var headers = client.DefaultRequestHeaders;
            var finalHeaders = new List<(string, string)>();

            // Enumerate through headers to convert them to tuples
            foreach (var header in headers)
            {
                var values = header.Value;
                foreach (var value in values)
                    finalHeaders.Add((header.Key, value));
            }
            return [.. finalHeaders];
        }

        /// <summary>
        /// Checks to see if the specified key from the header exists
        /// </summary>
        /// <param name="client">HTTP client</param>
        /// <param name="key">Key to query</param>
        /// <returns>True if found; false otherwise.</returns>
        public static bool HttpHeaderExists(HttpClient client, string key)
        {
            return client.DefaultRequestHeaders.Contains(key);
        }

        /// <summary>
        /// Gets the current user agent
        /// </summary>
        /// <param name="client">HTTP client</param>
        /// <returns>
        /// The current user agent. If there are two or more user agents set in the same header (by somehow adding the same
        /// key with different UA), returns the last user agent value.
        /// </returns>
        public static string HttpGetCurrentUserAgent(HttpClient client)
        {
            var userAgents = client.DefaultRequestHeaders.UserAgent;
            if (userAgents.Count > 0)
                // We don't support more than one UserAgent value, so return the last one and ignore the rest
                return userAgents.ElementAt(userAgents.Count - 1).ToString();
            return "";
        }

        /// <summary>
        /// Sets the current user agent
        /// </summary>
        /// <param name="client">HTTP client</param>
        /// <param name="userAgent">Target user agent</param>
        public static void HttpSetUserAgent(HttpClient client, string userAgent)
        {
            // Remove all user agent strings in case we have more than one instance
            while (HttpHeaderExists(client, "User-Agent"))
                HttpRemoveHeader(client, "User-Agent");

            // Now, set the user agent
            HttpAddHeader(client, "User-Agent", userAgent);
        }

        /// <summary>
        /// Neutralize the URI so the host name, <paramref name="httpSite"/>, doesn't appear twice.
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        /// <param name="httpSite">HTTP site</param>
        public static string NeutralizeUri(string ContentUri, string httpSite)
        {
            string NeutralizedUri = "";
            if (!ContentUri.StartsWith(httpSite))
                NeutralizedUri += httpSite;
            NeutralizedUri += ContentUri;
            return NeutralizedUri;
        }
    }
}

//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.ShellPacks.Shells.HTTP;

namespace Nitrocid.ShellPacks.Tools
{
    /// <summary>
    /// HTTP tools
    /// </summary>
    public static class HttpTools
    {

        /// <summary>
        /// Deletes the specified content from HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetodelete.html")</param>
        public async static Task HttpDelete(string ContentUri)
        {
            var client = (HttpClient?)HTTPShellCommon.ClientHTTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.HTTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_1"));
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            await client.DeleteAsync(TargetUri);
        }

        /// <summary>
        /// Gets the specified content string from HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        public async static Task<string> HttpGetString(string ContentUri)
        {
            var client = (HttpClient?)HTTPShellCommon.ClientHTTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.HTTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_1"));
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            return await client.GetStringAsync(TargetUri);
        }

        /// <summary>
        /// Gets the specified content from HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname, e.g. "filetoget.html")</param>
        public async static Task<HttpResponseMessage> HttpGet(string ContentUri)
        {
            var client = (HttpClient?)HTTPShellCommon.ClientHTTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.HTTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_1"));
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            return await client.GetAsync(TargetUri);
        }

        /// <summary>
        /// Puts the specified content string to the HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentString">String to put to the HTTP server</param>
        public async static Task<HttpResponseMessage> HttpPutString(string ContentUri, string ContentString)
        {
            var client = (HttpClient?)HTTPShellCommon.ClientHTTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.HTTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_1"));
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            var stringContent = new StringContent(ContentString);
            return await client.PutAsync(TargetUri, stringContent);
        }

        /// <summary>
        /// Puts the specified file to the HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentPath">Path to the file to open a stream and put it to the HTTP server</param>
        public async static Task<HttpResponseMessage> HttpPutFile(string ContentUri, string ContentPath)
        {
            var client = (HttpClient?)HTTPShellCommon.ClientHTTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.HTTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_1"));
            ContentPath = FilesystemTools.NeutralizePath(ContentPath);
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            var TargetStream = new FileStream(ContentPath, FileMode.Open, FileAccess.Read);
            var stringContent = new StreamContent(TargetStream);
            return await client.PutAsync(TargetUri, stringContent);
        }

        /// <summary>
        /// Posts the specified content string to the HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentString">String to post to the HTTP server</param>
        public async static Task<HttpResponseMessage> HttpPostString(string ContentUri, string ContentString)
        {
            var client = (HttpClient?)HTTPShellCommon.ClientHTTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.HTTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_1"));
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            var stringContent = new StringContent(ContentString);
            return await client.PostAsync(TargetUri, stringContent);
        }

        /// <summary>
        /// Posts the specified file to the HTTP server
        /// </summary>
        /// <param name="ContentUri">Content URI (starts after the HTTP hostname)</param>
        /// <param name="ContentPath">Path to the file to open a stream and post it to the HTTP server</param>
        public async static Task<HttpResponseMessage> HttpPostFile(string ContentUri, string ContentPath)
        {
            var client = (HttpClient?)HTTPShellCommon.ClientHTTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.HTTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_1"));
            ContentPath = FilesystemTools.NeutralizePath(ContentPath);
            var TargetUri = new Uri(NeutralizeUri(ContentUri));
            var TargetStream = new FileStream(ContentPath, FileMode.Open, FileAccess.Read);
            var stringContent = new StreamContent(TargetStream);
            return await client.PostAsync(TargetUri, stringContent);
        }

        /// <summary>
        /// Adds a request header to the future requests
        /// </summary>
        /// <param name="key">Key to assign a value to</param>
        /// <param name="value">Value to assign to this key</param>
        public static void HttpAddHeader(string key, string value)
        {
            var client = (HttpClient?)HTTPShellCommon.ClientHTTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.HTTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_1"));
            if (!HttpHeaderExists(key))
                client.DefaultRequestHeaders.Add(key, value);
            else
                throw new KernelException(KernelExceptionType.HTTPNetwork, LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_EXCEPTION_HEADEREXISTS"));
        }

        /// <summary>
        /// Adds a request header to the future requests
        /// </summary>
        /// <param name="key">Key to remove</param>
        public static void HttpRemoveHeader(string key)
        {
            var client = (HttpClient?)HTTPShellCommon.ClientHTTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.HTTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_1"));
            if (HttpHeaderExists(key))
                client.DefaultRequestHeaders.Remove(key);
            else
                throw new KernelException(KernelExceptionType.HTTPNetwork, LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_EXCEPTION_HEADERNOTEXISTS_REMOVE"));
        }

        /// <summary>
        /// Modifies a request header key for the future requests
        /// </summary>
        /// <param name="key">Key to assign a value to</param>
        /// <param name="value">Value to assign to this key</param>
        public static void HttpEditHeader(string key, string value)
        {
            if (HttpHeaderExists(key))
            {
                // We can't just index a key from the request header collection and expect it to set to another value. We need to
                // remove the key and re-add the same key with different value
                HttpRemoveHeader(key);
                HttpAddHeader(key, value);
            }
            else
                throw new KernelException(KernelExceptionType.HTTPNetwork, LanguageTools.GetLocalized("NKS_SHELLPACKS_HTTP_EXCEPTION_HEADERNOTEXISTS_EDIT"));
        }

        /// <summary>
        /// Makes a list of headers
        /// </summary>
        /// <returns>An array of tuples containing keys and values from the HTTP request headers</returns>
        public static (string, string)[] HttpListHeaders()
        {
            var client = (HttpClient?)HTTPShellCommon.ClientHTTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.HTTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_1"));
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
        /// <param name="key">Key to query</param>
        /// <returns>True if found; false otherwise.</returns>
        public static bool HttpHeaderExists(string key)
        {
            var client = (HttpClient?)HTTPShellCommon.ClientHTTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.HTTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_1"));
            return client.DefaultRequestHeaders.Contains(key);
        }

        /// <summary>
        /// Gets the current user agent
        /// </summary>
        /// <returns>
        /// The current user agent. If there are two or more user agents set in the same header (by somehow adding the same
        /// key with different UA), returns the last user agent value.
        /// </returns>
        public static string HttpGetCurrentUserAgent()
        {
            var client = (HttpClient?)HTTPShellCommon.ClientHTTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.HTTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_EXCEPTION_NOTCONNECTED_1"));
            var userAgents = client.DefaultRequestHeaders.UserAgent;
            if (userAgents.Count > 0)
                // We don't support more than one UserAgent value, so return the last one and ignore the rest
                return userAgents.ElementAt(userAgents.Count - 1).ToString();
            return "";
        }

        /// <summary>
        /// Sets the current user agent
        /// </summary>
        /// <param name="userAgent">Target user agent</param>
        public static void HttpSetUserAgent(string userAgent)
        {
            // Remove all user agent strings in case we have more than one instance
            while (HttpHeaderExists("User-Agent"))
                HttpRemoveHeader("User-Agent");

            // Now, set the user agent
            HttpAddHeader("User-Agent", userAgent);
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

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

using System.Net;
using System.Net.NetworkInformation;
using FS = Nitrocid.Base.Files.FilesystemTools;

namespace Nitrocid.Base.Drivers.Network
{
    /// <summary>
    /// Network driver interface for drivers
    /// </summary>
    public interface INetworkDriver : IDriver
    {

        /// <summary>
        /// Checks to see if the network is available
        /// </summary>
        bool NetworkAvailable { get; }

        /// <summary>
        /// Pings an address
        /// </summary>
        /// <param name="Address">Target address</param>
        /// <param name="Timeout">Timeout in milliseconds</param>
        /// <param name="Buffer">The buffer consisting of array of bytes</param>
        /// <returns>A ping reply status</returns>
        PingReply PingAddress(string Address, int Timeout, byte[] Buffer);

        /// <summary>
        /// Gets the filename from the URL
        /// </summary>
        /// <param name="Url">The target URL that contains the filename</param>
        string GetFilenameFromUrl(string Url);

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        bool DownloadFile(string URL);

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        bool DownloadFile(string URL, bool ShowProgress);

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="FileName">File name to download to</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        bool DownloadFile(string URL, string FileName);

        /// <summary>
        /// Downloads a file to the current working directory.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <param name="FileName">File name to download to</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        bool DownloadFile(string URL, bool ShowProgress, string FileName);

        /// <summary>
        /// Uploads a file to the current working directory.
        /// </summary>
        /// <param name="FileName">A target file name. Use <see cref="FS.NeutralizePath(string, bool)"/> to get full path of source.</param>
        /// <param name="URL">A URL to a file</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        bool UploadFile(string FileName, string URL);

        /// <summary>
        /// Uploads a file from the current working directory.
        /// </summary>
        /// <param name="FileName">A target file name. Use <see cref="FS.NeutralizePath(string, bool)"/> to get full path of source.</param>
        /// <param name="URL">A URL</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        bool UploadFile(string FileName, string URL, bool ShowProgress);

        /// <summary>
        /// Downloads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        string DownloadString(string URL);

        /// <summary>
        /// Downloads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>A resource string if successful; Throws exception if unsuccessful.</returns>
        string DownloadString(string URL, bool ShowProgress);

        /// <summary>
        /// Uploads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL to a file</param>
        /// <param name="Data">Content to upload</param>
        /// <returns>True if successful. Throws exception if unsuccessful.</returns>
        bool UploadString(string URL, string Data);

        /// <summary>
        /// Uploads a resource from URL as a string.
        /// </summary>
        /// <param name="URL">A URL</param>
        /// <param name="Data">Content to upload</param>
        /// <param name="ShowProgress">Whether or not to show progress bar</param>
        /// <returns>A resource string if successful; Throws exception if unsuccessful.</returns>
        bool UploadString(string URL, string Data, bool ShowProgress);

        /// <summary>
        /// Gets the online devices in your network, including the router and the broadcast address
        /// </summary>
        IPAddress[] GetOnlineDevicesInNetwork();

    }
}

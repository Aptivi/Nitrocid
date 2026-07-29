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
using System.Reflection;
using System.Text;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Events;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Network.Connections;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using Terminaux.Base.Extensions;
using Terminaux.Inputs;
using Terminaux.Reader;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;
using Textify.Tools.Placeholder;

namespace Nitrocid.ShellPacks.Shells.SFTP.Tools
{
    /// <summary>
    /// SFTP tools module
    /// </summary>
    public static class SFTPTools
    {
        /// <summary>
        /// Lists remote folders and files
        /// </summary>
        /// <param name="client">SFTP client</param>
        /// <param name="Path">Path to folder</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static List<string> SFTPListRemote(SftpClient client, string Path) =>
            SFTPListRemote(client, Path, ShellsInit.ShellsConfig.SFTPShowDetailsInList);

        /// <summary>
        /// Lists remote folders and files
        /// </summary>
        /// <param name="client">SFTP client</param>
        /// <param name="Path">Path to folder</param>
        /// <param name="ShowDetails">Shows the details of the file</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static List<string> SFTPListRemote(SftpClient client, string Path, bool ShowDetails)
        {
            var Entries = new List<string>();

            try
            {
                var EntryBuilder = new StringBuilder();
                IEnumerable<ISftpFile> Listing = client.ListDirectory(Path);
                foreach (ISftpFile DirListSFTP in Listing)
                {
                    EntryBuilder.Append($"- {DirListSFTP.Name}");

                    // Check to see if the file that we're dealing with is a symbolic link
                    if (DirListSFTP.IsSymbolicLink)
                    {
                        EntryBuilder.Append(" >> ");
                        EntryBuilder.Append(SFTPGetCanonicalPath(client, DirListSFTP.FullName));
                    }

                    if (DirListSFTP.IsRegularFile)
                    {
                        EntryBuilder.Append(": ");
                        if (ShowDetails)
                        {
                            long FileSize = DirListSFTP.Length;
                            DateTime ModDate = DirListSFTP.LastWriteTime;
                            EntryBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.ListValue).VTSequenceForeground() + $"{FileSize.SizeString()} | {LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_LSREMOTE_MODIFIED")} {ModDate}");
                        }
                    }
                    else if (DirListSFTP.IsDirectory)
                    {
                        EntryBuilder.Append('/');
                    }
                    Entries.Add(EntryBuilder.ToString());
                    EntryBuilder.Clear();
                }
                return Entries;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.SFTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_LIST_FAILED"), ex, ex.Message);
            }
        }

        /// <summary>
        /// Removes remote file or folder
        /// </summary>
        /// <param name="client">SFTP client</param>
        /// <param name="Target">Target folder or file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SFTPDeleteRemote(SftpClient client, string Target)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Deleting {0}...", vars: [Target]);

            // Delete a file or folder
            if (client.Exists(Target))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Deleting {0}...", vars: [Target]);
                client.Delete(Target);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "{0} is not found.", vars: [Target]);
                throw new KernelException(KernelExceptionType.SFTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_NOTFOUND"), Target);
            }
            DebugWriter.WriteDebug(DebugLevel.I, "Deleted {0}", vars: [Target]);
            return true;
        }

        /// <summary>
        /// Gets the absolute path for the given path
        /// </summary>
        /// <param name="client">SFTP client</param>
        /// <param name="Path">The remote path</param>
        /// <returns>Absolute path for a remote path</returns>
        public static string SFTPGetCanonicalPath(SftpClient client, string Path)
        {
            // GetCanonicalPath was supposed to be public, but it's in a private class called SftpSession. It should be in SftpClient, which is public.
            var SFTPType = client.GetType();
            var SFTPSessionField = SFTPType.GetField("_sftpSession", BindingFlags.Instance | BindingFlags.NonPublic);
            var SFTPSession = SFTPSessionField?.GetValue(client);
            var SFTPSessionType = SFTPSession?.GetType();
            var SFTPSessionCanon = SFTPSessionType?.GetMethod("GetCanonicalPath");
            if (SFTPSessionCanon is null)
                return "";
            string CanonicalPath = Convert.ToString(SFTPSessionCanon.Invoke(SFTPSession, [Path])) ?? "";
            DebugWriter.WriteDebug(DebugLevel.I, "Canonical path: {0}", vars: [CanonicalPath]);
            return CanonicalPath;
        }

        /// <summary>
        /// Makes a directory in the remote
        /// </summary>
        /// <param name="client">SFTP client</param>
        /// <param name="name">New directory name</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SFTPMakeDirectory(SftpClient client, string name)
        {
            try
            {
                client.CreateDirectory(name);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error creating SFTP directory {0}: {1}", vars: [name, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Checks to see if an SFTP file or directory exists
        /// </summary>
        /// <param name="client">SFTP client</param>
        /// <param name="name">Path to file or directory</param>
        /// <returns>True if found; False otherwise</returns>
        public static bool SFTPExists(SftpClient client, string name) =>
            SFTPFileExists(client, name) || SFTPDirectoryExists(client, name);

        /// <summary>
        /// Checks to see if an SFTP file exists
        /// </summary>
        /// <param name="client">SFTP client</param>
        /// <param name="name">Path to file</param>
        /// <returns>True if found; False otherwise</returns>
        public static bool SFTPFileExists(SftpClient client, string name)
        {
            try
            {
                return client.Exists(name) && !client.Get(name).IsDirectory;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error getting file state {0}: {1}", vars: [name, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Checks to see if an SFTP directory exists
        /// </summary>
        /// <param name="client">SFTP client</param>
        /// <param name="name">Path to file</param>
        /// <returns>True if found; False otherwise</returns>
        public static bool SFTPDirectoryExists(SftpClient client, string name)
        {
            try
            {
                return client.Exists(name) && client.Get(name).IsDirectory;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error getting file state {0}: {1}", vars: [name, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Downloads a file from the currently connected SFTP server
        /// </summary>
        /// <param name="client">SFTP client</param>
        /// <param name="File">A remote file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SFTPGetFile(SftpClient client, string File) =>
            SFTPGetFile(client, File, File);

        /// <summary>
        /// Downloads a file from the currently connected SFTP server
        /// </summary>
        /// <param name="client">SFTP client</param>
        /// <param name="File">A remote file</param>
        /// <param name="LocalFile">A name of the local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SFTPGetFile(SftpClient client, string File, string LocalFile)
        {
            try
            {
                // Show a message to download
                EventsManager.FireEvent(EventType.SFTPPreDownload, File);
                DebugWriter.WriteDebug(DebugLevel.I, "Downloading file {0}...", vars: [File]);

                // Try to download
                string LocalFilePath = FilesystemTools.NeutralizePath(LocalFile);
                var DownloadFileStream = new System.IO.FileStream(LocalFilePath, System.IO.FileMode.OpenOrCreate);
                client.DownloadFile(File, DownloadFileStream);

                // Show a message that it's downloaded
                DebugWriter.WriteDebug(DebugLevel.I, "Downloaded file {0}.", vars: [File]);
                EventsManager.FireEvent(EventType.SFTPPostDownload, File);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Download failed for file {0}: {1}", vars: [File, ex.Message]);
                EventsManager.FireEvent(EventType.SFTPDownloadError, File, ex);
            }
            return false;
        }

        /// <summary>
        /// Uploads a file to the currently connected SFTP server
        /// </summary>
        /// <param name="client">SFTP client</param>
        /// <param name="File">A remote file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SFTPUploadFile(SftpClient client, string File) =>
            SFTPUploadFile(client, File, File);

        /// <summary>
        /// Uploads a file to the currently connected SFTP server
        /// </summary>
        /// <param name="client">SFTP client</param>
        /// <param name="File">A remote file</param>
        /// <param name="LocalFile">A name of the local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SFTPUploadFile(SftpClient client, string File, string LocalFile)
        {
            try
            {
                // Show a message to download
                EventsManager.FireEvent(EventType.SFTPPreUpload, File);
                DebugWriter.WriteDebug(DebugLevel.I, "Uploading file {0}...", vars: [File]);

                // Try to upload
                string LocalFilePath = FilesystemTools.NeutralizePath(LocalFile);
                var UploadFileStream = new System.IO.FileStream(LocalFilePath, System.IO.FileMode.Open);
                client.UploadFile(UploadFileStream, File);
                DebugWriter.WriteDebug(DebugLevel.I, "Uploaded file {0}", vars: [File]);
                EventsManager.FireEvent(EventType.SFTPPostUpload, File);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Upload failed for file {0}: {1}", vars: [File, ex.Message]);
                EventsManager.FireEvent(EventType.SFTPUploadError, File, ex);
            }
            return false;
        }

        /// <summary>
        /// Downloads a file to string
        /// </summary>
        /// <param name="client">SFTP client</param>
        /// <param name="File">A text file.</param>
        /// <returns>Contents of the file</returns>
        public static string SFTPDownloadToString(SftpClient client, string File)
        {
            try
            {
                // Show a message to download
                EventsManager.FireEvent(EventType.SFTPPreDownload, File);
                DebugWriter.WriteDebug(DebugLevel.I, "Downloading {0}...", vars: [File]);

                // Try to download 3 times
                var DownloadedBytes = Array.Empty<byte>();
                string DownloadedContent = client.ReadAllText(File);

                // Show a message that it's downloaded
                DebugWriter.WriteDebug(DebugLevel.I, "Downloaded {0}.", vars: [File]);
                EventsManager.FireEvent(EventType.SFTPPostDownload, File, DownloadedContent);
                return DownloadedContent;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Download failed for {0}: {1}", vars: [File, ex.Message]);
                EventsManager.FireEvent(EventType.SFTPPostDownload, File, false);
            }
            return "";
        }

        /// <summary>
        /// Tries to connect to the FTP server
        /// </summary>
        /// <param name="address">An FTP server. You may specify it like "[address]" or "[address]:[port]"</param>
        public static NetworkConnection? SFTPTryToConnect(string address)
        {
            try
            {
                // Create an SFTP stream to connect to
                int indexOfPort = address.LastIndexOf(":");
                string SftpHost = address.Replace("sftp://", "");
                SftpHost = indexOfPort < 0 ? SftpHost : SftpHost.Replace(SftpHost[SftpHost.LastIndexOf(":")..], "");
                string SftpPortString = address.Replace("sftp://", "").Replace(SftpHost + ":", "");
                DebugWriter.WriteDebug(DebugLevel.W, "Host: {0}, Port: {1}", vars: [SftpHost, SftpPortString]);
                bool portParsed = int.TryParse(SftpHost == SftpPortString ? "22" : SftpPortString, out int SftpPort);
                if (!portParsed)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_CORRECTPORTREQUIRED"), true, ThemeColorType.Error);
                    return null;
                }

                // Prompt for username
                if (!string.IsNullOrWhiteSpace(ShellsInit.ShellsConfig.SFTPUserPromptStyle))
                    TextWriterColor.Write(PlaceParse.ProbePlaces(ShellsInit.ShellsConfig.SFTPUserPromptStyle), false, ThemeColorType.Input, address);
                else
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_PROMPTUSERNAME"), false, ThemeColorType.Input, address);
                string sftpUser = TermReader.Read();
                if (string.IsNullOrEmpty(sftpUser))
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "User is not provided. Fallback to \"anonymous\"");
                    sftpUser = "anonymous";
                }

                // Check to see if we're aborting or not
                var client = GetConnectionInfo(SftpHost, Convert.ToInt32(SftpPort), sftpUser);

                // Connect to SFTP
                return ConnectSFTP(client);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Error connecting to {0}: {1}", vars: [address, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_ERRORCONNECTING"), true, ThemeColorType.Error, address, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Prompts the user for the connection info
        /// </summary>
        /// <param name="Address">An IP address or hostname</param>
        /// <param name="Port">A port of the SSH/SFTP server. It's usually 22</param>
        /// <param name="Username">A username to authenticate with</param>
        public static ConnectionInfo PromptConnectionInfo(string Address, int Port, string Username)
        {
            // Authentication
            DebugWriter.WriteDebug(DebugLevel.I, "Address: {0}:{1}, Username: {2}", vars: [Address, Port, Username]);
            var AuthenticationMethods = new List<AuthenticationMethod>();
            int Answer;
            while (true)
            {
                // Ask for authentication method
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_AUTHMETHOD") + CharManager.NewLine, true, ThemeColorType.Question);
                TextWriterColor.Write("1) " + LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_AUTHMETHOD_PRIVATEKEY"), true, ThemeColorType.Option);
                TextWriterColor.Write("2) " + LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_AUTHMETHOD_PASSWORD") + CharManager.NewLine, true, ThemeColorType.Option);
                TextWriterColor.Write(">> ", false, ThemeColorType.Input);
                if (int.TryParse(TermReader.Read(), out Answer))
                {
                    // Check for answer
                    bool exitWhile = false;
                    switch (Answer)
                    {
                        case 1:
                        case 2:
                            exitWhile = true;
                            break;
                        default:
                            DebugWriter.WriteDebug(DebugLevel.W, "Option is not valid. Returning...");
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_INVALIDOPTION"), true, ThemeColorType.Error, Answer);
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_GOBACK"), true, ThemeColorType.Error);
                            Input.ReadKey();
                            break;
                    }

                    if (exitWhile)
                        break;
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Answer is not numeric.");
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_OPTIONNUMERIC"), true, ThemeColorType.Error);
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_GOBACK"), true, ThemeColorType.Error);
                    Input.ReadKey();
                }
            }

            switch (Answer)
            {
                case 1:
                    // Private key file
                    var AuthFiles = new List<PrivateKeyFile>();

                    // Prompt user
                    while (true)
                    {
                        string PrivateKeyFile, PrivateKeyPassphrase;
                        PrivateKeyFile PrivateKeyAuth;

                        // Ask for location
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_AUTHMETHOD_LOCATIONSPROMPT"), false, ThemeColorType.Input, Username);
                        PrivateKeyFile = TermReader.Read();
                        PrivateKeyFile = FilesystemTools.NeutralizePath(PrivateKeyFile);
                        if (FilesystemTools.FileExists(PrivateKeyFile))
                        {
                            // Ask for passphrase
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_AUTHMETHOD_KEYPASSPHRASE"), false, ThemeColorType.Input, PrivateKeyFile);
                            PrivateKeyPassphrase = TermReader.Read(password: true);

                            // Add authentication method
                            try
                            {
                                if (string.IsNullOrEmpty(PrivateKeyPassphrase))
                                    PrivateKeyAuth = new PrivateKeyFile(PrivateKeyFile);
                                else
                                    PrivateKeyAuth = new PrivateKeyFile(PrivateKeyFile, PrivateKeyPassphrase);
                                AuthFiles.Add(PrivateKeyAuth);
                            }
                            catch (Exception ex)
                            {
                                DebugWriter.WriteDebugStackTrace(ex);
                                DebugWriter.WriteDebug(DebugLevel.E, "Error trying to add private key authentication method: {0}", vars: [ex.Message]);
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_AUTHMETHOD_KEYADDFAILED") + " {0}", true, ThemeColorType.Error, ex.Message);
                            }
                        }
                        else if (PrivateKeyFile.EndsWith("/q"))
                            break;
                        else
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTIONINFO_AUTHMETHOD_KEYNOTFOUND"), true, ThemeColorType.Error, PrivateKeyFile);
                    }

                    // Add authentication method
                    AuthenticationMethods.Add(new PrivateKeyAuthenticationMethod(Username, AuthFiles.ToArray()));
                    break;
                case 2:
                    // Password
                    string Pass;

                    // Ask for password
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_PASSWORDPROMPT"), false, ThemeColorType.Input, Username);
                    Pass = TermReader.Read(password: true);

                    // Add authentication method
                    AuthenticationMethods.Add(new PasswordAuthenticationMethod(Username, Pass));
                    break;
            }
            return new(Address, Port, Username, [.. AuthenticationMethods]);
        }

        internal static SftpClient GetConnectionInfo(string SftpHost, int SftpPort, string SftpUser) =>
            new(PromptConnectionInfo(SftpHost, Convert.ToInt32(SftpPort), SftpUser));

        /// <summary>
        /// Tries to connect to the SFTP server.
        /// </summary>
        internal static NetworkConnection ConnectSFTP(SftpClient client)
        {
            // Connect
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_SFTP_CONNECTING"), client.ConnectionInfo.Host);
            DebugWriter.WriteDebug(DebugLevel.I, "Connecting to {0} with {1}...", vars: [client.ConnectionInfo.Host]);
            client.Connect();
            var sftpConnection = NetworkConnectionTools.EstablishConnection("SFTP client", client.ConnectionInfo.Host, NetworkConnectionType.SFTP, client);

            // Show that it's connected
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_CONNECTEDTO"), client.ConnectionInfo.Host);
            DebugWriter.WriteDebug(DebugLevel.I, "Connected.");
            return sftpConnection;
        }

    }
}

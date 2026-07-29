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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using FluentFTP;
using FluentFTP.Client.BaseClient;
using FluentFTP.Helpers;
using Nitrocid.Base.Files;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Events;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Network.Connections;
using Nitrocid.ShellPacks.Shells.FTP.Tools.Transfer;
using Terminaux.Base.Extensions;
using Terminaux.Inputs;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Choice;
using Terminaux.Reader;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;
using Textify.Tools.Placeholder;

namespace Nitrocid.ShellPacks.Shells.FTP.Tools
{
    /// <summary>
    /// FTP tools class
    /// </summary>
    public static class FTPTools
    {
        /// <summary>
        /// Lists remote folders and files
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="Path">Path to folder</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static List<string> FTPListRemote(FtpClient client, string Path) =>
            FTPListRemote(client, Path, ShellsInit.ShellsConfig.FtpShowDetailsInList);

        /// <summary>
        /// Lists remote folders and files
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="Path">Path to folder</param>
        /// <param name="ShowDetails">Shows the details of the file</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static List<string> FTPListRemote(FtpClient client, string Path, bool ShowDetails)
        {
            var EntryBuilder = new StringBuilder();
            var Entries = new List<string>();

            try
            {
                FtpListItem[] Listing = client.GetListing(Path, FtpListOption.Auto);
                foreach (FtpListItem DirListFTP in Listing)
                {
                    FtpListItem finalDirListFTP = DirListFTP;
                    EntryBuilder.Append($"- {finalDirListFTP.Name}");

                    // Check to see if the file that we're dealing with is a symbolic link
                    if (finalDirListFTP.Type == FtpObjectType.Link)
                    {
                        EntryBuilder.Append(" >> ");
                        if (!string.IsNullOrEmpty(finalDirListFTP.LinkTarget))
                            EntryBuilder.Append(finalDirListFTP.LinkTarget);
                        else
                            EntryBuilder.Append(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_FSENTRY_NOSYMLINKINFO"));
                        finalDirListFTP = finalDirListFTP.LinkObject;
                    }

                    if (finalDirListFTP is not null)
                    {
                        if (finalDirListFTP.Type == FtpObjectType.File)
                        {
                            if (ShowDetails)
                            {
                                EntryBuilder.Append(": ");
                                long FileSize = client.GetFileSize(finalDirListFTP.FullName);
                                DateTime ModDate = client.GetModifiedTime(finalDirListFTP.FullName);
                                EntryBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.ListValue).VTSequenceForeground() +
                                    $"{FileSize.SizeString()} | {LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_FSENTRY_MODIFIEDON")} {ModDate}");
                            }
                        }
                        else if (finalDirListFTP.Type == FtpObjectType.Directory)
                        {
                            EntryBuilder.Append('/');
                        }
                    }
                    Entries.Add(EntryBuilder.ToString());
                    EntryBuilder.Clear();
                }
                return Entries;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.FTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_LIST_FAILED"), ex, ex.Message);
            }
        }

        /// <summary>
        /// Removes remote file or folder
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="Target">Target folder or file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPDeleteRemote(FtpClient client, string Target)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Deleting {0}...", vars: [Target]);

            // Delete a file or folder
            if (client.FileExists(Target))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "{0} is a file.", vars: [Target]);
                client.DeleteFile(Target);
            }
            else if (client.DirectoryExists(Target))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "{0} is a folder.", vars: [Target]);
                client.DeleteDirectory(Target);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "{0} is not found.", vars: [Target]);
                throw new KernelException(KernelExceptionType.FTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_NOTFOUND"), Target);
            }
            DebugWriter.WriteDebug(DebugLevel.I, "Deleted {0}", vars: [Target]);
            return true;
        }

        /// <summary>
        /// Move file or directory to another area, or rename the file
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="Source">Source file or folder</param>
        /// <param name="Target">Target file or folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static bool FTPMoveItem(FtpClient client, string Source, string Target)
        {
            var Success = false;

            // Begin the moving process
            string SourceFile = Source.Split('/').Last();
            DebugWriter.WriteDebug(DebugLevel.I, "Moving from {0} to {1} with the source file of {2}...", vars: [Source, Target, SourceFile]);
            if (client.DirectoryExists(Source))
                Success = client.MoveDirectory(Source, Target);
            else if (client.FileExists(Source) & client.DirectoryExists(Target))
                Success = client.MoveFile(Source, Target + SourceFile);
            else if (client.FileExists(Source))
                Success = client.MoveFile(Source, Target);
            DebugWriter.WriteDebug(DebugLevel.I, "Moved. Result: {0}", vars: [Success]);
            return Success;
        }

        /// <summary>
        /// Copy file or directory to another area, or rename the file
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="Source">Source file or folder</param>
        /// <param name="Target">Target file or folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static bool FTPCopyItem(FtpClient client, string Source, string Target)
        {
            bool Success = true;
            object? Result = null;

            // Begin the copying process
            string SourceFile = Source.Split('/').Last();
            DebugWriter.WriteDebug(DebugLevel.I, "Copying from {0} to {1} with the source file of {2}...", vars: [Source, Target, SourceFile]);
            if (client.DirectoryExists(Source))
            {
                client.DownloadDirectory(PathsManagement.TempPath + "/FTPTransfer", Source);
                Result = client.UploadDirectory(PathsManagement.TempPath + "/FTPTransfer/" + Source, Target);
            }
            else if (client.FileExists(Source) & client.DirectoryExists(Target))
            {
                client.DownloadFile(PathsManagement.TempPath + "/FTPTransfer/" + SourceFile, Source);
                Result = client.UploadFile(PathsManagement.TempPath + "/FTPTransfer/" + SourceFile, Target + "/" + SourceFile);
            }
            else if (client.FileExists(Source))
            {
                client.DownloadFile(PathsManagement.TempPath + "/FTPTransfer/" + SourceFile, Source);
                Result = client.UploadFile(PathsManagement.TempPath + "/FTPTransfer/" + SourceFile, Target);
            }
            FilesystemTools.RemoveDirectory(PathsManagement.TempPath + "/FTPTransfer");

            // See if copied successfully
            if (Result is null)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Copied, but result is inconclusive. Assuming failure...");
                return false;
            }
            if (Result.GetType() == typeof(List<FtpResult>))
            {
                foreach (FtpResult FileResult in (IEnumerable)Result)
                {
                    if (FileResult.IsFailed)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Transfer for {0} failed: {1}", vars: [FileResult.Name, FileResult.Exception.Message]);
                        DebugWriter.WriteDebugStackTrace(FileResult.Exception);
                        Success = false;
                    }
                }
            }
            else if (Result.GetType() == typeof(FtpStatus))
            {
                if (((FtpStatus)Convert.ToInt32(Result)).IsFailure())
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Transfer failed");
                    Success = false;
                }
            }
            DebugWriter.WriteDebug(DebugLevel.I, "Copied. Result: {0}", vars: [Success]);
            return Success;
        }

        /// <summary>
        /// Changes the permissions of a remote file
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="Target">Target file</param>
        /// <param name="Chmod">Permissions in CHMOD format. See https://man7.org/linux/man-pages/man2/chmod.2.html chmod(2) for more info.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPChangePermissions(FtpClient client, string Target, int Chmod)
        {
            try
            {
                client.Chmod(Target, Chmod);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error setting permissions ({0}) to file {1}: {2}", vars: [Chmod, Target, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Makes a directory in the remote
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="name">New directory name</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPMakeDirectory(FtpClient client, string name)
        {
            try
            {
                return client.CreateDirectory(name);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error creating FTP directory {0}: {1}", vars: [name, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Checks to see if an FTP file or directory exists
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="name">Path to file or directory</param>
        /// <returns>True if found; False otherwise</returns>
        public static bool FTPExists(FtpClient client, string name) =>
            FTPFileExists(client, name) || FTPDirectoryExists(client, name);

        /// <summary>
        /// Checks to see if an FTP file exists
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="name">Path to file</param>
        /// <returns>True if found; False otherwise</returns>
        public static bool FTPFileExists(FtpClient client, string name)
        {
            try
            {
                return client.FileExists(name);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error getting file state {0}: {1}", vars: [name, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Checks to see if an FTP directory exists
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="name">Path to file</param>
        /// <returns>True if found; False otherwise</returns>
        public static bool FTPDirectoryExists(FtpClient client, string name)
        {
            try
            {
                return client.DirectoryExists(name);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error getting file state {0}: {1}", vars: [name, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Gets a hash for file
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="File">A file to be hashed</param>
        /// <param name="HashAlgorithm">A hash algorithm supported by the FTP server</param>
        /// <returns>The <see cref="FtpHash"/> instance containing computed hash of remote file</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static FtpHash FTPGetHash(FtpClient client, string File, FtpHashAlgorithm HashAlgorithm)
        {
            if (!string.IsNullOrEmpty(File))
            {
                if (client.FileExists(File))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Hashing {0} using {1}...", vars: [File, HashAlgorithm.ToString()]);
                    return client.GetChecksum(File, HashAlgorithm);
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not found.", vars: [File]);
                    throw new KernelException(KernelExceptionType.FTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_NOTFOUND"), File);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.FTPNetwork, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_EXCEPTION_REMOTEFILENEEDED_HASH"));
            }
        }

        /// <summary>
        /// Gets a hash for files in a directory
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="Directory">A directory for its contents to be hashed</param>
        /// <param name="HashAlgorithm">A hash algorithm supported by the FTP server</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static Dictionary<string, FtpHash> FTPGetHashes(FtpClient client, string Directory, FtpHashAlgorithm HashAlgorithm) =>
            FTPGetHashes(client, Directory, HashAlgorithm, ShellsInit.ShellsConfig.FtpRecursiveHashing);

        /// <summary>
        /// Gets a hash for files in a directory
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="Directory">A directory for its contents to be hashed</param>
        /// <param name="HashAlgorithm">A hash algorithm supported by the FTP server</param>
        /// <param name="Recurse">Whether to hash the files within the subdirectories too.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static Dictionary<string, FtpHash> FTPGetHashes(FtpClient client, string Directory, FtpHashAlgorithm HashAlgorithm, bool Recurse)
        {
            if (!string.IsNullOrEmpty(Directory))
            {
                if (client.DirectoryExists(Directory))
                {
                    var Hashes = new Dictionary<string, FtpHash>();
                    FtpListItem[] Items;
                    if (Recurse)
                    {
                        Items = client.GetListing(Directory, FtpListOption.Recursive);
                    }
                    else
                    {
                        Items = client.GetListing(Directory);
                    }
                    foreach (FtpListItem Item in Items)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Hashing {0} using {1}...", vars: [Item.FullName, HashAlgorithm.ToString()]);
                        Hashes.Add(Item.FullName, FTPGetHash(client, Item.FullName, HashAlgorithm));
                    }
                    return Hashes;
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "{0} is not found.", vars: [Directory]);
                    throw new KernelException(KernelExceptionType.FTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_NOTFOUND"), Directory);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.FTPNetwork, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_EXCEPTION_REMOTEDIRECTORYNEEDED"));
            }
        }

        /// <summary>
        /// Downloads a file from the currently connected FTP server
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="File">A remote file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPGetFile(FtpClient client, string File) =>
            FTPGetFile(client, File, File);

        /// <summary>
        /// Downloads a file from the currently connected FTP server
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="File">A remote file</param>
        /// <param name="LocalFile">A name of the local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPGetFile(FtpClient client, string File, string LocalFile)
        {
            try
            {
                // Show a message to download
                EventsManager.FireEvent(EventType.FTPPreDownload, File);
                DebugWriter.WriteDebug(DebugLevel.I, "Downloading file {0}...", vars: [File]);

                // Try to download 3 times
                string LocalFilePath = FilesystemTools.NeutralizePath(LocalFile);
                var Result = client.DownloadFile(LocalFilePath, File, FtpLocalExists.Resume, (FtpVerify)((int)FtpVerify.Retry + (int)FtpVerify.Throw), FTPTransferProgress.FileProgress);

                // Show a message that it's downloaded
                DebugWriter.WriteDebug(DebugLevel.I, "Downloaded file {0}.", vars: [File]);
                EventsManager.FireEvent(EventType.FTPPostDownload, File, Result.IsSuccess());
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Download failed for file {0}: {1}", vars: [File, ex.Message]);
                EventsManager.FireEvent(EventType.FTPPostDownload, File, false);
            }
            return false;
        }

        /// <summary>
        /// Downloads a folder from the currently connected FTP server
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="Folder">A remote folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPGetFolder(FtpClient client, string Folder) =>
            FTPGetFolder(client, Folder, "");

        /// <summary>
        /// Downloads a folder from the currently connected FTP server
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="Folder">A remote folder</param>
        /// <param name="LocalFolder">A local folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPGetFolder(FtpClient client, string Folder, string LocalFolder)
        {
            try
            {
                // Show a message to download
                EventsManager.FireEvent(EventType.FTPPreDownload, Folder);
                DebugWriter.WriteDebug(DebugLevel.I, "Downloading folder {0}...", vars: [Folder]);

                // Try to download folder
                string LocalFolderPath = FilesystemTools.NeutralizePath(LocalFolder);
                var Results = client.DownloadDirectory(LocalFolderPath, Folder, FtpFolderSyncMode.Update, FtpLocalExists.Resume, (FtpVerify)((int)FtpVerify.Retry + (int)FtpVerify.Throw), null, FTPTransferProgress.MultipleProgress);

                // Print download results to debugger
                var Failed = false;
                DebugWriter.WriteDebug(DebugLevel.I, "Folder download result:");
                foreach (FtpResult Result in Results)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "-- {0} --", vars: [Result.Name]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Success: {0}", vars: [Result.IsSuccess]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Skipped: {0}", vars: [Result.IsSkipped]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Failure: {0}", vars: [Result.IsFailed]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Size: {0}", vars: [Result.Size]);
                    DebugWriter.WriteDebug(DebugLevel.I, "Type: {0}", vars: [Result.Type]);
                    if (Result.IsFailed)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Download failed for {0}", vars: [Result.Name]);

                        // Download could fail with no exception in very rare cases.
                        if (Result.Exception is not null)
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Exception {0}", vars: [Result.Exception.Message]);
                            DebugWriter.WriteDebugStackTrace(Result.Exception);
                        }
                        Failed = true;
                    }
                    EventsManager.FireEvent(EventType.FTPPostDownload, Result.Name, !Failed);
                }

                // Show a message that it's downloaded
                if (!Failed)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Downloaded folder {0}.", vars: [Folder]);
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Downloaded folder {0} partially due to failure.", vars: [Folder]);
                }
                EventsManager.FireEvent(EventType.FTPPostDownload, Folder, !Failed);
                return !Failed;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Download failed for folder {0}: {1}", vars: [Folder, ex.Message]);
                EventsManager.FireEvent(EventType.FTPPostDownload, Folder, false);
            }
            return false;
        }

        /// <summary>
        /// Uploads a file to the currently connected FTP server
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="File">A local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPUploadFile(FtpClient client, string File) =>
            FTPUploadFile(client, File, File);

        /// <summary>
        /// Uploads a file to the currently connected FTP server
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="File">A local file</param>
        /// <param name="LocalFile">A name of the local file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPUploadFile(FtpClient client, string File, string LocalFile)
        {
            // Show a message to download
            EventsManager.FireEvent(EventType.FTPPreUpload, File);
            DebugWriter.WriteDebug(DebugLevel.I, "Uploading file {0}...", vars: [LocalFile]);
            DebugWriter.WriteDebug(DebugLevel.I, "Where in the remote: {0}", vars: [File]);

            // Try to upload
            string LocalFilePath = FilesystemTools.NeutralizePath(LocalFile);
            bool Success = Convert.ToBoolean(client.UploadFile(LocalFilePath, File, FtpRemoteExists.Resume, true, FtpVerify.Retry, FTPTransferProgress.FileProgress));
            DebugWriter.WriteDebug(DebugLevel.I, "Uploaded file {0} to {1} with status {2}.", vars: [LocalFile, File, Success]);
            EventsManager.FireEvent(EventType.FTPPostUpload, File, Success);
            return Success;
        }

        /// <summary>
        /// Uploads a folder to the currently connected FTP server
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="Folder">A local folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPUploadFolder(FtpClient client, string Folder) =>
            FTPUploadFolder(client, Folder, Folder);

        /// <summary>
        /// Uploads a folder to the currently connected FTP server
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="Folder">A remote folder</param>
        /// <param name="LocalFolder">A local folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPUploadFolder(FtpClient client, string Folder, string LocalFolder)
        {
            // Show a message to download
            EventsManager.FireEvent(EventType.FTPPreUpload, Folder);
            DebugWriter.WriteDebug(DebugLevel.I, "Uploading folder {0}...", vars: [Folder]);

            // Try to upload
            string LocalFolderPath = FilesystemTools.NeutralizePath(LocalFolder);
            var Results = client.UploadDirectory(LocalFolderPath, Folder, FtpFolderSyncMode.Update, FtpRemoteExists.Resume, FtpVerify.Retry, null, FTPTransferProgress.MultipleProgress);

            // Print upload results to debugger
            var Failed = false;
            DebugWriter.WriteDebug(DebugLevel.I, "Folder upload result:");
            foreach (FtpResult Result in Results)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "-- {0} --", vars: [Result.Name]);
                DebugWriter.WriteDebug(DebugLevel.I, "Success: {0}", vars: [Result.IsSuccess]);
                DebugWriter.WriteDebug(DebugLevel.I, "Skipped: {0}", vars: [Result.IsSkipped]);
                DebugWriter.WriteDebug(DebugLevel.I, "Failure: {0}", vars: [Result.IsFailed]);
                DebugWriter.WriteDebug(DebugLevel.I, "Size: {0}", vars: [Result.Size]);
                DebugWriter.WriteDebug(DebugLevel.I, "Type: {0}", vars: [Result.Type]);
                if (Result.IsFailed)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Upload failed for {0}", vars: [Result.Name]);

                    // Upload could fail with no exception in very rare cases.
                    if (Result.Exception is not null)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Exception {0}", vars: [Result.Exception.Message]);
                        DebugWriter.WriteDebugStackTrace(Result.Exception);
                    }
                    Failed = true;
                }
                EventsManager.FireEvent(EventType.FTPPostUpload, Result.Name, !Failed);
            }

            // Show a message that it's downloaded
            if (!Failed)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Uploaded folder {0}.", vars: [Folder]);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Uploaded folder {0} partially due to failure.", vars: [Folder]);
            }
            EventsManager.FireEvent(EventType.FTPPostUpload, Folder, !Failed);
            return !Failed;
        }

        /// <summary>
        /// Downloads a file to string
        /// </summary>
        /// <param name="client">FTP client</param>
        /// <param name="File">A text file.</param>
        /// <returns>Contents of the file</returns>
        public static string FTPDownloadToString(FtpClient client, string File)
        {
            try
            {
                // Show a message to download
                EventsManager.FireEvent(EventType.FTPPreDownload, File);
                DebugWriter.WriteDebug(DebugLevel.I, "Downloading {0}...", vars: [File]);

                // Try to download 3 times
                var DownloadedBytes = Array.Empty<byte>();
                var DownloadedContent = new StringBuilder();
                bool Downloaded = client.DownloadBytes(out DownloadedBytes, File);
                foreach (byte DownloadedByte in DownloadedBytes)
                    DownloadedContent.Append(Convert.ToChar(DownloadedByte));

                // Show a message that it's downloaded
                DebugWriter.WriteDebug(DebugLevel.I, "Downloaded {0}.", vars: [File]);
                EventsManager.FireEvent(EventType.FTPPostDownload, File, Downloaded);
                return DownloadedContent.ToString();
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebugStackTrace(ex);
                DebugWriter.WriteDebug(DebugLevel.E, "Download failed for {0}: {1}", vars: [File, ex.Message]);
                EventsManager.FireEvent(EventType.FTPPostDownload, File, false);
                throw new KernelException(KernelExceptionType.FTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_GET_FAILED") + " {1}", File, ex.Message);
            }
        }

        /// <summary>
        /// Prompts user for a password
        /// </summary>
        /// <param name="clientFTP">FTP client</param>
        /// <param name="user">A user name</param>
        /// <param name="Address">A host address</param>
        /// <param name="Port">A port for the address</param>
        /// <param name="EncryptionMode">FTP encryption mode</param>
        public static NetworkConnection? PromptForPassword(FtpClient? clientFTP, string user, string Address = "", int Port = 0, FtpEncryptionMode EncryptionMode = FtpEncryptionMode.Explicit)
        {
            // Make a new FTP client object instance (Used in case logging in using speed dial)
            if (clientFTP is null)
            {
                var ftpConfig = new FtpConfig()
                {
                    RetryAttempts = ShellsInit.ShellsConfig.FtpVerifyRetryAttempts,
                    ConnectTimeout = ShellsInit.ShellsConfig.FtpConnectTimeout,
                    DataConnectionConnectTimeout = ShellsInit.ShellsConfig.FtpDataConnectTimeout,
                    EncryptionMode = EncryptionMode,
                    InternetProtocolVersions = (FtpIpVersion)ShellsInit.ShellsConfig.FtpProtocolVersions
                };
                clientFTP = new FtpClient()
                {
                    Host = Address,
                    Port = Port,
                    Config = ftpConfig
                };
            }

            // Prompt for password
            if (!string.IsNullOrWhiteSpace(ShellsInit.ShellsConfig.FtpPassPromptStyle))
                TextWriterColor.Write(PlaceParse.ProbePlaces(ShellsInit.ShellsConfig.FtpPassPromptStyle), false, ThemeColorType.Input, user);
            else
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_PROMPTPASSWORD"), false, ThemeColorType.Input, user);

            // Get input
            string ftpPass = TermReader.Read(password: true);

            // Set up credentials
            clientFTP.Credentials = new NetworkCredential(user, ftpPass);

            // Connect to FTP
            return ConnectFTP(clientFTP);
        }

        /// <summary>
        /// Tries to connect to the FTP server
        /// </summary>
        /// <param name="address">An FTP server. You may specify it like "[address]" or "[address]:[port]"</param>
        public static NetworkConnection? TryToConnect(string address)
        {
            try
            {
                // Create an FTP stream to connect to
                int indexOfPort = address.LastIndexOf(":");
                string FtpHost = address.Replace("ftpes://", "").Replace("ftps://", "").Replace("ftp://", "");
                FtpHost = indexOfPort < 0 ? FtpHost : FtpHost.Replace(FtpHost[FtpHost.LastIndexOf(":")..], "");
                string FtpPortString = address.Replace("ftpes://", "").Replace("ftps://", "").Replace("ftp://", "").Replace(FtpHost + ":", "");
                DebugWriter.WriteDebug(DebugLevel.W, "Host: {0}, Port: {1}", vars: [FtpHost, FtpPortString]);
                bool portParsed = int.TryParse(FtpHost == FtpPortString ? "0" : FtpPortString, out int FtpPort);
                if (!portParsed)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_CORRECTPORTREQUIRED"), true, ThemeColorType.Error);
                    return null;
                }

                // Make a new FTP client object instance
                FtpConfig ftpConfig = new()
                {
                    RetryAttempts = ShellsInit.ShellsConfig.FtpVerifyRetryAttempts,
                    ConnectTimeout = ShellsInit.ShellsConfig.FtpConnectTimeout,
                    DataConnectionConnectTimeout = ShellsInit.ShellsConfig.FtpDataConnectTimeout,
                    EncryptionMode = FtpEncryptionMode.Auto,
                    InternetProtocolVersions = (FtpIpVersion)ShellsInit.ShellsConfig.FtpProtocolVersions
                };
                FtpClient _clientFTP = new()
                {
                    Host = FtpHost,
                    Port = FtpPort,
                    Config = ftpConfig
                };

                // Add handler for SSL validation
                if (ShellsInit.ShellsConfig.FtpTryToValidateCertificate)
                    _clientFTP.ValidateCertificate += new FtpSslValidation(TryToValidate);

                // Prompt for username
                if (!string.IsNullOrWhiteSpace(ShellsInit.ShellsConfig.FtpUserPromptStyle))
                    TextWriterColor.Write(PlaceParse.ProbePlaces(ShellsInit.ShellsConfig.FtpUserPromptStyle), false, ThemeColorType.Input, address);
                else
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_PROMPTUSERNAME"), false, ThemeColorType.Input, address);
                string ftpUser = TermReader.Read();
                if (string.IsNullOrEmpty(ftpUser))
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "User is not provided. Fallback to \"anonymous\"");
                    ftpUser = "anonymous";
                }

                // If we didn't abort, prompt for password
                return PromptForPassword(_clientFTP, ftpUser);
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
        /// Tries to connect to the FTP server.
        /// </summary>
        private static NetworkConnection? ConnectFTP(FtpClient clientFTP)
        {
            // Prepare profiles
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_PREPARINGPROFILES"));
            var profiles = clientFTP.AutoDetect(ShellsInit.ShellsConfig.FtpFirstProfileOnly);
            var profsel = new FtpProfile();
            DebugWriter.WriteDebug(DebugLevel.I, "Profile count: {0}", vars: [profiles.Count]);
            if (profiles.Count > 1)
            {
                // More than one profile
                if (ShellsInit.ShellsConfig.FtpUseFirstProfile)
                    profsel = profiles[0];
                else
                {
                    string profanswer;
                    var profanswered = false;
                    List<InputChoiceInfo> choices = [];
                    for (int i = 0; i <= profiles.Count - 1; i++)
                    {
                        var profile = profiles[i];
                        choices.Add(
                            new InputChoiceInfo($"{i + 1}", $"{profile.Host}, {profile.Credentials.UserName}, {profile.DataConnection}, {profile.Encoding.EncodingName}, {profile.Encryption}, {profile.Protocols}")
                        );
                    }
                    while (!profanswered)
                    {
                        profanswer = ChoiceStyle.PromptChoice(
                            LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_CONNECT_LISTPROFILE_PROMPT") +
                            "\n###: {0}, {1}, {2}, {3}, {4}, {5}".FormatString(
                                LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_CONNECT_LISTPROFILE_HOSTNAME"),
                                LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_CONNECT_LISTPROFILE_USERNAME"),
                                LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_CONNECT_LISTPROFILE_DATATYPE"),
                                LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_CONNECT_LISTPROFILE_ENCODING"),
                                LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_CONNECT_LISTPROFILE_ENCRYPTION"),
                                LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_CONNECT_LISTPROFILE_PROTOCOLS")
                            ), [.. choices], new()
                            {
                                OutputType = ChoiceOutputType.Modern
                            });
                        DebugWriter.WriteDebug(DebugLevel.I, "Selection: {0}", vars: [profanswer]);
                        if (profanswer.IsStringNumeric())
                        {
                            try
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Profile selected");
                                int AnswerNumber = Convert.ToInt32(profanswer);
                                profsel = profiles[AnswerNumber - 1];
                                profanswered = true;
                            }
                            catch (Exception ex)
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Profile invalid");
                                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_INVALIDPROFILE") + CharManager.NewLine, true, ThemeColorType.Error);
                                DebugWriter.WriteDebugStackTrace(ex);
                            }
                        }
                    }
                }
            }
            else if (profiles.Count == 1)
                // Select first profile
                profsel = profiles[0];
            else
            {
                // Failed trying to get profiles
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_NOPROFILESORTIMEOUT"), true, ThemeColorType.Error, clientFTP.Host);
                return null;
            }

            // Connect
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_CONNECTING"), clientFTP.Host, profiles.IndexOf(profsel));
            DebugWriter.WriteDebug(DebugLevel.I, "Connecting to {0} with {1}...", vars: [clientFTP.Host, profiles.IndexOf(profsel)]);
            clientFTP.Connect(profsel);
            var ftpConnection = NetworkConnectionTools.EstablishConnection("FTP connection", clientFTP.Host, NetworkConnectionType.FTP, clientFTP);

            // Show that it's connected
            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_CONNECTEDTO"), true, ThemeColorType.Success, clientFTP.Host);
            DebugWriter.WriteDebug(DebugLevel.I, "Connected.");
            return ftpConnection;
        }

        /// <summary>
        /// Tries to validate certificate
        /// </summary>
        public static void TryToValidate(BaseFtpClient control, FtpSslValidationEventArgs e)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Certificate checks");
            if (e.PolicyErrors == SslPolicyErrors.None)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Certificate accepted.");
                DebugWriter.WriteDebug(DebugLevel.I, e.Certificate.GetRawCertDataString());
                e.Accept = true;
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, $"Certificate error is {e.PolicyErrors}");
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_VALIDATIONFAILED_MESSAGE"), true, ThemeColorType.Error);
                TextWriterColor.Write("- {0}", true, ThemeColorType.Error, e.PolicyErrors.ToString());
                if (ShellsInit.ShellsConfig.FtpAlwaysAcceptInvalidCerts)
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Certificate accepted, although there are errors.");
                    DebugWriter.WriteDebug(DebugLevel.I, e.Certificate.GetRawCertDataString());
                    e.Accept = true;
                }
                else
                {
                    string Answer = "";
                    while (!Answer.Equals("y", StringComparison.OrdinalIgnoreCase) || !Answer.Equals("n", StringComparison.OrdinalIgnoreCase))
                    {
                        TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_VALIDATIONFAILED_PROMPT") + " (y/n) ", false, ThemeColorType.Question);
                        ConsoleColoring.SetConsoleColor(ThemeColorsTools.GetColor(ThemeColorType.Input));
                        Answer = Convert.ToString(Input.ReadKey().KeyChar);
                        TextWriterRaw.Write();
                        DebugWriter.WriteDebug(DebugLevel.I, $"Answer is {Answer}");
                        if (Answer.Equals("y", StringComparison.OrdinalIgnoreCase))
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Certificate accepted, although there are errors.");
                            DebugWriter.WriteDebug(DebugLevel.I, e.Certificate.GetRawCertDataString());
                            e.Accept = true;
                        }
                        else if (!Answer.Equals("n", StringComparison.OrdinalIgnoreCase))
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Invalid answer.");
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_VALIDATIONFAILED_INVALID"), true, ThemeColorType.Error);
                        }
                    }
                }
            }
        }
    }
}

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
using System.Reflection;
using System.Text;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Extras.SftpShell.SFTP;
using Nitrocid.Files;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Misc.Reflection;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using Terminaux.Colors;

namespace Nitrocid.Extras.SftpShell.Tools.Filesystem
{
    static class SFTPFilesystem
    {

        /// <summary>
        /// Lists remote folders and files
        /// </summary>
        /// <param name="Path">Path to folder</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static List<string> SFTPListRemote(string Path) => SFTPListRemote(Path, SFTPShellCommon.SFTPShowDetailsInList);

        /// <summary>
        /// Lists remote folders and files
        /// </summary>
        /// <param name="Path">Path to folder</param>
        /// <param name="ShowDetails">Shows the details of the file</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static List<string> SFTPListRemote(string Path, bool ShowDetails)
        {
            var EntryBuilder = new StringBuilder();
            var Entries = new List<string>();
            long FileSize;
            DateTime ModDate;
            IEnumerable<ISftpFile> Listing;

            try
            {
                var client = (SftpClient?)SFTPShellCommon.ClientSFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.SFTPShell, Translate.DoTranslation("Client is not connected yet"));
                if (!string.IsNullOrEmpty(Path))
                    Listing = client.ListDirectory(Path);
                else
                    Listing = client.ListDirectory(SFTPShellCommon.SFTPCurrentRemoteDir ?? "");
                foreach (ISftpFile DirListSFTP in Listing)
                {
                    EntryBuilder.Append($"- {DirListSFTP.Name}");

                    // Check to see if the file that we're dealing with is a symbolic link
                    if (DirListSFTP.IsSymbolicLink)
                    {
                        EntryBuilder.Append(" >> ");
                        EntryBuilder.Append(SFTPGetCanonicalPath(DirListSFTP.FullName));
                    }

                    if (DirListSFTP.IsRegularFile)
                    {
                        EntryBuilder.Append(": ");
                        if (ShowDetails)
                        {
                            FileSize = DirListSFTP.Length;
                            ModDate = DirListSFTP.LastWriteTime;
                            EntryBuilder.Append(KernelColorTools.GetColor(KernelColorType.ListValue).VTSequenceForeground() + $"{FileSize.SizeString()} | {Translate.DoTranslation("Modified:")} {ModDate}");
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
                throw new KernelException(KernelExceptionType.SFTPFilesystem, Translate.DoTranslation("Failed to list remote files: {0}"), ex, ex.Message);
            }
        }

        /// <summary>
        /// Removes remote file or folder
        /// </summary>
        /// <param name="Target">Target folder or file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SFTPDeleteRemote(string Target)
        {
            var client = (SftpClient?)SFTPShellCommon.ClientSFTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.SFTPShell, Translate.DoTranslation("Client is not connected yet"));
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
                throw new KernelException(KernelExceptionType.SFTPFilesystem, Translate.DoTranslation("{0} is not found in the server."), Target);
            }
            DebugWriter.WriteDebug(DebugLevel.I, "Deleted {0}", vars: [Target]);
            return true;
        }

        /// <summary>
        /// Changes FTP remote directory
        /// </summary>
        /// <param name="Directory">Remote directory</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool SFTPChangeRemoteDir(string Directory)
        {
            var client = (SftpClient?)SFTPShellCommon.ClientSFTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.SFTPShell, Translate.DoTranslation("Client is not connected yet"));
            if (!string.IsNullOrEmpty(Directory))
            {
                if (client.Exists(Directory))
                {
                    // Directory exists, go to the new directory
                    client.ChangeDirectory(Directory);
                    SFTPShellCommon.SFTPCurrentRemoteDir = client.WorkingDirectory;
                    return true;
                }
                else
                {
                    // Directory doesn't exist, go to the old directory
                    throw new KernelException(KernelExceptionType.SFTPFilesystem, Translate.DoTranslation("Directory {0} not found."), Directory);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.SFTPFilesystem, Translate.DoTranslation("Enter a remote directory. \"..\" to go back"));
            }
        }

        /// <summary>
        /// Changes FTP local directory
        /// </summary>
        /// <param name="Directory">Local directory</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool SFTPChangeLocalDir(string Directory)
        {
            string targetDir;
            targetDir = FilesystemTools.NeutralizePath(Directory, SFTPShellCommon.SFTPCurrDirect);

            // Check if folder exists
            if (FilesystemTools.FolderExists(targetDir))
            {
                // Parse written directory
                var parser = new System.IO.DirectoryInfo(targetDir);
                SFTPShellCommon.SFTPCurrDirect = parser.FullName;
                return true;
            }
            else
            {
                throw new KernelException(KernelExceptionType.SFTPFilesystem, Translate.DoTranslation("Local directory {0} doesn't exist."), Directory);
            }
        }

        /// <summary>
        /// Gets the absolute path for the given path
        /// </summary>
        /// <param name="Path">The remote path</param>
        /// <returns>Absolute path for a remote path</returns>
        public static string SFTPGetCanonicalPath(string Path)
        {
            var client = (SftpClient?)SFTPShellCommon.ClientSFTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.SFTPShell, Translate.DoTranslation("Client is not connected yet"));

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
        /// <param name="name">New directory name</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool SFTPMakeDirectory(string name)
        {
            try
            {
                var client = (SftpClient?)SFTPShellCommon.ClientSFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.SFTPShell, Translate.DoTranslation("Client is not connected yet"));
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
        /// <param name="name">Path to file or directory</param>
        /// <returns>True if found; False otherwise</returns>
        public static bool SFTPExists(string name) =>
            SFTPFileExists(name) || SFTPDirectoryExists(name);

        /// <summary>
        /// Checks to see if an SFTP file exists
        /// </summary>
        /// <param name="name">Path to file</param>
        /// <returns>True if found; False otherwise</returns>
        public static bool SFTPFileExists(string name)
        {
            try
            {
                var client = (SftpClient?)SFTPShellCommon.ClientSFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.SFTPShell, Translate.DoTranslation("Client is not connected yet"));
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
        /// <param name="name">Path to file</param>
        /// <returns>True if found; False otherwise</returns>
        public static bool SFTPDirectoryExists(string name)
        {
            try
            {
                var client = (SftpClient?)SFTPShellCommon.ClientSFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.SFTPShell, Translate.DoTranslation("Client is not connected yet"));
                return client.Exists(name) && client.Get(name).IsDirectory;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error getting file state {0}: {1}", vars: [name, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }
    }
}

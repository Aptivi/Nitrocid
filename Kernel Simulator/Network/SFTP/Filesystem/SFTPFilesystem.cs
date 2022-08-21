using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;

// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using KS.Files.Querying;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.Shells.SFTP;

namespace KS.Network.SFTP.Filesystem
{
    static class SFTPFilesystem
    {

        /// <summary>
        /// Lists remote folders and files
        /// </summary>
        /// <param name="Path">Path to folder</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="Exceptions.SFTPFilesystemException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static List<string> SFTPListRemote(string Path)
        {
            return SFTPListRemote(Path, SFTPShellCommon.SFTPShowDetailsInList);
        }

        /// <summary>
        /// Lists remote folders and files
        /// </summary>
        /// <param name="Path">Path to folder</param>
        /// <param name="ShowDetails">Shows the details of the file</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="Exceptions.SFTPFilesystemException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static List<string> SFTPListRemote(string Path, bool ShowDetails)
        {
            if (SFTPShellCommon.SFTPConnected)
            {
                var EntryBuilder = new StringBuilder();
                var Entries = new List<string>();
                long FileSize;
                DateTime ModDate;
                IEnumerable<Renci.SshNet.Sftp.SftpFile> Listing;

                try
                {
                    if (!string.IsNullOrEmpty(Path))
                    {
                        Listing = SFTPShellCommon.ClientSFTP.ListDirectory(Path);
                    }
                    else
                    {
                        Listing = SFTPShellCommon.ClientSFTP.ListDirectory(SFTPShellCommon.SFTPCurrentRemoteDir);
                    }
                    foreach (Renci.SshNet.Sftp.SftpFile DirListSFTP in Listing)
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
                                EntryBuilder.Append(ColorTools.ListValueColor.VTSequenceForeground + Translate.DoTranslation("{0} KB | Modified in: {1}").FormatString((FileSize / 1024d).ToString("N2"), ModDate.ToString()));
                            }
                        }
                        else if (DirListSFTP.IsDirectory)
                        {
                            EntryBuilder.Append("/");
                        }
                        Entries.Add(EntryBuilder.ToString());
                        EntryBuilder.Clear();
                    }
                    return Entries;
                }
                catch (Exception ex)
                {
                    DebugWriter.WStkTrc(ex);
                    throw new Kernel.Exceptions.SFTPFilesystemException(Translate.DoTranslation("Failed to list remote files: {0}"), ex, ex.Message);
                }
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("You should connect to server before listing all remote files."));
            }
            return null;
        }

        /// <summary>
        /// Removes remote file or folder
        /// </summary>
        /// <param name="Target">Target folder or file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="Exceptions.SFTPFilesystemException"></exception>
        public static bool SFTPDeleteRemote(string Target)
        {
            if (SFTPShellCommon.SFTPConnected)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Deleting {0}...", Target);

                // Delete a file or folder
                if (SFTPShellCommon.ClientSFTP.Exists(Target))
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Deleting {0}...", Target);
                    SFTPShellCommon.ClientSFTP.Delete(Target);
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.E, "{0} is not found.", Target);
                    throw new Kernel.Exceptions.SFTPFilesystemException(Translate.DoTranslation("{0} is not found in the server."), Target);
                    return false;
                }
                DebugWriter.Wdbg(DebugLevel.I, "Deleted {0}", Target);
                return true;
            }
            else
            {
                throw new Kernel.Exceptions.SFTPFilesystemException(Translate.DoTranslation("You must connect to server with administrative privileges before performing the deletion."));
            }
            return false;
        }

        /// <summary>
        /// Changes FTP remote directory
        /// </summary>
        /// <param name="Directory">Remote directory</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="Exceptions.SFTPFilesystemException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool SFTPChangeRemoteDir(string Directory)
        {
            if (SFTPShellCommon.SFTPConnected == true)
            {
                if (!string.IsNullOrEmpty(Directory))
                {
                    if (SFTPShellCommon.ClientSFTP.Exists(Directory))
                    {
                        // Directory exists, go to the new directory
                        SFTPShellCommon.ClientSFTP.ChangeDirectory(Directory);
                        SFTPShellCommon.SFTPCurrentRemoteDir = SFTPShellCommon.ClientSFTP.WorkingDirectory;
                        return true;
                    }
                    else
                    {
                        // Directory doesn't exist, go to the old directory
                        throw new Kernel.Exceptions.SFTPFilesystemException(Translate.DoTranslation("Directory {0} not found."), Directory);
                    }
                }
                else
                {
                    throw new ArgumentNullException(Directory, Translate.DoTranslation("Enter a remote directory. \"..\" to go back"));
                }
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("You must connect to a server before changing directory"));
            }
            return false;
        }

        public static bool SFTPChangeLocalDir(string Directory)
        {
            if (!string.IsNullOrEmpty(Directory))
            {
                string targetDir;
                targetDir = $"{SFTPShellCommon.SFTPCurrDirect}/{Directory}";
                Files.Filesystem.ThrowOnInvalidPath(targetDir);

                // Check if folder exists
                if (Checking.FolderExists(targetDir))
                {
                    // Parse written directory
                    var parser = new System.IO.DirectoryInfo(targetDir);
                    SFTPShellCommon.SFTPCurrDirect = parser.FullName;
                    return true;
                }
                else
                {
                    throw new Kernel.Exceptions.SFTPFilesystemException(Translate.DoTranslation("Local directory {0} doesn't exist."), Directory);
                }
            }
            else
            {
                throw new ArgumentNullException(Directory, Translate.DoTranslation("Enter a local directory. \"..\" to go back."));
            }
            return false;
        }

        /// <summary>
        /// Gets the absolute path for the given path
        /// </summary>
        /// <param name="Path">The remote path</param>
        /// <returns>Absolute path for a remote path</returns>
        public static string SFTPGetCanonicalPath(string Path)
        {
            if (SFTPShellCommon.SFTPConnected)
            {
                // GetCanonicalPath was supposed to be public, but it's in a private class called SftpSession. It should be in SftpClient, which is public.
                var SFTPType = SFTPShellCommon.ClientSFTP.GetType();
                var SFTPSessionField = SFTPType.GetField("_sftpSession", BindingFlags.Instance | BindingFlags.NonPublic);
                var SFTPSession = SFTPSessionField.GetValue(SFTPShellCommon.ClientSFTP);
                var SFTPSessionType = SFTPSession.GetType();
                var SFTPSessionCanon = SFTPSessionType.GetMethod("GetCanonicalPath");
                string CanonicalPath = Convert.ToString(SFTPSessionCanon.Invoke(SFTPSession, new string[] { Path }));
                DebugWriter.Wdbg(DebugLevel.I, "Canonical path: {0}", CanonicalPath);
                return CanonicalPath;
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("You must connect to server before performing filesystem operations."));
            }
        }

    }
}

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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentFTP;
using FluentFTP.Helpers;
using KS.ConsoleBase.Colors;
using KS.Files;
using KS.Files.Operations.Querying;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Misc.Reflection;
using KS.Shell.Shells.FTP;

namespace KS.Network.FTP.Filesystem
{
    /// <summary>
    /// FTP filesystem tools module
    /// </summary>
    public static class FTPFilesystem
    {

        /// <summary>
        /// Lists remote folders and files
        /// </summary>
        /// <param name="Path">Path to folder</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static List<string> FTPListRemote(string Path) => FTPListRemote(Path, FTPShellCommon.FtpShowDetailsInList);

        /// <summary>
        /// Lists remote folders and files
        /// </summary>
        /// <param name="Path">Path to folder</param>
        /// <param name="ShowDetails">Shows the details of the file</param>
        /// <returns>The list if successful; null if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static List<string> FTPListRemote(string Path, bool ShowDetails)
        {
            var EntryBuilder = new StringBuilder();
            var Entries = new List<string>();
            long FileSize;
            DateTime ModDate;
            FtpListItem[] Listing;

            try
            {
                if (!string.IsNullOrEmpty(Path))
                {
                    Listing = ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).GetListing(Path, FtpListOption.Auto);
                }
                else
                {
                    Listing = ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).GetListing(FTPShellCommon.FtpCurrentRemoteDir, FtpListOption.Auto);
                }
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
                            EntryBuilder.Append(Translate.DoTranslation("No symlink info"));
                        finalDirListFTP = finalDirListFTP.LinkObject;
                    }

                    if (finalDirListFTP is not null)
                    {
                        if (finalDirListFTP.Type == FtpObjectType.File)
                        {
                            if (ShowDetails)
                            {
                                EntryBuilder.Append(": ");
                                FileSize = ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).GetFileSize(finalDirListFTP.FullName);
                                ModDate = ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).GetModifiedTime(finalDirListFTP.FullName);
                                EntryBuilder.Append(KernelColorTools.GetColor(KernelColorType.ListValue).VTSequenceForeground +
                                    $"{FileSize.SizeString()} | {Translate.DoTranslation("Modified on")} {ModDate}");
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
                throw new KernelException(KernelExceptionType.FTPFilesystem, Translate.DoTranslation("Failed to list remote files: {0}"), ex, ex.Message);
            }
        }

        /// <summary>
        /// Removes remote file or folder
        /// </summary>
        /// <param name="Target">Target folder or file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPDeleteRemote(string Target)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Deleting {0}...", Target);

            // Delete a file or folder
            if (((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).FileExists(Target))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "{0} is a file.", Target);
                ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).DeleteFile(Target);
            }
            else if (((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).DirectoryExists(Target))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "{0} is a folder.", Target);
                ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).DeleteDirectory(Target);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "{0} is not found.", Target);
                throw new KernelException(KernelExceptionType.FTPFilesystem, Translate.DoTranslation("{0} is not found in the server."), Target);
            }
            DebugWriter.WriteDebug(DebugLevel.I, "Deleted {0}", Target);
            return true;
        }

        /// <summary>
        /// Changes FTP remote directory
        /// </summary>
        /// <param name="Directory">Remote directory</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool FTPChangeRemoteDir(string Directory)
        {
            if (!string.IsNullOrEmpty(Directory))
            {
                if (((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).DirectoryExists(Directory))
                {
                    // Directory exists, go to the new directory
                    ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).SetWorkingDirectory(Directory);
                    FTPShellCommon.FtpCurrentRemoteDir = ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).GetWorkingDirectory();
                    return true;
                }
                else
                {
                    // Directory doesn't exist, go to the old directory
                    throw new KernelException(KernelExceptionType.FTPFilesystem, Translate.DoTranslation("Directory {0} not found."), Directory);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.FTPFilesystem, Translate.DoTranslation("Enter a remote directory. \"..\" to go back"));
            }
        }

        /// <summary>
        /// Change the local directory
        /// </summary>
        /// <param name="Directory">Local directory to change to</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool FTPChangeLocalDir(string Directory)
        {
            if (!string.IsNullOrEmpty(Directory))
            {
                string targetDir;
                targetDir = $"{FTPShellCommon.FtpCurrentDirectory}/{Directory}";
                Files.FilesystemTools.ThrowOnInvalidPath(targetDir);

                // Check if folder exists
                if (Checking.FolderExists(targetDir))
                {
                    // Parse written directory
                    var parser = new DirectoryInfo(targetDir);
                    FTPShellCommon.FtpCurrentDirectory = parser.FullName;
                    return true;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.FTPFilesystem, Translate.DoTranslation("Local directory {0} doesn't exist."), Directory);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.FTPFilesystem, Translate.DoTranslation("Enter a local directory. \"..\" to go back."));
            }
        }

        /// <summary>
        /// Move file or directory to another area, or rename the file
        /// </summary>
        /// <param name="Source">Source file or folder</param>
        /// <param name="Target">Target file or folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static bool FTPMoveItem(string Source, string Target)
        {
            var Success = false;

            // Begin the moving process
            string SourceFile = Source.Split('/').Last();
            DebugWriter.WriteDebug(DebugLevel.I, "Moving from {0} to {1} with the source file of {2}...", Source, Target, SourceFile);
            if (((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).DirectoryExists(Source))
                Success = ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).MoveDirectory(Source, Target);
            else if (((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).FileExists(Source) & ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).DirectoryExists(Target))
                Success = ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).MoveFile(Source, Target + SourceFile);
            else if (((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).FileExists(Source))
                Success = ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).MoveFile(Source, Target);
            DebugWriter.WriteDebug(DebugLevel.I, "Moved. Result: {0}", Success);
            return Success;
        }

        /// <summary>
        /// Copy file or directory to another area, or rename the file
        /// </summary>
        /// <param name="Source">Source file or folder</param>
        /// <param name="Target">Target file or folder</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static bool FTPCopyItem(string Source, string Target)
        {
            bool Success = true;
            object Result = null;

            // Begin the copying process
            string SourceFile = Source.Split('/').Last();
            DebugWriter.WriteDebug(DebugLevel.I, "Copying from {0} to {1} with the source file of {2}...", Source, Target, SourceFile);
            if (((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).DirectoryExists(Source))
            {
                ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).DownloadDirectory(Paths.TempPath + "/FTPTransfer", Source);
                Result = ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).UploadDirectory(Paths.TempPath + "/FTPTransfer/" + Source, Target);
            }
            else if (((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).FileExists(Source) & ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).DirectoryExists(Target))
            {
                ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).DownloadFile(Paths.TempPath + "/FTPTransfer/" + SourceFile, Source);
                Result = ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).UploadFile(Paths.TempPath + "/FTPTransfer/" + SourceFile, Target + "/" + SourceFile);
            }
            else if (((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).FileExists(Source))
            {
                ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).DownloadFile(Paths.TempPath + "/FTPTransfer/" + SourceFile, Source);
                Result = ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).UploadFile(Paths.TempPath + "/FTPTransfer/" + SourceFile, Target);
            }
            Directory.Delete(Paths.TempPath + "/FTPTransfer", true);

            // See if copied successfully
            if (Result.GetType() == typeof(List<FtpResult>))
            {
                foreach (FtpResult FileResult in (IEnumerable)Result)
                {
                    if (FileResult.IsFailed)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Transfer for {0} failed: {1}", FileResult.Name, FileResult.Exception.Message);
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
            DebugWriter.WriteDebug(DebugLevel.I, "Copied. Result: {0}", Success);
            return Success;
        }

        /// <summary>
        /// Changes the permissions of a remote file
        /// </summary>
        /// <param name="Target">Target file</param>
        /// <param name="Chmod">Permissions in CHMOD format. See https://man7.org/linux/man-pages/man2/chmod.2.html chmod(2) for more info.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPChangePermissions(string Target, int Chmod)
        {
            try
            {
                ((FtpClient)FTPShellCommon.ClientFTP.ConnectionInstance).Chmod(Target, Chmod);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Error setting permissions ({0}) to file {1}: {2}", Chmod, Target, ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

    }
}

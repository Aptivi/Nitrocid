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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentFTP;
using FluentFTP.Helpers;
using Terminaux.Colors.Themes.Colors;
using Nitrocid.Base.Files;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.ShellPacks.Shells.FTP;

namespace Nitrocid.ShellPacks.Tools.Filesystem
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
        public static List<string> FTPListRemote(string Path) =>
            FTPListRemote(Path, ShellsInit.ShellsConfig.FtpShowDetailsInList);

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
                var instance = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.FTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_EXCEPTION_NOCLIENT"));
                if (!string.IsNullOrEmpty(Path))
                    Listing = instance.GetListing(Path, FtpListOption.Auto);
                else
                    Listing = instance.GetListing(FTPShellCommon.FtpCurrentRemoteDir, FtpListOption.Auto);
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
                                FileSize = instance.GetFileSize(finalDirListFTP.FullName);
                                ModDate = instance.GetModifiedTime(finalDirListFTP.FullName);
                                EntryBuilder.Append(ThemeColorsTools.GetColor(ThemeColorType.ListValue).VTSequenceForeground +
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
        /// <param name="Target">Target folder or file</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPDeleteRemote(string Target)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Deleting {0}...", vars: [Target]);

            // Delete a file or folder
            var instance = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.FTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_EXCEPTION_NOCLIENT"));
            if (instance.FileExists(Target))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "{0} is a file.", vars: [Target]);
                instance.DeleteFile(Target);
            }
            else if (instance.DirectoryExists(Target))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "{0} is a folder.", vars: [Target]);
                instance.DeleteDirectory(Target);
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
                var instance = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.FTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_EXCEPTION_NOCLIENT"));
                if (instance.DirectoryExists(Directory))
                {
                    // Directory exists, go to the new directory
                    instance.SetWorkingDirectory(Directory);
                    FTPShellCommon.FtpCurrentRemoteDir = instance.GetWorkingDirectory();
                    return true;
                }
                else
                {
                    // Directory doesn't exist, go to the old directory
                    throw new KernelException(KernelExceptionType.FTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_REMOTEDIRNOTFOUND"), Directory);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.FTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_NEEDSREMOTEDIR"));
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
                targetDir = FilesystemTools.NeutralizePath(Directory, FTPShellCommon.FtpCurrentDirectory);

                // Check if folder exists
                if (FilesystemTools.FolderExists(targetDir))
                {
                    // Parse written directory
                    var parser = new DirectoryInfo(targetDir);
                    FTPShellCommon.FtpCurrentDirectory = parser.FullName;
                    return true;
                }
                else
                {
                    throw new KernelException(KernelExceptionType.FTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_LOCALDIRNOTFOUND"), Directory);
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.FTPFilesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FS_NEEDSLOCALDIR"));
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
            var instance = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.FTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_EXCEPTION_NOCLIENT"));

            // Begin the moving process
            string SourceFile = Source.Split('/').Last();
            DebugWriter.WriteDebug(DebugLevel.I, "Moving from {0} to {1} with the source file of {2}...", vars: [Source, Target, SourceFile]);
            if (instance.DirectoryExists(Source))
                Success = instance.MoveDirectory(Source, Target);
            else if (instance.FileExists(Source) & instance.DirectoryExists(Target))
                Success = instance.MoveFile(Source, Target + SourceFile);
            else if (instance.FileExists(Source))
                Success = instance.MoveFile(Source, Target);
            DebugWriter.WriteDebug(DebugLevel.I, "Moved. Result: {0}", vars: [Success]);
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
            object? Result = null;
            var instance = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                throw new KernelException(KernelExceptionType.FTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_EXCEPTION_NOCLIENT"));

            // Begin the copying process
            string SourceFile = Source.Split('/').Last();
            DebugWriter.WriteDebug(DebugLevel.I, "Copying from {0} to {1} with the source file of {2}...", vars: [Source, Target, SourceFile]);
            if (instance.DirectoryExists(Source))
            {
                instance.DownloadDirectory(PathsManagement.TempPath + "/FTPTransfer", Source);
                Result = instance.UploadDirectory(PathsManagement.TempPath + "/FTPTransfer/" + Source, Target);
            }
            else if (instance.FileExists(Source) & instance.DirectoryExists(Target))
            {
                instance.DownloadFile(PathsManagement.TempPath + "/FTPTransfer/" + SourceFile, Source);
                Result = instance.UploadFile(PathsManagement.TempPath + "/FTPTransfer/" + SourceFile, Target + "/" + SourceFile);
            }
            else if (instance.FileExists(Source))
            {
                instance.DownloadFile(PathsManagement.TempPath + "/FTPTransfer/" + SourceFile, Source);
                Result = instance.UploadFile(PathsManagement.TempPath + "/FTPTransfer/" + SourceFile, Target);
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
        /// <param name="Target">Target file</param>
        /// <param name="Chmod">Permissions in CHMOD format. See https://man7.org/linux/man-pages/man2/chmod.2.html chmod(2) for more info.</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPChangePermissions(string Target, int Chmod)
        {
            try
            {
                var instance = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.FTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_EXCEPTION_NOCLIENT"));
                instance.Chmod(Target, Chmod);
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
        /// <param name="name">New directory name</param>
        /// <returns>True if successful; False if unsuccessful</returns>
        public static bool FTPMakeDirectory(string name)
        {
            try
            {
                var instance = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.FTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_EXCEPTION_NOCLIENT"));
                return instance.CreateDirectory(name);
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
        /// <param name="name">Path to file or directory</param>
        /// <returns>True if found; False otherwise</returns>
        public static bool FTPExists(string name) =>
            FTPFileExists(name) || FTPDirectoryExists(name);

        /// <summary>
        /// Checks to see if an FTP file exists
        /// </summary>
        /// <param name="name">Path to file</param>
        /// <returns>True if found; False otherwise</returns>
        public static bool FTPFileExists(string name)
        {
            try
            {
                var instance = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.FTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_EXCEPTION_NOCLIENT"));
                return instance.FileExists(name);
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
        /// <param name="name">Path to file</param>
        /// <returns>True if found; False otherwise</returns>
        public static bool FTPDirectoryExists(string name)
        {
            try
            {
                var instance = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                    throw new KernelException(KernelExceptionType.FTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_EXCEPTION_NOCLIENT"));
                return instance.DirectoryExists(name);
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

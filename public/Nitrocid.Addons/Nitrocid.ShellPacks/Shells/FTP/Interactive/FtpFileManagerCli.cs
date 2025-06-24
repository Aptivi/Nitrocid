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

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Files;
using Nitrocid.Base.Misc.Reflection;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Languages;
using Terminaux.Inputs.Interactive;
using Terminaux.Inputs.Styles.Infobox;
using Nitrocid.Base.Files.Instances;
using Nitrocid.Base.Files.Extensions;
using Textify.General;
using Nitrocid.Base.Kernel.Exceptions;
using Terminaux.Colors.Themes.Colors;
using FluentFTP;
using Nitrocid.ShellPacks.Tools.Filesystem;
using Nitrocid.ShellPacks.Tools.Transfer;

namespace Nitrocid.ShellPacks.Shells.FTP.Interactive
{
    internal class FtpFileManagerCli : BaseInteractiveTui<FileSystemEntry, FtpListItem>, IInteractiveTui<FileSystemEntry, FtpListItem>
    {
        internal bool refreshFirstPaneListing = true;
        internal bool refreshSecondPaneListing = true;
        private List<FileSystemEntry> firstPaneListing = [];
        private List<FtpListItem> secondPaneListing = [];

        /// <summary>
        /// Always true in the file manager as we want it to behave like Total Commander
        /// </summary>
        public override bool SecondPaneInteractable =>
            true;

        /// <inheritdoc/>
        public override IEnumerable<FileSystemEntry> PrimaryDataSource
        {
            get
            {
                try
                {
                    if (refreshFirstPaneListing)
                    {
                        refreshFirstPaneListing = false;
                        firstPaneListing = FilesystemTools.CreateList(FTPShellCommon.FtpCurrentDirectory, true);
                    }
                    return firstPaneListing;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get current directory list for the first pane [{0}]: {1}", vars: [FTPShellCommon.FtpCurrentDirectory, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    return [];
                }
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<FtpListItem> SecondaryDataSource
        {
            get
            {
                try
                {
                    if (refreshSecondPaneListing)
                    {
                        refreshSecondPaneListing = false;
                        var instance = (FtpClient?)FTPShellCommon.ClientFTP?.ConnectionInstance ??
                            throw new KernelException(KernelExceptionType.FTPShell, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTP_EXCEPTION_NOCLIENT"));
                        FTPShellCommon.FtpCurrentRemoteDir = FTPShellCommon.FtpCurrentRemoteDir;
                        secondPaneListing = [.. instance.GetListing(FTPShellCommon.FtpCurrentRemoteDir, FtpListOption.Auto)];
                    }
                    return secondPaneListing;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Failed to get current directory list for the second pane [{0}]: {1}", vars: [FTPShellCommon.FtpCurrentRemoteDir, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    return [];
                }
            }
        }

        /// <inheritdoc/>
        public override bool AcceptsEmptyData =>
            true;

        /// <inheritdoc/>
        public override string GetStatusFromItem(FileSystemEntry item)
        {
            FileSystemEntry FileInfoCurrentPane = item;

            // Check to see if we're given the file system info
            if (FileInfoCurrentPane == null)
                return LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_STATUS_NOINFO");

            // Now, populate the info to the status
            try
            {
                bool infoIsDirectory = FileInfoCurrentPane.Type == FileSystemEntryType.Directory;
                return $"[{(infoIsDirectory ? "/" : "*")}] {FileInfoCurrentPane.BaseEntry.Name}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <inheritdoc/>
        public override string GetEntryFromItem(FileSystemEntry item)
        {
            try
            {
                FileSystemEntry file = item;
                bool isDirectory = file.Type == FileSystemEntryType.Directory;
                if (Config.MainConfig.IfmShowFileSize)
                    return
                        // Name and directory indicator
                        $"[{(isDirectory ? "/" : "*")}] {file.BaseEntry.Name} | " +

                        // File size or directory size
                        $"{(!isDirectory ? ((FileInfo)file.BaseEntry).Length.SizeString() : FilesystemTools.GetAllSizesInFolder((DirectoryInfo)file.BaseEntry).SizeString())}"
                    ;
                else
                    return $"[{(isDirectory ? "/" : "*")}] {file.BaseEntry.Name}";
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get entry: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return "";
            }
        }

        /// <inheritdoc/>
        public override string GetStatusFromItemSecondary(FtpListItem item)
        {
            FtpListItem FileInfoCurrentPane = item;

            // Check to see if we're given the file system info
            if (FileInfoCurrentPane == null)
                return LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_STATUS_NOINFO");

            // Now, populate the info to the status
            try
            {
                bool infoIsDirectory = FileInfoCurrentPane.Type == FtpObjectType.Directory;
                return $"[{(infoIsDirectory ? "/" : "*")}] {FileInfoCurrentPane.Name}";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <inheritdoc/>
        public override string GetEntryFromItemSecondary(FtpListItem item)
        {
            try
            {
                FtpListItem file = item;
                bool isDirectory = file.Type == FtpObjectType.Directory;
                if (Config.MainConfig.IfmShowFileSize)
                    return
                        // Name and directory indicator
                        $"[{(isDirectory ? "/" : "*")}] {file.Name} | " +

                        // File size or none, because we don't need to increase server load
                        $"{(!isDirectory ? file.Size.SizeString() : "")}"
                    ;
                else
                    return $"[{(isDirectory ? "/" : "*")}] {file.Name}";
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to get entry: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                return "";
            }
        }

        internal void Open(FileSystemEntry? entry1, FtpListItem? entry2)
        {
            try
            {
                // Don't do anything if we haven't been provided anything.
                if (entry1 is null && entry2 is null)
                    return;

                // Determine whether to run this action locally or from the remote
                if (CurrentPane == 2)
                {
                    // We are dealing with the remote side.
                    var currentEntry = entry2;
                    if (currentEntry is null)
                        return;

                    // Now that the selected file or folder exists, check the type.
                    if (currentEntry.Type == FtpObjectType.Directory)
                    {
                        // We're dealing with a folder. Open it in the selected pane.
                        FTPFilesystem.FTPChangeRemoteDir(currentEntry.FullName + "/");
                        refreshSecondPaneListing = true;
                        InteractiveTuiTools.SelectionMovement(this, 1);
                    }
                }
                else
                {
                    // We are dealing with the local side.
                    var currentEntry = entry1;
                    if (currentEntry is null || !currentEntry.Exists)
                        return;

                    // Now that the selected file or folder exists, check the type.
                    if (currentEntry.Type == FileSystemEntryType.Directory)
                    {
                        // We're dealing with a folder. Open it in the selected pane.
                        FTPFilesystem.FTPChangeLocalDir(FilesystemTools.NeutralizePath(currentEntry.FilePath + "/"));
                        refreshFirstPaneListing = true;
                        InteractiveTuiTools.SelectionMovement(this, 1);
                    }
                    else if (currentEntry.Type == FileSystemEntryType.File)
                    {
                        // We're dealing with a file. Clear the screen and open the appropriate editor.
                        ThemeColorsTools.LoadBackground();
                        FilesystemTools.OpenDeterministically(currentEntry.FilePath);
                    }
                }
                
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FILEFOLDERCANTOPEN") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void GoUp()
        {
            if (CurrentPane == 2)
            {
                FTPFilesystem.FTPChangeRemoteDir("..");
                refreshSecondPaneListing = true;
            }
            else
            {
                FTPFilesystem.FTPChangeLocalDir("..");
                refreshFirstPaneListing = true;
            }
            InteractiveTuiTools.SelectionMovement(this, 1);
        }

        internal void PrintFileSystemEntry(FileSystemEntry? entry1, FtpListItem? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            // Render the final information string
            try
            {
                // Determine whether to run this action locally or from the remote
                var finalInfoRendered = new StringBuilder();
                if (CurrentPane == 2)
                {
                    // We are dealing with the remote side.
                    var currentEntry = entry2;
                    if (currentEntry is null)
                        return;

                    // Get the current FTP entry info details
                    finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_NAME").FormatString(currentEntry.Name));
                    finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_FULLNAME").FormatString(currentEntry.FullName));
                    if (currentEntry.Type == FtpObjectType.File)
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_FILESIZE").FormatString(currentEntry.Size.SizeString()));
                    else if (currentEntry.Type == FtpObjectType.Link)
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_SYMLINK").FormatString(currentEntry.LinkTarget));
                    finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_CREATIONTIME").FormatString(TimeDateRenderers.Render(currentEntry.RawCreated)));
                    finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_LASTWRITETIME").FormatString(TimeDateRenderers.Render(currentEntry.RawModified)));
                }
                else
                {
                    // We are dealing with the local side.
                    var currentEntry = entry1;
                    if (currentEntry is null || !currentEntry.Exists)
                        return;

                    string fullPath = currentEntry.FilePath;
                    if (FilesystemTools.FolderExists(fullPath))
                    {
                        // The file system info instance points to a folder
                        var DirInfo = new DirectoryInfo(fullPath);
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_NAME").FormatString(DirInfo.Name));
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_FULLNAME").FormatString(FilesystemTools.NeutralizePath(DirInfo.FullName)));
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_SIZE").FormatString(FilesystemTools.GetAllSizesInFolder(DirInfo).SizeString()));
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_CREATIONTIME").FormatString(TimeDateRenderers.Render(DirInfo.CreationTime)));
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_LASTACCESSTIME").FormatString(TimeDateRenderers.Render(DirInfo.LastAccessTime)));
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_LASTWRITETIME").FormatString(TimeDateRenderers.Render(DirInfo.LastWriteTime)));
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_ATTRIBUTES").FormatString(DirInfo.Attributes));
                        if (DirInfo.Parent is not null)
                            finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_PARENTDIR").FormatString(FilesystemTools.NeutralizePath(DirInfo.Parent.FullName)));
                    }
                    else
                    {
                        // The file system info instance points to a file
                        FileInfo fileInfo = new(fullPath);
                        bool isBinary = FilesystemTools.IsBinaryFile(fileInfo.FullName);
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_NAME").FormatString(fileInfo.Name));
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_FULLNAME").FormatString(FilesystemTools.NeutralizePath(fileInfo.FullName)));
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_FILESIZE").FormatString(fileInfo.Length.SizeString()));
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_CREATIONTIME").FormatString(TimeDateRenderers.Render(fileInfo.CreationTime)));
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_LASTACCESSTIME").FormatString(TimeDateRenderers.Render(fileInfo.LastAccessTime)));
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_LASTWRITETIME").FormatString(TimeDateRenderers.Render(fileInfo.LastWriteTime)));
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_ATTRIBUTES").FormatString(fileInfo.Attributes));
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_WHERETOFIND").FormatString(FilesystemTools.NeutralizePath(fileInfo.DirectoryName)));
                        if (!isBinary)
                        {
                            var Style = FilesystemTools.GetLineEndingFromFile(fullPath);
                            finalInfoRendered.AppendLine((LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_NEWLINESTYLE") + " {0}").FormatString(Style.ToString()));
                        }
                        finalInfoRendered.AppendLine((LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_BINFILE") + " {0}").FormatString(isBinary));
                        finalInfoRendered.AppendLine((LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_MIMEMETADATA") + " {0}").FormatString(MimeTypes.GetMimeType(fileInfo.Extension)));
                        finalInfoRendered.AppendLine((LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_MIMEMETADATAEXTENDED") + ": {0}").FormatString(MimeTypes.GetExtendedMimeType(fileInfo.FullName)));
                        finalInfoRendered.AppendLine((LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_FILETYPE") + ": {0}\n").FormatString(MimeTypes.GetMagicInfo(fileInfo.FullName)));

                        // .NET managed info
                        if (ReflectionCommon.IsDotnetAssemblyFile(fullPath, out AssemblyName? asmName) && asmName is not null)
                        {
                            finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_NAME").FormatString(asmName.Name));
                            finalInfoRendered.AppendLine((LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_FULLNAME2") + ": {0}").FormatString(asmName.FullName));
                            if (asmName.Version is not null)
                                finalInfoRendered.AppendLine((LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_VERSION") + ": {0}").FormatString(asmName.Version.ToString()));
                            finalInfoRendered.AppendLine((LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_CULTURENAME") + ": {0}").FormatString(asmName.CultureName));
                            finalInfoRendered.AppendLine((LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_CONTENTTYPE") + ": {0}\n").FormatString(asmName.ContentType.ToString()));
                        }
                        else
                        {
                            finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FSENTRY_NOTDOTNETASSEMBLY"));
                        }

                        // Other info handled by the extension handler
                        if (ExtensionHandlerTools.IsHandlerRegistered(fileInfo.Extension))
                        {
                            var handler = ExtensionHandlerTools.GetExtensionHandler(fileInfo.Extension) ??
                                throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_EXCEPTION_REGISTEREDHANDLERFAILED") + $" {fileInfo.Extension}");
                            finalInfoRendered.AppendLine(handler.InfoHandler(fullPath));
                        }
                    }
                }

                // Now, render the info box
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_CANTGETFSINFO") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void CopyFileOrDir(FileSystemEntry? entry1, FtpListItem? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            try
            {
                // Determine whether to run this action locally or from the remote
                if (CurrentPane == 2)
                {
                    // We are dealing with the remote side.
                    var currentEntry = entry2;
                    if (currentEntry is null)
                        return;

                    string dest = FilesystemTools.NeutralizePath(FTPShellCommon.FtpCurrentDirectory + "/" + currentEntry.Name, FTPShellCommon.FtpCurrentDirectory);
                    DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {dest}");
                    DebugCheck.AssertNull(dest, "destination is null!");
                    DebugCheck.Assert(!string.IsNullOrWhiteSpace(dest), "destination is empty or whitespace!");
                    if (currentEntry.Type == FtpObjectType.Directory)
                        FTPTransfer.FTPGetFolder(currentEntry.FullName, dest);
                    else
                        FTPTransfer.FTPGetFile(currentEntry.FullName, dest);
                }
                else
                {
                    // We are dealing with the local side.
                    var currentEntry = entry1;
                    if (currentEntry is null || !currentEntry.Exists)
                        return;

                    string dest = FTPShellCommon.FtpCurrentRemoteDir + "/" + Path.GetFileName(currentEntry.FilePath);
                    DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {dest}");
                    DebugCheck.AssertNull(dest, "destination is null!");
                    DebugCheck.Assert(!string.IsNullOrWhiteSpace(dest), "destination is empty or whitespace!");
                    if (currentEntry.Type == FileSystemEntryType.Directory)
                        FTPTransfer.FTPUploadFolder(dest, currentEntry.FilePath);
                    else
                        FTPTransfer.FTPUploadFile(dest, currentEntry.FilePath);
                }

                if (CurrentPane == 2)
                    refreshFirstPaneListing = true;
                else
                    refreshSecondPaneListing = true;
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_CANTCOPY") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void MoveFileOrDir(FileSystemEntry? entry1, FtpListItem? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            try
            {
                // Determine whether to run this action locally or from the remote
                if (CurrentPane == 2)
                {
                    // We are dealing with the remote side.
                    var currentEntry = entry2;
                    if (currentEntry is null)
                        return;

                    string dest = FilesystemTools.NeutralizePath(FTPShellCommon.FtpCurrentDirectory + "/" + currentEntry.Name, FTPShellCommon.FtpCurrentDirectory);
                    DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {dest}");
                    DebugCheck.AssertNull(dest, "destination is null!");
                    DebugCheck.Assert(!string.IsNullOrWhiteSpace(dest), "destination is empty or whitespace!");
                    if (currentEntry.Type == FtpObjectType.Directory)
                        FTPTransfer.FTPGetFolder(currentEntry.FullName, dest);
                    else
                        FTPTransfer.FTPGetFile(currentEntry.FullName, dest);
                    FTPFilesystem.FTPDeleteRemote(dest);
                }
                else
                {
                    // We are dealing with the local side.
                    var currentEntry = entry1;
                    if (currentEntry is null || !currentEntry.Exists)
                        return;

                    string dest = FTPShellCommon.FtpCurrentRemoteDir + "/" + Path.GetFileName(currentEntry.FilePath);
                    DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {dest}");
                    DebugCheck.AssertNull(dest, "destination is null!");
                    DebugCheck.Assert(!string.IsNullOrWhiteSpace(dest), "destination is empty or whitespace!");
                    if (currentEntry.Type == FileSystemEntryType.Directory)
                        FTPTransfer.FTPUploadFolder(dest, currentEntry.FilePath);
                    else
                        FTPTransfer.FTPUploadFile(dest, currentEntry.FilePath);
                    FilesystemTools.RemoveFileOrDir(currentEntry.FilePath);
                }

                refreshFirstPaneListing = true;
                refreshSecondPaneListing = true;
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_CANTMOVE") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void RemoveFileOrDir(FileSystemEntry? entry1, FtpListItem? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            try
            {
                // Determine whether to run this action locally or from the remote
                if (CurrentPane == 2)
                {
                    // We are dealing with the remote side.
                    var currentEntry = entry2;
                    if (currentEntry is null)
                        return;

                    FTPFilesystem.FTPDeleteRemote(currentEntry.FullName);
                }
                else
                {
                    // We are dealing with the local side.
                    var currentEntry = entry1;
                    if (currentEntry is null || !currentEntry.Exists)
                        return;

                    FilesystemTools.RemoveFileOrDir(currentEntry.FilePath);
                }

                if (CurrentPane == 2)
                    refreshSecondPaneListing = true;
                else
                    refreshFirstPaneListing = true;
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_CANTREMOVE") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void CopyTo(FileSystemEntry? entry1, FtpListItem? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            try
            {
                string path = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_COPYPROMPT"), Settings.InfoBoxSettings);

                // Determine whether to run this action locally or from the remote
                if (CurrentPane == 2)
                {
                    // We are dealing with the remote side.
                    var currentEntry = entry2;
                    if (currentEntry is null)
                        return;

                    path = FilesystemTools.NeutralizePath(FTPShellCommon.FtpCurrentDirectory + "/" + currentEntry.Name, FTPShellCommon.FtpCurrentDirectory);
                    DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                    DebugCheck.AssertNull(path, "destination is null!");
                    DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                    if (FTPFilesystem.FTPExists(path))
                    {
                        if (currentEntry.Type == FtpObjectType.Directory)
                            FTPTransfer.FTPGetFolder(currentEntry.FullName, path);
                        else
                            FTPTransfer.FTPGetFile(currentEntry.FullName, path);
                        refreshFirstPaneListing = true;
                    }
                    else
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FILENOTFOUND"), Settings.InfoBoxSettings);
                }
                else
                {
                    // We are dealing with the local side.
                    var currentEntry = entry1;
                    if (currentEntry is null || !currentEntry.Exists)
                        return;

                    path = FTPShellCommon.FtpCurrentRemoteDir + "/" + Path.GetFileName(currentEntry.FilePath);
                    DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                    DebugCheck.AssertNull(path, "destination is null!");
                    DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                    if (FilesystemTools.FolderExists(path))
                    {
                        if (FilesystemTools.TryParsePath(path))
                        {
                            if (currentEntry.Type == FileSystemEntryType.Directory)
                                FTPTransfer.FTPUploadFolder(path, currentEntry.FilePath);
                            else
                                FTPTransfer.FTPUploadFile(path, currentEntry.FilePath);
                            refreshSecondPaneListing = true;
                        }
                        else
                            InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_NEEDSCORRECTPATH"), Settings.InfoBoxSettings);
                    }
                    else
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FILENOTFOUND"), Settings.InfoBoxSettings);
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_CANTCOPY") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void MoveTo(FileSystemEntry? entry1, FtpListItem? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            try
            {
                string path = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_MOVEPROMPT"), Settings.InfoBoxSettings);
                
                // Determine whether to run this action locally or from the remote
                if (CurrentPane == 2)
                {
                    // We are dealing with the remote side.
                    var currentEntry = entry2;
                    if (currentEntry is null)
                        return;

                    path = FilesystemTools.NeutralizePath(FTPShellCommon.FtpCurrentDirectory + "/" + currentEntry.Name, FTPShellCommon.FtpCurrentDirectory);
                    DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                    DebugCheck.AssertNull(path, "destination is null!");
                    DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                    if (FTPFilesystem.FTPExists(path))
                    {
                        if (currentEntry.Type == FtpObjectType.Directory)
                            FTPTransfer.FTPGetFolder(currentEntry.FullName, path);
                        else
                            FTPTransfer.FTPGetFile(currentEntry.FullName, path);
                        FTPFilesystem.FTPDeleteRemote(path);
                        refreshSecondPaneListing = true;
                        refreshFirstPaneListing = true;
                    }
                    else
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FILENOTFOUND"), Settings.InfoBoxSettings);
                }
                else
                {
                    // We are dealing with the local side.
                    var currentEntry = entry1;
                    if (currentEntry is null || !currentEntry.Exists)
                        return;

                    path = FTPShellCommon.FtpCurrentRemoteDir + "/" + Path.GetFileName(currentEntry.FilePath);
                    DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                    DebugCheck.AssertNull(path, "destination is null!");
                    DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                    if (FilesystemTools.FolderExists(path))
                    {
                        if (FilesystemTools.TryParsePath(path))
                        {
                            if (currentEntry.Type == FileSystemEntryType.Directory)
                                FTPTransfer.FTPUploadFolder(path, currentEntry.FilePath);
                            else
                                FTPTransfer.FTPUploadFile(path, currentEntry.FilePath);
                            FilesystemTools.RemoveFileOrDir(currentEntry.FilePath);
                            refreshSecondPaneListing = true;
                            refreshFirstPaneListing = true;
                        }
                        else
                            InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_NEEDSCORRECTPATH"), Settings.InfoBoxSettings);
                    }
                    else
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FILENOTFOUND"), Settings.InfoBoxSettings);
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_CANTMOVE") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void Rename(FileSystemEntry? entry1, FtpListItem? entry2)
        {
            // Don't do anything if we haven't been provided anything.
            if (entry1 is null && entry2 is null)
                return;

            try
            {
                string filename = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_NEWFILENAMEPROMPT"), Settings.InfoBoxSettings);
                DebugWriter.WriteDebug(DebugLevel.I, $"New filename is {filename}");

                // Determine whether to run this action locally or from the remote
                if (CurrentPane == 2)
                {
                    // We are dealing with the remote side.
                    var currentEntry = entry2;
                    if (currentEntry is null)
                        return;

                    if (!FTPFilesystem.FTPExists(currentEntry.FullName.RemoveSuffix(currentEntry.Name) + $"/{filename}"))
                    {
                        FTPFilesystem.FTPMoveItem(currentEntry.FullName, currentEntry.FullName.RemoveSuffix(currentEntry.Name) + $"/{filename}");
                        refreshSecondPaneListing = true;
                    }
                    else
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FILEFOUND"), Settings.InfoBoxSettings);
                }
                else
                {
                    // We are dealing with the local side.
                    var currentEntry = entry1;
                    if (currentEntry is null || !currentEntry.Exists)
                        return;

                    if (!FilesystemTools.FileExists(filename))
                    {
                        if (FilesystemTools.TryParseFileName(filename))
                        {
                            FilesystemTools.MoveFileOrDir(currentEntry.FilePath, Path.GetDirectoryName(currentEntry.FilePath) + $"/{filename}");
                            refreshFirstPaneListing = true;
                        }
                        else
                            InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FILENAMEINVALID"), Settings.InfoBoxSettings);
                    }
                    else
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FILEFOUND"), Settings.InfoBoxSettings);
                }
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_CANTMOVE") + ": {0}".FormatString(ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), Settings.InfoBoxSettings);
            }
        }

        internal void MakeDir()
        {
            string path = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_NEWDIRNAMEPROMPT"), Settings.InfoBoxSettings);

            // Determine whether to run this action locally or from the remote
            if (CurrentPane == 2)
            {
                // We are dealing with the remote side.
                if (!FTPFilesystem.FTPExists(path))
                {
                    FTPFilesystem.FTPMakeDirectory(path);
                    refreshFirstPaneListing = true;
                }
                else
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FOLDERFOUND"), Settings.InfoBoxSettings);
            }
            else
            {
                // We are dealing with the local side.
                path = FilesystemTools.NeutralizePath(path, FTPShellCommon.FtpCurrentDirectory);
                if (!FilesystemTools.FolderExists(path))
                {
                    FilesystemTools.TryMakeDirectory(path);
                    refreshFirstPaneListing = true;
                }
                else
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_SHELLPACKS_FTPSFTP_FMCLI_FOLDERFOUND"), Settings.InfoBoxSettings);
            }
        }
    }
}

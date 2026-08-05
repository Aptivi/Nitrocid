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

extern alias TextifyDep;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Nitrocid.Base.Drivers.Encryption;
using Nitrocid.Base.Files.Extensions;
using Nitrocid.Base.Files.Unix;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Time.Renderers;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Reflection;
using SpecProbe.Software.Platform;
using Terminaux.Inputs;
using Terminaux.Inputs.Modules;
using Terminaux.Inputs.Styles;
using Terminaux.Inputs.Styles.Infobox;
using Terminaux.Inputs.Styles.Infobox.Tools;
using Terminaux.Sequences;
using TextifyDep::Textify.General;

namespace Nitrocid.Base.Files.Instances.Interactives
{
    internal static class FileManagerTuiCommon
    {
        internal static string GetStatusStringFrom(FileSystemEntry entry)
        {
            bool infoIsDirectory = entry.Type == FileSystemEntryType.Directory;
            UnixPermissionType permissionTypeUser = entry.UnixPermissions[0].Types;
            UnixPermissionType permissionTypeGroup = entry.UnixPermissions[1].Types;
            UnixPermissionType permissionTypeOther = entry.UnixPermissions[2].Types;
            string finalRenderedPermissions =
                $"[{UnixPermissionManager.BuildPermissionRepresentation(permissionTypeUser)}" +
                $" {UnixPermissionManager.BuildPermissionRepresentation(permissionTypeGroup)}" +
                $" {UnixPermissionManager.BuildPermissionRepresentation(permissionTypeOther)}]";
            string finalRenderedSpecialPermissions = $"[{UnixPermissionManager.BuildSpecialPermissionRepresentation(entry.UnixSpecial)}]";
            return
                $"[{(infoIsDirectory ? "/" : "*")}] " +
                (!PlatformHelper.IsOnWindows() ? $"{finalRenderedPermissions} {finalRenderedSpecialPermissions} " : "") +
                entry.BaseEntry.FullName;
        }

        internal static string GetEntryStringFrom(FileSystemEntry entry)
        {
            bool isDirectory = entry.Type == FileSystemEntryType.Directory;
            string entryName = $"[{(isDirectory ? "/" : "*")}] {entry.BaseEntry.Name}";
            if (Config.MainConfig.IfmShowFileSize)
                return $"{entryName} | {(!isDirectory ?
                    ((FileInfo)entry.BaseEntry).Length.SizeString() :
                    FilesystemTools.GetAllSizesInFolder((DirectoryInfo)entry.BaseEntry).SizeString())}";
            else
                return $"{entryName}";
        }

        internal static string GetInfoStringFrom(FileSystemEntry entry)
        {
            bool isDirectory = entry.Type == FileSystemEntryType.Directory;
            var size = entry.FileSize;
            var path = entry.FilePath;
            UnixPermissionType permissionTypeUser = entry.UnixPermissions[0].Types;
            UnixPermissionType permissionTypeGroup = entry.UnixPermissions[1].Types;
            UnixPermissionType permissionTypeOther = entry.UnixPermissions[2].Types;
            string finalRenderedName = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FILENAME") + $": {Path.GetFileName(entry.FilePath)}";
            string finalRenderedDir = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ISDIRECTORY") + $": {isDirectory}";
            string finalRenderedSize = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FILESIZE") + $": {size.SizeString()}";
            string finalRenderedPath = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FILEPATH") + $": {path}";
            // TODO: NKS_MISC_INTERACTIVES_FMTUI_PERMISSIONS -> Permissions
            // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIALPERMISSIONS -> Special permissions
            string finalRenderedPermissions = $"{LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERMISSIONS")}: " +
                $"[{UnixPermissionManager.BuildPermissionRepresentation(permissionTypeUser)}" +
                $" {UnixPermissionManager.BuildPermissionRepresentation(permissionTypeGroup)}" +
                $" {UnixPermissionManager.BuildPermissionRepresentation(permissionTypeOther)}]";
            string finalRenderedSpecialPermissions = $"{LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIALPERMISSIONS")}: " +
                $"[{UnixPermissionManager.BuildSpecialPermissionRepresentation(entry.UnixSpecial)}]";
            return
                finalRenderedName + CharManager.NewLine +
                finalRenderedDir + CharManager.NewLine +
                finalRenderedSize + CharManager.NewLine +
                finalRenderedPath + (PlatformHelper.IsOnWindows() ? "" : CharManager.NewLine +
                finalRenderedPermissions + CharManager.NewLine +
                finalRenderedSpecialPermissions)
            ;
        }

        internal static void PrintFileSystemEntry(FileSystemEntry entry, InfoBoxSettings infoBoxSettings)
        {
            // Render the final information string
            try
            {
                var finalInfoRendered = new StringBuilder();
                string fullPath = entry.FilePath;
                if (FilesystemTools.FolderExists(fullPath))
                {
                    // The file system info instance points to a folder
                    var DirInfo = new DirectoryInfo(fullPath);
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ENTRYNAME"), DirInfo.Name));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_FULLNAME"), FilesystemTools.NeutralizePath(DirInfo.FullName)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ENTRYSIZE"), FilesystemTools.GetAllSizesInFolder(DirInfo).SizeString()));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_CREATIONTIME"), TimeDateRenderers.Render(DirInfo.CreationTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_LASTACCESSTIME"), TimeDateRenderers.Render(DirInfo.LastAccessTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_LASTWRITETIME"), TimeDateRenderers.Render(DirInfo.LastWriteTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_ATTRIBUTES"), DirInfo.Attributes));
                    if (DirInfo.Parent is not null)
                        finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_PARENTDIRECTORY"), FilesystemTools.NeutralizePath(DirInfo.Parent.FullName)));
                }
                else
                {
                    // The file system info instance points to a file
                    FileInfo fileInfo = new(fullPath);
                    bool isBinary = FilesystemTools.IsBinaryFile(fileInfo.FullName);
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ENTRYNAME"), fileInfo.Name));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_FULLNAME"), FilesystemTools.NeutralizePath(fileInfo.FullName)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_FILESIZE"), fileInfo.Length.SizeString()));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_CREATIONTIME"), TimeDateRenderers.Render(fileInfo.CreationTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_LASTACCESSTIME"), TimeDateRenderers.Render(fileInfo.LastAccessTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_LASTWRITETIME"), TimeDateRenderers.Render(fileInfo.LastWriteTime)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_ATTRIBUTES"), fileInfo.Attributes));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INFO_WHERETOFIND"), FilesystemTools.NeutralizePath(fileInfo.DirectoryName)));
                    if (!isBinary)
                    {
                        var Style = FilesystemTools.GetLineEndingFromFile(fullPath);
                        finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NEWLINESTYLE") + " {0}", Style.ToString()));
                    }
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_BINARYFILE") + " {0}", isBinary));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_MIMEMETADATA") + " {0}", MimeTypes.GetMimeType(fileInfo.Extension)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_MIMEMETADATAEXT") + ": {0}", MimeTypes.GetExtendedMimeType(fileInfo.FullName)));
                    finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FILETYPE") + ": {0}\n", MimeTypes.GetMagicInfo(fileInfo.FullName)));

                    // .NET managed info
                    if (ReflectionCommon.IsDotnetAssemblyFile(fullPath, out AssemblyName? asmName) && asmName is not null)
                    {
                        finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_ENTRYNAME"), asmName.Name));
                        finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FULLNAME") + ": {0}", asmName.FullName));
                        if (asmName.Version is not null)
                            finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_VERSION") + ": {0}", asmName.Version.ToString()));
                        finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CULTURENAME") + ": {0}", asmName.CultureName));
                        finalInfoRendered.AppendLine(TextTools.FormatString(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CONTENTTYPE") + ": {0}\n", asmName.ContentType.ToString()));
                    }
                    else
                    {
                        finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOTDOTNETASM"));
                    }

                    // Other info handled by the extension handler
                    if (ExtensionHandlerTools.IsHandlerRegistered(fileInfo.Extension))
                    {
                        var handler = ExtensionHandlerTools.GetExtensionHandler(fileInfo.Extension) ??
                            throw new KernelException(KernelExceptionType.Filesystem, LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_EXCEPTION_HANDLERFAILED") + $" {fileInfo.Extension}");
                        finalInfoRendered.AppendLine(handler.InfoHandler(fullPath));
                    }
                }

                // Now, render the info box
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), infoBoxSettings);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANTGETFSINFO") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), infoBoxSettings);
            }
        }

        internal static void CopyFileOrDir(FileSystemEntry entry, string dest, InfoBoxSettings infoBoxSettings)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {dest}");
                DebugCheck.AssertNull(dest, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(dest), "destination is empty or whitespace!");
                FilesystemTools.CopyFileOrDir(entry.FilePath, dest);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANTCOPY") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), infoBoxSettings);
            }
        }

        internal static void MoveFileOrDir(FileSystemEntry entry, string dest, InfoBoxSettings infoBoxSettings)
        {
            try
            {
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {dest}");
                DebugCheck.AssertNull(dest, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(dest), "destination is empty or whitespace!");
                FilesystemTools.MoveFileOrDir(entry.FilePath, dest);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANTMOVE") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), infoBoxSettings);
            }
        }

        internal static void RemoveFileOrDir(FileSystemEntry entry, InfoBoxSettings infoBoxSettings)
        {
            try
            {
                FilesystemTools.RemoveFileOrDir(entry.FilePath);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANTREMOVE") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), infoBoxSettings);
            }
        }

        internal static void CopyTo(FileSystemEntry entry, string dest, InfoBoxSettings infoBoxSettings)
        {
            try
            {
                string path = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_TARGETPATHCOPY"), infoBoxSettings);
                path = FilesystemTools.NeutralizePath(path, dest) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                DebugCheck.AssertNull(path, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                if (FilesystemTools.FolderExists(path))
                {
                    if (FilesystemTools.TryParsePath(path))
                        FilesystemTools.CopyFileOrDir(entry.FilePath, path);
                    else
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INVALIDPATH"), infoBoxSettings);
                }
                else
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FILENOTFOUND"), infoBoxSettings);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANTCOPY") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), infoBoxSettings);
            }
        }

        internal static void MoveTo(FileSystemEntry entry, string dest, InfoBoxSettings infoBoxSettings)
        {
            try
            {
                string path = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_TARGETPATHMOVE"), infoBoxSettings);
                path = FilesystemTools.NeutralizePath(path, dest) + "/";
                DebugWriter.WriteDebug(DebugLevel.I, $"Destination is {path}");
                DebugCheck.AssertNull(path, "destination is null!");
                DebugCheck.Assert(!string.IsNullOrWhiteSpace(path), "destination is empty or whitespace!");
                if (FilesystemTools.FolderExists(path))
                {
                    if (FilesystemTools.TryParsePath(path))
                        FilesystemTools.MoveFileOrDir(entry.FilePath, path);
                    else
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INVALIDPATH"), infoBoxSettings);
                }
                else
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FILENOTFOUND"), infoBoxSettings);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANTMOVE") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), infoBoxSettings);
            }
        }

        internal static void Rename(FileSystemEntry entry, InfoBoxSettings infoBoxSettings)
        {
            try
            {
                string filename = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NEWFILENAMEPROMPT"), infoBoxSettings);
                DebugWriter.WriteDebug(DebugLevel.I, $"New filename is {filename}");
                if (!FilesystemTools.FileExists(filename))
                {
                    if (FilesystemTools.TryParseFileName(filename))
                        FilesystemTools.MoveFileOrDir(entry.FilePath, Path.GetDirectoryName(entry.FilePath) + $"/{filename}");
                    else
                        InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_INVALIDFILENAME"), infoBoxSettings);
                }
                else
                    InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FILEEXISTS"), infoBoxSettings);
            }
            catch (Exception ex)
            {
                var finalInfoRendered = new StringBuilder();
                finalInfoRendered.AppendLine(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_CANTMOVE") + TextTools.FormatString(": {0}", ex.Message));
                InfoBoxModalColor.WriteInfoBoxModal(finalInfoRendered.ToString(), infoBoxSettings);
            }
        }

        internal static void MakeDir(string dest, InfoBoxSettings infoBoxSettings)
        {
            string path = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NEWFOLDERNAMEPROMPT"), infoBoxSettings);
            path = FilesystemTools.NeutralizePath(path, dest);
            if (!FilesystemTools.FolderExists(path))
                FilesystemTools.TryMakeDirectory(path);
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_FOLDEREXISTS"), infoBoxSettings);
        }

        internal static void Hash(FileSystemEntry entry, InfoBoxSettings infoBoxSettings)
        {
            if (!FilesystemTools.FileExists(entry.FilePath))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOTAFILE"), infoBoxSettings);
                return;
            }

            // Render the hash box
            // TODO: NKS_MISC_INTERACTIVES_FMTUI_HASHDRIVERPROMPT_NEW -> Select a hash driver from the list below.
            string[] hashDrivers = EncryptionDriverTools.GetEncryptionDriverNames();
            int hashDriverIdx = InfoBoxSelectionColor.WriteInfoBoxSelection(InputChoiceTools.GetInputChoices(hashDrivers), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_HASHDRIVERPROMPT_NEW"), infoBoxSettings);
            if (hashDriverIdx < 0)
                return;
            string hashDriver = hashDrivers[hashDriverIdx];
            string hash = Encryption.GetEncryptedFile(entry.FilePath, hashDriver);
            InfoBoxModalColor.WriteInfoBoxModal(hash, infoBoxSettings);
        }

        internal static void Verify(FileSystemEntry entry, InfoBoxSettings infoBoxSettings)
        {
            if (!FilesystemTools.FileExists(entry.FilePath))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOTAFILE"), infoBoxSettings);
                return;
            }

            // Render the hash box
            // TODO: NKS_MISC_INTERACTIVES_FMTUI_HASHDRIVERPROMPT_NEW -> Select a hash driver from the list below.
            string[] hashDrivers = EncryptionDriverTools.GetEncryptionDriverNames();
            int hashDriverIdx = InfoBoxSelectionColor.WriteInfoBoxSelection(InputChoiceTools.GetInputChoices(hashDrivers), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_HASHDRIVERPROMPT_NEW"), infoBoxSettings);
            if (hashDriverIdx < 0)
                return;
            string hashDriver = hashDrivers[hashDriverIdx];
            string hash = Encryption.GetEncryptedFile(entry.FilePath, hashDriver);

            // Now, let the user write the expected hash
            string expectedHash = InfoBoxInputColor.WriteInfoBoxInput(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_EXPECTEDHASHPROMPT"), infoBoxSettings);
            if (expectedHash == hash)
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_HASHESMATCH"), infoBoxSettings);
            else
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_HASHESNOMATCH"), infoBoxSettings);
        }

        internal static void Preview(FileSystemEntry entry, InfoBoxSettings infoBoxSettings)
        {
            if (!FilesystemTools.FileExists(entry.FilePath))
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_NOTAFILE"), infoBoxSettings);
                return;
            }

            // Render the preview box
            string preview = FilesystemTools.RenderContents(entry.FilePath, false);
            string filtered = FilesystemTools.IsBinaryFile(entry.FilePath) ? preview : VtSequenceTools.FilterVTSequences(preview);
            InfoBoxModalColor.WriteInfoBoxModal(filtered, infoBoxSettings);
        }

        internal static void ShowUnixPermissionChangeInfoBox(FileSystemEntry fileSystemEntry, InfoBoxSettings infoBoxSettings)
        {
            try
            {
                // Input choices for permissions
                InputChoiceCategoryInfo[] permissionsInputs = [new("", [new("", [
                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_PERM_READ_NAME -> Read
                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_PERM_READ_TITLE -> File can be read or directory can be queried
                    new(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERM_READ_NAME"), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERM_READ_TITLE")),

                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_PERM_WRITE_NAME -> Write
                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_PERM_WRITE_TITLE -> File can be write or directory can be modified
                    new(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERM_WRITE_NAME"), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERM_WRITE_TITLE")),

                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_PERM_EXECUTE_NAME -> Execute
                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_PERM_EXECUTE_TITLE -> File can be execute or directory can be traversed
                    new(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERM_EXECUTE_NAME"), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERM_EXECUTE_TITLE")),
                ])])];

                // Input choices for special permissions
                InputChoiceCategoryInfo[] specialPermissionsInputs = [new("", [new("", [
                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_SETUID_NAME -> Setuid
                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_SETUID_TITLE -> File can be executed with the privileges of the file owner, usually a root user
                    new(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_SETUID_NAME"), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_SETUID_TITLE")),

                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_SETGID_NAME -> Setgid
                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_SETGID_TITLE -> File can be executed with the privileges of the file group, usually an administrative group
                    new(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_SETGID_NAME"), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_SETGID_TITLE")),

                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_STICKY_NAME -> Sticky
                    // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_STICKY_TITLE -> Files inside a directory with this bit set can only be manipulated with by file owner, directory owner, and a root user
                    new(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_STICKY_NAME"), LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIAL_PERM_STICKY_TITLE")),
                ])])];

                // Get current permissions
                var permissionDescriptors = fileSystemEntry.UnixPermissions;
                var specialPermissions = fileSystemEntry.UnixSpecial;

                // Get current user permissions
                List<int> userPerms = [];
                if (permissionDescriptors[0].Types.HasFlag(UnixPermissionType.Read))
                    userPerms.Add(0);
                if (permissionDescriptors[0].Types.HasFlag(UnixPermissionType.Write))
                    userPerms.Add(1);
                if (permissionDescriptors[0].Types.HasFlag(UnixPermissionType.Execute))
                    userPerms.Add(2);

                // Get current group permissions
                List<int> groupPerms = [];
                if (permissionDescriptors[1].Types.HasFlag(UnixPermissionType.Read))
                    groupPerms.Add(0);
                if (permissionDescriptors[1].Types.HasFlag(UnixPermissionType.Write))
                    groupPerms.Add(1);
                if (permissionDescriptors[1].Types.HasFlag(UnixPermissionType.Execute))
                    groupPerms.Add(2);

                // Get current other permissions
                List<int> otherPerms = [];
                if (permissionDescriptors[2].Types.HasFlag(UnixPermissionType.Read))
                    otherPerms.Add(0);
                if (permissionDescriptors[2].Types.HasFlag(UnixPermissionType.Write))
                    otherPerms.Add(1);
                if (permissionDescriptors[2].Types.HasFlag(UnixPermissionType.Execute))
                    otherPerms.Add(2);

                // Get current special permissions
                List<int> specialPerms = [];
                if (specialPermissions.HasFlag(UnixPermissionSpecial.SetUid))
                    specialPerms.Add(0);
                if (specialPermissions.HasFlag(UnixPermissionSpecial.SetGid))
                    specialPerms.Add(1);
                if (specialPermissions.HasFlag(UnixPermissionSpecial.Sticky))
                    specialPerms.Add(2);

                // Permission change input modules
                InputModule[] modules =
                [
                    new MultiComboBoxModule()
                    {
                        // TODO: NKS_MISC_INTERACTIVES_FMTUI_USERPERMS_NAME -> User permissions
                        // TODO: NKS_MISC_INTERACTIVES_FMTUI_USERPERMS_DESC -> You can set file permissions for the owner here
                        Name = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_USERPERMS_NAME"),
                        Description = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_USERPERMS_DESC"),
                        Choices = permissionsInputs,
                        Value = userPerms.ToArray(),
                    },
                    new MultiComboBoxModule()
                    {
                        // TODO: NKS_MISC_INTERACTIVES_FMTUI_GROUPPERMS_NAME -> Group permissions
                        // TODO: NKS_MISC_INTERACTIVES_FMTUI_GROUPPERMS_DESC -> You can set file permissions for the group here
                        Name = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_GROUPPERMS_NAME"),
                        Description = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_GROUPPERMS_DESC"),
                        Choices = permissionsInputs,
                        Value = groupPerms.ToArray(),
                    },
                    new MultiComboBoxModule()
                    {
                        // TODO: NKS_MISC_INTERACTIVES_FMTUI_OTHERPERMS_NAME -> Other permissions
                        // TODO: NKS_MISC_INTERACTIVES_FMTUI_OTHERPERMS_DESC -> You can set file permissions for others here
                        Name = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_OTHERPERMS_NAME"),
                        Description = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_OTHERPERMS_DESC"),
                        Choices = permissionsInputs,
                        Value = otherPerms.ToArray(),
                    },
                    new MultiComboBoxModule()
                    {
                        // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIALPERMS_NAME -> Special permissions
                        // TODO: NKS_MISC_INTERACTIVES_FMTUI_SPECIALPERMS_DESC -> You can set special permissions for files or directories
                        Name = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIALPERMS_NAME"),
                        Description = LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_SPECIALPERMS_DESC"),
                        Choices = specialPermissionsInputs,
                        Value = specialPerms.ToArray(),
                    },
                ];

                // Open an infobox
                // TODO: NKS_MISC_INTERACTIVES_FMTUI_PERMSINFOBOX_NAME -> You can change the permissions for this file or directory here.
                bool done = InfoBoxMultiInputColor.WriteInfoBoxMultiInput(modules, LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERMSINFOBOX_NAME") + "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", infoBoxSettings);
                if (!done)
                    return;

                // Grab the changed permissions
                var finalUserPerms = modules[0].GetValue<int[]>() ?? [];
                var finalGroupPerms = modules[1].GetValue<int[]>() ?? [];
                var finalOtherPerms = modules[2].GetValue<int[]>() ?? [];
                var finalSpecialPerms = modules[3].GetValue<int[]>() ?? [];

                // Convert them to actual enum values
                var typeUser = GetUnixPermissionTypeFromArray(finalUserPerms);
                var typeGroup = GetUnixPermissionTypeFromArray(finalGroupPerms);
                var typeOther = GetUnixPermissionTypeFromArray(finalOtherPerms);
                var typeSpecial = GetUnixPermissionSpecialFromArray(finalSpecialPerms);

                // Update descriptors
                permissionDescriptors[0].Types = typeUser;
                permissionDescriptors[1].Types = typeGroup;
                permissionDescriptors[2].Types = typeOther;

                // Set permissions
                FilesystemTools.SetUnixFileMode(fileSystemEntry.FilePath, permissionDescriptors, typeSpecial);
            }
            catch (Exception ex)
            {
                InfoBoxModalColor.WriteInfoBoxModal(LanguageTools.GetLocalized("NKS_MISC_INTERACTIVES_FMTUI_PERMSSETFAILURE") + $": {ex.Message}", infoBoxSettings);
            }
        }

        private static UnixPermissionType GetUnixPermissionTypeFromArray(int[] indexes)
        {
            var permissionsEnum = Enum.GetValues<UnixPermissionType>();
            var type = UnixPermissionType.None;
            foreach (var i in indexes)
                type |= permissionsEnum[i + 1];
            return type;
        }

        private static UnixPermissionSpecial GetUnixPermissionSpecialFromArray(int[] indexes)
        {
            var permissionsEnum = Enum.GetValues<UnixPermissionSpecial>();
            var type = UnixPermissionSpecial.None;
            foreach (var i in indexes)
                type |= permissionsEnum[i + 1];
            return type;
        }
    }
}

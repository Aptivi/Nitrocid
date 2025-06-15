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
using System.Linq;
using System.Text;
using MailKit;
using MailKit.Net.Imap;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.ShellPacks.Shells.Mail;

namespace Nitrocid.ShellPacks.Tools.Directory
{
    /// <summary>
    /// Mail directory module
    /// </summary>
    public static class MailDirectory
    {

        /// <summary>
        /// Creates mail folder
        /// </summary>
        /// <param name="Directory">Directory name</param>
        public static void CreateMailDirectory(string Directory)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Creating folder: {0}", vars: [Directory]);
            try
            {
                MailFolder MailFolder;
                lock (((ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0]).SyncRoot)
                {
                    MailFolder = OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                    MailFolder.Create(Directory, true);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to create folder {0}: {1}", vars: [Directory, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MAILDIR_CREATEFAILED"), ex, Directory, ex.Message);
            }
        }

        /// <summary>
        /// Deletes mail folder
        /// </summary>
        /// <param name="Directory">Directory name</param>
        public static void DeleteMailDirectory(string Directory)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Deleting folder: {0}", vars: [Directory]);
            try
            {
                MailFolder MailFolder;
                lock (((ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0]).SyncRoot)
                {
                    MailFolder = OpenFolder(Directory);
                    MailFolder.Delete();
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to delete folder {0}: {1}", vars: [Directory, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MAILDIR_DELETEFAILED"), ex, Directory, ex.Message);
            }
        }

        /// <summary>
        /// Deletes mail folder
        /// </summary>
        /// <param name="Directory">Directory name</param>
        /// <param name="NewName">New mail directory name</param>
        public static void RenameMailDirectory(string Directory, string NewName)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Renaming folder {0} to {1}", vars: [Directory, NewName]);
            try
            {
                MailFolder MailFolder;
                lock (((ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0]).SyncRoot)
                {
                    MailFolder = OpenFolder(Directory);
                    MailFolder.Rename(MailFolder.ParentFolder, NewName);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to delete folder {0}: {1}", vars: [Directory, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MAILDIR_DELETEFAILED"), ex, Directory, ex.Message);
            }
        }

        /// <summary>
        /// Changes current mail directory
        /// </summary>
        /// <param name="Directory">A mail directory</param>
        public static void MailChangeDirectory(string Directory)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Opening folder: {0}", vars: [Directory]);
            try
            {
                lock (((ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0]).SyncRoot)
                    OpenFolder(Directory);
                MailShellCommon.IMAP_CurrentDirectory = Directory;
                DebugWriter.WriteDebug(DebugLevel.I, "Current directory changed.");
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to open folder {0}: {1}", vars: [Directory, ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_CANTOPENMAILFOLDER"), ex, Directory, ex.Message);
            }
        }

        /// <summary>
        /// Locates the normal (not special) folder and opens it.
        /// </summary>
        /// <param name="FolderString">A folder to open (not a path)</param>
        /// <param name="FolderMode">Folder mode</param>
        /// <returns>A folder</returns>
        public static MailFolder OpenFolder(string FolderString, FolderAccess FolderMode = FolderAccess.ReadWrite)
        {
            var Opened = default(MailFolder);
            var client = (ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0];
            DebugWriter.WriteDebug(DebugLevel.I, "Personal namespace collection parsing started.");
            foreach (FolderNamespace nmspc in client.PersonalNamespaces)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Namespace: {0}", vars: [nmspc.Path]);
                foreach (MailFolder dir in client.GetFolders(nmspc).Cast<MailFolder>())
                {
                    if (dir.Name.Equals(FolderString, StringComparison.OrdinalIgnoreCase))
                    {
                        dir.Open(FolderMode);
                        Opened = dir;
                    }
                }
            }

            DebugWriter.WriteDebug(DebugLevel.I, "Shared namespace collection parsing started.");
            foreach (FolderNamespace nmspc in client.SharedNamespaces)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Namespace: {0}", vars: [nmspc.Path]);
                foreach (MailFolder dir in client.GetFolders(nmspc).Cast<MailFolder>())
                {
                    if (dir.Name.Equals(FolderString, StringComparison.OrdinalIgnoreCase))
                    {
                        dir.Open(FolderMode);
                        Opened = dir;
                    }
                }
            }

            DebugWriter.WriteDebug(DebugLevel.I, "Other namespace collection parsing started.");
            foreach (FolderNamespace nmspc in client.OtherNamespaces)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Namespace: {0}", vars: [nmspc.Path]);
                foreach (MailFolder dir in client.GetFolders(nmspc).Cast<MailFolder>())
                {
                    if (dir.Name.Equals(FolderString, StringComparison.OrdinalIgnoreCase))
                    {
                        dir.Open(FolderMode);
                        Opened = dir;
                    }
                }
            }

            if (Opened is not null)
            {
                return Opened;
            }
            else
            {
                throw new KernelException(KernelExceptionType.NoSuchMailDirectory, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_MAILDIR_DIRNOTFOUND"), FolderString);
            }
        }

        /// <summary>
        /// Lists directories
        /// </summary>
        /// <returns>A list of mail folder instances</returns>
        public static MailFolder[] MailListDirectories()
        {
            var client = (ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0];
            List<MailFolder> folders = [];
            lock (client.SyncRoot)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Personal namespace collection parsing started.");
                foreach (FolderNamespace nmspc in client.PersonalNamespaces)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Namespace: {0}", vars: [nmspc.Path]);
                    foreach (MailFolder dir in client.GetFolders(nmspc).Cast<MailFolder>())
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Folder: {0}", vars: [dir.Name]);
                        folders.Add(dir);
                    }
                }

                DebugWriter.WriteDebug(DebugLevel.I, "Shared namespace collection parsing started.");
                foreach (FolderNamespace nmspc in client.SharedNamespaces)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Namespace: {0}", vars: [nmspc.Path]);
                    foreach (MailFolder dir in client.GetFolders(nmspc).Cast<MailFolder>())
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Folder: {0}", vars: [dir.Name]);
                        folders.Add(dir);
                    }
                }

                DebugWriter.WriteDebug(DebugLevel.I, "Other namespace collection parsing started.");
                foreach (FolderNamespace nmspc in client.OtherNamespaces)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Namespace: {0}", vars: [nmspc.Path]);
                    foreach (MailFolder dir in client.GetFolders(nmspc).Cast<MailFolder>())
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Folder: {0}", vars: [dir.Name]);
                        folders.Add(dir);
                    }
                }
            }
            return [.. folders];
        }

        /// <summary>
        /// Renders a list of directories
        /// </summary>
        /// <returns>String list</returns>
        public static string MailRenderListDirectories()
        {
            var EntryBuilder = new StringBuilder();
            var client = (ImapClient)((object[]?)MailShellCommon.Client?.ConnectionInstance ?? [])[0];
            lock (client.SyncRoot)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Personal namespace collection parsing started.");
                foreach (FolderNamespace nmspc in client.PersonalNamespaces)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Namespace: {0}", vars: [nmspc.Path]);
                    EntryBuilder.AppendLine($"- {nmspc.Path}");
                    foreach (MailFolder dir in client.GetFolders(nmspc).Cast<MailFolder>())
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Folder: {0}", vars: [dir.Name]);
                        EntryBuilder.AppendLine($"  - {dir.Name}");
                    }
                }

                DebugWriter.WriteDebug(DebugLevel.I, "Shared namespace collection parsing started.");
                foreach (FolderNamespace nmspc in client.SharedNamespaces)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Namespace: {0}", vars: [nmspc.Path]);
                    EntryBuilder.AppendLine($"- {nmspc.Path}");
                    foreach (MailFolder dir in client.GetFolders(nmspc).Cast<MailFolder>())
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Folder: {0}", vars: [dir.Name]);
                        EntryBuilder.AppendLine($"  - {dir.Name}");
                    }
                }

                DebugWriter.WriteDebug(DebugLevel.I, "Other namespace collection parsing started.");
                foreach (FolderNamespace nmspc in client.OtherNamespaces)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Namespace: {0}", vars: [nmspc.Path]);
                    EntryBuilder.AppendLine($"- {nmspc.Path}");
                    foreach (MailFolder dir in client.GetFolders(nmspc).Cast<MailFolder>())
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Folder: {0}", vars: [dir.Name]);
                        EntryBuilder.AppendLine($"  - {dir.Name}");
                    }
                }
            }
            return EntryBuilder.ToString();
        }

    }
}

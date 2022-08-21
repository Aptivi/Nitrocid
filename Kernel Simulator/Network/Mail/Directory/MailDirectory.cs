using System;

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

using System.Text;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.Shells.Mail;
using MailKit;

namespace KS.Network.Mail.Directory
{
    public static class MailDirectory
    {

        /// <summary>
        /// Creates mail folder
        /// </summary>
        /// <param name="Directory">Directory name</param>
        public static void CreateMailDirectory(string Directory)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Creating folder: {0}", Directory);
            try
            {
                MailFolder MailFolder;
                lock (MailLogin.IMAP_Client.SyncRoot)
                {
                    MailFolder = OpenFolder(MailShellCommon.IMAP_CurrentDirectory);
                    MailFolder.Create(Directory, true);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed to create folder {0}: {1}", Directory, ex.Message);
                DebugWriter.WStkTrc(ex);
                throw new Kernel.Exceptions.MailException(Translate.DoTranslation("Unable to create mail folder {0}: {1}"), ex, Directory, ex.Message);
            }
        }

        /// <summary>
        /// Deletes mail folder
        /// </summary>
        /// <param name="Directory">Directory name</param>
        public static void DeleteMailDirectory(string Directory)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Deleting folder: {0}", Directory);
            try
            {
                MailFolder MailFolder;
                lock (MailLogin.IMAP_Client.SyncRoot)
                {
                    MailFolder = OpenFolder(Directory);
                    MailFolder.Delete();
                }
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed to delete folder {0}: {1}", Directory, ex.Message);
                DebugWriter.WStkTrc(ex);
                throw new Kernel.Exceptions.MailException(Translate.DoTranslation("Unable to delete mail folder {0}: {1}"), ex, Directory, ex.Message);
            }
        }

        /// <summary>
        /// Deletes mail folder
        /// </summary>
        /// <param name="Directory">Directory name</param>
        public static void RenameMailDirectory(string Directory, string NewName)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Renaming folder {0} to {1}", Directory, NewName);
            try
            {
                MailFolder MailFolder;
                lock (MailLogin.IMAP_Client.SyncRoot)
                {
                    MailFolder = OpenFolder(Directory);
                    MailFolder.Rename(MailFolder.ParentFolder, NewName);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed to delete folder {0}: {1}", Directory, ex.Message);
                DebugWriter.WStkTrc(ex);
                throw new Kernel.Exceptions.MailException(Translate.DoTranslation("Unable to delete mail folder {0}: {1}"), ex, Directory, ex.Message);
            }
        }

        /// <summary>
        /// Changes current mail directory
        /// </summary>
        /// <param name="Directory">A mail directory</param>
        public static void MailChangeDirectory(string Directory)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Opening folder: {0}", Directory);
            try
            {
                lock (MailLogin.IMAP_Client.SyncRoot)
                    OpenFolder(Directory);
                MailShellCommon.IMAP_CurrentDirectory = Directory;
                DebugWriter.Wdbg(DebugLevel.I, "Current directory changed.");
            }
            catch (Exception ex)
            {
                DebugWriter.Wdbg(DebugLevel.E, "Failed to open folder {0}: {1}", Directory, ex.Message);
                DebugWriter.WStkTrc(ex);
                throw new Kernel.Exceptions.MailException(Translate.DoTranslation("Unable to open mail folder {0}: {1}"), ex, Directory, ex.Message);
            }
        }

        /// <summary>
        /// Locates the normal (not special) folder and opens it.
        /// </summary>
        /// <param name="FolderString">A folder to open (not a path)</param>
        /// <returns>A folder</returns>
        public static MailFolder OpenFolder(string FolderString, FolderAccess FolderMode = FolderAccess.ReadWrite)
        {
            var Opened = default(MailFolder);
            DebugWriter.Wdbg(DebugLevel.I, "Personal namespace collection parsing started.");
            foreach (FolderNamespace nmspc in MailLogin.IMAP_Client.PersonalNamespaces)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Namespace: {0}", nmspc.Path);
                foreach (MailFolder dir in MailLogin.IMAP_Client.GetFolders(nmspc))
                {
                    if ((dir.Name.ToLower() ?? "") == (FolderString.ToLower() ?? ""))
                    {
                        dir.Open(FolderMode);
                        Opened = dir;
                    }
                }
            }

            DebugWriter.Wdbg(DebugLevel.I, "Shared namespace collection parsing started.");
            foreach (FolderNamespace nmspc in MailLogin.IMAP_Client.SharedNamespaces)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Namespace: {0}", nmspc.Path);
                foreach (MailFolder dir in MailLogin.IMAP_Client.GetFolders(nmspc))
                {
                    if ((dir.Name.ToLower() ?? "") == (FolderString.ToLower() ?? ""))
                    {
                        dir.Open(FolderMode);
                        Opened = dir;
                    }
                }
            }

            DebugWriter.Wdbg(DebugLevel.I, "Other namespace collection parsing started.");
            foreach (FolderNamespace nmspc in MailLogin.IMAP_Client.OtherNamespaces)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Namespace: {0}", nmspc.Path);
                foreach (MailFolder dir in MailLogin.IMAP_Client.GetFolders(nmspc))
                {
                    if ((dir.Name.ToLower() ?? "") == (FolderString.ToLower() ?? ""))
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
                throw new Kernel.Exceptions.NoSuchMailDirectoryException(Translate.DoTranslation("Mail folder {0} not found."), FolderString);
            }
        }

        /// <summary>
        /// Lists directories
        /// </summary>
        /// <returns>String list</returns>
        public static string MailListDirectories()
        {
            var EntryBuilder = new StringBuilder();
            lock (MailLogin.IMAP_Client.SyncRoot)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Personal namespace collection parsing started.");
                foreach (FolderNamespace nmspc in MailLogin.IMAP_Client.PersonalNamespaces)
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Namespace: {0}", nmspc.Path);
                    EntryBuilder.AppendLine($"- {nmspc.Path}");
                    foreach (MailFolder dir in MailLogin.IMAP_Client.GetFolders(nmspc))
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Folder: {0}", dir.Name);
                        EntryBuilder.AppendLine($"  - {dir.Name}");
                    }
                }

                DebugWriter.Wdbg(DebugLevel.I, "Shared namespace collection parsing started.");
                foreach (FolderNamespace nmspc in MailLogin.IMAP_Client.SharedNamespaces)
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Namespace: {0}", nmspc.Path);
                    EntryBuilder.AppendLine($"- {nmspc.Path}");
                    foreach (MailFolder dir in MailLogin.IMAP_Client.GetFolders(nmspc))
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Folder: {0}", dir.Name);
                        EntryBuilder.AppendLine($"  - {dir.Name}");
                    }
                }

                DebugWriter.Wdbg(DebugLevel.I, "Other namespace collection parsing started.");
                foreach (FolderNamespace nmspc in MailLogin.IMAP_Client.OtherNamespaces)
                {
                    DebugWriter.Wdbg(DebugLevel.I, "Namespace: {0}", nmspc.Path);
                    EntryBuilder.AppendLine($"- {nmspc.Path}");
                    foreach (MailFolder dir in MailLogin.IMAP_Client.GetFolders(nmspc))
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Folder: {0}", dir.Name);
                        EntryBuilder.AppendLine($"  - {dir.Name}");
                    }
                }
            }
            return EntryBuilder.ToString();
        }

    }
}

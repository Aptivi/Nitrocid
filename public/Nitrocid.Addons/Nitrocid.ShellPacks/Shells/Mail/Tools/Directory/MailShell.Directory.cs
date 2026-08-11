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

using MailKit;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.ShellPacks.Shells.Mail.Tools;
using Terminaux.Shell.Shells;

namespace Nitrocid.ShellPacks.Shells.Mail
{
    /// <summary>
    /// Mail directory module
    /// </summary>
    public partial class MailShell : BaseShell, IShell
    {
        /// <summary>
        /// Creates mail folder
        /// </summary>
        /// <param name="Directory">Directory name</param>
        public void CreateMailDirectory(string Directory)
        {
            // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED -> IMAP server is not connected
            if (ImapClient is null)
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED"));
            MailTools.CreateMailDirectory(ImapClient, Directory, IMAP_CurrentDirectory);
        }

        /// <summary>
        /// Deletes mail folder
        /// </summary>
        /// <param name="Directory">Directory name</param>
        public void DeleteMailDirectory(string Directory)
        {
            // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED -> IMAP server is not connected
            if (ImapClient is null)
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED"));
            MailTools.DeleteMailDirectory(ImapClient, Directory);
        }

        /// <summary>
        /// Deletes mail folder
        /// </summary>
        /// <param name="Directory">Directory name</param>
        /// <param name="NewName">New mail directory name</param>
        public void RenameMailDirectory(string Directory, string NewName)
        {
            // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED -> IMAP server is not connected
            if (ImapClient is null)
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED"));
            MailTools.RenameMailDirectory(ImapClient, Directory, NewName);
        }

        /// <summary>
        /// Changes current mail directory
        /// </summary>
        /// <param name="Directory">A mail directory</param>
        public void MailChangeDirectory(string Directory)
        {
            // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED -> IMAP server is not connected
            if (ImapClient is null)
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED"));
            MailTools.MailChangeDirectory(ImapClient, Directory);
            IMAP_CurrentDirectory = Directory;
        }

        /// <summary>
        /// Locates the normal (not special) folder and opens it.
        /// </summary>
        /// <param name="FolderString">A folder to open (not a path)</param>
        /// <param name="FolderMode">Folder mode</param>
        /// <returns>A folder</returns>
        public MailFolder OpenFolder(string FolderString, FolderAccess FolderMode = FolderAccess.ReadWrite)
        {
            // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED -> IMAP server is not connected
            if (ImapClient is null)
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED"));
            return MailTools.OpenFolder(ImapClient, FolderString, FolderMode);
        }

        /// <summary>
        /// Lists directories
        /// </summary>
        /// <returns>A list of mail folder instances</returns>
        public MailFolder[] MailListDirectories()
        {
            // TODO: NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED -> IMAP server is not connected
            if (ImapClient is null)
                throw new KernelException(KernelExceptionType.Mail, LanguageTools.GetLocalized("NKS_SHELLPACKS_MAIL_EXCEPTION_IMAPNOTCONNECTED"));
            return MailTools.MailListDirectories(ImapClient);
        }
    }
}

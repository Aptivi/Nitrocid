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
using System.IO;
using System.Threading;
using Nitrocid.Base.Files;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using SharpCompress.Archives;
using SharpCompress.Archives.GZip;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Readers;
using Terminaux.Inputs;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;

namespace Nitrocid.ShellPacks.Shells.Archive
{
    /// <summary>
    /// Archive shell class
    /// </summary>
    public partial class ArchiveShell : BaseShell, IShell
    {
        private FileStream? fileStream;
        private IArchive? archive;
        private string? currentDirectory;
        private string? currentArchiveDirectory;

        /// <inheritdoc/>
        public override string ShellType => "ArchiveShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        /// <summary>
        /// Current directory
        /// </summary>
        public string? CurrentDirectory
        {
            get => currentDirectory;
            set => currentDirectory = value;
        }

        /// <summary>
        /// Current archive directory
        /// </summary>
        public string? CurrentArchiveDirectory
        {
            get => currentArchiveDirectory;
            set => currentArchiveDirectory = value;
        }

        /// <summary>
        /// Archive instance
        /// </summary>
        public IArchive? Archive =>
            archive;

        /// <summary>
        /// File stream
        /// </summary>
        public FileStream? FileStream =>
            fileStream;

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Set current directory for RAR shell
            CurrentDirectory = FilesystemTools.CurrentDir;

            // Get file path
            string ArchiveFile = "";
            if (ShellArgs.Length > 0)
            {
                ArchiveFile = Convert.ToString(ShellArgs[0]) ?? "";
                if (string.IsNullOrEmpty(ArchiveFile))
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FILESHELLS_NEEDSFILE"), true, ThemeColorType.Error);
                    Bail = true;
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FILESHELLS_NEEDSFILE"), true, ThemeColorType.Error);
                Bail = true;
            }

            // Open file if not open
            // TODO: NKS_SHELLPACKS_ARCHIVE_EXCEPTION_FILESTREAMFAILED -> Opening archive file stream failed
            fileStream ??= new FileStream(ArchiveFile, FileMode.Open);
            if (FileStream is null)
                throw new KernelException(KernelExceptionType.Archive, LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_EXCEPTION_FILESTREAMFAILED"));
            ArchiveType type = ReaderFactory.OpenReader(FileStream).Type;

            // Select archive type and open it
            switch (type)
            {
                case ArchiveType.Rar:
                    archive ??= RarArchive.OpenArchive(FileStream);
                    break;
                case ArchiveType.Zip:
                    archive ??= ZipArchive.OpenArchive(FileStream);
                    break;
                case ArchiveType.GZip:
                    archive ??= GZipArchive.OpenArchive(FileStream);
                    break;
                case ArchiveType.SevenZip:
                    archive ??= SevenZipArchive.OpenArchive(FileStream);
                    break;
                case ArchiveType.Tar:
                    archive ??= TarArchive.OpenArchive(FileStream);
                    break;
                default:
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_ARCHIVE_EXCEPTION_TYPENOTSUPPORTED") + $" {type}", true, ThemeColorType.Error);
                    Bail = true;
                    break;
            }

            while (!Bail)
            {
                try
                {
                    // Prompt for the command
                    ShellManager.GetLine();
                }
                catch (ThreadInterruptedException)
                {
                    CancellationHandlers.DismissRequest();
                    Bail = true;
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_SHELL_ERROR") + " {0}", true, ThemeColorType.Error, ex.Message);
                    DebugWriter.WriteDebug(DebugLevel.E, "Shell will have to exit: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    Input.ReadKey();
                    Bail = true;
                }
            }

            // Close file stream
            Archive?.Dispose();
            FileStream?.Close();
        }
    }
}

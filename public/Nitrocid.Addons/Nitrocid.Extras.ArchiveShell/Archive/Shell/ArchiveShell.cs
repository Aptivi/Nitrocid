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
using System.IO;
using System.Threading;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Archives.Zip;
using SharpCompress.Archives.GZip;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Archives.Tar;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Nitrocid.Languages;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Debugging;
using Textify.General;
using Nitrocid.Files;
using Nitrocid.ConsoleBase.Inputs;

namespace Nitrocid.Extras.ArchiveShell.Archive.Shell
{
    /// <summary>
    /// Archive shell class
    /// </summary>
    public class ArchiveShell : BaseShell, IShell
    {
        /// <inheritdoc/>
        public override string ShellType => "ArchiveShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Set current directory for RAR shell
            ArchiveShellCommon.CurrentDirectory = FilesystemTools.CurrentDir;

            // Get file path
            string ArchiveFile = "";
            if (ShellArgs.Length > 0)
            {
                ArchiveFile = Convert.ToString(ShellArgs[0]) ?? "";
                if (string.IsNullOrEmpty(ArchiveFile))
                {
                    TextWriters.Write(Translate.DoTranslation("File not specified. Exiting shell..."), true, KernelColorType.Error);
                    Bail = true;
                }
            }
            else
            {
                TextWriters.Write(Translate.DoTranslation("File not specified. Exiting shell..."), true, KernelColorType.Error);
                Bail = true;
            }

            // Open file if not open
            ArchiveShellCommon.FileStream ??= new FileStream(ArchiveFile, FileMode.Open);
            ArchiveType type = ReaderFactory.Open(ArchiveShellCommon.FileStream).ArchiveType;

            // Select archive type and open it
            switch (type)
            {
                case ArchiveType.Rar:
                    ArchiveShellCommon.Archive ??= RarArchive.Open(ArchiveShellCommon.FileStream);
                    break;
                case ArchiveType.Zip:
                    ArchiveShellCommon.Archive ??= ZipArchive.Open(ArchiveShellCommon.FileStream);
                    break;
                case ArchiveType.GZip:
                    ArchiveShellCommon.Archive ??= GZipArchive.Open(ArchiveShellCommon.FileStream);
                    break;
                case ArchiveType.SevenZip:
                    ArchiveShellCommon.Archive ??= SevenZipArchive.Open(ArchiveShellCommon.FileStream);
                    break;
                case ArchiveType.Tar:
                    ArchiveShellCommon.Archive ??= TarArchive.Open(ArchiveShellCommon.FileStream);
                    break;
                default:
                    TextWriters.Write(Translate.DoTranslation("This archive type is not supported.") + $" {type}", true, KernelColorType.Error);
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
                    CancellationHandlers.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    TextWriters.Write(Translate.DoTranslation("There was an error in the shell.") + " {0}", true, KernelColorType.Error, ex.Message);
                    DebugWriter.WriteDebug(DebugLevel.E, "Shell will have to exit: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    InputTools.DetectKeypress();
                    Bail = true;
                }
            }

            // Close file stream
            ArchiveShellCommon.Archive?.Dispose();
            ArchiveShellCommon.FileStream?.Close();
            ArchiveShellCommon.CurrentDirectory = "";
            ArchiveShellCommon.CurrentArchiveDirectory = "";
            ArchiveShellCommon.Archive = null;
            ArchiveShellCommon.FileStream = null;
        }
    }
}

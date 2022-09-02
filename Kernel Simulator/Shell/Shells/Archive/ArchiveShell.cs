
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

using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files.Folders;
using KS.Kernel.Debugging;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using System;
using System.IO;
using System.Threading;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;
using SharpCompress.Readers;
using SharpCompress.Archives.Zip;

namespace KS.Shell.Shells.Archive
{
    /// <summary>
    /// Archive shell class
    /// </summary>
    public class ArchiveShell : ShellExecutor, IShell
    {
        public override ShellType ShellType
        {
            get
            {
                return ShellType.ArchiveShell;
            }
        }

        public override bool Bail { get; set; }

        public override void InitializeShell(params object[] ShellArgs)
        {
            // Set current directory for RAR shell
            ArchiveShellCommon.ArchiveShell_CurrentDirectory = CurrentDirectory.CurrentDir;

            // Get file path
            string ArchiveFile = "";
            if (ShellArgs.Length > 0)
            {
                ArchiveFile = Convert.ToString(ShellArgs[0]);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("File not specified. Exiting shell..."), true, ColorTools.ColTypes.Error);
                Bail = true;
            }

            // Open file if not open
            ArchiveShellCommon.ArchiveShell_FileStream ??= new FileStream(ArchiveFile, FileMode.Open);
            ArchiveType type = ReaderFactory.Open(ArchiveShellCommon.ArchiveShell_FileStream).ArchiveType;
            while (!Bail)
            {
                try
                {
                    // Select archive type and open it
                    switch (type)
                    {
                        case ArchiveType.Rar:
                            ArchiveShellCommon.ArchiveShell_Archive ??= RarArchive.Open(ArchiveShellCommon.ArchiveShell_FileStream);
                            break;
                        case ArchiveType.Zip:
                            ArchiveShellCommon.ArchiveShell_Archive ??= ZipArchive.Open(ArchiveShellCommon.ArchiveShell_FileStream);
                            break;
                    }

                    // See UESHShell.cs for more info
                    lock (CancellationHandlers.GetCancelSyncLock(ShellType))
                    {
                        // Prepare for prompt
                        PromptPresetManager.WriteShellPrompt(ShellType);

                        // Raise the event
                        Kernel.Kernel.KernelEventManager.RaiseArchiveShellInitialized();
                    }

                    // Prompt for the command
                    string WrittenCommand = Input.ReadLine();
                    if ((string.IsNullOrEmpty(WrittenCommand) | (WrittenCommand?.StartsWithAnyOf(new[] { " ", "#" }))) == false)
                    {
                        Kernel.Kernel.KernelEventManager.RaiseArchivePreExecuteCommand(WrittenCommand);
                        Shell.GetLine(WrittenCommand, "", ShellType.ArchiveShell);
                        Kernel.Kernel.KernelEventManager.RaiseArchivePostExecuteCommand(WrittenCommand);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    Flags.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriterColor.Write(Translate.DoTranslation("There was an error in the shell.") + Kernel.Kernel.NewLine + "Error {0}: {1}", true, ColorTools.ColTypes.Error, ex.GetType().FullName, ex.Message);
                    continue;
                }
            }

            // Close file stream
            ArchiveShellCommon.ArchiveShell_Archive.Dispose();
            ArchiveShellCommon.ArchiveShell_FileStream.Close();
            ArchiveShellCommon.ArchiveShell_CurrentDirectory = "";
            ArchiveShellCommon.ArchiveShell_CurrentArchiveDirectory = "";
            ArchiveShellCommon.ArchiveShell_Archive = null;
            ArchiveShellCommon.ArchiveShell_FileStream = null;
        }
    }
}


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

using System;
using System.IO;
using System.Threading;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files.Folders;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Shell.Prompts;
using KS.Shell.ShellBase;
using KS.Shell.ShellBase.Shells;
using SharpCompress.Archives.Rar;

namespace KS.Shell.Shells.Rar
{
    public class RarShell : ShellExecutor, IShell
    {

        public override ShellType ShellType
        {
            get
            {
                return ShellType.RARShell;
            }
        }

        public override bool Bail { get; set; }

        public override void InitializeShell(params object[] ShellArgs)
        {
            // Set current directory for RAR shell
            RarShellCommon.RarShell_CurrentDirectory = CurrentDirectory.CurrentDir;

            // Get file path
            string RarFile = "";
            if (ShellArgs.Length > 0)
            {
                RarFile = Convert.ToString(ShellArgs[0]);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("File not specified. Exiting shell..."), true, ColorTools.ColTypes.Error);
                Bail = true;
            }

            while (!Bail)
            {
                try
                {
                    // Open file if not open
                    RarShellCommon.RarShell_FileStream ??= new FileStream(RarFile, FileMode.Open);
                    RarShellCommon.RarShell_RarArchive ??= RarArchive.Open(RarShellCommon.RarShell_FileStream);

                    // See UESHShell.cs for more info
                    lock (CancellationHandlers.GetCancelSyncLock(ShellType))
                    {
                        // Prepare for prompt
                        if (Kernel.Kernel.DefConsoleOut is not null)
                        {
                            ConsoleBase.ConsoleWrapper.SetOut(Kernel.Kernel.DefConsoleOut);
                        }
                        PromptPresetManager.WriteShellPrompt(ShellType);

                        // Raise the event
                        Kernel.Kernel.KernelEventManager.RaiseRarShellInitialized();
                    }

                    // Prompt for the command
                    string WrittenCommand = Input.ReadLine();
                    if ((string.IsNullOrEmpty(WrittenCommand) | (WrittenCommand?.StartsWithAnyOf(new[] { " ", "#" }))) == false)
                    {
                        Kernel.Kernel.KernelEventManager.RaiseRarPreExecuteCommand(WrittenCommand);
                        Shell.GetLine(WrittenCommand, "", ShellType.RARShell);
                        Kernel.Kernel.KernelEventManager.RaiseRarPostExecuteCommand(WrittenCommand);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    Flags.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WStkTrc(ex);
                    TextWriterColor.Write(Translate.DoTranslation("There was an error in the shell.") + Kernel.Kernel.NewLine + "Error {0}: {1}", true, ColorTools.ColTypes.Error, ex.GetType().FullName, ex.Message);
                    continue;
                }
            }

            // Close file stream
            RarShellCommon.RarShell_RarArchive.Dispose();
            RarShellCommon.RarShell_CurrentDirectory = "";
            RarShellCommon.RarShell_CurrentArchiveDirectory = "";
            RarShellCommon.RarShell_RarArchive = null;
            RarShellCommon.RarShell_FileStream = null;
        }

    }
}
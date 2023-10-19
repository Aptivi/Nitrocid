
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files.Editors.HexEdit;
using KS.Kernel.Configuration;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.Hex
{
    /// <summary>
    /// The hex editor class
    /// </summary>
    public class HexShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "HexShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Get file path
            string FilePath = "";
            if (ShellArgs.Length > 0)
            {
                FilePath = Convert.ToString(ShellArgs[0]);
            }
            else
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("File not specified. Exiting shell..."), true, KernelColorType.Error);
                Bail = true;
            }
            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Please note that editing binary files using this shell is experimental and may lead to data corruption or data loss if not used properly.") + CharManager.NewLine + Translate.DoTranslation("DON'T LAUNCH THE SHELL UNLESS YOU KNOW WHAT YOU'RE DOING!"), true, KernelColorType.Warning);

            // Actual shell logic
            while (!Bail)
            {
                try
                {
                    // Open file if not open
                    if (HexEditShellCommon.HexEdit_FileStream is null)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "File not open yet. Trying to open {0}...", FilePath);
                        if (!HexEditTools.HexEdit_OpenBinaryFile(FilePath))
                        {
                            TextWriterColor.WriteKernelColor(Translate.DoTranslation("Failed to open file. Exiting shell..."), true, KernelColorType.Error);
                            Bail = true;
                            break;
                        }
                        HexEditShellCommon.HexEdit_AutoSave.Start();
                    }

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
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("There was an error in the shell.") + CharManager.NewLine + "Error {0}: {1}", true, KernelColorType.Error, ex.GetType().FullName, ex.Message);
                    continue;
                }
            }

            // Close file
            HexEditTools.HexEdit_CloseBinaryFile();
            HexEditShellCommon.HexEdit_AutoSave.Stop();
        }

    }
}

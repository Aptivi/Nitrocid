
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
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Editors.TextEdit;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.Text
{
    /// <summary>
    /// The text editor shell
    /// </summary>
    public class TextShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "TextShell";

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
                TextWriterColor.Write(Translate.DoTranslation("File not specified. Exiting shell..."), true, KernelColorType.Error);
                Bail = true;
            }

            // Actual shell logic
            while (!Bail)
            {
                try
                {
                    // Open file if not open
                    if (TextEditShellCommon.TextEdit_FileStream is null)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "File not open yet. Trying to open {0}...", FilePath);
                        if (!TextEditTools.TextEdit_OpenTextFile(FilePath))
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Failed to open file. Exiting shell..."), true, KernelColorType.Error);
                            Bail = true;
                            break;
                        }
                        TextEditShellCommon.TextEdit_AutoSave.Start();
                    }
                    // Prompt for the command
                    ShellManager.GetLine();
                }
                catch (ThreadInterruptedException)
                {
                    Flags.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriterColor.Write(Translate.DoTranslation("There was an error in the shell.") + CharManager.NewLine + "Error {0}: {1}", true, KernelColorType.Error, ex.GetType().FullName, ex.Message);
                    continue;
                }
            }

            // Close file
            TextEditTools.TextEdit_CloseTextFile();
            TextEditShellCommon.TextEdit_AutoSave.Stop();
        }

    }
}

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
using System.Threading;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Files.Editors.TextEdit;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;
using Nitrocid.Shell.ShellBase.Shells;
using Textify.General;

namespace Nitrocid.Shell.Shells.Text
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
                FilePath = Convert.ToString(ShellArgs[0]) ?? "";
                if (string.IsNullOrEmpty(FilePath))
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEXTEXT_NEEDSFILE"), true, KernelColorType.Error);
                    Bail = true;
                }
            }
            else
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEXTEXT_NEEDSFILE"), true, KernelColorType.Error);
                Bail = true;
            }

            // Open file if not open
            if (TextEditShellCommon.FileStream is null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "File not open yet. Trying to open {0}...", vars: [FilePath]);
                if (!TextEditTools.OpenTextFile(FilePath))
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEXTEXT_CANTOPEN"), true, KernelColorType.Error);
                    Bail = true;
                }
                TextEditShellCommon.AutoSave.Start();
            }

            // Actual shell logic
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
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ERRORINSHELL") + CharManager.NewLine + "Error {0}: {1}", true, KernelColorType.Error, ex.GetType().FullName ?? "<null>", ex.Message);
                    continue;
                }
            }

            // Close file
            TextEditTools.CloseTextFile();
            TextEditShellCommon.AutoSave.Stop();
        }

    }
}

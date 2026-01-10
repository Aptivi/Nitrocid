//
// Nitrocid KS  Copyright (C) 2018-2026  Aptivi
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
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Textify.General;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Files.Editors.HexEdit;
using Nitrocid.Base.ConsoleBase.Inputs;

namespace Nitrocid.Base.Shell.Shells.Hex
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
                FilePath = Convert.ToString(ShellArgs[0]) ?? "";
                if (string.IsNullOrEmpty(FilePath))
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEXTEXT_NEEDSFILE"), true, ThemeColorType.Error);
                    Bail = true;
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEXTEXT_NEEDSFILE"), true, ThemeColorType.Error);
                Bail = true;
            }
            TextWriterColor.Write(
                LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_WARNING1") + CharManager.NewLine +
                LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEX_WARNING2")
                , true, ThemeColorType.Warning);

            // Open file if not open
            if (HexEditShellCommon.FileStream is null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "File not open yet. Trying to open {0}...", vars: [FilePath]);
                if (!HexEditTools.OpenBinaryFile(FilePath))
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEXTEXT_CANTOPEN"), true, ThemeColorType.Error);
                    Bail = true;
                }
                HexEditShellCommon.AutoSave.Start();
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
                    CancellationHandlers.DismissRequest();
                    Bail = true;
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ERRORINSHELL") + " {0}", true, ThemeColorType.Error, ex.Message);
                    DebugWriter.WriteDebug(DebugLevel.E, "Shell will have to exit: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                    InputTools.DetectKeypress();
                    Bail = true;
                }
            }

            // Close file
            HexEditTools.CloseBinaryFile();
            HexEditShellCommon.AutoSave.Stop();
        }

    }
}

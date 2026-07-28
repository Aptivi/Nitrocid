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
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Nitrocid.Base.Files.Editors.TextEdit;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Terminaux.Inputs;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Threadify.Manager;

namespace Nitrocid.Base.Shell.Shells.Text
{
    /// <summary>
    /// The text editor shell
    /// </summary>
    public partial class TextShell : BaseShell, IShell
    {
        internal List<string> fileLines = [];
        internal FileStream? fileStream;
        internal List<string> FileLinesOrig = [];
        internal ThreadInstance AutoSave = new("Text Edit Autosave Thread", false, new ParameterizedThreadStart((shell) => HandleAutoSaveTextFile((TextShell?)shell)));

        /// <summary>
        /// File lines for text editor
        /// </summary>
        public List<string> FileLines =>
            fileLines;

        /// <summary>
        /// File stream for text editor
        /// </summary>
        public FileStream? FileStream =>
            fileStream;

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
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEXTEXT_NEEDSFILE"), true, ThemeColorType.Error);
                    Bail = true;
                }
            }
            else
            {
                TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEXTEXT_NEEDSFILE"), true, ThemeColorType.Error);
                Bail = true;
            }

            // Open file if not open
            if (FileStream is null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "File not open yet. Trying to open {0}...", vars: [FilePath]);
                if (!OpenTextFile(FilePath))
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_HEXTEXT_CANTOPEN"), true, ThemeColorType.Error);
                    Bail = true;
                }
                AutoSave.Start(this);
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
                    Input.ReadKey();
                    Bail = true;
                }
            }

            // Close file
            CloseTextFile();
            AutoSave.Stop();
        }

    }
}

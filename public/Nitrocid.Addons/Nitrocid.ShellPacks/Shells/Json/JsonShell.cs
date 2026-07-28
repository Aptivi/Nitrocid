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
using Newtonsoft.Json.Linq;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Languages;
using Terminaux.Inputs;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Terminaux.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Threadify.Manager;

namespace Nitrocid.ShellPacks.Shells.Json
{
    /// <summary>
    /// The JSON editor shell
    /// </summary>
    public partial class JsonShell : BaseShell, IShell
    {
        internal JToken FileTokenOrig = JToken.Parse("{}");
        internal FileStream? FileStream;
        internal ThreadInstance AutoSave = new("JSON Shell Autosave Thread", false, new ParameterizedThreadStart((shell) => HandleAutoSaveJsonFile((JsonShell?)shell)));
        internal int autoSaveInterval = 60;

        /// <summary>
        /// JSON shell file token
        /// </summary>
        public JToken FileToken { get; set; } = JToken.Parse("{}");

        /// <inheritdoc/>
        public override string ShellType => "JsonShell";

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
            if (FileStream is null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "File not open yet. Trying to open {0}...", vars: [FilePath]);
                if (!OpenJsonFile(FilePath))
                {
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FILESHELLS_OPENFAILED"), true, ThemeColorType.Error);
                    Bail = true;
                }
                AutoSave.Start(this);
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

            // Close file
            CloseJsonFile();
            AutoSave.Stop();
        }

    }
}

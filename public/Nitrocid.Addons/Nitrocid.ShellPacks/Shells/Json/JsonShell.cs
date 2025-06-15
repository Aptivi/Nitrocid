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
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.ShellPacks.Tools;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Shells;

namespace Nitrocid.ShellPacks.Shells.Json
{
    /// <summary>
    /// The JSON editor shell
    /// </summary>
    public class JsonShell : BaseShell, IShell
    {

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
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FILESHELLS_NEEDSFILE"), true, KernelColorType.Error);
                    Bail = true;
                }
            }
            else
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FILESHELLS_NEEDSFILE"), true, KernelColorType.Error);
                Bail = true;
            }

            // Open file if not open
            if (JsonShellCommon.FileStream is null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "File not open yet. Trying to open {0}...", vars: [FilePath]);
                if (!JsonTools.OpenJsonFile(FilePath))
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FILESHELLS_OPENFAILED"), true, KernelColorType.Error);
                    Bail = true;
                }
                JsonShellCommon.AutoSave.Start();
            }

            while (!Bail)
            {
                // Prompt for the command
                ShellManager.GetLine();
            }

            // Close file
            JsonTools.CloseJsonFile();
            JsonShellCommon.AutoSave.Stop();
        }

    }
}

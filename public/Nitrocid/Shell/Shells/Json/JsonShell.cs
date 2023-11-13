﻿//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Files.Editors.JsonShell;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.Json
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
                FilePath = Convert.ToString(ShellArgs[0]);
            }
            else
            {
                TextWriterColor.WriteKernelColor(Translate.DoTranslation("File not specified. Exiting shell..."), true, KernelColorType.Error);
                Bail = true;
            }

            // Open file if not open
            if (JsonShellCommon.FileStream is null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "File not open yet. Trying to open {0}...", FilePath);
                if (!JsonTools.OpenJsonFile(FilePath))
                {
                    TextWriterColor.WriteKernelColor(Translate.DoTranslation("Failed to open file. Exiting shell..."), true, KernelColorType.Error);
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
            JsonTools.CloseTextFile();
            JsonShellCommon.AutoSave.Stop();
        }

    }
}
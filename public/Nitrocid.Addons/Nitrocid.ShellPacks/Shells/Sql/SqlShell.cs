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
using Nitrocid.ShellPacks.Tools;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;
using Nitrocid.Kernel.Debugging;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Languages;
using Terminaux.Colors.Themes.Colors;
using Textify.General;

namespace Nitrocid.ShellPacks.Shells.Sql
{
    /// <summary>
    /// The SQL editor shell
    /// </summary>
    public class SqlShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "SqlShell";

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
            }
            else
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FILESHELLS_NEEDSFILE"), true, ThemeColorType.Error);
                Bail = true;
            }

            // Open file if not open
            if (SqlShellCommon.sqliteConnection is null)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "File not open yet. Trying to open {0}...", vars: [FilePath]);
                if (!SqlEditTools.SqlEdit_OpenSqlFile(FilePath))
                {
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_FILESHELLS_OPENFAILED"), true, ThemeColorType.Error);
                    Bail = true;
                }
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
                    DebugWriter.WriteDebugStackTrace(ex);
                    TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELLPACKS_COMMON_SHELL_ERROR") + CharManager.NewLine + "Error {0}: {1}", true, ThemeColorType.Error, ex.GetType().FullName ?? "<null>", ex.Message);
                    continue;
                }
            }

            // Close file
            SqlEditTools.SqlEdit_CloseSqlFile();
        }

    }
}

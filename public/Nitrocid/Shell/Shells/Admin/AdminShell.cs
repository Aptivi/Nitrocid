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
using Terminaux.Colors.Themes.Colors;
using Terminaux.Writer.ConsoleWriters;
using Nitrocid.Kernel.Debugging;
using Nitrocid.Languages;
using Nitrocid.Security.Permissions;
using Terminaux.Shell.Commands;
using Terminaux.Shell.Shells;

namespace Nitrocid.Shell.Shells.Admin
{
    /// <summary>
    /// The admin shell
    /// </summary>
    public class AdminShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "AdminShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Check the user permission
            PermissionsTools.Demand(PermissionTypes.OpenAdminShell);

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
                    TextWriterColor.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_ADMIN_ERROR"), true, ThemeColorType.Error, ex.Message);
                    DebugWriter.WriteDebug(DebugLevel.E, "Error: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
        }

    }
}

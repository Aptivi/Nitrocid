//
// Nitrocid KS  Copyright (C) 2018-2024  Aptivi
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
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Writers;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Screensaver;
using KS.Misc.Text;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;
using KS.Users.Login;

namespace KS.Shell.Shells.UESH
{
    /// <summary>
    /// The UESH shell
    /// </summary>
    public class UESHShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "Shell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            while (!Bail)
            {
                if (Login.LogoutRequested)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Requested log out: {0}", Login.LogoutRequested);
                    Login.LogoutRequested = false;
                    Login.LoggedIn = false;
                    Bail = true;
                }
                else if (!ScreensaverManager.InSaver)
                {
                    try
                    {
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
                        TextWriters.Write(Translate.DoTranslation("There was an error in the shell.") + CharManager.NewLine + "Error {0}: {1}", true, KernelColorType.Error, ex.GetType().FullName, ex.Message);
                        continue;
                    }
                }
            }
        }

    }
}

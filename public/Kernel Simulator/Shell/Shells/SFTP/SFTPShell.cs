
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Threading;
using KS.Files;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Kernel.Exceptions;
using KS.Languages;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.SFTP
{
    /// <summary>
    /// The SFTP shell
    /// </summary>
    public class SFTPShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "SFTPShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Parse shell arguments
            bool Connects = ShellArgs.Length > 0;
            string Address = "";
            if (Connects)
                Address = Convert.ToString(ShellArgs[0]);

            // Actual shell logic
            string SFTPStrCmd;
            var SFTPInitialized = false;
            while (!Bail)
            {
                try
                {
                    // Complete initialization
                    if (SFTPInitialized == false)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, $"Completing initialization of SFTP: {SFTPInitialized}");
                        SFTPShellCommon.SFTPCurrDirect = Paths.HomePath;
                        SFTPInitialized = true;
                    }

                    // Check if the shell is going to exit
                    if (Bail)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Exiting shell...");
                        SFTPShellCommon.SFTPConnected = false;
                        SFTPShellCommon.ClientSFTP?.Disconnect();
                        SFTPShellCommon.SFTPSite = "";
                        SFTPShellCommon.SFTPCurrDirect = "";
                        SFTPShellCommon.SFTPCurrentRemoteDir = "";
                        SFTPShellCommon.SFTPUser = "";
                        SFTPShellCommon.SFTPPass = "";
                        SFTPInitialized = false;
                        return;
                    }

                    // Try to connect if IP address is specified.
                    if (Connects)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, $"Currently connecting to {Address} by \"sftp (address)\"...");
                        SFTPStrCmd = $"connect {Address}";
                        Connects = false;
                        Shell.GetLine(SFTPStrCmd, "", ShellType);
                    }
                    else
                    {
                        // Prompt for the command
                        Shell.GetLine();
                    }
                }
                catch (ThreadInterruptedException)
                {
                    Flags.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebugStackTrace(ex);
                    throw new KernelException(KernelExceptionType.SFTPShell, Translate.DoTranslation("There was an error in the SFTP shell:") + " {0}", ex, ex.Message);
                }
            }
        }

    }
}

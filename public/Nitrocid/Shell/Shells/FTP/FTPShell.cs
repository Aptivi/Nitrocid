
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

namespace KS.Shell.Shells.FTP
{
    /// <summary>
    /// The FTP shell
    /// </summary>
    public class FTPShell : BaseShell, IShell
    {

        /// <inheritdoc/>
        public override string ShellType => "FTPShell";

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            // Parse shell arguments
            string FtpCommand;
            bool Connects = ShellArgs.Length > 0;
            string Address = "";
            if (Connects)
                Address = Convert.ToString(ShellArgs[0]);

            // Populate FTP current directory
            FTPShellCommon.FtpCurrentDirectory = Paths.HomePath;

            // Actual shell logic
            while (!Bail)
            {
                try
                {
                    // Check if the shell is going to exit
                    if (Bail)
                    {
                        DebugWriter.WriteDebug(DebugLevel.W, "Exiting shell...");
                        FTPShellCommon.FtpConnected = false;
                        FTPShellCommon.ClientFTP?.Disconnect();
                        FTPShellCommon.FtpSite = "";
                        FTPShellCommon.FtpCurrentDirectory = "";
                        FTPShellCommon.FtpCurrentRemoteDir = "";
                        FTPShellCommon.FtpUser = "";
                        FTPShellCommon.FtpPass = "";
                        return;
                    }

                    // Try to connect if IP address is specified.
                    if (Connects)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, $"Currently connecting to {Address} by \"ftp (address)\"...");
                        FtpCommand = $"connect {Address}";
                        Connects = false;
                        Shell.GetLine(FtpCommand, "", ShellType);
                    }
                    else
                    {
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
                    throw new KernelException(KernelExceptionType.FTPShell, Translate.DoTranslation("There was an error in the FTP shell:") + " {0}", ex, ex.Message);
                }
            }
        }

    }
}

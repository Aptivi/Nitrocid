﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Threading;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Text;
using KS.Misc.Writers.DebugWriters;
using KS.Network.FTP;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells
{
    public class FTPShell : ShellExecutor, IShell
    {

        private bool FtpInitialized;

        public override ShellType ShellType => ShellType.FTPShell;

        public override bool Bail { get; set; }

        public override void InitializeShell(params object[] ShellArgs)
        {
            // Parse shell arguments
            string FtpCommand;
            bool Connects = ShellArgs.Length > 0;
            string Address = "";
            if (Connects)
                Address = Convert.ToString(ShellArgs[0]);

            // Actual shell logic
            while (!Bail)
            {
                try
                {
                    // Complete initialization
                    if (FtpInitialized == false)
                    {
                        DebugWriter.Wdbg(DebugLevel.I, $"Completing initialization of FTP: {FtpInitialized}");
                        FTPShellCommon.FtpCurrentDirectory = Paths.HomePath;
                        Kernel.Kernel.KernelEventManager.RaiseFTPShellInitialized();
                        FtpInitialized = true;
                    }

                    // Check if the shell is going to exit
                    if (Bail)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Exiting shell...");
                        FTPShellCommon.FtpConnected = false;
                        FTPShellCommon.ClientFTP?.Disconnect();
                        FTPShellCommon.FtpSite = "";
                        FTPShellCommon.FtpCurrentDirectory = "";
                        FTPShellCommon.FtpCurrentRemoteDir = "";
                        FTPShellCommon.FtpUser = "";
                        FTPShellCommon.FtpPass = "";
                        FtpInitialized = false;
                        return;
                    }

                    // See UESHShell.vb for more info
                    lock (CancellationHandlers.GetCancelSyncLock(ShellType))
                    {
                        // Restore console state
                        if (Kernel.Kernel.DefConsoleOut is not null)
                        {
                            Console.SetOut(Kernel.Kernel.DefConsoleOut);
                        }

                        // Prompt for command
                        if (!Connects)
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Preparing prompt...");
                            PromptPresetManager.WriteShellPrompt(ShellType);
                        }
                    }

                    // Try to connect if IP address is specified.
                    if (Connects)
                    {
                        DebugWriter.Wdbg(DebugLevel.I, $"Currently connecting to {Address} by \"ftp (address)\"...");
                        FtpCommand = $"connect {Address}";
                        Connects = false;
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Normal shell");
                        FtpCommand = Input.ReadLine();
                    }

                    // Parse command
                    if ((string.IsNullOrEmpty(FtpCommand) | (FtpCommand?.StartsWithAnyOf([" ", "#"]))) == false)
                    {
                        Kernel.Kernel.KernelEventManager.RaiseFTPPreExecuteCommand(FtpCommand);
                        Shell.GetLine(FtpCommand, false, "", ShellType.FTPShell);
                        Kernel.Kernel.KernelEventManager.RaiseFTPPostExecuteCommand(FtpCommand);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    Flags.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    DebugWriter.WStkTrc(ex);
                    throw new Kernel.Exceptions.FTPShellException(Translate.DoTranslation("There was an error in the FTP shell:") + " {0}", ex, ex.Message);
                }
            }
        }

    }
}
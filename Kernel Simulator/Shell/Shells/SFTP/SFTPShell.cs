
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.SFTP
{
    public class SFTPShell : ShellExecutor, IShell
    {

        public override ShellType ShellType
        {
            get
            {
                return ShellType.SFTPShell;
            }
        }

        public override bool Bail { get; set; }

        public override void InitializeShell(params object[] ShellArgs)
        {
            // Parse shell arguments
            bool Connects = ShellArgs.Length > 0;
            string Address = "";
            if (Connects)
                Address = Convert.ToString(ShellArgs[0]);

            // Actual shell logic
            string SFTPStrCmd;
            var SFTPInitialized = default(bool);
            while (!Bail)
            {
                try
                {
                    // Complete initialization
                    if (SFTPInitialized == false)
                    {
                        DebugWriter.Wdbg(DebugLevel.I, $"Completing initialization of SFTP: {SFTPInitialized}");
                        SFTPShellCommon.SFTPCurrDirect = Paths.HomePath;
                        Kernel.Kernel.KernelEventManager.RaiseSFTPShellInitialized();
                        SFTPInitialized = true;
                    }

                    // Check if the shell is going to exit
                    if (Bail)
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "Exiting shell...");
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

                    // See UESHShell.cs for more info
                    lock (CancellationHandlers.GetCancelSyncLock(ShellType))
                    {
                        // Prompt for command
                        if (Kernel.Kernel.DefConsoleOut is not null)
                        {
                            ConsoleBase.ConsoleWrapper.SetOut(Kernel.Kernel.DefConsoleOut);
                        }
                        if (!Connects)
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Preparing prompt...");
                            PromptPresetManager.WriteShellPrompt(ShellType);
                        }
                    }

                    // Try to connect if IP address is specified.
                    if (Connects)
                    {
                        DebugWriter.Wdbg(DebugLevel.I, $"Currently connecting to {Address} by \"sftp (address)\"...");
                        SFTPStrCmd = $"connect {Address}";
                        Connects = false;
                    }
                    else
                    {
                        DebugWriter.Wdbg(DebugLevel.I, "Normal shell");
                        SFTPStrCmd = Input.ReadLine();
                    }

                    // Parse command
                    if ((string.IsNullOrEmpty(SFTPStrCmd) | (SFTPStrCmd?.StartsWithAnyOf(new[] { " ", "#" }))) == false)
                    {
                        Kernel.Kernel.KernelEventManager.RaiseSFTPPreExecuteCommand(SFTPStrCmd);
                        Shell.GetLine(SFTPStrCmd, "", ShellType.SFTPShell);
                        Kernel.Kernel.KernelEventManager.RaiseSFTPPostExecuteCommand(SFTPStrCmd);
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
                    throw new Kernel.Exceptions.SFTPShellException(Translate.DoTranslation("There was an error in the SFTP shell:") + " {0}", ex, ex.Message);
                }
            }
        }

    }
}

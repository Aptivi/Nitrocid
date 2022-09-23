
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
using KS.ConsoleBase.Inputs;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.HTTP
{
    /// <summary>
    /// The HTTP shell
    /// </summary>
    public class HTTPShell : ShellExecutor, IShell
    {

        /// <inheritdoc/>
        public override ShellType ShellType => ShellType.HTTPShell;

        /// <inheritdoc/>
        public override bool Bail { get; set; }

        /// <inheritdoc/>
        public override void InitializeShell(params object[] ShellArgs)
        {
            while (!Bail)
            {
                try
                {
                    // See UESHShell.cs for more info
                    lock (CancellationHandlers.GetCancelSyncLock(ShellType))
                    {
                        // Prompt for command
                        DebugWriter.WriteDebug(DebugLevel.I, "Preparing prompt...");
                        PromptPresetManager.WriteShellPrompt(ShellType);

                        // Raise the event
                        Kernel.Kernel.KernelEventManager.RaiseHTTPShellInitialized();
                    }

                    // Prompt for command
                    DebugWriter.WriteDebug(DebugLevel.I, "Normal shell");
                    string HttpCommand = Input.ReadLine();

                    // Parse command
                    if ((string.IsNullOrEmpty(HttpCommand) | (HttpCommand?.StartsWithAnyOf(new[] { " ", "#" }))) == false)
                    {
                        Kernel.Kernel.KernelEventManager.RaiseHTTPPreExecuteCommand(HttpCommand);
                        Shell.GetLine(HttpCommand, "", ShellType.HTTPShell);
                        Kernel.Kernel.KernelEventManager.RaiseHTTPPostExecuteCommand(HttpCommand);
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
                    throw new Kernel.Exceptions.HTTPShellException(Translate.DoTranslation("There was an error in the HTTP shell:") + " {0}", ex, ex.Message);
                }
            }

            // Exiting, so reset the site
            HTTPShellCommon.HTTPSite = "";
        }

    }
}

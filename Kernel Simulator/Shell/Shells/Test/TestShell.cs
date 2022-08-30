
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
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Shell.Prompts;
using KS.Shell.ShellBase.Commands;
using KS.Shell.ShellBase.Shells;

namespace KS.Shell.Shells.Test
{
    public class TestShell : ShellExecutor, IShell
    {

        public override ShellType ShellType
        {
            get
            {
                return ShellType.TestShell;
            }
        }

        public override bool Bail { get; set; }

        public override void InitializeShell(params object[] ShellArgs)
        {
            // Show the welcome message
            TextWriterColor.Write();
            SeparatorWriterColor.WriteSeparator(Translate.DoTranslation("Welcome to Test Shell!"), true);

            // Actual shell logic
            while (!Bail)
            {
                // See UESHShell.cs for more info
                lock (CancellationHandlers.GetCancelSyncLock(ShellType))
                {
                    // Write the prompt
                    PromptPresetManager.WriteShellPrompt(ShellType);

                    // Raise the event
                    Kernel.Kernel.KernelEventManager.RaiseTestShellInitialized();
                }

                // Parse the command
                string FullCmd = Input.ReadLine();
                try
                {
                    if ((string.IsNullOrEmpty(FullCmd) | (FullCmd?.StartsWithAnyOf(new[] { " ", "#" }))) == false)
                    {
                        Kernel.Kernel.KernelEventManager.RaiseTestPreExecuteCommand(FullCmd);
                        Shell.GetLine(FullCmd, "", ShellType.TestShell);
                        Kernel.Kernel.KernelEventManager.RaiseTestPostExecuteCommand(FullCmd);
                    }
                }
                catch (ThreadInterruptedException)
                {
                    Flags.CancelRequested = false;
                    Bail = true;
                }
                catch (Exception ex)
                {
                    TextWriterColor.Write(Translate.DoTranslation("Error in test shell: {0}"), true, ColorTools.ColTypes.Error, ex.Message);
                    DebugWriter.WriteDebug(DebugLevel.E, "Error: {0}", ex.Message);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
        }

    }
}

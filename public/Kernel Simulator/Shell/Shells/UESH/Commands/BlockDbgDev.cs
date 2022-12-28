
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

using KS.Kernel.Debugging.RemoteDebug;
using KS.Languages;
using KS.Misc.Writers.ConsoleWriters;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can block an IP address from entering the remote debugger.
    /// </summary>
    /// <remarks>
    /// If you wanted to moderate the remote debugger and block a device from joining it because it either causes problems or kept flooding the chat, you may use this command to block such offenders.
    /// <br></br>
    /// This command is available to administrators only. The blocked device can be unblocked using the unblockdbgdev command.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class BlockDbgDevCommand : BaseCommand, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (!RemoteDebugger.RDebugBlocked.Contains(ListArgsOnly[0]))
            {
                if (RemoteDebugTools.TryAddToBlockList(ListArgsOnly[0]))
                {
                    TextWriterColor.Write(Translate.DoTranslation("{0} can't join remote debug now."), ListArgsOnly[0]);
                }
                else
                {
                    TextWriterColor.Write(Translate.DoTranslation("Failed to block {0}."), ListArgsOnly[0]);
                }
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("{0} is already blocked."), ListArgsOnly[0]);
            }
        }

    }
}
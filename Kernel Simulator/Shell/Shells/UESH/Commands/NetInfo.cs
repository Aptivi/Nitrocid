using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Platform;
using KS.Misc.Writers.ConsoleWriters;

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

using KS.Network;
using KS.Shell.ShellBase.Commands;

namespace KS.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// You can view the detailed status of the network connection
    /// </summary>
    /// <remarks>
    /// This command lets you view the details about all your wireless/Ethernet adapters, packets, packets that has an error, etc. The information that are printed is diagnostic so if you can't connect to the Internet, you can use these information to diagnose.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class NetInfoCommand : CommandExecutor, ICommand
    {

        public override void Execute(string StringArgs, string[] ListArgsOnly, string[] ListSwitchesOnly)
        {
            if (KernelPlatform.IsOnWindows())
            {
                NetworkAdapterPrint.PrintAdapterProperties();
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Due to technical difficulties, we're unable to list adapter properties on Unix systems."), true, ColorTools.ColTypes.Error);
            }
        }

    }
}
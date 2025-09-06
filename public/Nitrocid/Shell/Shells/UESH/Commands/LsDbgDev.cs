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

using Nitrocid.ConsoleBase.Colors;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Debugging.RemoteDebug;
using Terminaux.Shell.Commands;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Lists debugging devices connected
    /// </summary>
    /// <remarks>
    /// This command lists all the connected IP addresses that are currently receiving debug messages. This is useful for listing, identifying, and possibly disconnecting the address from the debugging server.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class LsDbgDevCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            foreach (RemoteDebugDevice DebugDevice in RemoteDebugger.DebugDevices)
            {
                TextWriters.Write($"- {DebugDevice.ClientIP}: ", false, KernelColorType.ListEntry);
                TextWriters.Write(DebugDevice.ClientName, true, KernelColorType.ListValue);
            }
            return 0;
        }

    }
}

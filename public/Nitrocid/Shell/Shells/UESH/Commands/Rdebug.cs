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
using Nitrocid.Kernel;
using Nitrocid.Kernel.Debugging.RemoteDebug;
using Nitrocid.Kernel.Exceptions;
using Nitrocid.Languages;
using Nitrocid.Shell.ShellBase.Commands;

namespace Nitrocid.Shell.Shells.UESH.Commands
{
    /// <summary>
    /// Enables remote debug
    /// </summary>
    /// <remarks>
    /// If the kernel is on the debugging mode, you can use this command to turn on/off the functionality. If the remote debug is on, it will turn it off, and it will do inverse.
    /// <br></br>
    /// The remote debug will listen on a port that is unused as specified in the kernel settings, Debug Port.
    /// <br></br>
    /// The user must have at least the administrative privileges before they can run the below commands.
    /// </remarks>
    class RdebugCommand : BaseCommand, ICommand
    {

        public override int Execute(CommandParameters parameters, ref string variableValue)
        {
            if (KernelEntry.DebugMode)
            {
                if (RemoteDebugger.RDebugThread.IsAlive)
                {
                    RemoteDebugger.StopRDebugThread();
                }
                else
                {
                    RemoteDebugger.StartRDebugThread();
                }
                return 0;
            }
            else
            {
                TextWriters.Write(LanguageTools.GetLocalized("NKS_SHELL_SHELLS_UESH_RDEBUG_NOTENABLED"), true, KernelColorType.Error);
                return KernelExceptionTools.GetErrorCode(KernelExceptionType.Debug);
            }
        }

    }
}

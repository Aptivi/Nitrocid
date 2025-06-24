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

using Nitrocid.Base.Languages;
using System;

namespace Nitrocid.Base.Kernel.Debugging.RemoteDebug.Command.BaseCommands
{
    internal class TraceCommand : RemoteDebugBaseCommand
    {
        public override void Execute(RemoteDebugCommandParameters parameters, RemoteDebugDevice device)
        {
            if (DebugWriter.DebugStackTraces.Length != 0)
            {
                if (parameters.ArgumentsList.Length != 0)
                {
                    try
                    {
                        DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, DebugWriter.DebugStackTraces[Convert.ToInt32(parameters.ArgumentsList[0])], true, device);
                    }
                    catch (Exception ex)
                    {
                        DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_REMOTEDEBUG_TRACE_INVALIDINDEX") + " {2}", true, device, vars: [parameters.ArgumentsList[0], DebugWriter.DebugStackTraces.Length, ex.Message]);
                    }
                }
                else
                {
                    DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, DebugWriter.DebugStackTraces[0], true, device);
                }
            }
            else
            {
                DebugWriter.WriteDebugDeviceOnly(DebugLevel.I, LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_REMOTEDEBUG_TRACE_NOSTACKTRACE"), true, device);
            }
        }
    }
}


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

using System;
using KS.Kernel;
using KS.Misc.Writers.ConsoleWriters;
using KS.Drivers;
using KS.Shell.ShellBase.Shells;
using KS.Drivers.Console;

namespace KS.Shell.ShellBase.Commands
{
    static class CancellationHandlers
    {

        public static void CancelCommand(object sender, ConsoleCancelEventArgs e)
        {
            lock (GetCancelSyncLock(Shell.CurrentShellType))
            {
                if (e.SpecialKey == ConsoleSpecialKey.ControlC)
                {
                    Flags.CancelRequested = true;
                    TextWriterColor.Write();
                    DriverHandler.SetDriver<IConsoleDriver>("Null");
                    e.Cancel = true;
                    var StartCommandThread = ShellStart.ShellStack[ShellStart.ShellStack.Count - 1].ShellCommandThread;
                    StartCommandThread.Stop();
                    Shell.ProcessStartCommandThread.Stop();
                    DriverHandler.SetDriver<IConsoleDriver>("Terminal");
                }
            }
        }

        public static object GetCancelSyncLock(ShellType ShellType) => GetCancelSyncLock(Shell.GetShellTypeName(ShellType));

        public static object GetCancelSyncLock(string ShellType) => Shell.GetShellInfo(ShellType).ShellLock;

    }
}

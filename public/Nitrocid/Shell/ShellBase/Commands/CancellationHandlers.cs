
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
// 
// This file is part of Nitrocid KS
// 
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using KS.Drivers;
using KS.Shell.ShellBase.Shells;
using KS.Drivers.Console;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Debugging;
using KS.Kernel.Configuration;

namespace KS.Shell.ShellBase.Commands
{
    internal static class CancellationHandlers
    {

        internal static bool canCancel = false;
        internal static bool installed;

        internal static void CancelCommand(object sender, ConsoleCancelEventArgs e)
        {
            // We can't cancel in a situation where there are no shells.
            if (ShellStart.ShellStack.Count <= 0)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "No shells. Can't cancel.");
                e.Cancel = true;
                return;
            }

            // We can't cancel in situations where cancellation is not possible
            if (!canCancel)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Cancellation impossible. Can't cancel.");
                e.Cancel = true;
                return;
            }

            // Now, handle the command cancellation
            try
            {
                var StartCommandThread = ShellStart.ShellStack[^1].ShellCommandThread;
                var ProcessStartCommandThread = ShellManager.ProcessStartCommandThread;
                var syncLock = GetCancelSyncLock(ShellManager.CurrentShellType);
                lock (syncLock)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Locking to cancel...");
                    KernelFlags.CancelRequested = true;
                    TextWriterColor.Write();
                    DriverHandler.SetDriver<IConsoleDriver>("Null");
                    StartCommandThread.Stop();
                    ProcessStartCommandThread.Stop();
                    DriverHandler.SetDriver<IConsoleDriver>("Default");
                    DebugWriter.WriteDebug(DebugLevel.I, "Cancelled command.");
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Cannot cancel. {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            e.Cancel = true;
        }

        internal static void InstallHandler()
        {
            if (!installed)
            {
                Console.CancelKeyPress += CancelCommand;
                installed = true;
            }
        }

        internal static object GetCancelSyncLock(ShellType ShellType) =>
            GetCancelSyncLock(ShellManager.GetShellTypeName(ShellType));

        internal static object GetCancelSyncLock(string ShellType) =>
            ShellManager.GetShellInfo(ShellType).ShellLock;

    }
}

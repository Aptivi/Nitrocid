
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

using KS.ConsoleBase;
using KS.Kernel.Debugging;
using KS.Languages;
using KS.Network.RPC;
using System.Threading;
using KS.Kernel.Events;
using KS.Users.Permissions;
using System.Diagnostics;
using KS.Kernel.Journaling;
using KS.ConsoleBase.Writers.ConsoleWriters;
using KS.Kernel.Threading;

namespace KS.Kernel.Power
{
    /// <summary>
    /// Power management module
    /// </summary>
    public static class PowerManager
    {

        internal static Stopwatch Uptime = new();
        internal static KernelThread RPCPowerListener = new("RPC Power Listener Thread", true, (object arg) => PowerManage((PowerMode)arg)) { isCritical = true };

        /// <summary>
        /// Manage computer's (actually, simulated computer) power
        /// </summary>
        /// <param name="PowerMode">Selects the power mode</param>
        public static void PowerManage(PowerMode PowerMode) => PowerManage(PowerMode, "0.0.0.0", RemoteProcedure.RPCPort);

        /// <summary>
        /// Manage computer's (actually, simulated computer) power
        /// </summary>
        /// <param name="PowerMode">Selects the power mode</param>
        /// <param name="IP">IP address to remotely manage power</param>
        public static void PowerManage(PowerMode PowerMode, string IP) => PowerManage(PowerMode, IP, RemoteProcedure.RPCPort);

        /// <summary>
        /// Manage computer's (actually, simulated computer) power
        /// </summary>
        /// <param name="PowerMode">Selects the power mode</param>
        /// <param name="IP">IP address to remotely manage power</param>
        /// <param name="Port">Port of the remote system running KS RPC</param>
        public static void PowerManage(PowerMode PowerMode, string IP, int Port)
        {
            // Check to see if the current user is granted power management or not
            PermissionsTools.Demand(PermissionTypes.ManagePower);

            DebugWriter.WriteDebug(DebugLevel.I, "Power management has the argument of {0}", PowerMode);
            switch (PowerMode)
            {
                case PowerMode.Shutdown:
                    {
                        EventsManager.FireEvent(EventType.PreShutdown);
                        DebugWriter.WriteDebug(DebugLevel.W, "Kernel is shutting down!");
                        TextWriterColor.Write(Translate.DoTranslation("Shutting down..."));

                        // Simulate 0.0.1's behavior on shutting down
                        if (Flags.BeepOnShutdown)
                            ConsoleWrapper.Beep();
                        if (Flags.DelayOnShutdown)
                            Thread.Sleep(3000);

                        // Set appropriate flags
                        Flags.RebootRequested = true;
                        Flags.LogoutRequested = true;
                        Flags.KernelShutdown = true;
                        break;
                    }
                case PowerMode.Reboot:
                case PowerMode.RebootSafe:
                    {
                        EventsManager.FireEvent(EventType.PreReboot);
                        DebugWriter.WriteDebug(DebugLevel.W, "Kernel is restarting!");
                        TextWriterColor.Write(Translate.DoTranslation("Rebooting..."));

                        // Simulate 0.0.1's behavior on shutting down
                        if (Flags.BeepOnShutdown)
                            ConsoleWrapper.Beep();
                        if (Flags.DelayOnShutdown)
                            Thread.Sleep(3000);

                        // Set appropriate flags
                        Flags.RebootRequested = true;
                        Flags.LogoutRequested = true;
                        Flags.SafeMode = PowerMode == PowerMode.RebootSafe;
                        DebugWriter.WriteDebug(DebugLevel.I, "Safe mode changed to {0}", Flags.SafeMode);
                        break;
                    }
                case PowerMode.RemoteShutdown:
                    {
                        JournalManager.WriteJournal(Translate.DoTranslation("Remote power management invoked:") + $" {IP}:{Port} => {PowerMode}");
                        RPCCommands.SendCommand("<Request:Shutdown>(" + IP + ")", IP, Port);
                        break;
                    }
                case PowerMode.RemoteRestart:
                    {
                        JournalManager.WriteJournal(Translate.DoTranslation("Remote power management invoked:") + $" {IP}:{Port} => {PowerMode}");
                        RPCCommands.SendCommand("<Request:Reboot>(" + IP + ")", IP, Port);
                        break;
                    }
                case PowerMode.RemoteRestartSafe:
                    {
                        JournalManager.WriteJournal(Translate.DoTranslation("Remote power management invoked:") + $" {IP}:{Port} => {PowerMode}");
                        RPCCommands.SendCommand("<Request:RebootSafe>(" + IP + ")", IP, Port);
                        break;
                    }
            }
        }

        /// <summary>
        /// The kernel uptime (how long since the kernel booted up)
        /// </summary>
        public static string KernelUptime =>
            Uptime.Elapsed.ToString();

    }
}

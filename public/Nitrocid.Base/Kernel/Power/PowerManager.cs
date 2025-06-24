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

using System.Threading;
using System.Diagnostics;
using Terminaux.Reader;
using System.Reflection;
using System.IO;
using Terminaux.Shell.Shells;
using Terminaux.Writer.ConsoleWriters;
using Terminaux.Base;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Threading;
using Nitrocid.Base.Users.Login;
using Nitrocid.Base.Kernel.Starting.Environment;
using Nitrocid.Base.Security.Permissions;
using Nitrocid.Base.Kernel.Events;
using Nitrocid.Base.Kernel.Journaling;
using Nitrocid.Base.Network.Types.RPC;

namespace Nitrocid.Base.Kernel.Power
{
    /// <summary>
    /// Power management module
    /// </summary>
    public static class PowerManager
    {

        internal static bool KernelShutdown;
        internal static bool RebootRequested;
        internal static bool hardShutdown;
        internal static bool elevating;
        internal static bool rebootingToSafeMode;
        internal static bool rebootingToDebugMode;
        internal static bool rebootingToMaintenanceMode;
        internal static Stopwatch Uptime = new();
        internal static KernelThread RPCPowerListener = new("RPC Power Listener Thread", true, (arg) => PowerManage((PowerMode?)arg ?? PowerMode.Shutdown));

        internal static bool RebootingToSafeMode
        {
            get
            {
                bool status = rebootingToSafeMode;
                if (rebootingToSafeMode)
                    rebootingToSafeMode = false;
                return status;
            }
        }

        internal static bool RebootingToDebugMode
        {
            get
            {
                bool status = rebootingToDebugMode;
                if (rebootingToDebugMode)
                    rebootingToDebugMode = false;
                return status;
            }
        }

        internal static bool RebootingToMaintenanceMode
        {
            get
            {
                bool status = rebootingToMaintenanceMode;
                if (rebootingToMaintenanceMode)
                    rebootingToMaintenanceMode = false;
                return status;
            }
        }

        /// <summary>
        /// Manage computer's (actually, simulated computer) power
        /// </summary>
        /// <param name="PowerMode">Selects the power mode</param>
        public static void PowerManage(PowerMode PowerMode) =>
            PowerManage(PowerMode, "0.0.0.0", Config.MainConfig.RPCPort);

        /// <summary>
        /// Manage computer's (actually, simulated computer) power
        /// </summary>
        /// <param name="PowerMode">Selects the power mode</param>
        /// <param name="IP">IP address to remotely manage power</param>
        public static void PowerManage(PowerMode PowerMode, string IP) =>
            PowerManage(PowerMode, IP, Config.MainConfig.RPCPort);

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

            DebugWriter.WriteDebug(DebugLevel.I, "Power management has the argument of {0}", vars: [PowerMode]);
            switch (PowerMode)
            {
                case PowerMode.Shutdown:
                    {
                        EventsManager.FireEvent(EventType.PreShutdown);
                        DebugWriter.WriteDebug(DebugLevel.W, "Kernel is shutting down!");

                        // Simulate 0.0.1's behavior on shutting down
                        if (!Config.MainConfig.EnableSplash)
                        {
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_POWER_SHUTTINGDOWN"));
                            if (Config.MainConfig.BeepOnShutdown)
                                ConsoleWrapper.Beep();
                            if (Config.MainConfig.DelayOnShutdown)
                                Thread.Sleep(3000);
                        }

                        // Set appropriate flags
                        RebootRequested = true;
                        Login.LogoutRequested = true;
                        KernelShutdown = true;

                        // Kill all shells and interrupt any input
                        ShellManager.KillAllShells();
                        TermReaderTools.Interrupt();
                        break;
                    }
                case PowerMode.Reboot:
                case PowerMode.RebootSafe:
                case PowerMode.RebootMaintenance:
                case PowerMode.RebootDebug:
                    {
                        EventsManager.FireEvent(EventType.PreReboot);
                        DebugWriter.WriteDebug(DebugLevel.W, "Kernel is restarting!");
                        if (!Config.MainConfig.EnableSplash)
                            TextWriterColor.Write(LanguageTools.GetLocalized("NKS_KERNEL_POWER_REBOOTING"));

                        // Set appropriate flags
                        RebootRequested = true;
                        Login.LogoutRequested = true;
                        rebootingToSafeMode = PowerMode == PowerMode.RebootSafe;
                        DebugWriter.WriteDebug(DebugLevel.I, "Safe mode changed to {0}", vars: [rebootingToSafeMode]);
                        rebootingToMaintenanceMode = PowerMode == PowerMode.RebootMaintenance;
                        DebugWriter.WriteDebug(DebugLevel.I, "Maintenance mode changed to {0}", vars: [rebootingToMaintenanceMode]);
                        rebootingToDebugMode = PowerMode == PowerMode.RebootDebug;
                        DebugWriter.WriteDebug(DebugLevel.I, "Debug mode changed to {0}", vars: [rebootingToDebugMode]);

                        // Kill all shells and interrupt any input
                        ShellManager.KillAllShells();
                        TermReaderTools.Interrupt();
                        break;
                    }
                case PowerMode.RemoteShutdown:
                    {
                        JournalManager.WriteJournal(LanguageTools.GetLocalized("NKS_KERNEL_POWER_INVOKING") + $" {IP}:{Port} => {PowerMode}");
                        RPCCommands.SendCommand("<Request:Shutdown>(" + IP + ")", IP, Port);
                        break;
                    }
                case PowerMode.RemoteRestart:
                    {
                        JournalManager.WriteJournal(LanguageTools.GetLocalized("NKS_KERNEL_POWER_INVOKING") + $" {IP}:{Port} => {PowerMode}");
                        RPCCommands.SendCommand("<Request:Reboot>(" + IP + ")", IP, Port);
                        break;
                    }
                case PowerMode.RemoteRestartSafe:
                    {
                        JournalManager.WriteJournal(LanguageTools.GetLocalized("NKS_KERNEL_POWER_INVOKING") + $" {IP}:{Port} => {PowerMode}");
                        RPCCommands.SendCommand("<Request:RebootSafe>(" + IP + ")", IP, Port);
                        break;
                    }
                case PowerMode.RemoteRestartDebug:
                    {
                        JournalManager.WriteJournal(LanguageTools.GetLocalized("NKS_KERNEL_POWER_INVOKING") + $" {IP}:{Port} => {PowerMode}");
                        RPCCommands.SendCommand("<Request:RebootDebug>(" + IP + ")", IP, Port);
                        break;
                    }
                case PowerMode.RemoteRestartMaintenance:
                    {
                        JournalManager.WriteJournal(LanguageTools.GetLocalized("NKS_KERNEL_POWER_INVOKING") + $" {IP}:{Port} => {PowerMode}");
                        RPCCommands.SendCommand("<Request:RebootMaintenance>(" + IP + ")", IP, Port);
                        break;
                    }
            }
        }

        /// <summary>
        /// The kernel uptime (how long since the kernel booted up)
        /// </summary>
        public static string KernelUptime =>
            Uptime.Elapsed.ToString();

        internal static void ElevateSelf()
        {
            DebugCheck.Assert(KernelPlatform.IsOnWindows(), "tried to call this on non-Windows platforms");
            var selfProcess = new Process
            {
                StartInfo = new(Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".exe"))
                {
                    UseShellExecute = true,
                    Verb = "runas",
                    Arguments = string.Join(" ", EnvironmentTools.kernelArguments)
                },
            };

            // Now, go ahead and start.
            selfProcess.Start();
            elevating = false;
        }

    }
}

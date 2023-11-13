//
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
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

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
using Terminaux.Reader;
using KS.Shell.ShellBase.Shells;
using KS.Kernel.Configuration;
using KS.Users.Login;
using KS.Misc.Splash;
using System.Reflection;
using System.IO;
using KS.Kernel.Starting.Environment;
using KS.Shell.ShellBase.Commands.ProcessExecution;

namespace KS.Kernel.Power
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
        internal static KernelThread RPCPowerListener = new("RPC Power Listener Thread", true, (object arg) => PowerManage((PowerMode)arg));

        /// <summary>
        /// Beeps on shutdown (to restore the way of 0.0.1's shutdown)
        /// </summary>
        public static bool BeepOnShutdown =>
            Config.MainConfig.BeepOnShutdown;

        /// <summary>
        /// Delay on shutdown (to restore the way of 0.0.1's shutdown)
        /// </summary>
        public static bool DelayOnShutdown =>
            Config.MainConfig.DelayOnShutdown;

        /// <summary>
        /// Whether to simulate a situation where there is no APM available. If enabled, it shows the "It's now safe to
        /// turn off your computer" text.
        /// </summary>
        public static bool SimulateNoAPM =>
            Config.MainConfig.SimulateNoAPM;

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
            PowerManage(PowerMode, "0.0.0.0", RemoteProcedure.RPCPort);

        /// <summary>
        /// Manage computer's (actually, simulated computer) power
        /// </summary>
        /// <param name="PowerMode">Selects the power mode</param>
        /// <param name="IP">IP address to remotely manage power</param>
        public static void PowerManage(PowerMode PowerMode, string IP) =>
            PowerManage(PowerMode, IP, RemoteProcedure.RPCPort);

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

                        // Simulate 0.0.1's behavior on shutting down
                        if (!SplashManager.EnableSplash)
                        {
                            TextWriterColor.Write(Translate.DoTranslation("Shutting down..."));
                            if (BeepOnShutdown)
                                ConsoleWrapper.Beep();
                            if (DelayOnShutdown)
                                Thread.Sleep(3000);
                        }

                        // Set appropriate flags
                        RebootRequested = true;
                        Login.LogoutRequested = true;
                        KernelShutdown = true;

                        // Kill all shells and interrupt any input
                        for (int i = ShellManager.ShellStack.Count - 1; i >= 0; i--)
                            ShellManager.KillShellForced();
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
                        if (!SplashManager.EnableSplash)
                            TextWriterColor.Write(Translate.DoTranslation("Rebooting..."));

                        // Set appropriate flags
                        RebootRequested = true;
                        Login.LogoutRequested = true;
                        rebootingToSafeMode = PowerMode == PowerMode.RebootSafe;
                        DebugWriter.WriteDebug(DebugLevel.I, "Safe mode changed to {0}", rebootingToSafeMode);
                        rebootingToMaintenanceMode = PowerMode == PowerMode.RebootMaintenance;
                        DebugWriter.WriteDebug(DebugLevel.I, "Maintenance mode changed to {0}", rebootingToMaintenanceMode);
                        rebootingToDebugMode = PowerMode == PowerMode.RebootDebug;
                        DebugWriter.WriteDebug(DebugLevel.I, "Debug mode changed to {0}", rebootingToDebugMode);

                        // Kill all shells and interrupt any input
                        for (int i = ShellManager.ShellStack.Count - 1; i >= 0; i--)
                            ShellManager.KillShellForced();
                        TermReaderTools.Interrupt();
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
                case PowerMode.RemoteRestartDebug:
                    {
                        JournalManager.WriteJournal(Translate.DoTranslation("Remote power management invoked:") + $" {IP}:{Port} => {PowerMode}");
                        RPCCommands.SendCommand("<Request:RebootDebug>(" + IP + ")", IP, Port);
                        break;
                    }
                case PowerMode.RemoteRestartMaintenance:
                    {
                        JournalManager.WriteJournal(Translate.DoTranslation("Remote power management invoked:") + $" {IP}:{Port} => {PowerMode}");
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
                    Arguments = string.Join(" ", EnvironmentTools.arguments)
                },
            };
            selfProcess.StartInfo = ProcessExecutor.StripEnvironmentVariables(selfProcess.StartInfo);

            // Now, go ahead and start.
            selfProcess.Start();
            elevating = false;
        }

    }
}

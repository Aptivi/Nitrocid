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

using System.Net.Sockets;
using System.Threading;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Threading;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Splash;

namespace Nitrocid.Base.Network.Types.RPC
{
    /// <summary>
    /// Remote procedure module
    /// </summary>
    public static class RemoteProcedure
    {

        internal static int rpcPort = 12345;
        internal static UdpClient? RPCListen;
        internal static UdpClient rpcStandaloneClient = new();
        internal static KernelThread RPCThread = new("RPC Thread", true, RPCCommands.ReceiveCommand) { isCritical = true };
        internal static bool rpcStopping = false;

        /// <summary>
        /// Whether the RPC started
        /// </summary>
        public static bool RPCStarted =>
            RPCThread.IsAlive;

        /// <summary>
        /// Starts the RPC listener
        /// </summary>
        public static void StartRPC()
        {
            if (Config.MainConfig.RPCEnabled)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "RPC: Starting...");
                if (!RPCStarted)
                {
                    RPCListen = new UdpClient(Config.MainConfig.RPCPort) { EnableBroadcast = true };
                    DebugWriter.WriteDebug(DebugLevel.I, "RPC: Listener started");
                    RPCThread.Start();
                    DebugWriter.WriteDebug(DebugLevel.I, "RPC: Thread started");
                }
                else
                {
                    throw new KernelException(KernelExceptionType.RemoteProcedure, LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RPC_EXCEPTION_ALREADYSTARTED"));
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.RemoteProcedure, LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RPC_EXCEPTION_DISABLED"));
            }
        }

        /// <summary>
        /// The wrapper for <see cref="StartRPC"/>
        /// </summary>
        public static void WrapperStartRPC()
        {
            if (Config.MainConfig.RPCEnabled)
            {
                try
                {
                    StartRPC();
                    SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RPC_RUNNING"), 5, Config.MainConfig.RPCPort);
                }
                catch (ThreadStateException ex)
                {
                    SplashReport.ReportProgressError(LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RPC_ALREADYRUNNING"));
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            else
            {
                SplashReport.ReportProgress(LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RPC_EXCEPTION_DISABLED"), 3);
            }
        }

        /// <summary>
        /// Stops the RPC listener
        /// </summary>
        public static void StopRPC()
        {
            if (RPCStarted)
            {
                RPCCommands.rpcStopTrigger.Set();
                RPCThread.Stop();
                DebugWriter.WriteDebug(DebugLevel.I, "RPC stopped.");
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.E, "RPC hasn't started yet!");
            }
        }

    }
}

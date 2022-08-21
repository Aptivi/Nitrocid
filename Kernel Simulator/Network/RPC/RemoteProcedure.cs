
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

using System.Net.Sockets;
using System.Threading;
using Extensification.StringExts;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Splash;
using KS.Misc.Threading;
using KS.Misc.Writers.DebugWriters;

namespace KS.Network.RPC
{
    public static class RemoteProcedure
    {

        public static UdpClient RPCListen;
        public static int RPCPort = 12345;
        public static bool RPCEnabled = true;
        internal static KernelThread RPCThread = new KernelThread("RPC Thread", true, RPCCommands.ReceiveCommand);

        /// <summary>
        /// Whether the RPC started
        /// </summary>
        public static bool RPCStarted
        {
            get
            {
                return RPCThread.IsAlive;
            }
        }

        /// <summary>
        /// Starts the RPC listener
        /// </summary>
        public static void StartRPC()
        {
            if (RPCEnabled)
            {
                DebugWriter.Wdbg(DebugLevel.I, "RPC: Starting...");
                if (!RPCStarted)
                {
                    RPCListen = new UdpClient(RPCPort) { EnableBroadcast = true };
                    DebugWriter.Wdbg(DebugLevel.I, "RPC: Listener started");
                    RPCThread.Start();
                    DebugWriter.Wdbg(DebugLevel.I, "RPC: Thread started");
                }
                else
                {
                    throw new ThreadStateException(Translate.DoTranslation("Trying to start RPC while it's already started."));
                }
            }
            else
            {
                throw new ThreadStateException(Translate.DoTranslation("Not starting RPC because it's disabled."));
            }
        }

        /// <summary>
        /// The wrapper for <see cref="StartRPC"/>
        /// </summary>
        public static void WrapperStartRPC()
        {
            if (RPCEnabled)
            {
                try
                {
                    StartRPC();
                    SplashReport.ReportProgress(Translate.DoTranslation("RPC listening on all addresses using port {0}.").FormatString(RPCPort), 5, ColorTools.ColTypes.Neutral);
                }
                catch (ThreadStateException ex)
                {
                    SplashReport.ReportProgress(Translate.DoTranslation("RPC is already running."), 5, ColorTools.ColTypes.Error);
                    DebugWriter.WStkTrc(ex);
                }
            }
            else
            {
                SplashReport.ReportProgress(Translate.DoTranslation("Not starting RPC because it's disabled."), 3, ColorTools.ColTypes.Neutral);
            }
        }

        /// <summary>
        /// Stops the RPC listener
        /// </summary>
        public static void StopRPC()
        {
            if (RPCStarted)
            {
                RPCThread.Stop();
                RPCListen?.Close();
                RPCListen = null;
                DebugWriter.Wdbg(DebugLevel.I, "RPC stopped.");
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.E, "RPC hasn't started yet!");
            }
        }

    }
}
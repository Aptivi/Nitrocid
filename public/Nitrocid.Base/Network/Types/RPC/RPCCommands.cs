//
// Nitrocid  Copyright (C) 2018-2026  Aptivi
//
// This file is part of Nitrocid
//
// Nitrocid is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Events;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Network.Types.RPC.Commands;
using Textify.General;

namespace Nitrocid.Base.Network.Types.RPC
{
    /// <summary>
    /// RPC commands module
    /// </summary>
    public static class RPCCommands
    {
        internal static ManualResetEvent rpcStopTrigger = new(false);

        private readonly static Dictionary<RPCCommandEnum, IRPCCommand> RPCCommandReplyActions = new()
        {
            { RPCCommandEnum.Shutdown,            new ShutdownCommand() },
            { RPCCommandEnum.Reboot,              new RebootCommand() },
            { RPCCommandEnum.RebootSafe,          new RebootSafeCommand() },
            { RPCCommandEnum.RebootMaintenance,   new RebootMaintenanceCommand() },
            { RPCCommandEnum.RebootDebug,         new RebootDebugCommand() },
            { RPCCommandEnum.SaveScr,             new SaveScreenCommand() },
            { RPCCommandEnum.Exec,                new ExecCommand() },
            { RPCCommandEnum.Acknowledge,         new AcknowledgeCommand() },
            { RPCCommandEnum.Ping,                new PingCommand() },
            { RPCCommandEnum.Version,             new VersionCommand() },
            { RPCCommandEnum.VersionCode,         new VersionCodeCommand() },
            { RPCCommandEnum.ApiVersion,          new ApiVersionCommand() },
            { RPCCommandEnum.ApiVersionCode,      new ApiVersionCodeCommand() },
        };

        /// <summary>
        /// Send an RPC command to another instance of KS using the specified address
        /// </summary>
        /// <param name="Request">A request</param>
        /// <param name="IP">An IP address which the RPC is hosted</param>
        /// <param name="clientMode">Client mode (if true, doesn't require RPC server to be running)</param>
        /// <param name="additionalArgs">Additional arguments. If not provided, the IP address will be provided automatically.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SendCommand(RPCCommandEnum Request, string IP, bool clientMode = false, string additionalArgs = "") =>
            SendCommand(Request, IP, Config.MainConfig.RPCPort, clientMode, additionalArgs);

        /// <summary>
        /// Send an RPC command to another instance of KS using the specified address
        /// </summary>
        /// <param name="Request">A request</param>
        /// <param name="IP">An IP address which the RPC is hosted</param>
        /// <param name="Port">A port which the RPC is hosted</param>
        /// <param name="clientMode">Client mode (if true, doesn't require RPC server to be running)</param>
        /// <param name="additionalArgs">Additional arguments. If not provided, the IP address will be provided automatically.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SendCommand(RPCCommandEnum Request, string IP, int Port, bool clientMode = false, string additionalArgs = "") =>
            SendCommand($"<Request:{Request}>({(!string.IsNullOrEmpty(additionalArgs) ? additionalArgs : IP)})", IP, Port, clientMode);

        /// <summary>
        /// Send an RPC command to another instance of KS using the specified address
        /// </summary>
        /// <param name="Request">A request</param>
        /// <param name="IP">An IP address which the RPC is hosted</param>
        /// <param name="clientMode">Client mode (if true, doesn't require RPC server to be running)</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SendCommand(string Request, string IP, bool clientMode = false) =>
            SendCommand(Request, IP, Config.MainConfig.RPCPort, clientMode);

        /// <summary>
        /// Send an RPC command to another instance of KS using the specified address
        /// </summary>
        /// <param name="Request">A request</param>
        /// <param name="IP">An IP address which the RPC is hosted</param>
        /// <param name="Port">A port which the RPC is hosted</param>
        /// <param name="clientMode">Client mode (if true, doesn't require RPC server to be running)</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SendCommand(string Request, string IP, int Port, bool clientMode = false)
        {
            if (Config.MainConfig.RPCEnabled || clientMode)
            {
                // Get the command and the argument
                int argIdx = Request.IndexOf('(');
                string Cmd = Request[..argIdx];
                string RequestType = Cmd[(Cmd.IndexOf(":") + 1)..Cmd.IndexOf(">")];
                var commandEnum = Enum.Parse<RPCCommandEnum>(RequestType);
                DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}", vars: [Cmd]);
                string Arg = Request[(argIdx + 1)..];
                DebugWriter.WriteDebug(DebugLevel.I, "Prototype Arg: {0}", vars: [Arg]);
                Arg = Arg.Remove(Arg.Length - 1);
                DebugWriter.WriteDebug(DebugLevel.I, "Finished Arg: {0}", vars: [Arg]);

                // Check the command
                if (RPCCommandReplyActions.ContainsKey(commandEnum))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Command found.");

                    // Check the request type
                    var ByteMsg = Array.Empty<byte>();

                    // Populate the byte message to send the confirmation to
                    DebugWriter.WriteDebug(DebugLevel.I, "Stream opened for device {0}", vars: [Arg]);
                    ByteMsg = Encoding.Default.GetBytes($"{RequestType}, " + Arg + CharManager.NewLine);

                    // Send the response
                    DebugWriter.WriteDebug(DebugLevel.I, "Sending response to device...");
                    if (clientMode)
                        RemoteProcedure.rpcStandaloneClient.Send(ByteMsg, ByteMsg.Length, IP, Port);
                    else
                        RemoteProcedure.RPCListen?.Send(ByteMsg, ByteMsg.Length, IP, Port);
                    EventsManager.FireEvent(EventType.RPCCommandSent, Cmd, Arg, IP, Port);
                }
                else
                    // Rare case reached. Drop it.
                    DebugWriter.WriteDebug(DebugLevel.E, "Malformed request. {0}", vars: [Cmd]);
            }
            else
                throw new KernelException(KernelExceptionType.RemoteProcedure, LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RPC_EXCEPTION_SENDWITHOUTRPC"));
        }

        /// <summary>
        /// Thread to listen to commands.
        /// </summary>
        public static void ReceiveCommand()
        {
            StartReceivingCommand();
            rpcStopTrigger.WaitOne();
            RemoteProcedure.RPCListen?.Close();
            rpcStopTrigger.Reset();
        }

        internal static void ReplyTo(string message, IPEndPoint endpoint)
        {
            byte[] messageData = Encoding.UTF8.GetBytes(message);
            RemoteProcedure.RPCListen?.Send(messageData, messageData.Length, endpoint);
        }

        private static void StartReceivingCommand()
        {
            try
            {
                if (RemoteProcedure.RPCListen is not null && RemoteProcedure.RPCListen.Client is not null)
                    RemoteProcedure.RPCListen?.BeginReceive(new AsyncCallback(AcknowledgeMessage), null);
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Fatal error on receiver: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
        }

        private static void AcknowledgeMessage(IAsyncResult asyncResult)
        {
            try
            {
                if (RemoteProcedure.RPCListen is null || RemoteProcedure.RPCListen.Client is null)
                    return;
                var endpoint = new IPEndPoint(IPAddress.Any, Config.MainConfig.RPCPort);
                byte[] MessageBuffer = RemoteProcedure.RPCListen.EndReceive(asyncResult, ref endpoint);
                string Message = Encoding.Default.GetString(MessageBuffer).TrimNewLines();

                // Get the command and the argument. Remove the "Confirm" suffix for backwards compatibility
                int separatorIdx = Message.IndexOf(',');
                string Cmd = Message[..separatorIdx].RemoveSuffix("Confirm");
                var commandEnum = Enum.Parse<RPCCommandEnum>(Cmd);
                DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}", vars: [Cmd]);
                string Arg = Message[(separatorIdx + 2)..];
                DebugWriter.WriteDebug(DebugLevel.I, "Final Arg: {0}", vars: [Arg]);

                // If the message is not empty, parse it
                if (!string.IsNullOrEmpty(Message) && endpoint is not null)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "RPC: Received message {0}", vars: [Message]);
                    EventsManager.FireEvent(EventType.RPCCommandReceived, Message, endpoint.Address.ToString(), endpoint.Port);

                    // Invoke the action based on message
                    if (RPCCommandReplyActions.TryGetValue(commandEnum, out IRPCCommand? replyAction))
                        replyAction.Execute(Arg, endpoint);
                    else
                        DebugWriter.WriteDebug(DebugLevel.W, "Not found. Message was {0}", vars: [Message]);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to acknowledge message: {0}", vars: [ex.Message]);
                var SE = (SocketException?)ex.InnerException;
                if (SE is not null)
                {
                    if (SE.SocketErrorCode != SocketError.TimedOut)
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Error from host: {0}", vars: [SE.SocketErrorCode.ToString()]);
                        DebugWriter.WriteDebugStackTrace(ex);
                    }
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Fatal error: {0}", vars: [ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
            finally
            {
                StartReceivingCommand();
            }
        }
    }
}

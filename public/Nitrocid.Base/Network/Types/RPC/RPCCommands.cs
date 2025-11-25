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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Nitrocid.Base.Kernel;
using Nitrocid.Base.Kernel.Configuration;
using Nitrocid.Base.Kernel.Debugging;
using Nitrocid.Base.Kernel.Events;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Kernel.Power;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Misc.Notifications;
using Nitrocid.Base.Misc.Screensaver;
using Nitrocid.Base.Users.Login;
using Terminaux.Shell.Shells;
using Terminaux.Writer.ConsoleWriters;
using Textify.General;

namespace Nitrocid.Base.Network.Types.RPC
{
    /// <summary>
    /// RPC commands module
    /// </summary>
    public static class RPCCommands
    {
        internal static ManualResetEvent rpcStopTrigger = new(false);

        /// <summary>
        /// List of RPC commands.<br/>
        /// <br/>&lt;Request:Shutdown&gt;: Shuts down the remote kernel. Usage: &lt;Request:Shutdown&gt;(IP)
        /// <br/>&lt;Request:Reboot&gt;: Reboots the remote kernel. Usage: &lt;Request:Reboot&gt;(IP)
        /// <br/>&lt;Request:RebootSafe&gt;: Reboots the remote kernel to safe mode. Usage: &lt;Request:RebootSafe&gt;(IP)
        /// <br/>&lt;Request:RebootMaintenance&gt;: Reboots the remote kernel to maintenance mode. Usage: &lt;Request:RebootMaintenance&gt;(IP)
        /// <br/>&lt;Request:RebootDebug&gt;: Reboots the remote kernel to debug. Usage: &lt;Request:RebootDebug&gt;(IP)
        /// <br/>&lt;Request:SaveScr&gt;: Saves the screen remotely. Usage: &lt;Request:SaveScr&gt;(IP)
        /// <br/>&lt;Request:Exec&gt;: Executes a command remotely. Usage: &lt;Request:Exec&gt;(Command)
        /// <br/>&lt;Request:Acknowledge&gt;: Pings the remote kernel silently. Usage: &lt;Request:Acknowledge&gt;(IP)
        /// <br/>&lt;Request:Ping&gt;: Pings the remote kernel with notification. Usage: &lt;Request:Ping&gt;(IP)
        /// <br/>&lt;Request:Version&gt;: Returns the Nitrocid version. Usage: &lt;Request:Version&gt;(IP)
        /// <br/>&lt;Request:VersionCode&gt;: Returns the Nitrocid version code. Usage: &lt;Request:VersionCode&gt;(IP)
        /// <br/>&lt;Request:ApiVersion&gt;: Returns the Nitrocid mod API version. Usage: &lt;Request:ApiVersion&gt;(IP)
        /// <br/>&lt;Request:ApiVersionCode&gt;: Returns the Nitrocid mod API version code. Usage: &lt;Request:ApiVersionCode&gt;(IP)
        /// </summary>
        private readonly static List<string> RPCCommandsField =
        [
            "Shutdown",
            "Reboot",
            "RebootSafe",
            "RebootMaintenance",
            "RebootDebug",
            "SaveScr",
            "Exec",
            "Acknowledge",
            "Ping",
            "Version",
            "VersionCode",
            "ApiVersion",
            "ApiVersionCode",
        ];

        private readonly static Dictionary<string, Action<string, IPEndPoint>> RPCCommandReplyActions = new()
        {
            { "ShutdownConfirm",            (_, _)          => HandleShutdown() },
            { "RebootConfirm",              (_, _)          => HandleReboot() },
            { "RebootSafeConfirm",          (_, _)          => HandleRebootSafe() },
            { "RebootMaintenanceConfirm",   (_, _)          => HandleRebootMaintenance() },
            { "RebootDebugConfirm",         (_, _)          => HandleRebootDebug() },
            { "SaveScrConfirm",             (_, _)          => HandleSaveScr() },
            { "ExecConfirm",                (value, _)      => HandleExec(value) },
            { "AcknowledgeConfirm",         (_, endpoint)   => HandleAcknowledge(endpoint) },
            { "PingConfirm",                (value, _)      => HandlePing(value) },
            { "VersionConfirm",             (_, endpoint)   => HandleVersion(endpoint) },
            { "VersionCodeConfirm",         (_, endpoint)   => HandleVersionCode(endpoint) },
            { "ApiVersionConfirm",          (_, endpoint)   => HandleApiVersion(endpoint) },
            { "ApiVersionCodeConfirm",      (_, endpoint)   => HandleApiVersionCode(endpoint) },
        };

        /// <summary>
        /// Send an RPC command to another instance of KS using the specified address
        /// </summary>
        /// <param name="Request">A request</param>
        /// <param name="IP">An IP address which the RPC is hosted</param>
        /// <param name="clientMode">Client mode (if true, doesn't require RPC server to be running)</param>
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
                string Cmd = Request.Remove(Request.IndexOf("("));
                DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}", vars: [Cmd]);
                string Arg = Request[(Request.IndexOf("(") + 1)..];
                DebugWriter.WriteDebug(DebugLevel.I, "Prototype Arg: {0}", vars: [Arg]);
                Arg = Arg.Remove(Arg.Length - 1);
                DebugWriter.WriteDebug(DebugLevel.I, "Finished Arg: {0}", vars: [Arg]);

                // Check the command
                if (RPCCommandsField.Any(Cmd.Contains))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Command found.");

                    // Check the request type
                    string RequestType = Cmd[(Cmd.IndexOf(":") + 1)..Cmd.IndexOf(">")];
                    var ByteMsg = Array.Empty<byte>();

                    // Populate the byte message to send the confirmation to
                    DebugWriter.WriteDebug(DebugLevel.I, "Stream opened for device {0}", vars: [Arg]);
                    ByteMsg = Encoding.Default.GetBytes($"{RequestType}Confirm, " + Arg + CharManager.NewLine);

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

        private static void StartReceivingCommand()
        {
            try
            {
                if (RemoteProcedure.RPCListen is not null)
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
                if (RemoteProcedure.rpcStopping)
                    return;
                var endpoint = new IPEndPoint(IPAddress.Any, Config.MainConfig.RPCPort);
                byte[] MessageBuffer = RemoteProcedure.RPCListen.EndReceive(asyncResult, ref endpoint);
                string Message = Encoding.Default.GetString(MessageBuffer);

                // Get the command and the argument
                string Cmd = Message.Remove(Message.IndexOf(","));
                DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}", vars: [Cmd]);
                string Arg = Message[(Message.IndexOf(",") + 2)..].Replace(Environment.NewLine, "");
                DebugWriter.WriteDebug(DebugLevel.I, "Final Arg: {0}", vars: [Arg]);

                // If the message is not empty, parse it
                if (!string.IsNullOrEmpty(Message) && endpoint is not null)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "RPC: Received message {0}", vars: [Message]);
                    EventsManager.FireEvent(EventType.RPCCommandReceived, Message, endpoint.Address.ToString(), endpoint.Port);

                    // Invoke the action based on message
                    if (RPCCommandReplyActions.TryGetValue(Cmd, out Action<string, IPEndPoint>? replyAction))
                        replyAction.Invoke(Arg, endpoint);
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

        private static void ReplyTo(string message, IPEndPoint endpoint)
        {
            byte[] versionData = Encoding.UTF8.GetBytes(message);
            RemoteProcedure.RPCListen?.Send(versionData, versionData.Length, endpoint);
        }

        private static void HandleShutdown()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Shutdown confirmed from remote access.");
            PowerManager.RPCPowerListener.Start(PowerMode.Shutdown);
        }

        private static void HandleReboot()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Reboot confirmed from remote access.");
            PowerManager.RPCPowerListener.Start(PowerMode.Reboot);
        }

        private static void HandleRebootSafe()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Reboot to safe mode confirmed from remote access.");
            PowerManager.RPCPowerListener.Start(PowerMode.RebootSafe);
        }

        private static void HandleRebootMaintenance()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Reboot to maintenance mode confirmed from remote access.");
            PowerManager.RPCPowerListener.Start(PowerMode.RebootMaintenance);
        }

        private static void HandleRebootDebug()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Reboot to debug confirmed from remote access.");
            PowerManager.RPCPowerListener.Start(PowerMode.RebootDebug);
        }

        private static void HandleSaveScr()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Save screen confirmed from remote access.");
            ScreensaverManager.ShowSavers();
            while (ScreensaverManager.inSaver)
                Thread.Sleep(1);
        }

        private static void HandleExec(string value)
        {
            string Command = value.Replace("ExecConfirm, ", "").Replace(CharManager.NewLine, "");
            if (Login.LoggedIn)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Exec confirmed from remote access.");
                TextWriterRaw.Write();
                ShellManager.GetLine(Command);
            }
            else
                DebugWriter.WriteDebug(DebugLevel.W, "Tried to exec from remote access while not logged in. Dropping packet...");
        }

        private static void HandleAcknowledge(IPEndPoint endpoint)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "{0} says \"Hello.\"", vars: [endpoint.Address.ToString()]);
            ReplyTo("N-KS-ACK", endpoint);
        }

        private static void HandlePing(string value)
        {
            string IPAddr = value.Replace("PingConfirm, ", "").Replace(CharManager.NewLine, "");
            DebugWriter.WriteDebug(DebugLevel.I, "{0} pinged this device!", vars: [IPAddr]);
            NotificationManager.NotifySend(new Notification(LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RPC_PINGACK_TITLE"), TextTools.FormatString(LanguageTools.GetLocalized("NKS_NETWORK_TYPES_RPC_PINGACK_DESC"), IPAddr), NotificationPriority.Low, NotificationType.Normal));
        }

        private static void HandleVersion(IPEndPoint endpoint)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "{0} tried to get version, sending it to the requester...", vars: [endpoint.Address.ToString()]);
            ReplyTo(KernelReleaseInfo.VersionFullStr, endpoint);
        }

        private static void HandleVersionCode(IPEndPoint endpoint)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "{0} tried to get version code, sending it to the requester...", vars: [endpoint.Address.ToString()]);
            var version = KernelReleaseInfo.Version ?? new();
            long versionCode =
                ((long)version.Major << 48) |
                ((long)version.Minor << 32) |
                ((long)version.Build << 16) |
                (long)version.Revision;
            ReplyTo($"{versionCode}", endpoint);
        }

        private static void HandleApiVersion(IPEndPoint endpoint)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "{0} tried to get API version, sending it to the requester...", vars: [endpoint.Address.ToString()]);
            ReplyTo(KernelReleaseInfo.ApiVersion.ToString(), endpoint);
        }

        private static void HandleApiVersionCode(IPEndPoint endpoint)
        {
            DebugWriter.WriteDebug(DebugLevel.I, "{0} tried to get API version code, sending it to the requester...", vars: [endpoint.Address.ToString()]);
            var version = KernelReleaseInfo.ApiVersion ?? new();
            long versionCode =
                ((long)version.Major << 48) |
                ((long)version.Minor << 32) |
                ((long)version.Build << 16) |
                (long)version.Revision;
            ReplyTo($"{versionCode}", endpoint);
        }
    }
}

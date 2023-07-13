
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
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Kernel.Power;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Screensaver;
using KS.Misc.Text;
using KS.Misc.Writers.ConsoleWriters;
using KS.Kernel.Events;
using KS.Kernel.Exceptions;
using System.Linq;

namespace KS.Network.RPC
{
    /// <summary>
    /// RPC commands module
    /// </summary>
    public static class RPCCommands
    {
        private static bool received = false;

        /// <summary>
        /// List of RPC commands.<br/>
        /// <br/>&lt;Request:Shutdown&gt;: Shuts down the remote kernel. Usage: &lt;Request:Shutdown&gt;(IP)
        /// <br/>&lt;Request:Reboot&gt;: Reboots the remote kernel. Usage: &lt;Request:Reboot&gt;(IP)
        /// <br/>&lt;Request:RebootSafe&gt;: Reboots the remote kernel to safe mode. Usage: &lt;Request:RebootSafe&gt;(IP)
        /// <br/>&lt;Request:SaveScr&gt;: Saves the screen remotely. Usage: &lt;Request:SaveScr&gt;(IP)
        /// <br/>&lt;Request:Exec&gt;: Executes a command remotely. Usage: &lt;Request:Exec&gt;(Command)
        /// <br/>&lt;Request:Acknowledge&gt;: Pings the remote kernel silently. Usage: &lt;Request:Acknowledge&gt;(IP)
        /// <br/>&lt;Request:Ping&gt;: Pings the remote kernel with notification. Usage: &lt;Request:Ping&gt;(IP)
        /// </summary>
        private readonly static List<string> RPCCommandsField = new()
        {
            "Shutdown",
            "Reboot",
            "RebootSafe",
            "SaveScr",
            "Exec",
            "Acknowledge",
            "Ping"
        };

        private readonly static Dictionary<string, Action<string>> RPCCommandReplyActions = new()
        {
            { "ShutdownConfirm",    (_)     => HandleShutdown() },
            { "RebootConfirm",      (_)     => HandleReboot() },
            { "RebootSafeConfirm",  (_)     => HandleRebootSafe() },
            { "SaveScrConfirm",     (_)     => HandleSaveScr() },
            { "ExecConfirm",                   HandleExec },
            { "AcknowledgeConfirm",            HandleAcknowledge },
            { "PingConfirm",                   HandlePing },
        };

        /// <summary>
        /// Send an RPC command to another instance of KS using the specified address
        /// </summary>
        /// <param name="Request">A request</param>
        /// <param name="IP">An IP address which the RPC is hosted</param>
        public static void SendCommand(string Request, string IP) =>
            SendCommand(Request, IP, RemoteProcedure.RPCPort);

        /// <summary>
        /// Send an RPC command to another instance of KS using the specified address
        /// </summary>
        /// <param name="Request">A request</param>
        /// <param name="IP">An IP address which the RPC is hosted</param>
        /// <param name="Port">A port which the RPC is hosted</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SendCommand(string Request, string IP, int Port)
        {
            if (RemoteProcedure.RPCEnabled)
            {
                // Get the command and the argument
                string Cmd = Request.Remove(Request.IndexOf("("));
                DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}", Cmd);
                string Arg = Request[(Request.IndexOf("(") + 1)..];
                DebugWriter.WriteDebug(DebugLevel.I, "Prototype Arg: {0}", Arg);
                Arg = Arg.Remove(Arg.Length - 1);
                DebugWriter.WriteDebug(DebugLevel.I, "Finished Arg: {0}", Arg);

                // Check the command
                if (RPCCommandsField.Any(Cmd.Contains))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Command found.");

                    // Check the request type
                    string RequestType = Cmd[(Cmd.IndexOf(":") + 1)..(Cmd.IndexOf(">"))];
                    var ByteMsg = Array.Empty<byte>();

                    // Populate the byte message to send the confirmation to
                    DebugWriter.WriteDebug(DebugLevel.I, "Stream opened for device {0}", Arg);
                    ByteMsg = Encoding.Default.GetBytes($"{RequestType}Confirm, " + Arg + CharManager.NewLine);

                    // Send the response
                    DebugWriter.WriteDebug(DebugLevel.I, "Sending response to device...");
                    RemoteProcedure.RPCListen.Send(ByteMsg, ByteMsg.Length, IP, Port);
                    EventsManager.FireEvent(EventType.RPCCommandSent, Cmd, Arg, IP, Port);
                }
                else
                    // Rare case reached. Drop it.
                    DebugWriter.WriteDebug(DebugLevel.E, "Malformed request. {0}", Cmd);
            }
            else
                throw new KernelException(KernelExceptionType.RemoteProcedure, Translate.DoTranslation("Trying to send an RPC command while RPC didn't start."));
        }

        /// <summary>
        /// Thread to listen to commands.
        /// </summary>
        public static void ReceiveCommand()
        {
            var RemoteEndpoint = new IPEndPoint(IPAddress.Any, RemoteProcedure.RPCPort);
            while (!RemoteProcedure.rpcStopping)
            {
                try
                {
                    var receiveResult = RemoteProcedure.RPCListen.BeginReceive(new AsyncCallback(AcknowledgeMessage), null);
                    while (!received)
                    {
                        Thread.Sleep(100);
                        if (RemoteProcedure.rpcStopping)
                            break;
                    }
                }
                catch (Exception ex)
                {
                    SocketException SE = (SocketException)ex.InnerException;
                    if (SE is not null)
                    {
                        if (!(SE.SocketErrorCode == SocketError.TimedOut))
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Error from host: {0}", SE.SocketErrorCode.ToString());
                            DebugWriter.WriteDebugStackTrace(ex);
                        }
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Fatal error: {0}", ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                        EventsManager.FireEvent(EventType.RPCCommandError, ex, RemoteEndpoint.Address.ToString(), RemoteEndpoint.Port);
                    }
                }
            }
            RemoteProcedure.RPCListen.Close();
        }

        private static void AcknowledgeMessage(IAsyncResult asyncResult)
        {
            received = true;
            try
            {
                if (RemoteProcedure.RPCListen is null || RemoteProcedure.RPCListen.Client is null)
                    return;
                if (RemoteProcedure.rpcStopping)
                    return;
                if (RemoteProcedure.RPCListen.Available == 0)
                    return;
                var endpoint = new IPEndPoint(IPAddress.Any, RemoteProcedure.RPCPort);
                byte[] MessageBuffer = RemoteProcedure.RPCListen.EndReceive(asyncResult, ref endpoint);
                string Message = Encoding.Default.GetString(MessageBuffer);

                // Get the command and the argument
                string Cmd = Message.Remove(Message.IndexOf(","));
                DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}", Cmd);
                string Arg = Message[(Message.IndexOf(",") + 2)..].Replace(Environment.NewLine, "");
                DebugWriter.WriteDebug(DebugLevel.I, "Final Arg: {0}", Arg);

                // If the message is not empty, parse it
                if (!string.IsNullOrEmpty(Message))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "RPC: Received message {0}", Message);
                    EventsManager.FireEvent(EventType.RPCCommandReceived, Message, endpoint.Address.ToString(), endpoint.Port);

                    // Invoke the action based on message
                    if (RPCCommandReplyActions.ContainsKey(Cmd))
                        RPCCommandReplyActions[Cmd].Invoke(Arg);
                    else
                        DebugWriter.WriteDebug(DebugLevel.W, "Not found. Message was {0}", Message);
                }
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to acknowledge message: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            received = false;
        }

        private static void HandleShutdown()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Shutdown confirmed from remote access.");
            KernelTools.RPCPowerListener.Start(PowerMode.Shutdown);
        }

        private static void HandleReboot()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Reboot confirmed from remote access.");
            KernelTools.RPCPowerListener.Start(PowerMode.Reboot);
        }

        private static void HandleRebootSafe()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Reboot to safe mode confirmed from remote access.");
            KernelTools.RPCPowerListener.Start(PowerMode.RebootSafe);
        }

        private static void HandleSaveScr()
        {
            DebugWriter.WriteDebug(DebugLevel.I, "Save screen confirmed from remote access.");
            Screensaver.ShowSavers();
            while (Screensaver.inSaver)
                Thread.Sleep(1);
        }

        private static void HandleExec(string value)
        {
            string Command = value.Replace("ExecConfirm, ", "").Replace(CharManager.NewLine, "");
            if (Flags.LoggedIn)
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Exec confirmed from remote access.");
                TextWriterColor.Write();
                Shell.Shell.GetLine(Command);
            }
            else
                DebugWriter.WriteDebug(DebugLevel.W, "Tried to exec from remote access while not logged in. Dropping packet...");
        }

        private static void HandleAcknowledge(string value)
        {
            string IPAddr = value.Replace("AckConfirm, ", "").Replace(CharManager.NewLine, "");
            DebugWriter.WriteDebug(DebugLevel.I, "{0} says \"Hello.\"", IPAddr);
        }

        private static void HandlePing(string value)
        {
            string IPAddr = value.Replace("PingConfirm, ", "").Replace(CharManager.NewLine, "");
            DebugWriter.WriteDebug(DebugLevel.I, "{0} pinged this device!", IPAddr);
            NotificationManager.NotifySend(new Notification(Translate.DoTranslation("Ping!"), string.Format(Translate.DoTranslation("{0} pinged you."), IPAddr), NotificationManager.NotifPriority.Low, NotificationManager.NotifType.Normal));
        }
    }
}

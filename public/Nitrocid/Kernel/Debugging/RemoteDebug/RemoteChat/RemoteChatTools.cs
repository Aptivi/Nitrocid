﻿//
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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Nitrocid.Kernel.Configuration;
using Nitrocid.Kernel.Threading;
using Nitrocid.ConsoleBase.Writers;
using Nitrocid.Kernel.Time.Renderers;
using Nitrocid.Misc.Notifications;
using Nitrocid.Languages;
using Nitrocid.Misc.Text.Probers.Placeholder;
using Nitrocid.Kernel.Events;
using Nitrocid.ConsoleBase.Colors;
using Nitrocid.Kernel.Exceptions;

namespace Nitrocid.Kernel.Debugging.RemoteDebug.RemoteChat
{
    /// <summary>
    /// Remote chat module
    /// </summary>
    public static class RemoteChatTools
    {

        internal static bool RDebugFailed;
        internal static Exception? RDebugFailedReason;
        internal static List<RemoteDebugDevice> DebugChatDevices = [];
        internal static Socket? RDebugChatClient;
        internal static TcpListener? DebugChatTCP;
        internal static KernelThread RDebugChatThread = new("Remote Debug Chat Thread", true, StartRDebugger) { isCritical = true };
        internal static int debugChatPort = 3015;
        private static readonly AutoResetEvent RDebugChatBailer = new(false);

        /// <summary>
        /// Whether the remote debug chat is stopping
        /// </summary>
        public static bool RDebugChatStopping { get; internal set; }

        /// <summary>
        /// Thread to accept connections after the listener starts
        /// </summary>
        public static void StartRDebugger()
        {
            // Listen to a current IP address
            try
            {
                DebugChatTCP = new TcpListener(IPAddress.Any, Config.MainConfig.DebugChatPort);
                DebugChatTCP.Start();
            }
            catch (SocketException sex)
            {
                RDebugFailed = true;
                RDebugFailedReason = sex;
                DebugWriter.WriteDebugStackTrace(sex);
            }
            RDebugChatBailer.Set();

            // Run forever! Until the remote debugger is stopping.
            while (!RDebugChatStopping)
            {
                try
                {
                    Thread.Sleep(1);

                    // Variables
                    NetworkStream RDebugStream;
                    StreamWriter RDebugSWriter;
                    Socket RDebugClient;
                    string RDebugIP = "";
                    string RDebugEndpoint;
                    string RDebugName;
                    RemoteDebugDevice RDebugInstance;

                    // Check for pending connections
                    if (DebugChatTCP is null)
                        continue;
                    if (DebugChatTCP.Pending())
                    {
                        // Populate the device variables with the information
                        RDebugClient = DebugChatTCP.AcceptSocket();

                        // Set the timeout of ten milliseconds to ensure that no device "take turns in messaging"
                        RDebugStream = new NetworkStream(RDebugClient);

                        // Add the device to JSON
                        RDebugEndpoint = RDebugClient.RemoteEndPoint?.ToString() ?? "";
                        RemoteDebugDeviceInfo? deviceInfo;
                        if (!string.IsNullOrEmpty(RDebugEndpoint))
                        {
                            RDebugIP = RDebugEndpoint.Remove(RDebugClient.RemoteEndPoint?.ToString()?.IndexOf(":") ?? 0);
                            deviceInfo = RemoteDebugTools.AddDevice(RDebugIP, false);
                        }
                        else
                            continue;

                        // Get the remaining properties
                        var RDebugThread = new KernelThread($"Remote Debug Listener Thread for {RDebugIP}", false, Listen);
                        RDebugInstance = new RemoteDebugDevice(RDebugClient, RDebugStream, RDebugThread, deviceInfo);
                        RDebugSWriter = RDebugInstance.ClientStreamWriter;

                        // Check the name
                        RDebugName = deviceInfo.Name;
                        if (string.IsNullOrEmpty(RDebugName))
                            DebugWriter.WriteDebug(DebugLevel.W, "Debug device {0} has no name. Prompting for name...", RDebugIP);

                        // Check to see if the device is blocked
                        if (deviceInfo.Blocked)
                        {
                            // Blocked! Disconnect it.
                            DebugWriter.WriteDebug(DebugLevel.W, "Debug device {0} ({1}) tried to join remote debug, but blocked.", RDebugName, RDebugIP);
                            RDebugClient.Disconnect(true);
                        }
                        else
                        {
                            // Not blocked yet. Add the connection.
                            DebugChatDevices.Add(RDebugInstance);
                            RDebugSWriter.Write(Translate.DoTranslation(">> Remote Chat: version") + $" {RemoteDebugger.RDebugVersion}\r\n");
                            RDebugSWriter.Write(Translate.DoTranslation(">> Your address is {0}.") + "\r\n", RDebugIP);
                            if (string.IsNullOrEmpty(RDebugName))
                                RDebugSWriter.Write(Translate.DoTranslation(">> Welcome! This is your first time entering remote chat. In the remote debugger, use \"/register <name>\" to register. You can't talk until you register.") + "\r\n", RDebugName);
                            else
                                RDebugSWriter.Write(Translate.DoTranslation(">> Your name is {0}.") + "\r\n", RDebugName);

                            // Acknowledge the debugger
                            DebugWriter.WriteDebug(DebugLevel.I, "Debug device \"{0}\" ({1}) connected.", RDebugName, RDebugIP);
                            RDebugSWriter.Flush();
                            RDebugThread.Start(RDebugInstance);
                            EventsManager.FireEvent(EventType.RemoteDebugConnectionAccepted, RDebugIP);
                        }
                    }
                }
                catch (ThreadInterruptedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    if (Config.MainConfig.NotifyOnRemoteDebugConnectionError)
                    {
                        var RemoteDebugError = new Notification(Translate.DoTranslation("Remote debugger connection error"), ex.Message, NotificationPriority.Medium, NotificationType.Normal);
                        NotificationManager.NotifySend(RemoteDebugError);
                    }
                    else
                    {
                        TextWriters.Write(Translate.DoTranslation("Remote debugger connection error") + ": {0}", true, KernelColorType.Error, ex.Message);
                    }
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }

            RDebugChatStopping = false;
            DebugChatTCP?.Stop();
            DebugChatDevices.Clear();
        }

        /// <summary>
        /// Thread to listen to messages and post them to the debugger
        /// </summary>
        internal static void Listen(object? RDebugInstance)
        {
            if (RDebugInstance is not RemoteDebugDevice device)
                return;

            while (!RDebugChatStopping)
            {
                try
                {
                    Thread.Sleep(1);
                    if (device is null)
                        throw new KernelException(KernelExceptionType.RemoteDebugDeviceOperation, Translate.DoTranslation("Remote debug device doesn't seem to exist."));

                    // Variables
                    var MessageBuffer = new byte[65537];
                    var SocketStream = device.ClientStream;
                    var SocketStreamWriter = device.ClientStreamWriter;
                    string SocketIP = device.ClientIP;
                    string SocketName = device.ClientName;

                    // Read a message from the stream
                    if (!SocketStream.DataAvailable)
                        if (device.ClientSocket.Connected)
                            continue;
                        else
                            break;
                    SocketStream.Read(MessageBuffer, 0, 65536);
                    string Message = System.Text.Encoding.Default.GetString(MessageBuffer);

                    // Make some fixups regarding newlines, which means remove all instances of vbCr (Mac OS 9 newlines) and vbLf (Linux newlines).
                    // Windows hosts are affected, too, because it uses vbCrLf, which means (vbCr + vbLf)
                    Message = Message.Replace(Convert.ToChar(13), default);
                    Message = Message.Replace(Convert.ToChar(10), default);

                    // Now, remove all null chars
                    Message = Message.Replace(Convert.ToString(Convert.ToChar(0)), "");

                    // If the message is empty, return.
                    if (string.IsNullOrWhiteSpace(Message))
                        continue;

                    // Don't post message if it starts with a null character. On Unix, the nullchar detection always returns false even if it seems
                    // that the message starts with the actual character, not the null character, so detect nullchar by getting the first character
                    // from the message and comparing it to the null char ASCII number, which is 0.
                    if (Convert.ToInt32(Message[0]) != 0)
                    {
                        // Check to see if the unnamed stranger is trying to send a message
                        var deviceInfo = RemoteDebugTools.GetDeviceFromIp(SocketIP);
                        if (!string.IsNullOrEmpty(SocketName))
                        {
                            // Check the message format
                            if (string.IsNullOrWhiteSpace(Config.MainConfig.RDebugMessageFormat))
                                Config.MainConfig.RDebugMessageFormat = "{0}> {1}";

                            // Decide if we're recording the chat to the debug log
                            if (Config.MainConfig.RecordChatToDebugLog)
                                DebugWriter.WriteDebugLogOnly(DebugLevel.I, PlaceParse.ProbePlaces(Config.MainConfig.RDebugMessageFormat), SocketName, Message);
                            DebugWriter.WriteDebugChatsOnly(DebugLevel.I, PlaceParse.ProbePlaces(Config.MainConfig.RDebugMessageFormat), true, SocketName, Message);

                            // Add the message to the chat history
                            deviceInfo.chatHistory.Add($"[{TimeDateRenderers.Render()}] {Message}");
                        }
                    }
                }
                catch
                {
                    string SocketIP = device?.ClientIP ?? "";
                    if (!string.IsNullOrWhiteSpace(SocketIP))
                        RemoteDebugTools.DisconnectDevice(SocketIP);
                }
            }
        }

    }
}

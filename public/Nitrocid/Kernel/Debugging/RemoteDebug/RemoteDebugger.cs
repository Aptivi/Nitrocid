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

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Kernel.Events;
using KS.Kernel.Debugging.RemoteDebug.Command;
using KS.Kernel.Configuration;
using KS.Kernel.Time.Renderers;
using KS.Kernel.Threading;
using KS.ConsoleBase.Writers.ConsoleWriters;
using SemanVer.Instance;
using KS.Misc.Text.Probers.Placeholder;

namespace KS.Kernel.Debugging.RemoteDebug
{
    /// <summary>
    /// Remote debugger module
    /// </summary>
    public static class RemoteDebugger
    {

        internal static bool RDebugFailed;
        internal static Exception RDebugFailedReason;
        internal static List<RemoteDebugDevice> DebugDevices = new();
        internal static Socket RDebugClient;
        internal static TcpListener DebugTCP;
        internal static KernelThread RDebugThread = new("Remote Debug Thread", true, StartRDebugger) { isCritical = true };
        internal static int debugPort = 3014;
        private readonly static SemVer RDebugVersion = SemVer.ParseWithRev("0.8.3.1");
        private static readonly AutoResetEvent RDebugBailer = new(false);

        /// <summary>
        /// Remote debugger port
        /// </summary>
        public static int DebugPort =>
            Config.MainConfig.DebugPort;
        /// <summary>
        /// Whether the remote debug is stopping
        /// </summary>
        public static bool RDebugStopping { get; set; }
        /// <summary>
        /// Whether to automatically start the remote debugger
        /// </summary>
        public static bool RDebugAutoStart =>
            Config.MainConfig.RDebugAutoStart;
        /// <summary>
        /// Remote debug message format
        /// </summary>
        public static string RDebugMessageFormat =>
            Config.MainConfig.RDebugMessageFormat;

        /// <summary>
        /// Whether to start or stop the remote debugger
        /// </summary>
        public static void StartRDebugThread()
        {
            if (KernelEntry.DebugMode)
            {
                if (!RDebugThread.IsAlive)
                {
                    RDebugThread.Start();
                    RDebugBailer.WaitOne();
                }
            }
        }

        /// <summary>
        /// Whether to start or stop the remote debugger
        /// </summary>
        public static void StopRDebugThread()
        {
            if (KernelEntry.DebugMode)
            {
                if (RDebugThread.IsAlive)
                {
                    RDebugStopping = true;
                    RDebugThread.Stop();
                }
            }
        }

        /// <summary>
        /// Thread to accept connections after the listener starts
        /// </summary>
        public static void StartRDebugger()
        {
            // Listen to a current IP address
            try
            {
                DebugTCP = new TcpListener(IPAddress.Any, DebugPort);
                DebugTCP.Start();
            }
            catch (SocketException sex)
            {
                RDebugFailed = true;
                RDebugFailedReason = sex;
                DebugWriter.WriteDebugStackTrace(sex);
            }
            RDebugBailer.Set();

            // Run forever! Until the remote debugger is stopping.
            while (!RDebugStopping)
            {
                try
                {
                    Thread.Sleep(1);

                    // Variables
                    NetworkStream RDebugStream;
                    StreamWriter RDebugSWriter;
                    Socket RDebugClient;
                    string RDebugIP;
                    string RDebugEndpoint;
                    string RDebugName;
                    RemoteDebugDevice RDebugInstance;

                    // Check for pending connections
                    if (DebugTCP.Pending())
                    {
                        // Populate the device variables with the information
                        RDebugClient = DebugTCP.AcceptSocket();

                        // Set the timeout of ten milliseconds to ensure that no device "take turns in messaging"
                        RDebugStream = new NetworkStream(RDebugClient);

                        // Add the device to JSON
                        RDebugEndpoint = RDebugClient.RemoteEndPoint.ToString();
                        RDebugIP = RDebugEndpoint.Remove(RDebugClient.RemoteEndPoint.ToString().IndexOf(":"));
                        var device = RemoteDebugTools.AddDevice(RDebugIP, false);

                        // Get the remaining properties
                        var RDebugThread = new KernelThread($"Remote Debug Listener Thread for {RDebugIP}", false, Listen);
                        RDebugInstance = new RemoteDebugDevice(RDebugClient, RDebugStream, RDebugThread, device);
                        RDebugSWriter = RDebugInstance.ClientStreamWriter;

                        // Check the name
                        RDebugName = device.Name;
                        if (string.IsNullOrEmpty(RDebugName))
                            DebugWriter.WriteDebug(DebugLevel.W, "Debug device {0} has no name. Prompting for name...", RDebugIP);

                        // Check to see if the device is blocked
                        if (device.Blocked)
                        {
                            // Blocked! Disconnect it.
                            DebugWriter.WriteDebug(DebugLevel.W, "Debug device {0} ({1}) tried to join remote debug, but blocked.", RDebugName, RDebugIP);
                            RDebugClient.Disconnect(true);
                        }
                        else
                        {
                            // Not blocked yet. Add the connection.
                            DebugDevices.Add(RDebugInstance);
                            RDebugSWriter.Write(Translate.DoTranslation(">> Remote Debug and Chat: version") + $" {RDebugVersion}\r\n");
                            RDebugSWriter.Write(Translate.DoTranslation(">> Your address is {0}.") + "\r\n", RDebugIP);
                            if (string.IsNullOrEmpty(RDebugName))
                                RDebugSWriter.Write(Translate.DoTranslation(">> Welcome! This is your first time entering remote debug and chat. Use \"/register <name>\" to register.") + "\r\n", RDebugName);
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
                    if (RemoteDebugTools.NotifyOnRemoteDebugConnectionError)
                    {
                        var RemoteDebugError = new Notification(Translate.DoTranslation("Remote debugger connection error"), ex.Message, NotificationPriority.Medium, NotificationType.Normal);
                        NotificationManager.NotifySend(RemoteDebugError);
                    }
                    else
                    {
                        TextWriterColor.WriteKernelColor(Translate.DoTranslation("Remote debugger connection error") + ": {0}", true, KernelColorType.Error, ex.Message);
                    }
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }

            RDebugStopping = false;
            DebugTCP.Stop();
            DebugDevices.Clear();
        }

        /// <summary>
        /// Thread to listen to messages and post them to the debugger
        /// </summary>
        internal static void Listen(object RDebugInstance)
        {
            if (RDebugInstance is not RemoteDebugDevice device)
                return;

            while (!RDebugStopping)
            {
                try
                {
                    Thread.Sleep(1);

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
                    if (!(Convert.ToInt32(Message[0]) == 0))
                    {
                        // Now, check to see if the message is a command
                        if (Message.StartsWith("/"))
                        {
                            // The message is a command!
                            string finalCommand = Message[1..];
                            RemoteDebugCommandExecutor.ExecuteCommand(finalCommand, device);
                        }
                        else
                        {
                            // Check to see if the unnamed stranger is trying to send a message
                            var deviceInfo = RemoteDebugTools.GetDeviceFromIp(SocketIP);
                            if (!string.IsNullOrEmpty(SocketName))
                            {
                                // Check the message format
                                if (string.IsNullOrWhiteSpace(RDebugMessageFormat))
                                    Config.MainConfig.RDebugMessageFormat = "{0}> {1}";

                                // Decide if we're recording the chat to the debug log
                                if (RemoteDebugTools.RecordChatToDebugLog)
                                    DebugWriter.WriteDebugLogOnly(DebugLevel.I, PlaceParse.ProbePlaces(RDebugMessageFormat), SocketName, Message);
                                DebugWriter.WriteDebugDevicesOnly(DebugLevel.I, PlaceParse.ProbePlaces(RDebugMessageFormat), true, SocketName, Message);

                                // Add the message to the chat history
                                deviceInfo.chatHistory.Add($"[{TimeDateRenderers.Render()}] {Message}");
                            }
                        }
                    }
                }
                catch
                {
                    string SocketIP = device?.ClientIP;
                    if (!string.IsNullOrWhiteSpace(SocketIP))
                        RemoteDebugTools.DisconnectDevice(SocketIP);
                }
            }
        }

    }
}

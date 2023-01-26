
// Kernel Simulator  Copyright (C) 2018-2023  Aptivi
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using KS.ConsoleBase.Colors;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Probers;
using KS.Misc.Threading;
using KS.Misc.Writers.ConsoleWriters;
using KS.TimeDate;
using KS.Kernel.Events;
using KS.Kernel.Debugging.RemoteDebug.Command;

namespace KS.Kernel.Debugging.RemoteDebug
{
    /// <summary>
    /// Remote debugger module
    /// </summary>
    public static class RemoteDebugger
    {

        /// <summary>
        /// Remote debugger port
        /// </summary>
        public static int DebugPort
        {
            get => debugPort;
            set => debugPort = value < 0 ? 3014 : value;
        }
        /// <summary>
        /// Whether the remote debug is stopping
        /// </summary>
        public static bool RDebugStopping { get; set; }
        /// <summary>
        /// Whether to automatically start the remote debugger
        /// </summary>
        public static bool RDebugAutoStart { get; set; } = true;
        /// <summary>
        /// Remote debug message format
        /// </summary>
        public static string RDebugMessageFormat { get; set; } = "";
        internal static bool RDebugFailed;
        internal static Exception RDebugFailedReason;
        internal static List<string> RDebugBlocked = new();
        internal static List<RemoteDebugDevice> DebugDevices = new();
        internal static Socket RDebugClient;
        internal static TcpListener DebugTCP;
        internal static KernelThread RDebugThread = new("Remote Debug Thread", true, StartRDebugger) { isCritical = true };
        private static int debugPort = 3014;
        private readonly static string RDebugVersion = "0.7.1";
        private static readonly AutoResetEvent RDebugBailer = new(false);

        /// <summary>
        /// Whether to start or stop the remote debugger
        /// </summary>
        public static void StartRDebugThread()
        {
            if (Flags.DebugMode)
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
            if (Flags.DebugMode)
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
                        RemoteDebugTools.AddDeviceToJson(RDebugIP, false);

                        // Get the remaining properties
                        RDebugName = Convert.ToString(RemoteDebugTools.GetDeviceProperty(RDebugIP, RemoteDebugTools.DeviceProperty.Name));
                        var RDebugThread = new KernelThread($"Remote Debug Listener Thread for {RDebugIP}", false, Listen);
                        RDebugInstance = new RemoteDebugDevice(RDebugClient, RDebugStream, RDebugIP, RDebugName, RDebugThread);
                        RDebugSWriter = RDebugInstance.ClientStreamWriter;

                        // Check the name
                        if (string.IsNullOrEmpty(RDebugName))
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Debug device {0} has no name. Prompting for name...", RDebugIP);
                        }

                        // Check to see if the device is blocked
                        if (RDebugBlocked.Contains(RDebugIP))
                        {
                            // Blocked! Disconnect it.
                            DebugWriter.WriteDebug(DebugLevel.W, "Debug device {0} ({1}) tried to join remote debug, but blocked.", RDebugName, RDebugIP);
                            RDebugClient.Disconnect(true);
                        }
                        else
                        {
                            // Not blocked yet. Add the connection.
                            DebugDevices.Add(RDebugInstance);
                            RDebugSWriter.WriteLine(Translate.DoTranslation(">> Remote Debug and Chat: version") + " {0}", RDebugVersion);
                            RDebugSWriter.WriteLine(Translate.DoTranslation(">> Your address is {0}."), RDebugIP);
                            if (string.IsNullOrEmpty(RDebugName))
                            {
                                RDebugSWriter.WriteLine(Translate.DoTranslation(">> Welcome! This is your first time entering remote debug and chat. Use \"/register <name>\" to register.") + " ", RDebugName);
                            }
                            else
                            {
                                RDebugSWriter.WriteLine(Translate.DoTranslation(">> Your name is {0}."), RDebugName);
                            }

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
                    if (Flags.NotifyOnRemoteDebugConnectionError)
                    {
                        var RemoteDebugError = new Notification(Translate.DoTranslation("Remote debugger connection error"), ex.Message, NotificationManager.NotifPriority.Medium, NotificationManager.NotifType.Normal);
                        NotificationManager.NotifySend(RemoteDebugError);
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Remote debugger connection error") + ": {0}", true, KernelColorType.Error, ex.Message);
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
                        // Check to see if the unnamed stranger is trying to send a message
                        if (!string.IsNullOrEmpty(SocketName))
                        {
                            // Check the message format
                            if (string.IsNullOrWhiteSpace(RDebugMessageFormat))
                                RDebugMessageFormat = "{0}> {1}";

                            // Decide if we're recording the chat to the debug log
                            if (Flags.RecordChatToDebugLog)
                                DebugWriter.WriteDebug(DebugLevel.I, PlaceParse.ProbePlaces(RDebugMessageFormat), SocketName, Message);
                            else
                                DebugWriter.WriteDebugDevicesOnly(DebugLevel.I, PlaceParse.ProbePlaces(RDebugMessageFormat), SocketName, Message);

                            // Add the message to the chat history
                            RemoteDebugTools.SetDeviceProperty(SocketIP, RemoteDebugTools.DeviceProperty.ChatHistory, "[" + TimeDateRenderers.Render() + "] " + Message);
                        }

                        // Now, check to see if the message is a command
                        if (Message.StartsWith("/"))
                        {
                            string finalCommand = Message.Substring(1);
                            RemoteDebugCommandExecutor.ExecuteCommand(finalCommand, device.ClientIP);
                        }
                    }
                }
                catch (IOException ioex) when (ioex.Message.Contains("non-connected"))
                {
                    // HACK: Ugly workaround, but we have to search the message for "non-connected" to get the specific error message that we
                    // need to react appropriately. Removing the device from the debug devices list will allow the kernel to continue working
                    // without crashing just because of this exception. We had to search the above word in this phrase:
                    // 
                    // System.IO.IOException: The operation is not allowed on non-connected sockets.
                    //                                                        ^^^^^^^^^^^^^
                    // 
                    // Though, we wish to have a better workaround to detect this specific error message on .NET
                    DebugDevices.Remove(device);
                }
                catch (Exception ex)
                {
                    SocketException SE = (SocketException)ex.InnerException;
                    string SocketIP = device?.ClientIP;
                    if (SE is not null)
                    {
                        if (!(SE.SocketErrorCode == SocketError.WouldBlock))
                        {
                            if (SocketIP is not null)
                            {
                                DebugWriter.WriteDebug(DebugLevel.E, "Error from host {0}: {1}", SocketIP, SE.SocketErrorCode.ToString());
                                DebugWriter.WriteDebugStackTrace(ex);
                            }
                            else
                            {
                                DebugWriter.WriteDebug(DebugLevel.E, "Error from unknown host: {0}", SE.SocketErrorCode.ToString());
                                DebugWriter.WriteDebugStackTrace(ex);
                            }
                        }
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Unknown error of remote debug: {0}: {1}", ex.GetType().FullName, ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                    }
                }
            }
        }

    }
}

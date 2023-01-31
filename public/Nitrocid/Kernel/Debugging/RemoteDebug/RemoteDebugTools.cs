
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
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using KS.Files;
using KS.Files.Operations;
using KS.Kernel.Exceptions;
using KS.Languages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using KS.Kernel.Events;

namespace KS.Kernel.Debugging.RemoteDebug
{
    /// <summary>
    /// Remote debug tool module
    /// </summary>
    public static class RemoteDebugTools
    {

        /// <summary>
        /// Device property enumeration
        /// </summary>
        public enum DeviceProperty
        {
            /// <summary>
            /// Device name
            /// </summary>
            Name,
            /// <summary>
            /// Is the device blocked?
            /// </summary>
            Blocked,
            /// <summary>
            /// Device chat history
            /// </summary>
            ChatHistory
        }

        /// <summary>
        /// Disconnects a specified debug device
        /// </summary>
        /// <param name="IPAddr">An IP address of the connected debug device</param>
        public static void DisconnectDbgDev(string IPAddr)
        {
            var devices = RemoteDebugger.DebugDevices.Where((rdd) => rdd.ClientIP == IPAddr).ToList();
            for (int i = 0; i <= devices.Count - 1; i++)
            {
                string clientIp = RemoteDebugger.DebugDevices[i].ClientIP;
                string clientName = RemoteDebugger.DebugDevices[i].ClientName;
                RemoteDebugger.DebugDevices[i].ClientSocket.Disconnect(true);
                EventsManager.FireEvent(EventType.RemoteDebugConnectionDisconnected, IPAddr);
                RemoteDebugger.DebugDevices.RemoveAt(i);
                DebugWriter.WriteDebug(DebugLevel.W, "Debug device {0} ({1}) disconnected.", clientName, clientIp);
            }
            if (!devices.Any())
                throw new KernelException(KernelExceptionType.RemoteDebugDeviceNotFound, Translate.DoTranslation("Debug device {0} not found."), IPAddr);
        }

        /// <summary>
        /// Adds device to block list
        /// </summary>
        /// <param name="IP">An IP address for device</param>
        public static void AddToBlockList(string IP)
        {
            var BlockedDevices = ListDevices();
            DebugWriter.WriteDebug(DebugLevel.I, "Devices count: {0}", BlockedDevices.Length);
            if (BlockedDevices.Contains(IP) && !(bool)GetDeviceProperty(IP, DeviceProperty.Blocked))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Device {0} will be blocked...", IP);
                DisconnectDbgDev(IP);
                SetDeviceProperty(IP, DeviceProperty.Blocked, true);
                RemoteDebugger.RDebugBlocked.Add(IP);
            }
            else if (BlockedDevices.Contains(IP) && (bool)GetDeviceProperty(IP, DeviceProperty.Blocked))
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Trying to add an already-blocked device {0}. Adding to list...", IP);
                if (!RemoteDebugger.RDebugBlocked.Contains(IP))
                {
                    DisconnectDbgDev(IP);
                    RemoteDebugger.RDebugBlocked.Add(IP);
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Trying to add an already-blocked device {0}.", IP);
                    throw new KernelException(KernelExceptionType.RemoteDebugDeviceAlreadyExists, Translate.DoTranslation("Device already exists in the block list."));
                }
            }
        }

        /// <summary>
        /// Adds device to block list
        /// </summary>
        /// <param name="IP">An IP address for device</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool TryAddToBlockList(string IP)
        {
            try
            {
                AddToBlockList(IP);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to add device to block list: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Removes device from block list
        /// </summary>
        /// <param name="IP">A blocked IP address for device</param>
        public static void RemoveFromBlockList(string IP)
        {
            var BlockedDevices = ListDevices();
            DebugWriter.WriteDebug(DebugLevel.I, "Devices count: {0}", BlockedDevices.Length);
            if (BlockedDevices.Contains(IP))
            {
                DebugWriter.WriteDebug(DebugLevel.I, "Device {0} found.", IP);
                RemoteDebugger.RDebugBlocked.Remove(IP);
                SetDeviceProperty(IP, DeviceProperty.Blocked, false);
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Trying to remove an already-unblocked device {0}. Removing from list...", IP);
                if (!RemoteDebugger.RDebugBlocked.Remove(IP))
                    throw new KernelException(KernelExceptionType.RemoteDebugDeviceOperation, Translate.DoTranslation("Device doesn't exist in the block list."));
            }
        }

        /// <summary>
        /// Removes device from block list
        /// </summary>
        /// <param name="IP">A blocked IP address for device</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool TryRemoveFromBlockList(string IP)
        {
            try
            {
                RemoveFromBlockList(IP);
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to remove device from block list: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Populates blocked devices
        /// </summary>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool PopulateBlockedDevices()
        {
            try
            {
                var BlockEntries = ListDevices();
                DebugWriter.WriteDebug(DebugLevel.I, "Devices count: {0}", BlockEntries.Length);
                foreach (string BlockEntry in BlockEntries)
                {
                    if (Convert.ToBoolean(GetDeviceProperty(BlockEntry, DeviceProperty.Blocked)))
                        AddToBlockList(BlockEntry);
                }
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to populate block list: {0}", ex.Message);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Gets device property from device IP address
        /// </summary>
        /// <param name="DeviceIP">Device IP address from remote endpoint address</param>
        /// <param name="DeviceProperty">Device property</param>
        /// <returns>Device property if successful; nothing if unsuccessful.</returns>
        public static object GetDeviceProperty(string DeviceIP, DeviceProperty DeviceProperty)
        {
            string DeviceJsonContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.DebugDevNames));
            var DeviceNameToken = JObject.Parse(!string.IsNullOrEmpty(DeviceJsonContent) ? DeviceJsonContent : "{}");
            JObject DeviceProperties = DeviceNameToken[DeviceIP] as JObject;
            if (DeviceProperties is not null)
            {
                switch (DeviceProperty)
                {
                    case DeviceProperty.Name:
                        {
                            return DeviceProperties.Property("Name").Value.ToString();
                        }
                    case DeviceProperty.Blocked:
                        {
                            return DeviceProperties.Property("Blocked").Value;
                        }
                    case DeviceProperty.ChatHistory:
                        {
                            return DeviceProperties.Property("ChatHistory").Value.ToArray();
                        }
                }
            }
            else
            {
                throw new KernelException(KernelExceptionType.RemoteDebugDeviceNotFound, Translate.DoTranslation("No such device."));
            }
            return null;
        }

        /// <summary>
        /// Sets device property from device IP address
        /// </summary>
        /// <param name="DeviceIP">Device IP address from remote endpoint address</param>
        /// <param name="DeviceProperty">Device property</param>
        /// <param name="Value">Value</param>
        public static void SetDeviceProperty(string DeviceIP, DeviceProperty DeviceProperty, object Value)
        {
            string DeviceJsonContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.DebugDevNames));
            var DeviceNameToken = JObject.Parse(!string.IsNullOrEmpty(DeviceJsonContent) ? DeviceJsonContent : "{}");
            JObject DeviceProperties = DeviceNameToken[DeviceIP] as JObject;
            if (DeviceProperties is not null)
            {
                switch (DeviceProperty)
                {
                    case DeviceProperty.Name:
                        {
                            DeviceProperties["Name"] = JToken.FromObject(Value);
                            break;
                        }
                    case DeviceProperty.Blocked:
                        {
                            DeviceProperties["Blocked"] = JToken.FromObject(Value);
                            break;
                        }
                    case DeviceProperty.ChatHistory:
                        {
                            JArray ChatHistory = DeviceProperties["ChatHistory"] as JArray;
                            ChatHistory.Add(Value);
                            break;
                        }
                }
                File.WriteAllText(Paths.GetKernelPath(KernelPathType.DebugDevNames), JsonConvert.SerializeObject(DeviceNameToken, Formatting.Indented));
            }
            else
            {
                throw new KernelException(KernelExceptionType.RemoteDebugDeviceNotFound, Translate.DoTranslation("No such device."));
            }
        }

        /// <summary>
        /// Sets device property from device IP address
        /// </summary>
        /// <param name="DeviceIP">Device IP address from remote endpoint address</param>
        /// <param name="DeviceProperty">Device property</param>
        /// <param name="Value">Value</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool TrySetDeviceProperty(string DeviceIP, DeviceProperty DeviceProperty, object Value)
        {
            try
            {
                SetDeviceProperty(DeviceIP, DeviceProperty, Value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds new device IP address to JSON
        /// </summary>
        /// <param name="DeviceIP">Device IP address from remote endpoint address</param>
        /// <param name="ThrowException">Optionally throw exception</param>
        public static void AddDeviceToJson(string DeviceIP, bool ThrowException = true)
        {
            Making.MakeFile(Paths.GetKernelPath(KernelPathType.DebugDevNames), false);
            string DeviceJsonContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.DebugDevNames));
            var DeviceNameToken = JObject.Parse(!string.IsNullOrEmpty(DeviceJsonContent) ? DeviceJsonContent : "{}");
            if (DeviceNameToken[DeviceIP] is null)
            {
                var NewDevice = new JObject(new JProperty("Name", ""), new JProperty("Blocked", false), new JProperty("ChatHistory", new JArray()));
                DeviceNameToken.Add(DeviceIP, NewDevice);
                File.WriteAllText(Paths.GetKernelPath(KernelPathType.DebugDevNames), JsonConvert.SerializeObject(DeviceNameToken, Formatting.Indented));
            }
            else if (ThrowException)
                throw new KernelException(KernelExceptionType.RemoteDebugDeviceAlreadyExists, Translate.DoTranslation("Device already exists."));
        }

        /// <summary>
        /// Adds new device IP address to JSON
        /// </summary>
        /// <param name="DeviceIP">Device IP address from remote endpoint address</param>
        /// <param name="ThrowException">Optionally throw exception</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool TryAddDeviceToJson(string DeviceIP, bool ThrowException = true)
        {
            try
            {
                AddDeviceToJson(DeviceIP, ThrowException);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Removes a device IP address from JSON
        /// </summary>
        /// <param name="DeviceIP">Device IP address from remote endpoint address</param>
        public static void RemoveDeviceFromJson(string DeviceIP)
        {
            string DeviceJsonContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.DebugDevNames));
            var DeviceNameToken = JObject.Parse(!string.IsNullOrEmpty(DeviceJsonContent) ? DeviceJsonContent : "{}");
            if (DeviceNameToken[DeviceIP] is not null)
            {
                DeviceNameToken.Remove(DeviceIP);
                File.WriteAllText(Paths.GetKernelPath(KernelPathType.DebugDevNames), JsonConvert.SerializeObject(DeviceNameToken, Formatting.Indented));
            }
            else
            {
                throw new KernelException(KernelExceptionType.RemoteDebugDeviceNotFound, Translate.DoTranslation("No such device."));
            }
        }

        /// <summary>
        /// Removes a device IP address from JSON
        /// </summary>
        /// <param name="DeviceIP">Device IP address from remote endpoint address</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool TryRemoveDeviceFromJson(string DeviceIP)
        {
            try
            {
                RemoveDeviceFromJson(DeviceIP);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Lists all devices and puts them into an array
        /// </summary>
        public static string[] ListDevices()
        {
            Making.MakeFile(Paths.GetKernelPath(KernelPathType.DebugDevNames), false);
            string DeviceJsonContent = File.ReadAllText(Paths.GetKernelPath(KernelPathType.DebugDevNames));
            var DeviceNameToken = JObject.Parse(!string.IsNullOrEmpty(DeviceJsonContent) ? DeviceJsonContent : "{}");
            return DeviceNameToken.Properties().Select(p => p.Name).ToArray();
        }

        /// <summary>
        /// Disconnects debug device depending on exception
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="deviceIndex">Device index</param>
        internal static void DisconnectDependingOnException(Exception exception, int deviceIndex)
        {
            SocketException SE = (SocketException)exception.InnerException;
            if (SE is not null)
            {
                if ((SE.SocketErrorCode == SocketError.TimedOut) |
                    (SE.SocketErrorCode == SocketError.WouldBlock) |
                    (SE.SocketErrorCode == SocketError.ConnectionAborted))
                {
                    DisconnectDbgDev(RemoteDebugger.DebugDevices[deviceIndex].ClientIP);
                }
                else
                    DebugWriter.WriteDebugStackTrace(exception);
            }
            else
            {
                DisconnectDbgDev(RemoteDebugger.DebugDevices[deviceIndex].ClientIP);
                DebugWriter.WriteDebugStackTrace(exception);
            }
        }

    }
}

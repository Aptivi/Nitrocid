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
using System.Data;
using System.Linq;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Collections.Generic;
using Nitrocid.Base.Files;
using Nitrocid.Base.Languages;
using Nitrocid.Base.Kernel.Exceptions;
using Nitrocid.Base.Files.Paths;
using Nitrocid.Base.Kernel.Events;
using Nitrocid.Base.Kernel.Debugging.RemoteDebug.RemoteChat;

namespace Nitrocid.Base.Kernel.Debugging.RemoteDebug
{
    /// <summary>
    /// Remote debug tool module
    /// </summary>
    public static class RemoteDebugTools
    {

        private static List<RemoteDebugDeviceInfo> remoteDebugDevices = [];

        /// <summary>
        /// Disconnects a specified debug device
        /// </summary>
        /// <param name="address">An IP address of the connected debug device</param>
        public static void DisconnectDevice(string address)
        {
            // Normal remote debugger devices
            var devices = RemoteDebugger.DebugDevices.Where((rdd) => rdd.ClientIP == address).ToList();
            for (int i = 0; i <= devices.Count - 1; i++)
            {
                try
                {
                    string clientIp = RemoteDebugger.DebugDevices[i].ClientIP;
                    string clientName = RemoteDebugger.DebugDevices[i].ClientName;
                    RemoteDebugger.DebugDevices[i].ClientSocket.Disconnect(true);
                    RemoteDebugger.DebugDevices.RemoveAt(i);
                    EventsManager.FireEvent(EventType.RemoteDebugConnectionDisconnected, address);
                    DebugWriter.WriteDebug(DebugLevel.W, "Debug device {0} ({1}) disconnected.", vars: [clientName, clientIp]);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Debug device {0} failed to disconnect: {1}.", vars: [address, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }

            // Chat remote debugger devices
            var chats = RemoteChatTools.DebugChatDevices.Where((rdd) => rdd.ClientIP == address).ToList();
            for (int i = 0; i <= chats.Count - 1; i++)
            {
                try
                {
                    string clientIp = RemoteChatTools.DebugChatDevices[i].ClientIP;
                    string clientName = RemoteChatTools.DebugChatDevices[i].ClientName;
                    RemoteChatTools.DebugChatDevices[i].ClientSocket.Disconnect(true);
                    RemoteChatTools.DebugChatDevices.RemoveAt(i);
                    EventsManager.FireEvent(EventType.RemoteDebugConnectionDisconnected, address);
                    DebugWriter.WriteDebug(DebugLevel.W, "Debug device {0} ({1}) disconnected from chat.", vars: [clientName, clientIp]);
                }
                catch (Exception ex)
                {
                    DebugWriter.WriteDebug(DebugLevel.E, "Debug device {0} failed to disconnect from chat: {1}.", vars: [address, ex.Message]);
                    DebugWriter.WriteDebugStackTrace(ex);
                }
            }
        }

        /// <summary>
        /// Adds device to block list
        /// </summary>
        /// <param name="address">An IP address for device</param>
        /// <param name="throwIfNotFound">Throws if the device is not found to be blocked</param>
        public static void AddToBlockList(string address, bool throwIfNotFound = false)
        {
            var BlockedDevices = ListDeviceAddresses();
            DebugWriter.WriteDebug(DebugLevel.I, "Devices count: {0}", vars: [BlockedDevices.Length]);
            if (BlockedDevices.Contains(address))
            {
                var device = GetDeviceFromIp(address);
                if (!device.Blocked)
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Device {0} will be blocked...", vars: [address]);
                    DisconnectDevice(address);
                    device.blocked = true;
                    SaveAllDevices();
                }
                else
                {
                    DebugWriter.WriteDebug(DebugLevel.W, "Trying to add an already-blocked device {0}.", vars: [address]);
                    throw new KernelException(KernelExceptionType.RemoteDebugDeviceAlreadyExists, LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_REMOTEDEBUG_EXCEPTION_ALREADYBLOCKED"));
                }
            }
            else if (throwIfNotFound)
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Trying to add a non-existent device {0}.", vars: [address]);
                throw new KernelException(KernelExceptionType.RemoteDebugDeviceNotFound, LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_REMOTEDEBUG_EXCEPTION_DEVICENOTFOUND_BLOCK"));
            }
        }

        /// <summary>
        /// Adds device to block list
        /// </summary>
        /// <param name="address">An IP address for device</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool TryAddToBlockList(string address)
        {
            try
            {
                AddToBlockList(address);
                SaveAllDevices();
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to add device to block list: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Removes device from block list
        /// </summary>
        /// <param name="address">A blocked IP address for device</param>
        public static void RemoveFromBlockList(string address)
        {
            var BlockedDevices = ListDeviceAddresses();
            DebugWriter.WriteDebug(DebugLevel.I, "Devices count: {0}", vars: [BlockedDevices.Length]);
            if (BlockedDevices.Contains(address))
            {
                var device = GetDeviceFromIp(address);
                DebugWriter.WriteDebug(DebugLevel.I, "Device {0} found.", vars: [address]);
                device.blocked = false;
                SaveAllDevices();
            }
            else
            {
                DebugWriter.WriteDebug(DebugLevel.W, "Trying to remove an already-unblocked device {0}.", vars: [address]);
                throw new KernelException(KernelExceptionType.RemoteDebugDeviceOperation, LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_REMOTEDEBUG_EXCEPTION_NOLONGERBLOCKED"));
            }
        }

        /// <summary>
        /// Removes device from block list
        /// </summary>
        /// <param name="address">A blocked IP address for device</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool TryRemoveFromBlockList(string address)
        {
            try
            {
                RemoveFromBlockList(address);
                SaveAllDevices();
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to remove device from block list: {0}", vars: [ex.Message]);
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
                var addresses = ListDeviceAddresses();
                DebugWriter.WriteDebug(DebugLevel.I, "Devices count: {0}", vars: [addresses.Length]);
                foreach (string address in addresses)
                {
                    var device = GetDeviceFromIp(address);
                    if (device.Blocked)
                        AddToBlockList(address);
                }
                return true;
            }
            catch (Exception ex)
            {
                DebugWriter.WriteDebug(DebugLevel.E, "Failed to populate block list: {0}", vars: [ex.Message]);
                DebugWriter.WriteDebugStackTrace(ex);
            }
            return false;
        }

        /// <summary>
        /// Adds new device IP address to the list and saves it
        /// </summary>
        /// <param name="address">Device IP address from remote endpoint address</param>
        /// <param name="throwException">Optionally throw exception</param>
        /// <returns>An instance of <see cref="RemoteDebugDeviceInfo"/></returns>
        internal static RemoteDebugDeviceInfo AddDevice(string address, bool throwException = true)
        {
            var addresses = ListDeviceAddresses();
            if (!addresses.Contains(address))
            {
                var deviceInfo = new RemoteDebugDeviceInfo()
                {
                    address = address,
                    blocked = false,
                };
                remoteDebugDevices.Add(deviceInfo);
                SaveAllDevices();

                // To be able to be modified in the list
                return remoteDebugDevices[^1];
            }
            else if (throwException)
                throw new KernelException(KernelExceptionType.RemoteDebugDeviceAlreadyExists, LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_REMOTEDEBUG_EXCEPTION_DEVICEALREADYEXISTS"));
            return GetDeviceFromIp(address);
        }

        /// <summary>
        /// Removes a device IP address from the list
        /// </summary>
        /// <param name="address">Device IP address from remote endpoint address</param>
        public static void RemoveDevice(string address)
        {
            var addresses = ListDeviceAddresses();
            if (addresses.Contains(address))
            {
                var device = GetDeviceFromIp(address);
                remoteDebugDevices.Remove(device);
                SaveAllDevices();
            }
            else
            {
                throw new KernelException(KernelExceptionType.RemoteDebugDeviceNotFound, LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_REMOTEDEBUG_EXCEPTION_DEVICENOTFOUND"));
            }
        }

        /// <summary>
        /// Removes a device IP address from the list
        /// </summary>
        /// <param name="address">Device IP address from remote endpoint address</param>
        /// <returns>True if successful; False if unsuccessful.</returns>
        public static bool TryRemoveDevice(string address)
        {
            try
            {
                RemoveDevice(address);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Lists all device addresses and puts them into an array
        /// </summary>
        public static string[] ListDeviceAddresses() =>
            remoteDebugDevices.Select((info) => info.Address).ToArray();

        /// <summary>
        /// Lists all device names and puts them into an array
        /// </summary>
        public static string[] ListDeviceNames() =>
            remoteDebugDevices.Select((info) => info.Name).ToArray();

        /// <summary>
        /// Lists all devices and puts them into an array
        /// </summary>
        public static RemoteDebugDeviceInfo[] ListDevices() =>
            [.. remoteDebugDevices];

        /// <summary>
        /// Gets a device by IP address
        /// </summary>
        /// <param name="address">Device that holds the IP address</param>
        /// <returns>An instance of <see cref="RemoteDebugDeviceInfo"/> if found; throws otherwise.</returns>
        public static RemoteDebugDeviceInfo GetDeviceFromIp(string address) =>
            remoteDebugDevices.Single((rddi) => rddi.Address == address);

        /// <summary>
        /// Gets a device by name
        /// </summary>
        /// <param name="name">Device that holds the name</param>
        /// <returns>An instance of <see cref="RemoteDebugDeviceInfo"/> if found; throws otherwise.</returns>
        public static RemoteDebugDeviceInfo GetDeviceFromName(string name) =>
            remoteDebugDevices.Single((rddi) => rddi.Name == name);

        /// <summary>
        /// Disconnects debug device depending on exception
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="deviceIndex">Device index</param>
        internal static void DisconnectDependingOnException(Exception exception, int deviceIndex)
        {
            var device = RemoteDebugger.DebugDevices[deviceIndex];
            DisconnectDependingOnException(exception, device);
        }

        /// <summary>
        /// Disconnects debug device depending on exception
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="device">Device to contact</param>
        internal static void DisconnectDependingOnException(Exception exception, RemoteDebugDevice device)
        {
            SocketException? SE = (SocketException?)exception.InnerException;
            if (SE is not null)
            {
                if (SE.SocketErrorCode == SocketError.TimedOut ||
                    SE.SocketErrorCode == SocketError.WouldBlock ||
                    SE.SocketErrorCode == SocketError.ConnectionAborted ||
                    SE.SocketErrorCode == SocketError.Shutdown)
                    // A device was disconnected
                    DisconnectDevice(device.ClientIP);
                else
                    // Other error with the device occurred
                    DebugWriter.WriteDebugStackTrace(exception);
            }
            else
            {
                DisconnectDevice(device.ClientIP);
                DebugWriter.WriteDebugStackTrace(exception);
            }
        }

        internal static void SaveAllDevices() =>
            FilesystemTools.WriteContentsText(PathsManagement.GetKernelPath(KernelPathType.DebugDevices), JsonConvert.SerializeObject(remoteDebugDevices, Formatting.Indented));

        internal static void LoadAllDevices()
        {
            string devicesPath = PathsManagement.GetKernelPath(KernelPathType.DebugDevices);
            if (FilesystemTools.FileExists(devicesPath))
                remoteDebugDevices = JsonConvert.DeserializeObject<List<RemoteDebugDeviceInfo>>(FilesystemTools.ReadContentsText(devicesPath)) ??
                    throw new KernelException(KernelExceptionType.RemoteDebugDeviceOperation, LanguageTools.GetLocalized("NKS_KERNEL_DEBUGGING_REMOTEDEBUG_EXCEPTION_LOADDEVICESFAILED") + $" {devicesPath}");
        }
    }
}


'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports KS.Files.Operations
Imports System.IO
Imports Newtonsoft.Json.Linq

Namespace Network.RemoteDebug
    Public Module RemoteDebugTools

        ''' <summary>
        ''' Device property enumeration
        ''' </summary>
        Public Enum DeviceProperty
            ''' <summary>
            ''' Device name
            ''' </summary>
            Name
            ''' <summary>
            ''' Is the device blocked?
            ''' </summary>
            Blocked
            ''' <summary>
            ''' Device chat history
            ''' </summary>
            ChatHistory
        End Enum

        ''' <summary>
        ''' Disconnects a specified debug device
        ''' </summary>
        ''' <param name="IPAddr">An IP address of the connected debug device</param>
        Public Sub DisconnectDbgDev(IPAddr As String)
            Dim Found As Boolean
            For i As Integer = 0 To DebugDevices.Count - 1
                If Found Then
                    Exit Sub
                Else
                    If IPAddr = DebugDevices(i).ClientIP Then
                        Wdbg(DebugLevel.I, "Debug device {0} disconnected.", DebugDevices(i).ClientIP)
                        Found = True
                        DebugDevices(i).ClientSocket.Disconnect(True)
                        DebugDevices.RemoveAt(i)
                        KernelEventManager.RaiseRemoteDebugConnectionDisconnected(IPAddr)
                    End If
                End If
            Next
            If Not Found Then
                Throw New Exceptions.RemoteDebugDeviceNotFoundException(DoTranslation("Debug device {0} not found."), IPAddr)
            End If
        End Sub

        ''' <summary>
        ''' Adds device to block list
        ''' </summary>
        ''' <param name="IP">An IP address for device</param>
        Public Sub AddToBlockList(IP As String)
            Dim BlockedDevices() As String = ListDevices()
            Wdbg(DebugLevel.I, "Devices count: {0}", BlockedDevices.Length)
            If BlockedDevices.Contains(IP) And Not GetDeviceProperty(IP, DeviceProperty.Blocked) Then
                Wdbg(DebugLevel.I, "Device {0} will be blocked...", IP)
                DisconnectDbgDev(IP)
                SetDeviceProperty(IP, DeviceProperty.Blocked, True)
                RDebugBlocked.Add(IP)
            ElseIf BlockedDevices.Contains(IP) And GetDeviceProperty(IP, DeviceProperty.Blocked) Then
                Wdbg(DebugLevel.W, "Trying to add an already-blocked device {0}. Adding to list...", IP)
                If Not RDebugBlocked.Contains(IP) Then
                    DisconnectDbgDev(IP)
                    RDebugBlocked.Add(IP)
                Else
                    Wdbg(DebugLevel.W, "Trying to add an already-blocked device {0}.", IP)
                    Throw New Exceptions.RemoteDebugDeviceAlreadyExistsException(DoTranslation("Device already exists in the block list."))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Adds device to block list
        ''' </summary>
        ''' <param name="IP">An IP address for device</param>
        ''' <returns>True if successful; False if unsuccessful.</returns>
        Public Function TryAddToBlockList(IP As String) As Boolean
            Try
                AddToBlockList(IP)
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to add device to block list: {0}", ex.Message)
                WStkTrc(ex)
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Removes device from block list
        ''' </summary>
        ''' <param name="IP">A blocked IP address for device</param>
        Public Sub RemoveFromBlockList(IP As String)
            Dim BlockedDevices() As String = ListDevices()
            Wdbg(DebugLevel.I, "Devices count: {0}", BlockedDevices.Length)
            If BlockedDevices.Contains(IP) Then
                Wdbg(DebugLevel.I, "Device {0} found.", IP)
                RDebugBlocked.Remove(IP)
                SetDeviceProperty(IP, DeviceProperty.Blocked, False)
            Else
                Wdbg(DebugLevel.W, "Trying to remove an already-unblocked device {0}. Removing from list...", IP)
                If Not RDebugBlocked.Remove(IP) Then Throw New Exceptions.RemoteDebugDeviceOperationException(DoTranslation("Device doesn't exist in the block list."))
            End If
        End Sub

        ''' <summary>
        ''' Removes device from block list
        ''' </summary>
        ''' <param name="IP">A blocked IP address for device</param>
        ''' <returns>True if successful; False if unsuccessful.</returns>
        Public Function TryRemoveFromBlockList(IP As String) As Boolean
            Try
                RemoveFromBlockList(IP)
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to remove device from block list: {0}", ex.Message)
                WStkTrc(ex)
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Populates blocked devices
        ''' </summary>
        ''' <returns>True if successful; False if unsuccessful.</returns>
        Function PopulateBlockedDevices() As Boolean
            Try
                Dim BlockEntries() As String = ListDevices()
                Wdbg(DebugLevel.I, "Devices count: {0}", BlockEntries.Length)
                For Each BlockEntry As String In BlockEntries
                    If GetDeviceProperty(BlockEntry, DeviceProperty.Blocked) Then AddToBlockList(BlockEntry)
                Next
                Return True
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to populate block list: {0}", ex.Message)
                WStkTrc(ex)
            End Try
            Return False
        End Function

        ''' <summary>
        ''' Gets device property from device IP address
        ''' </summary>
        ''' <param name="DeviceIP">Device IP address from remote endpoint address</param>
        ''' <param name="DeviceProperty">Device property</param>
        ''' <returns>Device property if successful; nothing if unsuccessful.</returns>
        Public Function GetDeviceProperty(DeviceIP As String, DeviceProperty As DeviceProperty) As Object
            Dim DeviceJsonContent As String = File.ReadAllText(GetKernelPath(KernelPathType.DebugDevNames))
            Dim DeviceNameToken As JObject = JObject.Parse(If(Not String.IsNullOrEmpty(DeviceJsonContent), DeviceJsonContent, "{}"))
            Dim DeviceProperties As JObject = TryCast(DeviceNameToken(DeviceIP), JObject)
            If DeviceProperties IsNot Nothing Then
                Select Case DeviceProperty
                    Case DeviceProperty.Name
                        Return DeviceProperties.Property("Name").Value.ToString
                    Case DeviceProperty.Blocked
                        Return DeviceProperties.Property("Blocked").Value
                    Case DeviceProperty.ChatHistory
                        Return DeviceProperties.Property("ChatHistory").Value.ToArray
                End Select
            Else
                Throw New Exceptions.RemoteDebugDeviceNotFoundException(DoTranslation("No such device."))
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Sets device property from device IP address
        ''' </summary>
        ''' <param name="DeviceIP">Device IP address from remote endpoint address</param>
        ''' <param name="DeviceProperty">Device property</param>
        ''' <param name="Value">Value</param>
        Public Sub SetDeviceProperty(DeviceIP As String, DeviceProperty As DeviceProperty, Value As Object)
            Dim DeviceJsonContent As String = File.ReadAllText(GetKernelPath(KernelPathType.DebugDevNames))
            Dim DeviceNameToken As JObject = JObject.Parse(If(Not String.IsNullOrEmpty(DeviceJsonContent), DeviceJsonContent, "{}"))
            Dim DeviceProperties As JObject = TryCast(DeviceNameToken(DeviceIP), JObject)
            If DeviceProperties IsNot Nothing Then
                Select Case DeviceProperty
                    Case DeviceProperty.Name
                        DeviceProperties("Name") = JToken.FromObject(Value)
                    Case DeviceProperty.Blocked
                        DeviceProperties("Blocked") = JToken.FromObject(Value)
                    Case DeviceProperty.ChatHistory
                        Dim ChatHistory As JArray = TryCast(DeviceProperties("ChatHistory"), JArray)
                        ChatHistory.Add(Value)
                End Select
                File.WriteAllText(GetKernelPath(KernelPathType.DebugDevNames), JsonConvert.SerializeObject(DeviceNameToken, Formatting.Indented))
            Else
                Throw New Exceptions.RemoteDebugDeviceNotFoundException(DoTranslation("No such device."))
            End If
        End Sub

        ''' <summary>
        ''' Sets device property from device IP address
        ''' </summary>
        ''' <param name="DeviceIP">Device IP address from remote endpoint address</param>
        ''' <param name="DeviceProperty">Device property</param>
        ''' <param name="Value">Value</param>
        ''' <returns>True if successful; False if unsuccessful.</returns>
        Public Function TrySetDeviceProperty(DeviceIP As String, DeviceProperty As DeviceProperty, Value As Object) As Boolean
            Try
                SetDeviceProperty(DeviceIP, DeviceProperty, Value)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Adds new device IP address to JSON
        ''' </summary>
        ''' <param name="DeviceIP">Device IP address from remote endpoint address</param>
        ''' <param name="ThrowException">Optionally throw exception</param>
        Public Sub AddDeviceToJson(DeviceIP As String, Optional ThrowException As Boolean = True)
            MakeFile(GetKernelPath(KernelPathType.DebugDevNames), False)
            Dim DeviceJsonContent As String = File.ReadAllText(GetKernelPath(KernelPathType.DebugDevNames))
            Dim DeviceNameToken As JObject = JObject.Parse(If(Not String.IsNullOrEmpty(DeviceJsonContent), DeviceJsonContent, "{}"))
            If DeviceNameToken(DeviceIP) Is Nothing Then
                Dim NewDevice As New JObject(New JProperty("Name", ""),
                                             New JProperty("Blocked", False),
                                             New JProperty("ChatHistory", New JArray()))
                DeviceNameToken.Add(DeviceIP, NewDevice)
                File.WriteAllText(GetKernelPath(KernelPathType.DebugDevNames), JsonConvert.SerializeObject(DeviceNameToken, Formatting.Indented))
            Else
                If ThrowException Then Throw New Exceptions.RemoteDebugDeviceAlreadyExistsException(DoTranslation("Device already exists."))
            End If
        End Sub

        ''' <summary>
        ''' Adds new device IP address to JSON
        ''' </summary>
        ''' <param name="DeviceIP">Device IP address from remote endpoint address</param>
        ''' <param name="ThrowException">Optionally throw exception</param>
        ''' <returns>True if successful; False if unsuccessful.</returns>
        Public Function TryAddDeviceToJson(DeviceIP As String, Optional ThrowException As Boolean = True) As Boolean
            Try
                AddDeviceToJson(DeviceIP, ThrowException)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Removes a device IP address from JSON
        ''' </summary>
        ''' <param name="DeviceIP">Device IP address from remote endpoint address</param>
        Public Sub RemoveDeviceFromJson(DeviceIP As String)
            Dim DeviceJsonContent As String = File.ReadAllText(GetKernelPath(KernelPathType.DebugDevNames))
            Dim DeviceNameToken As JObject = JObject.Parse(If(Not String.IsNullOrEmpty(DeviceJsonContent), DeviceJsonContent, "{}"))
            If DeviceNameToken(DeviceIP) IsNot Nothing Then
                DeviceNameToken.Remove(DeviceIP)
                File.WriteAllText(GetKernelPath(KernelPathType.DebugDevNames), JsonConvert.SerializeObject(DeviceNameToken, Formatting.Indented))
            Else
                Throw New Exceptions.RemoteDebugDeviceNotFoundException(DoTranslation("No such device."))
            End If
        End Sub

        ''' <summary>
        ''' Removes a device IP address from JSON
        ''' </summary>
        ''' <param name="DeviceIP">Device IP address from remote endpoint address</param>
        ''' <returns>True if successful; False if unsuccessful.</returns>
        Public Function TryRemoveDeviceFromJson(DeviceIP As String) As Boolean
            Try
                RemoveDeviceFromJson(DeviceIP)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Lists all devices and puts them into an array
        ''' </summary>
        Public Function ListDevices() As String()
            MakeFile(GetKernelPath(KernelPathType.DebugDevNames), False)
            Dim DeviceJsonContent As String = File.ReadAllText(GetKernelPath(KernelPathType.DebugDevNames))
            Dim DeviceNameToken As JObject = JObject.Parse(If(Not String.IsNullOrEmpty(DeviceJsonContent), DeviceJsonContent, "{}"))
            Return DeviceNameToken.Properties.Select(Function(p) p.Name).ToArray
        End Function

    End Module
End Namespace

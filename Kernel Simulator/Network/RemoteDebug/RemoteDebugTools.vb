
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Imports System.IO
Imports Newtonsoft.Json.Linq

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
        ''' Device chat history
        ''' </summary>
        ChatHistory
    End Enum

    ''' <summary>
    ''' Disconnects a specified debug device
    ''' </summary>
    ''' <param name="IPAddr">An IP address of the connected debug device</param>
    Public Sub DisconnectDbgDev(ByVal IPAddr As String)
        Dim Found As Boolean
        For i As Integer = 0 To DebugDevices.Count - 1
            If Found Then
                Exit Sub
            Else
                If IPAddr = DebugDevices.Values(i) Then
                    Wdbg("I", "Debug device {0} disconnected.", DebugDevices.Values(i))
                    Found = True
                    DebugDevices.Keys(i).Disconnect(True)
                    dbgConns.Remove(dbgConns.Keys(i))
                    DebugDevices.Remove(DebugDevices.Keys(i))
                    EventManager.RaiseRemoteDebugConnectionDisconnected(IPAddr)
                End If
            End If
        Next
        If Not Found Then
            Throw New EventsAndExceptions.RemoteDebugDeviceNotFoundException(DoTranslation("Debug device {0} not found.", currentLang).FormatString(IPAddr))
        End If
    End Sub

    ''' <summary>
    ''' Adds device to block list
    ''' </summary>
    ''' <param name="IP">An IP address for device</param>
    ''' <returns>True if successful; False if unsuccessful.</returns>
    Public Function AddToBlockList(ByVal IP As String) As Boolean
        Try
            Dim BlockedDevices As List(Of String) = IO.File.ReadAllLines(paths("BlockedDevices")).ToList
            Wdbg("I", "Blocked devices count: {0}", BlockedDevices.Count)
            If Not BlockedDevices.Contains(IP) Then
                Wdbg("I", "Device {0} will be blocked...", IP)
                DisconnectDbgDev(IP)
                BlockedDevices.Add(IP)
                RDebugBlocked.Add(IP)
                IO.File.WriteAllLines(paths("BlockedDevices"), BlockedDevices)
                Return True
            Else
                Wdbg("W", "Trying to add an already-blocked device {0}. Adding to list...", IP)
                If Not RDebugBlocked.Contains(IP) Then
                    DisconnectDbgDev(IP)
                    RDebugBlocked.Add(IP)
                    Return True
                Else
                    Wdbg("W", "Trying to add an already-blocked device {0}.", IP)
                    Return False
                End If
            End If
        Catch ex As Exception
            Wdbg("E", "Failed to add device to block list: {0}", ex.Message)
            WStkTrc(ex)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Removes device from block list
    ''' </summary>
    ''' <param name="IP">A blocked IP address for device</param>
    ''' <returns>True if successful; False if unsuccessful.</returns>
    Public Function RemoveFromBlockList(ByVal IP As String) As Boolean
        Try
            Dim BlockedDevices As List(Of String) = IO.File.ReadAllLines(paths("BlockedDevices")).ToList
            Wdbg("I", "Blocked devices count: {0}", BlockedDevices.Count)
            If BlockedDevices.Contains(IP) Then
                Wdbg("I", "Device {0} found.", IP)
                For BlockedDeviceNum As Integer = 0 To BlockedDevices.Count - 1
                    Dim BlockedDevice As String = BlockedDevices(BlockedDeviceNum)
                    If BlockedDevice.StartsWith(IP) Then
                        Wdbg("I", "Removing device {0} from block list...", IP)
                        BlockedDevices.Remove(BlockedDevice)
                        RDebugBlocked.Remove(BlockedDevice)
                    End If
                Next
                IO.File.WriteAllLines(paths("BlockedDevices"), BlockedDevices)
                Return True
            Else
                Wdbg("W", "Trying to remove an already-unblocked device {0}. Removing from list...", IP)
                Return RDebugBlocked.Remove(IP)
            End If
            Wdbg("E", "Device {0} not found.", IP)
            Return False
        Catch ex As Exception
            Wdbg("E", "Failed to remove device from block list: {0}", ex.Message)
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
            If Not IO.File.Exists(paths("BlockedDevices")) Then MakeFile(paths("BlockedDevices"))
            Dim BlockEntries() As String = IO.File.ReadAllLines(paths("BlockedDevices"))
            Wdbg("I", "Blocked devices count: {0}", BlockEntries.Count)
            For Each BlockEntry As String In BlockEntries
                Dim BlockEntrySplit() As String = BlockEntry.Split(",")
                AddToBlockList(BlockEntrySplit(0))
            Next
            Return True
        Catch ex As Exception
            Wdbg("E", "Failed to populate block list: {0}", ex.Message)
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
    Public Function GetDeviceProperty(ByVal DeviceIP As String, ByVal DeviceProperty As DeviceProperty) As Object
        Dim DeviceNameToken As JObject = JObject.Parse(File.ReadAllText(paths("DebugDevNames")))
        Dim DeviceProperties As JObject = TryCast(DeviceNameToken(DeviceIP), JObject)
        If DeviceProperties IsNot Nothing Then
            Select Case DeviceProperty
                Case DeviceProperty.Name
                    Return DeviceProperties.Property("Name").Value.ToString
                Case DeviceProperty.ChatHistory
                    Return DeviceProperties.Property("ChatHistory").Value.ToArray
            End Select
        Else
            Throw New EventsAndExceptions.RemoteDebugDeviceNotFoundException(DoTranslation("No such device."))
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Sets device property from device IP address
    ''' </summary>
    ''' <param name="DeviceIP">Device IP address from remote endpoint address</param>
    ''' <param name="DeviceProperty">Device property</param>
    ''' <param name="Value">Value</param>
    ''' <returns>True if successful; False if unsuccessful.</returns>
    Public Function SetDeviceProperty(ByVal DeviceIP As String, ByVal DeviceProperty As DeviceProperty, ByVal Value As Object) As Boolean
        Dim DeviceNameToken As JObject = JObject.Parse(File.ReadAllText(paths("DebugDevNames")))
        Dim DeviceProperties As JObject = TryCast(DeviceNameToken(DeviceIP), JObject)
        If DeviceProperties IsNot Nothing Then
            Select Case DeviceProperty
                Case DeviceProperty.Name
                    DeviceProperties("Name") = JToken.FromObject(Value)
                Case DeviceProperty.ChatHistory
                    Dim ChatHistory As JArray = TryCast(DeviceProperties("ChatHistory"), JArray)
                    ChatHistory.Add(Value)
            End Select
            File.WriteAllText(paths("DebugDevNames"), JsonConvert.SerializeObject(DeviceNameToken, Formatting.Indented))
            Return True
        Else
            Throw New EventsAndExceptions.RemoteDebugDeviceNotFoundException(DoTranslation("No such device."))
            Return False
        End If
    End Function

    ''' <summary>
    ''' Adds new device IP address to JSON
    ''' </summary>
    ''' <param name="DeviceIP">Device IP address from remote endpoint address</param>
    ''' <param name="ThrowException">Optionally throw exception</param>
    ''' <returns>True if successful; False if unsuccessful.</returns>
    Public Function AddDeviceToJson(ByVal DeviceIP As String, Optional ThrowException As Boolean = True) As Boolean
        If Not File.Exists(paths("DebugDevNames")) Then MakeFile(paths("DebugDevNames"))
        Dim DeviceJsonContent As String = File.ReadAllText(paths("DebugDevNames"))
        Dim DeviceNameToken As JObject = JObject.Parse(If(Not String.IsNullOrEmpty(DeviceJsonContent), DeviceJsonContent, "{}"))
        If DeviceNameToken(DeviceIP) Is Nothing Then
            Dim NewDevice As New JObject(New JProperty("Name", ""),
                                         New JProperty("ChatHistory", New JArray()))
            DeviceNameToken.Add(DeviceIP, NewDevice)
            File.WriteAllText(paths("DebugDevNames"), JsonConvert.SerializeObject(DeviceNameToken, Formatting.Indented))
            Return True
        Else
            If ThrowException Then Throw New EventsAndExceptions.RemoteDebugDeviceAlreadyExistsException(DoTranslation("Device already exists."))
            Return False
        End If
    End Function

End Module

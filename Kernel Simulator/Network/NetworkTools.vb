
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
Imports System.Net.NetworkInformation
Imports System.Text
Imports Newtonsoft.Json.Linq

Public Module NetworkTools

    'Variables
    Public DRetries As Integer = 3
    Public URetries As Integer = 3
    Friend DFinish As Boolean
    Friend UFinish As Boolean

    Public Enum SpeedDialType
        ''' <summary>
        ''' FTP speed dial
        ''' </summary>
        FTP
        ''' <summary>
        ''' SFTP speed dial
        ''' </summary>
        SFTP
    End Enum

    ''' <summary>
    ''' Checks to see if the network is available
    ''' </summary>
    Public ReadOnly Property NetworkAvailable As Boolean
        Get
            Return NetworkInterface.GetIsNetworkAvailable()
        End Get
    End Property

    ''' <summary>
    ''' Print each of adapters' properties to the console.
    ''' </summary>
    Public Sub PrintAdapterProperties()
        Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces
        Dim NoV4, NoV6, Failed As Boolean
        Dim gp As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties
        Dim gs6 As IPGlobalStatistics = gp.GetIPv6GlobalStatistics
        Dim gs4 As IPGlobalStatistics = gp.GetIPv4GlobalStatistics
        Dim adapterNumber As Long = 0

        'Probe for adapter capabilities
        For Each adapter As NetworkInterface In adapters
            adapterNumber += 1
            W("==========================================", True, ColTypes.Neutral)

            'See if it supports IPv6
            If Not adapter.Supports(NetworkInterfaceComponent.IPv6) Then
                Wdbg(DebugLevel.W, "{0} doesn't support IPv6. Trying to get information about IPv4.", adapter.Description)
                W(DoTranslation("Adapter {0} doesn't support IPv6. Continuing..."), True, ColTypes.Error, adapter.Description)
                NoV6 = True
            End If

            'See if it supports IPv4
            If Not adapter.Supports(NetworkInterfaceComponent.IPv4) Then
                Wdbg(DebugLevel.E, "{0} doesn't support IPv4.", adapter.Description)
                W(DoTranslation("Adapter {0} doesn't support IPv4. Probe failed."), True, ColTypes.Error, adapter.Description)
                NoV4 = True
            End If

            'Get adapter IPv(4/6) properties
            If IsInternetAdapter(adapter) And Not NoV4 Then
                Wdbg(DebugLevel.I, "Adapter type of {0}: {1}", adapter.Description, adapter.NetworkInterfaceType.ToString)
                Dim adapterProperties As IPInterfaceProperties = adapter.GetIPProperties()
                Dim p As IPv4InterfaceProperties
                Dim s As IPv4InterfaceStatistics = adapter.GetIPv4Statistics
                Dim p6 As IPv6InterfaceProperties
                'TODO: IPV6InterfaceStatistics is not implemented yet
                Try
                    p = adapterProperties.GetIPv4Properties
                    p6 = adapterProperties.GetIPv6Properties
                Catch ex As NetworkInformationException
#Disable Warning BC42104
                    If p6 Is Nothing Then
                        Wdbg(DebugLevel.W, "Failed to get IPv6 properties.")
                        W(DoTranslation("Failed to get IPv6 properties for adapter {0}. Continuing..."), True, ColTypes.Error, adapter.Description)
                    End If
                    If p Is Nothing Then
                        Wdbg(DebugLevel.E, "Failed to get IPv4 properties.")
                        W(DoTranslation("Failed to get properties for adapter {0}"), True, ColTypes.Error, adapter.Description)
                        Failed = True
                    End If
                    WStkTrc(ex)
#Enable Warning BC42104
                End Try

                'Check if statistics is nothing
                If s Is Nothing Then
                    Wdbg(DebugLevel.E, "Failed to get statistics.")
                    W(DoTranslation("Failed to get statistics for adapter {0}"), True, ColTypes.Error, adapter.Description)
                    Failed = True
                End If

                'Print adapter infos if not failed
                If Not Failed Then
                    PrintAdapterIPv4Info(adapter, p, s, adapterNumber)
                    'Additionally, print adapter IPv6 infos if available
                    If Not NoV6 Then
                        PrintAdapterIPv6Info(adapter, p6, adapterNumber)
                    End If
                End If
            Else
                Wdbg(DebugLevel.W, "Adapter {0} doesn't belong in netinfo because the type is {1}", adapter.Description, adapter.NetworkInterfaceType)
            End If
        Next

        'Print general IPv4 and IPv6 information
        W("==========================================", True, ColTypes.Neutral)
        PrintGeneralNetInfo(gs4, gs6)
    End Sub

    ''' <summary>
    ''' Prints IPv4 info for adapter
    ''' </summary>
    ''' <param name="NInterface">A network interface or adapter</param>
    ''' <param name="Properties">Network properties</param>
    ''' <param name="Statistics">Network statistics</param>
    ''' <param name="AdapterNumber">Reference adapter number</param>
    Sub PrintAdapterIPv4Info(NInterface As NetworkInterface, Properties As IPv4InterfaceProperties, Statistics As IPv4InterfaceStatistics, AdapterNumber As Long)
        W(DoTranslation("IPv4 information:") + vbNewLine +
          DoTranslation("Adapter Number:") + " {0}" + vbNewLine +
          DoTranslation("Adapter Name:") + " {1}" + vbNewLine +
          DoTranslation("Maximum Transmission Unit: {2} Units") + vbNewLine +
          DoTranslation("DHCP Enabled:") + " {3}" + vbNewLine +
          DoTranslation("Non-unicast packets:") + " {4}/{5}" + vbNewLine +
          DoTranslation("Unicast packets:") + " {6}/{7}" + vbNewLine +
          DoTranslation("Error incoming/outgoing packets:") + " {8}/{9}", True, ColTypes.Neutral,
          AdapterNumber, NInterface.Description, Properties.Mtu, Properties.IsDhcpEnabled, Statistics.NonUnicastPacketsSent, Statistics.NonUnicastPacketsReceived,
          Statistics.UnicastPacketsSent, Statistics.UnicastPacketsReceived, Statistics.IncomingPacketsWithErrors, Statistics.OutgoingPacketsWithErrors)
    End Sub

    ''' <summary>
    ''' Prints IPv6 info for adapter
    ''' </summary>
    ''' <param name="NInterface">A network interface or adapter</param>
    ''' <param name="Properties">Network properties</param>
    ''' <param name="AdapterNumber">Reference adapter number</param>
    Sub PrintAdapterIPv6Info(NInterface As NetworkInterface, Properties As IPv6InterfaceProperties, AdapterNumber As Long)
        W(DoTranslation("IPv6 information:") + vbNewLine +
          DoTranslation("Adapter Number:") + " {0}" + vbNewLine +
          DoTranslation("Adapter Name:") + " {1}" + vbNewLine +
          DoTranslation("Maximum Transmission Unit: {2} Units"), True, ColTypes.Neutral,
          AdapterNumber, NInterface.Description, Properties.Mtu)
    End Sub

    ''' <summary>
    ''' Prints general network info
    ''' </summary>
    ''' <param name="IPv4Stat">IPv4 general statistics</param>
    ''' <param name="IPv6Stat">IPv6 general statistics</param>
    Sub PrintGeneralNetInfo(IPv4Stat As IPGlobalStatistics, IPv6Stat As IPGlobalStatistics)
        W(DoTranslation("General IPv6 properties") + vbNewLine +
          DoTranslation("Packets (inbound):") + " {0}/{1}" + vbNewLine +
          DoTranslation("Packets (outbound):") + " {2}/{3}" + vbNewLine +
          DoTranslation("Errors in received packets:") + " {4}/{5}/{6}" + vbNewLine +
          DoTranslation("General IPv4 properties") + vbNewLine +
          DoTranslation("Packets (inbound):") + " {7}/{8}" + vbNewLine +
          DoTranslation("Packets (outbound):") + " {9}/{10}" + vbNewLine +
          DoTranslation("Errors in received packets:") + " {11}/{12}/{13}", True, ColTypes.Neutral,
          IPv6Stat.ReceivedPackets, IPv6Stat.ReceivedPacketsDelivered, IPv6Stat.OutputPacketRequests, IPv6Stat.OutputPacketsDiscarded, IPv6Stat.ReceivedPacketsWithAddressErrors,
          IPv6Stat.ReceivedPacketsWithHeadersErrors, IPv6Stat.ReceivedPacketsWithUnknownProtocol, IPv4Stat.ReceivedPackets, IPv4Stat.ReceivedPacketsDelivered, IPv4Stat.OutputPacketRequests,
          IPv4Stat.OutputPacketsDiscarded, IPv4Stat.ReceivedPacketsWithAddressErrors, IPv4Stat.ReceivedPacketsWithHeadersErrors, IPv4Stat.ReceivedPacketsWithUnknownProtocol)
    End Sub

    Function IsInternetAdapter(InternetAdapter As NetworkInterface) As Boolean
        Return InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.Ethernet Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.Ethernet3Megabit Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.FastEthernetFx Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.FastEthernetT Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.GigabitEthernet Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.Wireless80211 Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.Tunnel
    End Function

    ''' <summary>
    ''' Pings an address
    ''' </summary>
    ''' <param name="Address">Target address</param>
    ''' <returns>A ping reply status</returns>
    Public Function PingAddress(Address As String) As PingReply
        Dim Pinger As New Ping
        Dim PingerOpts As New PingOptions With {.DontFragment = True}
        Dim PingBuffer() As Byte = Encoding.ASCII.GetBytes("Kernel Simulator")
        Dim PingTimeout As Integer = 60000 '60 seconds = 1 minute. timeout of Pinger.Send() takes milliseconds.
        Return Pinger.Send(Address, PingTimeout, PingBuffer, PingerOpts)
    End Function

    ''' <summary>
    ''' Changes host name
    ''' </summary>
    ''' <param name="NewHost">New host name</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function ChangeHostname(NewHost As String) As Boolean
        Try
            HName = NewHost
            Dim Token As JToken = GetConfigCategory(ConfigCategory.Login)
            SetConfigValueAndWrite(ConfigCategory.Login, Token, "Host Name", HName)
            Return True
        Catch ex As Exception
            WStkTrc(ex)
            Wdbg(DebugLevel.E, "Failed to change hostname: {0}", ex.Message)
            Throw New Exceptions.HostnameException(DoTranslation("Failed to change host name: {0}"), ex, ex.Message)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Adds an entry to speed dial
    ''' </summary>
    ''' <param name="Address">A speed dial address</param>
    ''' <param name="Port">A speed dial port</param>
    ''' <param name="User">A speed dial username</param>
    ''' <param name="EncryptionMode">A speed dial encryption mode</param>
    ''' <param name="SpeedDialType">Speed dial type</param>
    ''' <param name="ThrowException">Optionally throw exception</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function AddEntryToSpeedDial(Address As String, Port As Integer, User As String, SpeedDialType As SpeedDialType, Optional EncryptionMode As FtpEncryptionMode = FtpEncryptionMode.None, Optional ThrowException As Boolean = True) As Boolean
        Dim PathName As String = If(SpeedDialType = SpeedDialType.SFTP, "SFTPSpeedDial", "FTPSpeedDial")
        Dim SpeedDialEnum As KernelPathType = [Enum].Parse(GetType(KernelPathType), PathName)
        If Not File.Exists(GetKernelPath(SpeedDialEnum)) Then MakeFile(GetKernelPath(SpeedDialEnum))
        Dim SpeedDialJsonContent As String = File.ReadAllText(GetKernelPath(SpeedDialEnum))
        If SpeedDialJsonContent.StartsWith("[") Then
            ConvertSpeedDialEntries(SpeedDialType)
            SpeedDialJsonContent = File.ReadAllText(GetKernelPath(SpeedDialEnum))
        End If
        Dim SpeedDialToken As JObject = JObject.Parse(If(Not String.IsNullOrEmpty(SpeedDialJsonContent), SpeedDialJsonContent, "{}"))
        If SpeedDialToken(Address) Is Nothing Then
            Dim NewSpeedDial As New JObject(New JProperty("Address", Address),
                                            New JProperty("Port", Port),
                                            New JProperty("User", User),
                                            New JProperty("Type", SpeedDialType),
                                            New JProperty("FTP Encryption Mode", EncryptionMode))
            SpeedDialToken.Add(Address, NewSpeedDial)
            File.WriteAllText(GetKernelPath(SpeedDialEnum), JsonConvert.SerializeObject(SpeedDialToken, Formatting.Indented))
            Return True
        Else
            If ThrowException Then
                If SpeedDialType = SpeedDialType.FTP Then
                    Throw New Exceptions.FTPNetworkException(DoTranslation("Entry already exists."))
                ElseIf SpeedDialType = SpeedDialType.SFTP Then
                    Throw New Exceptions.SFTPNetworkException(DoTranslation("Entry already exists."))
                End If
            End If
            Return False
        End If
    End Function

    ''' <summary>
    ''' Lists all speed dial entries
    ''' </summary>
    ''' <param name="SpeedDialType">Speed dial type</param>
    ''' <returns>A list</returns>
    Public Function ListSpeedDialEntries(SpeedDialType As SpeedDialType) As Dictionary(Of String, JToken)
        Dim PathName As String = If(SpeedDialType = SpeedDialType.SFTP, "SFTPSpeedDial", "FTPSpeedDial")
        Dim SpeedDialEnum As KernelPathType = [Enum].Parse(GetType(KernelPathType), PathName)
        If Not File.Exists(GetKernelPath(SpeedDialEnum)) Then MakeFile(GetKernelPath(SpeedDialEnum))
        Dim SpeedDialJsonContent As String = File.ReadAllText(GetKernelPath(SpeedDialEnum))
        If SpeedDialJsonContent.StartsWith("[") Then
            ConvertSpeedDialEntries(SpeedDialType)
            SpeedDialJsonContent = File.ReadAllText(GetKernelPath(SpeedDialEnum))
        End If
        Dim SpeedDialToken As JObject = JObject.Parse(If(Not String.IsNullOrEmpty(SpeedDialJsonContent), SpeedDialJsonContent, "{}"))
        Dim SpeedDialEntries As New Dictionary(Of String, JToken)
        For Each SpeedDialAddress In SpeedDialToken.Properties
            SpeedDialEntries.Add(SpeedDialAddress.Name, SpeedDialAddress.Value)
        Next
        Return SpeedDialEntries
    End Function

    ''' <summary>
    ''' Convert speed dial entries from the old jsonified version (pre-0.0.16 RC1) to the new jsonified version
    ''' </summary>
    ''' <param name="SpeedDialType">Speed dial type</param>
    Public Sub ConvertSpeedDialEntries(SpeedDialType As SpeedDialType)
        Dim PathName As String = If(SpeedDialType = SpeedDialType.SFTP, "SFTPSpeedDial", "FTPSpeedDial")
        Dim SpeedDialEnum As KernelPathType = [Enum].Parse(GetType(KernelPathType), PathName)
        Dim SpeedDialJsonContent As String = File.ReadAllText(GetKernelPath(SpeedDialEnum))
        Dim SpeedDialToken As JArray = JArray.Parse(If(Not String.IsNullOrEmpty(SpeedDialJsonContent), SpeedDialJsonContent, "[]"))
        File.Delete(GetKernelPath(SpeedDialEnum))
        For Each SpeedDialEntry As String In SpeedDialToken
            Dim ChosenLineSeparation As String() = SpeedDialEntry.Split(",")
            Dim Address As String = ChosenLineSeparation(0)
            Dim Port As String = ChosenLineSeparation(1)
            Dim Username As String = ChosenLineSeparation(2)
            Dim Encryption As FtpEncryptionMode = If(SpeedDialType = SpeedDialType.FTP, [Enum].Parse(GetType(FtpEncryptionMode), ChosenLineSeparation(3)), FtpEncryptionMode.None)
            AddEntryToSpeedDial(Address, Port, Username, SpeedDialType, Encryption, False)
        Next
    End Sub

End Module

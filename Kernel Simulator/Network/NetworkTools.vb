
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Imports System.ComponentModel
Imports System.Net.NetworkInformation

Public Module NetworkTools

    'Variables
    Public adapterNumber As Long
    Public DRetries As Integer = 3
    Private DFinish As Boolean

    ''' <summary>
    ''' Print each of adapters' properties to the console.
    ''' </summary>
    Public Sub PrintAdapterProperties()
        Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces
        Dim NoV4, NoV6, Failed As Boolean
        Dim gp As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties
        Dim gs6 As IPGlobalStatistics = gp.GetIPv6GlobalStatistics
        Dim gs4 As IPGlobalStatistics = gp.GetIPv4GlobalStatistics

        'Probe for adapter capabilities
        For Each adapter As NetworkInterface In adapters
            adapterNumber += 1
            W("==========================================", True, ColTypes.Neutral)

            'See if it supports IPv6
            If Not adapter.Supports(NetworkInterfaceComponent.IPv6) Then
                Wdbg("W", "{0} doesn't support IPv6. Trying to get information about IPv4.", adapter.Description)
                W(DoTranslation("Adapter {0} doesn't support IPv6. Continuing...", currentLang), True, ColTypes.Err, adapter.Description)
                NoV6 = True
            End If

            'See if it supports IPv4
            If Not adapter.Supports(NetworkInterfaceComponent.IPv4) Then
                Wdbg("E", "{0} doesn't support IPv4.", adapter.Description)
                W(DoTranslation("Adapter {0} doesn't support IPv4. Probe failed.", currentLang), True, ColTypes.Err, adapter.Description)
                NoV4 = True
            End If

            'Get adapter IPv(4/6) properties
            If IsInternetAdapter(adapter) And Not NoV4 Then
                Wdbg("I", "Adapter type of {0}: {1}", adapter.Description, adapter.NetworkInterfaceType.ToString)
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
                        Wdbg("W", "Failed to get IPv6 properties.")
                        W(DoTranslation("Failed to get IPv6 properties for adapter {0}. Continuing...", currentLang), True, ColTypes.Err, adapter.Description)
                    End If
                    If p Is Nothing Then
                        Wdbg("E", "Failed to get IPv4 properties.")
                        W(DoTranslation("Failed to get properties for adapter {0}", currentLang), True, ColTypes.Err, adapter.Description)
                        Failed = True
                    End If
                    WStkTrc(ex)
#Enable Warning BC42104
                End Try

                'Check if statistics is nothing
                If s Is Nothing Then
                    Wdbg("E", "Failed to get statistics.")
                    W(DoTranslation("Failed to get statistics for adapter {0}", currentLang), True, ColTypes.Err, adapter.Description)
                    Failed = True
                End If

                'Print adapter infos if not failed
                If Not Failed Then
                    PrintAdapterIPv4Info(adapter, p, s)
                    'Additionally, print adapter IPv6 infos if available
                    If Not NoV6 Then
                        PrintAdapterIPv6Info(adapter, p6)
                    End If
                End If
            Else
                Wdbg("W", "Adapter {0} doesn't belong in netinfo because the type is {1}", adapter.Description, adapter.NetworkInterfaceType)
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
    Sub PrintAdapterIPv4Info(ByVal NInterface As NetworkInterface, ByVal Properties As IPv4InterfaceProperties, ByVal Statistics As IPv4InterfaceStatistics)
        W(DoTranslation("IPv4 information:", currentLang) + vbNewLine +
          DoTranslation("Adapter Number:", currentLang) + " {0}" + vbNewLine +
          DoTranslation("Adapter Name:", currentLang) + " {1}" + vbNewLine +
          DoTranslation("Maximum Transmission Unit: {2} Units", currentLang) + vbNewLine +
          DoTranslation("DHCP Enabled:", currentLang) + " {3}" + vbNewLine +
          DoTranslation("Non-unicast packets:", currentLang) + " {4}/{5}" + vbNewLine +
          DoTranslation("Unicast packets:", currentLang) + " {6}/{7}" + vbNewLine +
          DoTranslation("Error incoming/outgoing packets:", currentLang) + " {8}/{9}", True, ColTypes.Neutral,
          adapterNumber, NInterface.Description, Properties.Mtu, Properties.IsDhcpEnabled, Statistics.NonUnicastPacketsSent, Statistics.NonUnicastPacketsReceived,
          Statistics.UnicastPacketsSent, Statistics.UnicastPacketsReceived, Statistics.IncomingPacketsWithErrors, Statistics.OutgoingPacketsWithErrors)
    End Sub

    ''' <summary>
    ''' Prints IPv6 info for adapter
    ''' </summary>
    ''' <param name="NInterface">A network interface or adapter</param>
    ''' <param name="Properties">Network properties</param>
    Sub PrintAdapterIPv6Info(ByVal NInterface As NetworkInterface, ByVal Properties As IPv6InterfaceProperties)
        W(DoTranslation("IPv6 information:", currentLang) + vbNewLine +
          DoTranslation("Adapter Number:", currentLang) + " {0}" + vbNewLine +
          DoTranslation("Adapter Name:", currentLang) + " {1}" + vbNewLine +
          DoTranslation("Maximum Transmission Unit: {2} Units", currentLang), True, ColTypes.Neutral,
          adapterNumber, NInterface.Description, Properties.Mtu)
    End Sub

    ''' <summary>
    ''' Prints general network info
    ''' </summary>
    ''' <param name="IPv4Stat">IPv4 general statistics</param>
    ''' <param name="IPv6Stat">IPv6 general statistics</param>
    Sub PrintGeneralNetInfo(ByVal IPv4Stat As IPGlobalStatistics, ByVal IPv6Stat As IPGlobalStatistics)
        W(DoTranslation("General IPv6 properties", currentLang) + vbNewLine +
          DoTranslation("Packets (inbound):", currentLang) + " {0}/{1}" + vbNewLine +
          DoTranslation("Packets (outbound):", currentLang) + " {2}/{3}" + vbNewLine +
          DoTranslation("Errors in received packets:", currentLang) + " {4}/{5}/{6}" + vbNewLine +
          DoTranslation("General IPv4 properties", currentLang) + vbNewLine +
          DoTranslation("Packets (inbound):", currentLang) + " {7}/{8}" + vbNewLine +
          DoTranslation("Packets (outbound):", currentLang) + " {9}/{10}" + vbNewLine +
          DoTranslation("Errors in received packets:", currentLang) + " {11}/{12}/{13}", True, ColTypes.Neutral,
          IPv6Stat.ReceivedPackets, IPv6Stat.ReceivedPacketsDelivered, IPv6Stat.OutputPacketRequests, IPv6Stat.OutputPacketsDiscarded, IPv6Stat.ReceivedPacketsWithAddressErrors,
          IPv6Stat.ReceivedPacketsWithHeadersErrors, IPv6Stat.ReceivedPacketsWithUnknownProtocol, IPv4Stat.ReceivedPackets, IPv4Stat.ReceivedPacketsDelivered, IPv4Stat.OutputPacketRequests,
          IPv4Stat.OutputPacketsDiscarded, IPv4Stat.ReceivedPacketsWithAddressErrors, IPv4Stat.ReceivedPacketsWithHeadersErrors, IPv4Stat.ReceivedPacketsWithUnknownProtocol)
    End Sub

    Function IsInternetAdapter(ByVal InternetAdapter As NetworkInterface) As Boolean
        Return InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.Ethernet Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.Ethernet3Megabit Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.FastEthernetFx Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.FastEthernetT Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.GigabitEthernet Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.Wireless80211 Or
               InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.Tunnel
    End Function

    Dim IsError As Boolean
    Dim ReasonError As Exception

    ''' <summary>
    ''' Downloads a file to the current working directory.
    ''' </summary>
    ''' <param name="URL">A URL to a file</param>
    Sub DownloadFile(ByVal URL As String)
        Dim RetryCount As Integer = 1
        Wdbg("I", "URL: {0}", URL)
        While Not RetryCount > DRetries
            Try
                If Not (URL.StartsWith("ftp://") Or URL.StartsWith("ftps://") Or URL.StartsWith("ftpes://")) Then
                    If Not URL.StartsWith(" ") Then
                        'Limit the filename to the name without any URL arguments
                        Dim FileName As String = URL.Split("/").Last()
                        Wdbg("I", "Prototype Filename: {0}", FileName)
                        If FileName.Contains("?") Then
                            FileName = FileName.Remove(FileName.IndexOf("?"c))
                        End If
                        Wdbg("I", "Finished Filename: {0}", FileName)

                        'Download a file
                        Wdbg("I", "Directory location: {0}", CurrDir)
                        W(DoTranslation("While maintaining stable connection, it is downloading {0} to {1}...", currentLang), True, ColTypes.Neutral, FileName, CurrDir)
                        Dim WClient As New WebClient
                        AddHandler WClient.DownloadProgressChanged, AddressOf DownloadManager
                        AddHandler WClient.DownloadFileCompleted, AddressOf DownloadChecker
                        WClient.DownloadFileAsync(New Uri(URL), CurrDir + "/" + FileName)
                        While Not DFinish
                        End While
                        If IsError Then
                            Throw ReasonError
                        Else
                            W(vbNewLine + DoTranslation("Download has completed.", currentLang), True, ColTypes.Neutral)
                        End If
                    Else
                        W(DoTranslation("Specify the address", currentLang), True, ColTypes.Err)
                    End If
                Else
                    W(DoTranslation("Please use ""ftp"" if you are going to download files from the FTP server.", currentLang), True, ColTypes.Err)
                End If
                Exit Sub
            Catch ex As Exception
                DFinish = False
                W(DoTranslation("Download failed in try {0}: {1}", currentLang), True, ColTypes.Err, RetryCount, ex.Message)
                RetryCount += 1
                Wdbg("I", "Try count: {0}", RetryCount)
                WStkTrc(ex)
            End Try
        End While
    End Sub

    ''' <summary>
    ''' Thread to check for errors on download completion.
    ''' </summary>
    Private Sub DownloadChecker(sender As Object, e As AsyncCompletedEventArgs)
        Wdbg("I", "Download complete. Error: {0}", e.Error?.Message)
        If Not IsNothing(e.Error) Then
            ReasonError = e.Error
            IsError = True
            DFinish = True
        End If
    End Sub

    ''' <summary>
    ''' Thread to repeatedly report the download progress to the console.
    ''' </summary>
    Private Sub DownloadManager(sender As Object, e As DownloadProgressChangedEventArgs)
        If Not DFinish Then
            Console.SetCursorPosition(0, Console.CursorTop)
            WriteWhere(DoTranslation("{0} MB of {1} MB downloaded.", currentLang) + "    ", 0, Console.CursorTop, ColTypes.Neutral, FormatNumber(e.BytesReceived / 1024 / 1024, 2), FormatNumber(e.TotalBytesToReceive / 1024 / 1024, 2))
            If e.BytesReceived = e.TotalBytesToReceive Then
                DFinish = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Changes host name
    ''' </summary>
    ''' <param name="NewHost">New host name</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function ChangeHostname(ByVal NewHost As String) As Boolean
        Try
            HName = NewHost
            Dim ksconf As New IniFile()
            Dim pathConfig As String = paths("Configuration")
            ksconf.Load(pathConfig)
            ksconf.Sections("Login").Keys("Host Name").Value = HName
            ksconf.Save(pathConfig)
            Return True
        Catch ex As Exception
            WStkTrc(ex)
            Wdbg("E", "Failed to change hostname: {0}", ex.Message)
            Throw New EventsAndExceptions.HostnameException(DoTranslation("Failed to change host name: {0}", currentLang).FormatString(ex.Message))
        End Try
        Return False
    End Function

End Module

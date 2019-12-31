
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

Imports System.Net.NetworkInformation

Public Module NetworkTools

    'Variables
    Public adapterNumber As Long
    Public DRetries As Integer = 3
    Private DFinish As Boolean

    Public Sub GetProperties()
        Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces
        Dim NoV4, NoV6, Failed As Boolean
        Dim gp As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties
        Dim gs6 As IPGlobalStatistics = gp.GetIPv6GlobalStatistics
        Dim gs4 As IPGlobalStatistics = gp.GetIPv4GlobalStatistics
        For Each adapter As NetworkInterface In adapters
            adapterNumber += 1
            W("==========================================", True, ColTypes.Neutral)
            If Not adapter.Supports(NetworkInterfaceComponent.IPv6) Then
                Wdbg("{0} doesn't support IPv6. Trying to get information about IPv4.", adapter.Description)
                W(DoTranslation("Adapter {0} doesn't support IPv6. Continuing...", currentLang), True, ColTypes.Neutral, adapter.Description)
                NoV6 = True
            End If
            If Not adapter.Supports(NetworkInterfaceComponent.IPv4) Then
                Wdbg("{0} doesn't support IPv4.", adapter.Description)
                W(DoTranslation("Adapter {0} doesn't support IPv4. Probe failed.", currentLang), True, ColTypes.Neutral, adapter.Description)
                NoV4 = True
            End If
            If adapter.NetworkInterfaceType = NetworkInterfaceType.Ethernet Or
               adapter.NetworkInterfaceType = NetworkInterfaceType.Ethernet3Megabit Or
               adapter.NetworkInterfaceType = NetworkInterfaceType.FastEthernetFx Or
               adapter.NetworkInterfaceType = NetworkInterfaceType.FastEthernetT Or
               adapter.NetworkInterfaceType = NetworkInterfaceType.GigabitEthernet Or
               adapter.NetworkInterfaceType = NetworkInterfaceType.Wireless80211 Or
               adapter.NetworkInterfaceType = NetworkInterfaceType.Tunnel And Not NoV4 Then
                Wdbg("Adapter type of {0}: {1}", adapter.Description, adapter.NetworkInterfaceType.ToString)
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
                        W(DoTranslation("Failed to get IPv6 properties for adapter {0}. Continuing...", currentLang), True, ColTypes.Neutral, adapter.Description)
                    End If
                    If p Is Nothing Then
                        W(DoTranslation("Failed to get properties for adapter {0}", currentLang), True, ColTypes.Neutral, adapter.Description)
                        Failed = True
                    End If
                    WStkTrc(ex)
#Enable Warning BC42104
                End Try
                If s Is Nothing Then
                    W(DoTranslation("Failed to get statistics for adapter {0}", currentLang), True, ColTypes.Neutral, adapter.Description)
                    Failed = True
                End If
                If Not Failed Then
                    W(DoTranslation("IPv4 information:", currentLang) + vbNewLine +
                      DoTranslation("Adapter Number:", currentLang) + " {0}" + vbNewLine +
                      DoTranslation("Adapter Name:", currentLang) + " {1}" + vbNewLine +
                      DoTranslation("Maximum Transmission Unit: {2} Units", currentLang) + vbNewLine +
                      DoTranslation("DHCP Enabled:", currentLang) + " {3}" + vbNewLine +
                      DoTranslation("Non-unicast packets:", currentLang) + " {4}/{5}" + vbNewLine +
                      DoTranslation("Unicast packets:", currentLang) + " {6}/{7}" + vbNewLine +
                      DoTranslation("Error incoming/outgoing packets:", currentLang) + " {8}/{9}", True, ColTypes.Neutral,
                      adapterNumber, adapter.Description, p.Mtu, p.IsDhcpEnabled, s.NonUnicastPacketsSent, s.NonUnicastPacketsReceived,
                      s.UnicastPacketsSent, s.UnicastPacketsReceived, s.IncomingPacketsWithErrors, s.OutgoingPacketsWithErrors)
                    If Not NoV6 Then
                        W(DoTranslation("IPv6 information:", currentLang) + vbNewLine +
                          DoTranslation("Adapter Number:", currentLang) + " {0}" + vbNewLine +
                          DoTranslation("Adapter Name:", currentLang) + " {1}" + vbNewLine +
                          DoTranslation("Maximum Transmission Unit: {2} Units", currentLang), True, ColTypes.Neutral,
                          adapterNumber, adapter.Description, p6.Mtu)
                    End If
                End If
            Else
                Wdbg("Adapter {0} doesn't belong in netinfo because the type is {1}", adapter.Description, adapter.NetworkInterfaceType)
            End If
        Next
        W(DoTranslation("General IPv6 properties", currentLang) + vbNewLine +
          DoTranslation("Packets (inbound):", currentLang) + " {0}/{1}" + vbNewLine +
          DoTranslation("Packets (outbound):", currentLang) + " {2}/{3}" + vbNewLine +
          DoTranslation("Errors in received packets:", currentLang) + " {4}/{5}/{6}" + vbNewLine +
          DoTranslation("General IPv4 properties", currentLang) + vbNewLine +
          DoTranslation("Packets (inbound):", currentLang) + " {7}/{8}" + vbNewLine +
          DoTranslation("Packets (outbound):", currentLang) + " {9}/{10}" + vbNewLine +
          DoTranslation("Errors in received packets:", currentLang) + " {11}/{12}/{13}", True, ColTypes.Neutral,
          gs6.ReceivedPackets, gs6.ReceivedPacketsDelivered, gs6.OutputPacketRequests, gs6.OutputPacketsDiscarded, gs6.ReceivedPacketsWithAddressErrors,
          gs6.ReceivedPacketsWithHeadersErrors, gs6.ReceivedPacketsWithUnknownProtocol, gs4.ReceivedPackets, gs4.ReceivedPacketsDelivered, gs4.OutputPacketRequests,
          gs4.OutputPacketsDiscarded, gs4.ReceivedPacketsWithAddressErrors, gs4.ReceivedPacketsWithHeadersErrors, gs4.ReceivedPacketsWithUnknownProtocol)
    End Sub

    Sub DownloadFile(ByVal URL As String)
        Dim RetryCount As Integer = 1
        While Not RetryCount > DRetries
            Try
                If Not (URL.StartsWith("ftp://") Or URL.StartsWith("ftps://") Or URL.StartsWith("ftpes://") Or URL.StartsWith("sftp://")) Then
                    If Not URL.StartsWith(" ") Then
                        Dim FileName As String = URL.Split("/").Last()
                        Wdbg("Prototype Filename: {0}", FileName)
                        If FileName.Contains("?") Then
                            FileName = FileName.Remove(FileName.IndexOf("?"c))
                        End If
                        Wdbg("Finished Filename: {0}", FileName)
                        Wdbg("Directory location: {0}", CurrDir)
                        W(DoTranslation("While maintaining stable connection, it is downloading {0} to {1}...", currentLang), True, ColTypes.Neutral, FileName, CurrDir)
                        Dim WClient As New WebClient
                        AddHandler WClient.DownloadProgressChanged, AddressOf DownloadManager
                        WClient.DownloadFileAsync(New Uri(URL), CurrDir + "/" + FileName)
                        While Not DFinish
                        End While
                    Else
                        W(DoTranslation("Specify the address", currentLang), True, ColTypes.Neutral)
                    End If
                Else
                    W(DoTranslation("Please use ""ftp"" if you are going to download files from the FTP server.", currentLang), True, ColTypes.Neutral)
                End If
                Exit Sub
            Catch ex As Exception
                W(DoTranslation("Download failed in try {0}: {1}", currentLang), True, ColTypes.Neutral, RetryCount, ex.Message)
                RetryCount += 1
                Wdbg("Try count: {0}", RetryCount)
                WStkTrc(ex)
            End Try
        End While
    End Sub

    Private Sub DownloadManager(sender As Object, e As DownloadProgressChangedEventArgs)
        If Not DFinish Then
            Console.SetCursorPosition(0, Console.CursorTop)
            W(DoTranslation("{0} MB of {1} MB downloaded.", currentLang) + "    ", False, ColTypes.Neutral, e.BytesReceived / 1024 / 1024, e.TotalBytesToReceive / 1024 / 1024)
            If e.BytesReceived = e.TotalBytesToReceive Then
                W(vbNewLine + DoTranslation("Download has completed.", currentLang), True, ColTypes.Neutral)
                DFinish = True
            End If
        End If
    End Sub

End Module

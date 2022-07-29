
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Namespace Network
    Public Module NetworkAdapterPrint

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
                Write("==========================================", True, ColTypes.Neutral)

                'See if it supports IPv6
                If Not adapter.Supports(NetworkInterfaceComponent.IPv6) Then
                    Wdbg(DebugLevel.W, "{0} doesn't support IPv6. Trying to get information about IPv4.", adapter.Description)
                    Write(DoTranslation("Adapter {0} doesn't support IPv6. Continuing..."), True, ColTypes.Error, adapter.Description)
                    NoV6 = True
                End If

                'See if it supports IPv4
                If Not adapter.Supports(NetworkInterfaceComponent.IPv4) Then
                    Wdbg(DebugLevel.E, "{0} doesn't support IPv4.", adapter.Description)
                    Write(DoTranslation("Adapter {0} doesn't support IPv4. Probe failed."), True, ColTypes.Error, adapter.Description)
                    NoV4 = True
                End If

                'Get adapter IPv(4/6) properties
                If IsInternetAdapter(adapter) And Not NoV4 Then
                    Wdbg(DebugLevel.I, "Adapter type of {0}: {1}", adapter.Description, adapter.NetworkInterfaceType.ToString)
                    Dim adapterProperties As IPInterfaceProperties = adapter.GetIPProperties()
                    Dim p As IPv4InterfaceProperties
                    Dim s As IPv4InterfaceStatistics = adapter.GetIPv4Statistics
                    Dim p6 As IPv6InterfaceProperties
                    Try
                        p = adapterProperties.GetIPv4Properties
                        p6 = adapterProperties.GetIPv6Properties
                    Catch ex As NetworkInformationException
#Disable Warning BC42104
                        If p6 Is Nothing Then
                            Wdbg(DebugLevel.W, "Failed to get IPv6 properties.")
                            Write(DoTranslation("Failed to get IPv6 properties for adapter {0}. Continuing..."), True, ColTypes.Error, adapter.Description)
                        End If
                        If p Is Nothing Then
                            Wdbg(DebugLevel.E, "Failed to get IPv4 properties.")
                            Write(DoTranslation("Failed to get properties for adapter {0}"), True, ColTypes.Error, adapter.Description)
                            Failed = True
                        End If
                        WStkTrc(ex)
#Enable Warning BC42104
                    End Try

                    'Check if statistics is nothing
                    If s Is Nothing Then
                        Wdbg(DebugLevel.E, "Failed to get statistics.")
                        Write(DoTranslation("Failed to get statistics for adapter {0}"), True, ColTypes.Error, adapter.Description)
                        Failed = True
                    End If

                    'Print adapter infos if not failed
                    If Not Failed Then
                        PrintAdapterIPv4Info(adapter, p, s, adapterNumber)
                        'Additionally, print adapter IPv6 info if available
                        If Not NoV6 Then
                            PrintAdapterIPv6Info(adapter, p6, adapterNumber)
                        End If
                    End If
                Else
                    Wdbg(DebugLevel.W, "Adapter {0} doesn't belong in netinfo because the type is {1}", adapter.Description, adapter.NetworkInterfaceType)
                End If
            Next

            'Print general IPv4 and IPv6 information
            If GeneralNetworkInformation Then
                Write("==========================================", True, ColTypes.Neutral)
                PrintGeneralNetInfo(gs4, gs6)
            End If
        End Sub

        ''' <summary>
        ''' Prints IPv4 info for adapter
        ''' </summary>
        ''' <param name="NInterface">A network interface or adapter</param>
        ''' <param name="Properties">Network properties</param>
        ''' <param name="Statistics">Network statistics</param>
        ''' <param name="AdapterNumber">Reference adapter number</param>
        Sub PrintAdapterIPv4Info(NInterface As NetworkInterface, Properties As IPv4InterfaceProperties, Statistics As IPv4InterfaceStatistics, AdapterNumber As Long)
            If ExtensiveAdapterInformation Then
                Write(DoTranslation("IPv4 information:") + NewLine +
                  DoTranslation("Adapter Number:") + " {0}" + NewLine +
                  DoTranslation("Adapter Name:") + " {1}" + NewLine +
                  DoTranslation("Maximum Transmission Unit: {2} Units") + NewLine +
                  DoTranslation("DHCP Enabled:") + " {3}" + NewLine +
                  DoTranslation("Non-unicast packets:") + " {4}/{5}" + NewLine +
                  DoTranslation("Unicast packets:") + " {6}/{7}" + NewLine +
                  DoTranslation("Error incoming/outgoing packets:") + " {8}/{9}", True, ColTypes.Neutral,
                  AdapterNumber, NInterface.Description, Properties.Mtu, Properties.IsDhcpEnabled, Statistics.NonUnicastPacketsSent, Statistics.NonUnicastPacketsReceived,
                  Statistics.UnicastPacketsSent, Statistics.UnicastPacketsReceived, Statistics.IncomingPacketsWithErrors, Statistics.OutgoingPacketsWithErrors)
            Else
                Write(DoTranslation("IPv4 information:") + NewLine +
                  DoTranslation("Adapter Number:") + " {0}" + NewLine +
                  DoTranslation("Adapter Name:") + " {1}", True, ColTypes.Neutral,
                  AdapterNumber, NInterface.Description)
            End If
        End Sub

        ''' <summary>
        ''' Prints IPv6 info for adapter
        ''' </summary>
        ''' <param name="NInterface">A network interface or adapter</param>
        ''' <param name="Properties">Network properties</param>
        ''' <param name="AdapterNumber">Reference adapter number</param>
        Sub PrintAdapterIPv6Info(NInterface As NetworkInterface, Properties As IPv6InterfaceProperties, AdapterNumber As Long)
            If ExtensiveAdapterInformation Then
                Write(DoTranslation("IPv6 information:") + NewLine +
                  DoTranslation("Adapter Number:") + " {0}" + NewLine +
                  DoTranslation("Adapter Name:") + " {1}" + NewLine +
                  DoTranslation("Maximum Transmission Unit: {2} Units"), True, ColTypes.Neutral,
                  AdapterNumber, NInterface.Description, Properties.Mtu)
            Else
                Write(DoTranslation("IPv6 information:") + NewLine +
                  DoTranslation("Adapter Number:") + " {0}" + NewLine +
                  DoTranslation("Adapter Name:") + " {1}", True, ColTypes.Neutral,
                  AdapterNumber, NInterface.Description)
            End If
        End Sub

        ''' <summary>
        ''' Prints general network info
        ''' </summary>
        ''' <param name="IPv4Stat">IPv4 general statistics</param>
        ''' <param name="IPv6Stat">IPv6 general statistics</param>
        Sub PrintGeneralNetInfo(IPv4Stat As IPGlobalStatistics, IPv6Stat As IPGlobalStatistics)
            Write(DoTranslation("General IPv6 properties") + NewLine +
              DoTranslation("Packets (inbound):") + " {0}/{1}" + NewLine +
              DoTranslation("Packets (outbound):") + " {2}/{3}" + NewLine +
              DoTranslation("Errors in received packets:") + " {4}/{5}/{6}" + NewLine +
              DoTranslation("General IPv4 properties") + NewLine +
              DoTranslation("Packets (inbound):") + " {7}/{8}" + NewLine +
              DoTranslation("Packets (outbound):") + " {9}/{10}" + NewLine +
              DoTranslation("Errors in received packets:") + " {11}/{12}/{13}", True, ColTypes.Neutral,
              IPv6Stat.ReceivedPackets, IPv6Stat.ReceivedPacketsDelivered, IPv6Stat.OutputPacketRequests, IPv6Stat.OutputPacketsDiscarded, IPv6Stat.ReceivedPacketsWithAddressErrors,
              IPv6Stat.ReceivedPacketsWithHeadersErrors, IPv6Stat.ReceivedPacketsWithUnknownProtocol, IPv4Stat.ReceivedPackets, IPv4Stat.ReceivedPacketsDelivered, IPv4Stat.OutputPacketRequests,
              IPv4Stat.OutputPacketsDiscarded, IPv4Stat.ReceivedPacketsWithAddressErrors, IPv4Stat.ReceivedPacketsWithHeadersErrors, IPv4Stat.ReceivedPacketsWithUnknownProtocol)
        End Sub

    End Module
End Namespace

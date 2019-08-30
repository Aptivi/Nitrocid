
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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
    Public Computers As String

    Public Sub GetProperties()
        Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces
        For Each adapter As NetworkInterface In adapters
            adapterNumber += 1
            If adapter.Supports(NetworkInterfaceComponent.IPv4) = False Then
                Wdbg("{0} doesn't support IPv4 because ASSERT(adapter.Supp(IPv4) = True) = False.", adapter.Description)
            ElseIf adapter.NetworkInterfaceType = NetworkInterfaceType.Ethernet Or
                    adapter.NetworkInterfaceType = NetworkInterfaceType.Ethernet3Megabit Or
                    adapter.NetworkInterfaceType = NetworkInterfaceType.FastEthernetFx Or
                    adapter.NetworkInterfaceType = NetworkInterfaceType.FastEthernetT Or
                    adapter.NetworkInterfaceType = NetworkInterfaceType.GigabitEthernet Or
                    adapter.NetworkInterfaceType = NetworkInterfaceType.Wireless80211 Then
                Dim adapterProperties As IPInterfaceProperties = adapter.GetIPProperties()
                Dim p As IPv4InterfaceProperties = adapterProperties.GetIPv4Properties
                Dim s As IPv4InterfaceStatistics = adapter.GetIPv4Statistics
                If p Is Nothing Then
                    W(DoTranslation("Failed to get properties for adapter {0}", currentLang), True, "neutralText", adapter.Description)
                ElseIf s Is Nothing Then
                    W(DoTranslation("Failed to get statistics for adapter {0}", currentLang), True, "neutralText", adapter.Description)
                End If
                W(DoTranslation("Adapter Number:", currentLang) + " {0}" + vbNewLine +
                  DoTranslation("Adapter Name:", currentLang) + " {1}" + vbNewLine +
                  DoTranslation("Maximum Transmission Unit: {2} Units", currentLang) + vbNewLine +
                  DoTranslation("DHCP Enabled:", currentLang) + " {3}" + vbNewLine +
                  DoTranslation("Non-unicast packets:", currentLang) + " {4}/{5}" + vbNewLine +
                  DoTranslation("Unicast packets:", currentLang) + " {6}/{7}" + vbNewLine +
                  DoTranslation("Error incoming/outgoing packets:", currentLang) + " {8}/{9}", True, "neutralText",
                  adapterNumber, adapter.Description, p.Mtu, p.IsDhcpEnabled, s.NonUnicastPacketsSent, s.NonUnicastPacketsReceived,
                  s.UnicastPacketsSent, s.UnicastPacketsReceived, s.IncomingPacketsWithErrors, s.OutgoingPacketsWithErrors)
            Else
                Wdbg("Adapter {0} doesn't belong in netinfo because the type is {1}", adapter.Description, adapter.NetworkInterfaceType)
            End If
        Next
    End Sub

End Module

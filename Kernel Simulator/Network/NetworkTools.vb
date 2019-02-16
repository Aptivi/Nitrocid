
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

Imports System.Net
Imports System.Net.NetworkInformation
Imports System.DirectoryServices
Imports System.Net.Sockets.AddressFamily

Public Module NetworkTools

    'Variables
    Public adapterNumber As Long
    Public Computers As String

    Public Sub ListOnlineAndOfflineHosts()

        'Check if main network is available
        If My.Computer.Network.IsAvailable Then
            'Variables
            Dim ComputerNames() = Computers.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

            'Display information
            Wln(DoTranslation("net: Your computer name on network is {0}", currentLang) + vbNewLine +
                DoTranslation("net: Your host name is {1}", currentLang) + vbNewLine +
                DoTranslation("net: It appears that computers are in the domain or workgroup:", currentLang), "neutralText", My.Computer.Name, HName)

            'List online and offline computers
            For Each cmp In ComputerNames
                Wln("net: {0}", "neutralText", cmp)
            Next
        Else
            Wln(DoTranslation("net: WiFi or Ethernet is disconnected.", currentLang), "neutralText")
        End If

    End Sub

    Public Sub ListHostsInNetwork()

        'Error Handler
        On Error Resume Next

        'Check if main network is available
        If My.Computer.Network.IsAvailable Then
            'Variables
            Dim HostNameFromDNS As String
            Dim IPHostEntry As IPHostEntry
            Dim ComputerNames() = Computers.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

            'Display infromation
            Wln(DoTranslation("net: Your computer name on network is {0}", currentLang) + vbNewLine +
                DoTranslation("net: Your host name is {1}", currentLang) + vbNewLine +
                DoTranslation("net: It appears that computers are connected below:", currentLang), "neutralText", My.Computer.Name, HName)

            'List IP addresses of computers
            For Each cmp In ComputerNames
                HostNameFromDNS = cmp
                IPHostEntry = Dns.GetHostEntry(HostNameFromDNS)
                Dim p As New Ping()
                For Each ipheal In IPHostEntry.AddressList
                    Dim reply = p.Send(ipheal, 100)
                    If (reply.Status = IPStatus.Success) Then
                        Wln("net: {0}: {1}", "neutralText", cmp, ipheal.ToString)
                    End If
                Next
            Next

            'Get router address
            HostNameFromDNS = Dns.GetHostName()
            IPHostEntry = Dns.GetHostEntry(HostNameFromDNS)
            For Each ip In IPHostEntry.AddressList
                For Each router As NetworkInterface In NetworkInterface.GetAllNetworkInterfaces()
                    For Each UnicastIPAI As UnicastIPAddressInformation In router.GetIPProperties.UnicastAddresses
                        If UnicastIPAI.Address.AddressFamily = InterNetwork Then
                            If ip.Equals(UnicastIPAI.Address) Then
                                Dim adapterProperties As IPInterfaceProperties = router.GetIPProperties()
                                For Each gateway As GatewayIPAddressInformation In adapterProperties.GatewayAddresses
                                    Wln(DoTranslation("net: Router Address: {0}", currentLang), "neutralText", gateway.Address.ToString())
                                Next
                            End If
                        End If
                    Next
                Next
            Next
        Else
            Wln(DoTranslation("net: WiFi or Ethernet is disconnected.", currentLang), "neutralText")
        End If

    End Sub

    Public Sub GetNetworkComputers()

        'Error Handler
        On Error Resume Next

        'Variables
        Dim alWorkGroups As New ArrayList
        Dim de As New DirectoryEntry

        'Clear "Computers" variable for cleanup
        Computers = Nothing

        'Get workgroups and domain
        de.Path = "WinNT:"
        For Each d As DirectoryEntry In de.Children
            If d.SchemaClassName = "Domain" Then alWorkGroups.Add(d.Name)
            d.Dispose()
        Next

        'Get computers
        For Each workgroup As String In alWorkGroups
            de.Path = "WinNT://" & workgroup
            For Each d As DirectoryEntry In de.Children
                If d.SchemaClassName = "Computer" Then
                    Computers = Computers + " " + d.Name + " "
                End If
                d.Dispose()
            Next
        Next

    End Sub

    Public Sub ListHostsInTree()

        'Error Handler
        On Error Resume Next

        'Check if main network is available
        If My.Computer.Network.IsAvailable Then
            'Variables
            Dim HostNameFromDNS As String = Dns.GetHostName()
            Dim IPHostEntry As Net.IPHostEntry = Dns.GetHostEntry(HostNameFromDNS)
            Dim ComputerNames() = Computers.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

            'Get router
            For Each ip In IPHostEntry.AddressList
                For Each router As NetworkInterface In NetworkInterface.GetAllNetworkInterfaces()
                    For Each UnicastIPAI As UnicastIPAddressInformation In router.GetIPProperties.UnicastAddresses
                        If UnicastIPAI.Address.AddressFamily = InterNetwork Then
                            If ip.Equals(UnicastIPAI.Address) Then
                                Dim adapterProperties As IPInterfaceProperties = router.GetIPProperties()
                                For Each gateway As GatewayIPAddressInformation In adapterProperties.GatewayAddresses
                                    Wln(gateway.Address.ToString(), "neutralText")
                                Next
                            End If
                        End If
                    Next
                Next
            Next

            'Get IP addresses of computers
            For Each cmp In ComputerNames
                HostNameFromDNS = cmp
                IPHostEntry = Dns.GetHostEntry(HostNameFromDNS)
                Dim p As New System.Net.NetworkInformation.Ping()
                For Each ipheal In IPHostEntry.AddressList
                    Dim reply = p.Send(ipheal, 100)
                    If (reply.Status = IPStatus.Success) Then
                        Wln("|-> {0}: {1}", "neutralText", cmp, ipheal.ToString)
                    End If
                Next
            Next
        Else
            Wln(DoTranslation("net: WiFi or Ethernet is disconnected.", currentLang), "neutralText")
        End If

    End Sub

    Public Sub getProperties()

        Dim proper As IPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties
        Dim adapters As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces
        For Each adapter As NetworkInterface In adapters
            adapterNumber = adapterNumber + 1
            If adapter.Supports(NetworkInterfaceComponent.IPv4) = False Then
                Wdbg("{0} doesn't support IPv4 because ASSERT(adapter.Supp(IPv4) = True) = False.", adapter.Description)
            ElseIf (adapter.NetworkInterfaceType = NetworkInterfaceType.Ethernet Or
                    adapter.NetworkInterfaceType = NetworkInterfaceType.Ethernet3Megabit Or
                    adapter.NetworkInterfaceType = NetworkInterfaceType.FastEthernetFx Or
                    adapter.NetworkInterfaceType = NetworkInterfaceType.FastEthernetT Or
                    adapter.NetworkInterfaceType = NetworkInterfaceType.GigabitEthernet Or
                    adapter.NetworkInterfaceType = NetworkInterfaceType.Wireless80211) Then
                Dim adapterProperties As IPInterfaceProperties = adapter.GetIPProperties()
                Dim p As IPv4InterfaceProperties = adapterProperties.GetIPv4Properties
                Dim s As IPv4InterfaceStatistics = adapter.GetIPv4Statistics
                If (p Is Nothing) Then
                    Wln(DoTranslation("Failed to get properties for adapter {0}", currentLang), "neutralText", adapter.Description)
                ElseIf (s Is Nothing) Then
                    Wln(DoTranslation("Failed to get statistics for adapter {0}", currentLang), "neutralText", adapter.Description)
                End If
                Wln(DoTranslation("Adapter Number:", currentLang) + " {0}" + vbNewLine +
                    DoTranslation("Adapter Name:", currentLang) + " {1}" + vbNewLine +
                    DoTranslation("Maximum Transmission Unit: {2} Units", currentLang) + vbNewLine +
                    DoTranslation("DHCP Enabled:", currentLang) + " {3}" + vbNewLine +
                    DoTranslation("Non-unicast packets:", currentLang) + " {4}/{5}" + vbNewLine +
                    DoTranslation("Unicast packets:", currentLang) + " {6}/{7}" + vbNewLine +
                    DoTranslation("Error incoming/outgoing packets:", currentLang) + " {8}/{9}", "neutralText",
                    adapterNumber, adapter.Description, p.Mtu, p.IsDhcpEnabled, s.NonUnicastPacketsSent, s.NonUnicastPacketsReceived,
                    s.UnicastPacketsSent, s.UnicastPacketsReceived, s.IncomingPacketsWithErrors, s.OutgoingPacketsWithErrors)
            Else
                Wdbg("Adapter {0} doesn't belong in netinfo because the type is {1}", adapter.Description, adapter.NetworkInterfaceType)
            End If
        Next

    End Sub

End Module

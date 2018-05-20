
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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
Imports System.Net.NetworkInformation.NetworkInterface
Imports System.DirectoryServices
Imports System.Net.Sockets.AddressFamily

Module NetworkList

    'Variables
    Public Computers As String

    Sub ListHostsInNetwork()

        'Check if main network is available
        If (My.Computer.Network.IsAvailable = True) Then

            'Variables
            Dim HostNameFromDNS As String
            Dim IPHostEntry As IPHostEntry
            Dim ComputerNames() = Computers.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

            'Display infromation
            Wln("net: Your computer name on network is {0}" + vbNewLine + _
                "net: Your host name is {1}" + vbNewLine + _
                "net: It appears that your computers is connected below:", "neutralText", My.Computer.Name, My.Settings.HostName)

            'List IP addresses of computers
            For Each cmp In ComputerNames
                HostNameFromDNS = cmp
                IPHostEntry = Dns.GetHostEntry(HostNameFromDNS)
                Dim p As New System.Net.NetworkInformation.Ping()
                For Each ipheal In IPHostEntry.AddressList
                    Dim reply = p.Send(ipheal, 100)
                    If (reply.Status = IPStatus.Success) Then
                        Wln("net: {0}: {1}", "neutralText", cmp, ipheal)
                    End If
                Next
            Next

            'Get router address
            W("net: Router Address: ", "neutralText")
            HostNameFromDNS = Dns.GetHostName()
            IPHostEntry = Dns.GetHostEntry(HostNameFromDNS)
            For Each ip In IPHostEntry.AddressList
                For Each router As NetworkInterface In GetAllNetworkInterfaces()
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

        Else

            Wln("net: WiFi or Ethernet is disconnected.", "neutralText")

        End If

    End Sub

    Sub GetNetworkComputers()

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

    Sub ListHostsInTree()

        'Check if main network is available
        If My.Computer.Network.IsAvailable = True Then

            'Variables
            Dim HostNameFromDNS As String = Dns.GetHostName()
            Dim IPHostEntry As Net.IPHostEntry = Dns.GetHostEntry(HostNameFromDNS)
            Dim ComputerNames() = Computers.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

            'Get router
            For Each ip In IPHostEntry.AddressList
                For Each router As NetworkInterface In GetAllNetworkInterfaces()
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
                        Wln("|-> {0}: {1}", "neutralText", cmp, ipheal)
                    End If
                Next
            Next

        Else

            Wln("net: WiFi or Ethernet is disconnected.", "neutralText")

        End If

    End Sub

End Module

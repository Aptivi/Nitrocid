
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

        On Error GoTo ErrorNetwork1

        'Fetch all computers
        GetNetworkComputers()

        'Variables
        Dim HostNameFromDNS As String
        Dim IPHostEntry As IPHostEntry
        Dim ComputerNames() = Computers.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

        'Display infromation
        System.Console.WriteLine("net: Your computer name on network is {0}", My.Computer.Name)
        System.Console.WriteLine("net: Your host name is {0}", My.Settings.HostName)
        System.Console.WriteLine("net: It appears that your computers is connected below:")

        'List IP addresses of computers
        For Each cmp In ComputerNames
            HostNameFromDNS = cmp
            IPHostEntry = Dns.GetHostEntry(HostNameFromDNS)
            For Each ipheal In IPHostEntry.AddressList
                System.Console.WriteLine("{0}: {1}", cmp, ipheal.ToString)
            Next
        Next

        'Get router address
        System.Console.Write("net: Router Address: ")
        HostNameFromDNS = Dns.GetHostName()
        IPHostEntry = Dns.GetHostEntry(HostNameFromDNS)
        For Each ip In IPHostEntry.AddressList
            For Each router As NetworkInterface In GetAllNetworkInterfaces()
                For Each UnicastIPAI As UnicastIPAddressInformation In router.GetIPProperties.UnicastAddresses
                    If UnicastIPAI.Address.AddressFamily = InterNetwork Then
                        If ip.Equals(UnicastIPAI.Address) Then
                            Dim adapterProperties As IPInterfaceProperties = router.GetIPProperties()
                            For Each gateway As GatewayIPAddressInformation In adapterProperties.GatewayAddresses
                                System.Console.WriteLine(gateway.Address.ToString())
                            Next
                        End If
                    End If
                Next
            Next
        Next

        Exit Sub

ErrorNetwork1:
        System.Console.WriteLine("Error on network: {0}", Err.Description)

    End Sub

    Sub GetNetworkComputers()

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

        On Error GoTo ErrorNetwork2

        'Variables
        Dim HostNameFromDNS As String = Dns.GetHostName()
        Dim IPHostEntry As Net.IPHostEntry = Dns.GetHostEntry(HostNameFromDNS)

        'Get router
        For Each ip In IPHostEntry.AddressList
            For Each router As NetworkInterface In GetAllNetworkInterfaces()
                For Each UnicastIPAI As UnicastIPAddressInformation In router.GetIPProperties.UnicastAddresses
                    If UnicastIPAI.Address.AddressFamily = InterNetwork Then
                        If ip.Equals(UnicastIPAI.Address) Then
                            Dim adapterProperties As IPInterfaceProperties = router.GetIPProperties()
                            For Each gateway As GatewayIPAddressInformation In adapterProperties.GatewayAddresses
                                System.Console.WriteLine(gateway.Address.ToString())
                            Next
                        End If
                    End If
                Next
            Next
        Next

        'Get IP addresses of computers
        GetNetworkComputers()
        Dim ComputerNames() = Computers.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
        For Each cmp In ComputerNames
            HostNameFromDNS = cmp
            IPHostEntry = Dns.GetHostEntry(HostNameFromDNS)
            For Each ipheal As Net.IPAddress In IPHostEntry.AddressList
                System.Console.WriteLine("|-> {0}: {1}", cmp, ipheal.ToString)
            Next
        Next

        Exit Sub

ErrorNetwork2:
        System.Console.WriteLine("Error on network: {0}", Err.Description)

    End Sub

End Module

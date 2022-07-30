
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

Imports System.Net.NetworkInformation

Namespace Network
    Public Module NetworkAdapter

        ''' <summary>
        ''' Checks to see if the adapter is one of the Internet adapters (Ethernet, Ethernet3Megabit, FastEthernetFx, FastEthernetT, GigabitEthernet, Wireless80211, Tunnel)
        ''' </summary>
        ''' <param name="InternetAdapter">Network adapter</param>
        Public Function IsInternetAdapter(InternetAdapter As NetworkInterface) As Boolean
            Return InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.Ethernet Or
                   InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.Ethernet3Megabit Or
                   InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.FastEthernetFx Or
                   InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.FastEthernetT Or
                   InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.GigabitEthernet Or
                   InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.Wireless80211 Or
                   InternetAdapter.NetworkInterfaceType = NetworkInterfaceType.Tunnel
        End Function

    End Module
End Namespace

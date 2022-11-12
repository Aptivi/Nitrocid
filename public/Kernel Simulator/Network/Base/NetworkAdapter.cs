
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Net.NetworkInformation;

namespace KS.Network.Base
{
    /// <summary>
    /// Network adapter instance
    /// </summary>
    public static class NetworkAdapter
    {

        /// <summary>
        /// Checks to see if the adapter is one of the Internet adapters (Ethernet, Ethernet3Megabit, FastEthernetFx, FastEthernetT, GigabitEthernet, Wireless80211, Tunnel)
        /// </summary>
        /// <param name="InternetAdapter">Network adapter</param>
        public static bool IsInternetAdapter(NetworkInterface InternetAdapter) => InternetAdapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet | InternetAdapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet3Megabit | InternetAdapter.NetworkInterfaceType == NetworkInterfaceType.FastEthernetFx | InternetAdapter.NetworkInterfaceType == NetworkInterfaceType.FastEthernetT | InternetAdapter.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet | InternetAdapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 | InternetAdapter.NetworkInterfaceType == NetworkInterfaceType.Tunnel;

    }
}

## netinfo command

### Summary

You can view the detailed status of the network connection

### Description

This command lets you view the details about all your wireless/Ethernet adapters, packets, packets that has an error, etc. The information that are printed is diagnostic so if you can't connect to the Internet, you can use these information to diagnose.

The sections of the information about adapters:

- Adapter Number: This tells you the number of the current adapter we're getting information from
- Adapter Name: This tells you the name of the current adapter
- Maximum Transmission Unit: This tells you the maximum transmission unit of the adapter
- DHCP Enabled: This tells you whether the DHCP is enabled, or if it uses the static IP address for network configuration
- Non-unicast packets: This tells you the packets that are non-unicast
- Unicast packets: This tells you the packets that are unicast
- Error incoming/outgoing packets: This tells you the packets that has errors

### Command usage

* `netinfo`
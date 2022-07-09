# Remote debugging guide

## What is remote debugging?

Remote debugging is the way of viewing debug information about one device on another device around the network. It requires a high-speed LAN connection to be able to activate remote debugging using raw connection.

By default, it listens on port 3014, and can be changed by kernel configuration. The debugging output will be the same as the output file, but missing all the messages before you connected. The first message you will get is `Debug device <LocalIP> joined`.

It not only provides facilities to look at the debug output live, but you can also chat with the other debuggers to discuss about the current problem that happened, like asking questions, and more.

## How can I connect to debug?

Since all the connections are established using the raw text mode, there's no need to authenticate. All you need is the IP address of the remote machine and the port, 3014.

If you don't know what tool do you need to have on your system, follow the instructions with your platform.

### Linux/Unix systems

1. Install netcat using your package manager (e.g. `sudo apt install netcat`)
2. Instruct nc/netcat to connect to your debug machine to remotely debug (e.g. `nc <LocalIP> 3014`)

### Windows systems

1. Install [PuTTY](https://www.putty.org/)
2. Open it, set the connection mode to Raw, and write the IP address and the port.
3. Click on Open to see debugging messages on another machine.

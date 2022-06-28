## KSConverter

KSConverter is an application that can convert your Kernel Simulator configuration files from the old format (0.0.15.x or lower) to the newer, modern format (0.0.16.x or higher). It can even convert your configuration files from the first version that implemented it, which is 0.0.4.

It comes with Kernel Simulator and does not need to be installed separately in order to be used.

Your old configuration files will be backed up to KSBackups directory in your user profile directory so you can easil revert to a pre-1.3 API version of KS (0.0.15.x or lower).

### Conversion

KSConverter can convert the following configurations:

- Kernel configuration
- Blocked devices list to be added to list of devices
- FTP speed dial
- Users
- Aliases

...from the configuration files that were previously made on earlier versions of KS in the following formats:

| File                | Version              | Result
|:--------------------|:---------------------|:-------
| kernelConfig.ini    | 0.0.4.x -> 0.0.5.4   | KernelConfig.json
| kernelConfig.ini    | 0.0.5.5 -> 0.0.15.x  | KernelConfig.json
| blocked_devices.csv | 0.0.12.x -> 0.0.15.x | DebugDeviceNames.json
| ftp_speeddial.csv   | 0.0.11.x -> 0.0.15.x | FTP_SpeedDial.json
| users.csv           | 0.0.4 -> 0.0.15.x    | Users.json
| aliases.csv         | 0.0.6 -> 0.0.15.x    | Aliases.json

### How to use

- On Windows, you can just double-click on the KSConverter.exe file. It's usually found on the same directory as Kernel Simulator.
- On Linux, you can run `mono KSConverter.exe`

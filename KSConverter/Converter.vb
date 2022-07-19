
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

Imports System.IO
Imports FluentFTP
Imports KS.ConsoleBase
Imports KS.Files.Querying
Imports KS.Misc.Configuration
Imports KS.Misc.Writers.FancyWriters
Imports KS.Misc.Writers.ConsoleWriters
Imports KS.Network
Imports KS.Network.RemoteDebug
Imports KS.Shell.ShellBase

Module Converter

    ''' <summary>
    ''' Main entry point
    ''' </summary>
    Sub Main()
        'Check console
        CheckConsole()

        Try
            'Initialize all needed variables
            Dim ListOfOldPaths = GetOldPaths("")
            Dim ListOfBackups = GetOldPaths("KSBackup")

            'Initialize paths
            InitPaths()

            'Load user token
            LoadUserToken()

            'Make backup directory
            WriteSeparator("[1/7] Making backup directory", True, ColTypes.Stage)
            Debug.WriteLine($"Backup directory: {GetHomeDirectory() + "/KSBackup"}")
            Debug.WriteLine($"FolderExists = {FolderExists(GetHomeDirectory() + "/KSBackup")}")
            If Not FolderExists(GetHomeDirectory() + "/KSBackup") Then
                'Just make it!
                Write("  - Backup directory not found. Creating directory...", True, ColTypes.Progress)
                Debug.WriteLine("Creating directory...")
                Directory.CreateDirectory(GetHomeDirectory() + "/KSBackup")
            Else
                'Directory found. Skip the creation.
                Debug.WriteLine("Already found.")
                Write("  - Warning: backup directory is already found.", True, ColTypes.Warning)
            End If
            Console.WriteLine()

            'Make backup of old configuration files in case something goes wrong during conversion.
            WriteSeparator("[2/7] Making backup of old configuration files", True, ColTypes.Stage)
            For Each ConfigEntry As String In ListOfOldPaths.Keys
                Debug.WriteLine($"Old path config entry: {ConfigEntry}")
                Debug.WriteLine($"Old path exists: {FileExists(ListOfOldPaths(ConfigEntry))}")
                Debug.WriteLine($"Backup exists: {FileExists(ListOfBackups(ConfigEntry))}")
                If FileExists(ListOfOldPaths(ConfigEntry)) And Not FileExists(ListOfBackups(ConfigEntry)) Then
                    'Move the old config file to backup
                    Debug.WriteLine($"Moving {ConfigEntry} from {ListOfOldPaths(ConfigEntry)} to {ListOfBackups(ConfigEntry)}...")
                    Write("  - {0}: {1} -> {2}", True, ColTypes.Neutral, ConfigEntry, ListOfOldPaths(ConfigEntry), ListOfBackups(ConfigEntry))
                    File.Move(ListOfOldPaths(ConfigEntry), ListOfBackups(ConfigEntry))
                ElseIf FileExists(ListOfBackups(ConfigEntry)) Then
                    Debug.WriteLine("We already have backup!")
                    Write("  - Warning: {0} already exists", True, ColTypes.Warning, ListOfBackups(ConfigEntry))
                Else
                    'File not found. Skip it.
                    Debug.WriteLine("We don't have config.")
                    Write("  - Warning: {0} not found in home directory.", True, ColTypes.Warning, ListOfOldPaths(ConfigEntry))
                End If
            Next
            Console.WriteLine()

            'Import all blocked devices to DebugDeviceNames.json
            WriteSeparator("[3/7] Importing all blocked devices to DebugDeviceNames.json", True, ColTypes.Stage)
            Debug.WriteLine($"Blocked device backup exists = {FileExists(ListOfBackups("BlockedDevices"))}")
            If FileExists(ListOfBackups("BlockedDevices")) Then
                'Read blocked devices from old file
                Write("  - Reading blocked devices from blocked_devices.csv...", True, ColTypes.Progress)
                Debug.WriteLine($"Calling File.ReadAllLines on {ListOfBackups("BlockedDevices")}...")
                Dim BlockedDevices As List(Of String) = File.ReadAllLines(ListOfBackups("BlockedDevices")).ToList
                Debug.WriteLine($"We have {BlockedDevices.Count} devices.")
                Write("  - {0} devices found.", True, ColTypes.Neutral, BlockedDevices.Count)

                'Add blocked devices to new format
                Debug.WriteLine($"Iterating {BlockedDevices.Count} blocked devices...")
                For Each BlockedDevice As String In BlockedDevices
                    Debug.WriteLine($"Adding blocked device {BlockedDevice} to the new config format...")
                    Write("  - Adding {0} to DebugDeviceNames.json...", True, ColTypes.Progress, BlockedDevice)
                    AddDeviceToJson(BlockedDevice, False)
                    SetDeviceProperty(BlockedDevice, DeviceProperty.Blocked, True)
                Next
            Else
                'File not found. Skip stage.
                Debug.WriteLine("We don't have file.")
                Write("  - Warning: blocked_devices.csv not found in home directory.", True, ColTypes.Warning)
            End If
            Console.WriteLine()

            'Import all FTP speed dial settings to JSON
            WriteSeparator("[4/7] Importing all FTP speed dial addresses to FTP_SpeedDial.json", True, ColTypes.Stage)
            Debug.WriteLine($"Speed dial addresses exists = {FileExists(ListOfBackups("FTPSpeedDial"))}")
            If FileExists(ListOfBackups("FTPSpeedDial")) Then
                'Read FTP speed dial addresses from old file
                Write("  - Reading FTP speed dial addresses from ftp_speeddial.csv...", True, ColTypes.Progress)
                Debug.WriteLine($"Calling File.ReadAllLines on {ListOfBackups("FTPSpeedDial")}...")
                Dim SpeedDialLines As String() = File.ReadAllLines(ListOfBackups("FTPSpeedDial"))
                Debug.WriteLine($"We have {SpeedDialLines.Length} addresses.")
                Write("  - {0} addresses found.", True, ColTypes.Neutral, SpeedDialLines.Length)

                'Add addresses to new format
                For Each SpeedDialLine As String In SpeedDialLines
                    'Populate variables
                    Dim ChosenLineSeparation As String() = SpeedDialLine.Split(",")
                    Debug.WriteLine($"Separation count = {ChosenLineSeparation.Length}")
                    Dim Address As String = ChosenLineSeparation(0)
                    Debug.WriteLine($"Address = {Address}")
                    Dim Port As String = ChosenLineSeparation(1)
                    Debug.WriteLine($"Port = {Port}")
                    Dim Username As String = ChosenLineSeparation(2)
                    Debug.WriteLine($"Username = {Username}")
                    Dim Encryption As FtpEncryptionMode = [Enum].Parse(GetType(FtpEncryptionMode), ChosenLineSeparation(3))
                    Debug.WriteLine($"Encryption = {Encryption}")

                    'Add the entry!
                    Write("  - Adding {0} to FTP_SpeedDial.json...", True, ColTypes.Progress, Address)
                    AddEntryToSpeedDial(Address, Port, Username, SpeedDialType.FTP, Encryption)
                Next
            Else
                'File not found. Skip stage.
                Debug.WriteLine("We don't have file.")
                Write("  - Warning: ftp_speeddial.csv not found in home directory.", True, ColTypes.Warning)
            End If
            Console.WriteLine()

            'Import all users to JSON
            WriteSeparator("[5/7] Importing all users to Users.json", True, ColTypes.Stage)
            Debug.WriteLine($"Users file exists = {FileExists(ListOfBackups("Users"))}")
            If FileExists(ListOfBackups("Users")) Then
                'Read all users from old file
                Write("  - Reading users from users.csv...", True, ColTypes.Progress)
                Debug.WriteLine($"Calling File.ReadAllLines on {ListOfBackups("Users")}...")
                Dim UsersLines As String() = File.ReadAllLines(ListOfBackups("Users"))
                Debug.WriteLine($"We have {UsersLines.Length} addresses.")
                Write("  - {0} users found.", True, ColTypes.Neutral, UsersLines.Length)

                'Add users to new format
                For Each UsersLine As String In UsersLines
                    'Populate variables
                    Debug.WriteLine($"Parsing line {UsersLine}...")
                    Dim ChosenLineSeparation As String() = UsersLine.Split(",")
                    Debug.WriteLine($"Separation count = {ChosenLineSeparation.Length}")
                    Dim Username As String = ChosenLineSeparation(0)
                    Debug.WriteLine($"Username = {Username}")
                    Dim Password As String = ChosenLineSeparation(1)
                    Debug.WriteLine($"Password = {Password}")
                    Dim Administrator As String = If(ChosenLineSeparation.Length >= 3, ChosenLineSeparation(2), "False")
                    Debug.WriteLine($"Administrator = {Administrator}")
                    Dim Disabled As String = If(ChosenLineSeparation.Length >= 4, ChosenLineSeparation(3), "False")
                    Debug.WriteLine($"Disabled = {Disabled}")
                    Dim Anonymous As String = If(ChosenLineSeparation.Length >= 5, ChosenLineSeparation(4), "False")
                    Debug.WriteLine($"Anonymous = {Anonymous}")

                    'Add the entry!
                    Write("  - Adding {0} to Users.json...", True, ColTypes.Progress, Username)
                    InitializeUser(Username, Password, False)
                    If Administrator = "True" Then
                        Debug.WriteLine($"Adding the Administrator permission to {Username}...")
                        AddPermission(PermissionType.Administrator, Username)
                    End If
                    If Disabled = "True" Then
                        Debug.WriteLine($"Adding the Disabled permission to {Username}...")
                        AddPermission(PermissionType.Disabled, Username)
                    End If
                    If Anonymous = "True" Then
                        Debug.WriteLine($"Adding the Anonymous permission to {Username}...")
                        AddPermission(PermissionType.Anonymous, Username)
                    End If
                Next
            Else
                'File not found. Skip stage.
                Debug.WriteLine("We don't have file.")
                Write("  - Warning: users.csv not found in home directory.", True, ColTypes.Warning)
            End If
            Console.WriteLine()

            'Import all aliases to JSON
            WriteSeparator("[6/7] Importing all aliases to Aliases.json", True, ColTypes.Stage)
            Debug.WriteLine($"Aliases file exists = {FileExists(ListOfBackups("Aliases"))}")
            If FileExists(ListOfBackups("Aliases")) Then
                'Read all aliases from old file
                Write("  - Reading users from aliases.csv...", True, ColTypes.Progress)
                Debug.WriteLine($"Calling File.ReadAllLines on {ListOfBackups("Aliases")}...")
                Dim AliasesLines As String() = File.ReadAllLines(ListOfBackups("Aliases"))
                Debug.WriteLine($"We have {AliasesLines.Length} aliases.")
                Write("  - {0} aliases found.", True, ColTypes.Neutral, AliasesLines.Length)

                'Add aliases to new format
                For Each AliasLine As String In AliasesLines
                    'POpulate variables
                    Dim AliasLineSplit() As String = AliasLine.Split({", "}, StringSplitOptions.RemoveEmptyEntries)
                    Debug.WriteLine($"Separation count = {AliasLineSplit.Length}")
                    Dim AliasCommand As String = AliasLineSplit(1)
                    Debug.WriteLine($"AliasCommand = {AliasCommand}")
                    Dim ActualCommand As String = AliasLineSplit(2)
                    Debug.WriteLine($"ActualCommand = {ActualCommand}")
                    Dim AliasType As String = AliasLineSplit(0)
                    Debug.WriteLine($"AliasType = {AliasType}")

                    'Add the entry!
                    Write("  - Adding {0} to Aliases.json...", True, ColTypes.Progress, AliasCommand)
                    Select Case AliasType
                        Case "Shell"
                            If Not Aliases.ContainsKey(AliasCommand) Then
                                Debug.WriteLine($"Adding alias {AliasCommand}...")
                                Aliases.Add(AliasCommand, ActualCommand)
                            End If
                        Case "Remote"
                            If Not RemoteDebugAliases.ContainsKey(AliasCommand) Then
                                Debug.WriteLine($"Adding alias {AliasCommand}...")
                                RemoteDebugAliases.Add(AliasCommand, ActualCommand)
                            End If
                        Case "FTPShell"
                            If Not FTPShellAliases.ContainsKey(AliasCommand) Then
                                Debug.WriteLine($"Adding alias {AliasCommand}...")
                                FTPShellAliases.Add(AliasCommand, ActualCommand)
                            End If
                        Case "SFTPShell"
                            If Not SFTPShellAliases.ContainsKey(AliasCommand) Then
                                Debug.WriteLine($"Adding alias {AliasCommand}...")
                                SFTPShellAliases.Add(AliasCommand, ActualCommand)
                            End If
                        Case "Mail"
                            If Not MailShellAliases.ContainsKey(AliasCommand) Then
                                Debug.WriteLine($"Adding alias {AliasCommand}...")
                                MailShellAliases.Add(AliasCommand, ActualCommand)
                            End If
                        Case Else
                            Write("  - Invalid type {0}", True, ColTypes.Error, AliasType)
                    End Select
                Next

                'Save the changes
                Debug.WriteLine("Saving...")
                Write("  - Saving aliases to Aliases.json...", True, ColTypes.Progress)
                SaveAliases()
            Else
                'File not found. Skip stage.
                Debug.WriteLine("We don't have file.")
                Write("  - Warning: aliases.csv not found in home directory.", True, ColTypes.Warning)
            End If
            Console.WriteLine()

            'Import all config to JSON
            WriteSeparator("[7/7] Importing all kernel config to KernelConfig.json", True, ColTypes.Stage)
            Debug.WriteLine($"Config file exists = {FileExists(ListOfBackups("Configuration"))}")
            If FileExists(ListOfBackups("Configuration")) Then
#If Not NETCOREAPP Then
                'Read all config from old file
                Write("  - Reading config from kernelConfig.ini...", True, ColTypes.Progress)
                Debug.WriteLine("Reading configuration...")
                If Not ReadPreFivePointFiveConfig(ListOfBackups("Configuration")) Then
                    If Not ReadFivePointFiveConfig(ListOfBackups("Configuration")) Then
                        Debug.WriteLine("Incompatible format. Both ReadPreFivePointFiveConfig and ReadFivePointFiveConfig returned False. Regenerating...")
                        Write("  - Warning: kernelConfig.ini has incompatible format. Generating new config anyways...", True, ColTypes.Warning)
                    End If
                End If

                'Save the changes
                Debug.WriteLine("Saving...")
                Write("  - Saving configuration to KernelConfig.json...", True, ColTypes.Progress)
                CreateConfig()
#Else
                'We need to use .NET Framework version of KSConverter to be able to fully use MadMilkman.Ini
                'as we used it back when Kernel Simulator and MadMilkman.Ini were so tied to .NET Framework.
                Debug.WriteLine("Config conversion needs to be done in .NET Framework.")
                Write("  - Warning: To convert kernel configuration from kernelConfig.ini to KernelConfig.json, you need to run the .NET Framework version of KSConverter.", True, ColTypes.Warning)
#End If
            Else
                'File not found. Skip stage.
                Debug.WriteLine("We don't have file.")
                Write("  - Warning: kernelConfig.ini not found in home directory.", True, ColTypes.Warning)
            End If
            Console.WriteLine()

            'Print this message:
            Write("- Successfully converted all settings to new format! Enjoy!", True, ColTypes.Success)
            Write("- Press any key to exit.", True, ColTypes.Success)
            Console.ReadKey(True)
        Catch ex As Exception
            Write("- Error converting settings: {0}", True, ColTypes.Error, ex.Message)
            Write("- Press any key to exit. Stack trace below:", True, ColTypes.Error)
            Write(ex.StackTrace, True, ColTypes.Neutral)
            Console.ReadKey(True)
        End Try
    End Sub

End Module

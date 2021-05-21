
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

Imports KS.TextWriterColor
Imports KS.ColorTools
Imports KS.Kernel
Imports KS.KernelTools
Imports KS.RemoteDebugTools
Imports KS.NetworkTools
Imports KS.AliasManager
Imports KS.UserManagement
Imports KS.PermissionManagement
Imports System.IO

Module Converter

    'FIXME: This program is WIP, so the main entry point looks unfinished.
    ''' <summary>
    ''' Main entry point
    ''' </summary>
    Sub Main()
        'Check for terminal (macOS only). This check is needed because we have the stock Terminal.app (Apple_Terminal according to $TERM_PROGRAM) that
        'has incompatibilities with VT sequences, causing broken display. It claims it supports XTerm, yet it isn't fully XTerm-compliant, so we exit
        'the program early when this stock terminal is spotted.
#If STOCKTERMINALMACOS = False Then
        If IsOnMacOS() Then
            If GetTerminalEmulator() = "Apple_Terminal" Then
                Console.WriteLine("Kernel Simulator makes use of VT escape sequences, but Terminal.app has broken support for 255 and true colors. This program can't continue.")
                Environment.Exit(5)
            End If
        End If
#End If

        'Initialize all needed variables
        Dim ListOfOldPaths = GetOldPaths("")
        Dim ListOfBackups = GetOldPaths("KSBackup")

        'Initialize paths
        InitPaths()

        'Load user token
        LoadUserToken()

        'Make backup directory
        W("- Making backup directory...", True, ColTypes.Stage)
        If Not Directory.Exists(GetHomeDirectory() + "/KSBackup") Then
            'Just make it!
            W("  - Backup directory not found. Creating directory...", True, ColTypes.Neutral)
            Directory.CreateDirectory(GetHomeDirectory() + "/KSBackup")
        Else
            'Directory found. Skip the creation.
            W("  - Warning: backup directory is already found.", True, ColTypes.Warning)
        End If
        Console.WriteLine()

        'Make backup of old configuration files in case something goes wrong during conversion.
        W("- Making backup of old configuration files...", True, ColTypes.Stage)
        For Each ConfigEntry As String In ListOfOldPaths.Keys
            If File.Exists(ListOfOldPaths(ConfigEntry)) Then
                'Move the old config file to backup
                W("  - {0}: {1} -> {2}", True, ColTypes.Neutral, ConfigEntry, ListOfOldPaths(ConfigEntry), ListOfBackups(ConfigEntry))
                File.Move(ListOfOldPaths(ConfigEntry), ListOfBackups(ConfigEntry))
            Else
                'File not found. Skip it.
                W("  - Warning: {0} not found in home directory.", True, ColTypes.Warning, ListOfOldPaths(ConfigEntry))
            End If
        Next
        Console.WriteLine()

        'Import all blocked devices to DebugDeviceNames.json
        W("- Importing all blocked devices to DebugDeviceNames.json...", True, ColTypes.Stage)
        If File.Exists(ListOfBackups("BlockedDevices")) Then
            'Read blocked devices from old file
            W("  - Reading blocked devices from blocked_devices.csv...", True, ColTypes.Neutral)
            Dim BlockedDevices As List(Of String) = File.ReadAllLines(ListOfBackups("BlockedDevices")).ToList
            W("  - {0} devices found.", True, ColTypes.Neutral, BlockedDevices.Count)

            'Add blocked devices to new format
            For Each BlockedDevice As String In BlockedDevices
                W("  - Adding {0} to DebugDeviceNames.json...", True, ColTypes.Neutral, BlockedDevice)
                AddDeviceToJson(BlockedDevice, False)
                SetDeviceProperty(BlockedDevice, DeviceProperty.Blocked, True)
            Next
        Else
            'File not found. Skip stage.
            W("  - Warning: blocked_devices.csv not found in home directory.", True, ColTypes.Warning)
        End If
        Console.WriteLine()

        'Import all FTP speed dial settings to JSON
        W("- Importing all FTP speed dial addresses to FTP_SpeedDial.json...", True, ColTypes.Stage)
        If File.Exists(ListOfBackups("FTPSpeedDial")) Then
            'Read FTP speed dial addresses from old file
            W("  - Reading FTP speed dial addresses from ftp_speeddial.csv...", True, ColTypes.Neutral)
            Dim SpeedDialLines As String() = File.ReadAllLines(ListOfBackups("FTPSpeedDial"))
            W("  - {0} addresses found.", True, ColTypes.Neutral, SpeedDialLines.Count)

            'Add addresses to new format
            For Each SpeedDialLine As String In SpeedDialLines
                W("  - Adding {0} to FTP_SpeedDial.json...", True, ColTypes.Neutral, SpeedDialLine.Split(",")(0))
                AddEntryToSpeedDial(SpeedDialLine, SpeedDialType.FTP)
            Next
        Else
            'File not found. Skip stage.
            W("  - Warning: ftp_speeddial.csv not found in home directory.", True, ColTypes.Warning)
        End If
        Console.WriteLine()

        'Import all users to JSON
        W("- Importing all users to Users.json...", True, ColTypes.Stage)
        If File.Exists(ListOfBackups("Users")) Then
            'Read all users from old file
            W("  - Reading users from users.csv...", True, ColTypes.Neutral)
            Dim UsersLines As String() = File.ReadAllLines(ListOfBackups("Users"))
            W("  - {0} users found.", True, ColTypes.Neutral, UsersLines.Count)

            'Add users to new format
            For Each UsersLine As String In UsersLines
                W("  - Adding {0} to Users.json...", True, ColTypes.Neutral, UsersLine.Split(",")(0))
                InitializeUser(UsersLine.Split(",")(0), UsersLine.Split(",")(1), False)
                If UsersLine.Split(",")(2) = "True" Then
                    AddPermission(PermissionType.Administrator, UsersLine.Split(",")(0))
                End If
                If UsersLine.Split(",")(3) = "True" Then
                    AddPermission(PermissionType.Disabled, UsersLine.Split(",")(0))
                End If
                If UsersLine.Split(",")(4) = "True" Then
                    AddPermission(PermissionType.Anonymous, UsersLine.Split(",")(0))
                End If
            Next
        Else
            'File not found. Skip stage.
            W("  - Warning: users.csv not found in home directory.", True, ColTypes.Warning)
        End If
        Console.WriteLine()

        'Import all aliases to JSON
        W("- Importing all aliases to Aliases.json...", True, ColTypes.Stage)
        If File.Exists(ListOfBackups("Aliases")) Then
            'Read all aliases from old file
            W("  - Reading users from aliases.csv...", True, ColTypes.Neutral)
            Dim AliasesLines As String() = File.ReadAllLines(ListOfBackups("Aliases"))
            W("  - {0} aliases found.", True, ColTypes.Neutral, AliasesLines.Count)

            'Add aliases to new format
            For Each AliasLine As String In AliasesLines
                Dim AliasLineSplit() As String = AliasLine.Split({", "}, StringSplitOptions.RemoveEmptyEntries)
                Dim AliasCommand As String = AliasLineSplit(1)
                Dim ActualCommand As String = AliasLineSplit(2)
                Dim AliasType As String = AliasLineSplit(0)
                W("  - Adding {0} to Aliases.json...", True, ColTypes.Neutral, AliasCommand)
                Select Case AliasType
                    Case "Shell"
                        If Not Aliases.ContainsKey(AliasCommand) Then
                            Aliases.Add(AliasCommand, ActualCommand)
                        End If
                    Case "Remote"
                        If Not RemoteDebugAliases.ContainsKey(AliasCommand) Then
                            RemoteDebugAliases.Add(AliasCommand, ActualCommand)
                        End If
                    Case "FTPShell"
                        If Not FTPShellAliases.ContainsKey(AliasCommand) Then
                            FTPShellAliases.Add(AliasCommand, ActualCommand)
                        End If
                    Case "SFTPShell"
                        If Not SFTPShellAliases.ContainsKey(AliasCommand) Then
                            SFTPShellAliases.Add(AliasCommand, ActualCommand)
                        End If
                    Case "Mail"
                        If Not MailShellAliases.ContainsKey(AliasCommand) Then
                            MailShellAliases.Add(AliasCommand, ActualCommand)
                        End If
                    Case Else
                        W("  - Invalid type {0}", True, ColTypes.Err, AliasType)
                End Select
            Next
            W("  - Saving aliases to Aliases.json...", True, ColTypes.Neutral)
            SaveAliases()
        Else
            'File not found. Skip stage.
            W("  - Warning: aliases.csv not found in home directory.", True, ColTypes.Warning)
        End If
        Console.WriteLine()

#If Not CONFIGNOTDONE Then
        'Import all config to JSON
        W("- Importing all kernel config to KernelConfig.json...", True, ColTypes.Stage)
        If File.Exists(ListOfBackups("Configuration")) Then
            'Read all config from old file
            W("  - Reading config from kernelConfig.ini...", True, ColTypes.Neutral)
            'TODO: Make config conversion here.
        Else
            'File not found. Skip stage.
            W("  - Warning: kernelConfig.ini not found in home directory.", True, ColTypes.Warning)
        End If
        Console.WriteLine()

        W("- That's far as we got. Enjoy!", True, ColTypes.Stage)
        W("- Press any key to exit.", True, ColTypes.Stage)
        Console.ReadKey(True)
#End If

    End Sub

    ''' <summary>
    ''' Gets all paths that are used in 0.0.15.x or earlier kernels.
    ''' </summary>
    ''' <param name="AppendedPath">Optional path to append</param>
    Function GetOldPaths(ByVal AppendedPath As String) As Dictionary(Of String, String)
        'Initialize all needed variables
        Dim OldPaths As New Dictionary(Of String, String)

        'Check to see if we're appending new path name
        If Not String.IsNullOrEmpty(AppendedPath) Then AppendedPath = $"/{AppendedPath}"

        'Populate our dictionary with old paths
        If IsOnUnix() Then
#If CONFIGNOTDONE = False Then
            OldPaths.Add("Configuration", Environ("HOME") + $"{AppendedPath}/kernelConfig.ini")
#End If
            OldPaths.Add("Aliases", Environ("HOME") + $"{AppendedPath}/aliases.csv")
            OldPaths.Add("Users", Environ("HOME") + $"{AppendedPath}/users.csv")
            OldPaths.Add("FTPSpeedDial", Environ("HOME") + $"{AppendedPath}/ftp_speeddial.csv")
            OldPaths.Add("BlockedDevices", Environ("HOME") + $"{AppendedPath}/blocked_devices.csv")
        Else
#If CONFIGNOTDONE = False Then
            OldPaths.Add("Configuration", Environ("USERPROFILE").Replace("\", "/") + $"{AppendedPath}/kernelConfig.ini")
#End If
            OldPaths.Add("Aliases", Environ("USERPROFILE").Replace("\", "/") + $"{AppendedPath}/aliases.csv")
            OldPaths.Add("Users", Environ("USERPROFILE").Replace("\", "/") + $"{AppendedPath}/users.csv")
            OldPaths.Add("FTPSpeedDial", Environ("USERPROFILE").Replace("\", "/") + $"{AppendedPath}/ftp_speeddial.csv")
            OldPaths.Add("BlockedDevices", Environ("USERPROFILE").Replace("\", "/") + $"{AppendedPath}/blocked_devices.csv")
        End If

        'Return it.
        Return OldPaths
    End Function

    ''' <summary>
    ''' Gets all paths that are used in 0.0.16.x or later kernels.
    ''' </summary>
    Function GetNewPaths() As Dictionary(Of String, String)
        'Initialize all needed variables
        Dim NewPaths As New Dictionary(Of String, String)

        'Populate our dictionary with old paths
        If IsOnUnix() Then
#If CONFIGNOTDONE = False Then
            NewPaths.Add("Configuration", Environ("HOME") + "/KernelConfig.json")
#End If
            NewPaths.Add("Aliases", Environ("HOME") + "/Aliases.json")
            NewPaths.Add("Users", Environ("HOME") + "/Users.json")
            NewPaths.Add("FTPSpeedDial", Environ("HOME") + "/FTP_SpeedDial.json")
            NewPaths.Add("DebugDevNames", Environ("USERPROFILE").Replace("\", "/") + "/DebugDeviceNames.json")
        Else
#If CONFIGNOTDONE = False Then
            NewPaths.Add("Configuration", Environ("USERPROFILE").Replace("\", "/") + "/KernelConfig.json")
#End If
            NewPaths.Add("Aliases", Environ("USERPROFILE").Replace("\", "/") + "/Aliases.json")
            NewPaths.Add("Users", Environ("USERPROFILE").Replace("\", "/") + "/Users.json")
            NewPaths.Add("FTPSpeedDial", Environ("USERPROFILE").Replace("\", "/") + "/FTP_SpeedDial.json")
            NewPaths.Add("DebugDevNames", Environ("USERPROFILE").Replace("\", "/") + "/DebugDeviceNames.json")
        End If

        'Return it.
        Return NewPaths
    End Function

    ''' <summary>
    ''' Gets home directory depending on platform
    ''' </summary>
    Function GetHomeDirectory() As String
        If IsOnUnix() Then
            Return Environ("HOME")
        Else
            Return Environ("USERPROFILE")
        End If
    End Function

End Module

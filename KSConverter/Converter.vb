
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
Imports KS.Color
Imports KS.Kernel
Imports KS.RemoteDebugTools
Imports KS.NetworkTools
Imports System.IO

Module Converter

    'FIXME: This program is WIP, so the main entry point looks unfinished.
    ''' <summary>
    ''' Main entry point
    ''' </summary>
    Sub Main()
        'Initialize all needed variables
        Dim ListOfOldPaths = GetOldPaths("")
        Dim ListOfBackups = GetOldPaths("KSBackup")

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
#If ALIASESNOTDONE = False Then
            OldPaths.Add("Aliases", Environ("HOME") + $"{AppendedPath}/aliases.csv")
#End If
#If USERSNOTDONE = False Then
            OldPaths.Add("Users", Environ("HOME") + $"{AppendedPath}/users.csv")
#End If
            OldPaths.Add("FTPSpeedDial", Environ("HOME") + $"{AppendedPath}/ftp_speeddial.csv")
            OldPaths.Add("BlockedDevices", Environ("HOME") + $"{AppendedPath}/blocked_devices.csv")
        Else
#If CONFIGNOTDONE = False Then
            OldPaths.Add("Configuration", Environ("USERPROFILE").Replace("\", "/") + $"{AppendedPath}/kernelConfig.ini")
#End If
#If ALIASESNOTDONE = False Then
            OldPaths.Add("Aliases", Environ("USERPROFILE").Replace("\", "/") + $"{AppendedPath}/aliases.csv")
#End If
#If USERSNOTDONE = False Then
            OldPaths.Add("Users", Environ("USERPROFILE").Replace("\", "/") + $"{AppendedPath}/users.csv")
#End If
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
#If ALIASESNOTDONE = False Then
            NewPaths.Add("Aliases", Environ("HOME") + "/Aliases.json")
#End If
#If USERSNOTDONE = False Then
            NewPaths.Add("Users", Environ("HOME") + "/Users.json")
#End If
            NewPaths.Add("FTPSpeedDial", Environ("HOME") + "/FTP_SpeedDial.json")
            NewPaths.Add("DebugDevNames", Environ("USERPROFILE").Replace("\", "/") + "/DebugDeviceNames.json")
        Else
#If CONFIGNOTDONE = False Then
            NewPaths.Add("Configuration", Environ("USERPROFILE").Replace("\", "/") + "/KernelConfig.json")
#End If
#If ALIASESNOTDONE = False Then
            NewPaths.Add("Aliases", Environ("USERPROFILE").Replace("\", "/") + "/Aliases.json")
#End If
#If USERSNOTDONE = False Then
            NewPaths.Add("Users", Environ("USERPROFILE").Replace("\", "/") + "/Users.json")
#End If
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

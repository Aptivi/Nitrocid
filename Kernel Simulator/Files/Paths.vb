
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Public Module Paths

    Friend KernelPaths As New Dictionary(Of String, String)
    Friend OtherPaths As New Dictionary(Of String, String)

    ''' <summary>
    ''' Initializes the paths
    ''' </summary>
    Sub InitPaths()
        If IsOnUnix() Then
            KernelPaths.AddIfNotFound("Mods", Environment.GetEnvironmentVariable("HOME") + "/KSMods/")
            KernelPaths.AddIfNotFound("Configuration", Environment.GetEnvironmentVariable("HOME") + "/KernelConfig.json")
            KernelPaths.AddIfNotFound("Debugging", Environment.GetEnvironmentVariable("HOME") + "/kernelDbg.log")
            KernelPaths.AddIfNotFound("Aliases", Environment.GetEnvironmentVariable("HOME") + "/Aliases.json")
            KernelPaths.AddIfNotFound("Users", Environment.GetEnvironmentVariable("HOME") + "/Users.json")
            KernelPaths.AddIfNotFound("FTPSpeedDial", Environment.GetEnvironmentVariable("HOME") + "/FTP_SpeedDial.json")
            KernelPaths.AddIfNotFound("SFTPSpeedDial", Environment.GetEnvironmentVariable("HOME") + "/SFTP_SpeedDial.json")
            KernelPaths.AddIfNotFound("DebugDevNames", Environment.GetEnvironmentVariable("HOME") + "/DebugDeviceNames.json")
            KernelPaths.AddIfNotFound("MOTD", Environment.GetEnvironmentVariable("HOME") + "/MOTD.txt")
            KernelPaths.AddIfNotFound("MAL", Environment.GetEnvironmentVariable("HOME") + "/MAL.txt")
            KernelPaths.AddIfNotFound("CustomSaverSettings", Environment.GetEnvironmentVariable("HOME") + "/CustomSaverSettings.json")
            KernelPaths.AddIfNotFound("Events", Environment.GetEnvironmentVariable("HOME") + "/KSEvents/")
            KernelPaths.AddIfNotFound("Reminders", Environment.GetEnvironmentVariable("HOME") + "/KSReminders/")
            OtherPaths.AddIfNotFound("Home", Environment.GetEnvironmentVariable("HOME"))
            OtherPaths.AddIfNotFound("Temp", "/tmp")
        Else
            KernelPaths.AddIfNotFound("Mods", Environment.GetEnvironmentVariable("USERPROFILE") + "/KSMods/")
            KernelPaths.AddIfNotFound("Configuration", Environment.GetEnvironmentVariable("USERPROFILE") + "/KernelConfig.json")
            KernelPaths.AddIfNotFound("Debugging", Environment.GetEnvironmentVariable("USERPROFILE") + "/kernelDbg.log")
            KernelPaths.AddIfNotFound("Aliases", Environment.GetEnvironmentVariable("USERPROFILE") + "/Aliases.json")
            KernelPaths.AddIfNotFound("Users", Environment.GetEnvironmentVariable("USERPROFILE") + "/Users.json")
            KernelPaths.AddIfNotFound("FTPSpeedDial", Environment.GetEnvironmentVariable("USERPROFILE") + "/FTP_SpeedDial.json")
            KernelPaths.AddIfNotFound("SFTPSpeedDial", Environment.GetEnvironmentVariable("USERPROFILE") + "/SFTP_SpeedDial.json")
            KernelPaths.AddIfNotFound("DebugDevNames", Environment.GetEnvironmentVariable("USERPROFILE") + "/DebugDeviceNames.json")
            KernelPaths.AddIfNotFound("MOTD", Environment.GetEnvironmentVariable("USERPROFILE") + "/MOTD.txt")
            KernelPaths.AddIfNotFound("MAL", Environment.GetEnvironmentVariable("USERPROFILE") + "/MAL.txt")
            KernelPaths.AddIfNotFound("CustomSaverSettings", Environment.GetEnvironmentVariable("USERPROFILE") + "/CustomSaverSettings.json")
            KernelPaths.AddIfNotFound("Events", Environment.GetEnvironmentVariable("USERPROFILE") + "/KSEvents/")
            KernelPaths.AddIfNotFound("Reminders", Environment.GetEnvironmentVariable("USERPROFILE") + "/KSReminders/")
            OtherPaths.AddIfNotFound("Home", Environment.GetEnvironmentVariable("USERPROFILE"))
            OtherPaths.AddIfNotFound("Temp", Environment.GetEnvironmentVariable("TEMP"))
        End If
    End Sub

    ''' <summary>
    ''' Gets the neutralized kernel path
    ''' </summary>
    ''' <param name="PathType">Kernel path type</param>
    ''' <returns>A kernel path</returns>
    ''' <exception cref="Exceptions.InvalidKernelPathException"></exception>
    Public Function GetKernelPath(PathType As KernelPathType) As String
        If [Enum].IsDefined(GetType(KernelPathType), PathType) Then
            Return NeutralizePath(KernelPaths(PathType.ToString))
        Else
            Throw New Exceptions.InvalidKernelPathException(DoTranslation("Invalid kernel path type."))
        End If
    End Function

    ''' <summary>
    ''' Gets the neutralized other path
    ''' </summary>
    ''' <param name="PathType">Path type</param>
    ''' <returns>An "other" path</returns>
    ''' <exception cref="Exceptions.InvalidKernelPathException"></exception>
    Public Function GetOtherPath(PathType As OtherPathType) As String
        If [Enum].IsDefined(GetType(OtherPathType), PathType) Then
            Return NeutralizePath(OtherPaths(PathType.ToString))
        Else
            Throw New Exceptions.InvalidPathException(DoTranslation("Invalid path type."))
        End If
    End Function

End Module

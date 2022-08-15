
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

Imports System.Reflection

Namespace Files
    Public Module Paths

        'Basic paths

        ''' <summary>
        ''' Path to KS executable folder
        ''' </summary>
        Public ReadOnly Property ExecPath As String
            Get
                Return IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            End Get
        End Property

        ''' <summary>
        ''' Platform-dependent home path
        ''' </summary>
        Public ReadOnly Property HomePath As String
            Get
                If IsOnUnix() Then
                    Return Environment.GetEnvironmentVariable("HOME")
                Else
                    Return Environment.GetEnvironmentVariable("USERPROFILE").Replace("\", "/")
                End If
            End Get
        End Property

        ''' <summary>
        ''' Platform-dependent temp path
        ''' </summary>
        Public ReadOnly Property TempPath As String
            Get
                If IsOnUnix() Then
                    Return "/tmp"
                Else
                    Return Environment.GetEnvironmentVariable("TEMP").Replace("\", "/")
                End If
            End Get
        End Property

        ''' <summary>
        ''' Retro Kernel Simulator download path
        ''' </summary>
        Public ReadOnly Property RetroKSDownloadPath As String
            Get
#If NETCOREAPP Then
                If IsOnUnix() Then
                    Return Environment.GetEnvironmentVariable("HOME") + "/.config/retroks/exec/coreclr"
                Else
                    Return (Environment.GetEnvironmentVariable("LOCALAPPDATA") + "/RetroKS/exec/coreclr").Replace("\", "/")
                End If
#Else
                If IsOnUnix() Then
                    Return Environment.GetEnvironmentVariable("HOME") + "/.config/retroks/exec/fx"
                Else
                    Return (Environment.GetEnvironmentVariable("LOCALAPPDATA") + "/RetroKS/exec/fx").Replace("\", "/")
                End If
#End If
            End Get
        End Property

        'Kernel Simulator paths

        ''' <summary>
        ''' Mods path
        ''' </summary>
        Public ReadOnly Property ModsPath As String
            Get
                Return NeutralizePath(HomePath + "/KSMods/")
            End Get
        End Property

        ''' <summary>
        ''' Configuration path
        ''' </summary>
        Public ReadOnly Property ConfigurationPath As String
            Get
                Return NeutralizePath(HomePath + "/KernelConfig.json")
            End Get
        End Property

        ''' <summary>
        ''' Debugging path
        ''' </summary>
        Public ReadOnly Property DebuggingPath As String
            Get
                Return NeutralizePath(HomePath + "/kernelDbg.log")
            End Get
        End Property

        ''' <summary>
        ''' Aliases path
        ''' </summary>
        Public ReadOnly Property AliasesPath As String
            Get
                Return NeutralizePath(HomePath + "/Aliases.json")
            End Get
        End Property

        ''' <summary>
        ''' Users path
        ''' </summary>
        Public ReadOnly Property UsersPath As String
            Get
                Return NeutralizePath(HomePath + "/Users.json")
            End Get
        End Property

        ''' <summary>
        ''' FTPSpeedDial path
        ''' </summary>
        Public ReadOnly Property FTPSpeedDialPath As String
            Get
                Return NeutralizePath(HomePath + "/FTP_SpeedDial.json")
            End Get
        End Property

        ''' <summary>
        ''' SFTPSpeedDial path
        ''' </summary>
        Public ReadOnly Property SFTPSpeedDialPath As String
            Get
                Return NeutralizePath(HomePath + "/SFTP_SpeedDial.json")
            End Get
        End Property

        ''' <summary>
        ''' DebugDevNames path
        ''' </summary>
        Public ReadOnly Property DebugDevNamesPath As String
            Get
                Return NeutralizePath(HomePath + "/DebugDeviceNames.json")
            End Get
        End Property

        ''' <summary>
        ''' MOTD path
        ''' </summary>
        Public ReadOnly Property MOTDPath As String
            Get
                Return NeutralizePath(HomePath + "/MOTD.txt")
            End Get
        End Property

        ''' <summary>
        ''' MAL path
        ''' </summary>
        Public ReadOnly Property MALPath As String
            Get
                Return NeutralizePath(HomePath + "/MAL.txt")
            End Get
        End Property

        ''' <summary>
        ''' CustomSaverSettings path
        ''' </summary>
        Public ReadOnly Property CustomSaverSettingsPath As String
            Get
                Return NeutralizePath(HomePath + "/CustomSaverSettings.json")
            End Get
        End Property

        ''' <summary>
        ''' Events path
        ''' </summary>
        Public ReadOnly Property EventsPath As String
            Get
                Return NeutralizePath(HomePath + "/KSEvents/")
            End Get
        End Property

        ''' <summary>
        ''' Reminders path
        ''' </summary>
        Public ReadOnly Property RemindersPath As String
            Get
                Return NeutralizePath(HomePath + "/KSReminders/")
            End Get
        End Property

        ''' <summary>
        ''' CustomLanguages path
        ''' </summary>
        Public ReadOnly Property CustomLanguagesPath As String
            Get
                Return NeutralizePath(HomePath + "/KSLanguages/")
            End Get
        End Property

        ''' <summary>
        ''' CustomSplashes path
        ''' </summary>
        Public ReadOnly Property CustomSplashesPath As String
            Get
                Return NeutralizePath(HomePath + "/KSSplashes/")
            End Get
        End Property

        ''' <summary>
        ''' Gets the neutralized kernel path
        ''' </summary>
        ''' <param name="PathType">Kernel path type</param>
        ''' <returns>A kernel path</returns>
        ''' <exception cref="Exceptions.InvalidKernelPathException"></exception>
        Public Function GetKernelPath(PathType As KernelPathType) As String
            Select Case PathType
                Case KernelPathType.Aliases
                    Return AliasesPath
                Case KernelPathType.Configuration
                    Return ConfigurationPath
                Case KernelPathType.CustomLanguages
                    Return CustomLanguagesPath
                Case KernelPathType.CustomSaverSettings
                    Return CustomSaverSettingsPath
                Case KernelPathType.CustomSplashes
                    Return CustomSplashesPath
                Case KernelPathType.DebugDevNames
                    Return DebugDevNamesPath
                Case KernelPathType.Debugging
                    Return DebuggingPath
                Case KernelPathType.Events
                    Return EventsPath
                Case KernelPathType.FTPSpeedDial
                    Return FTPSpeedDialPath
                Case KernelPathType.MAL
                    Return MALPath
                Case KernelPathType.Mods
                    Return ModsPath
                Case KernelPathType.MOTD
                    Return MOTDPath
                Case KernelPathType.Reminders
                    Return RemindersPath
                Case KernelPathType.SFTPSpeedDial
                    Return SFTPSpeedDialPath
                Case KernelPathType.Users
                    Return UsersPath
                Case Else
                    Throw New Exceptions.InvalidKernelPathException(DoTranslation("Invalid kernel path type."))
            End Select
        End Function

    End Module
End Namespace

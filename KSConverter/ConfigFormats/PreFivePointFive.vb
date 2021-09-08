
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

Imports KS

Module PreFivePointFive

    'Pre-0.0.5.5 config format has "Kernel Version" as the first line.
    ''' <summary>
    ''' Takes configuration values and installs them to appropriate variables. Taken from config.vb version 0.0.5.2 with some removals that reflect this version
    ''' </summary>
    ''' <param name="PathToConfig">Path to pre-0.0.5.5 config (kernelConfig.ini)</param>
    Function ReadPreFivePointFiveConfig(PathToConfig As String) As Boolean
        Dim OldConfigReader As New IO.StreamReader(PathToConfig)
        Dim line As String = OldConfigReader.ReadLine
        Dim ValidFormat As Boolean
        Do While line <> ""
            If line.Contains("Kernel Version = ") Then
                ValidFormat = True
            End If
            If ValidFormat Then
                If line.Contains("Colored Shell = ") Then
                    If line.Replace("Colored Shell = ", "") = "False" Then
                        ColoredShell = False
                    End If
                End If
                If line.Contains("User Name Shell Color = ") Then
                    If ColoredShell = True Then UserNameShellColor = Val(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("User Name Shell Color = ", "")), ConsoleColors))
                ElseIf line.Contains("Host Name Shell Color = ") Then
                    If ColoredShell = True Then HostNameShellColor = Val(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("Host Name Shell Color = ", "")), ConsoleColors))
                ElseIf line.Contains("Continuable Kernel Error Color = ") Then
                    If ColoredShell = True Then ContKernelErrorColor = Val(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("Continuable Kernel Error Color = ", "")), ConsoleColors))
                ElseIf line.Contains("Uncontinuable Kernel Error Color = ") Then
                    If ColoredShell = True Then UncontKernelErrorColor = Val(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("Uncontinuable Kernel Error Color = ", "")), ConsoleColors))
                ElseIf line.Contains("Text Color = ") Then
                    If ColoredShell = True Then NeutralTextColor = Val(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("Text Color = ", "")), ConsoleColors))
                ElseIf line.Contains("License Color = ") Then
                    If ColoredShell = True Then LicenseColor = Val(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("License Color = ", "")), ConsoleColors))
                ElseIf line.Contains("Background Color = ") Then
                    If ColoredShell = True Then
                        BackgroundColor = Val(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("Background Color = ", "")), ConsoleColors))
                    End If
                ElseIf line.Contains("Input Color = ") Then
                    If ColoredShell = True Then InputColor = Val(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("Input Color = ", "")), ConsoleColors))
                ElseIf line.Contains("Listed command in Help Color = ") Then
                    If ColoredShell = True Then ListEntryColor = Val(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("Listed command in Help Color = ", "")), ConsoleColors))
                ElseIf line.Contains("Definition of command in Help Color = ") Then
                    If ColoredShell = True Then ListValueColor = Val(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("Definition of command in Help Color = ", "")), ConsoleColors))
                ElseIf line.Contains("Change Root Password = ") Then
                    If line.Replace("Change Root Password = ", "") = "True" Then
                        setRootPasswd = True
                    ElseIf line.Replace("Change Root Password = ", "") = "False" Then
                        setRootPasswd = False
                    End If
                ElseIf line.Contains("Set Root Password to = ") Then
                    If setRootPasswd = True Then
                        RootPasswd = line.Replace("Set Root Password to = ", "")
                    End If
                ElseIf line.Contains("Maintenance Mode = ") Then
                    If line.Replace("Maintenance Mode = ", "") = "True" Then
                        maintenance = True
                    ElseIf line.Replace("Maintenance Mode = ", "") = "False" Then
                        maintenance = False
                    End If
                ElseIf line.Contains("Prompt for Arguments on Boot = ") Then
                    If line.Replace("Prompt for Arguments on Boot = ", "") = "True" Then
                        argsOnBoot = True
                    ElseIf line.Replace("Prompt for Arguments on Boot = ", "") = "False" Then
                        argsOnBoot = False
                    End If
                ElseIf line.Contains("Clear Screen on Log-in = ") Then
                    If line.Replace("Clear Screen on Log-in = ", "") = "True" Then
                        clsOnLogin = True
                    ElseIf line.Replace("Clear Screen on Log-in = ", "") = "False" Then
                        clsOnLogin = False
                    End If
                ElseIf line.Contains("Show MOTD on Log-in = ") Then
                    If line.Replace("Show MOTD on Log-in = ", "") = "True" Then
                        showMOTD = True
                    ElseIf line.Replace("Show MOTD on Log-in = ", "") = "False" Then
                        showMOTD = False
                    End If
                ElseIf line.Contains("Simplified Help Command = ") Then
                    If line.Replace("Simplified Help Command = ", "") = "True" Then
                        simHelp = True
                    ElseIf line.Replace("Simplified Help Command = ", "") = "False" Then
                        simHelp = False
                    End If
                ElseIf line.Contains("Quiet Probe = ") Then
                    If line.Replace("Quiet Probe = ", "") = "True" Then
                        QuietHardwareProbe = True
                    ElseIf line.Replace("Quiet Probe = ", "") = "False" Then
                        QuietHardwareProbe = False
                    End If
                ElseIf line.Contains("Show Time/Date on Corner = ") Then
                    If line.Replace("Show Time/Date on Corner = ", "") = "True" Then
                        CornerTD = True
                    ElseIf line.Replace("Show Time/Date on Corner = ", "") = "False" Then
                        CornerTD = False
                    End If
                ElseIf line.Contains("MOTD = ") Then
                    MOTDMessage = line.Replace("MOTD = ", "")
                ElseIf line.Contains("Host Name = ") Then
                    HName = line.Replace("Host Name = ", "")
                ElseIf line.Contains("MOTD After Login = ") Then
                    MAL = line.Replace("MOTD After Login = ", "")
                End If
            End If
            line = OldConfigReader.ReadLine
        Loop
        OldConfigReader.Close()
        OldConfigReader.Dispose()
        Return ValidFormat
    End Function

End Module

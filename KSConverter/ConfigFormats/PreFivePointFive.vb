
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

#If Not NETCOREAPP Then
Imports KS.ConsoleBase
Imports KS.Kernel
Imports KS.Shell
Imports KS.Misc.Writers.ConsoleWriters

Module PreFivePointFive

    'Pre-0.0.5.5 config format has "Kernel Version" as the first line.
    ''' <summary>
    ''' Takes configuration values and installs them to appropriate variables. Taken from config.vb version 0.0.5.2 with some removals that reflect this version
    ''' </summary>
    ''' <param name="PathToConfig">Path to pre-0.0.5.5 config (kernelConfig.ini)</param>
    Function ReadPreFivePointFiveConfig(PathToConfig As String) As Boolean
        Try
            Dim OldConfigReader As New IO.StreamReader(PathToConfig)
            Dim line As String = OldConfigReader.ReadLine
            Dim ValidFormat As Boolean
            Debug.WriteLine("Reading pre-0.0.5.5 config...")
            Do While line <> ""
                Debug.WriteLine($"Parsing line {line}...")
                If line.Contains("Kernel Version = ") Then
                    Debug.WriteLine("Valid config!")
                    ValidFormat = True
                End If
                If ValidFormat Then
                    If line.Contains("Colored Shell = ") Then
                        If line.Replace("Colored Shell = ", "") = "False" Then
                            ColoredShell = False
                        End If
                    End If
                    If line.Contains("User Name Shell Color = ") Then
                        If ColoredShell = True Then UserNameShellColor = New Color(Convert.ToInt32(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("User Name Shell Color = ", "")), ConsoleColors)))
                    ElseIf line.Contains("Host Name Shell Color = ") Then
                        If ColoredShell = True Then HostNameShellColor = New Color(Convert.ToInt32(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("Host Name Shell Color = ", "")), ConsoleColors)))
                    ElseIf line.Contains("Continuable Kernel Error Color = ") Then
                        If ColoredShell = True Then ContKernelErrorColor = New Color(Convert.ToInt32(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("Continuable Kernel Error Color = ", "")), ConsoleColors)))
                    ElseIf line.Contains("Uncontinuable Kernel Error Color = ") Then
                        If ColoredShell = True Then UncontKernelErrorColor = New Color(Convert.ToInt32(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("Uncontinuable Kernel Error Color = ", "")), ConsoleColors)))
                    ElseIf line.Contains("Text Color = ") Then
                        If ColoredShell = True Then NeutralTextColor = New Color(Convert.ToInt32(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("Text Color = ", "")), ConsoleColors)))
                    ElseIf line.Contains("License Color = ") Then
                        If ColoredShell = True Then LicenseColor = New Color(Convert.ToInt32(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("License Color = ", "")), ConsoleColors)))
                    ElseIf line.Contains("Background Color = ") Then
                        If ColoredShell = True Then
                            BackgroundColor = New Color(Convert.ToInt32(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("Background Color = ", "")), ConsoleColors)))
                        End If
                    ElseIf line.Contains("Input Color = ") Then
                        If ColoredShell = True Then InputColor = New Color(Convert.ToInt32(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("Input Color = ", "")), ConsoleColors)))
                    ElseIf line.Contains("Listed command in Help Color = ") Then
                        If ColoredShell = True Then ListEntryColor = New Color(Convert.ToInt32(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("Listed command in Help Color = ", "")), ConsoleColors)))
                    ElseIf line.Contains("Definition of command in Help Color = ") Then
                        If ColoredShell = True Then ListValueColor = New Color(Convert.ToInt32(CType([Enum].Parse(GetType(ConsoleColors), line.Replace("Definition of command in Help Color = ", "")), ConsoleColors)))
                    ElseIf line.Contains("Maintenance Mode = ") Then
                        If line.Replace("Maintenance Mode = ", "") = "True" Then
                            Maintenance = True
                        ElseIf line.Replace("Maintenance Mode = ", "") = "False" Then
                            Maintenance = False
                        End If
                    ElseIf line.Contains("Prompt for Arguments on Boot = ") Then
                        If line.Replace("Prompt for Arguments on Boot = ", "") = "True" Then
                            ArgsOnBoot = True
                        ElseIf line.Replace("Prompt for Arguments on Boot = ", "") = "False" Then
                            ArgsOnBoot = False
                        End If
                    ElseIf line.Contains("Clear Screen on Log-in = ") Then
                        If line.Replace("Clear Screen on Log-in = ", "") = "True" Then
                            ClearOnLogin = True
                        ElseIf line.Replace("Clear Screen on Log-in = ", "") = "False" Then
                            ClearOnLogin = False
                        End If
                    ElseIf line.Contains("Show MOTD on Log-in = ") Then
                        If line.Replace("Show MOTD on Log-in = ", "") = "True" Then
                            ShowMOTD = True
                        ElseIf line.Replace("Show MOTD on Log-in = ", "") = "False" Then
                            ShowMOTD = False
                        End If
                    ElseIf line.Contains("Simplified Help Command = ") Then
                        If line.Replace("Simplified Help Command = ", "") = "True" Then
                            SimHelp = True
                        ElseIf line.Replace("Simplified Help Command = ", "") = "False" Then
                            SimHelp = False
                        End If
                    ElseIf line.Contains("Quiet Probe = ") Then
                        If line.Replace("Quiet Probe = ", "") = "True" Then
                            QuietHardwareProbe = True
                        ElseIf line.Replace("Quiet Probe = ", "") = "False" Then
                            QuietHardwareProbe = False
                        End If
                    ElseIf line.Contains("Show Time/Date on Corner = ") Then
                        If line.Replace("Show Time/Date on Corner = ", "") = "True" Then
                            CornerTimeDate = True
                        ElseIf line.Replace("Show Time/Date on Corner = ", "") = "False" Then
                            CornerTimeDate = False
                        End If
                    ElseIf line.Contains("MOTD = ") Then
                        MOTDMessage = line.Replace("MOTD = ", "")
                    ElseIf line.Contains("Host Name = ") Then
                        HostName = line.Replace("Host Name = ", "")
                    ElseIf line.Contains("MOTD After Login = ") Then
                        MAL = line.Replace("MOTD After Login = ", "")
                    End If
                End If
                line = OldConfigReader.ReadLine
            Loop
            OldConfigReader.Close()
            OldConfigReader.Dispose()
            Debug.WriteLine($"Returning ValidFormat as {ValidFormat}...")
            Return ValidFormat
        Catch ex As Exception
            Debug.WriteLine($"Error while converting config! {ex.Message}")
            Write("  - Warning: Failed to completely convert config. Some of the configurations might not be fully migrated.", True, ColTypes.Warning)
            Return False
        End Try
    End Function

End Module
#End If

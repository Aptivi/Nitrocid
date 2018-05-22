
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Module Config

    Sub createConfig(ByVal CmdArg As Boolean)
        Try
            Dim writer As New StreamWriter(Environ("USERPROFILE") + "\kernelConfig.ini")
            writer.WriteLine("Kernel Version = {0}" + vbNewLine + _
                             "Customized Colors on Boot = False" + vbNewLine + _
                             "User Name Shell Color = {1}" + vbNewLine + _
                             "Host Name Shell Color = {2}" + vbNewLine + _
                             "Continuable Kernel Error Color = {3}" + vbNewLine + _
                             "Uncontinuable Kernel Error Color = {4}" + vbNewLine + _
                             "Text Color = {5}" + vbNewLine + _
                             "License Color = {6}" + vbNewLine + _
                             "Create Demo Account = True" + vbNewLine + _
                             "Change Root Password = False" + vbNewLine + _
                             "Set Root Password to = toor" + vbNewLine + _
                             "Maintenance Mode = False" + vbNewLine + _
                             "Prompt for Arguments on Boot = False" + vbNewLine + _
                             "Clear Screen on Log-in = False" + vbNewLine + _
                             "Show MOTD on Log-in = True" + vbNewLine + _
                             "Simplified Help Command = False" + vbNewLine + _
                             "Colored Shell = True" + vbNewLine + _
                             "Probe Slots = True" + vbNewLine + _
                             "Quiet Probe = False" + vbNewLine + _
                             "Probe GPU = False" + vbNewLine + _
                             "Background Color = {7}" + vbNewLine + _
                             "Input Color = {8}", KernelVersion, userNameShellColor, hostNameShellColor, contKernelErrorColor, _
                                                uncontKernelErrorColor, neutralTextColor, licenseColor, backgroundColor, inputColor)
            writer.Close()
            writer.Dispose()
            If (CmdArg = True) Then
                DisposeExit.DisposeAll()
                Environment.Exit(0)
            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wdbg(ex.StackTrace, True)
                Wln("There is an error trying to create configuration: {0}." + vbNewLine + ex.StackTrace, "neutralText", Err.Description)
            Else
                Wln("There is an error trying to create configuration.", "neutralText")
            End If
            If (CmdArg = True) Then
                DisposeExit.DisposeAll()
                Environment.Exit(2)
            End If
        End Try
    End Sub

    Sub checkForUpgrade()
        Try
            Dim lns() As String = IO.File.ReadAllLines(Environ("USERPROFILE") + "\kernelConfig.ini")
            If (lns(0).Contains("Kernel Version = ") And lns(0).Replace("Kernel Version = ", "") <> KernelVersion) Then
                If (lns.Length > 0) AndAlso (lns(0).StartsWith("Kernel Version = ")) Then
                    Wln("An upgrade from {0} to {1} was detected. Updating configuration file...", "neutralText", lns(0).Replace("Kernel Version = ", ""), KernelVersion)
                    lns(0) = "Kernel Version = " + KernelVersion
                    IO.File.WriteAllLines(Environ("USERPROFILE") + "\kernelConfig.ini", lns)
                End If
            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wdbg(ex.StackTrace, True)
                Wln("There is an error trying to update configuration: {0}." + vbNewLine + ex.StackTrace, "neutralText", Err.Description)
            Else
                Wln("There is an error trying to update configuration.", "neutralText")
            End If
        End Try
    End Sub

    Sub readConfig()
        Try
            Dim line As String = configReader.ReadLine
            Do While line <> ""
                If (line.Contains("Customized Colors on Boot = ")) Then
                    If (line.Replace("Customized Colors on Boot = ", "") = "True") Then
                        customColor = True
                    Else
                        customColor = False
                    End If
                ElseIf (line.Contains("User Name Shell Color = ")) Then
                    userNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), line.Replace("User Name Shell Color = ", "")), ConsoleColor)
                ElseIf (line.Contains("Host Name Shell Color = ")) Then
                    hostNameShellColor = CType([Enum].Parse(GetType(ConsoleColor), line.Replace("Host Name Shell Color = ", "")), ConsoleColor)
                ElseIf (line.Contains("Continuable Kernel Error Color = ")) Then
                    contKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), line.Replace("Continuable Kernel Error Color = ", "")), ConsoleColor)
                ElseIf (line.Contains("Uncontinuable Kernel Error Color = ")) Then
                    uncontKernelErrorColor = CType([Enum].Parse(GetType(ConsoleColor), line.Replace("Uncontinuable Kernel Error Color = ", "")), ConsoleColor)
                ElseIf (line.Contains("Text Color = ")) Then
                    neutralTextColor = CType([Enum].Parse(GetType(ConsoleColor), line.Replace("Text Color = ", "")), ConsoleColor)
                ElseIf (line.Contains("License Color = ")) Then
                    licenseColor = CType([Enum].Parse(GetType(ConsoleColor), line.Replace("License Color = ", "")), ConsoleColor)
                ElseIf (line.Contains("Background Color = ")) Then
                    backgroundColor = CType([Enum].Parse(GetType(ConsoleColor), line.Replace("Background Color = ", "")), ConsoleColor)
                    LoadBackground.Load()
                ElseIf (line.Contains("Input Color = ")) Then
                    inputColor = CType([Enum].Parse(GetType(ConsoleColor), line.Replace("Input Color = ", "")), ConsoleColor)
                ElseIf (line.Contains("Create Demo Account = ")) Then
                    If (line.Replace("Create Demo Account = ", "") = "True") Then
                        enableDemo = True
                    ElseIf (line.Replace("Create Demo Account = ", "") = "False") Then
                        enableDemo = False
                    End If
                ElseIf (line.Contains("Change Root Password = ")) Then
                    If (line.Replace("Change Root Password = ", "") = "True") Then
                        setRootPasswd = True
                    ElseIf (line.Replace("Change Root Password = ", "") = "False") Then
                        setRootPasswd = False
                    End If
                ElseIf (line.Contains("Set Root Password to = ")) Then
                    If (setRootPasswd = True) Then
                        RootPasswd = line.Replace("Set Root Password to = ", "")
                    End If
                ElseIf (line.Contains("Maintenance Mode = ")) Then
                    If (line.Replace("Maintenance Mode = ", "") = "True") Then
                        maintenance = True
                    ElseIf (line.Replace("Maintenance Mode = ", "") = "False") Then
                        maintenance = False
                    End If
                ElseIf (line.Contains("Prompt for Arguments on Boot = ")) Then
                    If (line.Replace("Prompt for Arguments on Boot = ", "") = "True") Then
                        argsOnBoot = True
                    ElseIf (line.Replace("Prompt for Arguments on Boot = ", "") = "False") Then
                        argsOnBoot = False
                    End If
                ElseIf (line.Contains("Clear Screen on Log-in = ")) Then
                    If (line.Replace("Clear Screen on Log-in = ", "") = "True") Then
                        clsOnLogin = True
                    ElseIf (line.Replace("Clear Screen on Log-in = ", "") = "False") Then
                        clsOnLogin = False
                    End If
                ElseIf (line.Contains("Show MOTD on Log-in = ")) Then
                    If (line.Replace("Show MOTD on Log-in = ", "") = "True") Then
                        showMOTD = True
                    ElseIf (line.Replace("Show MOTD on Log-in = ", "") = "False") Then
                        showMOTD = False
                    End If
                ElseIf (line.Contains("Simplified Help Command = ")) Then
                    If (line.Replace("Simplified Help Command = ", "") = "True") Then
                        simHelp = True
                    ElseIf (line.Replace("Simplified Help Command = ", "") = "False") Then
                        simHelp = False
                    End If
                ElseIf (line.Contains("Colored Shell = ")) Then
                    If (line.Replace("Colored Shell = ", "") = "False") Then
                        TemplateSet.templateSet("LinuxUncolored")
                    End If
                ElseIf (line.Contains("Probe Slots = ")) Then
                    If (line.Replace("Probe Slots = ", "") = "True") Then
                        slotProbe = True
                    ElseIf (line.Replace("Probe Slots = ", "") = "False") Then
                        slotProbe = False
                    End If
                ElseIf (line.Contains("Quiet Probe = ")) Then
                    If (line.Replace("Quiet Probe = ", "") = "True") Then
                        quietProbe = True
                    ElseIf (line.Replace("Quiet Probe = ", "") = "False") Then
                        quietProbe = False
                    End If
                ElseIf (line.Contains("Probe GPU = ")) Then
                    If (line.Replace("Probe GPU = ", "") = "True") Then
                        GPUProbeFlag = True
                    ElseIf (line.Replace("Probe GPU = ", "") = "False") Then
                        GPUProbeFlag = False
                    End If
                End If
                line = configReader.ReadLine
            Loop
            configReader.Close()
            configReader.Dispose()
        Catch ex As Exception
            If (DebugMode = True) Then
                Wdbg(ex.StackTrace, True)
                Wln("There is an error trying to read configuration: {0}." + vbNewLine + ex.StackTrace, "neutralText", Err.Description)
            Else
                Wln("There is an error trying to read configuration.", "neutralText")
            End If
        End Try
    End Sub

End Module

﻿
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

Imports KS.Network.FTP

Namespace Shell.Shells
    Public Class FTPShell
        Inherits ShellExecutor
        Implements IShell

        Private FtpInitialized As Boolean

        Public Overrides ReadOnly Property ShellType As ShellType Implements IShell.ShellType
            Get
                Return ShellType.FTPShell
            End Get
        End Property

        Public Overrides Property Bail As Boolean Implements IShell.Bail

        Public Overrides Sub InitializeShell(ParamArray ShellArgs() As Object) Implements IShell.InitializeShell
            'Parse shell arguments
            Dim Connects As Boolean = ShellArgs.Length > 0
            Dim Address As String = ""
            If Connects Then Address = ShellArgs(0)

            'Actual shell logic
            Dim FtpCommand As String = ""
            While Not Bail
                SyncLock FTPCancelSync
                    Try
                        'Complete initialization
                        If FtpInitialized = False Then
                            Wdbg(DebugLevel.I, $"Completing initialization of FTP: {FtpInitialized}")
                            FtpCurrentDirectory = HomePath
                            KernelEventManager.RaiseFTPShellInitialized()
                            SwitchCancellationHandler(ShellType.FTPShell)
                            FtpInitialized = True
                        End If

                        'Check if the shell is going to exit
                        If Bail Then
                            Wdbg(DebugLevel.W, "Exiting shell...")
                            FtpConnected = False
                            ClientFTP?.Disconnect()
                            FtpSite = ""
                            FtpCurrentDirectory = ""
                            FtpCurrentRemoteDir = ""
                            FtpUser = ""
                            FtpPass = ""
                            FtpInitialized = False
                            SwitchCancellationHandler(LastShellType)
                            Exit Sub
                        End If

                        'Prompt for command
                        If DefConsoleOut IsNot Nothing Then
                            Console.SetOut(DefConsoleOut)
                        End If
                        If Not Connects Then
                            Wdbg(DebugLevel.I, "Preparing prompt...")
                            If FtpConnected Then
                                Wdbg(DebugLevel.I, "FTPShellPromptStyle = {0}", FTPShellPromptStyle)
                                If FTPShellPromptStyle = "" Then
                                    TextWriterColor.Write("[", False, ColTypes.Gray) : TextWriterColor.Write("{0}", False, ColTypes.UserName, FtpUser) : TextWriterColor.Write("@", False, ColTypes.Gray) : TextWriterColor.Write("{0}", False, ColTypes.HostName, FtpSite) : TextWriterColor.Write("]{0}> ", False, ColTypes.Gray, FtpCurrentRemoteDir) : TextWriterColor.Write("", False, InputColor)
                                Else
                                    Dim ParsedPromptStyle As String = ProbePlaces(FTPShellPromptStyle)
                                    ParsedPromptStyle.ConvertVTSequences
                                    TextWriterColor.Write(ParsedPromptStyle, False, ColTypes.Gray) : TextWriterColor.Write("", False, InputColor)
                                End If
                            Else
                                TextWriterColor.Write("{0}> ", False, ColTypes.Gray, FtpCurrentDirectory) : TextWriterColor.Write("", False, InputColor)
                            End If
                        End If

                        'Try to connect if IP address is specified.
                        If Connects Then
                            Wdbg(DebugLevel.I, $"Currently connecting to {Address} by ""ftp (address)""...")
                            FtpCommand = $"connect {Address}"
                            Connects = False
                        Else
                            Wdbg(DebugLevel.I, "Normal shell")
                            FtpCommand = Console.ReadLine()
                        End If
                        KernelEventManager.RaiseFTPPreExecuteCommand(FtpCommand)

                        'Parse command
                        If Not (FtpCommand = Nothing Or FtpCommand?.StartsWithAnyOf({" ", "#"})) Then
                            GetLine(FtpCommand, False, "", ShellType.FTPShell)
                            KernelEventManager.RaiseFTPPostExecuteCommand(FtpCommand)
                        End If
                    Catch ex As Exception
                        WStkTrc(ex)
                        Throw New Exceptions.FTPShellException(DoTranslation("There was an error in the FTP shell:") + " {0}", ex, ex.Message)
                    End Try
                End SyncLock
            End While
        End Sub

    End Class
End Namespace
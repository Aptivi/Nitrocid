
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

Imports System.Threading
Imports KS.Network.FTP
Imports KS.Shell.Prompts

Namespace Shell.Shells.FTP
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
            Dim FtpCommand As String
            Dim Connects As Boolean = ShellArgs.Length > 0
            Dim Address As String = ""
            If Connects Then Address = ShellArgs(0)

            'Actual shell logic
            While Not Bail
                Try
                    'Complete initialization
                    If FtpInitialized = False Then
                        Wdbg(DebugLevel.I, $"Completing initialization of FTP: {FtpInitialized}")
#If Not NETCOREAPP Then
                        FtpTrace.AddListener(New FTPTracer)
#endif
                        FtpTrace.LogUserName = FTPLoggerUsername
                        FtpTrace.LogPassword = False 'Don't remove this, make a config entry for it, or set it to True! It will introduce security problems.
                        FtpTrace.LogIP = FTPLoggerIP
                        FtpCurrentDirectory = HomePath
                        KernelEventManager.RaiseFTPShellInitialized()
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
                        Exit Sub
                    End If

                    'See UESHShell.vb for more info
                    SyncLock GetCancelSyncLock(ShellType)
                        'Restore console state
                        If DefConsoleOut IsNot Nothing Then
                            Console.SetOut(DefConsoleOut)
                        End If

                        'Prompt for command
                        If Not Connects Then
                            Wdbg(DebugLevel.I, "Preparing prompt...")
                            WriteShellPrompt(ShellType)
                        End If
                    End SyncLock

                    'Try to connect if IP address is specified.
                    If Connects Then
                        Wdbg(DebugLevel.I, $"Currently connecting to {Address} by ""ftp (address)""...")
                        FtpCommand = $"connect {Address}"
                        Connects = False
                    Else
                        Wdbg(DebugLevel.I, "Normal shell")
                        FtpCommand = ReadLine()
                    End If

                    'Parse command
                    If Not (FtpCommand = Nothing Or FtpCommand?.StartsWithAnyOf({" ", "#"})) Then
                        KernelEventManager.RaiseFTPPreExecuteCommand(FtpCommand)
                        GetLine(FtpCommand, "", ShellType.FTPShell)
                        KernelEventManager.RaiseFTPPostExecuteCommand(FtpCommand)
                    End If
                Catch taex As ThreadInterruptedException
                    CancelRequested = False
                    Bail = True
                Catch ex As Exception
                    WStkTrc(ex)
                    Throw New Exceptions.FTPShellException(DoTranslation("There was an error in the FTP shell:") + " {0}", ex, ex.Message)
                End Try
            End While
        End Sub

    End Class
End Namespace

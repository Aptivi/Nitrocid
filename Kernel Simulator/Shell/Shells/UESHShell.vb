
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

Imports System.Threading
Imports KS.Misc.Screensaver

Namespace Shell.Shells
    Public Class UESHShell
        Inherits ShellExecutor
        Implements IShell

        Public Overrides ReadOnly Property ShellType As ShellType Implements IShell.ShellType
            Get
                Return ShellType.Shell
            End Get
        End Property

        Public Overrides Property Bail As Boolean Implements IShell.Bail

        Public Overrides Sub InitializeShell(ParamArray ShellArgs() As Object) Implements IShell.InitializeShell
            While Not Bail
                If LogoutRequested Then
                    Wdbg(DebugLevel.I, "Requested log out: {0}", LogoutRequested)
                    LogoutRequested = False
                    LoggedIn = False
                    Bail = True
                ElseIf Not InSaver Then
                    Try
                        'Try to probe injected commands
                        Wdbg(DebugLevel.I, "Probing injected commands...")
                        If CommandFlag = True Then
                            CommandFlag = False
                            If ProbeInjectedCommands Then
                                For Each cmd In InjectedCommands
                                    GetLine(cmd, True)
                                Next
                            End If
                        End If

                        'We need to put a synclock in the below steps, because the cancellation handlers seem to be taking their time to try to suppress the
                        'thread abort error messages. If the shell tried to write to the console while these handlers were still working, the command prompt
                        'would either be incomplete or not printed to the console at all. As a side effect, we wouldn't fire the shell initialization event
                        'despite us calling the RaiseShellInitialized() routine, causing some mods that rely on this event to believe that the shell was still
                        'waiting for the command.
                        SyncLock GetCancelSyncLock(ShellType)
                            'Enable cursor (We put it here to avoid repeated "CursorVisible = True" statements in different command codes)
                            Console.CursorVisible = True

                            'Write a prompt
                            If DefConsoleOut IsNot Nothing Then
                                Console.SetOut(DefConsoleOut)
                            End If
                            CommandPromptWrite()

                            'Set an input color
                            SetInputColor()

                            'Raise shell initialization event
                            KernelEventManager.RaiseShellInitialized()
                        End SyncLock

                        'Wait for command
                        Wdbg(DebugLevel.I, "Waiting for command")
                        Dim strcommand As String = ReadLine()

                        'Now, parse the line as necessary
                        If Not InSaver Then
                            'Fire an event of PreExecuteCommand
                            KernelEventManager.RaisePreExecuteCommand(strcommand)

                            'Get the command
                            GetLine(strcommand)

                            'Fire an event of PostExecuteCommand
                            KernelEventManager.RaisePostExecuteCommand(strcommand)
                        End If
                    Catch taex As ThreadInterruptedException
                        CancelRequested = False
                        Bail = True
                    Catch ex As Exception
                        WStkTrc(ex)
                        Write(DoTranslation("There was an error in the shell.") + NewLine + "Error {0}: {1}", True, ColTypes.Error, ex.GetType.FullName, ex.Message)
                        Continue While
                    End Try
                End If
            End While
        End Sub

        ''' <summary>
        ''' Writes the input for command prompt
        ''' </summary>
        Public Sub CommandPromptWrite()
            'Check the custom shell prompt style and kernel mode
            Wdbg(DebugLevel.I, "ShellPromptStyle = {0}", ShellPromptStyle)
            If Not String.IsNullOrWhiteSpace(ShellPromptStyle) And Not Maintenance Then
                'Parse the shell prompt style
                Dim ParsedPromptStyle As String = ProbePlaces(ShellPromptStyle)
                ParsedPromptStyle.ConvertVTSequences
                Write(ParsedPromptStyle, False, ColTypes.Gray)
            ElseIf String.IsNullOrWhiteSpace(ShellPromptStyle) And Not Maintenance Then
                'Write the user dollar sign using the two styles, depending on the permission of the user
                If HasPermission(CurrentUser.Username, PermissionType.Administrator) Then
                    Write("[", False, ColTypes.Gray) : Write("{0}", False, ColTypes.UserName, CurrentUser.Username) : Write("@", False, ColTypes.Gray) : Write("{0}", False, ColTypes.HostName, HostName) : Write("]{0}", False, ColTypes.Gray, CurrDir) : Write(" # ", False, ColTypes.UserDollarSign)
                Else
                    Write("[", False, ColTypes.Gray) : Write("{0}", False, ColTypes.UserName, CurrentUser.Username) : Write("@", False, ColTypes.Gray) : Write("{0}", False, ColTypes.HostName, HostName) : Write("]{0}", False, ColTypes.Gray, CurrDir) : Write(" $ ", False, ColTypes.UserDollarSign)
                End If
            Else
                Write(DoTranslation("Maintenance Mode") + "> ", False, ColTypes.Gray)
            End If
        End Sub

    End Class
End Namespace

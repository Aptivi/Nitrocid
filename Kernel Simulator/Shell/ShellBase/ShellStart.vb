
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

Imports KS.Shell.Shells

Namespace Shell.ShellBase
    Public Module ShellStart

        Friend ShellStack As New List(Of ShellInfo)

        ''' <summary>
        ''' Starts the shell
        ''' </summary>
        ''' <param name="ShellType">The shell type</param>
        Public Sub StartShell(ShellType As ShellType, ParamArray ShellArgs() As Object)
            If ShellStack.Count >= 1 Then
                'The shell stack has a mother shell. Start another shell.
                StartShellForced(ShellType, ShellArgs)
            End If
        End Sub

        ''' <summary>
        ''' Force starts the shell
        ''' </summary>
        ''' <param name="ShellType">The shell type</param>
        Sub StartShellForced(ShellType As ShellType, ParamArray ShellArgs() As Object)
            'Make a shell executor based on shell type to select a specific executor (if the shell type is not UESH, and if the new shell isn't a mother shell)
            'Please note that the remote debug shell is not supported because it works on its own space, so it can't be interfaced using the standard IShell.
            Dim ShellExecute As ShellExecutor = GetShellExecutor(ShellType)

            'Make a new instance of shell information
            Dim ShellCommandThread As New KernelThread($"{ShellType} Command Thread", False, AddressOf ExecuteCommand)
            Dim ShellInfo As New ShellInfo(ShellType, ShellExecute, ShellCommandThread)

            'Add a new shell to the shell stack to indicate that we have a new shell (a visitor)!
            ShellStack.Add(ShellInfo)
            ShellExecute.InitializeShell(ShellArgs)
        End Sub

        ''' <summary>
        ''' Kills the last running shell
        ''' </summary>
        Public Sub KillShell()
            'We must have at least two shells to kill the last shell. Else, we will have zero shells running, making us look like we've logged out!
            If ShellStack.Count >= 2 Then
                ShellStack(ShellStack.Count - 1).ShellExecutor.Bail = True
                PurgeShells()
            Else
                Throw New InvalidOperationException(DoTranslation("Can not kill the mother shell!"))
            End If
        End Sub

        ''' <summary>
        ''' Force kills the last running shell
        ''' </summary>
        Sub KillShellForced()
            If ShellStack.Count >= 1 Then
                ShellStack(ShellStack.Count - 1).ShellExecutor.Bail = True
                PurgeShells()
            End If
        End Sub

        ''' <summary>
        ''' Cleans up the shell stack
        ''' </summary>
        Public Sub PurgeShells()
            'Remove these shells from the stack
            ShellStack.RemoveAll(Function(x) x.ShellExecutor.Bail = True)
        End Sub

        ''' <summary>
        ''' Gets the shell executor based on the shell type
        ''' </summary>
        ''' <param name="ShellType">The requested shell type</param>
        Function GetShellExecutor(ShellType As ShellType) As ShellExecutor
            Select Case ShellType
                Case ShellType.Shell
                    Return New UESHShell()
                Case ShellType.FTPShell
                    Return New FTPShell()
                Case ShellType.MailShell
                    Return New MailShell()
                Case ShellType.SFTPShell
                    Return New SFTPShell()
                Case ShellType.TextShell
                    Return New TextShell()
                Case ShellType.TestShell
                    Return New Shells.TestShell()
                Case ShellType.ZIPShell
                    Return New ZipShell()
                Case ShellType.RSSShell
                    Return New RSSShell()
                Case ShellType.JsonShell
                    Return New JsonShell()
                Case ShellType.HTTPShell
                    Return New HTTPShell()
                Case ShellType.HexShell
                    Return New HexShell()
            End Select
        End Function

    End Module
End Namespace

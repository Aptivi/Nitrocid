
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

        Friend ShellStack As New List(Of ShellExecutor)

        ''' <summary>
        ''' Starts the shell
        ''' </summary>
        ''' <param name="ShellType">The shell type</param>
        Public Sub StartShell(ShellType As ShellType, ParamArray ShellArgs() As Object)
            Dim ShellExecute As ShellExecutor = New UESHShell()

            'Make a shell executor based on shell type to select a specific executor (if the shell type is not UESH, and if the new shell isn't a mother shell)
            'Please note that the remote debug shell is not supported because it works on its own space, so it can't be interfaced using the standard IShell.
            If ShellStack.Count >= 1 Then
                Select Case ShellType
                    Case ShellType.Shell
                        ShellExecute = New UESHShell()
                    Case ShellType.FTPShell
                        ShellExecute = New FTPShell()
                    Case ShellType.MailShell
                        ShellExecute = New MailShell()
                    Case ShellType.SFTPShell
                        ShellExecute = New SFTPShell()
                    Case ShellType.TextShell
                        ShellExecute = New TextShell()
                    Case ShellType.TestShell
                        ShellExecute = New Shells.TestShell()
                    Case ShellType.ZIPShell
                        ShellExecute = New ZipShell()
                    Case ShellType.RSSShell
                        ShellExecute = New RSSShell()
                    Case ShellType.JsonShell
                        ShellExecute = New JsonShell()
                    Case ShellType.HTTPShell
                        ShellExecute = New HTTPShell()
                    Case ShellType.HexShell
                        ShellExecute = New HexShell()
                End Select
            End If

            'Add a new executor and put it to the shell stack to indicate that we have a new shell (a visitor)!
            ShellStack.Add(ShellExecute)
            ShellExecute.InitializeShell(ShellArgs)
        End Sub

        ''' <summary>
        ''' Force starts the shell
        ''' </summary>
        ''' <param name="ShellType">The shell type</param>
        Sub StartShellForced(ShellType As ShellType, ParamArray ShellArgs() As Object)
            Dim ShellExecute As ShellExecutor = New UESHShell()

            'Make a shell executor based on shell type to select a specific executor (if the shell type is not UESH, and if the new shell isn't a mother shell)
            'Please note that the remote debug shell is not supported because it works on its own space, so it can't be interfaced using the standard IShell.
            Select Case ShellType
                Case ShellType.Shell
                    ShellExecute = New UESHShell()
                Case ShellType.FTPShell
                    ShellExecute = New FTPShell()
                Case ShellType.MailShell
                    ShellExecute = New MailShell()
                Case ShellType.SFTPShell
                    ShellExecute = New SFTPShell()
                Case ShellType.TextShell
                    ShellExecute = New TextShell()
                Case ShellType.TestShell
                    ShellExecute = New Shells.TestShell()
                Case ShellType.ZIPShell
                    ShellExecute = New ZipShell()
                Case ShellType.RSSShell
                    ShellExecute = New RSSShell()
                Case ShellType.JsonShell
                    ShellExecute = New JsonShell()
                Case ShellType.HTTPShell
                    ShellExecute = New HTTPShell()
                Case ShellType.HexShell
                    ShellExecute = New HexShell()
            End Select

            'Add a new executor and put it to the shell stack to indicate that we have a new shell (a visitor)!
            ShellStack.Add(ShellExecute)
            ShellExecute.InitializeShell(ShellArgs)
        End Sub

        ''' <summary>
        ''' Kills the last running shell
        ''' </summary>
        Public Sub KillShell()
            'We must have at least two shells to kill the last shell. Else, we will have zero shells running, making us look like we've logged out!
            If ShellStack.Count >= 2 Then
                ShellStack(ShellStack.Count - 1).Bail = True
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
                ShellStack(ShellStack.Count - 1).Bail = True
                PurgeShells()
            End If
        End Sub

        ''' <summary>
        ''' Cleans up the shell stack
        ''' </summary>
        Public Sub PurgeShells()
            'Remove these shells from the stack
            ShellStack.RemoveAll(Function(x) x.Bail = True)
        End Sub

    End Module
End Namespace

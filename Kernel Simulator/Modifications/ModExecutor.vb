
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Public Module ModExecutor

    ''' <summary>
    ''' Mod command type
    ''' </summary>
    Public Enum ModType
        ''' <summary>
        ''' Normal UESH shell
        ''' </summary>
        Shell
        ''' <summary>
        ''' FTP shell
        ''' </summary>
        FTPShell
        ''' <summary>
        ''' Mail shell
        ''' </summary>
        MailShell
        ''' <summary>
        ''' Text shell
        ''' </summary>
        TextShell
    End Enum

    ''' <summary>
    ''' Executes the command provided by a mod
    ''' </summary>
    ''' <param name="cmd">A mod command with arguments</param>
    Sub ExecuteModCommand(ByVal cmd As String)
        Dim parts As String() = cmd.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim actualCmd As String = parts(0)
        Wdbg("I", "Command = {0}", actualCmd)
        For Each script As IScript In scripts.Values
            If (actualCmd = script.Cmd) And (script.Name <> Nothing) And (actualCmd <> script.Name) Then
                Wdbg("I", "Command = {0}", actualCmd)
                actualCmd = script.Name
            End If
        Next
        If cmd.StartsWith(parts(0) + " ") Then
            'These below will be executed if there are arguments
            Dim args As String = cmd.Replace($"{parts(0)} ", "")
            Wdbg("I", "Command {0} will be run with arguments: {1}", actualCmd, args)
            scripts(actualCmd).PerformCmd(args)
        Else
            'This will be executed if there are no arguments
            Wdbg("I", "Command {0} will be run.", actualCmd)
            scripts(actualCmd).PerformCmd()
        End If
        Wdbg("I", "Command executed successfully.")
    End Sub

End Module

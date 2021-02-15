
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
        ''' SFTP shell
        ''' </summary>
        SFTPShell
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
        For Each ModPart As Dictionary(Of String, IScript) In scripts.Values
            For Each script As IScript In ModPart.Values
                If (actualCmd = script.Cmd) And (script.Name <> Nothing) And (actualCmd <> script.Name) Then
                    actualCmd = script.Name
                End If
            Next
        Next
        If cmd.StartsWith(parts(0) + " ") Then
            'These below will be executed if there are arguments
            Dim args As String = cmd.Replace($"{parts(0)} ", "")
            Wdbg("I", "Command {0} will be run with arguments: {1}", actualCmd, args)
            For Each ModParts As String In scripts(actualCmd).Keys
                If scripts(actualCmd)(ModParts).Cmd = parts(0) Then
                    Wdbg("I", "Using command {0} from {1} to be executed...", parts(0), ModParts)
                    scripts(actualCmd)(ModParts).PerformCmd(args)
                End If
            Next
        Else
            'This will be executed if there are no arguments
            Wdbg("I", "Command {0} will be run.", actualCmd)
            For Each ModParts As String In scripts(actualCmd).Keys
                If scripts(actualCmd)(ModParts).Cmd = parts(0) Then
                    Wdbg("I", "Using command {0} from {1} to be executed...", parts(0), ModParts)
                    scripts(actualCmd)(ModParts).PerformCmd()
                End If
            Next
        End If
        Wdbg("I", "Command executed successfully.")
    End Sub

End Module

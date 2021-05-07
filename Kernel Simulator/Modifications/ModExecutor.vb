
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
    ''' Executes the command provided by a mod
    ''' </summary>
    ''' <param name="cmd">A mod command with arguments</param>
    Sub ExecuteModCommand(ByVal cmd As String)
        EventManager.RaisePreExecuteModCommand(cmd)
        Dim parts As String() = cmd.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim args As String = ""
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
            args = cmd.Replace($"{parts(0)} ", "")
            Wdbg("I", "Command {0} will be run with arguments: {1}", actualCmd, args)
        End If
        For Each ModParts As String In scripts(actualCmd).Keys
            If scripts(actualCmd)(ModParts).Cmd = parts(0) Then
                If (scripts(actualCmd)(ModParts).CmdRestricted And adminList(signedinusrnm)) Or Not scripts(actualCmd)(ModParts).CmdRestricted Then
                    Wdbg("I", "Using command {0} from {1} to be executed...", parts(0), ModParts)
                    scripts(actualCmd)(ModParts).PerformCmd(args)
                Else
                    Wdbg("E", "User {0} doesn't have permission to use {1} from {2}!", signedinusrnm, parts(0), ModParts)
                    W(DoTranslation("You don't have permission to use {0}"), True, ColTypes.Err, parts(0))
                End If
            End If
        Next
        EventManager.RaisePostExecuteModCommand(cmd)
        Wdbg("I", "Command executed successfully.")
    End Sub

End Module

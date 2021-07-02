
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

        'Variables
        Dim parts As String() = cmd.SplitEncloseDoubleQuotes(" ")
        Dim args As String = ""
        Dim actualCmd As String = parts(0)
        Wdbg("I", "Command = {0}", actualCmd)

        'Check to see if the command written needs normalization
        For Each ModPart As ModInfo In scripts.Values
            For Each script As IScript In ModPart.ModParts.Values
                If script.Commands IsNot Nothing Then
                    If script.Commands.ContainsKey(actualCmd) And (script.Name <> Nothing) And (actualCmd <> script.Name) Then
                        'The commands in the script has the actual command, the mod name is not null, and the command doesn't equal the mod name.
                        'In this case, make the actual command executed the script name.
                        actualCmd = script.Name
                        Wdbg("I", "Actual command = {0}", actualCmd)
                    End If
                End If
            Next
        Next

        'Prepare the argument string.
        If cmd.StartsWith(parts(0) + " ") Or cmd.StartsWith("""" + parts(0) + """ ") Then
            'These below will be executed if there are arguments
            args = cmd.Replace($"{parts(0)} ", "").Replace($"""{parts(0)}"" ", "")
            Wdbg("I", "Command {0} will be run with arguments: {1}", actualCmd, args)
        End If

        'Try to execute the command.
        For Each ModPart As String In scripts(actualCmd).ModParts.Keys
            Dim Script As IScript = scripts(actualCmd).ModParts(ModPart)
            If Script.Commands IsNot Nothing Then
                'Found commands dictionary! Now, check it for the command
                If Script.Commands.ContainsKey(parts(0)) Then
                    If Script.Commands(parts(0)).Type = ShellCommandType.Shell Then
                        'Command type is of shell. Check the user privileges for restricted commands.
                        If (Script.Commands(parts(0)).Strict And adminList(signedinusrnm)) Or Not Script.Commands(parts(0)).Strict Then
                            'User was authorized to use the command, or the command wasn't strict
                            Wdbg("I", "Using command {0} from {1} to be executed...", parts(0), ModPart)
                            Script.PerformCmd(Script.Commands(parts(0)), args)
                        Else
                            'User wasn't authorized.
                            Wdbg("E", "User {0} doesn't have permission to use {1} from {2}!", signedinusrnm, parts(0), ModPart)
                            W(DoTranslation("You don't have permission to use {0}"), True, ColTypes.Error, parts(0))
                        End If
                    Else
                        'Command type is not of shell. Execute anyways.
                        Wdbg("I", "Using command {0} from {1} to be executed...", parts(0), ModPart)
                        Script.PerformCmd(Script.Commands(parts(0)), args)
                    End If
                End If
            End If
        Next

        'Raise event
        EventManager.RaisePostExecuteModCommand(cmd)
        Wdbg("I", "Command executed successfully.")
    End Sub

End Module

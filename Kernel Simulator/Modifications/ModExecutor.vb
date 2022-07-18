
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

Namespace Modifications
    Module ModExecutor

        ''' <summary>
        ''' Executes the command provided by a mod
        ''' </summary>
        ''' <param name="cmd">A mod command with arguments</param>
        Sub ExecuteModCommand(cmd As String)
            KernelEventManager.RaisePreExecuteModCommand(cmd)

            'Variables
            Dim parts As String() = cmd.SplitEncloseDoubleQuotes(" ")
            Dim args As String = ""
            Dim actualCmd As String = parts(0)
            Wdbg(DebugLevel.I, "Command = {0}", actualCmd)

            'Check to see if the command written needs normalization
            For Each ModPart As ModInfo In Mods.Values
                For Each PartInfo As PartInfo In ModPart.ModParts.Values
                    Dim script As IScript = PartInfo.PartScript
                    If script.Commands IsNot Nothing Then
                        If script.Commands.ContainsKey(actualCmd) And (script.Name <> Nothing) And (actualCmd <> script.Name) Then
                            'The commands in the script has the actual command, the mod name is not null, and the command doesn't equal the mod name.
                            'In this case, make the actual command executed the script name.
                            actualCmd = script.Name
                            Wdbg(DebugLevel.I, "Actual command = {0}", actualCmd)
                        End If
                    End If
                Next
            Next

            'Prepare the argument string.
            If cmd.StartsWith(parts(0) + " ") Or cmd.StartsWith("""" + parts(0) + """ ") Then
                'These below will be executed if there are arguments
                args = cmd.Replace($"{parts(0)} ", "").Replace($"""{parts(0)}"" ", "")
                Wdbg(DebugLevel.I, "Command {0} will be run with arguments: {1}", actualCmd, args)
            End If

            'Try to execute the command.
            For Each ModPart As String In Mods(actualCmd).ModParts.Keys
                Dim Script As IScript = Mods(actualCmd).ModParts(ModPart).PartScript
                If Script.Commands IsNot Nothing Then
                    'Found commands dictionary! Now, check it for the command
                    If Script.Commands.ContainsKey(parts(0)) Then
                        'Populate the arguments info and command base variables
                        Dim ScriptCommandBase As CommandExecutor = Script.Commands(parts(0)).CommandBase
                        Dim ScriptCommandArgsInfo As New ProvidedCommandArgumentsInfo(cmd, Script.Commands(parts(0)).Type)
                        Dim ScriptCommandFullArgs() As String = ScriptCommandArgsInfo.FullArgumentsList
                        Dim ScriptCommandArgs() As String = ScriptCommandArgsInfo.ArgumentsList
                        Dim ScriptCommandSwitches() As String = ScriptCommandArgsInfo.SwitchesList
                        Dim ScriptCommandExecutable As Boolean

                        'Check to see if we're in the shell type command.
                        If Script.Commands(parts(0)).Type = ShellType.Shell Then
                            'Command type is of shell. Check the user privileges for restricted commands.
                            If (Script.Commands(parts(0)).Strict And HasPermission(CurrentUser.Username, PermissionType.Administrator)) Or Not Script.Commands(parts(0)).Strict Then
                                'User is authorized to use the command, or the command isn't strict
                                ScriptCommandExecutable = True
                            Else
                                'User wasn't authorized.
                                Wdbg(DebugLevel.E, "User {0} doesn't have permission to use {1} from {2}!", CurrentUser.Username, parts(0), ModPart)
                                Write(DoTranslation("You don't have permission to use {0}"), True, ColTypes.Error, parts(0))
                            End If
                        Else
                            'Command type is not of shell. Execute anyway.
                            ScriptCommandExecutable = True
                        End If

                        'If the command check went all well without any hiccups, execute the command.
                        If ScriptCommandExecutable Then
                            Wdbg(DebugLevel.I, "Using command {0} from {1} to be executed...", parts(0), ModPart)
                            If ScriptCommandBase IsNot Nothing Then
                                'Use the modern CommandBase.Execute() command
                                ScriptCommandBase.Execute(args, ScriptCommandFullArgs, ScriptCommandArgs, ScriptCommandSwitches)
                            End If
                        End If
                    End If
                End If
            Next

            'Raise event
            KernelEventManager.RaisePostExecuteModCommand(cmd)
            Wdbg(DebugLevel.I, "Command executed successfully.")
        End Sub

    End Module
End Namespace

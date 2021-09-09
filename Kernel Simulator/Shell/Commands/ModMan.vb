
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

Imports System.IO

Class ModManCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String) Implements ICommand.Execute
        If Not SafeMode Then
            Dim CommandMode As String = ListArgs(0).ToLower
            Dim TargetMod As String = ""
            Dim TargetModPath As String = ""

            'These command modes require two arguments to be passed, so re-check here and there.
            Select Case CommandMode
                Case "start", "stop", "info", "reload"
                    If ListArgs.Length > 1 Then
                        TargetMod = ListArgs(1)
                        TargetModPath = GetKernelPath(KernelPathType.Mods) + TargetMod
                        If Not (TryParsePath(TargetModPath) AndAlso File.Exists(TargetModPath)) Then
                            W(DoTranslation("Mod not found or file has invalid characters."), True, ColTypes.Error)
                            Exit Sub
                        End If
                    Else
                        Exit Sub
                    End If
            End Select

            'Now, the actual logic
            Select Case CommandMode
                Case "start"
                    W(DoTranslation("Starting mod") + " {0}...", True, ColTypes.Neutral, Path.GetFileNameWithoutExtension(TargetMod))
                    StartMod(TargetModPath)
                Case "stop"
                    StopMod(TargetModPath)
                Case "info"
                    For Each script As String In scripts.Keys
                        If scripts(script).ModFilePath = TargetModPath Then
                            WriteSeparator(script, True, ColTypes.Stage)
                            W("- " + DoTranslation("Mod name:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModName, True, ColTypes.ListValue)
                            W("- " + DoTranslation("Mod file name:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModFileName, True, ColTypes.ListValue)
                            W("- " + DoTranslation("Mod file path:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModFilePath, True, ColTypes.ListValue)
                            W("- " + DoTranslation("Mod version:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModVersion, True, ColTypes.ListValue)
                            W("- " + DoTranslation("Mod parts:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModParts.Count, True, ColTypes.ListValue)
                            For Each ModPart As String In scripts(script).ModParts.Keys
                                WriteSeparator("-- {0}", False, ColTypes.Stage, ModPart)
                                W("- " + DoTranslation("Part version:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModParts(ModPart).PartScript.Version, True, ColTypes.ListValue)
                                W("- " + DoTranslation("Part file name:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModParts(ModPart).PartFileName, True, ColTypes.ListValue)
                                W("- " + DoTranslation("Part file path:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModParts(ModPart).PartFilePath, True, ColTypes.ListValue)
                                If scripts(script).ModParts(ModPart).PartScript.Commands IsNot Nothing Then
                                    For Each ModCommand As String In scripts(script).ModParts(ModPart).PartScript.Commands.Keys
                                        WriteSeparator("--- {0}", False, ColTypes.Stage, ModCommand)
                                        W("- " + DoTranslation("Command name:") + " ", False, ColTypes.ListEntry) : W(ModCommand, True, ColTypes.ListValue)
                                        W("- " + DoTranslation("Command definition:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModParts(ModPart).PartScript.Commands(ModCommand).HelpDefinition, True, ColTypes.ListValue)
                                        W("- " + DoTranslation("Command usage:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModParts(ModPart).PartScript.Commands(ModCommand).HelpUsage, True, ColTypes.ListValue)
                                        W("- " + DoTranslation("Command type:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModParts(ModPart).PartScript.Commands(ModCommand).Type, True, ColTypes.ListValue)
                                        W("- " + DoTranslation("Strict command?") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModParts(ModPart).PartScript.Commands(ModCommand).Strict, True, ColTypes.ListValue)
                                        W("- " + DoTranslation("Arguments required?") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModParts(ModPart).PartScript.Commands(ModCommand).ArgumentsRequired, True, ColTypes.ListValue)
                                        W("- " + DoTranslation("Minimum count of required arguments:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModParts(ModPart).PartScript.Commands(ModCommand).MinimumArguments, True, ColTypes.ListValue)
                                        W("- " + DoTranslation("Wrappable command?") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModParts(ModPart).PartScript.Commands(ModCommand).Wrappable, True, ColTypes.ListValue)
                                        W("- " + DoTranslation("Setting shell variable?") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModParts(ModPart).PartScript.Commands(ModCommand).SettingVariable, True, ColTypes.ListValue)
                                        W("- " + DoTranslation("Can not run in maintenance mode?") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModParts(ModPart).PartScript.Commands(ModCommand).NoMaintenance, True, ColTypes.ListValue)
                                        W("- " + DoTranslation("Obsolete?") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModParts(ModPart).PartScript.Commands(ModCommand).Obsolete, True, ColTypes.ListValue)
                                    Next
                                End If
                            Next
                        End If
                    Next
                Case "reload"
                    StopMod(TargetModPath)
                    StartMod(TargetModPath)
                Case "list"
                    For Each script As String In scripts.Keys
                        WriteSeparator(script, True, ColTypes.Stage)
                        W("- " + DoTranslation("Mod name:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModName, True, ColTypes.ListValue)
                        W("- " + DoTranslation("Mod file name:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModFileName, True, ColTypes.ListValue)
                        W("- " + DoTranslation("Mod file path:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModFilePath, True, ColTypes.ListValue)
                        W("- " + DoTranslation("Mod version:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModVersion, True, ColTypes.ListValue)
                        W("- " + DoTranslation("Mod parts:") + " ", False, ColTypes.ListEntry) : W(scripts(script).ModParts.Count, True, ColTypes.ListValue)
                    Next
                Case "reloadall"
                    ReloadMods()
                Case "stopall"
                    StopMods()
                Case "startall"
                    StartMods()
                Case Else
                    W(DoTranslation("Invalid command {0}. Check the usage below:"), True, ColTypes.Error, CommandMode)
                    ShowHelp(Command)
            End Select
        Else
            W(DoTranslation("Mod management is disabled in safe mode."), True, ColTypes.Error)
        End If
    End Sub

End Class
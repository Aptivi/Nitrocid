
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

Imports System.IO
Imports Newtonsoft.Json.Linq
Imports KS.Misc.Editors.JsonShell.Commands

Namespace Misc.Editors.JsonShell
    Public Module JsonShellCommon

        'Variables
        Public ReadOnly JsonShell_Commands As New Dictionary(Of String, CommandInfo) From {
            {"addproperty", New CommandInfo("addproperty", ShellType.JsonShell, "Adds a new property at the end of the JSON file", New CommandArgumentInfo({"<parentProperty> <propertyName> <propertyValue>"}, True, 3), New JsonShell_AddPropertyCommand)},
            {"clear", New CommandInfo("clear", ShellType.JsonShell, "Clears the JSON file", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New JsonShell_ClearCommand)},
            {"delproperty", New CommandInfo("delproperty", ShellType.JsonShell, "Removes a property from the JSON file", New CommandArgumentInfo({"<propertyName>"}, True, 1), New JsonShell_DelPropertyCommand)},
            {"exit", New CommandInfo("exit", ShellType.JsonShell, "Exits the JSON shell", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New JsonShell_ExitCommand)},
            {"exitnosave", New CommandInfo("exitnosave", ShellType.JsonShell, "Exits the JSON shell without saving the changes", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New JsonShell_ExitNoSaveCommand)},
            {"help", New CommandInfo("help", ShellType.JsonShell, "Lists available commands", New CommandArgumentInfo({"[command]"}, False, 0), New JsonShell_HelpCommand)},
            {"print", New CommandInfo("print", ShellType.JsonShell, "Prints the JSON file", New CommandArgumentInfo({"[property]"}, False, 0), New JsonShell_PrintCommand)},
            {"save", New CommandInfo("save", ShellType.JsonShell, "Saves the JSON file", New CommandArgumentInfo({"[-b|-m]"}, False, 0), New JsonShell_SaveCommand)}
        }
        Public JsonShell_FileStream As FileStream
        Public JsonShell_FileToken As JToken = JToken.Parse("{}")
        Public JsonShell_AutoSave As New KernelThread("JSON Shell Autosave Thread", False, AddressOf JsonShell_HandleAutoSaveJsonFile)
        Public JsonShell_AutoSaveFlag As Boolean = True
        Public JsonShell_AutoSaveInterval As Integer = 60
        Public JsonShell_PromptStyle As String = ""
        Public JsonShell_Formatting As Formatting = Formatting.Indented
        Friend JsonShell_FileTokenOrig As JToken = JToken.Parse("{}")
        Friend ReadOnly JsonShell_ModCommands As New Dictionary(Of String, CommandInfo)

    End Module
End Namespace

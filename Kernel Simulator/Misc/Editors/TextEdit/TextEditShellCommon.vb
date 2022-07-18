
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

Imports System.IO
Imports KS.Misc.Editors.TextEdit.Commands

Namespace Misc.Editors.TextEdit
    Public Module TextEditShellCommon

        'Variables
        Public ReadOnly TextEdit_Commands As New Dictionary(Of String, CommandInfo) From {{"addline", New CommandInfo("addline", ShellType.TextShell, "Adds a new line with text at the end of the file", {"<text>"}, True, 1, New TextEdit_AddLineCommand)},
                                                                                          {"addlines", New CommandInfo("addlines", ShellType.TextShell, "Adds the new lines at the end of the file", Array.Empty(Of String), False, 0, New TextEdit_AddLinesCommand)},
                                                                                          {"clear", New CommandInfo("clear", ShellType.TextShell, "Clears the text file", Array.Empty(Of String), False, 0, New TextEdit_ClearCommand)},
                                                                                          {"delcharnum", New CommandInfo("delcharnum", ShellType.TextShell, "Deletes a character from character number in specified line", {"<charnumber> <linenumber>"}, True, 2, New TextEdit_DelCharNumCommand)},
                                                                                          {"delline", New CommandInfo("delline", ShellType.TextShell, "Removes the specified line number", {"<linenumber> [linenumber2]"}, True, 1, New TextEdit_DelLineCommand)},
                                                                                          {"delword", New CommandInfo("delword", ShellType.TextShell, "Deletes a word or phrase from line number", {"""<word/phrase>"" <linenumber> [linenumber2]"}, True, 2, New TextEdit_DelWordCommand)},
                                                                                          {"editline", New CommandInfo("editline", ShellType.TextShell, "Edits the specified line", {"<linenumber>"}, True, 1, New TextEdit_EditLineCommand)},
                                                                                          {"exit", New CommandInfo("exit", ShellType.TextShell, "Exits the text editor and save unsaved changes", Array.Empty(Of String), False, 0, New TextEdit_ExitCommand)},
                                                                                          {"exitnosave", New CommandInfo("exitnosave", ShellType.TextShell, "Exits the text editor", Array.Empty(Of String), False, 0, New TextEdit_ExitNoSaveCommand)},
                                                                                          {"help", New CommandInfo("help", ShellType.TextShell, "Lists available commands", {"[command]"}, False, 0, New TextEdit_HelpCommand)},
                                                                                          {"print", New CommandInfo("print", ShellType.TextShell, "Prints the contents of the file with line numbers to the console", {"[linenumber] [linenumber2]"}, False, 0, New TextEdit_PrintCommand)},
                                                                                          {"querychar", New CommandInfo("querychar", ShellType.TextShell, "Queries a character in a specified line or all lines", {"<char> <linenumber/all> [linenumber2]"}, True, 2, New TextEdit_QueryCharCommand)},
                                                                                          {"queryword", New CommandInfo("queryword", ShellType.TextShell, "Queries a word in a specified line or all lines", {"""<word/phrase>"" <linenumber/all> [linenumber2]"}, True, 2, New TextEdit_QueryWordCommand)},
                                                                                          {"querywordregex", New CommandInfo("querywordregex", ShellType.TextShell, "Queries a word in a specified line or all lines using regular expressions", {"""<regex>"" <linenumber/all> [linenumber2]"}, True, 2, New TextEdit_QueryWordRegexCommand)},
                                                                                          {"replace", New CommandInfo("replace", ShellType.TextShell, "Replaces a word or phrase with another one", {"""<word/phrase>"" ""<word/phrase>"""}, True, 2, New TextEdit_ReplaceCommand)},
                                                                                          {"replaceinline", New CommandInfo("replaceinline", ShellType.TextShell, "Replaces a word or phrase with another one in a line", {"""<word/phrase>"" ""<word/phrase>"" <linenumber> [linenumber2]"}, True, 3, New TextEdit_ReplaceInlineCommand)},
                                                                                          {"replaceregex", New CommandInfo("replaceregex", ShellType.TextShell, "Replaces a word or phrase with another one using regular expressions", {"""<regex>"" ""<word/phrase>"""}, True, 2, New TextEdit_ReplaceRegexCommand)},
                                                                                          {"replaceinlineregex", New CommandInfo("replaceinlineregex", ShellType.TextShell, "Replaces a word or phrase with another one in a line using regular expressions", {"""<regex>"" ""<word/phrase>"" <linenumber> [linenumber2]"}, True, 3, New TextEdit_ReplaceInlineRegexCommand)},
                                                                                          {"save", New CommandInfo("save", ShellType.TextShell, "Saves the file", Array.Empty(Of String), False, 0, New TextEdit_SaveCommand)}}
        Public TextEdit_FileLines As List(Of String)
        Public TextEdit_FileStream As FileStream
        Public TextEdit_AutoSave As New KernelThread("Text Edit Autosave Thread", False, AddressOf TextEdit_HandleAutoSaveTextFile)
        Public TextEdit_AutoSaveFlag As Boolean = True
        Public TextEdit_AutoSaveInterval As Integer = 60
        Public TextEdit_PromptStyle As String = ""
        Friend ReadOnly TextEdit_ModCommands As New Dictionary(Of String, CommandInfo)
        Friend TextEdit_FileLinesOrig As List(Of String)

    End Module
End Namespace

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
Imports System.Threading
Imports KS.Misc.HexEdit.Commands

Namespace Misc.HexEdit
    Public Module HexEditShellCommon

        'Variables
        Public ReadOnly HexEdit_Commands As New Dictionary(Of String, CommandInfo) From {{"addbyte", New CommandInfo("addbyte", ShellType.HexShell, "Adds a new byte at the end of the file", {"<byte>"}, True, 1, New HexEdit_AddByteCommand)},
                                                                                         {"addbytes", New CommandInfo("addbytes", ShellType.HexShell, "Adds the new bytes at the end of the file", {}, False, 0, New HexEdit_AddBytesCommand)},
                                                                                         {"clear", New CommandInfo("clear", ShellType.HexShell, "Clears the binary file", {}, False, 0, New HexEdit_ClearCommand)},
                                                                                         {"delbyte", New CommandInfo("delbyte", ShellType.HexShell, "Deletes a byte using the byte number", {"<bytenumber>"}, True, 1, New HexEdit_DelByteCommand)},
                                                                                         {"delbytes", New CommandInfo("delbytes", ShellType.HexShell, "Deletes the range of bytes", {"<startbyte> [endbyte]"}, True, 1, New HexEdit_DelBytesCommand)},
                                                                                         {"exit", New CommandInfo("exit", ShellType.HexShell, "Exits the hex editor and save unsaved changes", {}, False, 0, New HexEdit_ExitCommand)},
                                                                                         {"exitnosave", New CommandInfo("exitnosave", ShellType.HexShell, "Exits the hex editor", {}, False, 0, New HexEdit_ExitNoSaveCommand)},
                                                                                         {"help", New CommandInfo("help", ShellType.HexShell, "Lists available commands", {"[command]"}, False, 0, New HexEdit_HelpCommand)},
                                                                                         {"print", New CommandInfo("print", ShellType.HexShell, "Prints the contents of the file with byte numbers to the console", {"[startbyte] [endbyte]"}, False, 0, New HexEdit_PrintCommand)},
                                                                                         {"querybyte", New CommandInfo("querybyte", ShellType.HexShell, "Queries a byte in a specified range of bytes or all bytes", {"<byte> [startbyte] [endbyte]"}, True, 1, New HexEdit_QueryByteCommand)},
                                                                                         {"replace", New CommandInfo("replace", ShellType.HexShell, "Replaces a byte with another one", {"<byte> <replacedbyte>"}, True, 2, New HexEdit_ReplaceCommand)},
                                                                                         {"save", New CommandInfo("save", ShellType.HexShell, "Saves the file", {}, False, 0, New HexEdit_SaveCommand)}}
        Public HexEdit_ModCommands As New ArrayList
        Public HexEdit_FileStream As FileStream
        Public HexEdit_FileBytes As Byte()
        Friend HexEdit_FileBytesOrig As Byte()
        Public HexEdit_AutoSave As New KernelThread("Hex Edit Autosave Thread", False, AddressOf HexEdit_HandleAutoSaveBinaryFile)
        Public HexEdit_AutoSaveFlag As Boolean = True
        Public HexEdit_AutoSaveInterval As Integer = 60
        Public HexEdit_PromptStyle As String = ""

    End Module
End Namespace
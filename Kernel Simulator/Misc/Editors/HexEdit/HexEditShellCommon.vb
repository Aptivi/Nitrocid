﻿
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
Imports KS.Misc.Editors.HexEdit.Commands

Namespace Misc.Editors.HexEdit
    Public Module HexEditShellCommon

        'Variables
        Public ReadOnly HexEdit_Commands As New Dictionary(Of String, CommandInfo) From {
            {"addbyte", New CommandInfo("addbyte", ShellType.HexShell, "Adds a new byte at the end of the file", New CommandArgumentInfo({"<byte>"}, True, 1), New HexEdit_AddByteCommand)},
            {"addbytes", New CommandInfo("addbytes", ShellType.HexShell, "Adds the new bytes at the end of the file", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New HexEdit_AddBytesCommand)},
            {"clear", New CommandInfo("clear", ShellType.HexShell, "Clears the binary file", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New HexEdit_ClearCommand)},
            {"delbyte", New CommandInfo("delbyte", ShellType.HexShell, "Deletes a byte using the byte number", New CommandArgumentInfo({"<bytenumber>"}, True, 1), New HexEdit_DelByteCommand)},
            {"delbytes", New CommandInfo("delbytes", ShellType.HexShell, "Deletes the range of bytes", New CommandArgumentInfo({"<startbyte> [endbyte]"}, True, 1), New HexEdit_DelBytesCommand)},
            {"exitnosave", New CommandInfo("exitnosave", ShellType.HexShell, "Exits the hex editor", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New HexEdit_ExitNoSaveCommand)},
            {"help", New CommandInfo("help", ShellType.HexShell, "Lists available commands", New CommandArgumentInfo({"[command]"}, False, 0), New HexEdit_HelpCommand)},
            {"print", New CommandInfo("print", ShellType.HexShell, "Prints the contents of the file with byte numbers to the console", New CommandArgumentInfo({"[startbyte] [endbyte]"}, False, 0), New HexEdit_PrintCommand)},
            {"querybyte", New CommandInfo("querybyte", ShellType.HexShell, "Queries a byte in a specified range of bytes or all bytes", New CommandArgumentInfo({"<byte> [startbyte] [endbyte]"}, True, 1), New HexEdit_QueryByteCommand)},
            {"replace", New CommandInfo("replace", ShellType.HexShell, "Replaces a byte with another one", New CommandArgumentInfo({"<byte> <replacedbyte>"}, True, 2), New HexEdit_ReplaceCommand)},
            {"save", New CommandInfo("save", ShellType.HexShell, "Saves the file", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New HexEdit_SaveCommand)}
        }
        Public HexEdit_FileStream As FileStream
        Public HexEdit_FileBytes As List(Of Byte)
        Public HexEdit_AutoSave As New KernelThread("Hex Edit Autosave Thread", False, AddressOf HexEdit_HandleAutoSaveBinaryFile)
        Public HexEdit_AutoSaveFlag As Boolean = True
        Public HexEdit_AutoSaveInterval As Integer = 60
        Friend HexEdit_FileBytesOrig As Byte()
        Friend ReadOnly HexEdit_ModCommands As New Dictionary(Of String, CommandInfo)

    End Module
End Namespace
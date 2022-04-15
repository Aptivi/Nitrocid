
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

Imports System.Threading
Imports KS.Misc.Execution

Namespace Shell.ShellBase
    Module ShellStartThreads

        ''' <summary>
        ''' Master start command thread
        ''' </summary>
        Public StartCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "Shell Command Thread"}
        ''' <summary>
        ''' Process start command thread
        ''' </summary>
        Public ProcessStartCommandThread As New Thread(AddressOf ExecuteProcess) With {.Name = "Executable Command Thread"}
        ''' <summary>
        ''' Text editor start command thread
        ''' </summary>
        Public TextEdit_CommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "Text Edit Command Thread"}
        ''' <summary>
        ''' Zip start command thread
        ''' </summary>
        Public ZipShell_CommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "ZIP Shell Command Thread"}
        ''' <summary>
        ''' FTP start command thread
        ''' </summary>
        Public FTPStartCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "FTP Command Thread"}
        ''' <summary>
        ''' RSS start command thread
        ''' </summary>
        Public RSSCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "RSS Shell Command Thread"}
        ''' <summary>
        ''' Mail start command thread
        ''' </summary>
        Public MailStartCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "Mail Command Thread"}
        ''' <summary>
        ''' SFTP start command thread
        ''' </summary>
        Public SFTPStartCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "SFTP Command Thread"}
        ''' <summary>
        ''' Test start command thread
        ''' </summary>
        Public TStartCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "Test Shell Command Thread"}
        ''' <summary>
        ''' JSON start command thread
        ''' </summary>
        Public JsonShell_CommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "JSON Shell Command Thread"}
        ''' <summary>
        ''' HTTP start command thread
        ''' </summary>
        Public HTTPCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "HTTP Shell Command Thread"}
        ''' <summary>
        ''' Hex editor start command thread
        ''' </summary>
        Public HexEditorCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "Hex Editor Shell Command Thread"}

    End Module
End Namespace


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

Namespace Shell.ShellBase
    Public Module CancellationInstallers

        ''' <summary>
        ''' Switches the command cancellation handler for the shell
        ''' </summary>
        ''' <param name="ShellType">Target shell type</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        ''' <remarks>This is the workaround for a bug in .NET Framework regarding <see cref="Console.CancelKeyPress"/> event. More info can be found below:<br>
        ''' </br><see href="https://stackoverflow.com/a/22717063/6688914">Deep explanation of the bug</see></remarks>
        Public Function SwitchCancellationHandler(ShellType As ShellType) As Boolean
            LastShellType = CurrentShellType
            Select Case ShellType
                Case ShellType.FTPShell
                    AddHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf TestCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HexEditorCancelCommand
                Case ShellType.JsonShell
                    AddHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf TestCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HexEditorCancelCommand
                Case ShellType.MailShell
                    AddHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf TestCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HexEditorCancelCommand
                Case ShellType.RSSShell
                    AddHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf TestCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HexEditorCancelCommand
                Case ShellType.SFTPShell
                    AddHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf TestCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HexEditorCancelCommand
                Case ShellType.Shell
                    AddHandler Console.CancelKeyPress, AddressOf CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf TestCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HexEditorCancelCommand
                Case ShellType.TestShell
                    AddHandler Console.CancelKeyPress, AddressOf TestCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HexEditorCancelCommand
                Case ShellType.TextShell
                    AddHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf TestCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HexEditorCancelCommand
                Case ShellType.ZIPShell
                    AddHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf TestCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HexEditorCancelCommand
                Case ShellType.HTTPShell
                    AddHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf TestCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HexEditorCancelCommand
                Case ShellType.HexShell
                    AddHandler Console.CancelKeyPress, AddressOf HexEditorCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf FTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf EditorCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf JsonShell_CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf MailCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf RssShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf SFTPCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf TestCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf CancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf ZipShellCancelCommand
                    RemoveHandler Console.CancelKeyPress, AddressOf HTTPCancelCommand
                Case Else
                    Return False
            End Select
            CurrentShellType = ShellType
            Return True
        End Function

    End Module
End Namespace


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

Imports KS.Misc.Editors.JsonShell
Imports KS.Misc.Editors.TextEdit
Imports KS.Misc.Editors.HexEdit
Imports KS.Misc.ZipFile
Imports KS.Network.FTP
Imports KS.Network.Mail
Imports KS.Network.RSS
Imports KS.Network.SFTP
Imports KS.Shell.ShellBase.Shells
Imports KS.TestShell

Namespace Shell.Prompts
    Public Module CustomShellPromptStyle

        ''' <summary>
        ''' Gets the custom shell prompt style
        ''' </summary>
        ''' <param name="ShellType">The shell type</param>
        Public Function GetCustomShellPromptStyle(ShellType As ShellType) As String
            Select Case ShellType
                Case ShellType.Shell
                    Return ShellPromptStyle
                Case ShellType.TestShell
                    Return Test_PromptStyle
                Case ShellType.ZIPShell
                    Return ZipShell_PromptStyle
                Case ShellType.TextShell
                    Return TextEdit_PromptStyle
                Case ShellType.SFTPShell
                    Return SFTPShellPromptStyle
                Case ShellType.RSSShell
                    Return RSSShellPromptStyle
                Case ShellType.MailShell
                    Return MailShellPromptStyle
                Case ShellType.JsonShell
                    Return JsonShell_PromptStyle
                Case ShellType.HexShell
                    Return HexEdit_PromptStyle
                Case ShellType.FTPShell
                    Return FTPShellPromptStyle
                Case Else
                    Return ShellPromptStyle
            End Select
        End Function

    End Module
End Namespace

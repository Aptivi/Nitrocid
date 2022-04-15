
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

Imports KS.Misc.JsonShell
Imports KS.Misc.TextEdit
Imports KS.Misc.HexEdit
Imports KS.Misc.ZipFile
Imports KS.Network.FTP
Imports KS.Network.HTTP
Imports KS.Network.Mail
Imports KS.Network.RemoteDebug
Imports KS.Network.RSS
Imports KS.Network.SFTP
Imports KS.TestShell

Namespace Shell.ShellBase
    Public Module CommandManager

        ''' <summary>
        ''' Checks to see if the command is found in selected shell command type
        ''' </summary>
        ''' <param name="Command">A command</param>
        ''' <param name="ShellType">The shell type</param>
        ''' <returns>True if found; False if not found or shell type is invalid.</returns>
        Public Function IsCommandFound(Command As String, ShellType As ShellType) As Boolean
            Wdbg(DebugLevel.I, "Command: {0}, ShellType: {1}", Command, ShellType)
            Select Case ShellType
                Case ShellType.FTPShell
                    Return FTPCommands.ContainsKey(Command)
                Case ShellType.JsonShell
                    Return JsonShell_Commands.ContainsKey(Command)
                Case ShellType.MailShell
                    Return MailCommands.ContainsKey(Command)
                Case ShellType.RemoteDebugShell
                    Return DebugCommands.ContainsKey(Command)
                Case ShellType.RSSShell
                    Return RSSCommands.ContainsKey(Command)
                Case ShellType.SFTPShell
                    Return SFTPCommands.ContainsKey(Command)
                Case ShellType.Shell
                    Return Shell.Commands.ContainsKey(Command)
                Case ShellType.TestShell
                    Return Test_Commands.ContainsKey(Command)
                Case ShellType.TextShell
                    Return TextEdit_Commands.ContainsKey(Command)
                Case ShellType.ZIPShell
                    Return ZipShell_Commands.ContainsKey(Command)
                Case ShellType.HTTPShell
                    Return HTTPCommands.ContainsKey(Command)
                Case ShellType.HexShell
                    Return HexEdit_Commands.ContainsKey(Command)
                Case Else
                    Return False
            End Select
        End Function

        ''' <summary>
        ''' Checks to see if the command is found in all the shells
        ''' </summary>
        ''' <param name="Command">A command</param>
        ''' <returns>True if found; False if not found.</returns>
        Public Function IsCommandFound(Command As String) As Boolean
            Wdbg(DebugLevel.I, "Command: {0}", Command)
            Return FTPCommands.ContainsKey(Command) Or
                   JsonShell_Commands.ContainsKey(Command) Or
                   MailCommands.ContainsKey(Command) Or
                   DebugCommands.ContainsKey(Command) Or
                   RSSCommands.ContainsKey(Command) Or
                   SFTPCommands.ContainsKey(Command) Or
                   Shell.Commands.ContainsKey(Command) Or
                   Test_Commands.ContainsKey(Command) Or
                   TextEdit_Commands.ContainsKey(Command) Or
                   ZipShell_Commands.ContainsKey(Command) Or
                   HTTPCommands.ContainsKey(Command) Or
                   HexEdit_Commands.ContainsKey(Command)
        End Function

    End Module
End Namespace


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

Imports KS.Network.FTP.Commands

Namespace Network.FTP
    Public Module FTPShellCommon

        Public ReadOnly FTPCommands As New Dictionary(Of String, CommandInfo) From {
            {"connect", New CommandInfo("connect", ShellType.FTPShell, "Connects to an FTP server (it must start with ""ftp://"" or ""ftps://"")", New CommandArgumentInfo({"<server>"}, True, 1), New FTP_ConnectCommand)},
            {"cdl", New CommandInfo("cdl", ShellType.FTPShell, "Changes local directory to download to or upload from", New CommandArgumentInfo({"<directory>"}, True, 1), New FTP_CdlCommand)},
            {"cdr", New CommandInfo("cdr", ShellType.FTPShell, "Changes remote directory to download from or upload to", New CommandArgumentInfo({"<directory>"}, True, 1), New FTP_CdrCommand)},
            {"cp", New CommandInfo("cp", ShellType.FTPShell, "Copies file or directory to another file or directory.", New CommandArgumentInfo({"<sourcefileordir> <targetfileordir>"}, True, 2), New FTP_CpCommand)},
            {"del", New CommandInfo("del", ShellType.FTPShell, "Deletes remote file from server", New CommandArgumentInfo({"<file>"}, True, 1), New FTP_DelCommand)},
            {"disconnect", New CommandInfo("disconnect", ShellType.FTPShell, "Disconnects from server", New CommandArgumentInfo({"[-f]"}, False, 0), New FTP_DisconnectCommand, False, False, False, False, False)},
            {"execute", New CommandInfo("execute", ShellType.FTPShell, "Executes an FTP server command", New CommandArgumentInfo({"<command>"}, True, 1), New FTP_ExecuteCommand)},
            {"get", New CommandInfo("get", ShellType.FTPShell, "Downloads remote file to local directory using binary or text", New CommandArgumentInfo({"<file> [output]"}, True, 1), New FTP_GetCommand)},
            {"getfolder", New CommandInfo("getfolder", ShellType.FTPShell, "Downloads remote folder to local directory using binary or text", New CommandArgumentInfo({"<folder> [outputfolder]"}, True, 1), New FTP_GetFolderCommand)},
            {"help", New CommandInfo("help", ShellType.FTPShell, "Shows help screen", New CommandArgumentInfo({"[command]"}, False, 0), New FTP_HelpCommand)},
            {"info", New CommandInfo("info", ShellType.FTPShell, "FTP server information", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New FTP_InfoCommand)},
            {"lsl", New CommandInfo("lsl", ShellType.FTPShell, "Lists local directory", New CommandArgumentInfo({"[-showdetails|-suppressmessages] [dir]"}, False, 0), New FTP_LslCommand, False, False, False, False, False)},
            {"lsr", New CommandInfo("lsr", ShellType.FTPShell, "Lists remote directory", New CommandArgumentInfo({"[-showdetails] [dir]"}, False, 0), New FTP_LsrCommand, False, False, False, False, False)},
            {"mv", New CommandInfo("mv", ShellType.FTPShell, "Moves file or directory to another file or directory. You can also use that to rename files.", New CommandArgumentInfo({"<sourcefileordir> <targetfileordir>"}, True, 2), New FTP_MvCommand)},
            {"put", New CommandInfo("put", ShellType.FTPShell, "Uploads local file to remote directory using binary or text", New CommandArgumentInfo({"<file> [output]"}, True, 1), New FTP_PutCommand)},
            {"putfolder", New CommandInfo("putfolder", ShellType.FTPShell, "Uploads local folder to remote directory using binary or text", New CommandArgumentInfo({"<folder> [outputfolder]"}, True, 1), New FTP_PutFolderCommand)},
            {"pwdl", New CommandInfo("pwdl", ShellType.FTPShell, "Gets current local directory", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New FTP_PwdlCommand)},
            {"pwdr", New CommandInfo("pwdr", ShellType.FTPShell, "Gets current remote directory", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New FTP_PwdrCommand)},
            {"perm", New CommandInfo("perm", ShellType.FTPShell, "Sets file permissions. This is supported only on FTP servers that run Unix.", New CommandArgumentInfo({"<file> <permnumber>"}, True, 2), New FTP_PermCommand)},
            {"quickconnect", New CommandInfo("quickconnect", ShellType.FTPShell, "Uses information from Speed Dial to connect to any network quickly", New CommandArgumentInfo(Array.Empty(Of String), False, 0), New FTP_QuickConnectCommand)},
            {"sumfile", New CommandInfo("sumfile", ShellType.FTPShell, "Calculates file sums.", New CommandArgumentInfo({"<file> <MD5/SHA1/SHA256/SHA512/CRC>"}, True, 2), New FTP_SumFileCommand)},
            {"sumfiles", New CommandInfo("sumfiles", ShellType.FTPShell, "Calculates sums of files in specified directory.", New CommandArgumentInfo({"<file> <MD5/SHA1/SHA256/SHA512/CRC>"}, True, 2), New FTP_SumFilesCommand)},
            {"type", New CommandInfo("type", ShellType.FTPShell, "Sets the type for this session", New CommandArgumentInfo({"<a/b>"}, True, 1), New FTP_TypeCommand)}
        }
        Public FtpCurrentDirectory As String
        Public FtpCurrentRemoteDir As String
        Public FtpShowDetailsInList As Boolean = True
        Public FtpUserPromptStyle As String = ""
        Public FtpPassPromptStyle As String = ""
        Public FtpUseFirstProfile As Boolean
        Public FtpNewConnectionsToSpeedDial As Boolean = True
        Public FtpTryToValidateCertificate As Boolean = True
        Public FtpRecursiveHashing As Boolean
        Public FtpShowMotd As Boolean = True
        Public FtpAlwaysAcceptInvalidCerts As Boolean
        Public FtpVerifyRetryAttempts As Integer = 3
        Public FtpConnectTimeout As Integer = 15000
        Public FtpDataConnectTimeout As Integer = 15000
        Public FtpProtocolVersions As FtpIpVersion = FtpIpVersion.ANY
        Friend _clientFTP As FtpClient
        Friend FtpConnected As Boolean
        Friend FtpSite As String
        Friend FtpPass As String
        Friend FtpUser As String
        Friend ReadOnly FTPModCommands As New Dictionary(Of String, CommandInfo)

        ''' <summary>
        ''' The FTP client used to connect to the FTP server
        ''' </summary>
        Public ReadOnly Property ClientFTP As FtpClient
            Get
                Return _clientFTP
            End Get
        End Property

    End Module
End Namespace


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
Imports System.Threading

Public Module FTPGetCommand

    'FTP Client and thread
    Public FTPStartCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "FTP Command Thread"}

    ''' <summary>
    ''' Parses and executes the FTP command
    ''' </summary>
    ''' <param name="requestedCommand">A command. It may come with arguments</param>
    Public Sub ExecuteCommand(ByVal requestedCommand As String)
        'Variables
        Dim ArgumentInfo As New ProvidedCommandArgumentsInfo(requestedCommand, ShellCommandType.FTPShell)
        Dim Command As String = ArgumentInfo.Command
        Dim eqargs() As String = ArgumentInfo.ArgumentsList
        Dim strArgs As String = ArgumentInfo.ArgumentsText
        Dim RequiredArgumentsProvided As Boolean = ArgumentInfo.RequiredArgumentsProvided

        '5. Check to see if a requested command is obsolete
        If FTPCommands(Command).Obsolete Then
            Wdbg("I", "The command requested {0} is obsolete", Command)
            W(DoTranslation("This command is obsolete and will be removed in a future release."), True, ColTypes.Neutral)
        End If

        '6. Execute a command
        Try
            Select Case Command
                Case "connect"
                    If RequiredArgumentsProvided Then
                        If eqargs(0).StartsWith("ftp://") Or eqargs(0).StartsWith("ftps://") Or eqargs(0).StartsWith("ftpes://") Then
                            TryToConnect(eqargs(0))
                        Else
                            TryToConnect($"ftp://{eqargs(0)}")
                        End If
                    Else
                        W(DoTranslation("Enter an FTP server."), True, ColTypes.Neutral)
                    End If
                Case "cdl"
                    FTPChangeLocalDir(eqargs(0))
                Case "cdr"
                    FTPChangeRemoteDir(eqargs(0))
                Case "cp"
                    If RequiredArgumentsProvided Then
                        If FtpConnected Then
                            W(DoTranslation("Copying {0} to {1}..."), True, ColTypes.Neutral, eqargs(0), eqargs(1))
                            If FTPCopyItem(eqargs(0), eqargs(1)) Then
                                W(vbNewLine + DoTranslation("Copied successfully"), True, ColTypes.Neutral)
                            Else
                                W(vbNewLine + DoTranslation("Failed to copy {0} to {1}."), True, ColTypes.Neutral, eqargs(0), eqargs(1))
                            End If
                        Else
                            W(DoTranslation("You must connect to server before performing transmission."), True, ColTypes.Error)
                        End If
                    Else
                        W(DoTranslation("Enter a source path and a destination path."), True, ColTypes.Error)
                    End If
                Case "pwdl"
                    W(DoTranslation("Local directory: {0}"), True, ColTypes.Neutral, FtpCurrentDirectory)
                Case "pwdr"
                    If FtpConnected = True Then
                        W(DoTranslation("Remote directory: {0}"), True, ColTypes.Neutral, FtpCurrentRemoteDir)
                    Else
                        W(DoTranslation("You must connect to server before getting current remote directory."), True, ColTypes.Error)
                    End If
                Case "del"
                    If RequiredArgumentsProvided Then
                        If FtpConnected = True Then
                            'Print a message
                            W(DoTranslation("Deleting {0}..."), True, ColTypes.Neutral, eqargs(0))

                            'Make a confirmation message so user will not accidentally delete a file or folder
                            W(DoTranslation("Are you sure you want to delete {0} <y/n>?") + " ", False, ColTypes.Input, eqargs(0))
                            Dim answer As String = Console.ReadKey.KeyChar
                            Console.WriteLine()

                            Try
                                FTPDeleteRemote(eqargs(0))
                            Catch ex As Exception
                                W(ex.Message, True, ColTypes.Error)
                            End Try
                        Else
                            W(DoTranslation("You must connect to server with administrative privileges before performing the deletion."), True, ColTypes.Error)
                        End If
                    Else
                        W(DoTranslation("Enter a file or folder to remove. You must have administrative permissions on your account to be able to remove."), True, ColTypes.Error)
                    End If
                Case "disconnect"
                    If FtpConnected = True Then
                        'Set a connected flag to False
                        FtpConnected = False
                        ClientFTP.Disconnect()
                        W(DoTranslation("Disconnected from {0}"), True, ColTypes.Neutral, ftpsite)

                        'Clean up everything
                        ftpsite = ""
                        FtpCurrentRemoteDir = ""
                        FtpUser = ""
                        FtpPass = ""
                    Else
                        W(DoTranslation("You haven't connected to any server yet"), True, ColTypes.Error)
                    End If
                Case "get"
                    If RequiredArgumentsProvided Then
                        Dim RemoteFile As String = eqargs(0)
                        Dim LocalFile As String = If(eqargs.Count > 1, eqargs(1), "")
                        W(DoTranslation("Downloading file {0}..."), False, ColTypes.Neutral, RemoteFile)
                        Dim Result As Boolean = If(Not String.IsNullOrWhiteSpace(LocalFile), FTPGetFile(RemoteFile, LocalFile), FTPGetFile(RemoteFile))
                        If Result Then
                            Console.WriteLine()
                            W(DoTranslation("Downloaded file {0}."), True, ColTypes.Neutral, RemoteFile)
                        Else
                            Console.WriteLine()
                            W(DoTranslation("Download failed for file {0}."), True, ColTypes.Error, RemoteFile)
                        End If
                    Else
                        W(DoTranslation("Enter a file to download to local directory."), True, ColTypes.Error)
                    End If
                Case "getfolder"
                    If RequiredArgumentsProvided Then
                        Dim RemoteFolder As String = eqargs(0)
                        Dim LocalFolder As String = If(eqargs.Count > 1, eqargs(1), "")
                        W(DoTranslation("Downloading folder {0}..."), True, ColTypes.Neutral, RemoteFolder)
                        Dim Result As Boolean = If(Not String.IsNullOrWhiteSpace(LocalFolder), FTPGetFolder(RemoteFolder, LocalFolder), FTPGetFolder(RemoteFolder))
                        If Result Then
                            Console.WriteLine()
                            W(DoTranslation("Downloaded folder {0}."), True, ColTypes.Neutral, RemoteFolder)
                        Else
                            Console.WriteLine()
                            W(DoTranslation("Download failed for folder {0}."), True, ColTypes.Error, RemoteFolder)
                        End If
                    Else
                        W(DoTranslation("Enter a folder to download to local directory."), True, ColTypes.Error)
                    End If
                Case "lsl"
                    If eqargs?.Length > 0 And eqargs IsNot Nothing Then
                        List(eqargs(0))
                    Else
                        List(FtpCurrentDirectory)
                    End If
                Case "lsr"
                    Dim Entries As List(Of String) = FTPListRemote(If(eqargs IsNot Nothing, eqargs(0), ""))
                    Entries.Sort()
                    For Each Entry As String In Entries
                        W(Entry, True, ColTypes.ListEntry)
                    Next
                Case "mv"
                    If RequiredArgumentsProvided Then
                        If FtpConnected Then
                            W(DoTranslation("Moving {0} to {1}..."), True, ColTypes.Neutral, eqargs(0), eqargs(1))
                            If FTPMoveItem(eqargs(0), eqargs(1)) Then
                                W(vbNewLine + DoTranslation("Moved successfully"), True, ColTypes.Neutral)
                            Else
                                W(vbNewLine + DoTranslation("Failed to move {0} to {1}."), True, ColTypes.Neutral, eqargs(0), eqargs(1))
                            End If
                        Else
                            W(DoTranslation("You must connect to server before performing transmission."), True, ColTypes.Error)
                        End If
                    Else
                        W(DoTranslation("Enter a source path and a destination path."), True, ColTypes.Error)
                    End If
                Case "quickconnect"
                    If Not FtpConnected Then
                        QuickConnect()
                    Else
                        W(DoTranslation("You should disconnect from server before connecting to another server"), True, ColTypes.Error)
                    End If
                Case "perm"
                    If RequiredArgumentsProvided Then
                        If FtpConnected Then
                            If FTPChangePermissions(eqargs(0), eqargs(1)) Then
                                W(DoTranslation("Permissions set successfully for file") + " {0}", True, ColTypes.Neutral, eqargs(0))
                            Else
                                W(DoTranslation("Failed to set permissions of {0} to {1}."), True, ColTypes.Error, eqargs(0), eqargs(1))
                            End If
                        Else
                            W(DoTranslation("You must connect to server before performing filesystem operations."), True, ColTypes.Error)
                        End If
                    End If
                Case "type"
                    If RequiredArgumentsProvided Then
                        If eqargs(0).ToLower = "a" Then
                            ClientFTP.DownloadDataType = FtpDataType.ASCII
                            ClientFTP.ListingDataType = FtpDataType.ASCII
                            ClientFTP.UploadDataType = FtpDataType.ASCII
                            W(DoTranslation("Data type set to ASCII!"), True, ColTypes.Neutral)
                            W(DoTranslation("Beware that most files won't download or upload properly using this mode, so we highly recommend using the Binary mode on most situations."), True, ColTypes.Warning)
                        ElseIf eqargs(0).ToLower = "b" Then
                            ClientFTP.DownloadDataType = FtpDataType.Binary
                            ClientFTP.ListingDataType = FtpDataType.Binary
                            ClientFTP.UploadDataType = FtpDataType.Binary
                            W(DoTranslation("Data type set to Binary!"), True, ColTypes.Neutral)
                        Else
                            W(DoTranslation("Invalid data type."), True, ColTypes.Neutral)
                        End If
                    End If
                Case "put"
                    If RequiredArgumentsProvided Then
                        W(DoTranslation("Uploading file {0}..."), False, ColTypes.Neutral, eqargs(0))

                        'Begin the uploading process
                        If FTPUploadFile(eqargs(0)) Then
                            Console.WriteLine()
                            W(vbNewLine + DoTranslation("Uploaded file {0}"), True, ColTypes.Neutral, eqargs(0))
                        Else
                            Console.WriteLine()
                            W(vbNewLine + DoTranslation("Failed to upload {0}"), True, ColTypes.Neutral, eqargs(0))
                        End If
                    Else
                        W(DoTranslation("Enter a file to upload to remote directory. upload <file> <directory>"), True, ColTypes.Error)
                    End If
                Case "putfolder"
                    If RequiredArgumentsProvided Then
                        W(DoTranslation("Uploading folder {0}..."), True, ColTypes.Neutral, eqargs(0))

                        'Begin the uploading process
                        If FTPUploadFile(eqargs(0)) Then
                            Console.WriteLine()
                            W(vbNewLine + DoTranslation("Uploaded folder {0}"), True, ColTypes.Neutral, eqargs(0))
                        Else
                            Console.WriteLine()
                            W(vbNewLine + DoTranslation("Failed to upload {0}"), True, ColTypes.Neutral, eqargs(0))
                        End If
                    Else
                        W(DoTranslation("Enter a folder to upload to remote directory."), True, ColTypes.Error)
                    End If
                Case "exit"
                    'Set a flag
                    ftpexit = True
                Case "help"
                    If requestedCommand = "help" Then
                        ShowHelp(ShellCommandType.FTPShell)
                    Else
                        ShowHelp(strArgs, ShellCommandType.FTPShell)
                    End If
            End Select

            'See if the command is done (passed all required arguments)
            If FTPCommands(Command).ArgumentsRequired And Not RequiredArgumentsProvided Then
                W(DoTranslation("Required arguments are not passed to command {0}"), True, ColTypes.Error, Command)
                Wdbg("E", "Passed arguments were not enough to run command {0}. Arguments passed: {1}", Command, eqargs?.Length)
                ShowHelp(Command, ShellCommandType.FTPShell)
            End If
        Catch taex As ThreadAbortException
            Exit Sub
        Catch ex As Exception 'The InnerException CAN be Nothing
            If DebugMode = True Then
                If ex.InnerException IsNot Nothing Then 'This is required to fix NullReferenceException when there is nothing in InnerException, so please don't remove.
                    W(DoTranslation("Error trying to execute FTP command {3}.") + vbNewLine +
                      DoTranslation("Error {0}: {1} ") + DoTranslation("(Inner:") + " {4})" + vbNewLine + "{2}", True, ColTypes.Error, ex.GetType.FullName, ex.Message, ex.StackTrace, Command, ex.InnerException.Message)
                Else
                    W(DoTranslation("Error trying to execute FTP command {3}.") + vbNewLine +
                      DoTranslation("Error {0}: {1}") + vbNewLine + "{2}", True, ColTypes.Error, ex.GetType.FullName, ex.Message, ex.StackTrace, Command)
                End If
                WStkTrc(ex)
            Else
                If ex.InnerException IsNot Nothing Then
                    W(DoTranslation("Error trying to execute FTP command {2}.") + vbNewLine +
                      DoTranslation("Error {0}: {1} ") + DoTranslation("(Inner:") + " {3})", True, ColTypes.Error, ex.GetType.FullName, ex.Message, Command, ex.InnerException.Message)
                Else
                    W(DoTranslation("Error trying to execute FTP command {2}.") + vbNewLine +
                      DoTranslation("Error {0}: {1}"), True, ColTypes.Error, ex.GetType.FullName, ex.Message, Command)
                End If
            End If
            EventManager.RaiseFTPCommandError(requestedCommand, ex)
        End Try

    End Sub

    Sub FTPCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            Console.WriteLine()
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            FTPStartCommandThread.Abort()
        End If
    End Sub

End Module

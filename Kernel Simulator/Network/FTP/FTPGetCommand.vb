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
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic.FileIO

Public Module FTPGetCommand

    'FTP Client and thread
    Public ClientFTP As FtpClient
    Public FTPStartCommandThread As New Thread(AddressOf ExecuteCommand)

    ''' <summary>
    ''' Parses and executes the FTP command
    ''' </summary>
    ''' <param name="cmd">A command. It may come with arguments</param>
    Public Sub ExecuteCommand(ByVal cmd As String)
        'Variables
        Dim RequiredArgumentsProvided As Boolean = True

        'Get command and arguments
        Dim index As Integer = cmd.IndexOf(" ")
        If index = -1 Then index = cmd.Length
        Dim words = cmd.Split({" "c})
        Dim strArgs As String = cmd.Substring(index)
        If Not index = cmd.Length Then strArgs = strArgs.Substring(1)

        'Parse arguments
        Dim ArgsQ() As String
        Dim TStream As New MemoryStream(Encoding.Default.GetBytes(strArgs))
        Dim Parser As New TextFieldParser(TStream) With {
            .Delimiters = {" "},
            .HasFieldsEnclosedInQuotes = True
        }
        ArgsQ = Parser.ReadFields
        If ArgsQ IsNot Nothing Then
            For i As Integer = 0 To ArgsQ.Length - 1
                ArgsQ(i).Replace("""", "")
            Next
            RequiredArgumentsProvided = ArgsQ?.Length >= FTPCommands(words(0)).MinimumArguments
        ElseIf FTPCommands(words(0)).ArgumentsRequired And ArgsQ Is Nothing Then
            RequiredArgumentsProvided = False
        End If

        'Command code
        Try
            If words(0) = "connect" Then
                If RequiredArgumentsProvided Then
                    If ArgsQ(0).StartsWith("ftp://") Or ArgsQ(0).StartsWith("ftps://") Or ArgsQ(0).StartsWith("ftpes://") Then
                        TryToConnect(ArgsQ(0))
                    Else
                        TryToConnect($"ftp://{ArgsQ(0)}")
                    End If
                Else
                    W(DoTranslation("Enter an FTP server."), True, ColTypes.Neutral)
                End If
            ElseIf words(0) = "changelocaldir" Or words(0) = "cdl" Then
                FTPChangeLocalDir(ArgsQ(0))
            ElseIf words(0) = "changeremotedir" Or words(0) = "cdr" Then
                FTPChangeRemoteDir(ArgsQ(0))
            ElseIf words(0) = "copy" Or words(0) = "cp" Then
                If RequiredArgumentsProvided Then
                    If connected Then
                        W(DoTranslation("Copying {0} to {1}..."), True, ColTypes.Neutral, ArgsQ(0), ArgsQ(1))
                        If FTPCopyItem(ArgsQ(0), ArgsQ(1)) Then
                            W(vbNewLine + DoTranslation("Copied successfully"), True, ColTypes.Neutral)
                        Else
                            W(vbNewLine + DoTranslation("Failed to copy {0} to {1}."), True, ColTypes.Neutral, ArgsQ(0), ArgsQ(1))
                        End If
                    Else
                        W(DoTranslation("You must connect to server before performing transmission."), True, ColTypes.Err)
                    End If
                Else
                    W(DoTranslation("Enter a source path and a destination path."), True, ColTypes.Err)
                End If
            ElseIf words(0) = "currlocaldir" Or words(0) = "pwdl" Then
                W(DoTranslation("Local directory: {0}"), True, ColTypes.Neutral, currDirect)
            ElseIf words(0) = "currremotedir" Or words(0) = "pwdr" Then
                If connected = True Then
                    W(DoTranslation("Remote directory: {0}"), True, ColTypes.Neutral, currentremoteDir)
                Else
                    W(DoTranslation("You must connect to server before getting current remote directory."), True, ColTypes.Err)
                End If
            ElseIf words(0) = "delete" Or words(0) = "del" Then
                If RequiredArgumentsProvided Then
                    If connected = True Then
                        'Print a message
                        W(DoTranslation("Deleting {0}..."), True, ColTypes.Neutral, ArgsQ(0))

                        'Make a confirmation message so user will not accidentally delete a file or folder
                        W(DoTranslation("Are you sure you want to delete {0} <y/n>?"), False, ColTypes.Input, ArgsQ(0))
                        Dim answer As String = Console.ReadKey.KeyChar
                        Console.WriteLine()

                        Try
                            FTPDeleteRemote(ArgsQ(0))
                        Catch ex As Exception
                            W(ex.Message, True, ColTypes.Err)
                        End Try
                    Else
                        W(DoTranslation("You must connect to server with administrative privileges before performing the deletion."), True, ColTypes.Err)
                    End If
                Else
                    W(DoTranslation("Enter a file or folder to remove. You must have administrative permissions on your account to be able to remove."), True, ColTypes.Err)
                End If
            ElseIf cmd = "disconnect" Then
                If connected = True Then
                    'Set a connected flag to False
                    connected = False
                    ClientFTP.Disconnect()
                    W(DoTranslation("Disconnected from {0}"), True, ColTypes.Neutral, ftpsite)

                    'Clean up everything
                    ftpsite = ""
                    currentremoteDir = ""
                    user = ""
                    pass = ""
                Else
                    W(DoTranslation("You haven't connected to any server yet"), True, ColTypes.Err)
                End If
            ElseIf words(0) = "download" Or words(0) = "get" Then
                If RequiredArgumentsProvided Then
                    W(DoTranslation("Downloading file {0}..."), False, ColTypes.Neutral, ArgsQ(0))
                    If FTPGetFile(ArgsQ(0)) Then
                        Console.WriteLine()
                        W(DoTranslation("Downloaded file {0}."), True, ColTypes.Neutral, ArgsQ(0))
                    Else
                        Console.WriteLine()
                        W(DoTranslation("Download failed for file {0}."), True, ColTypes.Err, ArgsQ(0))
                    End If
                Else
                    W(DoTranslation("Enter a file to download to local directory."), True, ColTypes.Err)
                End If
            ElseIf cmd = "exit" Then
                'Set a flag
                ftpexit = True
            ElseIf words(0) = "help" Then
                If cmd = "help" Then
                    FTPShowHelp()
                Else
                    FTPShowHelp(strArgs)
                End If
            ElseIf words(0) = "listlocal" Or words(0) = "lsl" Then
                If cmd = "listlocal" Or cmd = "lsl" Then
                    List(CurrDir)
                Else
                    List(ArgsQ(0))
                End If
            ElseIf words(0) = "listremote" Or words(0) = "lsr" Then
                Dim Entries As List(Of String) = FTPListRemote(ArgsQ(0))
                For Each Entry As String In Entries
                    W(Entry, True, ColTypes.ListEntry)
                Next
            ElseIf words(0) = "move" Or words(0) = "mv" Then
                If RequiredArgumentsProvided Then
                    If connected Then
                        W(DoTranslation("Moving {0} to {1}..."), True, ColTypes.Neutral, ArgsQ(0), ArgsQ(1))
                        If FTPMoveItem(ArgsQ(0), ArgsQ(1)) Then
                            W(vbNewLine + DoTranslation("Moved successfully"), True, ColTypes.Neutral)
                        Else
                            W(vbNewLine + DoTranslation("Failed to move {0} to {1}."), True, ColTypes.Neutral, ArgsQ(0), ArgsQ(1))
                        End If
                    Else
                        W(DoTranslation("You must connect to server before performing transmission."), True, ColTypes.Err)
                    End If
                Else
                    W(DoTranslation("Enter a source path and a destination path."), True, ColTypes.Err)
                End If
            ElseIf words(0) = "quickconnect" Then
                If Not connected Then
                    QuickConnect()
                Else
                    W(DoTranslation("You should disconnect from server before connecting to another server"), True, ColTypes.Err)
                End If
            ElseIf words(0) = "upload" Or words(0) = "put" Then
                If RequiredArgumentsProvided Then
                    W(DoTranslation("Uploading file {0}..."), True, ColTypes.Neutral, ArgsQ(0))

                    'Begin the uploading process
                    If FTPUploadFile(ArgsQ(0)) Then
                        Console.WriteLine()
                        W(vbNewLine + DoTranslation("Uploaded file {0}"), True, ColTypes.Neutral, ArgsQ(0))
                    Else
                        Console.WriteLine()
                        W(vbNewLine + DoTranslation("Failed to upload {0}"), True, ColTypes.Neutral, ArgsQ(0))
                    End If
                Else
                    W(DoTranslation("Enter a file to upload to remote directory. upload <file> <directory>"), True, ColTypes.Err)
                End If
            End If
        Catch ex As Exception 'The InnerException CAN be Nothing
            If DebugMode = True Then
                If Not IsNothing(ex.InnerException) Then 'This is required to fix NullReferenceException when there is nothing in InnerException, so please don't remove.
                    W(DoTranslation("Error trying to execute FTP command {3}.") + vbNewLine +
                      DoTranslation("Error {0}: {1} ") + DoTranslation("(Inner:") + " {4})" + vbNewLine + "{2}", True, ColTypes.Err, Err.Number, ex.Message, ex.StackTrace, words(0), ex.InnerException.Message)
                Else
                    W(DoTranslation("Error trying to execute FTP command {3}.") + vbNewLine +
                      DoTranslation("Error {0}: {1}") + vbNewLine + "{2}", True, ColTypes.Err, Err.Number, ex.Message, ex.StackTrace, words(0))
                End If
                WStkTrc(ex)
            Else
                If Not IsNothing(ex.InnerException) Then
                    W(DoTranslation("Error trying to execute FTP command {2}.") + vbNewLine +
                      DoTranslation("Error {0}: {1} ") + DoTranslation("(Inner:") + "{3})", True, ColTypes.Err, Err.Number, ex.Message, words(0), ex.InnerException.Message)
                Else
                    W(DoTranslation("Error trying to execute FTP command {2}.") + vbNewLine +
                      DoTranslation("Error {0}: {1}"), True, ColTypes.Err, Err.Number, ex.Message, words(0))
                End If
            End If
            EventManager.RaiseFTPCommandError(cmd, ex)
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

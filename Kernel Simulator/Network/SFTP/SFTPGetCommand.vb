'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

Public Module SFTPGetCommand

    'SFTP Client and thread
    Public ClientSFTP As SftpClient
    Public SFTPStartCommandThread As New Thread(AddressOf ExecuteCommand)

    ''' <summary>
    ''' Parses and executes the SFTP command
    ''' </summary>
    ''' <param name="cmd">A command. It may come with arguments</param>
    Public Sub ExecuteCommand(ByVal cmd As String)

        Dim index As Integer = cmd.IndexOf(" ")
        If index = -1 Then index = cmd.Length
        Dim words = cmd.Split({" "c})
        Dim strArgs As String = cmd.Substring(index)
        If Not index = cmd.Length Then strArgs = strArgs.Substring(1)
        Dim ArgsQ() As String
        Dim TStream As New MemoryStream(Encoding.Default.GetBytes(strArgs))
        Dim Parser As New TextFieldParser(TStream) With {
            .Delimiters = {" "},
            .HasFieldsEnclosedInQuotes = True
        }
        ArgsQ = Parser.ReadFields
        If Not ArgsQ Is Nothing Then
            For i As Integer = 0 To ArgsQ.Length - 1
                ArgsQ(i).Replace("""", "")
            Next
        End If

        'Command code
        Try
            If words(0) = "connect" Then
                If ArgsQ?.Count <> 0 Then
                    If ArgsQ(0).StartsWith("sftp://") Then
                        SFTPTryToConnect(ArgsQ(0))
                    Else
                        SFTPTryToConnect($"sftp://{ArgsQ(0)}")
                    End If
                Else
                    W(DoTranslation("Enter an SFTP server.", currentLang), True, ColTypes.Neutral)
                End If
            ElseIf words(0) = "changelocaldir" Or words(0) = "cdl" Then
                SFTPChangeLocalDir(strArgs)
            ElseIf words(0) = "changeremotedir" Or words(0) = "cdr" Then
                SFTPChangeRemoteDir(strArgs)
            ElseIf words(0) = "currlocaldir" Or words(0) = "pwdl" Then
                W(DoTranslation("Local directory: {0}", currentLang), True, ColTypes.Neutral, SFTPCurrDirect)
            ElseIf words(0) = "currremotedir" Or words(0) = "pwdr" Then
                If connected = True Then
                    W(DoTranslation("Remote directory: {0}", currentLang), True, ColTypes.Neutral, SFTPCurrentRemoteDir)
                Else
                    W(DoTranslation("You must connect to server before getting current remote directory.", currentLang), True, ColTypes.Err)
                End If
            ElseIf words(0) = "delete" Or words(0) = "del" Then
                If cmd <> "delete" Or cmd <> "del" Then
                    If connected = True Then
                        'Print a message
                        W(DoTranslation("Deleting {0}...", currentLang), True, ColTypes.Neutral, strArgs)

                        'Make a confirmation message so user will not accidentally delete a file or folder
                        W(DoTranslation("Are you sure you want to delete {0} <y/n>?", currentLang), False, ColTypes.Input, strArgs)
                        Dim answer As String = Console.ReadKey.KeyChar
                        Try
                            SFTPDeleteRemote(strArgs)
                        Catch ex As Exception
                            W(ex.Message, True, ColTypes.Err)
                        End Try
                    Else
                        W(DoTranslation("You must connect to server with administrative privileges before performing the deletion.", currentLang), True, ColTypes.Err)
                    End If
                Else
                    W(DoTranslation("Enter a file or folder to remove. You must have administrative permissions on your account to be able to remove.", currentLang), True, ColTypes.Err)
                End If
            ElseIf cmd = "disconnect" Then
                If connected = True Then
                    'Set a connected flag to False
                    connected = False
                    ClientSFTP.Disconnect()
                    W(DoTranslation("Disconnected from {0}", currentLang), True, ColTypes.Neutral, ftpsite)

                    'Clean up everything
                    sftpsite = ""
                    SFTPCurrentRemoteDir = ""
                    SFTPUser = ""
                    SFTPPass = ""
                Else
                    W(DoTranslation("You haven't connected to any server yet", currentLang), True, ColTypes.Err)
                End If
            ElseIf words(0) = "download" Or words(0) = "get" Then
                If cmd <> "download" Or cmd <> "get" Then
                    W(DoTranslation("Downloading file {0}...", currentLang), False, ColTypes.Neutral, strArgs)
                    If SFTPGetFile(strArgs) Then
                        Console.WriteLine()
                        W(DoTranslation("Downloaded file {0}.", currentLang), True, ColTypes.Neutral, strArgs)
                    Else
                        W(DoTranslation("Download failed for file {0}.", currentLang), True, ColTypes.Err, strArgs)
                    End If
                Else
                    W(DoTranslation("Enter a file to download to local directory.", currentLang), True, ColTypes.Err)
                End If
            ElseIf cmd = "exit" Then
                'Set a flag
                sftpexit = True
            ElseIf cmd = "help" Then
                SFTPShowHelp()
            ElseIf words(0) = "listlocal" Or words(0) = "lsl" Then
                If cmd = "listlocal" Or cmd = "lsl" Then
                    List(CurrDir)
                Else
                    List(strArgs)
                End If
            ElseIf words(0) = "listremote" Or words(0) = "lsr" Then
                Dim Entries As List(Of String) = SFTPListRemote(strArgs)
                For Each Entry As String In Entries
                    W(Entry, True, ColTypes.Neutral)
                Next
            ElseIf words(0) = "upload" Or words(0) = "put" Then
                If cmd <> "upload" Or cmd <> "put" Then
                    W(DoTranslation("Uploading file {0}...", currentLang), True, ColTypes.Neutral, strArgs)

                    'Begin the uploading process
                    If SFTPUploadFile(strArgs) Then
                        Console.WriteLine()
                        W(vbNewLine + DoTranslation("Uploaded file {0}", currentLang), True, ColTypes.Neutral, strArgs)
                    Else
                        Console.WriteLine()
                        W(vbNewLine + DoTranslation("Failed to upload {0}", currentLang), True, ColTypes.Neutral, strArgs)
                    End If
                Else
                    W(DoTranslation("Enter a file to upload to remote directory. upload <file> <directory>", currentLang), True, ColTypes.Err)
                End If
            End If
        Catch ex As Exception 'The InnerException CAN be Nothing
            If DebugMode = True Then
                If Not IsNothing(ex.InnerException) Then 'This is required to fix NullReferenceException when there is nothing in InnerException, so please don't remove.
                    W(DoTranslation("Error trying to execute SFTP command {3}.", currentLang) + vbNewLine +
                      DoTranslation("Error {0}: {1} ", currentLang) + DoTranslation("(Inner:", currentLang) + " {4})" + vbNewLine + "{2}", True, ColTypes.Err, Err.Number, ex.Message, ex.StackTrace, words(0), ex.InnerException.Message)
                Else
                    W(DoTranslation("Error trying to execute SFTP command {3}.", currentLang) + vbNewLine +
                      DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, ColTypes.Err, Err.Number, ex.Message, ex.StackTrace, words(0))
                End If
                WStkTrc(ex)
            Else
                If Not IsNothing(ex.InnerException) Then
                    W(DoTranslation("Error trying to execute SFTP command {2}.", currentLang) + vbNewLine +
                      DoTranslation("Error {0}: {1} ", currentLang) + DoTranslation("(Inner:", currentLang) + "{3})", True, ColTypes.Err, Err.Number, ex.Message, words(0), ex.InnerException.Message)
                Else
                    W(DoTranslation("Error trying to execute SFTP command {2}.", currentLang) + vbNewLine +
                      DoTranslation("Error {0}: {1}", currentLang), True, ColTypes.Err, Err.Number, ex.Message, words(0))
                End If
            End If
            EventManager.RaiseFTPCommandError(cmd, ex)
        End Try

    End Sub

    Sub SFTPCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            SFTPStartCommandThread.Abort()
        End If
    End Sub

End Module

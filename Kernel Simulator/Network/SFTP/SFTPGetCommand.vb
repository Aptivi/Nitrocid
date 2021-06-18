
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

Public Module SFTPGetCommand

    'SFTP Client and thread
    Public ClientSFTP As SftpClient
    Public SFTPStartCommandThread As New Thread(AddressOf ExecuteCommand) With {.Name = "SFTP Command Thread"}

    ''' <summary>
    ''' Parses and executes the SFTP command
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
        Dim ArgsQ() As String = strArgs.SplitEncloseDoubleQuotes(" ")
        If ArgsQ IsNot Nothing Then
            RequiredArgumentsProvided = ArgsQ?.Length >= FTPCommands(words(0)).MinimumArguments
        ElseIf FTPCommands(words(0)).ArgumentsRequired And ArgsQ Is Nothing Then
            RequiredArgumentsProvided = False
        End If

        'Command code
        Try
            Select Case words(0)
                Case "connect"
                    If RequiredArgumentsProvided Then
                        If ArgsQ(0).StartsWith("sftp://") Then
                            SFTPTryToConnect(ArgsQ(0))
                        Else
                            SFTPTryToConnect($"sftp://{ArgsQ(0)}")
                        End If
                    Else
                        W(DoTranslation("Enter an SFTP server."), True, ColTypes.Neutral)
                    End If
                Case "cdl"
                    SFTPChangeLocalDir(ArgsQ(0))
                Case "cdr"
                    SFTPChangeRemoteDir(ArgsQ(0))
                Case "pwdl"
                    W(DoTranslation("Local directory: {0}"), True, ColTypes.Neutral, SFTPCurrDirect)
                Case "pwdr"
                    If SFTPConnected = True Then
                        W(DoTranslation("Remote directory: {0}"), True, ColTypes.Neutral, SFTPCurrentRemoteDir)
                    Else
                        W(DoTranslation("You must connect to server before getting current remote directory."), True, ColTypes.Error)
                    End If
                Case "del"
                    If RequiredArgumentsProvided Then
                        If SFTPConnected = True Then
                            'Print a message
                            W(DoTranslation("Deleting {0}..."), True, ColTypes.Neutral, ArgsQ(0))

                            'Make a confirmation message so user will not accidentally delete a file or folder
                            W(DoTranslation("Are you sure you want to delete {0} <y/n>?") + " ", False, ColTypes.Input, ArgsQ(0))
                            Dim answer As String = Console.ReadKey.KeyChar
                            Console.WriteLine()

                            Try
                                SFTPDeleteRemote(ArgsQ(0))
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
                    If SFTPConnected = True Then
                        'Set a connected flag to False
                        SFTPConnected = False
                        ClientSFTP.Disconnect()
                        W(DoTranslation("Disconnected from {0}"), True, ColTypes.Neutral, ftpsite)

                        'Clean up everything
                        sftpsite = ""
                        SFTPCurrentRemoteDir = ""
                        SFTPUser = ""
                        SFTPPass = ""
                    Else
                        W(DoTranslation("You haven't connected to any server yet"), True, ColTypes.Error)
                    End If
                Case "get"
                    If RequiredArgumentsProvided Then
                        W(DoTranslation("Downloading file {0}..."), False, ColTypes.Neutral, ArgsQ(0))
                        If SFTPGetFile(ArgsQ(0)) Then
                            Console.WriteLine()
                            W(DoTranslation("Downloaded file {0}."), True, ColTypes.Neutral, ArgsQ(0))
                        Else
                            Console.WriteLine()
                            W(DoTranslation("Download failed for file {0}."), True, ColTypes.Error, ArgsQ(0))
                        End If
                    Else
                        W(DoTranslation("Enter a file to download to local directory."), True, ColTypes.Error)
                    End If
                Case "lsl"
                    If ArgsQ?.Count > 0 And ArgsQ IsNot Nothing Then
                        List(ArgsQ(0))
                    Else
                        List(CurrDir)
                    End If
                Case "lsr"
                    Dim Entries As List(Of String) = SFTPListRemote(If(ArgsQ IsNot Nothing, ArgsQ(0), ""))
                    Entries.Sort()
                    For Each Entry As String In Entries
                        W(Entry, True, ColTypes.ListEntry)
                    Next
                Case "quickconnect"
                    If Not connected Then
                        SFTPQuickConnect()
                    Else
                        W(DoTranslation("You should disconnect from server before connecting to another server"), True, ColTypes.Error)
                    End If
                Case "put"
                    If RequiredArgumentsProvided Then
                        W(DoTranslation("Uploading file {0}..."), True, ColTypes.Neutral, ArgsQ(0))

                        'Begin the uploading process
                        If SFTPUploadFile(ArgsQ(0)) Then
                            Console.WriteLine()
                            W(vbNewLine + DoTranslation("Uploaded file {0}"), True, ColTypes.Neutral, ArgsQ(0))
                        Else
                            Console.WriteLine()
                            W(vbNewLine + DoTranslation("Failed to upload {0}"), True, ColTypes.Neutral, ArgsQ(0))
                        End If
                    Else
                        W(DoTranslation("Enter a file to upload to remote directory. upload <file> <directory>"), True, ColTypes.Error)
                    End If
                Case "exit"
                    'Set a flag
                    sftpexit = True
                Case "help"
                    If cmd = "help" Then
                        SFTPShowHelp()
                    Else
                        SFTPShowHelp(strArgs)
                    End If
            End Select
        Catch taex As ThreadAbortException
            Exit Sub
        Catch ex As Exception 'The InnerException CAN be Nothing
            If DebugMode = True Then
                If ex.InnerException IsNot Nothing Then 'This is required to fix NullReferenceException when there is nothing in InnerException, so please don't remove.
                    W(DoTranslation("Error trying to execute SFTP command {3}.") + vbNewLine +
                      DoTranslation("Error {0}: {1} ") + DoTranslation("(Inner:") + " {4})" + vbNewLine + "{2}", True, ColTypes.Error, ex.GetType.FullName, ex.Message, ex.StackTrace, words(0), ex.InnerException.Message)
                Else
                    W(DoTranslation("Error trying to execute SFTP command {3}.") + vbNewLine +
                      DoTranslation("Error {0}: {1}") + vbNewLine + "{2}", True, ColTypes.Error, ex.GetType.FullName, ex.Message, ex.StackTrace, words(0))
                End If
                WStkTrc(ex)
            Else
                If ex.InnerException IsNot Nothing Then
                    W(DoTranslation("Error trying to execute SFTP command {2}.") + vbNewLine +
                      DoTranslation("Error {0}: {1} ") + DoTranslation("(Inner:") + " {3})", True, ColTypes.Error, ex.GetType.FullName, ex.Message, words(0), ex.InnerException.Message)
                Else
                    W(DoTranslation("Error trying to execute SFTP command {2}.") + vbNewLine +
                      DoTranslation("Error {0}: {1}"), True, ColTypes.Error, ex.GetType.FullName, ex.Message, words(0))
                End If
            End If
            EventManager.RaiseSFTPCommandError(cmd, ex)
        End Try

    End Sub

    Sub SFTPCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            Console.WriteLine()
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            SFTPStartCommandThread.Abort()
        End If
    End Sub

End Module

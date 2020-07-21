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
Imports System.Threading

Public Module FTPGetCommand

    'Progress Bar Enabled
    Dim progressFlag As Boolean = True
    Dim ConsoleOriginalPosition_LEFT As Integer
    Dim ConsoleOriginalPosition_TOP As Integer

    'FTP Client and thread
    Public ClientFTP As FtpClient
    Public FTPStartCommandThread As New Thread(AddressOf ExecuteCommand)

    'To enable progress
    Public Complete As New Action(Of FtpProgress)(Sub(percentage)
                                                      'If the progress is not defined, disable progress bar
                                                      If percentage.Progress < 0 Then
                                                          progressFlag = False
                                                      Else
                                                          ConsoleOriginalPosition_LEFT = Console.CursorLeft
                                                          ConsoleOriginalPosition_TOP = Console.CursorTop
                                                          If progressFlag = True And percentage.Progress <> 100 Then
                                                              W(" {0}% (ETA: {1}d {2}:{3}:{4} @ {5})", False, ColTypes.Neutral, FormatNumber(percentage.Progress, 1), percentage.ETA.Days, percentage.ETA.Hours, percentage.ETA.Minutes, percentage.ETA.Seconds, percentage.TransferSpeedToString)
                                                          End If
                                                          Console.SetCursorPosition(ConsoleOriginalPosition_LEFT, ConsoleOriginalPosition_TOP)
                                                      End If
                                                  End Sub)

    ''' <summary>
    ''' Parses and executes the FTP command
    ''' </summary>
    ''' <param name="cmd">A command. It may come with arguments</param>
    Public Sub ExecuteCommand(ByVal cmd As String)

        Dim index As Integer = cmd.IndexOf(" ")
        If index = -1 Then index = cmd.Length
        Dim words = cmd.Split({" "c})
        Dim strArgs As String = cmd.Substring(index)
        If Not index = cmd.Length Then strArgs = strArgs.Substring(1)
        Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

        'Command code
        Try
            If words(0) = "connect" Then
                If args.Count <> 0 Then
                    If args(0).StartsWith("ftp://") Or args(0).StartsWith("ftps://") Or args(0).StartsWith("ftpes://") Then
                        TryToConnect(args(0))
                    Else
                        TryToConnect($"ftps://{args(0)}")
                    End If
                Else
                    W(DoTranslation("Enter an FTP server.", currentLang), True, ColTypes.Neutral)
                End If
            ElseIf words(0) = "changelocaldir" Or words(0) = "cdl" Then
                If cmd <> "changelocaldir" Or cmd <> "cdl" Then
                    'Check if folder exists
                    Dim targetDir As String
                    targetDir = $"{currDirect}/{strArgs}"
                    If IO.Directory.Exists(targetDir) Then
                        'Parse written directory
                        Dim parser As New IO.DirectoryInfo(targetDir)
                        currDirect = parser.FullName
                    Else
                        W(DoTranslation("Local directory {0} doesn't exist.", currentLang), True, ColTypes.Err, strArgs)
                    End If
                Else
                    W(DoTranslation("Enter a local directory. "".."" to go back.", currentLang), True, ColTypes.Err)
                End If
            ElseIf words(0) = "changeremotedir" Or words(0) = "cdr" Then
                If connected = True Then
                    If cmd <> "changeremotedir" Or cmd <> "cdr" Then
                        If ClientFTP.DirectoryExists(strArgs) = True Then
                            'Directory exists, go to the new directory
                            ClientFTP.SetWorkingDirectory(strArgs)
                            currentremoteDir = ClientFTP.GetWorkingDirectory
                        Else
                            'Directory doesn't exist, go to the old directory
                            W(DoTranslation("Directory {0} not found.", currentLang), True, ColTypes.Err, strArgs)
                        End If
                    Else
                        W(DoTranslation("Enter a remote directory. "".."" to go back", currentLang), True, ColTypes.Err)
                    End If
                Else
                    W(DoTranslation("You must connect to a server before changing directory", currentLang), True, ColTypes.Err)
                End If
            ElseIf words(0) = "currlocaldir" Or words(0) = "pwdl" Then
                W(DoTranslation("Local directory: {0}", currentLang), True, ColTypes.Neutral, currDirect)
            ElseIf words(0) = "currremotedir" Or words(0) = "pwdr" Then
                If connected = True Then
                    W(DoTranslation("Remote directory: {0}", currentLang), True, ColTypes.Neutral, currentremoteDir)
                Else
                    W(DoTranslation("You must connect to server before getting current remote directory.", currentLang), True, ColTypes.Err)
                End If
            ElseIf words(0) = "delete" Or words(0) = "del" Then
                If cmd <> "delete" Or cmd <> "del" Then
                    If connected = True Then
                        'Print a message
                        W(DoTranslation("Deleting file {0}...", currentLang), True, ColTypes.Neutral, strArgs)

                        'Make a confirmation message so user will not accidentally delete a file
                        W(DoTranslation("Are you sure you want to delete file {0} <y/n>?", currentLang), False, ColTypes.Input, strArgs)
                        Dim answer As String = Console.ReadKey.KeyChar

                        'If the answer is "y", then delete a file
                        If answer = "y" Then
                            ClientFTP.DeleteFile(strArgs)
                            W(vbNewLine + DoTranslation("Deleted file {0}", currentLang), True, ColTypes.Neutral, strArgs)
                        End If
                    Else
                        W(DoTranslation("You must connect to server with administrative privileges before performing the deletion.", currentLang), True, ColTypes.Err)
                    End If
                Else
                    W(DoTranslation("Enter a file to remove. You must have administrative permissions on your logged in username to be able to remove.", currentLang), True, ColTypes.Err)
                End If
            ElseIf cmd = "disconnect" Then
                If connected = True Then
                    'Set a connected flag to False
                    connected = False
                    ClientFTP.Disconnect()
                    W(DoTranslation("Disconnected from {0}", currentLang), True, ColTypes.Neutral, ftpsite)

                    'Clean up everything
                    ftpsite = ""
                    currentremoteDir = ""
                    user = ""
                    pass = ""
                Else
                    W(DoTranslation("You haven't connected to any server yet", currentLang), True, ColTypes.Err)
                End If
            ElseIf words(0) = "download" Or words(0) = "get" Then
                If cmd <> "download" Or cmd <> "get" Then
                    If connected = True Then
                        Try
                            'Show a message to download
                            W(DoTranslation("Downloading file {0}...", currentLang), False, ColTypes.Neutral, strArgs)

                            'Try to download 3 times
                            ClientFTP.DownloadFile($"{currDirect}/{strArgs}", strArgs, True, FtpVerify.Retry + FtpVerify.Throw, Complete)

                            'Show a message that it's downloaded
                            Console.WriteLine()
                            W(DoTranslation("Downloaded file {0}.", currentLang), True, ColTypes.Neutral, strArgs)
                        Catch ex As Exception
                            W(DoTranslation("Download failed for file {0} because the local file is corrupt.", currentLang), True, ColTypes.Err, strArgs)
                        End Try
                    Else
                        W(DoTranslation("You must connect to server before performing transmission.", currentLang), True, ColTypes.Err)
                    End If
                Else
                    W(DoTranslation("Enter a file to download to local directory.", currentLang), True, ColTypes.Err)
                End If
            ElseIf cmd = "exit" Then
                'Set a flag
                ftpexit = True
            ElseIf cmd = "help" Then
                FTPShowHelp()
            ElseIf words(0) = "listlocal" Or words(0) = "lsl" Then
                If cmd = "listlocal" Or cmd = "lsl" Then
                    List(CurrDir)
                Else
                    List(strArgs)
                End If
            ElseIf words(0) = "listremote" Or words(0) = "lsr" Then
                If connected = True Then
                    Dim FileSize As Long
                    Dim ModDate As DateTime
                    Dim Listing As FtpListItem()
                    If cmd <> "listremote" Or cmd <> "lsr" Then
                        Listing = ClientFTP.GetListing(strArgs)
                    Else
                        Listing = ClientFTP.GetListing(currentremoteDir)
                    End If
                    For Each DirListFTP As FtpListItem In Listing
                        W($"- {DirListFTP.Name}", False, ColTypes.HelpCmd)
                        If DirListFTP.Type = FtpFileSystemObjectType.File Then
                            W(": ", False, ColTypes.HelpCmd)
                            FileSize = ClientFTP.GetFileSize(DirListFTP.FullName)
                            ModDate = ClientFTP.GetModifiedTime(DirListFTP.FullName)
                            W(DoTranslation("{0} KB | Modified in: {1}", currentLang), False, ColTypes.HelpDef, FormatNumber(FileSize / 1024, 2), ModDate.ToString)
                        ElseIf DirListFTP.Type = FtpFileSystemObjectType.Directory Then
                            W("/", False, ColTypes.HelpCmd)
                        ElseIf DirListFTP.Type = FtpFileSystemObjectType.Link Then
                            W(">> ", False, ColTypes.HelpCmd)
                            W(ClientFTP.DereferenceLink(DirListFTP), False, ColTypes.HelpDef)
                        End If
                        Console.WriteLine()
                    Next
                Else
                    W(DoTranslation("You should connect to server before listing all remote files.", currentLang), True, ColTypes.Err)
                End If
            ElseIf words(0) = "quickconnect" Then
                If Not connected Then
                    If File.Exists(paths("FTPSpeedDial")) Then
                        Dim SpeedDialLines As String() = File.ReadAllLines(paths("FTPSpeedDial"))
                        Wdbg("I", "Speed dial length: {0}", SpeedDialLines.Length)
                        Dim Counter As Integer = 1
                        Dim Answer As String
                        Dim Answering As Boolean = True
                        If Not SpeedDialLines.Count = 0 Then
                            For Each SpeedDialLine As String In SpeedDialLines
                                Wdbg("I", "Speed dial line: {0}", SpeedDialLine)
                                W(DoTranslation("Select an address to connect to:", currentLang), True, ColTypes.Neutral)
                                W("{0}: {1}", True, ColTypes.Neutral, Counter, SpeedDialLine)
                                Counter += 1
                            Next
                            While Answering
                                W(">> ", False, ColTypes.Input)
                                Answer = Console.ReadKey.KeyChar
                                Wdbg("I", "Response: {0}", Answer)
                                Console.WriteLine()
                                If IsNumeric(Answer) Then
                                    Wdbg("I", "Response is numeric. IsNumeric(Answer) returned true. Checking to see if in-bounds...")
                                    Dim AnswerInt As Integer = Answer
                                    If AnswerInt <= SpeedDialLines.Length Then
                                        Answering = False
                                        Wdbg("I", "Response is in-bounds. Connecting...")
                                        Dim ChosenSpeedDialLine As String = SpeedDialLines(AnswerInt - 1)
                                        Wdbg("I", "Chosen connection: {0}", ChosenSpeedDialLine)
                                        Dim ChosenLineSeparation As String() = ChosenSpeedDialLine.Split(",")
                                        Dim Address As String = ChosenLineSeparation(0)
                                        Dim Port As String = ChosenLineSeparation(1)
                                        Dim Username As String = ChosenLineSeparation(2)
                                        Dim Encryption As FtpEncryptionMode = ChosenLineSeparation(3)
                                        Wdbg("I", "Address: {0}, Port: {1}, Username: {2}, Encryption: {3}", Address, Port, Username, Encryption)
                                        PromptForPassword(Username, Address, Port, Encryption)
                                    Else
                                        Wdbg("I", "Response is out-of-bounds. Retrying...")
                                        W(DoTranslation("The selection is out of range. Select between 1-{0}. Try again.", currentLang), True, ColTypes.Err, SpeedDialLines.Length)
                                    End If
                                Else
                                    Wdbg("W", "Response isn't numeric. IsNumeric(Answer) returned false.")
                                    W(DoTranslation("The selection is not a number. Try again.", currentLang), True, ColTypes.Err)
                                End If
                            End While
                        Else
                            Wdbg("E", "Speed dial is empty. Lines count is 0.")
                            W(DoTranslation("Speed dial is empty. Connect to a server to add an address to it.", currentLang), True, ColTypes.Err)
                        End If
                    Else
                        Wdbg("E", "File doesn't exist.")
                        W(DoTranslation("Speed dial doesn't exist. Connect to a server to add an address to it.", currentLang), True, ColTypes.Err)
                    End If
                Else
                    W(DoTranslation("You should disconnect from server before connecting to another server", currentLang), True, ColTypes.Err)
                End If
            ElseIf words(0) = "rename" Or words(0) = "ren" Then
                If cmd <> "rename" Or cmd <> "ren" Then
                    If connected = True Then
                        W(DoTranslation("Renaming file {0} to {1}...", currentLang), True, ColTypes.Neutral, args(0), args(1))

                        'Begin the renaming process
                        ClientFTP.Rename(args(0), args(1))

                        'Show a message
                        W(vbNewLine + DoTranslation("Renamed successfully", currentLang), True, ColTypes.Neutral)
                    Else
                        W(DoTranslation("You must connect to server before performing transmission.", currentLang), True, ColTypes.Err)
                    End If
                Else
                    W(DoTranslation("Enter a file and the new file name.", currentLang), True, ColTypes.Err)
                End If
            ElseIf words(0) = "upload" Or words(0) = "put" Then
                If cmd <> "upload" Or cmd <> "put" Then
                    If connected = True Then
                        W(DoTranslation("Uploading file {0}...", currentLang), True, ColTypes.Neutral, strArgs)

                        'Begin the uploading process
                        ClientFTP.UploadFile($"{currDirect}/{strArgs}", strArgs, True, True, FtpVerify.Retry, Complete)
                        Console.WriteLine()

                        'Show a message
                        W(vbNewLine + DoTranslation("Uploaded file {0}", currentLang), True, ColTypes.Neutral, strArgs)
                    Else
                        W(DoTranslation("You must connect to server before performing transmission.", currentLang), True, ColTypes.Err)
                    End If
                Else
                    W(DoTranslation("Enter a file to upload to remote directory. upload <file> <directory>", currentLang), True, ColTypes.Err)
                End If
            End If
        Catch ex As Exception 'The InnerException CAN be Nothing
            If DebugMode = True Then
                If Not IsNothing(ex.InnerException) Then 'This is required to fix NullReferenceException when there is nothing in InnerException, so please don't remove.
                    W(DoTranslation("Error trying to execute FTP command {3}.", currentLang) + vbNewLine +
                      DoTranslation("Error {0}: {1} ", currentLang) + DoTranslation("(Inner:", currentLang) + " {4})" + vbNewLine + "{2}", True, ColTypes.Err, Err.Number, ex.Message, ex.StackTrace, words(0), ex.InnerException.Message)
                Else
                    W(DoTranslation("Error trying to execute FTP command {3}.", currentLang) + vbNewLine +
                      DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, ColTypes.Err, Err.Number, ex.Message, ex.StackTrace, words(0))
                End If
                WStkTrc(ex)
            Else
                If Not IsNothing(ex.InnerException) Then
                    W(DoTranslation("Error trying to execute FTP command {2}.", currentLang) + vbNewLine +
                      DoTranslation("Error {0}: {1} ", currentLang) + DoTranslation("(Inner:", currentLang) + "{3})", True, ColTypes.Err, Err.Number, ex.Message, words(0), ex.InnerException.Message)
                Else
                    W(DoTranslation("Error trying to execute FTP command {2}.", currentLang) + vbNewLine +
                      DoTranslation("Error {0}: {1}", currentLang), True, ColTypes.Err, Err.Number, ex.Message, words(0))
                End If
            End If
            EventManager.RaiseFTPCommandError()
        End Try

    End Sub

    Sub FTPCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            FTPStartCommandThread.Abort()
        End If
    End Sub

End Module

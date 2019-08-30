'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Public Module FTPGetCommand

    'Progress Bar Enabled
    Dim progressFlag As Boolean = True
    Dim ConsoleOriginalPosition_LEFT As Integer
    Dim ConsoleOriginalPosition_TOP As Integer
    Public ClientFTP As FtpClient

    'To enable progress
    Public Complete As New Progress(Of FtpProgress)(Sub(percentage)
                                                        'If the progress is not defined, disable progress bar
                                                        If percentage.Progress < 0 Then
                                                            progressFlag = False
                                                        Else
                                                            ConsoleOriginalPosition_LEFT = Console.CursorLeft
                                                            ConsoleOriginalPosition_TOP = Console.CursorTop
                                                            If progressFlag = True And percentage.Progress <> 100 Then
                                                                Console.Write("{0}% (ETA: {1} @ {2})             ", FormatNumber(percentage.Progress, 1), percentage.ETA, percentage.TransferSpeedToString)
                                                                'W("{0}% (ETA: {1} @ {2})             ", False, "neutralText", FormatNumber(percentage.Progress, 1), percentage.ETA, percentage.TransferSpeedToString)
                                                            End If
                                                            Console.SetCursorPosition(ConsoleOriginalPosition_LEFT, ConsoleOriginalPosition_TOP)
                                                        End If
                                                    End Sub)

    Public Sub ExecuteCommand(ByVal cmd As String)

        Dim index As Integer = cmd.IndexOf(" ")
        If index = -1 Then index = cmd.Length
        Dim words = cmd.Split({" "c})
        Dim strArgs As String = cmd.Substring(index)
        If Not index = cmd.Length Then strArgs = strArgs.Substring(1)
        Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

        'Command code
        Try
            If cmd.Substring(0, index) = "connect" Then
                If cmd <> "connect" Then
                    TryToConnect($"ftps://{strArgs}")
                Else
                    W(DoTranslation("Enter an FTP server.", currentLang), True, "neutralText")
                End If
            ElseIf cmd.Substring(0, index) = "changelocaldir" Or cmd.Substring(0, index) = "cdl" Then
                If cmd <> "changelocaldir" Or cmd <> "cdl" Then
                    'Check if folder exists
                    Dim targetDir As String
                    targetDir = $"{currDirect}/{strArgs}"
                    If IO.Directory.Exists(targetDir) Then
                        'Parse written directory
                        Dim parser As New IO.DirectoryInfo(targetDir)
                        currDirect = parser.FullName
                    Else
                        W(DoTranslation("Local directory {0} doesn't exist.", currentLang), True, "neutralText", strArgs)
                    End If
                Else
                    W(DoTranslation("Enter a local directory. "".."" to go back.", currentLang), True, "neutralText")
                End If
            ElseIf cmd.Substring(0, index) = "changeremotedir" Or cmd.Substring(0, index) = "cdr" Then
                If connected = True Then
                    If cmd <> "changeremotedir" Or cmd <> "cdr" Then
                        If ClientFTP.DirectoryExists(strArgs) = True Then
                            'Directory exists, go to the new directory
                            ClientFTP.SetWorkingDirectory(strArgs)
                            currentremoteDir = ClientFTP.GetWorkingDirectory
                        Else
                            'Directory doesn't exist, go to the old directory
                            W(DoTranslation("Directory {0} not found.", currentLang), True, "neutralText", strArgs)
                        End If
                    Else
                        W(DoTranslation("Enter a remote directory. "".."" to go back", currentLang), True, "neutralText")
                    End If
                Else
                    W(DoTranslation("You must connect to a server before changing directory", currentLang), True, "neutralText")
                End If
            ElseIf cmd.Substring(0, index) = "currlocaldir" Or cmd.Substring(0, index) = "pwdl" Then
                W(DoTranslation("Local directory: {0}", currentLang), True, "neutralText", currDirect)
            ElseIf cmd.Substring(0, index) = "currremotedir" Or cmd.Substring(0, index) = "pwdr" Then
                If connected = True Then
                    W(DoTranslation("Remote directory: {0}", currentLang), True, "neutralText", currentremoteDir)
                Else
                    W(DoTranslation("You must connect to server before getting current remote directory.", currentLang), True, "neutralText")
                End If
            ElseIf cmd.Substring(0, index) = "delete" Or cmd.Substring(0, index) = "del" Then
                If cmd <> "delete" Or cmd <> "del" Then
                    If connected = True Then
                        'Print a message
                        W(DoTranslation("Deleting file {0}...", currentLang), True, "neutralText", strArgs)

                        'Make a confirmation message so user will not accidentally delete a file
                        W(DoTranslation("Are you sure you want to delete file {0} <y/n>?", currentLang), False, "input", strArgs)
                        Dim answer As String = Console.ReadKey.KeyChar

                        'If the answer is "y", then delete a file
                        If answer = "y" Then
                            ClientFTP.DeleteFile(strArgs)
                            W(vbNewLine + DoTranslation("Deleted file {0}", currentLang), True, "neutralText", strArgs)
                        End If
                    Else
                        W(DoTranslation("You must connect to server with administrative privileges before performing the deletion.", currentLang), True, "neutralText")
                    End If
                Else
                    W(DoTranslation("Enter a file to remove. You must have administrative permissions on your logged in username to be able to remove.", currentLang), True, "neutralText")
                End If
            ElseIf cmd = "disconnect" Then
                If connected = True Then
                    'Set a connected flag to False
                    connected = False
                    ClientFTP.Disconnect()
                    W(DoTranslation("Disconnected from {0}", currentLang), True, "neutralText", ftpsite)

                    'Clean up everything
                    ftpsite = ""
                    currentremoteDir = ""
                    user = ""
                    pass = ""
                Else
                    W(DoTranslation("You haven't connected to any server yet", currentLang), True, "neutralText")
                End If
            ElseIf cmd.Substring(0, index) = "download" Or cmd.Substring(0, index) = "get" Then
                If cmd <> "download" Or cmd <> "get" Then
                    If connected = True Then
                        Try
                            'Show a message to download
                            W(DoTranslation("Downloading file {0}...", currentLang), False, "neutralText", strArgs)

                            'Try to download 3 times
                            ClientFTP.DownloadFile($"{currDirect}/{strArgs}", strArgs, True, FtpVerify.Retry + FtpVerify.Throw, Complete)

                            'Show a message that it's downloaded
                            Console.WriteLine()
                            W(DoTranslation("Downloaded file {0}.", currentLang), True, "neutralText", strArgs)
                        Catch ex As Exception
                            W(DoTranslation("Download failed for file {0} because the local file is corrupt.", currentLang), True, "neutralText", strArgs)
                        End Try
                    Else
                        W(DoTranslation("You must connect to server before performing transmission.", currentLang), True, "neutralText")
                    End If
                Else
                    W(DoTranslation("Enter a file to download to local directory.", currentLang), True, "neutralText")
                End If
            ElseIf cmd = "exit" Then
                'Set a flag
                ftpexit = True
            ElseIf cmd = "help" Then
                FTPShowHelp()
            ElseIf cmd.Substring(0, index) = "listlocal" Or cmd.Substring(0, index) = "lsl" Then
                If cmd = "listlocal" Or cmd = "lsl" Then
                    List(CurrDir)
                Else
                    List(strArgs)
                End If
            ElseIf cmd.Substring(0, index) = "listremote" Or cmd.Substring(0, index) = "lsr" Then
                If cmd <> "listremote" Or cmd <> "lsr" Then
                    If connected = True Then
                        Dim FileSize As Long
                        Dim ModDate As DateTime
                        For Each DirListFTP As FtpListItem In ClientFTP.GetListing(strArgs)
                            W($"- {DirListFTP.FullName}", False, "neutralText")
                            If DirListFTP.Type = FtpFileSystemObjectType.File Then
                                W(": ", False, "neutralText")
                                FileSize = ClientFTP.GetFileSize(DirListFTP.FullName)
                                ModDate = ClientFTP.GetModifiedTime(DirListFTP.FullName)
                                W(DoTranslation("{0} KB | Modified in: {1}", currentLang), False, "neutralText", FormatNumber(FileSize / 1024, 2), ModDate.ToString)
                            End If
                            Console.WriteLine()
                        Next
                    Else
                        W(DoTranslation("You should connect to server before listing all remote files.", currentLang), True, "neutralText")
                    End If
                Else
                    If connected = True Then
                        Dim FileSize As Long
                        Dim ModDate As DateTime
                        For Each DirListFTP As FtpListItem In ClientFTP.GetListing(currentremoteDir)
                            W($"- {DirListFTP.FullName}", True, "neutralText")
                            If DirListFTP.Type = FtpFileSystemObjectType.File Then
                                W(": ", True, "neutralText")
                                FileSize = ClientFTP.GetFileSize(DirListFTP.FullName)
                                ModDate = ClientFTP.GetModifiedTime(DirListFTP.FullName)
                                W(DoTranslation("{0} KB | Modified in: {1}", currentLang), True, "neutralText", FormatNumber(FileSize / 1024, 2), ModDate.ToString)
                            End If
                            Console.WriteLine()
                        Next
                    Else
                        W(DoTranslation("You should connect to server before listing all remote files.", currentLang), True, "neutralText")
                    End If
                End If
            ElseIf cmd.Substring(0, index) = "rename" Or cmd.Substring(0, index) = "ren" Then
                If cmd <> "rename" Or cmd <> "ren" Then
                    If connected = True Then
                        W(DoTranslation("Renaming file {0} to {1}...", currentLang), True, "neutralText", args(0), args(1))

                        'Begin the renaming process
                        ClientFTP.Rename(args(0), args(1))

                        'Show a message
                        W(vbNewLine + DoTranslation("Renamed successfully", currentLang), True, "neutralText")
                    Else
                        W(DoTranslation("You must connect to server before performing transmission.", currentLang), True, "neutralText")
                    End If
                Else
                    W(DoTranslation("Enter a file and the new file name.", currentLang), True, "neutralText")
                End If
            ElseIf cmd.Substring(0, index) = "upload" Or cmd.Substring(0, index) = "put" Then
                If cmd <> "upload" Or cmd <> "put" Then
                    If connected = True Then
                        W(DoTranslation("Uploading file {0}...", currentLang), True, "neutralText", strArgs)

                        'Begin the uploading process
                        ClientFTP.UploadFile($"{currDirect}/{strArgs}", strArgs, True, True, FtpVerify.Retry, Complete)
                        Console.WriteLine()

                        'Show a message
                        W(vbNewLine + DoTranslation("Uploaded file {0}", currentLang), True, "neutralText", strArgs)
                    Else
                        W(DoTranslation("You must connect to server before performing transmission.", currentLang), True, "neutralText")
                    End If
                Else
                    W(DoTranslation("Enter a file to upload to remote directory. upload <file> <directory>", currentLang), True, "neutralText")
                End If
            End If
        Catch ex As Exception 'The InnerException CAN be Nothing
            If DebugMode = True Then
                If Not IsNothing(ex.InnerException) Then 'This is required to fix NullReferenceException when there is nothing in InnerException, so please don't remove.
                    W(DoTranslation("Error trying to execute FTP command {3}.", currentLang) + vbNewLine +
                      DoTranslation("Error {0}: {1} ", currentLang) + DoTranslation("(Inner:", currentLang) + " {4})" + vbNewLine + "{2}", True, "neutralText", Err.Number, Err.Description, ex.StackTrace, words(0), ex.InnerException.Message)
                Else
                    W(DoTranslation("Error trying to execute FTP command {3}.", currentLang) + vbNewLine +
                      DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, "neutralText", Err.Number, Err.Description, ex.StackTrace, words(0))
                End If
                WStkTrc(ex)
            Else
                If Not IsNothing(ex.InnerException) Then
                    W(DoTranslation("Error trying to execute FTP command {2}.", currentLang) + vbNewLine +
                      DoTranslation("Error {0}: {1} ", currentLang) + DoTranslation("(Inner:", currentLang) + "{3})", True, "neutralText", Err.Number, Err.Description, words(0), ex.InnerException.Message)
                Else
                    W(DoTranslation("Error trying to execute FTP command {2}.", currentLang) + vbNewLine +
                      DoTranslation("Error {0}: {1}", currentLang), True, "neutralText", Err.Number, Err.Description, words(0))
                End If
            End If
            EventManager.RaiseFTPCommandError()
        End Try

    End Sub

End Module

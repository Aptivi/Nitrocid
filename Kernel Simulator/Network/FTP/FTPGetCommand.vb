
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

Imports System.Threading

Public Module FTPGetCommand

    'Progress Bar Enabled
    Dim progressFlag As Boolean = True
    Dim ConsoleOriginalPosition_LEFT As Integer
    Dim ConsoleOriginalPosition_TOP As Integer
    Public ClientFTP As FtpClient

    'To enable progress
    Public Complete As New Progress(Of Double)(Function(percentage)
                                                   'If the progress is not defined, disable progress bar
                                                   If percentage < 0 Then
                                                       progressFlag = False
                                                   Else
                                                       ConsoleOriginalPosition_LEFT = Console.CursorLeft
                                                       ConsoleOriginalPosition_TOP = Console.CursorTop
                                                       If progressFlag = True And percentage <> 100 Then
                                                           W("{0}%", "neutralText", FormatNumber(percentage, 1))
                                                       End If
                                                       Console.SetCursorPosition(ConsoleOriginalPosition_LEFT, ConsoleOriginalPosition_TOP)
                                                   End If
                                                   Thread.Sleep(500)
                                               End Function)

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
                    If connected = True Then
                        Wln(DoTranslation("You should disconnect from server before connecting to another server", currentLang), "neutralText")
                    Else
                        Try
                            'Create an FTP stream to connect to
                            ClientFTP = New FtpClient With {
                                .Host = strArgs,
                                .RetryAttempts = 3
                            }

                            'Prompt for username and for password
                            W(DoTranslation("Username for {0}: ", currentLang), "input", strArgs)
                            user = Console.ReadLine()
                            If user = "" Then user = "anonymous"
                            W(DoTranslation("Password for {0}: ", currentLang), "input", user)

                            'Get input
                            While True
                                Dim character As Char = Console.ReadKey(True).KeyChar
                                If character = vbCr Or character = vbLf Then
                                    Console.WriteLine()
                                    Exit While
                                Else
                                    pass += character
                                End If
                            End While

                            'Set up credentials
                            ClientFTP.Credentials = New NetworkCredential(user, pass)

                            'Connect
                            Dim address As New Uri(strArgs)
                            ClientFTP = FtpClient.Connect(address)

                            'Show that it's connected
                            Wln(DoTranslation("Connected to {0}", currentLang), "neutralText", strArgs)
                            connected = True

                            'Prepare to print current FTP directory
                            currentremoteDir = ClientFTP.GetWorkingDirectory
                            ftpsite = ClientFTP.Host
                        Catch ex As Exception
                            Wdbg("Error connecting to {0}: {1}", args(0), ex.Message)
                            Wdbg("{0}", ex.StackTrace)
                            If DebugMode = True Then
                                Wln(DoTranslation("Error when trying to connect to {0}: {1}", currentLang) + vbNewLine +
                                    DoTranslation("Stack Trace: {2}", currentLang), "neutralText", args(0), ex.Message, ex.StackTrace)
                            Else
                                Wln(DoTranslation("Error when trying to connect to {0}: {1}", currentLang), "neutralText", args(0), ex.Message)
                            End If
                        End Try
                    End If
                Else
                    Wln(DoTranslation("Enter an FTP server that starts with ""ftp://"" or ""ftps://""", currentLang), "neutralText")
                End If
            ElseIf cmd.Substring(0, index) = "changelocaldir" Or cmd.Substring(0, index) = "cdl" Then
                If cmd <> "changelocaldir" Or cmd <> "cdl" Then
                    'Check if folder exists
                    Dim targetDir As String
                    If EnvironmentOSType.Contains("Unix") Then
                        targetDir = currDirect + "/" + strArgs
                    Else
                        targetDir = currDirect + "\" + strArgs
                    End If
                    If IO.Directory.Exists(targetDir) Then
                        'Parse written directory
                        Dim parser As New IO.DirectoryInfo(targetDir)
                        currDirect = parser.FullName
                    Else
                        Wln(DoTranslation("Local directory {0} doesn't exist.", currentLang), "neutralText", strArgs)
                    End If
                Else
                    Wln(DoTranslation("Enter a local directory. "".."" to go back.", currentLang), "neutralText")
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
                            Wln(DoTranslation("Directory {0} not found.", currentLang), "neutralText", strArgs)
                        End If
                    Else
                        Wln(DoTranslation("Enter a remote directory. "".."" to go back", currentLang), "neutralText")
                    End If
                Else
                    Wln(DoTranslation("You must connect to a server before changing directory", currentLang), "neutralText")
                End If
            ElseIf cmd.Substring(0, index) = "currlocaldir" Or cmd.Substring(0, index) = "pwdl" Then
                Wln(DoTranslation("Local directory: {0}", currentLang), "neutralText", currDirect)
            ElseIf cmd.Substring(0, index) = "currremotedir" Or cmd.Substring(0, index) = "pwdr" Then
                If connected = True Then
                    Wln(DoTranslation("Remote directory: {0}", currentLang), "neutralText", currentremoteDir)
                Else
                    Wln(DoTranslation("You must connect to server before getting current remote directory.", currentLang), "neutralText")
                End If
            ElseIf cmd.Substring(0, index) = "delete" Or cmd.Substring(0, index) = "del" Then
                If cmd <> "delete" Or cmd <> "del" Then
                    If connected = True Then
                        'Print a message
                        Wln(DoTranslation("Deleting file {0}...", currentLang), "neutralText", strArgs)

                        'Make a confirmation message so user will not accidentally delete a file
                        W(DoTranslation("Are you sure you want to delete file {0} <y/n>?", currentLang), "input", strArgs)
                        Dim answer As String = Console.ReadKey.KeyChar

                        'If the answer is "y", then delete a file
                        If answer = "y" Then
                            ClientFTP.DeleteFile(strArgs)
                            Wln(vbNewLine + DoTranslation("Deleted file {0}", currentLang), "neutralText", strArgs)
                        End If
                    Else
                        Wln(DoTranslation("You must connect to server with administrative privileges before performing the deletion.", currentLang), "neutralText")
                    End If
                Else
                    Wln(DoTranslation("Enter a file to remove. You must have administrative permissions on your logged in username to be able to remove.", currentLang), "neutralText")
                End If
            ElseIf cmd = "disconnect" Then
                If connected = True Then
                    'Set a connected flag to False
                    connected = False
                    ClientFTP.Disconnect()
                    Wln(DoTranslation("Disconnected from {0}", currentLang), "neutralText", ftpsite)

                    'Clean up everything
                    ftpsite = ""
                    currentremoteDir = ""
                    user = ""
                    pass = ""
                Else
                    Wln(DoTranslation("You haven't connected to any server yet", currentLang), "neutralText")
                End If
            ElseIf cmd.Substring(0, index) = "download" Or cmd.Substring(0, index) = "get" Then
                If cmd <> "download" Or cmd <> "get" Then
                    If connected = True Then
                        Try
                            'Show a message to download
                            W(DoTranslation("Downloading file {0}...", currentLang), "neutralText", strArgs)

                            'Try to download 3 times
                            ClientFTP.DownloadFile(currDirect + strArgs, strArgs, True, FtpVerify.Retry + FtpVerify.Throw, Complete)

                            'Show a message that it's downloaded
                            Console.WriteLine()
                            Wln(DoTranslation("Downloaded file {0}.", currentLang), "neutralText", strArgs)
                        Catch ex As Exception
                            Wln(DoTranslation("Download failed for file {0} because the local file is corrupt.", currentLang), "neutralText", strArgs)
                        End Try
                    Else
                        Wln(DoTranslation("You must connect to server before performing transmission.", currentLang), "neutralText")
                    End If
                Else
                    Wln(DoTranslation("Enter a file to download to local directory.", currentLang), "neutralText")
                End If
            ElseIf cmd = "exit" Then
                'Set a flag
                ftpexit = True
            ElseIf cmd = "help" Then
                FTPShowHelp()
            ElseIf cmd.Substring(0, index) = "listlocal" Or cmd.Substring(0, index) = "lsl" Then
                Dim working As String
                If cmd <> "listlocal" Or cmd <> "lsl" Then
                    'List local directory that is not on the current directory
                    If EnvironmentOSType.Contains("Unix") Then
                        working = currDirect + "/" + strArgs
                    Else
                        working = currDirect + "\" + strArgs
                    End If
                    ListLocal(working)
                Else
                    'List current local directory
                    working = currDirect
                    ListLocal(working)
                End If
            ElseIf cmd.Substring(0, index) = "listremote" Or cmd.Substring(0, index) = "lsr" Then
                If cmd <> "listremote" Or cmd <> "lsr" Then
                    If connected = True Then
                        Dim FileSize As Long
                        Dim ModDate As DateTime
                        For Each DirListFTP As FtpListItem In ClientFTP.GetListing(strArgs)
                            W("- " + DirListFTP.FullName + " ", "neutralText")
                            If DirListFTP.Type = FtpFileSystemObjectType.File Then
                                FileSize = ClientFTP.GetFileSize(DirListFTP.FullName)
                                ModDate = ClientFTP.GetModifiedTime(DirListFTP.FullName)

                                'TODO: Make size human-readable
                                W(DoTranslation("{0} | Modified in: {1}", currentLang), "neutralText", FileSize.ToString, ModDate.ToString)
                            End If
                            Console.WriteLine()
                        Next
                    Else
                        Wln(DoTranslation("You should connect to server before listing all remote files.", currentLang), "neutralText")
                    End If
                Else
                    If connected = True Then
                        Dim FileSize As Long
                        Dim ModDate As DateTime
                        For Each DirListFTP As FtpListItem In ClientFTP.GetListing(currentremoteDir)
                            W("- " + DirListFTP.FullName + " ", "neutralText")
                            If DirListFTP.Type = FtpFileSystemObjectType.File Then
                                FileSize = ClientFTP.GetFileSize(DirListFTP.FullName)
                                ModDate = ClientFTP.GetModifiedTime(DirListFTP.FullName)
                                W(DoTranslation("{0} | Modified in: {1}", currentLang), "neutralText", FileSize.ToString, ModDate.ToString)
                            End If
                            Console.WriteLine()
                        Next
                    Else
                        Wln(DoTranslation("You should connect to server before listing all remote files.", currentLang), "neutralText")
                    End If
                End If
            ElseIf cmd.Substring(0, index) = "rename" Or cmd.Substring(0, index) = "ren" Then
                If cmd <> "rename" Or cmd <> "ren" Then
                    If connected = True Then
                        Wln(DoTranslation("Renaming file {0} to {1}...", currentLang), "neutralText", args(0), args(1))

                        'Begin the renaming process
                        ClientFTP.Rename(args(0), args(1))

                        'Show a message
                        Wln(vbNewLine + DoTranslation("Renamed successfully", currentLang), "neutralText")
                    Else
                        Wln(DoTranslation("You must connect to server before performing transmission.", currentLang), "neutralText")
                    End If
                Else
                    Wln(DoTranslation("Enter a file and the new file name.", currentLang), "neutralText")
                End If
            ElseIf cmd.Substring(0, index) = "upload" Or cmd.Substring(0, index) = "put" Then
                If cmd <> "upload" Or cmd <> "put" Then
                    If connected = True Then
                        Wln(DoTranslation("Uploading file {0}...", currentLang), "neutralText", strArgs.Substring(strArgs.IndexOf(" ")))

                        'Begin the uploading process
                        ClientFTP.UploadFile(currDirect + args(0), strArgs.Substring(strArgs.IndexOf(" ")), True, True, FtpVerify.Retry, Complete)
                        Console.WriteLine()

                        'Show a message
                        Wln(vbNewLine + DoTranslation("Uploaded file {0}", currentLang), "neutralText", strArgs.Substring(strArgs.IndexOf(" ")))
                    Else
                        Wln(DoTranslation("You must connect to server before performing transmission.", currentLang), "neutralText")
                    End If
                Else
                    Wln(DoTranslation("Enter a file to upload to remote directory. upload <file> <directory>", currentLang), "neutralText")
                End If
            End If
        Catch ex As Exception 'The InnerException CAN be Nothing
            If DebugMode = True Then
                If Not IsNothing(ex.InnerException) Then 'This is required to fix NullReferenceException when there is nothing in InnerException, so please don't remove.
                    Wln(DoTranslation("Error trying to execute FTP command {3}.", currentLang) + vbNewLine +
                        DoTranslation("Error {0}: {1} ", currentLang) + DoTranslation("(Inner:", currentLang) + " {4})" + vbNewLine + "{2}", "neutralText", Err.Number, Err.Description, ex.StackTrace, words(0), ex.InnerException.Message)
                Else
                    Wln(DoTranslation("Error trying to execute FTP command {3}.", currentLang) + vbNewLine +
                        DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", "neutralText", Err.Number, Err.Description, ex.StackTrace, words(0))
                End If
                Wdbg(ex.StackTrace, True)
            Else
                If Not IsNothing(ex.InnerException) Then
                    Wln(DoTranslation("Error trying to execute FTP command {2}.", currentLang) + vbNewLine +
                        DoTranslation("Error {0}: {1} ", currentLang) + DoTranslation("(Inner:", currentLang) + "{3})", "neutralText", Err.Number, Err.Description, words(0), ex.InnerException.Message)
                Else
                    Wln(DoTranslation("Error trying to execute FTP command {2}.", currentLang) + vbNewLine +
                        DoTranslation("Error {0}: {1}", currentLang), "neutralText", Err.Number, Err.Description, words(0))
                End If
            End If
            EventManager.RaiseFTPCommandError()
        End Try

    End Sub

    Public Sub ListLocal(ByVal dir As String)

        For Each dirfile In IO.Directory.GetDirectories(dir)
            Try
                W("- .{0}: ", "helpCmd", dirfile.Replace(dir, "")) : Wln(DoTranslation("{0} folders, {1} files", currentLang), "helpDef", IO.Directory.GetDirectories(dirfile).Count, IO.Directory.GetFiles(dirfile).Count)
            Catch exc As UnauthorizedAccessException
                Wln(DoTranslation("(Access Denied)", currentLang), "helpDef")
                Continue For
            End Try
        Next
        For Each file In IO.Directory.GetFiles(dir)
            Dim fileinfo As New IO.FileInfo(file)
            Dim size As Double = fileinfo.Length / 1024
            Try
                W("- .{0}: ", "helpCmd", file.Replace(dir, "")) : Wln(DoTranslation("{0} KB, Created in {1} {2}, Modified in {3} {4}", currentLang), "helpDef",
                                                                      FormatNumber(size, 2), FormatDateTime(IO.File.GetCreationTime(file), DateFormat.ShortDate),
                                                                      FormatDateTime(IO.File.GetCreationTime(file), DateFormat.ShortTime),
                                                                      FormatDateTime(IO.File.GetLastWriteTime(file), DateFormat.ShortDate),
                                                                      FormatDateTime(IO.File.GetLastWriteTime(file), DateFormat.ShortTime))
            Catch exc As UnauthorizedAccessException
                Wln(DoTranslation("(Access Denied)", currentLang), "helpDef")
                Continue For
            End Try
        Next

    End Sub

End Module

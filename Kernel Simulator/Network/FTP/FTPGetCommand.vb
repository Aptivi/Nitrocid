
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

    Public response As FtpWebResponse

    Public Sub ExecuteCommand(ByVal cmd As String)

        Dim index As Integer = cmd.IndexOf(" ")
        If (index = -1) Then index = cmd.Length
        Dim words = cmd.Split({" "c})
        Dim strArgs As String = cmd.Substring(index)
        If Not (index = cmd.Length) Then strArgs = strArgs.Substring(1)
        Dim args() As String = strArgs.Split({" "c}, StringSplitOptions.RemoveEmptyEntries)

        'Command code
        Try
            If (cmd = "binary" Or cmd = "bin") Then
                If (connected = False) Then
                    Wln("You should connect to server before switching transfer modes", "neutralText")
                Else
                    'Use Binary mode to transfer files
                    ftpstream.UseBinary = True
                    Binary = True
                    Wln("Transfer mode: Binary", "neutralText")
                End If
            ElseIf (cmd.Substring(0, index) = "connect") Then
                If (cmd <> "connect") Then
                    If (connected = True) Then
                        Wln("You should disconnect from server before connecting to another server", "neutralText")
                    Else
                        Try
                            'Create an FTP stream to connect to
                            ftpstream = WebRequest.Create(strArgs)

                            'Prompt for username and for password
                            W("Username for {0}: ", "input", strArgs)
                            user = Console.ReadLine()
                            If (user = "") Then user = "anonymous"
                            W("Password for {0} at {1}: ", "input", user, strArgs)
                            pass = Console.ReadLine()

                            'Set up credentials
                            ftpstream.Credentials = New NetworkCredential(user, pass)

                            'Get the working directory and get the response
                            ftpstream.Method = WebRequestMethods.Ftp.PrintWorkingDirectory
                            response = ftpstream.GetResponse

                            'Show that it's connected
                            Wln("Connected to {0}", "neutralText", strArgs)
                            Wln(response.WelcomeMessage, "neutralText")

                            'Set up a connected flag
                            connected = True

                            'Prepare to print current FTP directory
                            currentremoteDir = response.ResponseUri.AbsolutePath

                            'Make ftpsite only "ftp://ftp.example.com" or "ftps://ftps.example.com"
                            ftpsite = strArgs
                            If Not (currentremoteDir = "/") Then ftpsite = ftpsite.Replace(currentremoteDir, "")

                        Catch ex As Exception
                            Wdbg("Error connecting to {0}: {1}", True, args(0), ex.Message)
                            Wdbg("{0}", True, ex.StackTrace)
                            If (DebugMode = True) Then
                                Wln("Error when trying to connect to {0}: {1}" + vbNewLine + "Stack Trace: {2}", "neutralText", args(0), ex.Message, ex.StackTrace)
                            Else
                                Wln("Error when trying to connect to {0}: {1}", "neutralText", args(0), ex.Message)
                            End If
                        End Try
                    End If
                Else
                    Wln("Enter an FTP server that starts with ""ftp://"" or ""ftps://""", "neutralText")
                End If
            ElseIf (cmd.Substring(0, index) = "changelocaldir" Or cmd.Substring(0, index) = "cdl") Then
                If (cmd <> "changelocaldir" Or cmd <> "cdl") Then
                    'Check if folder exists
                    Dim targetDir As String
                    If (EnvironmentOSType.Contains("Unix")) Then
                        targetDir = currDirect + "/" + strArgs
                    Else
                        targetDir = currDirect + "\" + strArgs
                    End If
                    If (IO.Directory.Exists(targetDir)) Then
                        'Parse written directory
                        Dim parser As New IO.DirectoryInfo(targetDir)
                        currDirect = parser.FullName
                    Else
                        Wln("Local directory {0} doesn't exist.", "neutralText", strArgs)
                    End If
                Else
                    Wln("Enter a local directory. "".."" to go back.", "neutralText")
                End If
            ElseIf (cmd.Substring(0, index) = "changeremotedir" Or cmd.Substring(0, index) = "cdr") Then
                If (cmd <> "changeremotedir" Or cmd <> "cdr") Then
                    'Old and New directory variable
                    Dim oldDir As String = ftpsite + currentremoteDir + "/"
                    Dim newDir As String = oldDir + strArgs

                    'Open a new stream request which points to changed remote directory
                    ftpstream = WebRequest.Create(newDir)
                    ftpstream.Credentials = New NetworkCredential(user, pass)

                    'A boolean to check to use if the directory exists
                    Dim exist As Boolean

                    'Try to list directory, and if it exists, make "exist = True", otherwise, "exist = False"
                    Try
                        ftpstream.Method = WebRequestMethods.Ftp.ListDirectory
                        Using ftpstream.GetResponse
                            exist = True
                        End Using
                    Catch ex As WebException
                        Wdbg("Error changing directory to {0}: {1}", True, newDir, response.StatusDescription)
                        Wdbg("{0}", True, ex.StackTrace)
                        exist = False
                    End Try

                    If (exist = True) Then
                        'Directory exists, go to the new directory
                        ftpstream = WebRequest.Create(newDir)
                        ftpstream.Credentials = New NetworkCredential(user, pass)
                        ftpstream.Method = WebRequestMethods.Ftp.PrintWorkingDirectory
                        response = ftpstream.GetResponse
                        currentremoteDir = response.ResponseUri.AbsolutePath
                    Else
                        'Directory doesn't exist, go to the old directory
                        Wln("Directory {0} not found. Reason: {1}", "neutralText", strArgs, response.StatusDescription)
                        ftpstream = WebRequest.Create(oldDir)
                        ftpstream.Credentials = New NetworkCredential(user, pass)
                        If (Binary = False) Then ftpstream.UseBinary = False
                    End If
                Else
                    Wln("Enter a remote directory. "".."" to go back", "neutralText")
                End If
            ElseIf (cmd.Substring(0, index) = "currlocaldir" Or cmd.Substring(0, index) = "pwdl") Then
                Wln("Local directory: {0}", "neutralText", currDirect)
            ElseIf (cmd.Substring(0, index) = "currremotedir" Or cmd.Substring(0, index) = "pwdr") Then
                If (connected = True) Then
                    Wln("Remote directory: {0}", "neutralText", currentremoteDir)
                Else
                    Wln("You must connect to server before getting current remote directory.", "neutralText")
                End If
            ElseIf (cmd.Substring(0, index) = "delete" Or cmd.Substring(0, index) = "del") Then
                If (cmd <> "delete" Or cmd <> "del") Then
                    If (connected = True) Then
                        'Old directory and New file variable
                        Dim oldDir As String = ftpsite + currentremoteDir + "/"
                        Dim newFile As String = oldDir + strArgs

                        'Try to delete file
                        Try
                            'Print a message
                            Wln("Deleting file {0}...", "neutralText", strArgs)

                            'Make a request with a file to be deleted
                            ftpstream = WebRequest.Create(newFile)
                            ftpstream.Credentials = New NetworkCredential(user, pass)
                            If (Binary = False) Then ftpstream.UseBinary = False
                            ftpstream.Method = WebRequestMethods.Ftp.DeleteFile

                            'Make a confirmation message so user will not accidentally delete a file
                            W("Are you sure you want to delete file {0} <y/n>?", "input", strArgs)
                            Dim answer As String = Console.ReadKey.KeyChar

                            'If the answer is "y", then delete a file
                            If (answer = "y") Then
                                response = ftpstream.GetResponse
                                Wln(vbNewLine + "Deleted file {0}", "neutralText", strArgs)
                            End If
                        Catch ex As WebException
                            Wdbg("Error deleting file {0}: {1}", True, strArgs, ex.Message)
                            Wdbg("{0}", True, ex.StackTrace)
                            If (DebugMode = True) Then
                                Wln("Error when trying to delete file {0}: {1}" + vbNewLine + "Stack Trace: {2}", "neutralText", strArgs, response.StatusDescription, ex.StackTrace)
                            Else
                                Wln("Error when trying to delete file {0}: {1}", "neutralText", strArgs, response.StatusDescription)
                            End If
                        End Try

                        'Either way, go to the old directory that has the deleted file
                        ftpstream = WebRequest.Create(oldDir)
                        ftpstream.Credentials = New NetworkCredential(user, pass)
                        If (Binary = False) Then ftpstream.UseBinary = False
                    Else
                        Wln("You must connect to server with administrative privileges before performing the deletion.", "neutralText")
                    End If
                Else
                    Wln("Enter a file to remove. You must have administrative permissions on your logged in username to be able to remove.", "neutralText")
                End If
            ElseIf (cmd = "disconnect") Then
                If (connected = True) Then
                    'Set a connected flag to False
                    connected = False
                    Wln("Disconnected from {0}", "neutralText", ftpsite)

                    'Clean up everything
                    ftpsite = ""
                    currentremoteDir = ""
                    user = ""
                    pass = ""
                Else
                    Wln("You haven't connected to any server yet", "neutralText")
                End If
            ElseIf (cmd.Substring(0, index) = "download" Or cmd.Substring(0, index) = "get") Then
                If (cmd <> "download" Or cmd <> "get") Then
                    If (connected = True) Then
                        'Old directory and new file variables
                        Dim oldDir As String = ftpsite + currentremoteDir + "/"
                        Dim newFile As String = oldDir + strArgs

                        'Try to download file
                        Try
                            'Show a message to download
                            Wln("Downloading file {0}...", "neutralText", strArgs)

                            'Make a new request with the new file
                            ftpstream = WebRequest.Create(newFile)
                            ftpstream.Credentials = New NetworkCredential(user, pass)
                            If (Binary = False) Then ftpstream.UseBinary = False
                            ftpstream.Method = WebRequestMethods.Ftp.DownloadFile

                            'Get a response
                            response = ftpstream.GetResponse

                            'Make a stream and get a response stream
                            Dim dStream As IO.Stream = response.GetResponseStream()

                            'Split the strArgs if there is "/"
                            Dim strArgsSeparated As String()
                            strArgsSeparated = strArgs.Split({"/"}, StringSplitOptions.RemoveEmptyEntries)

                            'Conditions if Binary or ASCII
                            If (Binary = True) Then
                                'Make a dFile stream
                                Dim dFile As New IO.FileStream(IO.Path.Combine(currDirect, strArgsSeparated(strArgsSeparated.Length - 1)), IO.FileMode.Create)

                                'Make dStream copy to dFile by binary
                                Wln("Downloading using Binary mode...", "neutralText")
                                dStream.CopyTo(dFile)

                                'Close stream
                                dStream.Close()
                            Else
                                'Make a dFile stream reader
                                Dim dFile As New IO.StreamReader(dStream)

                                'Make a Down stream writer
                                Dim Down As New IO.StreamWriter(IO.Path.Combine(currDirect, strArgsSeparated(strArgsSeparated.Length - 1)), False)

                                'Make dFile copy to Down by text (ASCII)
                                Wln("Downloading using ASCII mode...", "neutralText")
                                Down.WriteLine(dFile.ReadToEnd())

                                'Close stream
                                dStream.Close()
                            End If

                            'Download is finished.
                            Wln(vbNewLine + "Downloaded file {0}. Status {1}", "neutralText", strArgs, response.StatusDescription)
                        Catch ex As WebException
                            Wdbg("Error downloading file {0}: {1}", True, strArgs, ex.Message)
                            Wdbg("{0}", True, ex.StackTrace)
                            If (DebugMode = True) Then
                                Wln("Error when trying to download file {0}: {1}" + vbNewLine + "Stack Trace: {2}", "neutralText", strArgs, response.StatusDescription, ex.StackTrace)
                            Else
                                Wln("Error when trying to download file {0}: {1}", "neutralText", strArgs, response.StatusDescription)
                            End If
                        End Try

                        'Go to the old directory
                        ftpstream = WebRequest.Create(oldDir)
                        ftpstream.Credentials = New NetworkCredential(user, pass)
                        If (Binary = False) Then ftpstream.UseBinary = False
                    Else
                        Wln("You must connect to server before performing transmission.", "neutralText")
                    End If
                Else
                    Wln("Enter a file to download to local directory.", "neutralText")
                End If
            ElseIf (cmd = "exit") Then
                'Set a flag
                ftpexit = True
            ElseIf (cmd = "help") Then
                FTPHelpSystem.FTPShowHelp()
            ElseIf (cmd.Substring(0, index) = "listlocal" Or cmd.Substring(0, index) = "lsl") Then
                Dim working As String
                If (cmd <> "listlocal" Or cmd <> "lsl") Then
                    'List local directory that is not on the current directory
                    If (EnvironmentOSType.Contains("Unix")) Then
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
            ElseIf (cmd.Substring(0, index) = "listremote" Or cmd.Substring(0, index) = "lsr") Then
                Dim line As String = "" : Dim listftp As New List(Of String)
                If (cmd <> "listremote" Or cmd <> "lsr") Then
                    If (connected = True) Then
                        'Old and new directory variables
                        Dim oldDir As String = ftpsite + currentremoteDir + "/"
                        Dim newDir As String = oldDir + strArgs

                        'Make a new request
                        ftpstream = WebRequest.Create(newDir)
                        ftpstream.Credentials = New NetworkCredential(user, pass)
                        If (Binary = False) Then ftpstream.UseBinary = False
                        ftpstream.Method = WebRequestMethods.Ftp.ListDirectoryDetails

                        Try
                            'Get a response
                            response = ftpstream.GetResponse()

                            'Make a directory listing stream reader
                            Dim streamlist As New IO.StreamReader(response.GetResponseStream)

                            'Try to list directory
                            If streamlist IsNot Nothing Then line = streamlist.ReadLine
                            While line IsNot Nothing
                                listftp.Add(line)
                                line = streamlist.ReadLine
                            End While

                            'Print the list
                            For Each listing In listftp
                                Wln(listing, "neutralText")
                            Next
                        Catch ex As WebException
                            Wdbg("Error listing {0}: {1}", True, strArgs, ex.Message)
                            Wdbg("{0}", True, ex.StackTrace)
                            If (DebugMode = True) Then
                                Wln("Error when trying to list folder {0}: {1}" + vbNewLine + "Stack Trace: {2}", "neutralText", strArgs, response.StatusDescription, ex.StackTrace)
                            Else
                                Wln("Error when trying to list folder {0}: {1}", "neutralText", strArgs, response.StatusDescription)
                            End If
                        End Try

                        'Either way, go to the old directory that has the deleted file
                        ftpstream = WebRequest.Create(oldDir)
                        ftpstream.Credentials = New NetworkCredential(user, pass)
                        If (Binary = False) Then ftpstream.UseBinary = False
                    Else
                        Wln("You should connect to server before listing all remote files.", "neutralText")
                    End If
                Else
                    If (connected = True) Then
                        'A variable
                        Dim direct As String = ftpsite + currentremoteDir

                        'List current directory method
                        If (Binary = False) Then ftpstream.UseBinary = False
                        ftpstream.Method = WebRequestMethods.Ftp.ListDirectoryDetails

                        Try
                            'Get a response
                            response = ftpstream.GetResponse()

                            'Make a directory listing stream reader
                            Dim streamlist As New IO.StreamReader(response.GetResponseStream)

                            'Try to make a list
                            If streamlist IsNot Nothing Then line = streamlist.ReadLine
                            While line IsNot Nothing
                                listftp.Add(line)
                                line = streamlist.ReadLine
                            End While

                            'Print the list
                            For Each listing In listftp
                                Wln(listing, "neutralText")
                            Next
                        Catch ex As WebException
                            Wdbg("Error listing {0}: {1}", True, strArgs, ex.Message)
                            Wdbg("{0}", True, ex.StackTrace)
                            If (DebugMode = True) Then
                                Wln("Error when trying to list folder {0}: {1}" + vbNewLine + "Stack Trace: {2}", "neutralText", strArgs, response.StatusDescription, ex.StackTrace)
                            Else
                                Wln("Error when trying to list folder {0}: {1}", "neutralText", strArgs, response.StatusDescription)
                            End If
                        End Try

                        'Either way, go to the old directory that has the deleted file
                        ftpstream = WebRequest.Create(direct)
                        ftpstream.Credentials = New NetworkCredential(user, pass)
                        If (Binary = False) Then ftpstream.UseBinary = False
                    Else
                        Wln("You should connect to server before listing all remote files.", "neutralText")
                    End If
                End If
            ElseIf (cmd = "passive") Then
                If (connected = True And Passive = True) Then
                    'Switch to active mode
                    ftpstream.UsePassive = False
                    Passive = False
                    Wln("FTP mode: Active", "neutralText")
                ElseIf (connected = True And Passive = False) Then
                    'Switch to passive mode
                    ftpstream.UsePassive = True
                    Passive = True
                    Wln("FTP mode: Passive", "neutralText")
                Else
                    Wln("You should connect to server before switching between passive and active transfer mode.", "neutralText")
                End If
            ElseIf (cmd = "ssl") Then
                If (connected = True And SSL = False) Then
                    'Enable SSL
                    ftpstream.EnableSsl = True
                    SSL = True
                    Wln("SSL turned on", "neutralText")
                ElseIf (connected = True And SSL = True) Then
                    'Disable SSL
                    ftpstream.EnableSsl = False
                    SSL = False
                    Wln("SSL turned off", "neutralText")
                Else
                    Wln("You should connect to SSL-secured server before switching SSL mode.", "neutralText")
                End If
            ElseIf (cmd = "text" Or cmd = "txt") Then
                If (connected = False) Then
                    Wln("You should connect to server before switching transfer modes", "neutralText")
                Else
                    ftpstream.UseBinary = False
                    Binary = False
                    Wln("Transfer mode: Text", "neutralText")
                End If
            ElseIf (cmd.Substring(0, index) = "upload" Or cmd.Substring(0, index) = "put") Then
                If (cmd <> "upload" Or cmd <> "put") Then
                    If (connected = True) Then
                        'Old directory and new file variable
                        Dim oldDir As String = ftpsite + currentremoteDir + "/"
                        Dim newFile As String = oldDir + strArgs

                        'Local file which will be uploaded
                        Dim upFile As String
                        If (EnvironmentOSType.Contains("Unix")) Then
                            upFile = currDirect + "/" + strArgs
                        Else
                            upFile = currDirect + "\" + strArgs
                        End If

                        'Try to upload file
                        Try
                            'Print a message
                            Wln("Uploading file {0}...", "neutralText", strArgs)

                            'Make a new request to upload
                            ftpstream = WebRequest.Create(newFile)
                            ftpstream.Credentials = New NetworkCredential(user, pass)
                            If (Binary = False) Then ftpstream.UseBinary = False
                            ftpstream.Method = WebRequestMethods.Ftp.UploadFile

                            'Get a response
                            response = ftpstream.GetResponse

                            'Store the uploading file into array and make a stream
                            Dim uplFile() As Byte = IO.File.ReadAllBytes(upFile)
                            Dim uploading As IO.Stream = ftpstream.GetRequestStream

                            'Begin the uploading process
                            uploading.Write(uplFile, 0, uplFile.Length)

                            'Show a message, close stream
                            Wln(vbNewLine + "Uploaded file {0}", "neutralText", strArgs)
                            uploading.Close()
                        Catch ex As WebException
                            Wdbg("Error uploading {0}: {1}", True, strArgs, ex.Message)
                            Wdbg("{0}", True, ex.StackTrace)
                            If (DebugMode = True) Then
                                Wln("Error when trying to upload {0}: {1}" + vbNewLine + "Stack Trace: {2}", "neutralText", strArgs, response.StatusDescription, ex.StackTrace)
                            Else
                                Wln("Error when trying to upload {0}: {1}", "neutralText", strArgs, response.StatusDescription)
                            End If
                        End Try

                        'Go back to the old directory
                        ftpstream = WebRequest.Create(oldDir)
                        ftpstream.Credentials = New NetworkCredential(user, pass)
                        If (Binary = False) Then ftpstream.UseBinary = False
                    Else
                        Wln("You must connect to server before performing transmission.", "neutralText")
                    End If
                Else
                    Wln("Enter a file to download to local directory.", "neutralText")
                End If
            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln("Error trying to execute FTP command {3}." + vbNewLine + "Error {0}: {1}" + vbNewLine + "{2}", "neutralText", _
                    Err.Number, Err.Description, ex.StackTrace, words(0))
                Wdbg(ex.StackTrace, True)
            Else
                Wln("Error trying to execute FTP command {2}." + vbNewLine + "Error {0}: {1}", "neutralText", Err.Number, Err.Description, words(0))
            End If
        End Try

    End Sub

    Public Sub ListLocal(ByVal dir As String)

        For Each dirfile In IO.Directory.GetDirectories(dir)
            Try
                W("- .{0}: ", "helpCmd", dirfile.Replace(dir, "")) : Wln("{0} folders, {1} files", "helpDef", _
                                                                                                IO.Directory.GetDirectories(dirfile).Count, _
                                                                                                IO.Directory.GetFiles(dirfile).Count)
            Catch exc As UnauthorizedAccessException
                Wln("(Access Denied)", "helpDef")
                Continue For
            End Try
        Next
        For Each file In IO.Directory.GetFiles(dir)
            Dim fileinfo As New IO.FileInfo(file)
            Dim size As Double = fileinfo.Length / 1024
            Try
                W("- .{0}: ", "helpCmd", file.Replace(dir, "")) : Wln("{0} KB, Created in {1} {2}, Modified in {3} {4}", "helpDef", _
                                                                                             FormatNumber(size, 2), FormatDateTime(IO.File.GetCreationTime(file), DateFormat.ShortDate), _
                                                                                             FormatDateTime(IO.File.GetCreationTime(file), DateFormat.ShortTime), _
                                                                                             FormatDateTime(IO.File.GetLastWriteTime(file), DateFormat.ShortDate), _
                                                                                             FormatDateTime(IO.File.GetLastWriteTime(file), DateFormat.ShortTime))
            Catch exc As UnauthorizedAccessException
                Wln("(Access Denied)", "helpDef")
                Continue For
            End Try
        Next

    End Sub

End Module

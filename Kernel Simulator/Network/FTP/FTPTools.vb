
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

Module FTPTools

    Public Sub TryToConnect(ByVal address As String)
        If connected = True Then
            W(DoTranslation("You should disconnect from server before connecting to another server", currentLang), True, "neutralText")
        Else
            Try
                'Create an FTP stream to connect to
                ClientFTP = New FtpClient With {
                    .Host = address,
                    .RetryAttempts = 3
                }

                'Prompt for username and for password
                W(DoTranslation("Username for {0}: ", currentLang), False, "input", address)
                user = Console.ReadLine()
                If user = "" Then user = "anonymous"
                W(DoTranslation("Password for {0}: ", currentLang), False, "input", user)

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
                Dim Uri As New Uri(address)
                ClientFTP = FtpClient.Connect(Uri)

                'Show that it's connected
                W(DoTranslation("Connected to {0}", currentLang), True, "neutralText", address)
                connected = True

                'Prepare to print current FTP directory
                currentremoteDir = ClientFTP.GetWorkingDirectory
                ftpsite = ClientFTP.Host
            Catch ex As Exception
                Wdbg("Error connecting to {0}: {1}", address, ex.Message)
                Wdbg("{0}", ex.StackTrace)
                If DebugMode = True Then
                    W(DoTranslation("Error when trying to connect to {0}: {1}", currentLang) + vbNewLine +
                      DoTranslation("Stack Trace: {2}", currentLang), True, "neutralText", address, ex.Message, ex.StackTrace)
                Else
                    W(DoTranslation("Error when trying to connect to {0}: {1}", currentLang), True, "neutralText", address, ex.Message)
                End If
            End Try
        End If
    End Sub

    Public Sub ListLocal(ByVal dir As String)

        For Each dirfile In IO.Directory.GetDirectories(dir)
            Try
                W("- .{0}: ", False, "helpCmd", dirfile.Replace(dir, "")) : W(DoTranslation("{0} folders, {1} files", currentLang), True, "helpDef", IO.Directory.GetDirectories(dirfile).Count, IO.Directory.GetFiles(dirfile).Count)
            Catch exc As UnauthorizedAccessException
                W(DoTranslation("(Access Denied)", currentLang), True, "helpDef")
                Continue For
            End Try
        Next
        For Each file In IO.Directory.GetFiles(dir)
            Dim fileinfo As New IO.FileInfo(file)
            Dim size As Double = fileinfo.Length / 1024
            Try
                W("- .{0}: ", False, "helpCmd", file.Replace(dir, "")) : W(DoTranslation("{0} KB, Created in {1} {2}, Modified in {3} {4}", currentLang), True, "helpDef",
                                                                           FormatNumber(size, 2), FormatDateTime(IO.File.GetCreationTime(file), DateFormat.ShortDate),
                                                                           FormatDateTime(IO.File.GetCreationTime(file), DateFormat.ShortTime),
                                                                           FormatDateTime(IO.File.GetLastWriteTime(file), DateFormat.ShortDate),
                                                                           FormatDateTime(IO.File.GetLastWriteTime(file), DateFormat.ShortTime))
            Catch exc As UnauthorizedAccessException
                W(DoTranslation("(Access Denied)", currentLang), True, "helpDef")
                Continue For
            End Try
        Next

    End Sub

End Module

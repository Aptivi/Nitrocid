
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

Imports System.Threading

Public Module Filesystem

    'Variables
    Public CurrDirStructure As New List(Of String)
    Public CurrDir As String
    Public UACNotice As New Thread(AddressOf UACNoticeShow)


    'Subs
    Public Sub SetCurrDir(ByVal dir As String)
        Dim direct As String
        dir = dir.Replace("\", "/")
        direct = $"{CurrDir}/{dir}"
        Wdbg("I", "Prototype directory: {0}", direct)
        If direct.Contains(CurrDir.Replace("\", "/")) And direct.AllIndexesOf(CurrDir.Replace("\", "/")).Count > 1 Then
            direct = ReplaceLastOccurrence(direct, CurrDir, "")
        End If
        Wdbg("I", "Final directory: {0}", direct)
        Wdbg("I", "Directory exists? {1}", IO.Directory.Exists(direct))
        If IO.Directory.Exists(direct) Then
            Try
                Dim Parser As New IO.DirectoryInfo(direct)
                CurrDir = Parser.FullName.Replace("\", "/")
                Wdbg("I", "Initializing structure of {0}...", CurrDir)
                InitStructure()
            Catch sex As Security.SecurityException
                Wdbg("E", "Security error: {0} ({1})", sex.Message, sex.PermissionType)
                W(DoTranslation("You are unauthorized to set current directory to {0}: {1}", currentLang), True, ColTypes.Neutral, direct, sex.Message)
                WStkTrc(sex)
            Catch ptlex As IO.PathTooLongException
                Wdbg("I", "Directory length: {0}", direct.Length)
                W(DoTranslation("The path you've specified is too long.", currentLang), True, ColTypes.Neutral)
                WStkTrc(ptlex)
            End Try
        Else
            W(DoTranslation("Directory {0} not found", currentLang), True, ColTypes.Neutral, dir)
        End If
    End Sub
    Public Sub ReadContents(ByVal filename As String)
        Using FStream As New IO.StreamReader(filename)
            Wdbg("I", "Stream to file {0} opened.", filename)
            While Not FStream.EndOfStream
                W(FStream.ReadLine, True, ColTypes.Neutral)
            End While
        End Using
    End Sub
    Public Sub InitFS()
        CurrDir = paths("Home")
        Wdbg("I", "Initializing structure of {0}...", CurrDir)
        InitStructure()
    End Sub
    Public Sub InitStructure()
        Wdbg("I", "Populating list...")
        UACNotice.Start()
        Dim DirStructure As New List(Of String)
        Wdbg("I", "Populating directory structure...")
        DirStructure.AddRange(IO.Directory.EnumerateDirectories(CurrDir, "*", IO.SearchOption.TopDirectoryOnly))
        For Each Dir As String In DirStructure
            Try
                'Why CurrDirStructure? Because "For Each" block doesn't support modifications inside the loop.
                Wdbg("I", "Populating directory structure for ""{0}""...", Dir)
                CurrDirStructure.AddRange(IO.Directory.EnumerateDirectories(Dir, "*", IO.SearchOption.AllDirectories))
            Catch ex As UnauthorizedAccessException
                Dim OffendingDir As String = ex.Message.Substring(ex.Message.IndexOf("'") + 1, ex.Message.LastIndexOf("'") - ex.Message.IndexOf("'"))
                OffendingDir = OffendingDir.Replace("'", "")
                Wdbg("E", "Unauthorized to enumerate a directory. {0}", OffendingDir)
            End Try
        Next
        CurrDirStructure.AddRange(DirStructure)
        CurrDirStructure.Add(CurrDir)
        Wdbg("I", "All structures added with the count of {0}", CurrDirStructure.Count)
        For i As Integer = 0 To CurrDirStructure.Count - 1
            CurrDirStructure(i) = CurrDirStructure(i).Replace("\", "/")
        Next
        Wdbg("I", "All directories are made universal to all platforms.")
        UACNotice.Abort()
        UACNotice = New Thread(AddressOf UACNoticeShow)
    End Sub
    Public Sub List(ByVal folder As String)
        Wdbg("I", "Folder {0} will be checked if it is empty or equals CurrDir ({1})...", folder, CurrDir)
        If Not folder = CurrDir And Not folder = "" Then
            folder = $"{CurrDir}/{folder}"
            If folder.Contains(CurrDir.Replace("\", "/")) Then
                folder = folder.Replace(CurrDir, "").Remove(0, 1)
            End If
        End If
        Wdbg("I", "Final folder: {0}", folder)
        If CurrDirStructure.Contains(folder) Or IO.Directory.Exists(folder) Then
            Dim enumeration As IEnumerable(Of String)
            Try
                enumeration = IO.Directory.EnumerateFileSystemEntries(folder)
            Catch sex As Security.SecurityException
                W(DoTranslation("You are unauthorized to list in {0}: {1}", currentLang), True, ColTypes.Neutral, folder, sex.Message)
                W(DoTranslation("Permission {0} failed", currentLang), True, ColTypes.Neutral, sex.PermissionType)
                WStkTrc(sex)
                Exit Sub
            Catch ptlex As IO.PathTooLongException
                W(DoTranslation("The path you've specified is too long.", currentLang), True, ColTypes.Neutral)
                WStkTrc(ptlex)
                Exit Sub
            Catch ex As Exception
                W(DoTranslation("Unknown error while listing in directory: {0}", currentLang), True, ColTypes.Neutral, ex.Message)
                WStkTrc(ex)
                Exit Sub
            End Try
#Disable Warning BC42104
            For Each Entry As String In enumeration
#Enable Warning BC42104
                Wdbg("I", "Enumerating {0}...", Entry)
                Try
                    If IO.File.Exists(Entry) Then
                        Dim FInfo As New IO.FileInfo(Entry)

                        'Print information
                        W("- " + Entry + ": " + DoTranslation("{0} KB, Created in {1} {2}, Modified in {3} {4}", currentLang), True, ColTypes.Neutral,
                            FormatNumber(FInfo.Length / 1024, 2), FInfo.CreationTime.ToShortDateString, FInfo.CreationTime.ToShortTimeString,
                                                                  FInfo.LastWriteTime.ToShortDateString, FInfo.LastWriteTime.ToShortTimeString)
                    ElseIf IO.Directory.Exists(Entry) Then
                        Dim DInfo As New IO.DirectoryInfo(Entry)

                        'Get all file sizes in a folder
                        Dim Files As List(Of IO.FileInfo)
                        If FullParseMode Then
                            Files = DInfo.EnumerateFiles("*", IO.SearchOption.AllDirectories).ToList
                        Else
                            Files = DInfo.EnumerateFiles("*", IO.SearchOption.TopDirectoryOnly).ToList
                        End If
                        Wdbg("I", "{0} files to be parsed", Files.Count)
                        Dim TotalSize As Long = 0 'In bytes
                        For Each DFile As IO.FileInfo In Files
                            Wdbg("I", "File {0}, Size {1} bytes", DFile.Name, DFile.Length)
                            TotalSize += DFile.Length
                        Next

                        'Print information
                        W("- " + Entry + ": " + DoTranslation("{0} KB, Created in {1} {2}, Modified in {3} {4}", currentLang), True, ColTypes.Neutral,
                            FormatNumber(TotalSize / 1024, 2), DInfo.CreationTime.ToShortDateString, DInfo.CreationTime.ToShortTimeString,
                                                               DInfo.LastWriteTime.ToShortDateString, DInfo.LastWriteTime.ToShortTimeString)
                    End If
                Catch ex As UnauthorizedAccessException 'Error while getting info
                    W("- " + DoTranslation("You are not authorized to get info for {0}.", currentLang), True, ColTypes.Neutral, Entry)
                    WStkTrc(ex)
                End Try
            Next
        Else
            W(DoTranslation("Directory {0} not found", currentLang), True, ColTypes.Neutral, folder)
            Wdbg("I", "CurrDirStructure = {0}, IO.Directory.Exists = {1}", CurrDirStructure.Contains(folder), IO.Directory.Exists(folder))
        End If
    End Sub
    Private Sub UACNoticeShow()
        Try
            For i As Integer = 0 To 10
                If i = 10 Then
                    W(DoTranslation("It seems that the file system population takes too long.", currentLang), True, ColTypes.Neutral)
                End If
                Thread.Sleep(1000)
            Next
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

End Module

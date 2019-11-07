
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

Public Module Filesystem

    'Variables
    Public CurrDirStructure As New List(Of String)
    Public CurrDir As String

    'Subs
    Public Sub SetCurrDir(ByVal dir As String)
        Dim direct As String
        direct = $"{CurrDir}/{dir}"
        If direct.Contains(CurrDir.Replace("\", "/")) Then
            direct = direct.Replace(CurrDir, "").Remove(0, 1)
        End If
        Wdbg("Directory {0} exists? {1}", direct, IO.Directory.Exists(direct))
        If IO.Directory.Exists(direct) Then
            Try
                Dim Parser As New IO.DirectoryInfo(direct)
                CurrDir = Parser.FullName.Replace("\", "/")
                Wdbg("Initializing structure of {0}...", CurrDir)
                InitStructure()
            Catch sex As Security.SecurityException
                W(DoTranslation("You are unauthorized to set current directory to {0}: {1}", currentLang), True, ColTypes.Neutral, direct, sex.Message)
                W(DoTranslation("Permission {0} failed", currentLang), True, ColTypes.Neutral, sex.PermissionType)
                WStkTrc(sex)
            Catch ptlex As IO.PathTooLongException
                W(DoTranslation("The path you've specified is too long.", currentLang), True, ColTypes.Neutral)
                WStkTrc(ptlex)
            End Try
        Else
            W(DoTranslation("Directory {0} not found", currentLang), True, ColTypes.Neutral, dir)
        End If
    End Sub
    Public Sub ReadContents(ByVal filename As String)
        Using FStream As New IO.StreamReader(filename)
            Wdbg("Stream to file {0} opened.", filename)
            While Not FStream.EndOfStream
                W(FStream.ReadLine, True, ColTypes.Neutral)
            End While
        End Using
    End Sub
    Public Sub InitFS()
        CurrDir = paths("Home")
        Wdbg("Initializing structure of {0}...", CurrDir)
        InitStructure()
    End Sub
    Public Sub InitStructure()
        Wdbg("Populating list...")
        CurrDirStructure.AddRange(IO.Directory.EnumerateFileSystemEntries(CurrDir, "*", IO.SearchOption.TopDirectoryOnly))
        CurrDirStructure.Add(CurrDir)
        Wdbg("All structures added with the count of {0}", CurrDirStructure.Count)
        For i As Integer = 0 To CurrDirStructure.Count - 1
            CurrDirStructure(i) = CurrDirStructure(i).Replace("\", "/")
        Next
        Wdbg("All directories are made universal to all platforms.")
    End Sub
    Public Sub List(ByVal folder As String)
        Wdbg("Folder {0} will be checked if it is empty or equals CurrDir ({1})...", folder, CurrDir)
        If Not folder = CurrDir And Not folder = "" Then folder = $"{CurrDir}/{folder}"
        Wdbg("Final folder: {0}", folder)
        If CurrDirStructure.Contains(folder) Then
            If IO.Directory.Exists(folder) Then
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
                    Wdbg("Enumerating {0}...", Entry)
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
                            Dim TotalSize As Long = 0 'In bytes
                            For Each DFile As IO.FileInfo In Files
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
                Wdbg("IO.Directory.Exists returned False")
            End If
        Else
            W(DoTranslation("{0} is not found.", currentLang), True, ColTypes.Neutral, folder)
            Wdbg("CurrDirStructure doesn't contain {0}.", folder)
        End If
    End Sub

End Module

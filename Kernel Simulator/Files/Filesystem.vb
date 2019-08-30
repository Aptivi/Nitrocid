
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
        If IO.Directory.Exists(direct) Then
            Dim Parser As New IO.DirectoryInfo(direct)
            CurrDir = Parser.FullName.Replace("\", "/")
            InitStructure()
        Else
            W(DoTranslation("Directory {0} not found", currentLang), True, "neutralText", dir)
        End If
    End Sub
    Public Sub ReadContents(ByVal filename As String)
        Using FStream As New IO.StreamReader(filename)
            While Not FStream.EndOfStream
                W(FStream.ReadLine, True, "neutralText")
            End While
        End Using
    End Sub
    Public Sub InitFS()
        CurrDir = paths("Home")
        InitStructure()
    End Sub
    Public Sub InitStructure()
        CurrDirStructure.AddRange(IO.Directory.EnumerateFileSystemEntries(CurrDir, "*", IO.SearchOption.TopDirectoryOnly))
        CurrDirStructure.Add(CurrDir)
        For i As Integer = 0 To CurrDirStructure.Count - 1
            CurrDirStructure(i) = CurrDirStructure(i).Replace("\", "/")
        Next
    End Sub
    Public Sub List(ByVal folder As String)
        If Not folder = CurrDir And Not folder = "" Then folder = $"{CurrDir}/{folder}"
        If CurrDirStructure.Contains(folder) Then
            If IO.Directory.Exists(folder) Then
                For Each Entry As String In IO.Directory.EnumerateFileSystemEntries(folder)
                    Try
                        If IO.File.Exists(Entry) Then
                            Dim FInfo As New IO.FileInfo(Entry)

                            'Print information
                            W("- " + Entry + ": " + DoTranslation("{0} KB, Created in {1} {2}, Modified in {3} {4}", currentLang), True, "neutralText",
                                FormatNumber(FInfo.Length / 1024, 2), FInfo.CreationTime.ToShortDateString, FInfo.CreationTime.ToShortTimeString,
                                                                      FInfo.LastWriteTime.ToShortDateString, FInfo.LastWriteTime.ToShortTimeString)
                        ElseIf IO.Directory.Exists(Entry) Then
                            Dim DInfo As New IO.DirectoryInfo(Entry)

                            'Get all file sizes in a folder
                            Dim Files As List(Of IO.FileInfo) = DInfo.EnumerateFiles("*", IO.SearchOption.AllDirectories).ToList
                            Dim TotalSize As Long = 0 'In bytes
                            For Each DFile As IO.FileInfo In Files
                                TotalSize += DFile.Length
                            Next

                            'Print information
                            W("- " + Entry + ": " + DoTranslation("{0} KB, Created in {1} {2}, Modified in {3} {4}", currentLang), True, "neutralText",
                                FormatNumber(TotalSize / 1024, 2), DInfo.CreationTime.ToShortDateString, DInfo.CreationTime.ToShortTimeString,
                                                                   DInfo.LastWriteTime.ToShortDateString, DInfo.LastWriteTime.ToShortTimeString)
                        End If
                    Catch ex As UnauthorizedAccessException 'Error while getting info
                        W("- " + DoTranslation("You are not authorized to get info for {0}.", currentLang), True, "neutralText", Entry)
                    End Try
                Next
            Else
                W(DoTranslation("Directory {0} not found", currentLang), True, "neutralText", folder)
            End If
        Else
            W(DoTranslation("{0} is not found.", currentLang), True, "neutralText", folder)
        End If
    End Sub

End Module

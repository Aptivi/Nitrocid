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

Public Module Filesystem

    'Variables
    Public CurrDir As String

    'Subs
    ''' <summary>
    ''' Sets the current working directory
    ''' </summary>
    ''' <param name="dir">A directory</param>
    Public Sub SetCurrDir(ByVal dir As String)
        dir = NeutralizePath(dir)
        Wdbg("I", "Directory exists? {0}", IO.Directory.Exists(dir))
        If IO.Directory.Exists(dir) Then
            Try
                Dim Parser As New IO.DirectoryInfo(dir)
                CurrDir = Parser.FullName.Replace("\", "/")
            Catch sex As Security.SecurityException
                Wdbg("E", "Security error: {0} ({1})", sex.Message, sex.PermissionType)
                W(DoTranslation("You are unauthorized to set current directory to {0}: {1}", currentLang), True, ColTypes.Err, dir, sex.Message)
                WStkTrc(sex)
            Catch ptlex As IO.PathTooLongException
                Wdbg("I", "Directory length: {0}", dir.Length)
                W(DoTranslation("The path you've specified is too long.", currentLang), True, ColTypes.Err)
                WStkTrc(ptlex)
            End Try
        Else
            W(DoTranslation("Directory {0} not found", currentLang), True, ColTypes.Err, dir)
        End If
    End Sub

    ''' <summary>
    ''' Reads the contents of a file and writes it to the console
    ''' </summary>
    ''' <param name="filename">Full path to file</param>
    Public Sub ReadContents(ByVal filename As String)
        Using FStream As New IO.StreamReader(filename)
            Wdbg("I", "Stream to file {0} opened.", filename)
            While Not FStream.EndOfStream
                W(FStream.ReadLine, True, ColTypes.Neutral)
            End While
        End Using
    End Sub

    ''' <summary>
    ''' List all files and folders in a specified folder
    ''' </summary>
    ''' <param name="folder">Full path to folder</param>
    Public Sub List(ByVal folder As String)
        Wdbg("I", "Folder {0} will be checked if it is empty or equals CurrDir ({1})...", folder, CurrDir)
        If IO.Directory.Exists(folder) Then
            Dim enumeration As IEnumerable(Of String)
            Try
                enumeration = IO.Directory.EnumerateFileSystemEntries(folder)
            Catch sex As Security.SecurityException
                W(DoTranslation("You are unauthorized to list in {0}: {1}", currentLang), True, ColTypes.Err, folder, sex.Message)
                W(DoTranslation("Permission {0} failed", currentLang), True, ColTypes.Err, sex.PermissionType)
                WStkTrc(sex)
                Exit Sub
            Catch ptlex As IO.PathTooLongException
                W(DoTranslation("The path you've specified is too long.", currentLang), True, ColTypes.Err)
                WStkTrc(ptlex)
                Exit Sub
            Catch ex As Exception
                W(DoTranslation("Unknown error while listing in directory: {0}", currentLang), True, ColTypes.Err, ex.Message)
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
                        If (FInfo.Attributes = IO.FileAttributes.Hidden And HiddenFiles) Or Not FInfo.Attributes.HasFlag(IO.FileAttributes.Hidden) Then
                            If FInfo.Name.EndsWith(".uesh") Then
                                W("- " + FInfo.Name + ": ", False, ColTypes.Stage)
                            Else
                                W("- " + FInfo.Name + ": ", False, ColTypes.HelpCmd)
                            End If
                            W(DoTranslation("{0} KB, Created in {1} {2}, Modified in {3} {4}", currentLang), True, ColTypes.HelpDef,
                              FormatNumber(FInfo.Length / 1024, 2), FInfo.CreationTime.ToShortDateString, FInfo.CreationTime.ToShortTimeString,
                                                                    FInfo.LastWriteTime.ToShortDateString, FInfo.LastWriteTime.ToShortTimeString)
                        End If
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
                            If (DFile.Attributes = IO.FileAttributes.Hidden And HiddenFiles) Or Not DFile.Attributes.HasFlag(IO.FileAttributes.Hidden) Then
                                Wdbg("I", "File {0}, Size {1} bytes", DFile.Name, DFile.Length)
                                TotalSize += DFile.Length
                            End If
                        Next

                        'Print information
                        If (DInfo.Attributes = IO.FileAttributes.Hidden And HiddenFiles) Or Not DInfo.Attributes.HasFlag(IO.FileAttributes.Hidden) Then
                            W("- " + DInfo.Name + "/: ", False, ColTypes.HelpCmd)
                            W(DoTranslation("{0} KB, Created in {1} {2}, Modified in {3} {4}", currentLang), True, ColTypes.HelpDef,
                              FormatNumber(TotalSize / 1024, 2), DInfo.CreationTime.ToShortDateString, DInfo.CreationTime.ToShortTimeString,
                                                                 DInfo.LastWriteTime.ToShortDateString, DInfo.LastWriteTime.ToShortTimeString)
                        End If
                    End If
                Catch ex As UnauthorizedAccessException 'Error while getting info
                    Dim Directory As String = Entry.Replace("\", "/").Split("/")(Entry.Replace("\", "/").Split("/").Length - 1)
                    W("- " + DoTranslation("You are not authorized to get info for {0}.", currentLang), True, ColTypes.Err, Directory)
                    WStkTrc(ex)
                End Try
            Next
        Else
            W(DoTranslation("Directory {0} not found", currentLang), True, ColTypes.Err, folder)
            Wdbg("I", "IO.Directory.Exists = {0}", IO.Directory.Exists(folder))
        End If
    End Sub

    ''' <summary>
    ''' Simplifies the path to the correct one. It converts the path format to the unified format.
    ''' </summary>
    ''' <param name="Path">Target path, be it a file or a folder</param>
    ''' <returns>Absolute path</returns>
    Public Function NeutralizePath(ByVal Path As String)
        Path = Path.Replace("\", "/")
        If (EnvironmentOSType.Contains("Windows") And Not Path.Contains(":/")) Or EnvironmentOSType.Contains("Unix") Then
            Path = $"{CurrDir}/{Path}"
        End If
        Wdbg("I", "Prototype directory: {0}", Path)
        If Path.Contains(CurrDir.Replace("\", "/")) And Path.AllIndexesOf(CurrDir.Replace("\", "/")).Count > 1 Then
            Path = ReplaceLastOccurrence(Path, CurrDir, "")
        End If
        Wdbg("I", "Final directory: {0}", Path)
        Return Path
    End Function

End Module

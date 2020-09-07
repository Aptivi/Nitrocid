
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

Imports System.Text
Imports MailKit

Public Module MailDirectory

    ''' <summary>
    ''' Changes current mail directory
    ''' </summary>
    ''' <param name="Directory">A mail directory</param>
    Public Sub MailChangeDirectory(ByVal Directory As String)
        Wdbg("I", "Opening folder: {0}", Directory)
        Try
            SyncLock IMAP_Client.SyncRoot
                OpenFolder(Directory)
            End SyncLock
            IMAP_CurrentDirectory = Directory
            Wdbg("I", "Current directory changed.")
        Catch ex As Exception
            Wdbg("E", "Failed to open folder {0}: {1}", Directory, ex.Message)
            WStkTrc(ex)
            Throw New EventsAndExceptions.MailException(DoTranslation("Unable to open mail folder {0}: {1}", currentLang).FormatString(Directory, ex.Message))
        End Try
    End Sub

    ''' <summary>
    ''' Locates the normal (not special) folder and opens it.
    ''' </summary>
    ''' <param name="FolderString">A folder to open (not a path)</param>
    ''' <returns>A folder</returns>
    Public Function OpenFolder(ByVal FolderString As String, Optional ByVal FolderMode As FolderAccess = FolderAccess.ReadWrite) As MailFolder
        Dim Opened As MailFolder
        Wdbg("I", "Personal namespace collection parsing started.")
        For Each nmspc As FolderNamespace In IMAP_Client.PersonalNamespaces
            Wdbg("I", "Namespace: {0}", nmspc.Path)
            For Each dir As MailFolder In IMAP_Client.GetFolders(nmspc)
                If dir.Name = FolderString Then
                    dir.Open(FolderMode)
                    Opened = dir
                End If
            Next
        Next

        Wdbg("I", "Shared namespace collection parsing started.")
        For Each nmspc As FolderNamespace In IMAP_Client.SharedNamespaces
            Wdbg("I", "Namespace: {0}", nmspc.Path)
            For Each dir As MailFolder In IMAP_Client.GetFolders(nmspc)
                If dir.Name = FolderString Then
                    dir.Open(FolderMode)
                    Opened = dir
                End If
            Next
        Next

        Wdbg("I", "Other namespace collection parsing started.")
        For Each nmspc As FolderNamespace In IMAP_Client.OtherNamespaces
            Wdbg("I", "Namespace: {0}", nmspc.Path)
            For Each dir As MailFolder In IMAP_Client.GetFolders(nmspc)
                If dir.Name = FolderString Then
                    dir.Open(FolderMode)
                    Opened = dir
                End If
            Next
        Next

#Disable Warning BC42104
        Return Opened
#Enable Warning BC42104
    End Function

    ''' <summary>
    ''' Lists directories
    ''' </summary>
    ''' <returns>String list</returns>
    Public Function MailListDirectories() As String
        Dim EntryBuilder As New StringBuilder
        SyncLock IMAP_Client.SyncRoot
            Wdbg("I", "Personal namespace collection parsing started.")
            For Each nmspc As FolderNamespace In IMAP_Client.PersonalNamespaces
                Wdbg("I", "Namespace: {0}", nmspc.Path)
                EntryBuilder.AppendLine($"- {nmspc.Path}")
                For Each dir As MailFolder In IMAP_Client.GetFolders(nmspc)
                    Wdbg("I", "Folder: {0}", dir.Name)
                    EntryBuilder.AppendLine($"  - {dir.Name}")
                Next
            Next

            Wdbg("I", "Shared namespace collection parsing started.")
            For Each nmspc As FolderNamespace In IMAP_Client.SharedNamespaces
                Wdbg("I", "Namespace: {0}", nmspc.Path)
                EntryBuilder.AppendLine($"- {nmspc.Path}")
                For Each dir As MailFolder In IMAP_Client.GetFolders(nmspc)
                    Wdbg("I", "Folder: {0}", dir.Name)
                    EntryBuilder.AppendLine($"  - {dir.Name}")
                Next
            Next

            Wdbg("I", "Other namespace collection parsing started.")
            For Each nmspc As FolderNamespace In IMAP_Client.OtherNamespaces
                Wdbg("I", "Namespace: {0}", nmspc.Path)
                EntryBuilder.AppendLine($"- {nmspc.Path}")
                For Each dir As MailFolder In IMAP_Client.GetFolders(nmspc)
                    Wdbg("I", "Folder: {0}", dir.Name)
                    EntryBuilder.AppendLine($"  - {dir.Name}")
                Next
            Next
        End SyncLock
        Return EntryBuilder.ToString
    End Function

End Module

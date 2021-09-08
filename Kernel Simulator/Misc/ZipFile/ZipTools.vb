
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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
Imports System.IO.Compression

Public Module ZipTools

    ''' <summary>
    ''' Lists all ZIP entries according to the target directory or the current directory
    ''' </summary>
    ''' <param name="Target">Target directory in an archive</param>
    Public Function ListZipEntries(Target As String) As List(Of ZipArchiveEntry)
        If String.IsNullOrWhiteSpace(Target) Then Target = ZipShell_CurrentArchiveDirectory
        Dim Entries As New List(Of ZipArchiveEntry)
        For Each ArchiveEntry As ZipArchiveEntry In ZipShell_ZipArchive?.Entries
            Wdbg("I", "Parsing entry {0}...", ArchiveEntry.FullName)
            If Target IsNot Nothing Then
                If ArchiveEntry.FullName.StartsWith(Target) Then
                    Wdbg("I", "Entry {0} found in target {1}. Adding...", ArchiveEntry.FullName, Target)
                    Entries.Add(ArchiveEntry)
                End If
            ElseIf Target Is Nothing Then
                Wdbg("I", "Adding entry {0}...", ArchiveEntry.FullName)
                Entries.Add(ArchiveEntry)
            End If
        Next
        Wdbg("I", "Entries: {0}", Entries.Count)
        Return Entries
    End Function

    ''' <summary>
    ''' Extracts a ZIP entry to a target directory
    ''' </summary>
    ''' <param name="Target">Target file in an archive</param>
    ''' <param name="Where">Where in the local filesystem to extract?</param>
    Public Function ExtractZipFileEntry(Target As String, Where As String, Optional FullTargetPath As Boolean = False) As Boolean
        If String.IsNullOrWhiteSpace(Target) Then Throw New ArgumentException(DoTranslation("Can't extract nothing."))
        If String.IsNullOrWhiteSpace(Where) Then Where = ZipShell_CurrentDirectory

        'Define absolute target
        Dim AbsoluteTarget As String = ZipShell_CurrentArchiveDirectory + "/" + Target
        If AbsoluteTarget.StartsWith("/") Then AbsoluteTarget = AbsoluteTarget.RemoveLetter(0)
        Wdbg("I", "Target: {0}, AbsoluteTarget: {1}", Target, AbsoluteTarget)

        'Define local destination while getting an entry from target
        Dim LocalDestination As String = Where + "/"
        Dim ZipEntry As ZipArchiveEntry = ZipShell_ZipArchive.GetEntry(AbsoluteTarget)
        If FullTargetPath Then
            LocalDestination += ZipEntry.FullName.Replace(ZipEntry.Name, "")
        End If
        Wdbg("I", "Where: {0}", LocalDestination)

        'Try to extract file
        Directory.CreateDirectory(LocalDestination)
        ZipEntry.ExtractToFile(LocalDestination + ZipEntry.Name, True)
        Return True
    End Function

    ''' <summary>
    ''' Changes the working archive directory
    ''' </summary>
    ''' <param name="Target">Target directory</param>
    Public Function ChangeWorkingArchiveDirectory(Target As String) As Boolean
        If String.IsNullOrWhiteSpace(Target) Then Target = ZipShell_CurrentArchiveDirectory

        'Check to see if we're going back
        If Target.Contains("..") Then
            Wdbg("I", "Target contains going back. Counting...")
            Dim CADSplit As List(Of String) = ZipShell_CurrentArchiveDirectory.Split("/").ToList
            Dim TargetSplit As List(Of String) = Target.Split("/").ToList
            Dim CADBackSteps As Integer

            'Add back steps if target is ".."
            Wdbg("I", "Target length: {0}", TargetSplit.Count)
            For i As Integer = 0 To TargetSplit.Count - 1
                Wdbg("I", "Target part {0}: {1}", i, TargetSplit(i))
                If TargetSplit(i) = ".." Then
                    Wdbg("I", "Target is going back. Adding step...")
                    CADBackSteps += 1
                    TargetSplit(i) = ""
                    Wdbg("I", "Steps: {0}", CADBackSteps)
                End If
            Next

            'Remove empty strings
            TargetSplit.RemoveAll(Function(x) x = "")
            Wdbg("I", "Target length: {0}", TargetSplit.Count)

            'Remove every last entry that goes back
            Wdbg("I", "Old CADSplit length: {0}", CADSplit.Count)
            For Steps As Integer = CADBackSteps To 1 Step -1
                Wdbg("I", "Current step: {0}", Steps)
                Wdbg("I", "Removing index {0} from CADSplit...", CADSplit.Count - Steps)
                CADSplit.RemoveAt(CADSplit.Count - Steps)
                Wdbg("I", "New CADSplit length: {0}", CADSplit.Count)
            Next

            'Set current archive directory and target
            ZipShell_CurrentArchiveDirectory = String.Join("/", CADSplit)
            Wdbg("I", "Setting CAD to {0}...", ZipShell_CurrentArchiveDirectory)
            Target = String.Join("/", TargetSplit)
            Wdbg("I", "Setting target to {0}...", Target)
        End If

        'Prepare the target
        Target = ZipShell_CurrentArchiveDirectory + "/" + Target
        If Target.StartsWith("/") Then Target = Target.RemoveLetter(0)
        Wdbg("I", "Setting target to {0}...", Target)

        'Enumerate entries
        For Each Entry As ZipArchiveEntry In ListZipEntries(Target)
            Wdbg("I", "Entry: {0}", Entry.FullName)
            If Entry.FullName.StartsWith(Target) Then
                Wdbg("I", "{0} found ({1}). Changing...", Target, Entry.FullName)
                ZipShell_CurrentArchiveDirectory = Entry.FullName.RemoveLetter(Entry.FullName.Length - 1)
                Wdbg("I", "Setting CAD to {0}...", ZipShell_CurrentArchiveDirectory)
                Return True
            End If
        Next

        'Assume that we didn't find anything.
        Wdbg("E", "{0} not found.", Target)
        Return False
    End Function

    ''' <summary>
    ''' Changes the working local directory
    ''' </summary>
    ''' <param name="Target">Target directory</param>
    Public Function ChangeWorkingZipLocalDirectory(Target As String) As Boolean
        If String.IsNullOrWhiteSpace(Target) Then Target = ZipShell_CurrentDirectory
        If Directory.Exists(NeutralizePath(Target, ZipShell_CurrentDirectory)) Then
            Wdbg("I", "{0} found. Changing...", Target)
            ZipShell_CurrentDirectory = Target
            Return True
        Else
            Wdbg("E", "{0} not found.", Target)
            Return False
        End If
    End Function

End Module

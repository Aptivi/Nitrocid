
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Imports KS.Files.Querying
Imports KS.Files.Operations
Imports SharpCompress.Archives.Rar
Imports System.IO
Imports SharpCompress.Readers

Namespace Misc.RarFile
    Public Module RarTools

        ''' <summary>
        ''' Lists all RAR entries according to the target directory or the current directory
        ''' </summary>
        ''' <param name="Target">Target directory in an archive</param>
        Public Function ListRarEntries(Target As String) As List(Of RarArchiveEntry)
            If String.IsNullOrWhiteSpace(Target) Then Target = RarShell_CurrentArchiveDirectory
            Dim Entries As New List(Of RarArchiveEntry)
            For Each ArchiveEntry As RarArchiveEntry In RarShell_RarArchive?.Entries
                Wdbg(DebugLevel.I, "Parsing entry {0}...", ArchiveEntry.Key)
                If Target IsNot Nothing Then
                    If ArchiveEntry.Key.StartsWith(Target) Then
                        Wdbg(DebugLevel.I, "Entry {0} found in target {1}. Adding...", ArchiveEntry.Key, Target)
                        Entries.Add(ArchiveEntry)
                    End If
                ElseIf Target Is Nothing Then
                    Wdbg(DebugLevel.I, "Adding entry {0}...", ArchiveEntry.Key)
                    Entries.Add(ArchiveEntry)
                End If
            Next
            Wdbg(DebugLevel.I, "Entries: {0}", Entries.Count)
            Return Entries
        End Function

        ''' <summary>
        ''' Extracts a RAR entry to a target directory
        ''' </summary>
        ''' <param name="Target">Target file in an archive</param>
        ''' <param name="Where">Where in the local filesystem to extract?</param>
        Public Function ExtractRarFileEntry(Target As String, Where As String, Optional FullTargetPath As Boolean = False) As Boolean
            If String.IsNullOrWhiteSpace(Target) Then Throw New ArgumentException(DoTranslation("Can't extract nothing."))
            If String.IsNullOrWhiteSpace(Where) Then Where = RarShell_CurrentDirectory

            'Define absolute target
            Dim AbsoluteTarget As String = RarShell_CurrentArchiveDirectory + "/" + Target
            If AbsoluteTarget.StartsWith("/") Then AbsoluteTarget = AbsoluteTarget.RemoveLetter(0)
            Wdbg(DebugLevel.I, "Target: {0}, AbsoluteTarget: {1}", Target, AbsoluteTarget)

            'Define local destination while getting an entry from target
            Dim LocalDestination As String = Where + "/"
            Dim RarEntry As RarArchiveEntry = RarShell_RarArchive.Entries.Where(Function(x) x.Key = AbsoluteTarget).ToArray()(0)
            If FullTargetPath Then
                LocalDestination += RarEntry.Key
            End If
            Wdbg(DebugLevel.I, "Where: {0}", LocalDestination)

            'Try to extract file
            Directory.CreateDirectory(LocalDestination)
            MakeFile(LocalDestination + RarEntry.Key)
            RarShell_FileStream.Seek(0, SeekOrigin.Begin)
            Dim RarReader As IReader = ReaderFactory.Open(RarShell_FileStream)
            While RarReader.MoveToNextEntry()
                If RarReader.Entry.Key = RarEntry.Key And Not RarReader.Entry.IsDirectory Then
                    RarReader.WriteEntryToFile(LocalDestination + RarEntry.Key)
                End If
            End While
            Return True
        End Function

        ''' <summary>
        ''' Changes the working archive directory
        ''' </summary>
        ''' <param name="Target">Target directory</param>
        Public Function ChangeWorkingArchiveDirectory(Target As String) As Boolean
            If String.IsNullOrWhiteSpace(Target) Then Target = RarShell_CurrentArchiveDirectory

            'Check to see if we're going back
            If Target.Contains("..") Then
                Wdbg(DebugLevel.I, "Target contains going back. Counting...")
                Dim CADSplit As List(Of String) = RarShell_CurrentArchiveDirectory.Split("/").ToList
                Dim TargetSplit As List(Of String) = Target.Split("/").ToList
                Dim CADBackSteps As Integer

                'Add back steps if target is ".."
                Wdbg(DebugLevel.I, "Target length: {0}", TargetSplit.Count)
                For i As Integer = 0 To TargetSplit.Count - 1
                    Wdbg(DebugLevel.I, "Target part {0}: {1}", i, TargetSplit(i))
                    If TargetSplit(i) = ".." Then
                        Wdbg(DebugLevel.I, "Target is going back. Adding step...")
                        CADBackSteps += 1
                        TargetSplit(i) = ""
                        Wdbg(DebugLevel.I, "Steps: {0}", CADBackSteps)
                    End If
                Next

                'Remove empty strings
                TargetSplit.RemoveAll(Function(x) x = "")
                Wdbg(DebugLevel.I, "Target length: {0}", TargetSplit.Count)

                'Remove every last entry that goes back
                Wdbg(DebugLevel.I, "Old CADSplit length: {0}", CADSplit.Count)
                For Steps As Integer = CADBackSteps To 1 Step -1
                    Wdbg(DebugLevel.I, "Current step: {0}", Steps)
                    Wdbg(DebugLevel.I, "Removing index {0} from CADSplit...", CADSplit.Count - Steps)
                    CADSplit.RemoveAt(CADSplit.Count - Steps)
                    Wdbg(DebugLevel.I, "New CADSplit length: {0}", CADSplit.Count)
                Next

                'Set current archive directory and target
                RarShell_CurrentArchiveDirectory = String.Join("/", CADSplit)
                Wdbg(DebugLevel.I, "Setting CAD to {0}...", RarShell_CurrentArchiveDirectory)
                Target = String.Join("/", TargetSplit)
                Wdbg(DebugLevel.I, "Setting target to {0}...", Target)
            End If

            'Prepare the target
            Target = RarShell_CurrentArchiveDirectory + "/" + Target
            If Target.StartsWith("/") Then Target = Target.RemoveLetter(0)
            Wdbg(DebugLevel.I, "Setting target to {0}...", Target)

            'Enumerate entries
            For Each Entry As RarArchiveEntry In ListRarEntries(Target)
                Wdbg(DebugLevel.I, "Entry: {0}", Entry.Key)
                If Entry.Key.StartsWith(Target) Then
                    Wdbg(DebugLevel.I, "{0} found ({1}). Changing...", Target, Entry.Key)
                    RarShell_CurrentArchiveDirectory = Entry.Key.RemoveLetter(Entry.Key.Length - 1)
                    Wdbg(DebugLevel.I, "Setting CAD to {0}...", RarShell_CurrentArchiveDirectory)
                    Return True
                End If
            Next

            'Assume that we didn't find anything.
            Wdbg(DebugLevel.E, "{0} not found.", Target)
            Return False
        End Function

        ''' <summary>
        ''' Changes the working local directory
        ''' </summary>
        ''' <param name="Target">Target directory</param>
        Public Function ChangeWorkingRarLocalDirectory(Target As String) As Boolean
            If String.IsNullOrWhiteSpace(Target) Then Target = RarShell_CurrentDirectory
            If FolderExists(NeutralizePath(Target, RarShell_CurrentDirectory)) Then
                Wdbg(DebugLevel.I, "{0} found. Changing...", Target)
                RarShell_CurrentDirectory = Target
                Return True
            Else
                Wdbg(DebugLevel.E, "{0} not found.", Target)
                Return False
            End If
        End Function

    End Module
End Namespace

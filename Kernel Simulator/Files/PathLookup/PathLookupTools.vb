
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

Namespace Files.PathLookup
    Public Module PathLookupTools

        ''' <summary>
        ''' Gets the lookup path list
        ''' </summary>
        Public Function GetPathList() As List(Of String)
            Return PathsToLookup.Split(PathLookupDelimiter).ToList
        End Function

        ''' <summary>
        ''' Adds a (non-)neutralized path to lookup
        ''' </summary>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function AddToPathLookup(Path As String) As Boolean
            ThrowOnInvalidPath(Path)
            Dim LookupPaths As List(Of String) = GetPathList()
            Path = NeutralizePath(Path)
            LookupPaths.Add(Path)
            PathsToLookup = String.Join(PathLookupDelimiter, LookupPaths)
            Return True
        End Function

        ''' <summary>
        ''' Adds a (non-)neutralized path to lookup
        ''' </summary>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function AddToPathLookup(Path As String, RootPath As String) As Boolean
            ThrowOnInvalidPath(Path)
            ThrowOnInvalidPath(RootPath)
            Dim LookupPaths As List(Of String) = GetPathList()
            Path = NeutralizePath(Path, RootPath)
            LookupPaths.Add(Path)
            PathsToLookup = String.Join(PathLookupDelimiter, LookupPaths)
            Return True
        End Function

        ''' <summary>
        ''' Removes an existing (non-)neutralized path from lookup
        ''' </summary>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function RemoveFromPathLookup(Path As String) As Boolean
            ThrowOnInvalidPath(Path)
            Dim LookupPaths As List(Of String) = GetPathList()
            Dim Returned As Boolean
            Path = NeutralizePath(Path)
            Returned = LookupPaths.Remove(Path)
            PathsToLookup = String.Join(PathLookupDelimiter, LookupPaths)
            Return Returned
        End Function

        ''' <summary>
        ''' Removes an existing (non-)neutralized path from lookup
        ''' </summary>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function RemoveFromPathLookup(Path As String, RootPath As String) As Boolean
            ThrowOnInvalidPath(Path)
            ThrowOnInvalidPath(RootPath)
            Dim LookupPaths As List(Of String) = GetPathList()
            Dim Returned As Boolean
            Path = NeutralizePath(Path, RootPath)
            Returned = LookupPaths.Remove(Path)
            PathsToLookup = String.Join(PathLookupDelimiter, LookupPaths)
            Return Returned
        End Function

        ''' <summary>
        ''' Checks to see if the file exists in PATH and writes the result (path to file) to a string variable, if any.
        ''' </summary>
        ''' <param name="FilePath">A full path to file or just a file name</param>
        ''' <param name="Result">The neutralized path</param>
        ''' <returns>True if successful; False if unsuccessful</returns>
        Public Function FileExistsInPath(FilePath As String, ByRef Result As String) As Boolean
            ThrowOnInvalidPath(FilePath)
            Dim LookupPaths As List(Of String) = GetPathList()
            Dim ResultingPath As String
            For Each LookupPath As String In LookupPaths
                ResultingPath = NeutralizePath(FilePath, LookupPath)
                If FileExists(ResultingPath) Then
                    Result = ResultingPath
                    Return True
                End If
            Next
            Return False
        End Function

    End Module
End Namespace

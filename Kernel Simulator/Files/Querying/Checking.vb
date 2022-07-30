
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Namespace Files.Querying
    Public Module Checking

        ''' <summary>
        ''' Checks to see if the file exists. Windows 10/11 bug aware.
        ''' </summary>
        ''' <param name="File">Target file</param>
        ''' <returns>True if exists; False if not. Throws on trying to trigger the Windows 10/11 BSOD/corruption bug</returns>
        Public Function FileExists(File As String, Optional Neutralize As Boolean = False) As Boolean
            ThrowOnInvalidPath(File)
            If Neutralize Then File = NeutralizePath(File)
            Return IO.File.Exists(File)
        End Function

        ''' <summary>
        ''' Checks to see if the folder exists. Windows 10/11 bug aware.
        ''' </summary>
        ''' <param name="Folder">Target folder</param>
        ''' <returns>True if exists; False if not. Throws on trying to trigger the Windows 10/11 BSOD/corruption bug</returns>
        Public Function FolderExists(Folder As String, Optional Neutralize As Boolean = False) As Boolean
            ThrowOnInvalidPath(Folder)
            If Neutralize Then Folder = NeutralizePath(Folder)
            Return Directory.Exists(Folder)
        End Function

    End Module
End Namespace

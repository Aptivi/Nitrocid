
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

Imports Extensification.StringExts.Manipulation
Imports System.IO
Imports KS.Files.Folders
Imports KS.Files.Querying

Namespace Files
    Public Module Filesystem

        'Variables
        Public ShowFilesystemProgress As Boolean = True

        ''' <summary>
        ''' Simplifies the path to the correct one. It converts the path format to the unified format.
        ''' </summary>
        ''' <param name="Path">Target path, be it a file or a folder</param>
        ''' <returns>Absolute path</returns>
        ''' <exception cref="FileNotFoundException"></exception>
        Public Function NeutralizePath(Path As String, Optional Strict As Boolean = False) As String
            Return NeutralizePath(Path, CurrentDir, Strict)
        End Function

        ''' <summary>
        ''' Simplifies the path to the correct one. It converts the path format to the unified format.
        ''' </summary>
        ''' <param name="Path">Target path, be it a file or a folder</param>
        ''' <param name="Source">Source path in which the target is found. Must be a directory</param>
        ''' <returns>Absolute path</returns>
        ''' <exception cref="FileNotFoundException"></exception>
        Public Function NeutralizePath(Path As String, Source As String, Optional Strict As Boolean = False) As String
            If Path Is Nothing Then Path = ""
            If Source Is Nothing Then Source = ""

            ThrowOnInvalidPath(Path)
            ThrowOnInvalidPath(Source)

            'Replace backslashes with slashes if any.
            Path = Path.Replace("\", "/")
            Source = Source.Replace("\", "/")

            'Append current directory to path
            If (IsOnWindows() And Not Path.Contains(":/")) Or (IsOnUnix() And Not Path.StartsWith("/")) Then
                If Not Source.EndsWith("/") Then
                    Path = $"{Source}/{Path}"
                Else
                    Path = $"{Source}{Path}"
                End If
            End If

            'Replace last occurrences of current directory of path with nothing.
            If Not Source = "" Then
                If Path.Contains(Source) And Path.AllIndexesOf(Source).Count > 1 Then
                    Path = ReplaceLastOccurrence(Path, Source, "")
                End If
            End If

            'If strict, checks for existence of file
            If Strict Then
                If FileExists(Path) Or FolderExists(Path) Then
                    Return Path
                Else
                    Throw New FileNotFoundException(DoTranslation("Neutralized a non-existent path.") + " {0}".FormatString(Path))
                End If
            Else
                Return Path
            End If
        End Function

        ''' <summary>
        ''' Mitigates Windows 10/11 NTFS corruption/Blue Screen of Death (BSOD) bug
        ''' </summary>
        ''' <param name="Path">Target path</param>
        ''' <remarks>
        ''' - When we try to access the secret NTFS bitmap path, which contains <b>$i30</b>, from the partition root path, we'll trigger the "Your disk is corrupt." <br></br>
        ''' - When we try to access the <b>kernelconnect</b> secret device from the system partition root path, we'll trigger the BSOD. <br></br><br></br>
        ''' This sub will try to prevent access to these paths on unpatched systems and patched systems by throwing <see cref="ArgumentException"/>
        ''' </remarks>
        Public Sub ThrowOnInvalidPath(Path As String)
            If IsOnWindows() And (Path.Contains("$i30") Or Path.Contains("\\.\globalroot\device\condrv\kernelconnect")) Then
                Wdbg(DebugLevel.F, "Trying to access invalid path. Path was {0}", Path)
                Throw New ArgumentException(DoTranslation("Trying to access invalid path."), NameOf(Path))
            End If
        End Sub

    End Module
End Namespace
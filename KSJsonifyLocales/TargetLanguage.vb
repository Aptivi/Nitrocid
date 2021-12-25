
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

Public Class TargetLanguage

    ''' <summary>
    ''' The file name of the language
    ''' </summary>
    Public ReadOnly Property FileName As String
    ''' <summary>
    ''' The language name
    ''' </summary>
    Public ReadOnly Property LanguageName As String
    ''' <summary>
    ''' Chooses whether the language is custom or from the KS resources
    ''' </summary>
    Public ReadOnly Property CustomLanguage As Boolean

    ''' <summary>
    ''' Makes a new class instance of TargetLanguage
    ''' </summary>
    ''' <param name="FileName">The file name of the language</param>
    ''' <param name="LanguageName">The language name</param>
    ''' <param name="CustomLanguage">Chooses whether the language is custom or from the KS resources</param>
    Public Sub New(FileName As String, LanguageName As String, CustomLanguage As Boolean)
        Me.FileName = FileName
        Me.LanguageName = LanguageName
        Me.CustomLanguage = CustomLanguage
    End Sub

End Class

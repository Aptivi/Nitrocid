
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

Namespace Modifications
    Public Class ModInfo

        ''' <summary>
        ''' The mod name. If no name is specified, or if it only consists of whitespaces (space), the file name is taken.
        ''' </summary>
        Public ReadOnly Property ModName As String
        ''' <summary>
        ''' The mod file name
        ''' </summary>
        Public ReadOnly Property ModFileName As String
        ''' <summary>
        ''' The mod file path
        ''' </summary>
        Public ReadOnly Property ModFilePath As String
        ''' <summary>
        ''' The mod parts and their scripts
        ''' </summary>
        Friend Property ModParts As Dictionary(Of String, PartInfo)
        ''' <summary>
        ''' The mod version. We recommend using <seealso href="https://semver.org/">Semantic Versioning</seealso> scheme.
        ''' </summary>
        Public ReadOnly Property ModVersion As String

        ''' <summary>
        ''' Creates new mod info instance
        ''' </summary>
        Friend Sub New(ModName As String, ModFileName As String, ModFilePath As String, ModParts As Dictionary(Of String, PartInfo), ModVersion As String)
            'Validate values. Check to see if the name is null. If so, it will take the mod file name.
            If String.IsNullOrWhiteSpace(ModName) Then
                ModName = ModFileName
            End If

            'Check to see if the mod parts is null or zero. If so, throw exception.
            If ModParts Is Nothing OrElse ModParts.Count = 0 Then
                Throw New Exceptions.ModNoPartsException(DoTranslation("There are no parts in mod."))
            End If

            'Install values to new instance
            Me.ModName = ModName
            Me.ModFileName = ModFileName
            Me.ModFilePath = ModFilePath
            Me.ModParts = ModParts
            Me.ModVersion = ModVersion
        End Sub

    End Class
End Namespace


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

Module UserManual

    'Variables
    Public sectionsManual As Dictionary(Of String, String)

    'This is not final! This is here only for preparation for 0.0.5.9's features.
    ''' <summary>
    ''' We haven't implemented anything yet.
    ''' </summary>
    ''' <param name="section">A section on the current manual page</param>
    ''' <remarks></remarks>
    Public Sub ReadManual(ByVal section As String)
        Throw New NotImplementedException
    End Sub

    ''' <summary>
    ''' We haven't implemented anything yet.
    ''' </summary>
    ''' <param name="sectionFile">A file which is a section file</param>
    ''' <remarks></remarks>
    Public Sub ParseManual(ByVal sectionFile As String)
        Throw New NotImplementedException
    End Sub

End Module

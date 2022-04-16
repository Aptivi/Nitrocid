
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

Namespace Misc.Reflection
    Public Module StringManipulate

        ''' <summary>
        ''' Formats the string
        ''' </summary>
        ''' <param name="Format">The string to format</param>
        ''' <param name="Vars">The variables used</param>
        ''' <returns>A formatted string if successful, or the unformatted one if failed.</returns>
        Public Function FormatString(Format As String, ParamArray Vars() As Object) As String
            Dim FormattedString As String = Format
            Try
                FormattedString = String.Format(Format, Vars)
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to format string: {0}", ex.Message)
                WStkTrc(ex)
            End Try
            Return FormattedString
        End Function

    End Module
End Namespace
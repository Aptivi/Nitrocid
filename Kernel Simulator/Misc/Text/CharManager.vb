
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

Namespace Misc.Text
    Public Module CharManager

        ''' <summary>
        ''' Gets all the letters and the numbers.
        ''' </summary>
        Public Function GetAllLettersAndNumbers() As Char()
            Return Enumerable.Range(0, Convert.ToInt32(Char.MaxValue) + 1) _
                             .Select(Function(CharNum) Convert.ToChar(CharNum)) _
                             .Where(Function(c) Char.IsLetterOrDigit(c)) _
                             .ToArray()
        End Function

        ''' <summary>
        ''' Gets all the letters.
        ''' </summary>
        Public Function GetAllLetters() As Char()
            Return Enumerable.Range(0, Convert.ToInt32(Char.MaxValue) + 1) _
                             .Select(Function(CharNum) Convert.ToChar(CharNum)) _
                             .Where(Function(c) Char.IsLetter(c)) _
                             .ToArray()
        End Function

        ''' <summary>
        ''' Gets all the numbers.
        ''' </summary>
        Public Function GetAllNumbers() As Char()
            Return Enumerable.Range(0, Convert.ToInt32(Char.MaxValue) + 1) _
                             .Select(Function(CharNum) Convert.ToChar(CharNum)) _
                             .Where(Function(c) Char.IsNumber(c)) _
                             .ToArray()
        End Function

    End Module
End Namespace
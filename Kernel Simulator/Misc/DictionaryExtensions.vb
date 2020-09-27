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

Public Module DictionaryExtensions

    ''' <summary>
    ''' Gets a key from a value from the dictionary
    ''' </summary>
    ''' <typeparam name="TKey">Key</typeparam>
    ''' <typeparam name="TValue">Value</typeparam>
    ''' <param name="Dict">Source dictionary</param>
    ''' <param name="Value">Value</param>
    ''' <returns>Key from value</returns>
    <Runtime.CompilerServices.Extension>
    Public Function GetKeyFromValue(Of TKey, TValue)(ByVal Dict As Dictionary(Of TKey, TValue), ByVal Value As Object) As Object
        For Each DictKey As Object In Dict.Keys
            If Dict(DictKey).Equals(Value) Then
                Return DictKey
            End If
        Next
        Return Nothing
    End Function

End Module

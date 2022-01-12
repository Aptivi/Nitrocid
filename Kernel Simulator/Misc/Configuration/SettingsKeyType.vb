
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

''' <summary>
''' Key type for settings entry
''' </summary>
Enum SettingsKeyType
    ''' <summary>
    ''' Unknown type
    ''' </summary>
    SUnknown
    ''' <summary>
    ''' The value is of <see cref="Boolean"/>
    ''' </summary>
    SBoolean
    ''' <summary>
    ''' The value is of <see cref="Integer"/>
    ''' </summary>
    SInt
    ''' <summary>
    ''' The value is of <see cref="String"/>, but accepts less than or equal to 255 characters
    ''' </summary>
    SString
    ''' <summary>
    ''' The value is of <see cref="String"/>, but accepts more than 255 characters
    ''' </summary>
    SLongString
    ''' <summary>
    ''' The value is of the selection, which can either come from enums, or from <see cref="IEnumerable"/>, like <see cref="Generic.List(Of T)"/>
    ''' </summary>
    SSelection
    ''' <summary>
    ''' The value is of <see cref="IEnumerable"/>, like <see cref="Generic.List(Of T)"/>
    ''' </summary>
    SList
    ''' <summary>
    ''' The value is variant and comes from a function
    ''' </summary>
    SVariant
    ''' <summary>
    ''' The value is of <see cref="Color"/> and comes from the color wheel
    ''' </summary>
    SColor
    ''' <summary>
    ''' The value is of <see cref="String"/>, but masked. Useful for passwords.
    ''' </summary>
    SMaskedString
End Enum
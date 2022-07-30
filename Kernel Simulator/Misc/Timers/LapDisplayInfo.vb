
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

Namespace Misc.Timers
    Class LapDisplayInfo

        ''' <summary>
        ''' The lap color
        ''' </summary>
        Public ReadOnly LapColor As Color
        ''' <summary>
        ''' The lap interval
        ''' </summary>
        Public ReadOnly LapInterval As TimeSpan

        Sub New(LapColor As Color, LapInterval As TimeSpan)
            Me.LapColor = LapColor
            Me.LapInterval = LapInterval
        End Sub

    End Class
End Namespace

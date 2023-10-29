﻿
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

Namespace Scripting
    ''' <summary>
    ''' The list of known UESH conditions
    ''' </summary>
    Public Enum UESHConditions
        ''' <summary>
        ''' No condition
        ''' </summary>
        none = 0
        ''' <summary>
        ''' Equals
        ''' </summary>
        eq
        ''' <summary>
        ''' Doesn't equal
        ''' </summary>
        neq
        ''' <summary>
        ''' Less than
        ''' </summary>
        les
        ''' <summary>
        ''' Greater than
        ''' </summary>
        gre
        ''' <summary>
        ''' Less than or equals to
        ''' </summary>
        lesoreq
        ''' <summary>
        ''' Greater than or equals to
        ''' </summary>
        greoreq
        ''' <summary>
        ''' File exists
        ''' </summary>
        fileex
        ''' <summary>
        ''' File doesn't exist
        ''' </summary>
        filenex
        ''' <summary>
        ''' Directory exists
        ''' </summary>
        direx
        ''' <summary>
        ''' Directory doesn't exist
        ''' </summary>
        dirnex
    End Enum
End Namespace
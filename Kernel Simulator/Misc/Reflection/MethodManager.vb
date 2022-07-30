
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

Imports System.Reflection

Namespace Misc.Reflection
    Public Module MethodManager

        ''' <summary>
        ''' Gets a method from method name
        ''' </summary>
        ''' <param name="Method">Method name. Use operator NameOf to get name.</param>
        ''' <returns>Method information</returns>
        Public Function GetMethod(Method As String) As MethodBase
            Dim PossibleTypes As Type()
            Dim PossibleMethod As MethodInfo

            'Get types of possible flag locations
            PossibleTypes = Assembly.GetExecutingAssembly.GetTypes

            'Get fields of flag modules
            For Each PossibleType As Type In PossibleTypes
                PossibleMethod = PossibleType.GetMethod(Method)
                If PossibleMethod IsNot Nothing Then Return PossibleMethod
            Next
            Return Nothing
        End Function

    End Module
End Namespace
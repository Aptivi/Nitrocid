
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

Imports KS.Misc.Reflection

Namespace Kernel.Exceptions
    ''' <summary>
    ''' Thrown when alias already exists
    ''' </summary>
    Public Class AliasAlreadyExistsException
        Inherits Exception

        Public Sub New()
            MyBase.New()
        End Sub
        Public Sub New(message As String)
            MyBase.New(message)
        End Sub
        Public Sub New(message As String, ParamArray vars() As Object)
            MyBase.New(FormatString(message, vars))
        End Sub
        Public Sub New(message As String, e As Exception)
            MyBase.New(message, e)
        End Sub
        Public Sub New(message As String, e As Exception, ParamArray vars() As Object)
            MyBase.New(FormatString(message, vars), e)
        End Sub

    End Class
End Namespace
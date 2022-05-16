
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

Imports Newtonsoft.Json.Linq

Namespace Kernel
    Public Class KernelUpdateInfo

        ''' <summary>
        ''' Updated kernel version
        ''' </summary>
        Public ReadOnly Property UpdateVersion As Version
        ''' <summary>
        ''' Update file URL
        ''' </summary>
        Public ReadOnly Property UpdateURL As Uri

        ''' <summary>
        ''' Installs a new instance of class KernelUpdateInfo
        ''' </summary>
        ''' <param name="UpdateVer">The kernel version fetched from the update token</param>
        ''' <param name="UpdateUrl">The kernel URL fetched from the update token</param>
        Protected Friend Sub New(UpdateVer As Version, UpdateUrl As String)
            Try
                UpdateVersion = UpdateVer
                Me.UpdateURL = New Uri(UpdateUrl)
                Wdbg(DebugLevel.I, "Added new update {0} with {1} as URI", UpdateVer.ToString, UpdateUrl)
            Catch ex As Exception
                Wdbg(DebugLevel.E, "Failed to create new instance of update class with update {0} with {1} as URI: {2}", UpdateVer.ToString, UpdateUrl, ex.Message)
                WStkTrc(ex)
            End Try
        End Sub

    End Class
End Namespace
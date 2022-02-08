
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
    Public Class KernelUpdate

        ''' <summary>
        ''' Updated kernel version
        ''' </summary>
        Public ReadOnly Property UpdateVersion As Version
        ''' <summary>
        ''' Update file URL
        ''' </summary>
        Public ReadOnly Property UpdateURL As Uri
        ''' <summary>
        ''' Is the kernel up to date?
        ''' </summary>
        Public ReadOnly Property Updated As Boolean

        ''' <summary>
        ''' Installs a new instance of class KernelUpdate
        ''' </summary>
        ''' <param name="UpdateToken">The kernel update token</param>
        Protected Friend Sub New(UpdateToken As JToken)
            Dim UpdateVer As New Version(UpdateToken.First.SelectToken("tag_name").ToString.ReplaceAll({"v", "-alpha", "-beta"}, ""))
            Dim CurrentVer As New Version(KernelVersion)
            Dim UpdateURL As String = UpdateToken.First.SelectToken("html_url")
            Dim UpdateURI As New Uri(UpdateURL)
            Wdbg(DebugLevel.I, "Update version: {0}", UpdateVer.ToString)
            Wdbg(DebugLevel.I, "Update URL: {0}", UpdateURL)

            'Install the values
            UpdateVersion = UpdateVer
            Me.UpdateURL = UpdateURI
            Updated = UpdateVersion > CurrentVer
        End Sub

    End Class
End Namespace
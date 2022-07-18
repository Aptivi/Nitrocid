
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

Namespace Misc.Screensaver.Customized
    Public Class CustomSaverInfo

        ''' <summary>
        ''' Name of the screensaver
        ''' </summary>
        Public ReadOnly Property SaverName As String
        ''' <summary>
        ''' File name of the screensaver
        ''' </summary>
        Public ReadOnly Property FileName As String
        ''' <summary>
        ''' File path of the screensaver
        ''' </summary>
        Public ReadOnly Property FilePath As String
        ''' <summary>
        ''' The screensaver base code
        ''' </summary>
        Public ReadOnly Property ScreensaverBase As BaseScreensaver

        ''' <summary>
        ''' Creates new screensaver info instance
        ''' </summary>
        Friend Sub New(SaverName As String, FileName As String, FilePath As String, ScreensaverBase As BaseScreensaver)
            Me.SaverName = If(String.IsNullOrWhiteSpace(SaverName), FileName, SaverName)
            Me.FileName = FileName
            Me.FilePath = FilePath
            Me.ScreensaverBase = ScreensaverBase
        End Sub

    End Class
End Namespace


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

Namespace Misc.Splash
    Public Class SplashInfo

        ''' <summary>
        ''' Splash name
        ''' </summary>
        Public ReadOnly Property SplashName As String
        ''' <summary>
        ''' Does the splash display progress?
        ''' </summary>
        Public ReadOnly Property DisplaysProgress As Boolean
        ''' <summary>
        ''' Splash entry point
        ''' </summary>
        Public ReadOnly Property EntryPoint As ISplash

        ''' <summary>
        ''' Installs a new class instance of splash info
        ''' </summary>
        ''' <param name="SplashName">Splash name</param>
        ''' <param name="DisplaysProgress">Does the splash display progress?</param>
        ''' <param name="EntryPoint">Splash entry point</param>
        Protected Friend Sub New(SplashName As String, EntryPoint As ISplash, Optional DisplaysProgress As Boolean = True)
            Me.SplashName = SplashName
            Me.DisplaysProgress = DisplaysProgress
            Me.EntryPoint = EntryPoint
        End Sub

    End Class
End Namespace

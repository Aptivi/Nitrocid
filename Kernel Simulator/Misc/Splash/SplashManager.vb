
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

Imports System.Threading

Public Module SplashManager

    Public ReadOnly Splashes As New Dictionary(Of String, SplashInfo) From {{"Simple", New SplashInfo("Simple", True, 3, 1, 9, 1, New SplashSimple)},
                                                                            {"Blank", New SplashInfo("Blank", False, 0, 0, 0, 0, New SplashBlank)}}
    Public SplashName As String = "Simple"

    ''' <summary>
    ''' Current splash screen
    ''' </summary>
    Public ReadOnly Property CurrentSplash As ISplash
        Get
            If Splashes.ContainsKey(SplashName) Then
                Return Splashes(SplashName).EntryPoint
            Else
                Return Splashes("Simple").EntryPoint
            End If
        End Get
    End Property

    ''' <summary>
    ''' Current splash screen info instance
    ''' </summary>
    Public ReadOnly Property CurrentSplashInfo As SplashInfo
        Get
            If Splashes.ContainsKey(SplashName) Then
                Return Splashes(SplashName)
            Else
                Return Splashes("Simple")
            End If
        End Get
    End Property

    Friend SplashThread As New Thread(Sub() CurrentSplash.Display())

    ''' <summary>
    ''' Opens the splash screen
    ''' </summary>
    Sub OpenSplash()
        If EnableSplash Then
            Console.CursorVisible = False
            CurrentSplash.Opening()
            If Not SplashThread.IsAlive Then SplashThread.Start()
        End If
    End Sub

    ''' <summary>
    ''' Closes the splash screen
    ''' </summary>
    Sub CloseSplash()
        If EnableSplash Then
            CurrentSplash.Closing()
            SplashThread = New Thread(Sub() CurrentSplash.Display())
            Console.CursorVisible = True
        End If
        _KernelBooted = True
    End Sub

End Module

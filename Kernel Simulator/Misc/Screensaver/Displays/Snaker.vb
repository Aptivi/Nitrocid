
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

Imports KS.Misc.Games

Namespace Misc.Screensaver.Displays
    Public Module SnakerSettings

        Private _snaker255Colors As Boolean
        Private _snakerTrueColor As Boolean = True
        Private _snakerDelay As Integer = 100
        Private _snakerStageDelay As Integer = 5000
        Private _snakerMinimumRedColorLevel As Integer = 0
        Private _snakerMinimumGreenColorLevel As Integer = 0
        Private _snakerMinimumBlueColorLevel As Integer = 0
        Private _snakerMinimumColorLevel As Integer = 0
        Private _snakerMaximumRedColorLevel As Integer = 255
        Private _snakerMaximumGreenColorLevel As Integer = 255
        Private _snakerMaximumBlueColorLevel As Integer = 255
        Private _snakerMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [Snaker] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property Snaker255Colors As Boolean
            Get
                Return _snaker255Colors
            End Get
            Set(value As Boolean)
                _snaker255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property SnakerTrueColor As Boolean
            Get
                Return _snakerTrueColor
            End Get
            Set(value As Boolean)
                _snakerTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property SnakerDelay As Integer
            Get
                Return _snakerDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 100
                _snakerDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] How many milliseconds to wait before making the next stage?
        ''' </summary>
        Public Property SnakerStageDelay As Integer
            Get
                Return _snakerStageDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 5000
                _snakerStageDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] The minimum red color level (true color)
        ''' </summary>
        Public Property SnakerMinimumRedColorLevel As Integer
            Get
                Return _snakerMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _snakerMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] The minimum green color level (true color)
        ''' </summary>
        Public Property SnakerMinimumGreenColorLevel As Integer
            Get
                Return _snakerMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _snakerMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] The minimum blue color level (true color)
        ''' </summary>
        Public Property SnakerMinimumBlueColorLevel As Integer
            Get
                Return _snakerMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _snakerMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property SnakerMinimumColorLevel As Integer
            Get
                Return _snakerMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_snaker255Colors Or _snakerTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _snakerMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] The maximum red color level (true color)
        ''' </summary>
        Public Property SnakerMaximumRedColorLevel As Integer
            Get
                Return _snakerMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _snakerMinimumRedColorLevel Then value = _snakerMinimumRedColorLevel
                If value > 255 Then value = 255
                _snakerMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] The maximum green color level (true color)
        ''' </summary>
        Public Property SnakerMaximumGreenColorLevel As Integer
            Get
                Return _snakerMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _snakerMinimumGreenColorLevel Then value = _snakerMinimumGreenColorLevel
                If value > 255 Then value = 255
                _snakerMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] The maximum blue color level (true color)
        ''' </summary>
        Public Property SnakerMaximumBlueColorLevel As Integer
            Get
                Return _snakerMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _snakerMinimumBlueColorLevel Then value = _snakerMinimumBlueColorLevel
                If value > 255 Then value = 255
                _snakerMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [Snaker] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property SnakerMaximumColorLevel As Integer
            Get
                Return _snakerMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_snaker255Colors Or _snakerTrueColor, 255, 15)
                If value <= _snakerMinimumColorLevel Then value = _snakerMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _snakerMaximumColorLevel = value
            End Set
        End Property

    End Module

    Public Class SnakerDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Public Overrides Property ScreensaverName As String = "Snaker" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            InitializeSnaker(True)
            SleepNoBlock(SnakerDelay, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace

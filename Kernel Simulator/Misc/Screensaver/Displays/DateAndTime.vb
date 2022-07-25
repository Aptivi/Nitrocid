
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

Imports KS.ConsoleBase.Colors
Imports KS.TimeDate

Namespace Misc.Screensaver.Displays
    Public Module DateAndTimeSettings

        Private _dateAndTime255Colors As Boolean
        Private _dateAndTimeTrueColor As Boolean = True
        Private _dateAndTimeDelay As Integer = 1000
        Private _dateAndTimeMinimumRedColorLevel As Integer = 0
        Private _dateAndTimeMinimumGreenColorLevel As Integer = 0
        Private _dateAndTimeMinimumBlueColorLevel As Integer = 0
        Private _dateAndTimeMinimumColorLevel As Integer = 0
        Private _dateAndTimeMaximumRedColorLevel As Integer = 255
        Private _dateAndTimeMaximumGreenColorLevel As Integer = 255
        Private _dateAndTimeMaximumBlueColorLevel As Integer = 255
        Private _dateAndTimeMaximumColorLevel As Integer = 255

        ''' <summary>
        ''' [DateAndTime] Enable 255 color support. Has a higher priority than 16 color support.
        ''' </summary>
        Public Property DateAndTime255Colors As Boolean
            Get
                Return _dateAndTime255Colors
            End Get
            Set(value As Boolean)
                _dateAndTime255Colors = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] Enable truecolor support. Has a higher priority than 255 color support.
        ''' </summary>
        Public Property DateAndTimeTrueColor As Boolean
            Get
                Return _dateAndTimeTrueColor
            End Get
            Set(value As Boolean)
                _dateAndTimeTrueColor = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] How many milliseconds to wait before making the next write?
        ''' </summary>
        Public Property DateAndTimeDelay As Integer
            Get
                Return _dateAndTimeDelay
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 1000
                _dateAndTimeDelay = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] The minimum red color level (true color)
        ''' </summary>
        Public Property DateAndTimeMinimumRedColorLevel As Integer
            Get
                Return _dateAndTimeMinimumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _dateAndTimeMinimumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] The minimum green color level (true color)
        ''' </summary>
        Public Property DateAndTimeMinimumGreenColorLevel As Integer
            Get
                Return _dateAndTimeMinimumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _dateAndTimeMinimumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] The minimum blue color level (true color)
        ''' </summary>
        Public Property DateAndTimeMinimumBlueColorLevel As Integer
            Get
                Return _dateAndTimeMinimumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= 0 Then value = 0
                If value > 255 Then value = 255
                _dateAndTimeMinimumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] The minimum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property DateAndTimeMinimumColorLevel As Integer
            Get
                Return _dateAndTimeMinimumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMinimumLevel As Integer = If(_dateAndTime255Colors Or _dateAndTimeTrueColor, 255, 15)
                If value <= 0 Then value = 0
                If value > FinalMinimumLevel Then value = FinalMinimumLevel
                _dateAndTimeMinimumColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] The maximum red color level (true color)
        ''' </summary>
        Public Property DateAndTimeMaximumRedColorLevel As Integer
            Get
                Return _dateAndTimeMaximumRedColorLevel
            End Get
            Set(value As Integer)
                If value <= _dateAndTimeMinimumRedColorLevel Then value = _dateAndTimeMinimumRedColorLevel
                If value > 255 Then value = 255
                _dateAndTimeMaximumRedColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] The maximum green color level (true color)
        ''' </summary>
        Public Property DateAndTimeMaximumGreenColorLevel As Integer
            Get
                Return _dateAndTimeMaximumGreenColorLevel
            End Get
            Set(value As Integer)
                If value <= _dateAndTimeMinimumGreenColorLevel Then value = _dateAndTimeMinimumGreenColorLevel
                If value > 255 Then value = 255
                _dateAndTimeMaximumGreenColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] The maximum blue color level (true color)
        ''' </summary>
        Public Property DateAndTimeMaximumBlueColorLevel As Integer
            Get
                Return _dateAndTimeMaximumBlueColorLevel
            End Get
            Set(value As Integer)
                If value <= _dateAndTimeMinimumBlueColorLevel Then value = _dateAndTimeMinimumBlueColorLevel
                If value > 255 Then value = 255
                _dateAndTimeMaximumBlueColorLevel = value
            End Set
        End Property
        ''' <summary>
        ''' [DateAndTime] The maximum color level (255 colors or 16 colors)
        ''' </summary>
        Public Property DateAndTimeMaximumColorLevel As Integer
            Get
                Return _dateAndTimeMaximumColorLevel
            End Get
            Set(value As Integer)
                Dim FinalMaximumLevel As Integer = If(_dateAndTime255Colors Or _dateAndTimeTrueColor, 255, 15)
                If value <= _dateAndTimeMinimumColorLevel Then value = _dateAndTimeMinimumColorLevel
                If value > FinalMaximumLevel Then value = FinalMaximumLevel
                _dateAndTimeMaximumColorLevel = value
            End Set
        End Property

    End Module

    Public Class DateAndTimeDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random

        Public Overrides Property ScreensaverName As String = "DateAndTime" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            Console.BackgroundColor = ConsoleColor.Black
            Console.Clear()
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Console.CursorVisible = False
            Console.Clear()

            'Write date and time
            SetConsoleColor(ChangeDateAndTimeColor)
            WriteWherePlain(RenderDate, Console.WindowWidth / 2 - RenderDate.Length / 2, Console.WindowHeight / 2 - 1, False)
            WriteWherePlain(RenderTime, Console.WindowWidth / 2 - RenderTime.Length / 2, Console.WindowHeight / 2, False)

            'Delay
            SleepNoBlock(DateAndTimeDelay, ScreensaverDisplayerThread)
        End Sub

        ''' <summary>
        ''' Changes the color of date and time
        ''' </summary>
        Function ChangeDateAndTimeColor() As Color
            Dim ColorInstance As Color
            If DateAndTimeTrueColor Then
                Dim RedColorNum As Integer = RandomDriver.Next(DateAndTimeMinimumRedColorLevel, DateAndTimeMaximumRedColorLevel)
                Dim GreenColorNum As Integer = RandomDriver.Next(DateAndTimeMinimumGreenColorLevel, DateAndTimeMaximumGreenColorLevel)
                Dim BlueColorNum As Integer = RandomDriver.Next(DateAndTimeMinimumBlueColorLevel, DateAndTimeMaximumBlueColorLevel)
                ColorInstance = New Color(RedColorNum, GreenColorNum, BlueColorNum)
            ElseIf DateAndTime255Colors Then
                Dim ColorNum As Integer = RandomDriver.Next(DateAndTimeMinimumColorLevel, DateAndTimeMaximumColorLevel)
                ColorInstance = New Color(ColorNum)
            Else
                ColorInstance = New Color(colors(RandomDriver.Next(DateAndTimeMinimumColorLevel, DateAndTimeMaximumColorLevel)))
            End If
            Return ColorInstance
        End Function

    End Class
End Namespace

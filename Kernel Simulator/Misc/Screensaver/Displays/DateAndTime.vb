
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

Imports KS.TimeDate
Imports System.Threading

Namespace Misc.Screensaver.Displays
    Module DateAndTimeDisplay

        Public DateAndTime As New KernelThread("DateAndTime screensaver thread", True, AddressOf DateAndTime_DoWork)

        ''' <summary>
        ''' Handles the code of Bouncing Text
        ''' </summary>
        Sub DateAndTime_DoWork()
            Try
                'Preparations
                Console.BackgroundColor = ConsoleColor.Black
                Console.Clear()

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False
                    Console.Clear()

                    'Write date and time
                    SetConsoleColor(ChangeDateAndTimeColor)
                    WriteWherePlain(RenderDate, Console.WindowWidth / 2 - RenderDate.Length / 2, Console.WindowHeight / 2 - 1, False)
                    WriteWherePlain(RenderTime, Console.WindowWidth / 2 - RenderTime.Length / 2, Console.WindowHeight / 2, False)

                    'Delay
                    SleepNoBlock(DateAndTimeDelay, DateAndTime)
                Loop
            Catch taex As ThreadAbortException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Changes the color of bouncing text
        ''' </summary>
        Function ChangeDateAndTimeColor() As Color
            Dim RandomDriver As New Random
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

    End Module
End Namespace

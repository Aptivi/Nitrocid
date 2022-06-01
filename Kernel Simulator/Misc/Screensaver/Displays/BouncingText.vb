
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

Namespace Misc.Screensaver.Displays
    Module BouncingTextDisplay

        Public BouncingText As New KernelThread("BouncingText screensaver thread", True, AddressOf BouncingText_DoWork)

        ''' <summary>
        ''' Handles the code of Bouncing Text
        ''' </summary>
        Sub BouncingText_DoWork()
            Try
                'Variables
                Dim Direction As String = "BottomRight"
                Dim RowText, ColumnFirstLetter, ColumnLastLetter As Integer
                Dim CurrentWindowWidth As Integer = Console.WindowWidth
                Dim CurrentWindowHeight As Integer = Console.WindowHeight
                Dim ResizeSyncing As Boolean
                Dim BouncingColor As Color

                'Preparations
                SetConsoleColor(New Color(BouncingTextBackgroundColor), True)
                SetConsoleColor(New Color(BouncingTextForegroundColor))
                Console.Clear()
                RowText = Console.WindowHeight / 2
                ColumnFirstLetter = (Console.WindowWidth / 2) - BouncingTextWrite.Length / 2
                ColumnLastLetter = (Console.WindowWidth / 2) + BouncingTextWrite.Length / 2

                'Sanity checks for color levels
                If BouncingTextTrueColor Or BouncingText255Colors Then
                    BouncingTextMinimumRedColorLevel = If(BouncingTextMinimumRedColorLevel >= 0 And BouncingTextMinimumRedColorLevel <= 255, BouncingTextMinimumRedColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", BouncingTextMinimumRedColorLevel)
                    BouncingTextMinimumGreenColorLevel = If(BouncingTextMinimumGreenColorLevel >= 0 And BouncingTextMinimumGreenColorLevel <= 255, BouncingTextMinimumGreenColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", BouncingTextMinimumGreenColorLevel)
                    BouncingTextMinimumBlueColorLevel = If(BouncingTextMinimumBlueColorLevel >= 0 And BouncingTextMinimumBlueColorLevel <= 255, BouncingTextMinimumBlueColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", BouncingTextMinimumBlueColorLevel)
                    BouncingTextMinimumColorLevel = If(BouncingTextMinimumColorLevel >= 0 And BouncingTextMinimumColorLevel <= 255, BouncingTextMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", BouncingTextMinimumColorLevel)
                    BouncingTextMaximumRedColorLevel = If(BouncingTextMaximumRedColorLevel >= 0 And BouncingTextMaximumRedColorLevel <= 255, BouncingTextMaximumRedColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", BouncingTextMaximumRedColorLevel)
                    BouncingTextMaximumGreenColorLevel = If(BouncingTextMaximumGreenColorLevel >= 0 And BouncingTextMaximumGreenColorLevel <= 255, BouncingTextMaximumGreenColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", BouncingTextMaximumGreenColorLevel)
                    BouncingTextMaximumBlueColorLevel = If(BouncingTextMaximumBlueColorLevel >= 0 And BouncingTextMaximumBlueColorLevel <= 255, BouncingTextMaximumBlueColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", BouncingTextMaximumBlueColorLevel)
                    BouncingTextMaximumColorLevel = If(BouncingTextMaximumColorLevel >= 0 And BouncingTextMaximumColorLevel <= 255, BouncingTextMaximumColorLevel, 255)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", BouncingTextMaximumColorLevel)
                Else
                    BouncingTextMinimumColorLevel = If(BouncingTextMinimumColorLevel >= 0 And BouncingTextMinimumColorLevel <= 15, BouncingTextMinimumColorLevel, 0)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", BouncingTextMinimumColorLevel)
                    BouncingTextMaximumColorLevel = If(BouncingTextMaximumColorLevel >= 0 And BouncingTextMaximumColorLevel <= 15, BouncingTextMaximumColorLevel, 15)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", BouncingTextMaximumColorLevel)
                End If

                'Screensaver logic
                Do While True
                    Console.CursorVisible = False
                    Console.Clear()

#Disable Warning BC42104
                    'Define the color
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Row text: {0}", RowText)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Column first letter of text: {0}", ColumnFirstLetter)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Column last letter of text: {0}", ColumnLastLetter)
                    If BouncingColor Is Nothing Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Defining color...")
                        BouncingColor = ChangeBouncingTextColor()
                    End If
                    If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                    If Not ResizeSyncing Then
                        WriteWhere(BouncingTextWrite, ColumnFirstLetter, RowText, True, BouncingColor)
                    Else
                        WdbgConditional(ScreensaverDebug, DebugLevel.W, "We're resize-syncing! Setting RowText, ColumnFirstLetter, and ColumnLastLetter to its original position...")
                        RowText = Console.WindowHeight / 2
                        ColumnFirstLetter = (Console.WindowWidth / 2) - BouncingTextWrite.Length / 2
                        ColumnLastLetter = (Console.WindowWidth / 2) + BouncingTextWrite.Length / 2
                    End If
#Enable Warning BC42104

                    'Change the direction of text
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Text is facing {0}.", Direction)
                    If Direction = "BottomRight" Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Increasing row and column text position")
                        RowText += 1
                        ColumnFirstLetter += 1
                        ColumnLastLetter += 1
                    ElseIf Direction = "BottomLeft" Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Increasing row and decreasing column text position")
                        RowText += 1
                        ColumnFirstLetter -= 1
                        ColumnLastLetter -= 1
                    ElseIf Direction = "TopRight" Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Decreasing row and increasing column text position")
                        RowText -= 1
                        ColumnFirstLetter += 1
                        ColumnLastLetter += 1
                    ElseIf Direction = "TopLeft" Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Decreasing row and column text position")
                        RowText -= 1
                        ColumnFirstLetter -= 1
                        ColumnLastLetter -= 1
                    End If

                    'Check to see if the text is on the edge
                    If RowText = Console.WindowHeight - 2 Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're on the bottom.")
                        Direction = Direction.Replace("Bottom", "Top")
                        BouncingColor = ChangeBouncingTextColor()
                    ElseIf RowText = 1 Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're on the top.")
                        Direction = Direction.Replace("Top", "Bottom")
                        BouncingColor = ChangeBouncingTextColor()
                    End If

                    If ColumnLastLetter = Console.WindowWidth - 1 Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're on the right.")
                        Direction = Direction.Replace("Right", "Left")
                        BouncingColor = ChangeBouncingTextColor()
                    ElseIf ColumnFirstLetter = 1 Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "We're on the left.")
                        Direction = Direction.Replace("Left", "Right")
                        BouncingColor = ChangeBouncingTextColor()
                    End If

                    'Reset resize sync
                    ResizeSyncing = False
                    CurrentWindowWidth = Console.WindowWidth
                    CurrentWindowHeight = Console.WindowHeight
                    SleepNoBlock(BouncingTextDelay, BouncingText)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

        ''' <summary>
        ''' Changes the color of bouncing text
        ''' </summary>
        Function ChangeBouncingTextColor() As Color
            Dim RandomDriver As New Random
            Dim ColorInstance As Color
            If BouncingTextTrueColor Then
                Dim RedColorNum As Integer = RandomDriver.Next(BouncingTextMinimumRedColorLevel, BouncingTextMaximumRedColorLevel)
                Dim GreenColorNum As Integer = RandomDriver.Next(BouncingTextMinimumGreenColorLevel, BouncingTextMaximumGreenColorLevel)
                Dim BlueColorNum As Integer = RandomDriver.Next(BouncingTextMinimumBlueColorLevel, BouncingTextMaximumBlueColorLevel)
                ColorInstance = New Color(RedColorNum, GreenColorNum, BlueColorNum)
            ElseIf BouncingText255Colors Then
                Dim ColorNum As Integer = RandomDriver.Next(BouncingTextMinimumColorLevel, BouncingTextMaximumColorLevel)
                ColorInstance = New Color(ColorNum)
            Else
                ColorInstance = New Color(colors(RandomDriver.Next(BouncingTextMinimumColorLevel, BouncingTextMaximumColorLevel)))
            End If
            Return ColorInstance
        End Function

    End Module
End Namespace

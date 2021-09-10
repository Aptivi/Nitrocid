
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Imports System.ComponentModel

Module BouncingTextDisplay

    Public WithEvents BouncingText As New BackgroundWorker With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of Bouncing Text
    ''' </summary>
    Sub BouncingText_DoWork(sender As Object, e As DoWorkEventArgs) Handles BouncingText.DoWork
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()
        Console.CursorVisible = False
        Try
            Dim Direction As String = "BottomRight"
            Dim RowText, ColumnFirstLetter, ColumnLastLetter As Integer
            RowText = Console.WindowHeight / 2
            ColumnFirstLetter = (Console.WindowWidth / 2) - BouncingTextWrite.Length / 2
            ColumnLastLetter = (Console.WindowWidth / 2) + BouncingTextWrite.Length / 2
            Do While True
                SleepNoBlock(BouncingTextDelay, BouncingText)
                Console.Clear()
                If BouncingText.CancellationPending = True Then
                    Wdbg(DebugLevel.W, "Cancellation is pending. Cleaning everything up...")
                    e.Cancel = True
                    SetInputColor()
                    LoadBack()
                    Console.CursorVisible = True
                    Wdbg(DebugLevel.I, "All clean. Bouncing Text screensaver stopped.")
                    SaverAutoReset.Set()
                    Exit Do
                Else
                    'Define the color
#Disable Warning BC42104
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Row text: {0}", RowText)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Column first letter of text: {0}", ColumnFirstLetter)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Column last letter of text: {0}", ColumnLastLetter)
                    Dim BouncingColor As Color
                    If BouncingColor Is Nothing Then
                        WdbgConditional(ScreensaverDebug, DebugLevel.I, "Defining color...")
                        BouncingColor = ChangeBouncingTextColor()
                    End If
                    WriteWhereC(BouncingTextWrite, ColumnFirstLetter, RowText, True, BouncingColor)
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
                End If
            Loop
        Catch ex As Exception
            Wdbg(DebugLevel.W, "Screensaver experienced an error: {0}. Cleaning everything up...", ex.Message)
            WStkTrc(ex)
            e.Cancel = True
            SetInputColor()
            LoadBack()
            Console.CursorVisible = True
            Wdbg(DebugLevel.I, "All clean. Bouncing Text screensaver stopped.")
            W(DoTranslation("Screensaver experienced an error while displaying: {0}. Press any key to exit."), True, ColTypes.Error, ex.Message)
            SaverAutoReset.Set()
        End Try
    End Sub

    ''' <summary>
    ''' Changes the color of bouncing text
    ''' </summary>
    Function ChangeBouncingTextColor() As Color
        Dim RandomDriver As New Random
        Dim ColorInstance As Color
        If BouncingTextTrueColor Then
            Dim RedColorNum As Integer = RandomDriver.Next(1, 255)
            Dim GreenColorNum As Integer = RandomDriver.Next(1, 255)
            Dim BlueColorNum As Integer = RandomDriver.Next(1, 255)
            Dim ColorStorage As New RGB(RedColorNum, GreenColorNum, BlueColorNum)
            ColorInstance = New Color(ColorStorage.ToString)
        ElseIf BouncingText255Colors Then
            Dim ColorNum As Integer = RandomDriver.Next(1, 255)
            ColorInstance = New Color(ColorNum)
        Else
            ColorInstance = New Color(colors(RandomDriver.Next(1, colors.Length - 1)))
        End If
        Return ColorInstance
    End Function

End Module

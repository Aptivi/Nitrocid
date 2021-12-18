
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
Imports Figgle

Module FigletDisplay

    Public WithEvents Figlet As New NamedBackgroundWorker("Figlet screensaver thread") With {.WorkerSupportsCancellation = True}

    ''' <summary>
    ''' Handles the code of Figlet
    ''' </summary>
    Sub Figlet_DoWork(sender As Object, e As DoWorkEventArgs) Handles Figlet.DoWork
        'Variables
        Dim Randomizer As New Random
        Dim ConsoleMiddleWidth As Integer = Console.WindowWidth / 2
        Dim ConsoleMiddleHeight As Integer = Console.WindowHeight / 2
        Dim FigletFonts As Dictionary(Of String, Object) = GetProperties(GetType(FiggleFonts))
        Dim FigletFontUsed As FiggleFont = FigletFonts(FigletFont)
        Dim CurrentWindowWidth As Integer = Console.WindowWidth
        Dim CurrentWindowHeight As Integer = Console.WindowHeight
        Dim ResizeSyncing As Boolean

        'Preparations
        Console.BackgroundColor = ConsoleColor.Black
        Console.ForegroundColor = ConsoleColor.White
        Console.Clear()

        'Sanity checks for color levels
        If FigletTrueColor Or Figlet255Colors Then
            FigletMinimumRedColorLevel = If(FigletMinimumRedColorLevel >= 0 And FigletMinimumRedColorLevel <= 255, FigletMinimumRedColorLevel, 0)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum red color level: {0}", FigletMinimumRedColorLevel)
            FigletMinimumGreenColorLevel = If(FigletMinimumGreenColorLevel >= 0 And FigletMinimumGreenColorLevel <= 255, FigletMinimumGreenColorLevel, 0)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum green color level: {0}", FigletMinimumGreenColorLevel)
            FigletMinimumBlueColorLevel = If(FigletMinimumBlueColorLevel >= 0 And FigletMinimumBlueColorLevel <= 255, FigletMinimumBlueColorLevel, 0)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum blue color level: {0}", FigletMinimumBlueColorLevel)
            FigletMinimumColorLevel = If(FigletMinimumColorLevel >= 0 And FigletMinimumColorLevel <= 255, FigletMinimumColorLevel, 0)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", FigletMinimumColorLevel)
            FigletMaximumRedColorLevel = If(FigletMaximumRedColorLevel >= 0 And FigletMaximumRedColorLevel <= 255, FigletMaximumRedColorLevel, 255)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum red color level: {0}", FigletMaximumRedColorLevel)
            FigletMaximumGreenColorLevel = If(FigletMaximumGreenColorLevel >= 0 And FigletMaximumGreenColorLevel <= 255, FigletMaximumGreenColorLevel, 255)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum green color level: {0}", FigletMaximumGreenColorLevel)
            FigletMaximumBlueColorLevel = If(FigletMaximumBlueColorLevel >= 0 And FigletMaximumBlueColorLevel <= 255, FigletMaximumBlueColorLevel, 255)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum blue color level: {0}", FigletMaximumBlueColorLevel)
            FigletMaximumColorLevel = If(FigletMaximumColorLevel >= 0 And FigletMaximumColorLevel <= 255, FigletMaximumColorLevel, 255)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", FigletMaximumColorLevel)
        Else
            FigletMinimumColorLevel = If(FigletMinimumColorLevel >= 0 And FigletMinimumColorLevel <= 16, FigletMinimumColorLevel, 0)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Minimum color level: {0}", FigletMinimumColorLevel)
            FigletMaximumColorLevel = If(FigletMaximumColorLevel >= 0 And FigletMaximumColorLevel <= 16, FigletMaximumColorLevel, 16)
            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Maximum color level: {0}", FigletMaximumColorLevel)
        End If

        'Screensaver logic
        Do While True
            Console.CursorVisible = False
            If Figlet.CancellationPending = True Then
                HandleSaverCancel()
                Exit Do
            Else
                Console.Clear()

                'Set colors
                Dim ColorStorage As New Color(255, 255, 255)
                If FigletTrueColor Then
                    Dim RedColorNum As Integer = Randomizer.Next(FigletMinimumRedColorLevel, FigletMaximumRedColorLevel)
                    Dim GreenColorNum As Integer = Randomizer.Next(FigletMinimumGreenColorLevel, FigletMaximumGreenColorLevel)
                    Dim BlueColorNum As Integer = Randomizer.Next(FigletMinimumBlueColorLevel, FigletMaximumBlueColorLevel)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color (R;G;B: {0};{1};{2})", RedColorNum, GreenColorNum, BlueColorNum)
                    ColorStorage = New Color(RedColorNum, GreenColorNum, BlueColorNum)
                ElseIf Figlet255Colors Then
                    Dim ColorNum As Integer = Randomizer.Next(FigletMinimumColorLevel, FigletMaximumColorLevel)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", ColorNum)
                    ColorStorage = New Color(ColorNum)
                Else
                    Console.BackgroundColor = CType(Randomizer.Next(FigletMinimumColorLevel, FigletMaximumColorLevel), ConsoleColor)
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Got color ({0})", Console.BackgroundColor)
                End If

                'Prepare the figlet font for writing
                Dim FigletWrite As String = FigletText.ReplaceAll({vbCr, vbLf}, " - ")
                FigletWrite = FigletFontUsed.Render(FigletWrite)
                Dim FigletWriteLines() As String = FigletWrite.SplitNewLines.SkipWhile(Function(x) String.IsNullOrEmpty(x)).ToArray
                Dim FigletHeight As Integer = ConsoleMiddleHeight - FigletWriteLines.Length / 2
                Dim FigletWidth As Integer = ConsoleMiddleWidth - FigletWriteLines(0).Length / 2

                'Actually write it
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    If Figlet255Colors Or FigletTrueColor Then
                        WriteWhere(FigletWrite, FigletWidth, FigletHeight, True, ColorStorage)
                    Else
                        WriteWherePlain(FigletWrite, FigletWidth, FigletHeight, True)
                    End If
                End If
                SleepNoBlock(FigletDelay, Figlet)

                'Reset resize sync
                ResizeSyncing = False
                CurrentWindowWidth = Console.WindowWidth
                CurrentWindowHeight = Console.WindowHeight
            End If
        Loop
    End Sub

    ''' <summary>
    ''' Checks for any screensaver error
    ''' </summary>
    Sub CheckForError(sender As Object, e As RunWorkerCompletedEventArgs) Handles Figlet.RunWorkerCompleted
        HandleSaverError(e.Error)
    End Sub

End Module

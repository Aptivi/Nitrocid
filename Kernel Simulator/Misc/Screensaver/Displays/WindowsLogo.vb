
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
    Public Class WindowsLogoDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private CurrentWindowWidth As Integer
        Private CurrentWindowHeight As Integer
        Private ResizeSyncing As Boolean
        Private Drawn As Boolean

        Public Overrides Property ScreensaverName As String = "WindowsLogo" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            CurrentWindowWidth = Console.WindowWidth
            CurrentWindowHeight = Console.WindowHeight
            Console.BackgroundColor = ConsoleColor.Black
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Console.CursorVisible = False
            If ResizeSyncing Then
                Drawn = False

                'Reset resize sync
                ResizeSyncing = False
                CurrentWindowWidth = Console.WindowWidth
                CurrentWindowHeight = Console.WindowHeight
            Else
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True

                'Get the required positions for the four boxes
                Dim UpperLeftBoxEndX As Integer = (Console.WindowWidth / 2) - 1
                Dim UpperLeftBoxStartX As Integer = UpperLeftBoxEndX / 2
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Upper left box X position {0} -> {1}", UpperLeftBoxStartX, UpperLeftBoxEndX)

                Dim UpperLeftBoxStartY As Integer = 2
                Dim UpperLeftBoxEndY As Integer = (Console.WindowHeight / 2) - 1
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Upper left box Y position {0} -> {1}", UpperLeftBoxStartY, UpperLeftBoxEndY)

                Dim LowerLeftBoxEndX As Integer = (Console.WindowWidth / 2) - 1
                Dim LowerLeftBoxStartX As Integer = LowerLeftBoxEndX / 2
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Lower left box X position {0} -> {1}", LowerLeftBoxStartX, LowerLeftBoxEndX)

                Dim LowerLeftBoxStartY As Integer = (Console.WindowHeight / 2) + 1
                Dim LowerLeftBoxEndY As Integer = Console.WindowHeight - 2
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Lower left box X position {0} -> {1}", LowerLeftBoxStartX, LowerLeftBoxEndX)

                Dim UpperRightBoxStartX As Integer = (Console.WindowWidth / 2) + 2
                Dim UpperRightBoxEndX As Integer = (Console.WindowWidth / 2) + UpperRightBoxStartX / 2
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Upper right box X position {0} -> {1}", UpperRightBoxStartX, UpperRightBoxEndX)

                Dim UpperRightBoxStartY As Integer = 2
                Dim UpperRightBoxEndY As Integer = (Console.WindowHeight / 2) - 1
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Upper right box X position {0} -> {1}", UpperRightBoxStartX, UpperRightBoxEndX)

                Dim LowerRightBoxStartX As Integer = (Console.WindowWidth / 2) + 2
                Dim LowerRightBoxEndX As Integer = (Console.WindowWidth / 2) + LowerRightBoxStartX / 2
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Lower right box X position {0} -> {1}", LowerRightBoxStartX, LowerRightBoxEndX)

                Dim LowerRightBoxStartY As Integer = (Console.WindowHeight / 2) + 1
                Dim LowerRightBoxEndY As Integer = Console.WindowHeight - 2
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Lower right box X position {0} -> {1}", LowerRightBoxStartX, LowerRightBoxEndX)

                'Draw the Windows 11 logo
                If Not Drawn Then
                    Console.BackgroundColor = ConsoleColor.Black
                    Console.Clear()
                    SetConsoleColor(New Color($"0;120;212"), True)

                    'First, draw the upper left box
                    For X As Integer = UpperLeftBoxStartX To UpperLeftBoxEndX
                        For Y As Integer = UpperLeftBoxStartY To UpperLeftBoxEndY
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Filling upper left box {0},{1}...", X, Y)
                            Console.SetCursorPosition(X, Y)
                            Console.Write(" ")
                        Next
                    Next

                    'Second, draw the lower left box
                    For X As Integer = LowerLeftBoxStartX To LowerLeftBoxEndX
                        For Y As Integer = LowerLeftBoxStartY To LowerLeftBoxEndY
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Filling lower left box {0},{1}...", X, Y)
                            Console.SetCursorPosition(X, Y)
                            Console.Write(" ")
                        Next
                    Next

                    'Third, draw the upper right box
                    For X As Integer = UpperRightBoxStartX To UpperRightBoxEndX
                        For Y As Integer = UpperRightBoxStartY To UpperRightBoxEndY
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Filling upper right box {0},{1}...", X, Y)
                            Console.SetCursorPosition(X, Y)
                            Console.Write(" ")
                        Next
                    Next

                    'Fourth, draw the lower right box
                    For X As Integer = LowerRightBoxStartX To LowerRightBoxEndX
                        For Y As Integer = LowerRightBoxStartY To LowerRightBoxEndY
                            WdbgConditional(ScreensaverDebug, DebugLevel.I, "Filling lower right box {0},{1}...", X, Y)
                            Console.SetCursorPosition(X, Y)
                            Console.Write(" ")
                        Next
                    Next

                    'Set drawn
                    Drawn = True
                    WdbgConditional(ScreensaverDebug, DebugLevel.I, "Drawn!")
                End If
            End If
            If Drawn Then SleepNoBlock(1000, ScreensaverDisplayerThread)
        End Sub

    End Class
End Namespace

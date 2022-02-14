
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
    Module GlitterMatrixDisplay

        Public GlitterMatrix As New KernelThread("GlitterMatrix screensaver thread", True, AddressOf GlitterMatrix_DoWork)

        ''' <summary>
        ''' Handles the code of Glitter Matrix
        ''' </summary>
        Sub GlitterMatrix_DoWork()
            'Variables
            Dim RandomDriver As New Random()
            Dim CurrentWindowWidth As Integer = Console.WindowWidth
            Dim CurrentWindowHeight As Integer = Console.WindowHeight
            Dim ResizeSyncing As Boolean

            'Preparations
            SetConsoleColor(New Color(GlitterMatrixBackgroundColor), True)
            SetConsoleColor(New Color(GlitterMatrixForegroundColor))
            Console.Clear()
            Wdbg(DebugLevel.I, "Console geometry: {0}x{1}", Console.WindowWidth, Console.WindowHeight)

            'Screensaver logic
            Do While True
                Console.CursorVisible = False
                Dim Left As Integer = RandomDriver.Next(Console.WindowWidth)
                Dim Top As Integer = RandomDriver.Next(Console.WindowHeight)
                WdbgConditional(ScreensaverDebug, DebugLevel.I, "Selected left and top: {0}, {1}", Left, Top)
                Console.SetCursorPosition(Left, Top)
                If CurrentWindowHeight <> Console.WindowHeight Or CurrentWindowWidth <> Console.WindowWidth Then ResizeSyncing = True
                If Not ResizeSyncing Then
                    Console.Write(CStr(RandomDriver.Next(2)))
                Else
                    WdbgConditional(ScreensaverDebug, DebugLevel.W, "Color-syncing. Clearing...")
                    Console.Clear()
                End If

                'Reset resize sync
                ResizeSyncing = False
                CurrentWindowWidth = Console.WindowWidth
                CurrentWindowHeight = Console.WindowHeight
                SleepNoBlock(GlitterMatrixDelay, GlitterMatrix)
            Loop
        End Sub

    End Module
End Namespace

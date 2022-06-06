
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
    Public Module RandomSaverDisplay

        Friend RandomSaver As New KernelThread("RandomSaver screensaver thread", True, AddressOf RandomSaver_DoWork)

        ''' <summary>
        ''' Handles the code of RandomSaver
        ''' </summary>
        Sub RandomSaver_DoWork()
            Try
                'Variables
                Dim RandomDriver As New Random()
                Dim ScreensaverIndex As Integer = RandomDriver.Next(Screensavers.Count)
                Dim ScreensaverName As String = Screensavers.Keys(ScreensaverIndex)

                'Preparations
                Console.BackgroundColor = ConsoleColor.Black
                Console.ForegroundColor = ConsoleColor.White
                Console.Clear()
                Console.CursorVisible = False

                'Screensaver logic
                Do Until ScreensaverName <> "random"
                    'We don't want another "random" screensaver showing up, so keep selecting until it's no longer "random"
                    ScreensaverIndex = RandomDriver.Next(Screensavers.Count)
                    ScreensaverName = Screensavers.Keys(ScreensaverIndex)
                Loop

                'Run the screensaver thread and wait
                Screensavers(ScreensaverName).Start()
                Do While Screensavers(ScreensaverName).IsAlive
                    SleepNoBlock(10, RandomSaver)
                Loop
            Catch taex As ThreadInterruptedException
                HandleSaverCancel()
            Catch ex As Exception
                HandleSaverError(ex)
            End Try
        End Sub

    End Module
End Namespace

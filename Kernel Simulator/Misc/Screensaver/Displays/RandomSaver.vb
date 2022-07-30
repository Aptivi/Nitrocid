
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

Namespace Misc.Screensaver.Displays
    Public Class RandomSaverDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private RandomDriver As Random

        Public Overrides Property ScreensaverName As String = "RandomSaver" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            RandomDriver = New Random
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            Console.Clear()
            Console.CursorVisible = False
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Dim ScreensaverIndex As Integer = RandomDriver.Next(Screensavers.Count)
            Dim ScreensaverName As String = Screensavers.Keys(ScreensaverIndex)
            Dim Screensaver As BaseScreensaver = Screensavers(ScreensaverName)

            'We don't want another "random" screensaver showing up, so keep selecting until it's no longer "random"
            Do Until ScreensaverName <> "random"
                ScreensaverIndex = RandomDriver.Next(Screensavers.Count)
                ScreensaverName = Screensavers.Keys(ScreensaverIndex)
                Screensaver = Screensavers(ScreensaverName)
            Loop

            'Run the screensaver
            DisplayScreensaver(Screensaver)
        End Sub

    End Class
End Namespace

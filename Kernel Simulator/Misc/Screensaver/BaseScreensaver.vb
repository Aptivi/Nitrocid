
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

Namespace Misc.Screensaver
    Public MustInherit Class BaseScreensaver
        Implements IScreensaver

        Public Overridable Property ScreensaverName As String = "BaseScreensaver" Implements IScreensaver.ScreensaverName

        Public Overridable Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overridable Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            Console.BackgroundColor = ConsoleColor.Black
            Console.ForegroundColor = ConsoleColor.White
            Console.Clear()
            Console.CursorVisible = False
        End Sub

        Public Overridable Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            SleepNoBlock(10, ScreensaverDisplayerThread)
        End Sub
    End Class
End Namespace

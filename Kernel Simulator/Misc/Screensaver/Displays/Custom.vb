
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

Namespace Misc.Screensaver.Displays
    Public Class CustomDisplay
        Inherits BaseScreensaver
        Implements IScreensaver

        Private ReadOnly Property CustomSaver As BaseScreensaver

        'To Screensaver Developers: ONLY put the effect code in your scrnSaver() sub.
        '                           Set colors, write welcome message, etc. with the exception of infinite loop and the effect code in preDisplay() sub
        '                           Recommended: Turn off console cursor, and clear the screen in preDisplay() sub.
        '                           Substitute: TextWriterColor.Write() with System.Console.WriteLine() or System.Console.Write().

        'WARNING: Please refrain from using ICustomSaver; use IScreensaver instead, which is more dynamic.
        '         This implementation doesn't call PostDisplay().
        Public Overrides Property ScreensaverName As String = "Custom" Implements IScreensaver.ScreensaverName

        Public Overrides Property ScreensaverSettings As Dictionary(Of String, Object) Implements IScreensaver.ScreensaverSettings

        Public Overrides Sub ScreensaverPreparation() Implements IScreensaver.ScreensaverPreparation
            'Variable preparations
            Console.CursorVisible = False
            Wdbg(DebugLevel.I, "Entered CustomSaver.ScreensaverPreparation().")
            CustomSaver.ScreensaverPreparation()
            Wdbg(DebugLevel.I, "Exited CustomSaver.ScreensaverPreparation().")
        End Sub

        Public Overrides Sub ScreensaverLogic() Implements IScreensaver.ScreensaverLogic
            Wdbg(DebugLevel.I, "Entered CustomSaver.ScreensaverLogic().")
            CustomSaver.ScreensaverLogic()
            Wdbg(DebugLevel.I, "Exited CustomSaver.ScreensaverLogic().")
        End Sub

        Public Sub New(CustomSaver As BaseScreensaver)
            Me.CustomSaver = CustomSaver
        End Sub

    End Class
End Namespace

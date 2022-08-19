
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

Namespace Shell.Shells.Test.Commands
    ''' <summary>
    ''' It lets you test the powerline glyphs
    ''' </summary>
    Class Test_PowerLineTestCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim TransitionChar As Char = Convert.ToChar(&HE0B0)
            Dim PadlockChar As Char = Convert.ToChar(&HE0A2)
            Dim GitBranchChar As Char = Convert.ToChar(&HE0A0)
            Write(DoTranslation("Be sure to use a console font supporting PowerLine glyphs, or the output may not render properly. We recommend") + " Cascadia Code/Mono PL", True, ColTypes.Warning)
            Write(" One ", False, New Color(ConsoleColor.Black), New Color(85, 255, 255))
            Write(TransitionChar, False, New Color(85, 255, 255), New Color(255, 85, 255))
            Write(" Two ", False, New Color(ConsoleColor.Black), New Color(255, 85, 255))
            Write(TransitionChar, False, New Color(255, 85, 255), New Color(255, 255, 85))
            Write($" {PadlockChar} Secure ", False, New Color(ConsoleColor.Black), New Color(255, 255, 85))
            Write(TransitionChar, False, New Color(255, 255, 85), New Color(255, 255, 255))
            Write($" {GitBranchChar} master ", False, New Color(ConsoleColor.Black), New Color(255, 255, 255))
            Write(TransitionChar, True, New Color(255, 255, 255), BackgroundColor)
        End Sub

    End Class
End Namespace

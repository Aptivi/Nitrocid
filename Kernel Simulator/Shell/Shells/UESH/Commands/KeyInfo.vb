
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

Namespace Shell.Shells.UESH.Commands
    Class KeyInfoCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Write(DoTranslation("Enter a key or a combination of keys to display its information."), True, ColTypes.Neutral)
            Dim KeyPress As ConsoleKeyInfo = Console.ReadKey(True)

            'Pressed key
            Write("- " + DoTranslation("Pressed key") + ": ", False, ColTypes.ListEntry)
            Write(KeyPress.Key.ToString, True, ColTypes.ListValue)

            'If the pressed key is a control key, don't write the actual key char so as not to corrupt the output
            If Not Char.IsControl(KeyPress.KeyChar) Then
                Write("- " + DoTranslation("Pressed key character") + ": ", False, ColTypes.ListEntry)
                Write(KeyPress.KeyChar, True, ColTypes.ListValue)
            End If

            'Pressed key character code
            Write("- " + DoTranslation("Pressed key character code") + ": ", False, ColTypes.ListEntry)
            Write($"0x{Convert.ToInt32(KeyPress.KeyChar):X2} [{Convert.ToInt32(KeyPress.KeyChar)}]", True, ColTypes.ListValue)

            'Pressed modifiers
            Write("- " + DoTranslation("Pressed modifiers") + ": ", False, ColTypes.ListEntry)
            Write(KeyPress.Modifiers.ToString, True, ColTypes.ListValue)

            'Keyboard shortcut
            Write("- " + DoTranslation("Keyboard shortcut") + ": ", False, ColTypes.ListEntry)
            Write($"{String.Join(" +", KeyPress.Modifiers.ToString.Split(", "))} + {KeyPress.Key}", True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace

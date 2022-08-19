
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
    ''' <summary>
    ''' Converts the hexadecimal representation of the color to RGB numbers in KS format.
    ''' </summary>
    ''' <remarks>
    ''' If you want to get the semicolon-delimited sequence of the RGB color numbers from the hexadecimal representation of the color, you can use this command. You can use this to form a complete VT sequence of changing color.
    ''' </remarks>
    Class ColorHexToRgbKSCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim Hex As String = ListArgsOnly(0)
            Dim RGB As String

            'Do the job
            RGB = ColorTools.ConvertFromHexToRGB(Hex)
            Write("- " + DoTranslation("RGB color sequence:") + " ", False, ColTypes.ListEntry)
            Write(RGB, True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace

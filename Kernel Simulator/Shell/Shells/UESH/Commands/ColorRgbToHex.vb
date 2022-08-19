
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
    ''' Converts the color RGB numbers to hex.
    ''' </summary>
    ''' <remarks>
    ''' If you want to get the hexadecimal representation of the color from the RGB color numbers, you can use this command.
    ''' </remarks>
    Class ColorRgbToHexCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim R, G, B As Integer
            Dim Hex As String

            'Check to see if we have the numeric arguments
            If Not Integer.TryParse(ListArgsOnly(0), R) Then
                Write(DoTranslation("The red color level must be numeric."), True, ColTypes.Error)
                Exit Sub
            End If
            If Not Integer.TryParse(ListArgsOnly(1), G) Then
                Write(DoTranslation("The green color level must be numeric."), True, ColTypes.Error)
                Exit Sub
            End If
            If Not Integer.TryParse(ListArgsOnly(2), B) Then
                Write(DoTranslation("The blue color level must be numeric."), True, ColTypes.Error)
                Exit Sub
            End If

            'Do the job
            Hex = ConvertFromRGBToHex(R, G, B)
            Write("- " + DoTranslation("Color hexadecimal representation:") + " ", False, ColTypes.ListEntry)
            Write(Hex, True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace

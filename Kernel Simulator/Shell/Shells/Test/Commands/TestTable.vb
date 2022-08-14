
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

Imports KS.TimeDate

Namespace Shell.Shells.Test.Commands
    Class Test_TestTableCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim Headers() As String = {"Ubuntu Version", "Release Date", "Support End", "ESM Support End"}
            Dim Rows(,) As String = {{"12.04 (Precise Pangolin)", Render(New Date(2012, 4, 26)), Render(New Date(2017, 4, 28)), Render(New Date(2019, 4, 28))},
                                 {"14.04 (Trusty Tahr)", Render(New Date(2014, 4, 17)), Render(New Date(2019, 4, 25)), Render(New Date(2024, 4, 25))},
                                 {"16.04 (Xenial Xerus)", Render(New Date(2016, 4, 21)), Render(New Date(2021, 4, 30)), Render(New Date(2026, 4, 30))},
                                 {"18.04 (Bionic Beaver)", Render(New Date(2018, 4, 26)), Render(New Date(2023, 4, 30)), Render(New Date(2028, 4, 30))},
                                 {"20.04 (Focal Fossa)", Render(New Date(2020, 4, 23)), Render(New Date(2025, 4, 25)), Render(New Date(2030, 4, 25))},
                                 {"22.04 (Jammy Jellyfish)", Render(New Date(2022, 4, 26)), Render(New Date(2027, 4, 25)), Render(New Date(2032, 4, 25))}}
            Dim Margin As Integer = If(ListArgsOnly.Count > 0, ListArgsOnly(0), 2)
            WriteTable(Headers, Rows, Margin)
        End Sub

    End Class
End Namespace

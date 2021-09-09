
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Class WeatherCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String) Implements ICommand.Execute
        If ListArgs(0) = "listcities" Then
            Dim Cities As Dictionary(Of Long, String) = ListAllCities()
            WriteList(Cities)
        Else
            Dim APIKey As String
            W(DoTranslation("You can get your own API key at https://home.openweathermap.org/api_keys."), True, ColTypes.Neutral)
            W(DoTranslation("Enter your API key:") + " ", False, ColTypes.Input)
            APIKey = ReadLineNoInput("*")
            Console.WriteLine()
            PrintWeatherInfo(ListArgs(0), APIKey)
        End If
    End Sub

End Class

'    Kernel Simulator  Copyright (C) 2018  EoflaOE
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

Module unitConv

    Private resultVal As Object

    Sub Converter(ByVal sourceUnit As String, ByVal targetUnit As String, ByVal value As Object)

        'TODO: Add currency conversion (might require Internet)
        'Begin with size conversion first...
        If (sourceUnit.IndexOf("B", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("TB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1099511627776
        ElseIf (sourceUnit.IndexOf("B", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("GB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1073741824
        ElseIf (sourceUnit.IndexOf("B", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("MB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1048576
        ElseIf (sourceUnit.IndexOf("B", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("KB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1024
        ElseIf (sourceUnit.IndexOf("B", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Bits", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 8
        ElseIf (sourceUnit.IndexOf("KB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("TB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1073741824
        ElseIf (sourceUnit.IndexOf("KB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("GB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1048576
        ElseIf (sourceUnit.IndexOf("KB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("MB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1024
        ElseIf (sourceUnit.IndexOf("KB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("B", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1024
        ElseIf (sourceUnit.IndexOf("KB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Bits", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 8192
        ElseIf (sourceUnit.IndexOf("MB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("TB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1048576
        ElseIf (sourceUnit.IndexOf("MB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("GB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1024
        ElseIf (sourceUnit.IndexOf("MB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("KB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1024
        ElseIf (sourceUnit.IndexOf("MB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("B", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1048576
        ElseIf (sourceUnit.IndexOf("MB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Bits", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 8388608
        ElseIf (sourceUnit.IndexOf("GB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("TB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1024
        ElseIf (sourceUnit.IndexOf("GB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("MB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1024
        ElseIf (sourceUnit.IndexOf("GB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("KB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1048576
        ElseIf (sourceUnit.IndexOf("GB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("B", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1073741824
        ElseIf (sourceUnit.IndexOf("GB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Bits", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 8589934592
        ElseIf (sourceUnit.IndexOf("TB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("GB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1024
        ElseIf (sourceUnit.IndexOf("TB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("MB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1048576
        ElseIf (sourceUnit.IndexOf("TB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("KB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1073741824
        ElseIf (sourceUnit.IndexOf("TB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("B", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1099511627776
        ElseIf (sourceUnit.IndexOf("TB", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Bits", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 8796093022208
        ElseIf (sourceUnit.IndexOf("Bits", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("TB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 8796093022208
        ElseIf (sourceUnit.IndexOf("Bits", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("GB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 8589934592
        ElseIf (sourceUnit.IndexOf("Bits", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("MB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 8388608
        ElseIf (sourceUnit.IndexOf("Bits", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("KB", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 8192
        ElseIf (sourceUnit.IndexOf("Bits", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("B", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 8
        ElseIf (sourceUnit.IndexOf("Octal", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Binary", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then '... then to binaries ...
            resultVal = Convert.ToString(Convert.ToInt64(value), 2)
        ElseIf (sourceUnit.IndexOf("Binary", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Octal", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Or (sourceUnit.IndexOf("Decimal", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Octal", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = Oct(value)
        ElseIf (sourceUnit.IndexOf("Decimal", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Hexadecimal", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Or (sourceUnit.IndexOf("Binary", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Hexadecimal", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Or (sourceUnit.IndexOf("Octal", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Hexadecimal", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then '... then to decimals ...
            resultVal = Hex(value)
        ElseIf (sourceUnit.IndexOf("Hexadecimal", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Decimal", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = Val("&H" & value)
        ElseIf (sourceUnit.IndexOf("mm", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("cm", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then '... then to measurements ...
            resultVal = value / 10
        ElseIf (sourceUnit.IndexOf("mm", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("m", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000
        ElseIf (sourceUnit.IndexOf("mm", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("km", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000000
        ElseIf (sourceUnit.IndexOf("cm", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("mm", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 10
        ElseIf (sourceUnit.IndexOf("cm", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("m", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 100
        ElseIf (sourceUnit.IndexOf("cm", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("km", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 100000
        ElseIf (sourceUnit.IndexOf("m", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("mm", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000
        ElseIf (sourceUnit.IndexOf("m", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("cm", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 100
        ElseIf (sourceUnit.IndexOf("m", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("km", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000
        ElseIf (sourceUnit.IndexOf("km", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("mm", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000000
        ElseIf (sourceUnit.IndexOf("km", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("cm", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 100000
        ElseIf (sourceUnit.IndexOf("km", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("m", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000
        ElseIf (sourceUnit.IndexOf("Celsius", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Fahrenheit", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then '... then to temperature ...
            resultVal = value * 9 / 5 + 32
        ElseIf (sourceUnit.IndexOf("Celsius", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kelvin", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value + 273.15
        ElseIf (sourceUnit.IndexOf("Celsius", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Reaumur", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 4 / 5
        ElseIf (sourceUnit.IndexOf("Celsius", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Delisle", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (100 - value) * 3 / 2
        ElseIf (sourceUnit.IndexOf("Celsius", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Romer", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 21 / 40 + 7.5
        ElseIf (sourceUnit.IndexOf("Celsius", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Rankine", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (value + 273.15) * 9 / 5
        ElseIf (sourceUnit.IndexOf("Fahrenheit", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Celsius", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (value - 32) * 5 / 9
        ElseIf (sourceUnit.IndexOf("Fahrenheit", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kelvin", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (value + 459.67) * 5 / 9
        ElseIf (sourceUnit.IndexOf("Fahrenheit", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Reaumur", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (value - 32) * 4 / 9
        ElseIf (sourceUnit.IndexOf("Fahrenheit", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Delisle", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (212 - value) * 5 / 6
        ElseIf (sourceUnit.IndexOf("Fahrenheit", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Romer", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (value - 32) * 7 / 24 + 7.5
        ElseIf (sourceUnit.IndexOf("Fahrenheit", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Rankine", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value + 459.67
        ElseIf (sourceUnit.IndexOf("Kelvin", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Celsius", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value - 273.15
        ElseIf (sourceUnit.IndexOf("Kelvin", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Fahrenheit", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 9 / 5 - 459.67
        ElseIf (sourceUnit.IndexOf("Kelvin", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Reaumur", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (value - 273.15) * 4 / 5
        ElseIf (sourceUnit.IndexOf("Kelvin", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Delisle", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (373.15 - value) * 3 / 2
        ElseIf (sourceUnit.IndexOf("Kelvin", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Romer", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (value - 273.15) * 21 / 40 + 7.5
        ElseIf (sourceUnit.IndexOf("Kelvin", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Rankine", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 9 / 5
        ElseIf (sourceUnit.IndexOf("Reaumur", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Celsius", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 5 / 4
        ElseIf (sourceUnit.IndexOf("Reaumur", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Fahrenheit", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 9 / 4 + 32
        ElseIf (sourceUnit.IndexOf("Reaumur", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kelvin", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 5 / 4 + 273.15
        ElseIf (sourceUnit.IndexOf("Reaumur", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Delisle", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (80 - value) * 15 / 8
        ElseIf (sourceUnit.IndexOf("Reaumur", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Romer", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 21 / 32 + 7.5
        ElseIf (sourceUnit.IndexOf("Reaumur", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Rankine", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 9 / 4 + 491.67
        ElseIf (sourceUnit.IndexOf("Delisle", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Celsius", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = 100 - value * 2 / 3
        ElseIf (sourceUnit.IndexOf("Delisle", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Fahrenheit", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = 212 - value * 6 / 5
        ElseIf (sourceUnit.IndexOf("Delisle", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kelvin", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = 373.15 - (value * 2 / 3)
        ElseIf (sourceUnit.IndexOf("Delisle", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Reaumur", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = 80 - value * 8 / 15
        ElseIf (sourceUnit.IndexOf("Delisle", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Romer", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = 60 - value * 7 / 20
        ElseIf (sourceUnit.IndexOf("Delisle", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Rankine", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = 671.67 - value * 6 / 5
        ElseIf (sourceUnit.IndexOf("Romer", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Celsius", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (value - 7.5) * 40 / 21
        ElseIf (sourceUnit.IndexOf("Romer", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Fahrenheit", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (value - 7.5) * 24 / 7 + 32
        ElseIf (sourceUnit.IndexOf("Romer", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kelvin", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (value - 7.5) * 40 / 21 + 273.15
        ElseIf (sourceUnit.IndexOf("Romer", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Delisle", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (60 - value) * 20 / 7
        ElseIf (sourceUnit.IndexOf("Romer", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Reaumur", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (value - 7.5) * 32 / 21
        ElseIf (sourceUnit.IndexOf("Romer", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Rankine", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (value - 7.5) * 24 / 7 + 491.67
        ElseIf (sourceUnit.IndexOf("Rankine", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Celsius", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (value - 491.67) * 5 / 9
        ElseIf (sourceUnit.IndexOf("Rankine", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Fahrenheit", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value - 459.67
        ElseIf (sourceUnit.IndexOf("Rankine", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kelvin", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 5 / 9
        ElseIf (sourceUnit.IndexOf("Rankine", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Delisle", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (671.67 - value) * 5 / 6
        ElseIf (sourceUnit.IndexOf("Rankine", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Romer", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (value - 491.67) * 7 / 24 + 7.5
        ElseIf (sourceUnit.IndexOf("Rankine", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Reaumur", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = (value - 491.67) * 4 / 9
        ElseIf (sourceUnit.IndexOf("j", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("kj", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then '... then to energy ...
            resultVal = value / 1000
        ElseIf (sourceUnit.IndexOf("kj", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("j", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000
        ElseIf (sourceUnit.IndexOf("m/s", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("km/h", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 3.6
        ElseIf (sourceUnit.IndexOf("m/s", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("cm/ms", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then 'Note that cm/ms is Centimeters per Millisecond
            resultVal = value / 10
        ElseIf (sourceUnit.IndexOf("cm/ms", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("km/h", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 36
        ElseIf (sourceUnit.IndexOf("cm/ms", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("m/s", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 10
        ElseIf (sourceUnit.IndexOf("km/h", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("m/s", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 3.6
        ElseIf (sourceUnit.IndexOf("km/h", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("cm/ms", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 36
        ElseIf (sourceUnit.IndexOf("Grams", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kilograms", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then '... then to mass ...
            resultVal = value / 1000
        ElseIf (sourceUnit.IndexOf("Grams", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Tons", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000 / 1000
        ElseIf (sourceUnit.IndexOf("Grams", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kilotons", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000 / 1000 / 1000
        ElseIf (sourceUnit.IndexOf("Grams", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Megatons", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000 / 1000 / 1000 / 1000
        ElseIf (sourceUnit.IndexOf("Kilograms", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Grams", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000
        ElseIf (sourceUnit.IndexOf("Kilograms", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Tons", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000
        ElseIf (sourceUnit.IndexOf("Kilograms", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kilotons", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000 / 1000
        ElseIf (sourceUnit.IndexOf("Kilograms", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Megatons", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000 / 1000 / 1000
        ElseIf (sourceUnit.IndexOf("Tons", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Grams", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000 * 1000
        ElseIf (sourceUnit.IndexOf("Tons", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kilograms", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000
        ElseIf (sourceUnit.IndexOf("Tons", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kilotons", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000
        ElseIf (sourceUnit.IndexOf("Tons", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Megatons", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000 / 1000
        ElseIf (sourceUnit.IndexOf("Kilotons", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Grams", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000 * 1000 * 1000
        ElseIf (sourceUnit.IndexOf("Kilotons", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kilograms", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000 * 1000
        ElseIf (sourceUnit.IndexOf("Kilotons", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Tons", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000
        ElseIf (sourceUnit.IndexOf("Kilotons", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Megatons", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000
        ElseIf (sourceUnit.IndexOf("Megatons", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Grams", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000 * 1000 * 1000 * 1000
        ElseIf (sourceUnit.IndexOf("Megatons", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kilograms", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000 * 1000 * 1000
        ElseIf (sourceUnit.IndexOf("Megatons", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Tons", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000 * 1000
        ElseIf (sourceUnit.IndexOf("Megatons", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kilotons", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000
        ElseIf (sourceUnit.IndexOf("n", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("kn", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then '... then to force ... | Note: kn is Kilonewtons
            resultVal = value / 1000
        ElseIf (sourceUnit.IndexOf("kn", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("n", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000
        ElseIf (sourceUnit.IndexOf("Hz", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("kHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then '... then to frequency ...
            resultVal = value / 1000
        ElseIf (sourceUnit.IndexOf("Hz", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("MHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000 / 1000
        ElseIf (sourceUnit.IndexOf("Hz", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("GHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000 / 1000 / 1000
        ElseIf (sourceUnit.IndexOf("kHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Hz", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000
        ElseIf (sourceUnit.IndexOf("kHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("MHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000
        ElseIf (sourceUnit.IndexOf("kHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("GHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000 / 1000
        ElseIf (sourceUnit.IndexOf("MHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Hz", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000 * 1000
        ElseIf (sourceUnit.IndexOf("MHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("kHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000
        ElseIf (sourceUnit.IndexOf("MHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("GHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000
        ElseIf (sourceUnit.IndexOf("GHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Hz", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000 * 1000 * 1000
        ElseIf (sourceUnit.IndexOf("GHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("kHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000 * 1000
        ElseIf (sourceUnit.IndexOf("GHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("MHz", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000
        ElseIf (sourceUnit.IndexOf("Number", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Money", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then '... then to number types ...
            resultVal = FormatCurrency(value, 2)
        ElseIf (sourceUnit.IndexOf("Number", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Percent", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = FormatPercent(value, 2)
        ElseIf (sourceUnit.IndexOf("Centivolts", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Volts", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then '... then to electricity ...
            resultVal = value / 1000
        ElseIf (sourceUnit.IndexOf("Centivolts", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kilovolts", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000 / 1000
        ElseIf (sourceUnit.IndexOf("Volts", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Centivolts", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000
        ElseIf (sourceUnit.IndexOf("Volts", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kilovolts", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000
        ElseIf (sourceUnit.IndexOf("Kilovolts", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Centivolts", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000 * 1000
        ElseIf (sourceUnit.IndexOf("Kilovolts", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Volts", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000
        ElseIf (sourceUnit.IndexOf("Watts", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kilowatts", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then '... then to electricity's wattage ...
            resultVal = value / 1000
        ElseIf (sourceUnit.IndexOf("Kilowatts", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Watts", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000
        ElseIf (sourceUnit.IndexOf("Milliliters", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Liters", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then '... then to liquid's volume ...
            resultVal = value / 1000
        ElseIf (sourceUnit.IndexOf("Milliliters", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kiloliters", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000 / 1000
        ElseIf (sourceUnit.IndexOf("Liters", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Milliliters", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000
        ElseIf (sourceUnit.IndexOf("Liters", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Kiloliters", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value / 1000
        ElseIf (sourceUnit.IndexOf("Kiloliters", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Milliliters", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000 * 1000
        ElseIf (sourceUnit.IndexOf("Kiloliters", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Liters", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1000
        ElseIf (sourceUnit.IndexOf("Ounces", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Gallons", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then '... then to more liquid's volume ...
            resultVal = value * 0.0078125
        ElseIf (sourceUnit.IndexOf("Gallons", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Ounces", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 128.0
        ElseIf (sourceUnit.IndexOf("Feet", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Inches", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then '... then finally to more measurements.
            resultVal = value * 12
        ElseIf (sourceUnit.IndexOf("Feet", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Yards", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 0.3333333333
        ElseIf (sourceUnit.IndexOf("Feet", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Miles", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 0.0001893939
        ElseIf (sourceUnit.IndexOf("Inches", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Feet", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 0.0833333333
        ElseIf (sourceUnit.IndexOf("Inches", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Yards", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 0.0277777778
        ElseIf (sourceUnit.IndexOf("Inches", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Miles", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 0.0000157828
        ElseIf (sourceUnit.IndexOf("Yards", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Feet", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 3
        ElseIf (sourceUnit.IndexOf("Yards", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Inches", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 36
        ElseIf (sourceUnit.IndexOf("Yards", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Miles", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 0.0005681818
        ElseIf (sourceUnit.IndexOf("Miles", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Feet", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 5280
        ElseIf (sourceUnit.IndexOf("Miles", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Inches", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 63360
        ElseIf (sourceUnit.IndexOf("Miles", 0, StringComparison.CurrentCultureIgnoreCase) > -1 And targetUnit.IndexOf("Yards", 0, StringComparison.CurrentCultureIgnoreCase) > -1) Then
            resultVal = value * 1760
        Else
            Wln("{0} cannot be converted to {1}.", "neutralText", sourceUnit, targetUnit)
            Exit Sub
        End If
        Wln("{0} to {1}: {2}", "neutralText", sourceUnit, targetUnit, resultVal)

    End Sub

End Module

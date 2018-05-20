
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
        If (sourceUnit = "B" And targetUnit = "TB") Then
            resultVal = value / 1099511627776
        ElseIf (sourceUnit = "B" And targetUnit = "GB") Then
            resultVal = value / 1073741824
        ElseIf (sourceUnit = "B" And targetUnit = "MB") Then
            resultVal = value / 1048576
        ElseIf (sourceUnit = "B" And targetUnit = "KB") Then
            resultVal = value / 1024
        ElseIf (sourceUnit = "B" And targetUnit = "Bits") Then
            resultVal = value * 8
        ElseIf (sourceUnit = "KB" And targetUnit = "TB") Then
            resultVal = value / 1073741824
        ElseIf (sourceUnit = "KB" And targetUnit = "GB") Then
            resultVal = value / 1048576
        ElseIf (sourceUnit = "KB" And targetUnit = "MB") Then
            resultVal = value / 1024
        ElseIf (sourceUnit = "KB" And targetUnit = "B") Then
            resultVal = value * 1024
        ElseIf (sourceUnit = "KB" And targetUnit = "Bits") Then
            resultVal = value * 8192
        ElseIf (sourceUnit = "MB" And targetUnit = "TB") Then
            resultVal = value / 1048576
        ElseIf (sourceUnit = "MB" And targetUnit = "GB") Then
            resultVal = value / 1024
        ElseIf (sourceUnit = "MB" And targetUnit = "KB") Then
            resultVal = value * 1024
        ElseIf (sourceUnit = "MB" And targetUnit = "B") Then
            resultVal = value * 1048576
        ElseIf (sourceUnit = "MB" And targetUnit = "Bits") Then
            resultVal = value * 8388608
        ElseIf (sourceUnit = "GB" And targetUnit = "TB") Then
            resultVal = value / 1024
        ElseIf (sourceUnit = "GB" And targetUnit = "MB") Then
            resultVal = value * 1024
        ElseIf (sourceUnit = "GB" And targetUnit = "KB") Then
            resultVal = value * 1048576
        ElseIf (sourceUnit = "GB" And targetUnit = "B") Then
            resultVal = value * 1073741824
        ElseIf (sourceUnit = "GB" And targetUnit = "Bits") Then
            resultVal = value * 8589934592
        ElseIf (sourceUnit = "TB" And targetUnit = "GB") Then
            resultVal = value * 1024
        ElseIf (sourceUnit = "TB" And targetUnit = "MB") Then
            resultVal = value * 1048576
        ElseIf (sourceUnit = "TB" And targetUnit = "KB") Then
            resultVal = value * 1073741824
        ElseIf (sourceUnit = "TB" And targetUnit = "B") Then
            resultVal = value * 1099511627776
        ElseIf (sourceUnit = "TB" And targetUnit = "Bits") Then
            resultVal = value * 8796093022208
        ElseIf (sourceUnit = "Bits" And targetUnit = "TB") Then
            resultVal = value / 8796093022208
        ElseIf (sourceUnit = "Bits" And targetUnit = "GB") Then
            resultVal = value / 8589934592
        ElseIf (sourceUnit = "Bits" And targetUnit = "MB") Then
            resultVal = value / 8388608
        ElseIf (sourceUnit = "Bits" And targetUnit = "KB") Then
            resultVal = value / 8192
        ElseIf (sourceUnit = "Bits" And targetUnit = "B") Then
            resultVal = value / 8
        ElseIf (sourceUnit = "Octal" And targetUnit = "Binary") Then '... then to binaries ...
            resultVal = Convert.ToString(Convert.ToInt64(value), 2)
        ElseIf (sourceUnit = "Binary" And targetUnit = "Octal") Or (sourceUnit = "Decimal" And targetUnit = "Octal") Then
            resultVal = Oct(value)
        ElseIf (sourceUnit = "Decimal" And targetUnit = "Hexadecimal") Or (sourceUnit = "Binary" And targetUnit = "Hexadecimal") Or (sourceUnit = "Octal" And targetUnit = "Hexadecimal") Then '... then to decimals ...
            resultVal = Hex(value)
        ElseIf (sourceUnit = "Hexadecimal" And targetUnit = "Decimal") Then
            resultVal = Val("&H" & value)
        ElseIf (sourceUnit = "mm" And targetUnit = "cm") Then '... then to measurements ...
            resultVal = value / 10
        ElseIf (sourceUnit = "mm" And targetUnit = "m") Then
            resultVal = value / 1000
        ElseIf (sourceUnit = "mm" And targetUnit = "km") Then
            resultVal = value / 1000000
        ElseIf (sourceUnit = "cm" And targetUnit = "mm") Then
            resultVal = value * 10
        ElseIf (sourceUnit = "cm" And targetUnit = "m") Then
            resultVal = value / 100
        ElseIf (sourceUnit = "cm" And targetUnit = "km") Then
            resultVal = value / 100000
        ElseIf (sourceUnit = "m" And targetUnit = "mm") Then
            resultVal = value * 1000
        ElseIf (sourceUnit = "m" And targetUnit = "cm") Then
            resultVal = value * 100
        ElseIf (sourceUnit = "m" And targetUnit = "km") Then
            resultVal = value / 1000
        ElseIf (sourceUnit = "km" And targetUnit = "mm") Then
            resultVal = value * 1000000
        ElseIf (sourceUnit = "km" And targetUnit = "cm") Then
            resultVal = value * 100000
        ElseIf (sourceUnit = "km" And targetUnit = "m") Then
            resultVal = value * 1000
            'TODO: Add more uncommon temperatures (Reaumur, Delisle, Romer, and Rankine)
        ElseIf (sourceUnit = "Celsius" And targetUnit = "Fahrenheit") Then '... then to temperature ...
            resultVal = value * 9 / 5 + 32
        ElseIf (sourceUnit = "Celsius" And targetUnit = "Kelvin") Then
            resultVal = value + 273.15
        ElseIf (sourceUnit = "Fahrenheit" And targetUnit = "Celsius") Then
            resultVal = (value - 32) * 5 / 9
        ElseIf (sourceUnit = "Fahrenheit" And targetUnit = "Kelvin") Then
            resultVal = (value + 459.67) * 5 / 9
        ElseIf (sourceUnit = "Kelvin" And targetUnit = "Celsius") Then
            resultVal = value - 273.15
        ElseIf (sourceUnit = "Kelvin" And targetUnit = "Fahrenheit") Then
            resultVal = value * 9 / 5 - 459.67
        ElseIf (sourceUnit = "j" And targetUnit = "kj") Then '... then to energy ...
            resultVal = value / 1000
        ElseIf (sourceUnit = "kj" And targetUnit = "j") Then
            resultVal = value * 1000
        ElseIf (sourceUnit = "m/s" And targetUnit = "km/h") Then
            resultVal = value * 3.6
        ElseIf (sourceUnit = "m/s" And targetUnit = "cm/ms") Then 'Note that cm/ms is Centimeters per Millisecond
            resultVal = value / 10
        ElseIf (sourceUnit = "cm/ms" And targetUnit = "km/h") Then
            resultVal = value * 36
        ElseIf (sourceUnit = "cm/ms" And targetUnit = "m/s") Then
            resultVal = value * 10
        ElseIf (sourceUnit = "km/h" And targetUnit = "m/s") Then
            resultVal = value / 3.6
        ElseIf (sourceUnit = "km/h" And targetUnit = "cm/ms") Then
            resultVal = value / 36
        ElseIf (sourceUnit = "Grams" And targetUnit = "Kilograms") Then '... then to mass ...
            resultVal = value / 1000
        ElseIf (sourceUnit = "Grams" And targetUnit = "Tons") Then
            resultVal = value / 1000 / 1000
        ElseIf (sourceUnit = "Grams" And targetUnit = "Kilotons") Then
            resultVal = value / 1000 / 1000 / 1000
        ElseIf (sourceUnit = "Grams" And targetUnit = "Megatons") Then
            resultVal = value / 1000 / 1000 / 1000 / 1000
        ElseIf (sourceUnit = "Kilograms" And targetUnit = "Grams") Then
            resultVal = value * 1000
        ElseIf (sourceUnit = "Kilograms" And targetUnit = "Tons") Then
            resultVal = value / 1000
        ElseIf (sourceUnit = "Kilograms" And targetUnit = "Kilotons") Then
            resultVal = value / 1000 / 1000
        ElseIf (sourceUnit = "Kilograms" And targetUnit = "Megatons") Then
            resultVal = value / 1000 / 1000 / 1000
        ElseIf (sourceUnit = "Tons" And targetUnit = "Grams") Then
            resultVal = value * 1000 * 1000
        ElseIf (sourceUnit = "Tons" And targetUnit = "Kilograms") Then
            resultVal = value * 1000
        ElseIf (sourceUnit = "Tons" And targetUnit = "Kilotons") Then
            resultVal = value / 1000
        ElseIf (sourceUnit = "Tons" And targetUnit = "Megatons") Then
            resultVal = value / 1000 / 1000
        ElseIf (sourceUnit = "Kilotons" And targetUnit = "Grams") Then
            resultVal = value * 1000 * 1000 * 1000
        ElseIf (sourceUnit = "Kilotons" And targetUnit = "Kilograms") Then
            resultVal = value * 1000 * 1000
        ElseIf (sourceUnit = "Kilotons" And targetUnit = "Tons") Then
            resultVal = value * 1000
        ElseIf (sourceUnit = "Kilotons" And targetUnit = "Megatons") Then
            resultVal = value / 1000
        ElseIf (sourceUnit = "Megatons" And targetUnit = "Grams") Then
            resultVal = value * 1000 * 1000 * 1000 * 1000
        ElseIf (sourceUnit = "Megatons" And targetUnit = "Kilograms") Then
            resultVal = value * 1000 * 1000 * 1000
        ElseIf (sourceUnit = "Megatons" And targetUnit = "Tons") Then
            resultVal = value * 1000 * 1000
        ElseIf (sourceUnit = "Megatons" And targetUnit = "Kilotons") Then
            resultVal = value * 1000
        ElseIf (sourceUnit = "n" And targetUnit = "kn") Then '... then to force ... | Note: kn is Kilonewtons
            resultVal = value / 1000
        ElseIf (sourceUnit = "kn" And targetUnit = "n") Then
            resultVal = value * 1000
        ElseIf (sourceUnit = "Hz" And targetUnit = "kHz") Then '... then to frequency ...
            resultVal = value / 1000
        ElseIf (sourceUnit = "Hz" And targetUnit = "MHz") Then
            resultVal = value / 1000 / 1000
        ElseIf (sourceUnit = "Hz" And targetUnit = "GHz") Then
            resultVal = value / 1000 / 1000 / 1000
        ElseIf (sourceUnit = "kHz" And targetUnit = "Hz") Then
            resultVal = value * 1000
        ElseIf (sourceUnit = "kHz" And targetUnit = "MHz") Then
            resultVal = value / 1000
        ElseIf (sourceUnit = "kHz" And targetUnit = "GHz") Then
            resultVal = value / 1000 / 1000
        ElseIf (sourceUnit = "MHz" And targetUnit = "Hz") Then
            resultVal = value * 1000 * 1000
        ElseIf (sourceUnit = "MHz" And targetUnit = "kHz") Then
            resultVal = value * 1000
        ElseIf (sourceUnit = "MHz" And targetUnit = "GHz") Then
            resultVal = value / 1000
        ElseIf (sourceUnit = "GHz" And targetUnit = "Hz") Then
            resultVal = value * 1000 * 1000 * 1000
        ElseIf (sourceUnit = "GHz" And targetUnit = "kHz") Then
            resultVal = value * 1000 * 1000
        ElseIf (sourceUnit = "GHz" And targetUnit = "MHz") Then
            resultVal = value * 1000
        ElseIf (sourceUnit = "Number" And targetUnit = "Money") Then '... then to number types ...
            resultVal = FormatCurrency(value, 2)
        ElseIf (sourceUnit = "Number" And targetUnit = "Percent") Then
            resultVal = FormatPercent(value, 2)
        ElseIf (sourceUnit = "Centivolts" And targetUnit = "Volts") Then '... then to electricity ...
            resultVal = value / 1000
        ElseIf (sourceUnit = "Centivolts" And targetUnit = "Kilovolts") Then
            resultVal = value / 1000 / 1000
        ElseIf (sourceUnit = "Volts" And targetUnit = "Centivolts") Then
            resultVal = value * 1000
        ElseIf (sourceUnit = "Volts" And targetUnit = "Kilovolts") Then
            resultVal = value / 1000
        ElseIf (sourceUnit = "Kilovolts" And targetUnit = "Centivolts") Then
            resultVal = value * 1000 * 1000
        ElseIf (sourceUnit = "Kilovolts" And targetUnit = "Volts") Then
            resultVal = value * 1000
        ElseIf (sourceUnit = "Watts" And targetUnit = "Kilowatts") Then '... then to electricity's wattage ...
            resultVal = value / 1000
        ElseIf (sourceUnit = "Kilowatts" And targetUnit = "Watts") Then
            resultVal = value * 1000
        ElseIf (sourceUnit = "Milliliters" And targetUnit = "Liters") Then '... then to liquid's volume ...
            resultVal = value / 1000
        ElseIf (sourceUnit = "Milliliters" And targetUnit = "Kiloliters") Then
            resultVal = value / 1000 / 1000
        ElseIf (sourceUnit = "Liters" And targetUnit = "Milliliters") Then
            resultVal = value * 1000
        ElseIf (sourceUnit = "Liters" And targetUnit = "Kiloliters") Then
            resultVal = value / 1000
        ElseIf (sourceUnit = "Kiloliters" And targetUnit = "Milliliters") Then
            resultVal = value * 1000 * 1000
        ElseIf (sourceUnit = "Kiloliters" And targetUnit = "Liters") Then
            resultVal = value * 1000
        ElseIf (sourceUnit = "Ounces" And targetUnit = "Gallons") Then '... then to more liquid's volume ...
            resultVal = value * 0.0078125
        ElseIf (sourceUnit = "Gallons" And targetUnit = "Ounces") Then
            resultVal = value * 128.0
        ElseIf (sourceUnit = "Feet" And targetUnit = "Inches") Then '... then finally to more measurements.
            resultVal = value * 12
        ElseIf (sourceUnit = "Feet" And targetUnit = "Yards") Then
            resultVal = value * 0.3333333333
        ElseIf (sourceUnit = "Feet" And targetUnit = "Miles") Then
            resultVal = value * 0.0001893939
        ElseIf (sourceUnit = "Inches" And targetUnit = "Feet") Then
            resultVal = value * 0.0833333333
        ElseIf (sourceUnit = "Inches" And targetUnit = "Yards") Then
            resultVal = value * 0.0277777778
        ElseIf (sourceUnit = "Inches" And targetUnit = "Miles") Then
            resultVal = value * 0.0000157828
        ElseIf (sourceUnit = "Yards" And targetUnit = "Feet") Then
            resultVal = value * 3
        ElseIf (sourceUnit = "Yards" And targetUnit = "Inches") Then
            resultVal = value * 36
        ElseIf (sourceUnit = "Yards" And targetUnit = "Miles") Then
            resultVal = value * 0.0005681818
        ElseIf (sourceUnit = "Miles" And targetUnit = "Feet") Then
            resultVal = value * 5280
        ElseIf (sourceUnit = "Miles" And targetUnit = "Inches") Then
            resultVal = value * 63360
        ElseIf (sourceUnit = "Miles" And targetUnit = "Yards") Then
            resultVal = value * 1760
        Else
            Wln("{0} cannot be converted to {1}.", "neutralText")
        End If
        Wln("{0} to {1}: {2}", "neutralText", sourceUnit, targetUnit, resultVal)

    End Sub

End Module

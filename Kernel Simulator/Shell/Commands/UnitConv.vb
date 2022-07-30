
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

Imports UnitsNet

Namespace Shell.Commands
    Class UnitConvCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim UnitType As String = ListArgsOnly(0)
            Dim QuantityNum As Integer = ListArgsOnly(1)
            Dim SourceUnit As String = ListArgsOnly(2)
            Dim TargetUnit As String = ListArgsOnly(3)
            Dim QuantityInfos As QuantityInfo() = Quantity.Infos.Where(Function(x) x.Name = UnitType).ToArray()
            Dim TargetUnitInstance As [Enum] = UnitParser.Default.Parse(TargetUnit, QuantityInfos(0).UnitType)
            Dim ConvertedUnit As IQuantity = Quantity.Parse(QuantityInfos(0).ValueType, $"{QuantityNum} {SourceUnit}").ToUnit(TargetUnitInstance)
            Write("- {0} => {1}: ", False, ColTypes.ListEntry, SourceUnit, TargetUnit)
            Write(ConvertedUnit.ToString(CurrentCult.NumberFormat), True, ColTypes.ListValue)
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("Available unit types and their units:"), True, ColTypes.Neutral)
            For Each QuantityInfo As QuantityInfo In Quantity.Infos
                Write("- {0}:", True, ColTypes.ListEntry, QuantityInfo.Name)
                For Each UnitValues As [Enum] In QuantityInfo.UnitInfos().Select(Function(x) x.Value)
                    Write("  - {0}: ", False, ColTypes.ListEntry, String.Join(", ", UnitAbbreviationsCache.Default.GetDefaultAbbreviation(UnitValues.GetType, Convert.ToInt32(UnitValues))))
                    Write(UnitValues.ToString(), True, ColTypes.ListValue)
                Next
            Next
        End Sub

    End Class
End Namespace


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

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Lists all units
    ''' </summary>
    ''' <remarks>
    ''' If you don't know what units are there, you can use this command. If you don't know what unit types are there, use its help entry.
    ''' </remarks>
    Class ListUnitsCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Dim Quantities = Quantity.Infos.Where(Function(x) x.Name = ListArgsOnly(0))
            If Quantities.Count <> 0 Then
                Write(DoTranslation("Available unit types and their units:"), True, ColTypes.Neutral)
                For Each QuantityInfo As QuantityInfo In Quantities
                    Write("- {0}:", True, ColTypes.ListEntry, QuantityInfo.Name)
                    For Each UnitValues As [Enum] In QuantityInfo.UnitInfos().Select(Function(x) x.Value)
                        Write("  - {0}: ", False, ColTypes.ListEntry, String.Join(", ", UnitAbbreviationsCache.Default.GetDefaultAbbreviation(UnitValues.GetType, Convert.ToInt32(UnitValues))))
                        Write(UnitValues.ToString(), True, ColTypes.ListValue)
                    Next
                Next
            Else
                Write(DoTranslation("No such unit type:") + " {0}", True, ColTypes.Error, ListArgsOnly(0))
            End If
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("Available unit types:"), True, ColTypes.Neutral)
            For Each QuantityInfo As QuantityInfo In Quantity.Infos
                Write("- {0}", True, ColTypes.ListEntry, QuantityInfo.Name)
            Next
        End Sub

    End Class
End Namespace

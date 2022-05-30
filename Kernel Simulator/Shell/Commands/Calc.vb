
'    Kernel Simulator  Copyright (C) 2018-2022  EoflaOE
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

Imports Extensification.Legacy.StringExts

Namespace Shell.Commands
    Class CalcCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Try
#If Not NETCOREAPP Then
                Dim Res As String = Evaluate(StringArgs)
                Wdbg(DebugLevel.I, "Res = {0}", Res)
                If Res = "" Then 'If there is an error in calculation
                    Write(DoTranslation("Error in calculation."), True, ColTypes.Error)
                Else 'Calculation done
                    Write(StringArgs + " = " + Res, True, ColTypes.Neutral)
                End If
#Else
                Throw New PlatformNotSupportedException(DoTranslation("This feature isn't implemented on .NET. Use the .NET Framework version of KS."))
#End If
            Catch ex As Exception
                WStkTrc(ex)
                Write(DoTranslation("Error in calculation."), True, ColTypes.Error)
            End Try
        End Sub

    End Class
End Namespace
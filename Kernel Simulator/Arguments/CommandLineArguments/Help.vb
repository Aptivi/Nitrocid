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

Class CommandLine_HelpArgument
    Inherits ArgumentExecutor
    Implements IArgument

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements IArgument.Execute
        W("- testMod: ", False, ColTypes.ListEntry) : W(AvailableCMDLineArgs("testMod").GetTranslatedHelpEntry, True, ColTypes.ListValue)
        W("- testInteractive: ", False, ColTypes.ListEntry) : W(AvailableCMDLineArgs("testInteractive").GetTranslatedHelpEntry, True, ColTypes.ListValue)
        W("- debug: ", False, ColTypes.ListEntry) : W(AvailableCMDLineArgs("debug").GetTranslatedHelpEntry, True, ColTypes.ListValue)
        W("- args: ", False, ColTypes.ListEntry) : W(AvailableCMDLineArgs("args").GetTranslatedHelpEntry, True, ColTypes.ListValue)
        W("- reset: ", False, ColTypes.ListEntry) : W(AvailableCMDLineArgs("reset").GetTranslatedHelpEntry, True, ColTypes.ListValue)
        W(DoTranslation("* Press any key to start the kernel or ESC to exit."), True, ColTypes.Tip)
        If Console.ReadKey(True).Key = ConsoleKey.Escape Then
            Environment.Exit(0)
        End If
    End Sub

End Class
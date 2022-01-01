
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

Class ChLangCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
        Dim AlwaysTransliterated, AlwaysTranslated, Force As Boolean
        If ListSwitchesOnly.Contains("-alwaystransliterated") Then AlwaysTransliterated = True
        If ListSwitchesOnly.Contains("-alwaystranslated") Then AlwaysTranslated = True '-alwaystransliterated has higher priority.
        If ListSwitchesOnly.Contains("-force") Then Force = True
        PromptForSetLang(ListArgsOnly(0), Force, AlwaysTransliterated, AlwaysTranslated)
    End Sub

    Public Sub HelpHelper()
        Dim UsageLength As Integer = DoTranslation("Usage:").Length
        Write(" ".Repeat(UsageLength) + " " + " <language>: " + String.Join("/", Languages.Keys), True, ColTypes.Neutral)
        Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
        Write("  -alwaystransliterated: ", False, ColTypes.ListEntry) : Write(DoTranslation("Always use the transliterated version"), True, ColTypes.ListValue)
        Write("  -alwaystranslated: ", False, ColTypes.ListEntry) : Write(DoTranslation("Always use the translated version"), True, ColTypes.ListValue)
        Write("  -force: ", False, ColTypes.ListEntry) : Write(DoTranslation("Force switching language"), True, ColTypes.ListValue)
    End Sub

End Class
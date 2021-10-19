
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

Class ChoiceCommand
    Inherits CommandExecutor
    Implements ICommand

    Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
        Dim Titles As New List(Of String)
        Dim PressEnter As Boolean
        Dim OutputType As ChoiceOutputType = DefaultChoiceOutputType
        If ListSwitchesOnly.Contains("-multiple") Then PressEnter = True
        If ListSwitchesOnly.Contains("-single") Then PressEnter = False

        'Add the provided working titles
        If ListArgsOnly.Length > 3 Then
            Titles.AddRange(ListArgsOnly.Skip(3))
        End If

        'Check for output type switches
        If ListSwitchesOnly.Length > 0 Then
            If ListSwitchesOnly(0) = "-o" Then OutputType = ChoiceOutputType.OneLine
            If ListSwitchesOnly(0) = "-t" Then OutputType = ChoiceOutputType.TwoLines
            If ListSwitchesOnly(0) = "-m" Then OutputType = ChoiceOutputType.Modern
            If ListSwitchesOnly(0) = "-a" Then OutputType = ChoiceOutputType.Table
        End If

        'Prompt for choice
        PromptChoice(ListArgsOnly(2), ListArgsOnly(0), ListArgsOnly(1), Titles.ToArray, OutputType, PressEnter)
    End Sub

    Public Sub HelpHelper()
        Dim UsageLength As Integer = DoTranslation("Usage:").Length
        W(" ".Repeat(UsageLength) + " " + DoTranslation("where <$variable> is any variable that will be used to store response") + vbNewLine +
          " ".Repeat(UsageLength) + " " + DoTranslation("where <answers> are one-lettered answers of the question separated in slashes"), True, ColTypes.Neutral)
    End Sub

End Class
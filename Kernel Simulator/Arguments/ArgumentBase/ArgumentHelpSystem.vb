
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

Namespace Arguments.ArgumentBase
    Public Module ArgumentHelpSystem

        ''' <summary>
        ''' Shows the help of an argument, or argument list if nothing is specified
        ''' </summary>
        ''' <param name="ArgumentType">A specified argument type</param>
        Public Sub ShowArgsHelp(ArgumentType As ArgumentType)
            ShowArgsHelp("", ArgumentType)
        End Sub

        ''' <summary>
        ''' Shows the help of an argument, or argument list if nothing is specified
        ''' </summary>
        ''' <param name="Argument">A specified argument</param>
        Public Sub ShowArgsHelp(Argument As String)
            ShowArgsHelp(Argument, ArgumentType.KernelArgs)
        End Sub

        ''' <summary>
        ''' Shows the help of an argument, or argument list if nothing is specified
        ''' </summary>
        ''' <param name="Argument">A specified argument</param>
        ''' <param name="ArgumentType">A specified argument type</param>
        Public Sub ShowArgsHelp(Argument As String, ArgumentType As ArgumentType)
            'Determine argument type
            Dim ArgumentList As Dictionary(Of String, ArgumentInfo) = AvailableArgs
            Select Case ArgumentType
                Case ArgumentType.KernelArgs
                    ArgumentList = AvailableArgs
                Case ArgumentType.CommandLineArgs
                    ArgumentList = AvailableCMDLineArgs
                Case ArgumentType.PreBootCommandLineArgs
                    ArgumentList = AvailablePreBootCMDLineArgs
            End Select

            'Check to see if argument exists
            If Not String.IsNullOrWhiteSpace(Argument) And ArgumentList.ContainsKey(Argument) Then
                Dim HelpDefinition As String = ArgumentList(Argument).GetTranslatedHelpEntry
                Dim HelpUsage As String = ArgumentList(Argument).HelpUsage

                'Print usage information
                Write(DoTranslation("Usage:") + $" {Argument} {HelpUsage}: {HelpDefinition}", True, ColTypes.Neutral)

                'Extra help action for some arguments
                If ArgumentList(Argument).AdditionalHelpAction IsNot Nothing Then
                    ArgumentList(Argument).AdditionalHelpAction.DynamicInvoke()
                End If
            ElseIf String.IsNullOrWhiteSpace(Argument) Then
                'List the available arguments
                If Not SimHelp Then
                    For Each cmd As String In ArgumentList.Keys
                        Write("- {0}: ", False, ColTypes.ListEntry, cmd)
                        Write("{0}", True, ColTypes.ListValue, ArgumentList(cmd).GetTranslatedHelpEntry)
                    Next
                Else
                    For Each cmd As String In ArgumentList.Keys
                        Write("{0}, ", False, ColTypes.ListEntry, cmd)
                    Next
                End If
            Else
                Write(DoTranslation("No help for argument ""{0}""."), True, ColTypes.Error, Argument)
            End If
        End Sub

    End Module
End Namespace

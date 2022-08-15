
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

Imports KS.Files.Print

Namespace Shell.Shells.UESH.Commands
    ''' <summary>
    ''' Prints files to console.
    ''' </summary>
    ''' <remarks>
    ''' This command lets you print the contents of a text file to the console.
    ''' <br></br>
    ''' <list type="table">
    ''' <listheader>
    ''' <term>Switches</term>
    ''' <description>Description</description>
    ''' </listheader>
    ''' <item>
    ''' <term>-lines</term>
    ''' <description>Prints the line numbers alongside the contents</description>
    ''' </item>
    ''' <item>
    ''' <term>-nolines</term>
    ''' <description>Prints only the contents</description>
    ''' </item>
    ''' </list>
    ''' <br></br>
    ''' </remarks>
    Class CatCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            Try
                Dim PrintLines As Boolean = PrintLineNumbers
                If ListSwitchesOnly.Contains("-lines") Then PrintLines = True
                If ListSwitchesOnly.Contains("-nolines") Then PrintLines = False '-lines and -nolines cancel together.
                PrintContents(ListArgs(0), PrintLines)
            Catch ex As Exception
                WStkTrc(ex)
                Write(ex.Message, True, ColTypes.Error)
            End Try
        End Sub

        Public Overrides Sub HelpHelper()
            Write(DoTranslation("This command has the below switches that change how it works:"), True, ColTypes.Neutral)
            Write("  -lines: ", False, ColTypes.ListEntry) : Write(DoTranslation("Prints the line numbers that follow the line being printed"), True, ColTypes.ListValue)
            Write("  -nolines: ", False, ColTypes.ListEntry) : Write(DoTranslation("Prevents printing the line numbers"), True, ColTypes.ListValue)
        End Sub

    End Class
End Namespace

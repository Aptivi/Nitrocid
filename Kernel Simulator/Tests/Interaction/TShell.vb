
'    Kernel Simulator  Copyright (C) 2018-2019  EoflaOE
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

Module TShell

    Public TShellCmds As String() = {"print", "printf", "printd", "printdf", "testevent", "probehw", "garbage", "panic", "panicf", "translate", "places", "loadmods", "debug", "rdebug", "testmd5", "testsha256", "testregexp", "colortest", "help", "exit"}
    Sub InitTShell()
        Dim ExitFlag As Boolean
        Dim FullCmd As String
        While Not ExitFlag
            W("(t)> ", False, ColTypes.Input)
            FullCmd = Console.ReadLine
            Try
                If TShellCmds.Contains(FullCmd.Split(" ")(0)) Then
                    TParseCommand(FullCmd)
                Else
                    W(DoTranslation("Command {0} not found. See the ""help"" command for the list of commands.", currentLang), True, ColTypes.Neutral, FullCmd.Split(" ")(0))
                End If
            Catch ex As Exception
                W(DoTranslation("Error in unit testing: {0}"), True, ColTypes.Neutral, ex.Message)
                Wdbg("Error: {0}", ex.Message)
                WStkTrc(ex)
            End Try
        End While
    End Sub

End Module

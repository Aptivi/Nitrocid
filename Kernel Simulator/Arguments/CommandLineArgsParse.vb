
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

Imports System.Environment
Imports System.IO

Module CommandLineArgsParse

    Sub parseCMDArguments(ByVal arg As String)

        Try
            If (GetCommandLineArgs.Length <> 0 And availableCMDLineArgs.Contains(arg) = True) Then
                If (arg = "createConf") Then
                    Config.createConfig(True)
                ElseIf (arg = "promptArgs") Then
                    ArgumentPrompt.PromptArgs()
                    If (argsFlag = True) Then
                        ArgumentParse.ParseArguments()
                    End If
                End If
            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln("Error while parsing real command-line arguments: {0} " + vbNewLine + _
                    "{1}", "neutralText", Err.Description, ex.StackTrace) : Wdbg(ex.StackTrace, True)
            End If
        End Try

    End Sub

End Module

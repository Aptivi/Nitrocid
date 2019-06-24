
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

Module CommandLineArgsParse

    Sub ParseCMDArguments(ByVal arg As String)
        Try
            If Environment.GetCommandLineArgs.Length <> 0 And availableCMDLineArgs.Contains(arg) = True Then
                Dim argArgs As String = Environment.GetCommandLineArgs.Skip(2).ToArray.Join(" ")

                'Parse arguments
                If arg = "createConf" Then
                    If Not IO.File.Exists(paths("Configuration")) Then createConfig(True, False)
                ElseIf arg = "promptArgs" Then
                    PromptArgs()
                    If argsFlag = True Then ParseArguments()
                ElseIf arg = "testMod" Then
                    StartParse(argArgs)
                    If scripts.Count = 0 Then
                        Environment.Exit(1)
                    Else
                        Environment.Exit(0)
                    End If
                End If
            End If
        Catch ex As Exception
            Wln(DoTranslation("Error while parsing real command-line arguments: {0}", currentLang) + vbNewLine +
                "{1}", "neutralText", Err.Description, ex.StackTrace)
            If arg = "testMod" Or arg = "createConf" Then
                Environment.Exit(1)
            End If
        End Try
    End Sub

End Module

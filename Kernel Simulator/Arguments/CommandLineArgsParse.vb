
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

Module CommandLineArgsParse

    Public AvailableCMDLineArgs() As String = {"testMod", "testInteractive", "debug", "args", "help"}

    ''' <summary>
    ''' Parses the command line arguments
    ''' </summary>
    ''' <param name="arg">Argument</param>
    Sub ParseCMDArguments(ByVal arg As String)
        Try
            If Environment.GetCommandLineArgs.Length <> 0 And AvailableCMDLineArgs.Contains(arg) = True Then
                Dim argArgs As String = Environment.GetCommandLineArgs.Skip(2).ToArray.Join(" ")

                'Parse arguments
                If arg = "testMod" Then
                    StartParse(argArgs)
                    If scripts.Count = 0 Then
                        Environment.Exit(1)
                    Else
                        Environment.Exit(0)
                    End If
                ElseIf arg = "testInteractive" Then
                    InitTShell()
                    If TEST_ShutdownFlag Then Environment.Exit(0)
                ElseIf arg = "debug" Then
                    DebugMode = True
                ElseIf arg = "args" Then
                    argsOnBoot = True
                ElseIf arg = "help" Then
                    W("- testMod: ", False, ColTypes.HelpCmd) : W(DoTranslation("Tests mods by providing mod files", currentLang), True, ColTypes.HelpDef)
                    W("- testInteractive: ", False, ColTypes.HelpCmd) : W(DoTranslation("Opens a test shell", currentLang), True, ColTypes.HelpDef)
                    W("- debug: ", False, ColTypes.HelpCmd) : W(DoTranslation("Enables debug mode", currentLang), True, ColTypes.HelpDef)
                    W("- args: ", False, ColTypes.HelpCmd) : W(DoTranslation("Prompts for arguments", currentLang), True, ColTypes.HelpDef)
                    W(DoTranslation("* Press any key to start the kernel or ESC to exit.", currentLang), True, ColTypes.Neutral)
                    If Console.ReadKey(True).Key = ConsoleKey.Escape Then
                        Environment.Exit(0)
                    End If
                End If
            End If
        Catch ex As Exception
            W(DoTranslation("Error while parsing real command-line arguments: {0}", currentLang) + vbNewLine + "{1}", True, ColTypes.Err, ex.Message, ex.StackTrace)
            If arg = "testMod" Or arg = "createConf" Then
                Environment.Exit(1)
            End If
        End Try
    End Sub

End Module

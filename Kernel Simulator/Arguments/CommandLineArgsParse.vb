
'    Kernel Simulator  Copyright (C) 2018-2020  EoflaOE
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

    Public availableCMDLineArgs() As String = {"createConf", "testMod", "testInteractive", "debug", "args", 'Normal kernel arguments
                                               "CI-TestPrint", "CI-TestWdbg", "CI-TestConfig", "CI-TestInitialize", "CI-TestEval", 'CI test artifacts
                                               "CI-TestStrTrunc", "CI-TestSSEs", "CI-TestMOTD", "CI-TestMAL", "CI-TestPlace", "CI-TestCalc"} 'TODO: Add more artifacts
    Sub ParseCMDArguments(ByVal arg As String)
        Try
            If Environment.GetCommandLineArgs.Length <> 0 And availableCMDLineArgs.Contains(arg) = True Then
                Dim argArgs As String = Environment.GetCommandLineArgs.Skip(2).ToArray.Join(" ")

                'Parse arguments
                If arg = "createConf" Then
                    If Not IO.File.Exists(paths("Configuration")) Then CreateConfig(True, False)
                ElseIf arg = "testMod" Then
                    StartParse(argArgs)
                    If scripts.Count = 0 Then
                        Environment.Exit(1)
                    Else
                        Environment.Exit(0)
                    End If
                ElseIf arg = "testInteractive" Then
                    InitTShell()
                    Environment.Exit(0)
                ElseIf arg = "debug" Then
                    DebugMode = True
                ElseIf arg = "args" Then
                    argsOnBoot = True
                    '-------------- Only use these in CI Tests and Artifacts --------------
                ElseIf arg = "CI-TestPrint" Then
                    W("Test print in CI terminal output.", True, ColTypes.Neutral)
                    Environment.Exit(0)
                ElseIf arg = "CI-TestWdbg" Then 'Essentially the same as debug, but exits the kernel on successful boot with debug log writing (kernel flag)
                    DebugMode = True
                    CI_TestWdbg = True
                ElseIf arg = "CI-TestConfig" Then 'Create config, write kernelConfig.ini contents to console, then exit
                    If Not IO.File.Exists(paths("Configuration")) Then CreateConfig(True, False)
                    Dim ConfigRead() As String = IO.File.ReadAllLines(paths("Configuration"))
                    For Each Line As String In ConfigRead
                        W(Line, True, ColTypes.Neutral)
                    Next
                    Environment.Exit(0)
                ElseIf arg = "CI-TestInitialize" Then 'Sets the kernel flag so it exits on successful boot
                    CI_TestInit = True
                    CI_TestInitStopwatch.Start()
                ElseIf arg = "CI-TestEval" Then 'Tests string evaluation, then exits
                    Try
                        W("KS.Flags.CornerTD = {0}", True, ColTypes.Neutral, Evaluate("KS.Flags.CornerTD"))
                        W("KS.Color.inputColor = {0}", True, ColTypes.Neutral, Evaluate("KS.Color.inputColor"))
                        W("KS.Shell.strictCmds.Length = {0}", True, ColTypes.Neutral, Evaluate("KS.Shell.strictCmds.Length"))
                        Environment.Exit(0)
                    Catch ex As Exception
                        W(ex.Message, True, ColTypes.Neutral)
                        Environment.Exit(1)
                    End Try
                ElseIf arg = "CI-TestStrTrunc" Then
                    W(Truncate("Kernel Simulator on CI environment, text too long", 30), True, ColTypes.Neutral)
                    Environment.Exit(0)
                ElseIf arg = "CI-TestSSEs" Then
                    If EnvironmentOSType.Contains("Unix") Then
                        W("SSE: {0}, SSE2: {0}, SSE3: {0}", True, ColTypes.Neutral, CheckSSE(1), CheckSSE(2), CheckSSE(3))
                    Else
                        W("SSE: {0}, SSE2: {0}, SSE3: {0}", True, ColTypes.Neutral, CPUFeatures_Win.IsProcessorFeaturePresent(CPUFeatures_Win.SSEnum.InstructionsSSEAvailable),
                                                                                    CPUFeatures_Win.IsProcessorFeaturePresent(CPUFeatures_Win.SSEnum.InstructionsSSE2Available),
                                                                                    CPUFeatures_Win.IsProcessorFeaturePresent(CPUFeatures_Win.SSEnum.InstructionsSSE3Available))
                    End If
                    Environment.Exit(0)
                ElseIf arg = "CI-TestMOTD" Then
                    ReadMOTDFromFile(MessageType.MOTD)
                    W(MOTDMessage, True, ColTypes.Neutral)
                    Environment.Exit(0)
                ElseIf arg = "CI-TestMAL" Then
                    ReadMOTDFromFile(MessageType.MAL)
                    W(MAL, True, ColTypes.Neutral)
                    Environment.Exit(0)
                ElseIf arg = "CI-TestPlace" Then
                    W("Placeholder <system>: {0}", True, ColTypes.Neutral, ProbePlaces("<system>"))
                    W("Placeholder <timezone>: {0}", True, ColTypes.Neutral, ProbePlaces("<timezone>"))
                    W("Placeholder <shortdate>: {0}", True, ColTypes.Neutral, ProbePlaces("<shortdate>"))
                    Environment.Exit(0)
                ElseIf arg = "CI-TestCalc" Then
                    W("3 * 6 = {0}", True, ColTypes.Neutral, DoCalc("3*6").Keys(0))
                    W("16 / 4 = {0}", True, ColTypes.Neutral, DoCalc("16/4").Keys(0))
                    W("6 - 2 = {0}", True, ColTypes.Neutral, DoCalc("6-2").Keys(0))
                    Environment.Exit(0)
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

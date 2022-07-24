
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

Imports KS.Scripting.Conditions.Types

Namespace Scripting.Conditions
    Public Module UESHConditional

        Private ReadOnly Conditions As New Dictionary(Of String, BaseCondition) From {
            {"eq", New EqualsCondition()},
            {"neq", New NotEqualsCondition()},
            {"les", New LessThanCondition()},
            {"lesoreq", New LessThanOrEqualCondition()},
            {"gre", New GreaterThanCondition()},
            {"greoreq", New GreaterThanOrEqualCondition()},
            {"fileex", New FileExistsCondition()},
            {"filenex", New FileNotExistsCondition()},
            {"direx", New DirectoryExistsCondition()},
            {"dirnex", New DirectoryNotExistsCondition()},
            {"has", New ContainsCondition()},
            {"hasno", New NotContainsCondition()},
            {"ispath", New ValidPathCondition()},
            {"isnotpath", New InvalidPathCondition()},
            {"isfname", New ValidFileNameCondition()},
            {"isnotfname", New InvalidFileNameCondition()},
            {"none", New NoneCondition()}
        }

        ''' <summary>
        ''' The available condition names
        ''' </summary>
        Public ReadOnly Property AvailableConditions As Dictionary(Of String, BaseCondition)
            Get
                Return Conditions
            End Get
        End Property

        ''' <summary>
        ''' Checks if the UESH condition was satisfied
        ''' </summary>
        ''' <param name="ConditionToSatisfy">The UESH condition to satisfy</param>
        Public Function ConditionSatisfied(ConditionToSatisfy As String) As Boolean
            If Not String.IsNullOrWhiteSpace(ConditionToSatisfy) Then
                Dim Satisfied As Boolean

                'First, check for the existence of one of the conditional words
                Wdbg(DebugLevel.I, "Checking expression {0} for condition", ConditionToSatisfy)
                Dim EnclosedWords As List(Of String) = ConditionToSatisfy.SplitEncloseDoubleQuotes(" ")?.ToList
                Dim ConditionFound As Boolean
                Dim ConditionType As String = "none"
                Dim ConditionBase As BaseCondition = AvailableConditions(ConditionType)
                For Each Condition As String In AvailableConditions.Keys
                    If EnclosedWords.Contains(Condition) Then
                        Wdbg(DebugLevel.I, "Condition found in the expression string. It was {0}", Condition)
                        ConditionFound = True
                        ConditionType = Condition
                        ConditionBase = AvailableConditions(ConditionType)
                    End If
                Next
                If Not ConditionFound Then Throw New Exceptions.UESHConditionParseException(DoTranslation("The condition was not found in the expression."))

                'Check the expression for argument numbers and middle condition
                Dim RequiredArguments As Integer = ConditionBase.ConditionRequiredArguments
                Dim ConditionPosition As Integer = ConditionBase.ConditionPosition
                If EnclosedWords.Count < RequiredArguments Then
                    Wdbg(DebugLevel.E, "Argument count {0} is less than the required arguments {1}", EnclosedWords.Count, RequiredArguments)
                    Throw New Exceptions.UESHConditionParseException(DoTranslation("Condition {0} requires {1} arguments. Got {2}."), ConditionType, RequiredArguments, EnclosedWords.Count)
                End If
                If Not AvailableConditions.ContainsKey(EnclosedWords(ConditionPosition - 1)) Then
                    Wdbg(DebugLevel.E, "Condition should be in position {0}, but {1} is not a condition.", ConditionPosition, EnclosedWords(ConditionPosition - 1))
                    Throw New Exceptions.UESHConditionParseException(DoTranslation("The condition needs to be placed in the end."))
                End If

                'Execute the conditions
                Try
                    Select Case RequiredArguments
                        Case 1
                            Satisfied = ConditionBase.IsConditionSatisfied("", "")
                        Case 2
                            Dim Variable As String = ""
                            Select Case ConditionPosition
                                Case 1
                                    'Expression can be "<condition> <variable>". Since there is no middle here, assume first.
                                    Variable = EnclosedWords(1)
                                Case 2
                                    'Expression can be "<variable> <condition>"
                                    Variable = EnclosedWords(0)
                            End Select
                            Satisfied = ConditionBase.IsConditionSatisfied(Variable, "")
                        Case 3
                            Dim FirstVariable As String = ""
                            Dim SecondVariable As String = ""
                            Select Case ConditionPosition
                                Case 1
                                    'Expression can be "<condition> <variable> <variable>"
                                    FirstVariable = EnclosedWords(1)
                                    SecondVariable = EnclosedWords(2)
                                Case 2
                                    'Expression can be "<variable> <condition> <variable>"
                                    FirstVariable = EnclosedWords(0)
                                    SecondVariable = EnclosedWords(2)
                                Case 3
                                    'Expression can be "<variable> <variable> <condition>"
                                    FirstVariable = EnclosedWords(0)
                                    SecondVariable = EnclosedWords(1)
                            End Select
                            Satisfied = ConditionBase.IsConditionSatisfied(FirstVariable, SecondVariable)
                        Case Else
                            Dim Variables As String() = EnclosedWords.SkipWhile(Function(str) str = EnclosedWords(ConditionPosition - 1)).ToArray
                            Satisfied = ConditionBase.IsConditionSatisfied(Variables)
                    End Select
                    Wdbg(DebugLevel.I, "Satisfied: {0}", Satisfied)
                    Return Satisfied
                Catch ex As Exception
                    Wdbg(DebugLevel.E, "Syntax error in {0}: {1}", ConditionToSatisfy, ex.Message)
                    WStkTrc(ex)
                    Throw New Exceptions.UESHConditionParseException(DoTranslation("Error parsing expression due to syntax error.") + " {0}: {1}", ex, ConditionToSatisfy, ex.Message)
                End Try
            End If
            Return False
        End Function

    End Module
End Namespace

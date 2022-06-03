
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

Namespace Scripting
    Public Module UESHConditional

        ''' <summary>
        ''' The available condition names
        ''' </summary>
        Public ReadOnly Property AvailableConditions As String()
            Get
                Return [Enum].GetNames(GetType(UESHConditions))
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
                Dim ConditionType As UESHConditions = UESHConditions.none
                For Each Condition As String In AvailableConditions
                    If EnclosedWords.Contains(Condition) Then
                        Wdbg(DebugLevel.I, "Condition found in the expression string. It was {0}", Condition)
                        ConditionFound = True
                        ConditionType = [Enum].Parse(GetType(UESHConditions), Condition)
                    End If
                Next
                If Not ConditionFound Then Throw New Exceptions.UESHConditionParseException(DoTranslation("The condition was not found in the expression."))
                If ConditionType = UESHConditions.none Then
                    Wdbg(DebugLevel.I, "Doing nothing because the condition is none. Returning true...")
                    Return True
                End If

                'Check the expression for argument numbers and middle condition
                Dim RequiredArguments As Integer
                Dim ConditionShouldBeInMiddle As Boolean = False
                Dim ConditionShouldBeInBeginning As Boolean = False
                Select Case ConditionType
                    Case UESHConditions.direx, UESHConditions.dirnex, UESHConditions.fileex, UESHConditions.filenex
                        RequiredArguments = 2
                        ConditionShouldBeInBeginning = True
                    Case Else
                        RequiredArguments = 3
                        ConditionShouldBeInMiddle = True
                End Select
                If EnclosedWords.Count < RequiredArguments Then
                    Wdbg(DebugLevel.E, "Argument count {0} is less than the required arguments {1}", EnclosedWords.Count, RequiredArguments)
                    Throw New Exceptions.UESHConditionParseException(DoTranslation("Condition {0} requires {1} arguments. Got {2}."), ConditionType.ToString, RequiredArguments, EnclosedWords.Count)
                End If
                If ConditionShouldBeInMiddle And Not AvailableConditions.Contains(EnclosedWords(1)) Then
                    Wdbg(DebugLevel.E, "Condition should be in the middle, but {0} is not a condition.", EnclosedWords(1))
                    Throw New Exceptions.UESHConditionParseException(DoTranslation("The condition needs to be placed in the middle."))
                End If
                If ConditionShouldBeInBeginning And Not AvailableConditions.Contains(EnclosedWords(0)) Then
                    Wdbg(DebugLevel.E, "Condition should be in the beginning, but {0} is not a condition.", EnclosedWords(0))
                    Throw New Exceptions.UESHConditionParseException(DoTranslation("The condition needs to be placed in the beginning."))
                End If

                'Execute the conditions
                Try
                    Select Case ConditionType
                        Case UESHConditions.eq
                            Dim FirstVariable As String = EnclosedWords(0)
                            Dim SecondVariable As String = EnclosedWords(2)
                            Satisfied = UESHVariableEqual(FirstVariable, SecondVariable)
                        Case UESHConditions.neq
                            Dim FirstVariable As String = EnclosedWords(0)
                            Dim SecondVariable As String = EnclosedWords(2)
                            Satisfied = UESHVariableNotEqual(FirstVariable, SecondVariable)
                        Case UESHConditions.les
                            Dim FirstVariable As String = EnclosedWords(0)
                            Dim SecondVariable As String = EnclosedWords(2)
                            Satisfied = UESHVariableLessThan(FirstVariable, SecondVariable)
                        Case UESHConditions.gre
                            Dim FirstVariable As String = EnclosedWords(0)
                            Dim SecondVariable As String = EnclosedWords(2)
                            Satisfied = UESHVariableGreaterThan(FirstVariable, SecondVariable)
                        Case UESHConditions.lesoreq
                            Dim FirstVariable As String = EnclosedWords(0)
                            Dim SecondVariable As String = EnclosedWords(2)
                            Satisfied = UESHVariableLessThanOrEqual(FirstVariable, SecondVariable)
                        Case UESHConditions.greoreq
                            Dim FirstVariable As String = EnclosedWords(0)
                            Dim SecondVariable As String = EnclosedWords(2)
                            Satisfied = UESHVariableGreaterThanOrEqual(FirstVariable, SecondVariable)
                        Case UESHConditions.fileex
                            Dim Variable As String = EnclosedWords(1)
                            Satisfied = UESHVariableFileExists(Variable)
                        Case UESHConditions.filenex
                            Dim Variable As String = EnclosedWords(1)
                            Satisfied = UESHVariableFileDoesNotExist(Variable)
                        Case UESHConditions.direx
                            Dim Variable As String = EnclosedWords(1)
                            Satisfied = UESHVariableDirectoryExists(Variable)
                        Case UESHConditions.dirnex
                            Dim Variable As String = EnclosedWords(1)
                            Satisfied = UESHVariableDirectoryDoesNotExist(Variable)
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

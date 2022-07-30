
'    Kernel Simulator  Copyright (C) 2018-2019  Aptivi
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

Imports KS.Files.Querying
Imports System.IO
Imports System.Text

Namespace ManPages
    Module PageParser

        ''' <summary>
        ''' Initializes a manual page
        ''' </summary>
        ''' <param name="ManualFile">A manual file path (neutralized)</param>
        Public Sub InitMan(ManualFile As String)
            ManualFile = NeutralizePath(ManualFile)
            If FileExists(ManualFile) Then
                'File found, but we need to verify that we're actually dealing with the manual page
                If Path.GetExtension(ManualFile) = ".man" Then
                    'We found the manual, but we need to check its contents.
                    Wdbg(DebugLevel.I, "Found manual page {0}.", ManualFile)
                    Wdbg(DebugLevel.I, "Parsing manpage...")
                    Dim ManualInstance As New Manual(ManualFile)
                    AddManualPage(ManualInstance.Title, ManualInstance)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Checks to see if the manual page is valid
        ''' </summary>
        ''' <param name="ManualFile">A manual file path (neutralized)</param>
        ''' <param name="ManTitle">Manual title to install to the new manual class instance</param>
        ''' <param name="ManRevision">Manual revision to install to the new manual class instance</param>
        ''' <param name="Body">Body to install to the new manual class instance</param>
        ''' <param name="Todos">Todo list to install to the new manual class instance</param>
        Friend Function CheckManual(ManualFile As String, ByRef ManTitle As String, ByRef ManRevision As String, ByRef Body As StringBuilder, ByRef Todos As List(Of String)) As Boolean
            Dim Success As Boolean = True
            Try
                Dim InternalParseDone As Boolean = False
                ManualFile = NeutralizePath(ManualFile)
                Wdbg(DebugLevel.I, "Current manual file: {0}", ManualFile)

                'First, get all lines in the file
                Dim ManLines() As String = File.ReadAllLines(ManualFile)
                For Each ManLine As String In ManLines
                    'Check for the rest if the manpage has MAN START section
                    If InternalParseDone Then
                        Dim BodyParsing As Boolean
                        'Check for the TODOs
                        Dim TodoConstant As String = "TODO"
                        If ManLine.StartsWith("~~-") And ManLine.Contains(TodoConstant) Then
                            Wdbg(DebugLevel.I, "TODO found on this line: {0}", ManLine)
                            Todos.Add(ManLine)
                        End If

                        'Check the manual metadata
                        Dim RevisionConstant As String = "-REVISION:"
                        Dim TitleConstant As String = "-TITLE:"
                        Dim BodyStartConstant As String = "-BODY START-"
                        Dim BodyEndConstant As String = "-BODY END-"

                        'Check the body or manual metadata
                        If Not ManLine.StartsWith("~~-") Then
                            If BodyParsing Then
                                'If we're not at the end of the body
                                If ManLine <> BodyEndConstant Then
                                    If Not String.IsNullOrWhiteSpace(ManLine) Then Wdbg(DebugLevel.I, "Appending {0} to builder", ManLine)
                                    Body.AppendLine(ProbePlaces(ManLine))
                                Else
                                    'Stop parsing the body
                                    BodyParsing = False
                                End If
                            Else
                                'Check for constants
                                If ManLine.StartsWith(RevisionConstant) Then
                                    'Found the revision constant
                                    Wdbg(DebugLevel.I, "Revision found on this line: {0}", ManLine)
                                    Dim Rev As String = ManLine.Substring(RevisionConstant.Length)
                                    If String.IsNullOrWhiteSpace(Rev) Then
                                        Wdbg(DebugLevel.W, "Revision not defined. Assuming v1...")
                                        Rev = "1"
                                    End If
                                    ManRevision = Rev
                                ElseIf ManLine.StartsWith(TitleConstant) Then
                                    'Found the title constant
                                    Wdbg(DebugLevel.I, "Title found on this line: {0}", ManLine)
                                    Dim Title As String = ManLine.Substring(TitleConstant.Length)
                                    If String.IsNullOrWhiteSpace(Title) Then
                                        Wdbg(DebugLevel.W, "Title not defined.")
                                        Title = $"Untitled ({Pages.Count})"
                                    End If
                                    ManTitle = Title
                                ElseIf ManLine = "-BODY START-" Then
                                    BodyParsing = True
                                End If
                            End If
                        End If
                    End If

                    'Check to see if the manual starts with (*MAN START*) header
                    If ManLine = "(*MAN START*)" Then
                        Wdbg(DebugLevel.I, "Successfully found (*MAN START*) in manpage {0}.", ManualFile)
                        InternalParseDone = True
                    End If
                Next

                'Check for body
                If InternalParseDone Then
                    Wdbg(DebugLevel.I, "Valid manual page! ({0})", ManualFile)
                    If String.IsNullOrWhiteSpace(Body.ToString) Then
                        Wdbg(DebugLevel.W, "Body for ""{0}"" does not contain anything.", ManualFile)
                        Body.AppendLine(DoTranslation("Consider filling this manual page."))
                    End If
                Else
                    Throw New Exceptions.InvalidManpageException(DoTranslation("The manual page {0} is invalid."), ManualFile)
                End If
            Catch ex As Exception
                Success = False
                Wdbg(DebugLevel.E, "The manual page {0} is invalid. {1}", ManTitle, ex.Message)
                WStkTrc(ex)
            End Try
            Return Success
        End Function

    End Module
End Namespace
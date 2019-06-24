
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

Public Module Groups

    'Variables
    Public adminList As New Dictionary(Of String, Boolean)()         'Users that are allowed to have administrative access.
    Public disabledList As New Dictionary(Of String, Boolean)()      'Users that are unable to login

    Public Sub Permission(ByVal type As String, ByVal username As String, ByVal mode As String, Optional ByVal quiet As Boolean = False)

        'Variables
        Dim DoneFlag As Boolean = False

        'Adds user into permission lists.
        Try
            If mode = "Allow" Then
                For Each availableUsers As String In userword.Keys.ToArray
                    If username = availableUsers Then
                        If type = "Admin" Then
                            DoneFlag = True
                            adminList(username) = True
                            Wdbg("adminList.Added = {0}", adminList(username))
                            If quiet = False Then
                                Wln(DoTranslation("The user {0} has been added to the admin list.", currentLang), "neutralText", username)
                            End If
                        ElseIf type = "Disabled" Then
                            DoneFlag = True
                            disabledList(username) = True
                            Wdbg("disabledList.Added = {0}", disabledList(username))
                            If quiet = False Then
                                Wln(DoTranslation("The user {0} has been added to the disabled list.", currentLang), "neutralText", username)
                            End If
                        Else
                            If quiet = False Then
                                Wln(DoTranslation("Failed to add user into permission lists: invalid type {0}", currentLang), "neutralText", type)
                            End If
                            Exit Sub
                        End If
                    End If
                Next
                If DoneFlag = False And quiet = False Then
                    Wdbg("ASSERT(isFound({0})) = False", username)
                    Wln(DoTranslation("Failed to add user into permission lists: invalid user {0}", currentLang), "neutralText", username)
                End If
            ElseIf mode = "Disallow" Then
                For Each availableUsers As String In userword.Keys.ToArray
                    If username = availableUsers And username <> signedinusrnm Then
                        If type = "Admin" Then
                            DoneFlag = True
                            Wdbg("adminList.ToBeRemoved = {0}", username)
                            adminList(username) = False
                            If quiet = False Then
                                Wln(DoTranslation("The user {0} has been removed from the admin list.", currentLang), "neutralText", username)
                            End If
                        ElseIf type = "Disabled" Then
                            DoneFlag = True
                            Wdbg("disabledList.ToBeRemoved = {0}", username)
                            disabledList(username) = False
                            If quiet = False Then
                                Wln(DoTranslation("The user {0} has been removed from the disabled list.", currentLang), "neutralText", username)
                            End If
                        Else
                            If quiet = False Then
                                Wln(DoTranslation("Failed to remove user from permission lists: invalid type {0}", currentLang), "neutralText", type)
                            End If
                            Exit Sub
                        End If
                    ElseIf username = signedinusrnm Then
                        Wln(DoTranslation("You are already logged in.", currentLang), "neutralText")
                        Exit Sub
                    End If
                Next
                If DoneFlag = False And quiet = False Then
                    Wdbg("ASSERT(isFound({0})) = False", username)
                    Wln(DoTranslation("Failed to remove user from permission lists: invalid user {0}", currentLang), "neutralText", username)
                End If
            Else
                If quiet = False Then
                    Wln(DoTranslation("You have found a bug in the permission system: invalid mode {0}", currentLang), "neutralText", mode)
                End If
            End If
        Catch ex As Exception
            If DebugMode = True Then
                Wln(DoTranslation("You have either found a bug, or the permission you tried to add or remove is already done, or other error.", currentLang) + vbNewLine +
                    DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", "neutralText", Err.Number, Err.Description, ex.StackTrace)
                Wdbg(ex.StackTrace, True)
            Else
                Wln(DoTranslation("You have either found a bug, or the permission you tried to add or remove is already done, or other error.", currentLang) + vbNewLine +
                    DoTranslation("Error {0}: {1}", currentLang), "neutralText", Err.Number, Err.Description)
            End If
        End Try

    End Sub

    Public Sub PermissionEditForNewUser(ByVal oldName As String, ByVal username As String)

        'Edit username (continuation for changeName() sub)
        Try
            If adminList.ContainsKey(oldName) = True And disabledList.ContainsKey(oldName) = True Then
                Dim temporary1 As Boolean = adminList(oldName)
                Dim temporary2 As Boolean = disabledList(oldName)
                Wdbg("adminList.ToBeRemoved = {0}", String.Join(", ", oldName))
                Wdbg("disabledList.ToBeRemoved = {0}", String.Join(", ", oldName))
                adminList.Remove(oldName)
                disabledList.Remove(oldName)
                adminList.Add(username, temporary1)
                disabledList.Add(username, temporary2)
                Wdbg("adminList.Added = {0}", adminList(username))
                Wdbg("disabledList.Added = {0}", disabledList(username))
            End If
        Catch ex As Exception
            If DebugMode = True Then
                Wln(DoTranslation("You have either found a bug, or the permission you tried to edit for a new user has failed.", currentLang) + vbNewLine +
                    DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", "neutralText", Err.Number, Err.Description, ex.StackTrace)
                Wdbg(ex.StackTrace, True)
            Else
                Wln(DoTranslation("You have either found a bug, or the permission you tried to edit for a new user has failed.", currentLang) + vbNewLine +
                    DoTranslation("Error {0}: {1}", currentLang), "neutralText", Err.Number, Err.Description)
            End If
        End Try

    End Sub

End Module


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

Module Groups

    'Variables
    Public adminList As New Dictionary(Of String, Boolean)()         'Users that are allowed to have administrative access.
    Public disabledList As New Dictionary(Of String, Boolean)()      'Users that are unable to login

    Sub permission(ByVal type As String, ByVal username As String, ByVal mode As String, Optional ByVal quiet As Boolean = False)

        'Variables
        Dim DoneFlag As Boolean = False

        'Adds user into permission lists.
        Try
            If (mode = "Allow") Then
                For Each availableUsers As String In userword.Keys.ToArray
                    If (username = availableUsers) Then
                        If (type = "Admin") Then
                            DoneFlag = True
                            adminList(username) = True
                            Wdbg("adminList.Added = {0}", True, adminList(username))
                            If (quiet = False) Then
                                Wln("The user {0} has been added to the admin list.", "neutralText", username)
                            End If
                        ElseIf (type = "Disabled") Then
                            DoneFlag = True
                            disabledList(username) = True
                            Wdbg("disabledList.Added = {0}", True, disabledList(username))
                            If (quiet = False) Then
                                Wln("The user {0} has been added to the disabled list.", "neutralText", username)
                            End If
                        Else
                            If (quiet = False) Then
                                Wln("Failed to add user into permission lists: invalid type {0}", "neutralText", type)
                            End If
                            Exit Sub
                        End If
                    End If
                Next
                If (DoneFlag = False And quiet = False) Then
                    Wdbg("ASSERT(isFound({0})) = False", True, username)
                    Wln("Failed to add user into permission lists: invalid user {0}", "neutralText", username)
                End If
            ElseIf (mode = "Disallow") Then
                For Each availableUsers As String In userword.Keys.ToArray
                    If (username = availableUsers And username <> signedinusrnm) Then
                        If (type = "Admin") Then
                            DoneFlag = True
                            Wdbg("adminList.ToBeRemoved = {0}", True, username)
                            adminList(username) = False
                            If (quiet = False) Then
                                Wln("The user {0} has been removed from the admin list.", "neutralText", username)
                            End If
                        ElseIf (type = "Disabled") Then
                            DoneFlag = True
                            Wdbg("disabledList.ToBeRemoved = {0}", True, username)
                            disabledList(username) = False
                            If (quiet = False) Then
                                Wln("The user {0} has been removed from the disabled list.", "neutralText", username)
                            End If
                        Else
                            If (quiet = False) Then
                                Wln("Failed to remove user from permission lists: invalid type {0}", "neutralText", type)
                            End If
                            Exit Sub
                        End If
                    ElseIf (username = signedinusrnm) Then
                        Wln("You are already logged in.", "neutralText")
                        Exit Sub
                    End If
                Next
                If (DoneFlag = False And quiet = False) Then
                    Wdbg("ASSERT(isFound({0})) = False", True, username)
                    Wln("Failed to remove user from permission lists: invalid user {0}", "neutralText", username)
                End If
            Else
                If (quiet = False) Then
                    Wln("You have found a bug in the permission system: invalid mode {0}", "neutralText", mode)
                End If
            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln("You have either found a bug, or the permission you tried to add or remove is already done, or other error." + vbNewLine + _
                    "Error {0}: {1}" + vbNewLine + "{2}", "neutralText", Err.Number, Err.Description, ex.StackTrace)
                Wdbg(ex.StackTrace, True)
            Else
                Wln("You have either found a bug, or the permission you tried to add or remove is already done, or other error." + vbNewLine + _
                    "Error {0}: {1}", "neutralText", Err.Number, Err.Description)
            End If
        End Try

    End Sub

    Sub permissionEditForNewUser(ByVal oldName As String, ByVal username As String)

        'Edit username (continuation for changeName() sub)
        Try
            If (adminList.ContainsKey(oldName) = True And disabledList.ContainsKey(oldName) = True) Then
                Dim temporary1 As Boolean = adminList(oldName)
                Dim temporary2 As Boolean = disabledList(oldName)
                Wdbg("adminList.ToBeRemoved = {0}", True, String.Join(", ", oldName))
                Wdbg("disabledList.ToBeRemoved = {0}", True, String.Join(", ", oldName))
                adminList.Remove(oldName)
                disabledList.Remove(oldName)
                adminList.Add(username, temporary1)
                disabledList.Add(username, temporary2)
                Wdbg("adminList.Added = {0}", True, adminList(username))
                Wdbg("disabledList.Added = {0}", True, disabledList(username))
            End If
        Catch ex As Exception
            If (DebugMode = True) Then
                Wln("You have either found a bug, or the permission you tried to edit for a new user has failed." + vbNewLine + _
                    "Error {0}: {1}" + vbNewLine + "{2}", "neutralText", Err.Number, Err.Description, ex.StackTrace)
                Wdbg(ex.StackTrace, True)
            Else
                Wln("You have either found a bug, or the permission you tried to edit for a new user has failed." + vbNewLine + _
                    "Error {0}: {1}", "neutralText", Err.Number, Err.Description)
            End If
        End Try

    End Sub

    Sub permissionPrompt()

        'Variables
        Dim DoneFlag As Boolean = False
        On Error Resume Next

        'Asks for username, and then permission prompts.
        W("Username to be managed: ", "input")
        Dim answermuser = System.Console.ReadLine()
        If (answermuser = "q") Then
            Exit Sub
        Else
            For Each userPerm As String In userword.Keys.ToArray
                If (userPerm = answermuser And answermuser <> "root") Then
                    W("Type (Admin / Disabled): ", "input")
                    Dim answermtype = System.Console.ReadLine()
                    If (answermtype = "Admin" Or answermtype = "Disabled") Then
                        W("Add or remove? <Add/Remove> ", "input")
                        Dim answermaddremove = System.Console.ReadLine()
                        If (answermaddremove = "Add" Or answermaddremove = "Remove") Then
                            permission(answermtype, True, answermuser, answermaddremove)
                        Else
                            Wln("Type {0} not found.", "neutralText", answermtype)
                        End If
                    Else
                        Wln("Invalid type {0}.", "neutralText", answermtype)
                        Exit Sub
                    End If
                ElseIf (userPerm = answermuser And answermuser = "root") Then
                    Wln("User {0}'s permission cannot be changed.", "neutralText", answermuser)
                    Exit Sub
                End If
            Next
            If (DoneFlag = False) Then
                Wln("User {0} not found.", "neutralText", answermuser)
            End If
        End If


    End Sub

    Sub permissionEditingPrompt()

        'Variables
        Dim DoneFlag As Boolean = False
        On Error Resume Next

        'Asks for username, and then permission prompts.
        W("Username to be managed: ", "input")
        Dim answermuser = System.Console.ReadLine()
        If (answermuser = "q") Then
            Exit Sub
        Else
            For Each userPerm As String In userword.Keys.ToArray
                If (userPerm = answermuser And answermuser <> "root") Then
                    W("Type (Admin / Disabled): ", "input")
                    Dim answermtype = System.Console.ReadLine()
                    If (answermtype = "Admin" Or answermtype = "Disabled") Then
                        W("Is the user allowed? <y/n> ", "input")
                        Dim answermallow = System.Console.ReadLine()
                        If (answermallow = "y") Then
                            permission(answermtype, answermuser, "Allow")
                            DoneFlag = True
                        ElseIf (answermallow = "n") Then
                            permission(answermtype, answermuser, "Disallow")
                            DoneFlag = True
                        Else
                            Wln("Invalid choice", "neutralText")
                        End If
                    Else
                        Wln("Type {0} not found.", "neutralText", answermtype)
                    End If
                ElseIf (userPerm = answermuser And answermuser = "root") Then
                    Wln("User {0}'s permission cannot be changed.", "neutralText", answermuser)
                    Exit Sub
                ElseIf (userPerm = "q") Then
                    Exit Sub
                End If
            Next
            If (DoneFlag = False) Then
                Wln("User {0} not found.", "neutralText", answermuser)
            End If
        End If


    End Sub

End Module

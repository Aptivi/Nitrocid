
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

Imports System.Security.Cryptography
Imports System.Text

Public Module UserManagement

    'Variables
    Public adminList As New Dictionary(Of String, Boolean)()         'Users that are allowed to have administrative access.
    Public disabledList As New Dictionary(Of String, Boolean)()      'Users that are unable to login

    '---------- User Management ----------
    Public Sub InitializeUser(ByVal uninitUser As String, Optional ByVal unpassword As String = "")

        Try
            Dim hashbyte As Byte() = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(unpassword))
            unpassword = GetArrayEnc(hashbyte)
            userword.Add(uninitUser, unpassword)
            Wdbg("Username {0} added. Readying permissions...", uninitUser)
            adminList.Add(uninitUser, False)
            disabledList.Add(uninitUser, False)
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("Error trying to add username.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, ColTypes.Neutral, Err.Number, Err.Description, ex.StackTrace)
                WStkTrc(ex)
            Else
                W(DoTranslation("Error trying to add username.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang), True, ColTypes.Neutral, Err.Number, Err.Description)
            End If
        End Try

    End Sub

    Public Sub Adduser(ByVal newUser As String, Optional ByVal newPassword As String = "")

        'Adds users
        W(DoTranslation("usrmgr: Creating username {0}...", currentLang), True, ColTypes.Neutral, newUser)
        If Not userword.ContainsKey(newUser) Then
            If newPassword = Nothing Then
                InitializeUser(newUser)
            Else
                InitializeUser(newUser, newPassword)
            End If
        Else
            W(DoTranslation("usrmgr: Username {0} is already found", currentLang), True, ColTypes.Neutral, newUser)
        End If

    End Sub

    'This sub is an accomplice of in-shell command arguments.
    Public Sub RemoveUserFromDatabase(ByVal user As String)

        Try
            Dim DoneFlag As String = "No"
            If InStr(user, " ") > 0 Then
                W(DoTranslation("Spaces are not allowed.", currentLang), True, ColTypes.Neutral)
            ElseIf user = "q" Then
                DoneFlag = "Cancelled"
            ElseIf user.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
                W(DoTranslation("Special characters are not allowed.", currentLang), True, ColTypes.Neutral)
            ElseIf user = Nothing Then
                W(DoTranslation("Blank username.", currentLang), True, ColTypes.Neutral)
            ElseIf userword.ContainsKey(user) = False Then
                Wdbg("ASSERT(isFound({0})) = False", user)
                W(DoTranslation("User {0} not found.", currentLang), True, ColTypes.Neutral, user)
            Else
                For Each usersRemove As String In userword.Keys.ToArray
                    If usersRemove = user And user = "root" Then
                        W(DoTranslation("User {0} isn't allowed to be removed.", currentLang), True, ColTypes.Neutral, user)
                    ElseIf user = usersRemove And usersRemove = signedinusrnm Then
                        W(DoTranslation("User {0} is already logged in. Log-out and log-in as another admin.", currentLang), True, ColTypes.Neutral, user)
                        Wdbg("ASSERT({0}.isLoggedIn(ASSERT({0} = {1}) = True)) = True", user, signedinusrnm)
                    ElseIf usersRemove = user And user <> "root" Then
                        adminList.Remove(user)
                        disabledList.Remove(user)
                        Wdbg("userword.ToBeRemoved = {0}", String.Join(", ", userword(user).ToArray))
                        userword.Remove(user)
                        W(DoTranslation("User {0} removed.", currentLang), True, ColTypes.Neutral, user)
                        DoneFlag = "Yes"
                    End If
                Next
            End If
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("Error trying to remove username.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, ColTypes.Neutral, Err.Number, Err.Description, ex.StackTrace)
                WStkTrc(ex)
            Else
                W(DoTranslation("Error trying to remove username.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang), True, ColTypes.Neutral, Err.Number, Err.Description)
            End If
        End Try
    End Sub

    '---------- Previously on Groups.vb ----------
    Public Sub Permission(ByVal type As String, ByVal username As String, ByVal mode As String)

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
                            W(DoTranslation("The user {0} has been added to the admin list.", currentLang), True, ColTypes.Neutral, username)
                        ElseIf type = "Disabled" Then
                            DoneFlag = True
                            disabledList(username) = True
                            Wdbg("disabledList.Added = {0}", disabledList(username))
                            W(DoTranslation("The user {0} has been added to the disabled list.", currentLang), True, ColTypes.Neutral, username)
                        Else
                            W(DoTranslation("Failed to add user into permission lists: invalid type {0}", currentLang), True, ColTypes.Neutral, type)
                            Exit Sub
                        End If
                    End If
                Next
                If DoneFlag = False Then
                    Wdbg("ASSERT(isFound({0})) = False", username)
                    W(DoTranslation("Failed to add user into permission lists: invalid user {0}", currentLang), True, ColTypes.Neutral, username)
                End If
            ElseIf mode = "Disallow" Then
                For Each availableUsers As String In userword.Keys.ToArray
                    If username = availableUsers And username <> signedinusrnm Then
                        If type = "Admin" Then
                            DoneFlag = True
                            Wdbg("adminList.ToBeRemoved = {0}", username)
                            adminList(username) = False
                            W(DoTranslation("The user {0} has been removed from the admin list.", currentLang), True, ColTypes.Neutral, username)
                        ElseIf type = "Disabled" Then
                            DoneFlag = True
                            Wdbg("disabledList.ToBeRemoved = {0}", username)
                            disabledList(username) = False
                            W(DoTranslation("The user {0} has been removed from the disabled list.", currentLang), True, ColTypes.Neutral, username)
                        Else
                            W(DoTranslation("Failed to remove user from permission lists: invalid type {0}", currentLang), True, ColTypes.Neutral, type)
                            Exit Sub
                        End If
                    ElseIf username = signedinusrnm Then
                        W(DoTranslation("You are already logged in.", currentLang), True, ColTypes.Neutral)
                        Exit Sub
                    End If
                Next
                If Not DoneFlag Then
                    Wdbg("ASSERT(isFound({0})) = False", username)
                    W(DoTranslation("Failed to remove user from permission lists: invalid user {0}", currentLang), True, ColTypes.Neutral, username)
                End If
            Else
                W(DoTranslation("You have found a bug in the permission system: invalid mode {0}", currentLang), True, ColTypes.Neutral, mode)
            End If
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("You have either found a bug, or the permission you tried to add or remove is already done, or other error.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, ColTypes.Neutral, Err.Number, Err.Description, ex.StackTrace)
                WStkTrc(ex)
            Else
                W(DoTranslation("You have either found a bug, or the permission you tried to add or remove is already done, or other error.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang), True, ColTypes.Neutral, Err.Number, Err.Description)
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
                W(DoTranslation("You have either found a bug, or the permission you tried to edit for a new user has failed.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, ColTypes.Neutral, Err.Number, Err.Description, ex.StackTrace)
                WStkTrc(ex)
            Else
                W(DoTranslation("You have either found a bug, or the permission you tried to edit for a new user has failed.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang), True, ColTypes.Neutral, Err.Number, Err.Description)
            End If
        End Try

    End Sub

End Module

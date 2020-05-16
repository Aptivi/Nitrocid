
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

Imports System.IO
Imports System.Text.RegularExpressions

Public Module UserManagement

    'Variables
    Public adminList As New Dictionary(Of String, Boolean)()         'Users that are allowed to have administrative access.
    Public disabledList As New Dictionary(Of String, Boolean)()      'Users that are unable to login
    Public UsersWriter As StreamWriter

    '---------- User Management ----------
    Public Sub InitializeUser(ByVal uninitUser As String, Optional ByVal unpassword As String = "", Optional ByVal ComputationNeeded As Boolean = True)

        Try
            'Compute hash of a password
            Dim Regexp As New Regex("^([a-fA-F0-9]{64})$")
            If ComputationNeeded Then
                unpassword = GetEncryptedString(unpassword, Algorithms.SHA256)
                Wdbg("I", "Hash computed.")
            ElseIf Not Regexp.IsMatch(unpassword) Then
                Throw New InvalidOperationException("Trying to add unencrypted password to users list. That won't work properly, since login relies on encrypted passwords.")
            End If

            'Add user
            If Not File.Exists(paths("Users")) Then File.Create(paths("Users")).Close()
            Dim UsersLines As List(Of String) = File.ReadAllLines(paths("Users")).ToList
            If Not userword.ContainsKey(uninitUser) Then userword.Add(uninitUser, unpassword)
            UsersWriter = New StreamWriter(paths("Users"), True) With {.AutoFlush = True}
            If Not UsersLines.Contains(uninitUser + "," + unpassword) Then UsersWriter.WriteLine(uninitUser + "," + unpassword)
            UsersWriter.Close() : UsersWriter.Dispose()

            'Ready permissions
            Wdbg("I", "Username {0} added. Readying permissions...", uninitUser)
            If Not adminList.ContainsKey(uninitUser) Then adminList.Add(uninitUser, False)
            If Not disabledList.ContainsKey(uninitUser) Then disabledList.Add(uninitUser, False)
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("Error trying to add username.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, ColTypes.Err, Err.Number, ex.Message, ex.StackTrace)
                WStkTrc(ex)
            Else
                W(DoTranslation("Error trying to add username.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang), True, ColTypes.Err, Err.Number, ex.Message)
            End If
        End Try

    End Sub

    'This time, this sub is different from first versions of KS. It reads the user file and adds them to the list.
    Public Sub InitializeUsers()
        'Opens file stream
        Dim UsersLines As List(Of String) = File.ReadAllLines(paths("Users")).ToList
        For Each Line As String In UsersLines
            InitializeUser(Line.Remove(Line.IndexOf(",")), Line.Substring(Line.IndexOf(",") + 1), False)
        Next
    End Sub

    Public Sub AddUser(ByVal newUser As String, Optional ByVal newPassword As String = "")

        'Adds user
        W(DoTranslation("usrmgr: Creating username {0}...", currentLang), True, ColTypes.Neutral, newUser)
        Wdbg("I", "Creating user {0}...", newUser)
        If Not userword.ContainsKey(newUser) Then
            If newPassword = Nothing Then
                Wdbg("W", "Initializing user with no password")
                InitializeUser(newUser)
            Else
                Wdbg("I", "Initializing user with password")
                InitializeUser(newUser, newPassword)
            End If
        Else
            Wdbg("I", "User {0} already found.", newUser)
            W(DoTranslation("usrmgr: Username {0} is already found", currentLang), True, ColTypes.Err, newUser)
        End If

    End Sub

    'This sub is an accomplice of in-shell command arguments.
    Public Sub RemoveUserFromDatabase(ByVal user As String)
        Try
            If InStr(user, " ") > 0 Then
                Wdbg("W", "There are spaces in username.")
                W(DoTranslation("Spaces are not allowed.", currentLang), True, ColTypes.Err)
            ElseIf user.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
                Wdbg("W", "There are special characters in username.")
                W(DoTranslation("Special characters are not allowed.", currentLang), True, ColTypes.Err)
            ElseIf user = Nothing Then
                Wdbg("W", "Username is blank.")
                W(DoTranslation("Blank username.", currentLang), True, ColTypes.Err)
            ElseIf userword.ContainsKey(user) = False Then
                Wdbg("W", "Username {0} not found in list", user)
                W(DoTranslation("User {0} not found.", currentLang), True, ColTypes.Err, user)
            Else
                'Try to remove user
                If userword.Keys.ToArray.Contains(user) And user = "root" Then
                    Wdbg("W", "User is root, and is a system account")
                    W(DoTranslation("User {0} isn't allowed to be removed.", currentLang), True, ColTypes.Err, user)
                ElseIf userword.Keys.ToArray.Contains(user) And user = signedinusrnm Then
                    Wdbg("W", "User has logged in, so can't delete self.")
                    W(DoTranslation("User {0} is already logged in. Log-out and log-in as another admin.", currentLang), True, ColTypes.Err, user)
                ElseIf userword.Keys.ToArray.Contains(user) And user <> "root" Then
                    Wdbg("I", "Removing permissions...")
                    adminList.Remove(user)
                    disabledList.Remove(user)

                    'Remove user
                    Wdbg("I", "userword.ToBeRemoved = {0}", user)
                    userword.Remove(user)

                    'Remove user from users.csv
                    Dim UsersLines As List(Of String) = File.ReadAllLines(paths("Users")).ToList
                    For i As Integer = 0 To UsersLines.Count - 1
                        If UsersLines(i).StartsWith($"{user},") Then
                            UsersLines.RemoveAt(i)
                            Exit For
                        End If
                    Next
                    File.WriteAllLines(paths("Users"), UsersLines)
                    W(DoTranslation("User {0} removed.", currentLang), True, ColTypes.Neutral, user)
                End If
            End If
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("Error trying to remove username.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, ColTypes.Err, Err.Number, ex.Message, ex.StackTrace)
                WStkTrc(ex)
            Else
                W(DoTranslation("Error trying to remove username.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang), True, ColTypes.Err, Err.Number, ex.Message)
            End If
        End Try
    End Sub

    '---------- Previously on Groups.vb ----------
    Public Sub Permission(ByVal type As String, ByVal username As String, ByVal mode As String)

        'Adds user into permission lists.
        Try
            Wdbg("I", "Mode: {0}", mode)
            If mode = "Allow" Then
                If userword.Keys.ToArray.Contains(username) Then
                    Wdbg("I", "Type is {0}", type)
                    If type = "Admin" Then
                        adminList(username) = True
                        Wdbg("I", "User {0} allowed (Admin): {1}", username, adminList(username))
                        W(DoTranslation("The user {0} has been added to the admin list.", currentLang), True, ColTypes.Neutral, username)
                    ElseIf type = "Disabled" Then
                        disabledList(username) = True
                        Wdbg("I", "User {0} allowed (Disabled): {1}", username, disabledList(username))
                        W(DoTranslation("The user {0} has been added to the disabled list.", currentLang), True, ColTypes.Neutral, username)
                    Else
                        Wdbg("W", "Type is invalid")
                        W(DoTranslation("Failed to add user into permission lists: invalid type {0}", currentLang), True, ColTypes.Err, type)
                        Exit Sub
                    End If
                Else
                    Wdbg("W", "User {0} not found on list", username)
                    W(DoTranslation("Failed to add user into permission lists: invalid user {0}", currentLang), True, ColTypes.Err, username)
                End If
            ElseIf mode = "Disallow" Then
                If userword.Keys.ToArray.Contains(username) And username <> signedinusrnm Then
                    Wdbg("I", "Type is {0}", type)
                    If type = "Admin" Then
                        Wdbg("I", "User {0} allowed (Admin): {1}", username, adminList(username))
                        adminList(username) = False
                        W(DoTranslation("The user {0} has been removed from the admin list.", currentLang), True, ColTypes.Neutral, username)
                    ElseIf type = "Disabled" Then
                        Wdbg("I", "User {0} allowed (Disabled): {1}", username, disabledList(username))
                        disabledList(username) = False
                        W(DoTranslation("The user {0} has been removed from the disabled list.", currentLang), True, ColTypes.Neutral, username)
                    Else
                        Wdbg("W", "Type is invalid")
                        W(DoTranslation("Failed to remove user from permission lists: invalid type {0}", currentLang), True, ColTypes.Err, type)
                        Exit Sub
                    End If
                ElseIf username = signedinusrnm Then
                    W(DoTranslation("You are already logged in.", currentLang), True, ColTypes.Err)
                    Exit Sub
                Else
                    Wdbg("W", "User {0} not found on list", username)
                    W(DoTranslation("Failed to remove user from permission lists: invalid user {0}", currentLang), True, ColTypes.Err, username)
                End If
            Else
                Wdbg("W", "Mode is invalid")
                W(DoTranslation("You have found a bug in the permission system: invalid mode {0}", currentLang), True, ColTypes.Err, mode)
            End If
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("You have either found a bug, or the permission you tried to add or remove is already done, or other error.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, ColTypes.Err, Err.Number, ex.Message, ex.StackTrace)
                WStkTrc(ex)
            Else
                W(DoTranslation("You have either found a bug, or the permission you tried to add or remove is already done, or other error.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang), True, ColTypes.Err, Err.Number, ex.Message)
            End If
        End Try

    End Sub

    Public Sub PermissionEditForNewUser(ByVal oldName As String, ByVal username As String)

        'Edit username (continuation for changeName() sub)
        Try
            If adminList.ContainsKey(oldName) = True And disabledList.ContainsKey(oldName) = True Then
                'Store permissions
                Dim temporary1 As Boolean = adminList(oldName)
                Dim temporary2 As Boolean = disabledList(oldName)

                'Remove old user entry
                Wdbg("I", "Removing {0} from Admin List", oldName)
                adminList.Remove(oldName)
                Wdbg("I", "Removing {0} from Disabled List", oldName)
                disabledList.Remove(oldName)

                'Add new user entry
                adminList.Add(username, temporary1)
                Wdbg("I", "Added {0} to Admin List with value of {1}", username, temporary1)
                disabledList.Add(username, temporary2)
                Wdbg("I", "Added {0} to Disabled List with value of {1}", username, temporary2)
            End If
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("You have either found a bug, or the permission you tried to edit for a new user has failed.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang) + vbNewLine + "{2}", True, ColTypes.Err, Err.Number, ex.Message, ex.StackTrace)
                WStkTrc(ex)
            Else
                W(DoTranslation("You have either found a bug, or the permission you tried to edit for a new user has failed.", currentLang) + vbNewLine +
                  DoTranslation("Error {0}: {1}", currentLang), True, ColTypes.Err, Err.Number, ex.Message)
            End If
        End Try

    End Sub

End Module

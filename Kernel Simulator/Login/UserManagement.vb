
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
    ''' <summary>
    ''' Initializes the uninitialized user (usually a new user)
    ''' </summary>
    ''' <param name="uninitUser">A new user</param>
    ''' <param name="unpassword">A password of a user in encrypted form</param>
    ''' <param name="ComputationNeeded">Whether or not a password encryption is needed</param>
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

    ''' <summary>
    ''' Reads the user file and adds them to the list.
    ''' </summary>
    Public Sub InitializeUsers()
        'Opens file stream
        Dim UsersLines As List(Of String) = File.ReadAllLines(paths("Users")).ToList
        For Each Line As String In UsersLines
            InitializeUser(Line.Remove(Line.IndexOf(",")), Line.Substring(Line.IndexOf(",") + 1), False)
        Next
    End Sub

    ''' <summary>
    ''' Adds a new user
    ''' </summary>
    ''' <param name="newUser">A new user</param>
    ''' <param name="newPassword">A password</param>
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

    ''' <summary>
    ''' Removes a user from users database
    ''' </summary>
    ''' <param name="user">A user</param>
    ''' <remarks>This sub is an accomplice of in-shell command arguments.</remarks>
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
    ''' <summary>
    ''' This enumeration lists all permission types.
    ''' </summary>
    Public Enum PermissionType As Integer
        Administrator = 1
        Disabled
    End Enum

    ''' <summary>
    ''' It specifies whether or not to allow permission
    ''' </summary>
    Public Enum PermissionManagementMode As Integer
        Allow = 1
        Disallow
    End Enum

    ''' <summary>
    ''' Manages permissions
    ''' </summary>
    ''' <param name="PermType">A type of permission</param>
    ''' <param name="Username">A specified username</param>
    ''' <param name="PermissionMode">Whether to allow or disallow a specified type for a user</param>
    Public Sub Permission(ByVal PermType As PermissionType, ByVal Username As String, ByVal PermissionMode As PermissionManagementMode)

        'Adds user into permission lists.
        Try
            Wdbg("I", "Mode: {0}", PermissionMode)
            If PermissionMode = PermissionManagementMode.Allow Then
                If userword.Keys.ToArray.Contains(Username) Then
                    Wdbg("I", "Type is {0}", PermType)
                    If PermType = PermissionType.Administrator Then
                        adminList(Username) = True
                        Wdbg("I", "User {0} allowed (Admin): {1}", Username, adminList(Username))
                        W(DoTranslation("The user {0} has been added to the admin list.", currentLang), True, ColTypes.Neutral, Username)
                    ElseIf PermType = PermissionType.Disabled Then
                        disabledList(Username) = True
                        Wdbg("I", "User {0} allowed (Disabled): {1}", Username, disabledList(Username))
                        W(DoTranslation("The user {0} has been added to the disabled list.", currentLang), True, ColTypes.Neutral, Username)
                    Else
                        Wdbg("W", "Type is invalid")
                        W(DoTranslation("Failed to add user into permission lists: invalid type {0}", currentLang), True, ColTypes.Err, PermType)
                        Exit Sub
                    End If
                Else
                    Wdbg("W", "User {0} not found on list", Username)
                    W(DoTranslation("Failed to add user into permission lists: invalid user {0}", currentLang), True, ColTypes.Err, Username)
                End If
            ElseIf PermissionMode = PermissionManagementMode.Disallow Then
                If userword.Keys.ToArray.Contains(Username) And Username <> signedinusrnm Then
                    Wdbg("I", "Type is {0}", PermType)
                    If PermType = PermissionType.Administrator Then
                        Wdbg("I", "User {0} allowed (Admin): {1}", Username, adminList(Username))
                        adminList(Username) = False
                        W(DoTranslation("The user {0} has been removed from the admin list.", currentLang), True, ColTypes.Neutral, Username)
                    ElseIf PermType = PermissionType.Disabled Then
                        Wdbg("I", "User {0} allowed (Disabled): {1}", Username, disabledList(Username))
                        disabledList(Username) = False
                        W(DoTranslation("The user {0} has been removed from the disabled list.", currentLang), True, ColTypes.Neutral, Username)
                    Else
                        Wdbg("W", "Type is invalid")
                        W(DoTranslation("Failed to remove user from permission lists: invalid type {0}", currentLang), True, ColTypes.Err, PermType)
                        Exit Sub
                    End If
                ElseIf Username = signedinusrnm Then
                    W(DoTranslation("You are already logged in.", currentLang), True, ColTypes.Err)
                    Exit Sub
                Else
                    Wdbg("W", "User {0} not found on list", Username)
                    W(DoTranslation("Failed to remove user from permission lists: invalid user {0}", currentLang), True, ColTypes.Err, Username)
                End If
            Else
                Wdbg("W", "Mode is invalid")
                W(DoTranslation("You have found a bug in the permission system: invalid mode {0}", currentLang), True, ColTypes.Err, PermissionMode)
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

    ''' <summary>
    ''' Edits the permission database for new user name
    ''' </summary>
    ''' <param name="OldName">Old username</param>
    ''' <param name="Username">New username</param>
    Public Sub PermissionEditForNewUser(ByVal OldName As String, ByVal Username As String)

        'Edit username (continuation for changeName() sub)
        Try
            If adminList.ContainsKey(OldName) = True And disabledList.ContainsKey(OldName) = True Then
                'Store permissions
                Dim Temporary1 As Boolean = adminList(OldName)
                Dim Temporary2 As Boolean = disabledList(OldName)

                'Remove old user entry
                Wdbg("I", "Removing {0} from Admin List", OldName)
                adminList.Remove(OldName)
                Wdbg("I", "Removing {0} from Disabled List", OldName)
                disabledList.Remove(OldName)

                'Add new user entry
                adminList.Add(Username, Temporary1)
                Wdbg("I", "Added {0} to Admin List with value of {1}", Username, Temporary1)
                disabledList.Add(Username, Temporary2)
                Wdbg("I", "Added {0} to Disabled List with value of {1}", Username, Temporary2)
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


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
    ''' <returns>True if successful; False if successful</returns>
    ''' <exception cref="InvalidOperationException"></exception>
    ''' <exception cref="Exceptions.UserCreationException"></exception>
    Function InitializeUser(ByVal uninitUser As String, Optional ByVal unpassword As String = "", Optional ByVal ComputationNeeded As Boolean = True) As Boolean
        Try
            'Compute hash of a password
            Dim Regexp As New Regex("^([a-fA-F0-9]{64})$")
            If ComputationNeeded Then
                unpassword = GetEncryptedString(unpassword, Algorithms.SHA256)
                Wdbg("I", "Hash computed.")
            ElseIf Not Regexp.IsMatch(unpassword) Then
                Throw New InvalidOperationException("Trying to add unencrypted password to users list. That won't work properly, since login relies on encrypted passwords.")
            End If

            'Add user locally
            If Not File.Exists(paths("Users")) Then File.Create(paths("Users")).Close()
            Dim UsersLines As List(Of String) = File.ReadAllLines(paths("Users")).ToList
            If Not userword.ContainsKey(uninitUser) Then userword.Add(uninitUser, unpassword)

            'Add user globally
            UsersWriter = New StreamWriter(paths("Users"), True) With {.AutoFlush = True}
            If Not UsersLines.Count = 0 Then
                For i As Integer = 0 To UsersLines.Count - 1
                    If Not userword.ContainsKey(uninitUser) Then
                        UsersWriter.WriteLine(uninitUser + "," + unpassword)
                        Exit For
                    End If
                Next
            Else
                UsersWriter.WriteLine(uninitUser + "," + unpassword)
            End If
            UsersWriter.Close() : UsersWriter.Dispose()

            'Ready permissions
            Wdbg("I", "Username {0} added. Readying permissions...", uninitUser)
            InitPermissionsForNewUser(uninitUser)
            Return True
        Catch ex As Exception
            Throw New Exceptions.UserCreationException(DoTranslation("Error trying to add username.") + vbNewLine +
                                                                DoTranslation("Error {0}: {1}").FormatString(ex.Message))
            WStkTrc(ex)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Reads the user file and adds them to the list.
    ''' </summary>
    Public Sub InitializeUsers()
        'Opens file stream
        Dim UsersLines As List(Of String) = File.ReadAllLines(paths("Users")).ToList
        Dim SplitEntries() As String
        For Each Line As String In UsersLines
            SplitEntries = Line.Split(",")
            InitializeUser(SplitEntries(0), SplitEntries(1), False)
        Next
    End Sub

    Function GetUserEncryptedPassword(ByVal User As String)
        'Opens file stream
        Dim UsersLines As List(Of String) = File.ReadAllLines(paths("Users")).ToList
        Dim SplitEntries() As String
        For Each Line As String In UsersLines
            SplitEntries = Line.Split(",")
            If SplitEntries(0) = User Then
                Return SplitEntries(1)
            End If
        Next
        Return ""
    End Function

    ''' <summary>
    ''' Adds a new user
    ''' </summary>
    ''' <param name="newUser">A new user</param>
    ''' <param name="newPassword">A password</param>
    ''' <exception cref="Exceptions.UserCreationException"></exception>
    Public Function AddUser(ByVal newUser As String, Optional ByVal newPassword As String = "") As Boolean
        'Adds user
        Wdbg("I", "Creating user {0}...", newUser)
        If InStr(newUser, " ") > 0 Then
            Wdbg("W", "There are spaces in username.")
            Throw New Exceptions.UserCreationException(DoTranslation("Spaces are not allowed."))
        ElseIf newUser.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
            Wdbg("W", "There are special characters in username.")
            Throw New Exceptions.UserCreationException(DoTranslation("Special characters are not allowed."))
        ElseIf newUser = Nothing Then
            Wdbg("W", "Username is blank.")
            Throw New Exceptions.UserCreationException(DoTranslation("Blank username."))
        ElseIf Not userword.ContainsKey(newUser) Then
            Try
                If newPassword = Nothing Then
                    Wdbg("W", "Initializing user with no password")
                    InitializeUser(newUser)
                Else
                    Wdbg("I", "Initializing user with password")
                    InitializeUser(newUser, newPassword)
                End If
                EventManager.RaiseUserAdded(newUser)
                Return True
            Catch ex As Exception
                Wdbg("E", "Failed to create user {0}: {1}", ex.Message)
                WStkTrc(ex)
                Throw New Exceptions.UserCreationException(DoTranslation("usrmgr: Failed to create username {0}: {1}").FormatString(newUser, ex.Message))
            End Try
        Else
            Wdbg("W", "User {0} already found.", newUser)
            Throw New Exceptions.UserCreationException(DoTranslation("usrmgr: Username {0} is already found").FormatString(newUser))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Removes a user from users database
    ''' </summary>
    ''' <param name="user">A user</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="Exceptions.UserManagementException"></exception>
    ''' <remarks>This sub is an accomplice of in-shell command arguments.</remarks>
    Public Function RemoveUser(ByVal user As String) As Boolean
        If InStr(user, " ") > 0 Then
            Wdbg("W", "There are spaces in username.")
            Throw New Exceptions.UserManagementException(DoTranslation("Spaces are not allowed."))
        ElseIf user.IndexOfAny("[~`!@#$%^&*()-+=|{}':;.,<>/?]".ToCharArray) <> -1 Then
            Wdbg("W", "There are special characters in username.")
            Throw New Exceptions.UserManagementException(DoTranslation("Special characters are not allowed."))
        ElseIf user = Nothing Then
            Wdbg("W", "Username is blank.")
            Throw New Exceptions.UserManagementException(DoTranslation("Blank username."))
        ElseIf userword.ContainsKey(user) = False Then
            Wdbg("W", "Username {0} not found in list", user)
            Throw New Exceptions.UserManagementException(DoTranslation("User {0} not found.").FormatString(user))
        Else
            'Try to remove user
            If userword.Keys.ToArray.Contains(user) And user = "root" Then
                Wdbg("W", "User is root, and is a system account")
                Throw New Exceptions.UserManagementException(DoTranslation("User {0} isn't allowed to be removed.").FormatString(user))
            ElseIf userword.Keys.ToArray.Contains(user) And user = signedinusrnm Then
                Wdbg("W", "User has logged in, so can't delete self.")
                Throw New Exceptions.UserManagementException(DoTranslation("User {0} is already logged in. Log-out and log-in as another admin.").FormatString(user))
            ElseIf userword.Keys.ToArray.Contains(user) And user <> "root" Then
                Try
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

                    'Raise event
                    EventManager.RaiseUserRemoved(user)
                    Return True
                Catch ex As Exception
                    Throw New Exceptions.UserManagementException(DoTranslation("Error trying to remove username.") + vbNewLine +
                                                                 DoTranslation("Error {0}: {1}").FormatString(ex.Message))
                    WStkTrc(ex)
                End Try
            End If
        End If
        Return False
    End Function

    Public Function ChangeUsername(ByVal OldName As String, ByVal Username As String) As Boolean
        If userword.ContainsKey(OldName) Then
            If Not userword.ContainsKey(Username) Then
                Try
                    'Store user password
                    Dim Temporary As String = userword(OldName)

                    'Rename username in dictionary
                    userword.Remove(OldName)
                    userword.Add(Username, Temporary)
                    PermissionEditForNewUser(OldName, Username)

                    'Rename username in users.csv
                    Dim UsersLines As List(Of String) = File.ReadAllLines(paths("Users")).ToList
                    For i As Integer = 0 To UsersLines.Count - 1
                        If UsersLines(i).StartsWith($"{OldName},") Then
                            UsersLines(i) = UsersLines(i).Replace(OldName, Username)
                            Exit For
                        End If
                    Next
                    File.WriteAllLines(paths("Users"), UsersLines)

                    'Raise event
                    EventManager.RaiseUsernameChanged(OldName, Username)
                    Return True
                Catch ex As Exception
                    WStkTrc(ex)
                    Throw New Exceptions.UserManagementException(DoTranslation("Failed to rename user. {0}").FormatString(ex.Message))
                End Try
            Else
                Throw New Exceptions.UserManagementException(DoTranslation("The new name you entered is already found."))
            End If
        Else
            Throw New Exceptions.UserManagementException(DoTranslation("User {0} not found.").FormatString(OldName))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Initializes root account
    ''' </summary>
    Sub InitializeSystemAccount()
        If setRootPasswd Then
            AddUser("root", RootPasswd)
        ElseIf File.Exists(paths("Users")) Then
            InitializeUser("root", GetUserEncryptedPassword("root"), False)
        Else
            AddUser("root")
        End If
        Permission(PermissionType.Administrator, "root", PermissionManagementMode.Allow)
    End Sub

    ''' <summary>
    ''' Changes user password
    ''' </summary>
    ''' <param name="Target">Target username</param>
    ''' <param name="CurrentPass">Current user password</param>
    ''' <param name="NewPass">New user password</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="Exceptions.UserManagementException"></exception>
    Public Function ChangePassword(ByVal Target As String, ByVal CurrentPass As String, ByVal NewPass As String)
        CurrentPass = GetEncryptedString(CurrentPass, Algorithms.SHA256)
        If CurrentPass = userword(Target) Then
            If adminList(signedinusrnm) And userword.ContainsKey(Target) Then
                'Change password locally
                NewPass = GetEncryptedString(NewPass, Algorithms.SHA256)
                userword.Item(Target) = NewPass

                'Change password globally
                Dim UsersLines As List(Of String) = File.ReadAllLines(paths("Users")).ToList
                For i As Integer = 0 To UsersLines.Count - 1
                    If UsersLines(i).StartsWith($"{Target},") Then
                        UsersLines(i) = UsersLines(i).Replace(CurrentPass, NewPass)
                        Exit For
                    End If
                Next
                File.WriteAllLines(paths("Users"), UsersLines)

                'Raise event
                EventManager.RaiseUserPasswordChanged(Target)
                Return True
            ElseIf adminList(signedinusrnm) And Not userword.ContainsKey(Target) Then
                Throw New Exceptions.UserManagementException(DoTranslation("User not found"))
            ElseIf adminList(Target) And Not adminList(signedinusrnm) Then
                Throw New Exceptions.UserManagementException(DoTranslation("You are not authorized to change password of {0} because the target was an admin.").FormatString(Target))
            End If
        Else
            Throw New Exceptions.UserManagementException(DoTranslation("Wrong user password."))
        End If
        Return False
    End Function

End Module

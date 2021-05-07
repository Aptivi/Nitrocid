
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
Imports Newtonsoft.Json.Linq

Public Module UserManagement

    'Variables
    Public adminList As New Dictionary(Of String, Boolean)         'Users that are allowed to have administrative access.
    Public disabledList As New Dictionary(Of String, Boolean)      'Users that are unable to login
    Public AnonymousList As New Dictionary(Of String, Boolean)     'Users that shouldn't be listed in user list
    Public UsersToken As JArray

    Public Enum UserProperty
        Username
        Password
        Permissions
    End Enum

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
                Throw New InvalidOperationException("Trying to add unencrypted password to users list.")
            End If

            'Add user locally
            If Not userword.ContainsKey(uninitUser) Then userword.Add(uninitUser, unpassword)

            'Add user globally
            If Not UsersToken.Count = 0 Then
                Dim UserExists As Boolean
                For Each UserToken As JObject In UsersToken
                    If UserToken("username").ToString = uninitUser Then
                        UserExists = True
                    End If
                Next
                If Not UserExists Then
                    Dim NewUser As New JObject(New JProperty("username", uninitUser),
                                               New JProperty("password", unpassword),
                                               New JProperty("permissions", New JArray))
                    UsersToken.Add(NewUser)
                End If
            Else
                Dim NewUser As New JObject(New JProperty("username", uninitUser),
                                           New JProperty("password", unpassword),
                                           New JProperty("permissions", New JArray))
                UsersToken.Add(NewUser)
            End If
            File.WriteAllText(paths("Users"), JsonConvert.SerializeObject(UsersToken, Formatting.Indented))

            'Ready permissions
            Wdbg("I", "Username {0} added. Readying permissions...", uninitUser)
            InitPermissionsForNewUser(uninitUser)
            Return True
        Catch ex As Exception
            Throw New Exceptions.UserCreationException(DoTranslation("Error trying to add username.") + vbNewLine +
                                                       DoTranslation("Error {0}: {1}").FormatString(Err.Number, ex.Message), ex)
            WStkTrc(ex)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Reads the user file and adds them to the list.
    ''' </summary>
    Public Sub InitializeUsers()
        'Opens file stream
        Dim UsersTokenContent As String = File.ReadAllText(paths("Users"))
        Dim UninitUsersToken As JArray = JArray.Parse(If(Not String.IsNullOrEmpty(UsersTokenContent), UsersTokenContent, "{}"))
        For Each UserToken As JObject In UninitUsersToken
            InitializeUser(UserToken("username"), UserToken("password"), False)
        Next
    End Sub

    ''' <summary>
    ''' Loads user token
    ''' </summary>
    Sub LoadUserToken()
        If Not File.Exists(paths("Users")) Then File.Create(paths("Users")).Close()
        Dim UsersTokenContent As String = File.ReadAllText(paths("Users"))
        UsersToken = JArray.Parse(If(Not String.IsNullOrEmpty(UsersTokenContent), UsersTokenContent, "[]"))
    End Sub

    ''' <summary>
    ''' Gets user property
    ''' </summary>
    ''' <param name="User">Target user</param>
    ''' <param name="PropertyType">Property type</param>
    ''' <returns>JSON token of user property</returns>
    Public Function GetUserProperty(ByVal User As String, ByVal PropertyType As UserProperty) As JToken
        For Each UserToken As JObject In UsersToken
            If UserToken("username").ToString = User Then
                Return UserToken.SelectToken(PropertyType.ToString.ToLower)
            End If
        Next
    End Function

    ''' <summary>
    ''' Sets user property
    ''' </summary>
    ''' <param name="User">Target user</param>
    ''' <param name="PropertyType">Property type</param>
    ''' <param name="Value">Value</param>
    Public Sub SetUserProperty(ByVal User As String, ByVal PropertyType As UserProperty, ByVal Value As String)
        For Each UserToken As JObject In UsersToken
            If UserToken("username").ToString = User Then
                Select Case PropertyType
                    Case UserProperty.Username
                        UserToken("username") = Value
                    Case UserProperty.Password
                        UserToken("password") = Value
                    Case UserProperty.Permissions
                        Throw New NotSupportedException("Use AddPermission and RemovePermission for this.")
                    Case Else
                        Throw New ArgumentException("Property type is invalid")
                End Select
            End If
        Next
        File.WriteAllText(paths("Users"), JsonConvert.SerializeObject(UsersToken, Formatting.Indented))
    End Sub

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
                Throw New Exceptions.UserCreationException(DoTranslation("usrmgr: Failed to create username {0}: {1}").FormatString(newUser, ex.Message), ex)
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
                    AnonymousList.Remove(user)

                    'Remove user
                    Wdbg("I", "Removing username {0}...", user)
                    userword.Remove(user)

                    'Remove user from Users.json
                    For Each UserToken As JObject In UsersToken
                        If UserToken("username").ToString = user Then
                            UserToken.Remove()
                            Exit For
                        End If
                    Next
                    File.WriteAllText(paths("Users"), JsonConvert.SerializeObject(UsersToken, Formatting.Indented))

                    'Raise event
                    EventManager.RaiseUserRemoved(user)
                    Return True
                Catch ex As Exception
                    WStkTrc(ex)
                    Throw New Exceptions.UserManagementException(DoTranslation("Error trying to remove username.") + vbNewLine +
                                                                 DoTranslation("Error {0}: {1}").FormatString(ex.Message), ex)
                End Try
            End If
        End If
        Return False
    End Function

    ''' <summary>
    ''' Changes the username
    ''' </summary>
    ''' <param name="OldName">Old username</param>
    ''' <param name="Username">New username</param>
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

                    'Rename username in Users.json
                    SetUserProperty(OldName, UserProperty.Username, Username)

                    'Raise event
                    EventManager.RaiseUsernameChanged(OldName, Username)
                    Return True
                Catch ex As Exception
                    WStkTrc(ex)
                    Throw New Exceptions.UserManagementException(DoTranslation("Failed to rename user. {0}").FormatString(ex.Message), ex)
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
            If GetUserProperty("root", UserProperty.Password) IsNot Nothing Then
                InitializeUser("root", GetUserProperty("root", UserProperty.Password), False)
            Else
                AddUser("root")
            End If
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
                SetUserProperty(Target, UserProperty.Password, NewPass)

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

    ''' <summary>
    ''' Lists all users and includes anonymous and disabled users if enabled.
    ''' </summary>
    ''' <param name="IncludeAnonymous">Include anonymous users</param>
    ''' <param name="IncludeDisabled">Include disabled users</param>
    Public Function ListAllUsers(Optional ByVal IncludeAnonymous As Boolean = False, Optional ByVal IncludeDisabled As Boolean = False) As List(Of String)
        Dim UsersList As New List(Of String)(userword.Keys)
        If Not IncludeAnonymous Then
            UsersList.RemoveAll(New Predicate(Of String)(Function(x) AnonymousList.Keys.Contains(x) And AnonymousList(x) = True))
        End If
        If Not IncludeDisabled Then
            UsersList.RemoveAll(New Predicate(Of String)(Function(x) disabledList.Keys.Contains(x) And disabledList(x) = True))
        End If
        Return UsersList
    End Function

    ''' <summary>
    ''' Handles the prompts for setting up a first user
    ''' </summary>
    Sub FirstUserTrigger()
        Dim [Step] As Integer = 1
        Dim AnswerUsername As String = ""
        Dim AnswerPassword As String = ""
        Dim AnswerType As Integer

        'First, select user name
        W(DoTranslation("It looks like you've got no user except root. This is bad. We'll guide you how to create one."), True, ColTypes.Neutral)
        While [Step] = 1
            W(DoTranslation("Write your username.") + vbNewLine, True, ColTypes.Neutral)
            W(">> ", False, ColTypes.Input)
            AnswerUsername = Console.ReadLine
            Wdbg("I", "Answer: {0}", AnswerUsername)
            If String.IsNullOrWhiteSpace(AnswerUsername) Then
                Wdbg("W", "Username is not valid. Returning...")
                W(DoTranslation("You must write your username."), True, ColTypes.Err)
                W(DoTranslation("Press any key to go back."), True, ColTypes.Err)
                Console.ReadKey()
            Else
                [Step] += 1
            End If
        End While

        'Second, write password
        While [Step] = 2
            W(DoTranslation("Write your password.") + vbNewLine, True, ColTypes.Neutral)
            W(">> ", False, ColTypes.Input)
            AnswerPassword = ReadLineNoInput("*")
            Console.WriteLine()
            Wdbg("I", "Answer: {0}", AnswerPassword)
            If String.IsNullOrWhiteSpace(AnswerPassword) Then
                Wdbg("W", "Password is not valid. Returning...")
                W(DoTranslation("You must write your password."), True, ColTypes.Err)
                W(DoTranslation("Press any key to go back."), True, ColTypes.Err)
                Console.ReadKey()
            Else
                [Step] += 1
            End If
        End While

        'Third, select account type
        While [Step] = 3
            W(DoTranslation("Select account type.") + vbNewLine, True, ColTypes.Neutral)
            W("1) " + DoTranslation("Administrator: This account type has the most power in the kernel, allowing you to use system management programs."), True, ColTypes.Option)
            W("2) " + DoTranslation("Normal User: This account type is slightly more restricted than administrators."), True, ColTypes.Option)
            W(">> ", False, ColTypes.Input)
            AnswerType = Val(Console.ReadKey(True).KeyChar)
            Console.WriteLine()
            Wdbg("I", "Answer: {0}", AnswerType)
            Select Case AnswerType
                Case 1, 2
                    [Step] += 1
                Case Else '???
                    Wdbg("W", "Option is not valid. Returning...")
                    W(DoTranslation("Specified option {0} is invalid."), True, ColTypes.Err, AnswerType)
                    W(DoTranslation("Press any key to go back."), True, ColTypes.Err)
                    Console.ReadKey()
            End Select
        End While

        'Finally, create an account
        AddUser(AnswerUsername, AnswerPassword)
        If AnswerType = 1 Then Permission(PermissionType.Administrator, AnswerUsername, PermissionManagementMode.Allow)
        W(DoTranslation("Congratulations! You've made a new account! To finish this off, log in as your new account."), True, ColTypes.Neutral)
    End Sub

End Module

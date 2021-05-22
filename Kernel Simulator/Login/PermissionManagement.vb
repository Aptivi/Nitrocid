
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
Imports Newtonsoft.Json.Linq

Public Module PermissionManagement

    ''' <summary>
    ''' This enumeration lists all permission types.
    ''' </summary>
    Public Enum PermissionType As Integer
        Administrator = 1
        Disabled
        Anonymous
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
    Sub Permission(ByVal PermType As PermissionType, ByVal Username As String, ByVal PermissionMode As PermissionManagementMode)

        'Adds user into permission lists.
        Try
            Wdbg("I", "Mode: {0}", PermissionMode)
            If PermissionMode = PermissionManagementMode.Allow Then
                AddPermission(PermType, Username)
                W(DoTranslation("The user {0} has been added to the ""{1}"" list."), True, ColTypes.Neutral, Username, PermType.ToString)
            ElseIf PermissionMode = PermissionManagementMode.Disallow Then
                RemovePermission(PermType, Username)
                W(DoTranslation("The user {0} has been removed from the ""{1}"" list."), True, ColTypes.Neutral, Username, PermType.ToString)
            Else
                Wdbg("W", "Mode is invalid")
                W(DoTranslation("Invalid mode {0}"), True, ColTypes.Err, PermissionMode)
            End If
        Catch ex As Exception
            If DebugMode = True Then
                W(DoTranslation("You have either found a bug, or the permission you tried to add or remove is already done, or other error.") + vbNewLine +
                  DoTranslation("Error {0}: {1}") + vbNewLine + "{2}", True, ColTypes.Err, Err.Number, ex.Message, ex.StackTrace)
                WStkTrc(ex)
            Else
                W(DoTranslation("You have either found a bug, or the permission you tried to add or remove is already done, or other error.") + vbNewLine +
                  DoTranslation("Error {0}: {1}"), True, ColTypes.Err, Err.Number, ex.Message)
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Adds user to one of permission types
    ''' </summary>
    ''' <param name="PermType">Whether it be Admin or Disabled</param>
    ''' <param name="Username">A username to be managed</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="Exceptions.PermissionManagementException"></exception>
    Public Function AddPermission(ByVal PermType As PermissionType, ByVal Username As String) As Boolean
        'Sets the required permissions to false.
        If userword.Keys.ToArray.Contains(Username) Then
            Wdbg("I", "Type is {0}", PermType)
            If PermType = PermissionType.Administrator Then
                adminList(Username) = True
                Wdbg("I", "User {0} allowed (Admin): {1}", Username, adminList(Username))
            ElseIf PermType = PermissionType.Disabled Then
                disabledList(Username) = True
                Wdbg("I", "User {0} allowed (Disabled): {1}", Username, disabledList(Username))
            ElseIf PermType = PermissionType.Anonymous Then
                AnonymousList(Username) = True
                Wdbg("I", "User {0} allowed (Anonymous): {1}", Username, AnonymousList(Username))
            Else
                Wdbg("W", "Type is invalid")
                Throw New Exceptions.PermissionManagementException(DoTranslation("Failed to add user into permission lists: invalid type {0}"), PermType)
                Return False
            End If
        Else
            Wdbg("W", "User {0} not found on list", Username)
            Throw New Exceptions.PermissionManagementException(DoTranslation("Failed to add user into permission lists: invalid user {0}"), Username)
            Return False
        End If

        'Save changes
        For Each UserToken As JObject In UsersToken
            If UserToken("username").ToString = Username Then
                If Not CType(UserToken("permissions"), JArray).ToObject(GetType(List(Of String))).Contains(PermType.ToString) Then
                    CType(UserToken("permissions"), JArray).Add(PermType.ToString)
                End If
            End If
        Next
        File.WriteAllText(paths("Users"), JsonConvert.SerializeObject(UsersToken, Formatting.Indented))
        Return True
    End Function

    ''' <summary>
    ''' Removes user from one of permission types
    ''' </summary>
    ''' <param name="PermType">Whether it be Admin or Disabled</param>
    ''' <param name="Username">A username to be managed</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="Exceptions.PermissionManagementException"></exception>
    Public Function RemovePermission(ByVal PermType As PermissionType, ByVal Username As String) As Boolean
        'Sets the required permissions to false.
        If userword.Keys.ToArray.Contains(Username) And Username <> signedinusrnm Then
            Wdbg("I", "Type is {0}", PermType)
            If PermType = PermissionType.Administrator Then
                adminList(Username) = False
                Wdbg("I", "User {0} allowed (Admin): {1}", Username, adminList(Username))
            ElseIf PermType = PermissionType.Disabled Then
                disabledList(Username) = False
                Wdbg("I", "User {0} allowed (Disabled): {1}", Username, disabledList(Username))
            ElseIf PermType = PermissionType.Anonymous Then
                AnonymousList(Username) = False
                Wdbg("I", "User {0} allowed (Anonymous): {1}", Username, AnonymousList(Username))
            Else
                Wdbg("W", "Type is invalid")
                Throw New Exceptions.PermissionManagementException(DoTranslation("Failed to remove user from permission lists: invalid type {0}"), PermType)
                Return False
            End If
        ElseIf Username = signedinusrnm Then
            Throw New Exceptions.PermissionManagementException(DoTranslation("You are already logged in."))
            Return False
        Else
            Wdbg("W", "User {0} not found on list", Username)
            Throw New Exceptions.PermissionManagementException(DoTranslation("Failed to remove user from permission lists: invalid user {0}"), Username)
            Return False
        End If

        'Save changes
        For Each UserToken As JObject In UsersToken
            If UserToken("username").ToString = Username Then
                Dim PermissionArray As List(Of String) = UserToken("permissions").ToObject(GetType(List(Of String)))
                PermissionArray.Remove(PermType.ToString)
                UserToken("permissions") = JArray.FromObject(PermissionArray)
            End If
        Next
        File.WriteAllText(paths("Users"), JsonConvert.SerializeObject(UsersToken, Formatting.Indented))
        Return True
    End Function

    ''' <summary>
    ''' Edits the permission database for new user name
    ''' </summary>
    ''' <param name="OldName">Old username</param>
    ''' <param name="Username">New username</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="Exceptions.PermissionManagementException"></exception>
    Public Function PermissionEditForNewUser(ByVal OldName As String, ByVal Username As String) As Boolean
        'Edit username
        If adminList.ContainsKey(OldName) And disabledList.ContainsKey(OldName) And AnonymousList.ContainsKey(OldName) Then
            Try
                'Store permissions
                Dim AdminAllowed As Boolean = adminList(OldName)
                Dim DisabledAllowed As Boolean = disabledList(OldName)
                Dim AnonymousAllowed As Boolean = AnonymousList(OldName)

                'Remove old user entry
                Wdbg("I", "Removing {0} from Admin List", OldName)
                adminList.Remove(OldName)
                Wdbg("I", "Removing {0} from Disabled List", OldName)
                disabledList.Remove(OldName)
                Wdbg("I", "Removing {0} from Anonymous List", OldName)
                AnonymousList.Remove(OldName)

                'Add new user entry
                adminList.Add(Username, AdminAllowed)
                Wdbg("I", "Added {0} to Admin List with value of {1}", Username, AdminAllowed)
                disabledList.Add(Username, DisabledAllowed)
                Wdbg("I", "Added {0} to Disabled List with value of {1}", Username, DisabledAllowed)
                AnonymousList.Add(Username, AnonymousAllowed)
                Wdbg("I", "Added {0} to Anonymous List with value of {1}", Username, AnonymousAllowed)
                Return True
            Catch ex As Exception
                WStkTrc(ex)
                Throw New Exceptions.PermissionManagementException(DoTranslation("You have either found a bug, or the permission you tried to edit for a new user has failed.") + vbNewLine +
                                                                   DoTranslation("Error {0}: {1}"), ex, Err.Number, ex.Message)
            End Try
        Else
            Throw New Exceptions.PermissionManagementException(DoTranslation("One of the permission lists doesn't contain username {0}."), OldName)
        End If
        Return False
    End Function

    ''' <summary>
    ''' Initializes permissions for a new user with default settings
    ''' </summary>
    ''' <param name="NewUser">A new user name</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="Exceptions.PermissionManagementException"></exception>
    Public Function InitPermissionsForNewUser(ByVal NewUser As String) As Boolean
        Try
            'Initialize permissions locally
            If Not adminList.ContainsKey(NewUser) Then adminList.Add(NewUser, False)
            If Not disabledList.ContainsKey(NewUser) Then disabledList.Add(NewUser, False)
            If Not AnonymousList.ContainsKey(NewUser) Then AnonymousList.Add(NewUser, False)
            Return True
        Catch ex As Exception
            WStkTrc(ex)
            Throw New Exceptions.PermissionManagementException(DoTranslation("Failed to initialize permissions for user {0}: {1}"), ex, NewUser, ex.Message)
        End Try
        Return False
    End Function

    ''' <summary>
    ''' Loads permissions for all users
    ''' </summary>
    ''' <returns>True if successful; False if unsuccessful</returns>
    ''' <exception cref="Exceptions.PermissionManagementException"></exception>
    Public Function LoadPermissions() As Boolean
        Try
            For Each UserToken As JObject In UsersToken
                Dim User As String = UserToken("username")
                adminList(User) = False
                disabledList(User) = False
                AnonymousList(User) = False
                For Each Perm As String In CType(UserToken("permissions"), JArray)
                    Select Case Perm
                        Case "Administrator"
                            adminList(User) = True
                        Case "Disabled"
                            disabledList(User) = True
                        Case "Anonymous"
                            AnonymousList(User) = True
                    End Select
                Next
            Next
            Return True
        Catch ex As Exception
            WStkTrc(ex)
            Throw New Exceptions.PermissionManagementException(DoTranslation("Failed to load permissions from file: {0}"), ex, ex.Message)
        End Try
        Return False
    End Function

End Module
